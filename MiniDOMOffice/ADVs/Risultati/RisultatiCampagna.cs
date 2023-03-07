using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.ADV;
using minidom.repositories;

namespace minidom
{

    namespace repositories
    {
        /// <summary>
        /// Repository di oggetti di tipo <see cref="CRisultatoCampagna"/>
        /// </summary>
        [Serializable]
        public sealed class CRisultatiCampagnaClass
            : CModulesClass<CRisultatoCampagna>
        {
            /// <summary>
            /// Costruttore
            /// </summary>
            public CRisultatiCampagnaClass() 
                : base("modADVResults", typeof(CRisultatoCampagnaCursor), 0)
            {
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

                    // Tabella tbl_ADVResults 
                    using(var cursor = new CRisultatoCampagnaCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IDDestinatario.Value = mi.IDPersona2;
                        while (cursor.Read())
                        {
                            rec = new CMergePersonaRecord();
                            rec.NomeTabella = "tbl_ADVResults";
                            rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                            rec.FieldName = "IDDestinatario";
                            mi.TabelleModificate.Add(rec);

                            cursor.Item.Destinatario = mi.Persona1;
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

                    // Tabella tbl_ADVResults 
                    var items = mi.GetAffectedRecorsIDs("tbl_ADVResults", "IDDestinatario");
                    //if (!string.IsNullOrEmpty(items))
                    //    Databases.APPConn.ExecuteCommand("UPDATE [tbl_ADVResults] SET [IDDestinatario]=" + DBUtils.GetID(persona1) + ", [NomeDestinatario]=" + DBUtils.DBString(persona1.Nominativo) + "  WHERE [ID] In (" + items + ")");
                    using (var cursor = new CRisultatoCampagnaCursor())
                    {
                        cursor.ID.ValueIn(items);
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            cursor.Item.Destinatario = mi.Persona2;
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

    public partial class ADV
    {

       

        private static CRisultatiCampagnaClass m_RisultatiCampagna = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CRisultatoCampagna"/>
        /// </summary>
        public static CRisultatiCampagnaClass RisultatiCampagna
        {
            get
            {
                if (m_RisultatiCampagna is null)
                    m_RisultatiCampagna = new CRisultatiCampagnaClass();
                return m_RisultatiCampagna;
            }
        }
    }
}