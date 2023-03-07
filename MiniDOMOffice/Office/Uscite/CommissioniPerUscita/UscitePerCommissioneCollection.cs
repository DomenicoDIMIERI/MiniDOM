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
    public partial class Office
    {

        /// <summary>
        /// Collezione di uscite effettuate per una commissione
        /// </summary>
        [Serializable]
        public class UscitePerCommissioneCollection 
            : CCollection<CommissionePerUscita>
        {
            [NonSerialized] private Commissione m_Commissione;

            /// <summary>
            /// Costruttore
            /// </summary>
            public UscitePerCommissioneCollection()
            {
                m_Commissione = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="commissione"></param>
            public UscitePerCommissioneCollection(Commissione commissione) : this()
            {
                Load(commissione);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CommissionePerUscita value)
            {
                if (m_Commissione is object)
                    value.SetCommissione(m_Commissione);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CommissionePerUscita oldValue, CommissionePerUscita newValue)
            {
                if (m_Commissione is object)
                    newValue.SetCommissione(m_Commissione);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="commissione"></param>
            protected internal void Load(Commissione commissione)
            {
                if (commissione is null)
                    throw new ArgumentNullException("commissione");
                
                this.Clear();

                this.m_Commissione = commissione;
                if (DBUtils.GetID(commissione, 0) == 0)
                    return;
                using (var cursor = new CommissioniPerUscitaCursor())
                {
                    cursor.IDCommissione.Value = DBUtils.GetID(commissione, 0);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }
            }

            /// <summary>
            /// Imposta la commissione
            /// </summary>
            /// <param name="commissione"></param>
            protected internal virtual void SetCommissione(Commissione commissione)
            {
                m_Commissione = commissione;
                foreach (var item in this)
                    item.SetCommissione(commissione);
            }
        }
    }
}