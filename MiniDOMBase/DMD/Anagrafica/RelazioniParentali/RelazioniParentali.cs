using System;
using System.Collections;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Anagrafica;

namespace minidom.repositories
{
    /// <summary>
    /// Rappresenta le relazioni di parentela/affinità tra le persone
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public sealed class CRelazioniParentaliClass
        : CModulesClass<CRelazioneParentale>
    {

        private readonly string[] SupportedNames = new[] { "Marito", "Moglie", "Genitore", "Figlio", "Figlia", "Padre", "Madre", "Nonno", "Nonna", "Zio", "Zia", "Nipote", "Cognato", "Cognata", "Genero", "Nuora", "Suocero", "Suocera", "Fratello", "Sorella", "Amico", "Amica", "Conoscente", "Ex Marito", "Ex Moglie", "Convivente", "Cugino", "Cugina", "Collega" };

        /// <summary>
        /// Costruttore
        /// </summary>
        public CRelazioniParentaliClass() 
            : base("modAnaRelazioni", typeof(CRelazioneParentaleCursor))
        {
        }

        /// <summary>
        /// Restituisce i nomi delle relazioni supportate
        /// </summary>
        /// <returns></returns>
        public string[] GetSupportedNames()
        {
            return SupportedNames;
        }

        /// <summary>
        /// Restituisce le relazioni inverse di una relazione (es. Figlio restituisce Genitore, Marito restituisce moglie)
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        public string[] GetInvertedRelations(string relation)
        {
            switch (DMD.Strings.LCase(DMD.Strings.Trim(relation)) ?? "")
            {
                case "marito":
                    {
                        return new[] { "Moglie" };
                    }

                case "moglie":
                    {
                        return new[] { "Marito" };
                    }

                case "figlio":
                case "figlia":
                    {
                        return new[] { "Padre", "Madre", "Genitore" };
                    }

                case "padre":
                case "madre":
                case "genitore":
                    {
                        return new[] { "Figlio", "Figlia" };
                    }

                case "nonno":
                case "nonna":
                    {
                        return new[] { "Nipote" };
                    }

                case "zio":
                case "zia":
                    {
                        return new[] { "Nipote" };
                    }

                case "nipote":
                    {
                        return new[] { "Nonno", "Nonna", "Zio", "Zia" };
                    }

                case "cognato":
                case "cognata":
                    {
                        return new[] { "Cognato", "Cognata" };
                    }

                case "genero":
                case "nuora":
                    {
                        return new[] { "Suocero", "Suocera" };
                    }

                case "suocero":
                case "suocera":
                    {
                        return new[] { "Genero", "Nuora" };
                    }

                case "fratello":
                case "sorella":
                    {
                        return new[] { "Fratello", "Sorella" };
                    }

                case "amico":
                case "amica":
                    {
                        return new[] { "Amico", "Amica" };
                    }

                case "conoscente":
                    {
                        return new[] { "Conoscente" };
                    }

                case "ex marito":
                    {
                        return new[] { "Ex Moglie" };
                    }

                case "ex moglie":
                    {
                        return new[] { "Ex Marito" };
                    }

                case "convivente":
                    {
                        return new[] { "Convivente" };
                    }

                case "cugino":
                case "cugina":
                    {
                        return new[] { "Cugino", "Cugina" };
                    }

                case "collega":
                    {
                        return new[] { "Collega" };
                    }

                default:
                    {
                        return new[] { "" };
                    }
            }
        }

        /// <summary>
        /// Inizializza il modulo
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            minidom.Anagrafica.PersonaCreated += HandlePeronaModified;
            minidom.Anagrafica.PersonaDeleted += HandlePeronaModified;
            minidom.Anagrafica.PersonaModified += HandlePeronaModified;
            minidom.Anagrafica.PersonaMerged += HandlePeronaMerged;
            minidom.Anagrafica.PersonaUnMerged += HandlePeronaUnMerged;
        }

        /// <summary>
        /// Termina il modulo
        /// </summary>
        public override void Terminate()
        {
            minidom.Anagrafica.PersonaCreated -= HandlePeronaModified;
            minidom.Anagrafica.PersonaDeleted -= HandlePeronaModified;
            minidom.Anagrafica.PersonaModified -= HandlePeronaModified;
            minidom.Anagrafica.PersonaMerged -= HandlePeronaMerged;
            minidom.Anagrafica.PersonaUnMerged -= HandlePeronaUnMerged;
            base.Terminate();
        }

        /// <summary>
        /// Gestisce l'evento PersonaMerged
        /// </summary>
        /// <param name="e"></param>
        private void HandlePeronaMerged(MergePersonaEventArgs e)
        {
            lock (this.cacheLock)
            {
                var mi = e.MI;
                var persona = mi.Persona1;
                var persona1 = mi.Persona2;
                CMergePersonaRecord rec;

                if (!(mi.Persona1 is CPersonaFisica)) return;

                // Tabella tbl_PersoneRelazioni (IDPersona1) 
                using(var cursor = new CRelazioneParentaleCursor())
                {
                    cursor.IgnoreRights = true;
                    cursor.IDPersona1.value = mi.IDPersona2;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    while (cursor.Read())
                    {
                        rec = new CMergePersonaRecord();
                        rec.NomeTabella = "tbl_PersoneRelazioni";
                        rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                        rec.FieldName = "IDPersona1";
                        mi.TabelleModificate.Add(rec);

                        cursor.Item.Persona1 = (CPersonaFisica) mi.Persona1;
                        cursor.Item.Save();
                    }
                }

                using (var cursor = new CRelazioneParentaleCursor())
                {
                    cursor.IgnoreRights = true;
                    cursor.IDPersona2.value = mi.IDPersona2;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    while (cursor.Read())
                    {
                        rec = new CMergePersonaRecord();
                        rec.NomeTabella = "tbl_PersoneRelazioni";
                        rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                        rec.FieldName = "IDPersona2";
                        mi.TabelleModificate.Add(rec);

                        cursor.Item.Persona2 = (CPersonaFisica)mi.Persona1;
                        cursor.Item.Save();
                    }
                }

            }
        }

        /// <summary>
        /// Gestisce l'evento PersonaUnmerged
        /// </summary>
        /// <param name="e"></param>
        private void HandlePeronaUnMerged(MergePersonaEventArgs e)
        {
            lock (this.cacheLock)
            {
                var mi = e.MI;
                var items = mi.GetAffectedRecorsIDs("tbl_PersoneRelazioni", "IDPersona1");
                //if (!string.IsNullOrEmpty(items))
                //    Databases.APPConn.ExecuteCommand("UPDATE [tbl_PersoneRelazioni] SET [IDPersona1]=" + DBUtils.GetID(persona1) + ", [NomePersona1]=" + Databases.DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                using (var cursor = new CRelazioneParentaleCursor())
                {
                    cursor.ID.ValueIn(items);
                    cursor.IgnoreRights = true;
                    //cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    while (cursor.Read())
                    {
                        cursor.Item.Persona1 = (CPersonaFisica)mi.Persona2;
                        cursor.Item.Save();
                    }
                }

                items = mi.GetAffectedRecorsIDs("tbl_PersoneRelazioni", "IDPersona2");
                using (var cursor = new CRelazioneParentaleCursor())
                {
                    cursor.ID.ValueIn(items);
                    cursor.IgnoreRights = true;
                    //cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    while (cursor.Read())
                    {
                        cursor.Item.Persona2 = (CPersonaFisica)mi.Persona2;
                        cursor.Item.Save();
                    }
                }

            }
        }

        /// <summary>
        /// Gestisce l'evento PersonaChanged
        /// </summary>
        /// <param name="e"></param>
        private void HandlePeronaModified(PersonaEventArgs e)
        {
            lock (this.cacheLock)
            {
                var p = e.Persona;
 
            }
        }

        /// <summary>
        /// Restituisce le relazioni parentali per la persona
        /// </summary>
        /// <param name="personID"></param>
        /// <returns></returns>
        public CCollection<CRelazioneParentale> GetRelazioni(int personID)
        {
            var ret = new CCollection<CRelazioneParentale>();
            using (var cursor = new CRelazioneParentaleCursor())
            {
                cursor.IDPersona2.Value = personID;
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.NomeRelazione.SortOrder = SortEnum.SORT_ASC;
                cursor.IgnoreRights = true;
                while (cursor.Read())
                {
                    ret.Add(cursor.Item);
                }
            }

            using (var cursor = new CRelazioneParentaleCursor())
            {
                cursor.IDPersona2.Value = personID;
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.NomeRelazione.SortOrder = SortEnum.SORT_ASC;
                cursor.IgnoreRights = true;
                while (cursor.Read())
                {
                    ret.Add(cursor.Item);
                }
            }
            ret.Sort();
            return ret;
        }

        /// <summary>
        /// Restituisce le relazioni parentali per la persona
        /// </summary>
        /// <param name="persona"></param>
        /// <returns></returns>
        public CCollection<CRelazioneParentale> GetRelazioni(CPersonaFisica persona)
        {
            if (persona is null)
                throw new ArgumentNullException("persona");
            return GetRelazioni(DBUtils.GetID(persona, 0));
        }
    }


}

namespace minidom
{
    public partial class Anagrafica
    {


        private static CRelazioniParentaliClass m_RelazioniParentali = null;

        /// <summary>
        /// Repository delle relazioni parentali
        /// </summary>
        public static CRelazioniParentaliClass RelazioniParentali
        {
            get
            {
                if (m_RelazioniParentali is null)
                    m_RelazioniParentali = new CRelazioniParentaliClass();
                return m_RelazioniParentali;
            }
        }
    }
}