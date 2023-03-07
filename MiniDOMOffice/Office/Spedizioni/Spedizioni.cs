using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Office;


namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di <see cref="Spedizione"/>
        /// </summary>
        /// <remarks></remarks>
        public class CSpedizioniClass 
            : CModulesClass<Spedizione>
        {
            

            /// <summary>
            /// Costruttore
            /// </summary>
            public CSpedizioniClass() 
                : base("modOfficeSpedizioni", typeof(minidom.Office.SpedizioniCursor), 0)
            {
            }


            /// <summary>
            /// Inizializza le risorse
            /// </summary>
            public override void Initialize()
            {
                base.Initialize();
                minidom.Anagrafica.PersonaMerged += this.HandlePersonaMerged;
                minidom.Anagrafica.PersonaUnMerged += this.HandlePersonaUnMerged;
            }

            /// <summary>
            /// Rilascia le risorse
            /// </summary>
            public override void Terminate()
            {
                minidom.Anagrafica.PersonaMerged -= this.HandlePersonaMerged;
                minidom.Anagrafica.PersonaUnMerged -= this.HandlePersonaUnMerged;
                base.Terminate();
            }

            /// <summary>
            /// Gestisce il merge delle persone
            /// </summary>
            /// <param name="e"></param>
            /// <param name="sender"></param>
            private void HandlePersonaMerged(object sender, MergePersonaEventArgs e)
            {
                lock (Anagrafica.@lock)
                {
                    var mi = e.MI;
                    var persona1 = mi.Persona1;
                    var persona2 = mi.Persona2;
                    CMergePersonaRecord rec;


                    using (var cursor = new SpedizioniCursor())
                    {
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IDMittente.Value = mi.IDPersona2;
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            rec = new CMergePersonaRecord();
                            rec.NomeTabella = "tbl_OfficeSpedizioni";
                            rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                            rec.FieldName = "IDMittente";
                            mi.TabelleModificate.Add(rec);

                            cursor.Item.Mittente = mi.Persona1;
                            cursor.Item.Save();
                        }
                    }

                    using (var cursor = new SpedizioniCursor())
                    {
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IDDestinatario.Value = mi.IDPersona2;
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            rec = new CMergePersonaRecord();
                            rec.NomeTabella = "tbl_OfficeSpedizioni";
                            rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                            rec.FieldName = "IDDestinatario";
                            mi.TabelleModificate.Add(rec);

                            cursor.Item.Mittente = mi.Persona1;
                            cursor.Item.Save();
                        }
                    }


                }
            }

            /// <summary>
            /// Gestisce l'evento Unmerge di una persona fisica o di un'azienda
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            protected virtual void HandlePersonaUnMerged(object sender, MergePersonaEventArgs e)
            {
                lock (this.cacheLock)
                {
                    var mi = e.MI;
                    var items = mi.GetAffectedRecorsIDs("tbl_OfficeSpedizioni", "IDMittente");
                    using (var cursor = new SpedizioniCursor())
                    {
                        cursor.ID.ValueIn(items);
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            cursor.Item.Mittente = mi.Persona2;
                            cursor.Item.Save();
                        }
                    }

                    items = mi.GetAffectedRecorsIDs("tbl_OfficeSpedizioni", "IDDestinatario");
                    using (var cursor = new SpedizioniCursor())
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


            //private Sistema.CModule InitModule()
            //{
            //    var ret = Sistema.Modules.GetItemByName("modOfficeSpedizioni");
            //    if (ret is null)
            //    {
            //        ret = new Sistema.CModule("modOfficeSpedizioni");
            //        ret.Description = "Spedizioni";
            //        ret.DisplayName = "Spedizioni";
            //        ret.Parent = minidom.Office.Module;
            //        ret.Stato = ObjectStatus.OBJECT_VALID;
            //        ret.Save();
            //        ret.InitializeStandardActions();
            //    }

            //    return ret;
            //}


        }
    }

    public partial class Office
    {
        private static CSpedizioniClass m_Spedizioni = null;

        /// <summary>
        /// Repository di <see cref="Spedizione"/>
        /// </summary>
        /// <remarks></remarks>
        public static CSpedizioniClass Spedizioni
        {
            get
            {
                if (m_Spedizioni is null)
                    m_Spedizioni = new CSpedizioniClass();
                return m_Spedizioni;
            }
        }
    }
}