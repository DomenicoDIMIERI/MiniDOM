using System;
using DMD;
using DMD.Databases;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    namespace repositories
    {
        /// <summary>
        /// Repository di oggetti <see cref="CContatto"/>
        /// </summary>
        [Serializable]
        public sealed class CContattiClass
            : CModulesClass<CContatto>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CContattiClass()
                : base("modRecapiti", typeof(CContattoCursor), 0)
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
            /// Gestisce l'evento PersonaMoficied
            /// </summary>
            /// <param name="e"></param>
            private void HandlePeronaModified(PersonaEventArgs e)
            {
                lock (this.cacheLock)
                {


                }
            }

            /// <summary>
            /// Gestisce l'evento PersonaUnmerged
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

                    // Tabella tbl_Contatti 
                    using(var cursor = new CContattoCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.PersonaID.Value = mi.IDPersona2;
                        while (cursor.Read())
                        {
                            rec = new CMergePersonaRecord();
                            rec.NomeTabella = "tbl_Contatti";
                            rec.FieldName = "Persona";
                            rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                            mi.TabelleModificate.Add(rec);

                            cursor.Item.SetPersona(mi.Persona1);
                            cursor.Item.Save(true);
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
                    // Tabella tbl_Contatti 
                    var items = mi.GetAffectedRecorsIDs("tbl_Contatti", "Persona");
                    //if (!string.IsNullOrEmpty(items))
                    //    Databases.APPConn.ExecuteCommand("UPDATE [tbl_Contatti] SET [Persona]=" + DBUtils.GetID(persona1) + " WHERE [ID] In (" + items + ")");
                    using (var cursor = new CContattoCursor())
                    {
                        cursor.ID.ValueIn(items);
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            cursor.Item.SetPersona(mi.Persona2);
                            cursor.Item.Save();
                        }
                    }
                }
            }

        }

    }

    public partial class Anagrafica
    {


        private static CContattiClass m_Contatti = null;

        /// <summary>
        /// Repository di oggetti <see cref="CContatto"/>
        /// </summary>
        public static CContattiClass Contatti
        {
            get
            {
                if (m_Contatti is null) m_Contatti = new CContattiClass();
                return m_Contatti;
            }
        }


    }
}