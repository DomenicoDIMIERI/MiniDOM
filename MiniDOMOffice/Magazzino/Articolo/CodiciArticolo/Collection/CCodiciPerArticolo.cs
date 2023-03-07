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
        /// Collezione dei codici articolo
        /// </summary>
        [Serializable]
        public class CCodiciPerArticolo 
            : CKeyCollection<CodiceArticolo>
        {
            [NonSerialized] private Articolo m_Articolo;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCodiciPerArticolo()
            {
                m_Articolo = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="articolo"></param>
            public CCodiciPerArticolo(Articolo articolo) : this()
            {
                Load(articolo);
            }

            /// <summary>
            /// Imposta l'articolo a cui appartiene la collezione
            /// </summary>
            /// <param name="articolo"></param>
            protected internal virtual void SetArticolo(Articolo articolo)
            {
                m_Articolo = articolo;
                if (articolo is object)
                {
                    foreach (var c in this)
                    {
                        c.SetArticolo(this.m_Articolo);
                    }
                }
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CodiceArticolo oldValue, CodiceArticolo newValue)
            {
                if (m_Articolo is object)
                {
                    newValue.SetArticolo(m_Articolo);
                    newValue.SetOrdine(index);                     
                }

                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CodiceArticolo value)
            {
                if (m_Articolo is object)
                {
                    value.SetArticolo(m_Articolo);
                    value.SetOrdine(index);
                }

                base.OnInsert(index, value);
            }

            /// <summary>
            /// Carica da ldb
            /// </summary>
            /// <param name="articolo"></param>
            protected internal virtual void Load(Articolo articolo)
            {
                if (articolo is null)
                    throw new ArgumentNullException("articolo");
                
                Clear();

                m_Articolo = articolo;
                
                if (DBUtils.GetID(articolo, 0) == 0)
                    return;

                using (var cursor = new CodiceArticoloCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDArticolo.Value = DBUtils.GetID(articolo);
                    cursor.Ordine.SortOrder = SortEnum.SORT_ASC;
                    while (cursor.Read())
                    {
                        Add(cursor.Item.Nome, cursor.Item);
                    }
                }
            }
        }
    }
}