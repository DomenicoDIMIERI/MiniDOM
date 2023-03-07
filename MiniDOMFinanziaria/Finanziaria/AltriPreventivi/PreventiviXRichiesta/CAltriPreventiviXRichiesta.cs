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
using static minidom.Finanziaria;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Collezione dei preventivi della concorrenza registrati in una richiesta di finanziamento
        /// </summary>
        [Serializable]
        public class CAltriPreventiviXRichiesta 
            : CCollection<CAltroPreventivo>
        {
            
            [NonSerialized] private CRichiestaFinanziamento m_RichiestaDiFinanziamento;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAltriPreventiviXRichiesta()
            {
                m_RichiestaDiFinanziamento = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="richiesta"></param>
            public CAltriPreventiviXRichiesta(CRichiestaFinanziamento richiesta) : this()
            {
                Load(richiesta);
            }

            /// <summary>
            /// Riferimento alla richiesta di finanziamento
            /// </summary>
            public CRichiestaFinanziamento RichiestaDiFinanziamento
            {
                get
                {
                    return m_RichiestaDiFinanziamento;
                }
            }

            /// <summary>
            /// Imposta la richiesta di finanziamento
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetRichiesta(CRichiestaFinanziamento value)
            {
                m_RichiestaDiFinanziamento = value;
                if (value is object)
                {
                    foreach(var obj in this)
                        obj.SetRichiestaDiFinanziamento(value);
                }
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CAltroPreventivo value)
            {
                if (m_RichiestaDiFinanziamento is object)
                    value.SetRichiestaDiFinanziamento(m_RichiestaDiFinanziamento);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CAltroPreventivo oldValue, CAltroPreventivo newValue)
            {
                if (m_RichiestaDiFinanziamento is object)
                    newValue.SetRichiestaDiFinanziamento(m_RichiestaDiFinanziamento);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica la collezione
            /// </summary>
            /// <param name="richiesta"></param>
            protected void Load(CRichiestaFinanziamento richiesta)
            {
                if (richiesta is null)
                    throw new ArgumentNullException("richiesta");
                
                Clear();

                m_RichiestaDiFinanziamento = richiesta;

                if (DBUtils.GetID(richiesta, 0) == 0) return;

                using (var cursor = new CAltriPreventiviCursor())
                { 
                    cursor.IgnoreRights = true;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDRichiestaDiFinanziamento.Value = DBUtils.GetID(richiesta, 0);
                    cursor.Data.SortOrder = SortEnum.SORT_ASC;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }
            }
        }
    }
}