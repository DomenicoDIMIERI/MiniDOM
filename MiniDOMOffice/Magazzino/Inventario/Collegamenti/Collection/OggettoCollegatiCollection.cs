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
using static minidom.Store;


namespace minidom
{
    public partial class Store
    {

        /// <summary>
        /// Collezione di oggetti collegati ad un oggetto
        /// </summary>
        [Serializable]
        public class OggettiCollegatiCollection 
            : CCollection<OggettoCollegato>
        {
            [NonSerialized]
            private OggettoInventariato m_Owner;

            /// <summary>
            /// Costruttore
            /// </summary>
            public OggettiCollegatiCollection()
            {
                m_Owner = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            public OggettiCollegatiCollection(OggettoInventariato owner) : this()
            {
                Load(owner);
            }

            /// <summary>
            /// Imposta l'oggetto
            /// </summary>
            /// <param name="owner"></param>
            protected internal virtual void SetOwner(OggettoInventariato owner)
            {
                m_Owner = owner;
                //if (owner is object)
                //{
                //    foreach(var c in this)
                //    {
                //        c.SetOggetto1(owner);
                //    }
                //}
            }

            /// <summary>
            /// Carica le relazioni
            /// </summary>
            /// <param name="owner"></param>
            protected internal virtual void Load(OggettoInventariato owner)
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                Clear();
                m_Owner = owner;
                int oID = DBUtils.GetID(owner, 0);
                if (oID == 0)
                    return;
                using (var cursor = new OggettoCollegatoCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    //cursor.IDOggetto1.Value = DBUtils.GetID(owner, 0);
                    cursor.WhereClauses *= (cursor.Field("IDOggetto1").EQ(oID) + cursor.Field("IDOggetto2").EQ(oID));
                    while (cursor.Read())
                    {
                        var item = cursor.Item;
                        if (item.IDOggetto1 == oID)
                            item.SetOggetto1(owner);
                        if (item.IDOggetto2 == oID)
                            item.SetOggetto2(owner);
                        this.Add(item);                        
                    }
                }
            }
        }
    }
}