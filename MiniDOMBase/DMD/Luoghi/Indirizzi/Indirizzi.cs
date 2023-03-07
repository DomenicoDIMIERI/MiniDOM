using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository degli indirizzi
        /// </summary>
        [Serializable]
        public sealed class CIndirizziClass 
            : CModulesClass<CIndirizzo>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CIndirizziClass() 
                : base("modIndirizzi", typeof(CIndirizziCursor))
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

                    // Tabella tbl_Indirizzi 
                    using(var cursor = new CIndirizziCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.PersonaID.Value = mi.IDPersona2;
                        while (cursor.Read())
                        {
                            rec = new CMergePersonaRecord();
                            rec.NomeTabella = "tbl_Indirizzi";
                            rec.FieldName = "Persona";
                            rec.RecordID = DBUtils.GetID(cursor.Item, 0);
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
                    // Tabella tbl_Indirizzi 
                    var items = mi.GetAffectedRecorsIDs("tbl_Indirizzi", "Persona");
                    //if (!string.IsNullOrEmpty(items))
                    //    Databases.APPConn.ExecuteCommand("UPDATE [tbl_Indirizzi] SET [Persona]=" + DBUtils.GetID(persona1) + " WHERE [ID] In (" + items + ")");
                    using (var cursor = new CIndirizziCursor())
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

        }



    }
}