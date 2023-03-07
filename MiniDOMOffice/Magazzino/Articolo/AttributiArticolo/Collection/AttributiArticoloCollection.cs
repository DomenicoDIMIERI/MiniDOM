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
        /// Collezione degli attributi specificato per una istanza di un Articolo
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class AttributiArticoloCollection 
            : CKeyCollection<AttributoArticolo>
        {
            [NonSerialized]
            private Articolo m_Articolo;

            /// <summary>
            /// Costruttore
            /// </summary>
            public AttributiArticoloCollection()
            {
                m_Articolo = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="Articolo"></param>
            public AttributiArticoloCollection(Articolo Articolo) : this()
            {
                Load(Articolo);
            }

            /// <summary>
            /// Restituisce l'articolo a cui appartiene la collezione
            /// </summary>
            public Articolo Articolo
            {
                get
                {
                    return m_Articolo;
                }
            }

            /// <summary>
            /// Imposta l'articolo
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetArticolo(Articolo value)
            {
                m_Articolo = value;
                if (value is object)
                {
                    foreach (AttributoArticolo item in this)
                        item.SetArticolo(value);
                }
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, AttributoArticolo value)
            {
                if (m_Articolo is object)
                    value.SetArticolo(m_Articolo);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, AttributoArticolo oldValue, AttributoArticolo newValue)
            {
                if (m_Articolo is object)
                    newValue.SetArticolo(m_Articolo);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica
            /// </summary>
            /// <param name="articolo"></param>
            protected internal void Load(Articolo articolo)
            {
                if (articolo is null)
                    throw new ArgumentNullException("articolo");
               
                this.Clear();

                this.m_Articolo = articolo;

                if (DBUtils.GetID(articolo, 0) == 0)
                    return;

                using (var cursor = new AttributoArticoloCursor())
                {
                    cursor.IgnoreRights = true;
                    cursor.IDArticolo.Value = DBUtils.GetID(articolo, 0);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    while (cursor.Read())
                    {
                        Add(cursor.Item.NomeAttributo, cursor.Item);
                    }
                }

                this.Sort();
            }
        }
    }
}