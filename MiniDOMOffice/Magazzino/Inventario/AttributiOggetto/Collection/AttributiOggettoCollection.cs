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
        /// Collezione degli attributi specificato per una istanza di un Oggetto
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class AttributiOggettoCollection
            : CKeyCollection<AttributoOggetto>
        {
            [NonSerialized]
            private OggettoInventariato m_Oggetto;

            /// <summary>
            /// Costruttore
            /// </summary>
            public AttributiOggettoCollection()
            {
                m_Oggetto = null;
            }


            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="Oggetto"></param>
            public AttributiOggettoCollection(OggettoInventariato Oggetto) 
                : this()
            {
                Load(Oggetto);
            }

            /// <summary>
            /// Restitusice un riferimento all'oggetto inventariato
            /// </summary>
            public OggettoInventariato Oggetto
            {
                get
                {
                    return m_Oggetto;
                }
            }

            /// <summary>
            /// Imposta il riferimento all'oggetto inventariato
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetOggetto(OggettoInventariato value)
            {
                m_Oggetto = value;
                if (value is object)
                {
                    foreach (AttributoOggetto item in this)
                        item.SetOggetto(value);
                }
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, AttributoOggetto value)
            {
                if (m_Oggetto is object)
                    value.SetOggetto(m_Oggetto);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, AttributoOggetto oldValue, AttributoOggetto newValue)
            {
                if (m_Oggetto is object)
                    newValue.SetOggetto(m_Oggetto);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica la collezione
            /// </summary>
            /// <param name="Oggetto"></param>
            protected internal virtual void Load(OggettoInventariato Oggetto)
            {
                if (Oggetto is null)
                    throw new ArgumentNullException("Oggetto");
                
                Clear();
                
                m_Oggetto = Oggetto;

                if (DBUtils.GetID(Oggetto, 0) == 0)
                    return;

                using (var cursor = new AttributoOggettoCursor())
                {
                    cursor.IgnoreRights = true;
                    cursor.IDOggetto.Value = DBUtils.GetID(Oggetto, 0);
                    while (cursor.Read())
                    {
                        Add(cursor.Item.NomeAttributo, cursor.Item);

                    }
                }

                Sort();
            }
        }
    }
}