using System;
using System.Collections;
using minidom.repositories;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Anagrafica;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CMergePersona"/>
        /// </summary>
        [Serializable]
        public class CMergePersone
            : CModulesClass<Anagrafica.CMergePersona>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CMergePersone()
                : base("modMergePersone", typeof(Anagrafica.CMergePersonaCursor), 0)
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

                    // Tabella tbl_MergePersone 
                    using(var cursor = new CMergePersonaCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IDPersona1.Value = mi.IDPersona2;
                        while (cursor.Read())
                        {
                            rec = mi.Add("tbl_MergePersone", "IDPersona1", DBUtils.GetID(cursor.Item, 0));
                            cursor.Item.Persona1 = mi.Persona1;
                            cursor.Item.Save();
                        }
                    }

                    using (var cursor = new CMergePersonaCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IDPersona2.Value = mi.IDPersona2;
                        while (cursor.Read())
                        {
                            rec = mi.Add("tbl_MergePersone", "IDPersona2", DBUtils.GetID(cursor.Item, 0));
                            cursor.Item.Persona2 = mi.Persona1;
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

                    // Tabella tbl_MergePersone
                    var items = mi.GetAffectedRecorsIDs("tbl_MergePersone", "IDPersona1");
                    //if (!string.IsNullOrEmpty(items))
                    //    Databases.APPConn.ExecuteCommand("UPDATE [tbl_MergePersone] SET [IDPersona1]=" + DBUtils.GetID(persona1) + ", [NomePersona1]=" + Databases.DBUtils.DBString(persona1.Nominativo) + "  WHERE [ID] In (" + items + ")");
                    using (var cursor = new CMergePersonaCursor())
                    {
                        cursor.ID.ValueIn(items);
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            cursor.Item.Persona1 = mi.Persona2;
                            cursor.Item.Save();
                        }
                    }

                    items = mi.GetAffectedRecorsIDs("tbl_MergePersone", "IDPersona2");
                    //if (!string.IsNullOrEmpty(items))
                    //    Databases.APPConn.ExecuteCommand("UPDATE [tbl_MergePersone] SET [IDPersona1]=" + DBUtils.GetID(persona1) + ", [NomePersona1]=" + Databases.DBUtils.DBString(persona1.Nominativo) + "  WHERE [ID] In (" + items + ")");
                    using (var cursor = new CMergePersonaCursor())
                    {
                        cursor.ID.ValueIn(items);
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            cursor.Item.Persona2 = mi.Persona2;
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
            /// Restituisce l'ultima unione effettuata
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            public Anagrafica.CMergePersona GetLastMerge(Anagrafica.CPersona persona)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");

                using (var cursor = new Anagrafica.CMergePersonaCursor())
                {
                    cursor.IDPersona1.Value = DBUtils.GetID(persona, 0);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.DataOperazione.SortOrder = SortEnum.SORT_DESC;
                    return cursor.Item;
                }
            }
        }
    }

    public partial class Anagrafica
    {


     
        private static CMergePersone m_MergePersone = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CMergePersona"/>
        /// </summary>
        public static CMergePersone MergePersone
        {
            get
            {
                if (m_MergePersone is null)
                    m_MergePersone = new CMergePersone();
                return m_MergePersone;
            }
        }
    }
}