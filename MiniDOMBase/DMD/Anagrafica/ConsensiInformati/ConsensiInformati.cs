using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="ConsensoInformato"/>
        /// </summary>
        [Serializable]
        public sealed class CConsensiInformatiClass 
            : CModulesClass<ConsensoInformato>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public  CConsensiInformatiClass() 
                : base("modConsensiInformati", typeof(Anagrafica.ConsensoInformatoCursor), 0)
            {
            }

            /// <summary>
            /// Restituisce tutti i consensi informati espressi dalla persona
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            public ConsensoInformatoColleciton GetConsensiByPersona(CPersona persona)
            {
                return new ConsensoInformatoColleciton(persona);
            }

            /// <summary>
            /// Restituisce tutti i consensi informati espressi dalla persona
            /// </summary>
            /// <param name="idPersona"></param>
            /// <returns></returns>
            public ConsensoInformatoColleciton GetConsensiByPersona(int idPersona)
            {
                return GetConsensiByPersona(Persone.GetItemById(idPersona));
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
                    var persona1 = mi.Persona1;
                    var persona2 = mi.Persona2;
                    CMergePersonaRecord rec;

                    // Tabella tbl_PersoneConsensi 
                    using(var cursor = new ConsensoInformatoCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.IDPersona.Value = mi.IDPersona2;
                        while (cursor.Read())
                        {
                            rec = new CMergePersonaRecord();
                            rec.NomeTabella = "tbl_PersoneConsensi";
                            rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                            rec.FieldName = "IDPersona";
                            mi.TabelleModificate.Add(rec);

                            cursor.Item.Persona = mi.Persona1;
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

                    // Tabella tbl_PersoneConsensi 
                    var items = mi.GetAffectedRecorsIDs("tbl_PersoneConsensi", "IDPersona");
                    //if (!string.IsNullOrEmpty(items))
                    //    Databases.APPConn.ExecuteCommand("UPDATE [tbl_PersoneConsensi] SET [IDPersona]=" + DBUtils.GetID(persona1) + ", [NomePersona]=" + Databases.DBUtils.DBString(persona1.Nominativo) + " WHERE [ID] In (" + items + ")");
                    using (var cursor = new ConsensoInformatoCursor())
                    {
                        cursor.ID.ValueIn(items);
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            cursor.Item.Persona = mi.Persona2;
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

        }
    }

    public partial class Anagrafica
    {
        private static CConsensiInformatiClass m_ConsensiInformati = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="ConsensoInformato"/>
        /// </summary>
        public static CConsensiInformatiClass ConsensiInformati
        {
            get
            {
                if (m_ConsensiInformati is null)
                    m_ConsensiInformati = new CConsensiInformatiClass();
                return m_ConsensiInformati;
            }
        }
    }
}