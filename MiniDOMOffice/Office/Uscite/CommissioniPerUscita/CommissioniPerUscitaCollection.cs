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
        /// Collezione di commissioni svolte durante un'uscita
        /// </summary>
        [Serializable]
        public class CommissioniPerUscitaCollection 
            : CCollection<CommissionePerUscita>
        {
            [NonSerialized] private Uscita m_Uscita;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CommissioniPerUscitaCollection()
            {
                m_Uscita = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="uscita"></param>
            public CommissioniPerUscitaCollection(Uscita uscita) 
                : this()
            {
                Load(uscita);
            }

            /// <summary>
            /// Aggiunge una commissione svolta
            /// </summary>
            /// <param name="commissione"></param>
            /// <param name="operatore"></param>
            /// <param name="esito"></param>
            /// <returns></returns>
            public CommissionePerUscita Add(
                                Commissione commissione, 
                                CUser operatore, 
                                string esito
                                )
            {
                if (m_Uscita is null)
                    throw new ArgumentNullException("Uscita");
                var cxu = new CommissionePerUscita();
                cxu.Commissione = commissione;
                cxu.Uscita = m_Uscita;
                cxu.Operatore = operatore;
                cxu.DescrizioneEsito = esito;
                cxu.Stato = ObjectStatus.OBJECT_VALID;
                Add(cxu);
                cxu.Save();
                return cxu;
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CommissionePerUscita value)
            {
                if (m_Uscita is object)
                    value.SetUscita(m_Uscita);
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
                if (m_Uscita is object)
                    newValue.SetUscita(m_Uscita);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="uscita"></param>
            protected internal void Load(Uscita uscita)
            {
                if (uscita is null)
                    throw new ArgumentNullException("uscita");
                Clear();
                m_Uscita = uscita;
                if (DBUtils.GetID(uscita, 0) == 0)
                    return;

                using (var cursor = new CommissioniPerUscitaCursor())
                {
                    cursor.IDUscita.Value = DBUtils.GetID(uscita, 0);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }

                }
            }

            /// <summary>
            /// Imposta l'uscita
            /// </summary>
            /// <param name="uscita"></param>
            internal void SetUscita(Uscita uscita)
            {
                m_Uscita = uscita;
                foreach (var item in this)
                    item.SetUscita(uscita);
            }
        }
    }
}