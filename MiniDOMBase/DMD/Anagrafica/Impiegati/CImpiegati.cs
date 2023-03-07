using DMD.Databases;
using DMD.Databases.Collections;
using System;
using System.Collections;
using System.Diagnostics;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Collezione di impiegati di un'azienda
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CImpiegati 
            : CCollection<CImpiegato>
        {

            [NonSerialized] private CAzienda m_Azienda;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CImpiegati()
            {
                m_Azienda = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="azienda"></param>
            public CImpiegati(CAzienda azienda) : this()
            {
                LoadAzienda(azienda);
            }

            /// <summary>
            /// Imposta l'azienda
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetAzienda(CAzienda value)
            {
                m_Azienda = value;
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CImpiegato value)
            {
                if (m_Azienda is object)
                    value.SetAzienda(m_Azienda);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CImpiegato oldValue, CImpiegato newValue)
            {
                if (m_Azienda is object)
                    newValue.SetAzienda(m_Azienda);

                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Aggiunge l'impiegato
            /// </summary>
            /// <param name="persona"></param>
            /// <param name="ufficio"></param>
            /// <param name="posizione"></param>
            /// <returns></returns>
            public CImpiegato AddImpiegato(CPersonaFisica persona, string ufficio, string posizione)
            {
                var item = new CImpiegato();
                item.Azienda = m_Azienda;
                item.Persona = persona;
                item.Ufficio = ufficio;
                item.Posizione = posizione;
                item.Stato = ObjectStatus.OBJECT_VALID;
                Add(item);
                return item;
            }

            /// <summary>
            /// Carica gli impiegati
            /// </summary>
            /// <param name="azienda"></param>
            /// <returns></returns>
            protected internal bool LoadAzienda(CAzienda azienda)
            {
                if (azienda is null)
                    throw new ArgumentNullException("azienda");
                Clear();
                m_Azienda = azienda;
                if (DBUtils.GetID(azienda, 0) == 0)
                    return true;

                using (var cursor = new CImpiegatiCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.AziendaID.Value = DBUtils.GetID(azienda, 0);
                    cursor.ID.SortOrder = SortEnum.SORT_DESC;
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        this.Add(cursor.Item);                         
                    }
                }

                return true;
            }
        }
    }
}