using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Collezione di uffici di un'azienda
        /// </summary>
        [Serializable]
        public class CAziendaUffici 
            : CCollection<CUfficio>
        {

            [NonSerialized] private CAzienda m_Azienda;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAziendaUffici()
            {
                m_Azienda = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="azienda"></param>
            public CAziendaUffici(CAzienda azienda) : this()
            {
                Load(azienda);
            }

            /// <summary>
            /// Restituisce un riferimento all'azienda 
            /// </summary>
            public CAzienda Azienda
            {
                get
                {
                    return m_Azienda;
                }
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CUfficio value)
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
            protected override void OnSet(int index, CUfficio oldValue, CUfficio newValue)
            {
                if (m_Azienda is object)
                    newValue.SetAzienda(m_Azienda);

                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica gli uffici
            /// </summary>
            /// <param name="value"></param>
            protected internal void Load(CAzienda value)
            {
                if (value is null)
                    throw new ArgumentNullException("azienda");
                
                this.Clear();

                this.m_Azienda = value;
                
                if (DBUtils.GetID(value, 0) == 0)
                    return;

                using (var cursor = new CUfficiCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDAzienda.Value = DBUtils.GetID(value, 0);
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }
            }
        }
    }
}