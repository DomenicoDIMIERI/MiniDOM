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
        /// Repository di oggetti <see cref="IntestatarioContoCorrente"/>
        /// </summary>
        [Serializable]
        public sealed class CItestatariClass
            : CModulesClass<IntestatarioContoCorrente>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CItestatariClass()
                : base("modIntestatariContiCorrente", typeof(IntestatarioContoCorrenteCursor), 0)
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

                    // Tabella tbl_ContiCorrentiInt 
                    using(var cursor = new IntestatarioContoCorrenteCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.Stato.Value = ObjectStatus.OBJECT_TEMP;
                        cursor.IDPersona.Value = mi.IDPersona2;
                        while (cursor.Read ())
                        {
                            rec = mi.Add("tbl_ContiCorrentiInt", "IDPersona", DBUtils.GetID(cursor.Item, 0));
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


                    // Tabella tbl_ContiCorrentiInt 
                    var items = mi.GetAffectedRecorsIDs("tbl_ContiCorrentiInt", "IDPersona");
                    //if (!string.IsNullOrEmpty(items))
                    //    Databases.APPConn.ExecuteCommand("UPDATE [tbl_ContiCorrentiInt] SET [IDPersona]=" + DBUtils.GetID(persona1) + ", [NomePersona]=" + Databases.DBUtils.DBString(persona1.Nominativo) + "  WHERE [ID] In (" + items + ")");
                    using (var cursor = new IntestatarioContoCorrenteCursor())
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