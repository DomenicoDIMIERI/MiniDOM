using DMD.Databases;
using DMD.Databases.Collections;
using System;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Collezione di intestatari del conto corrente
        /// </summary>
        [Serializable]
        public class CIntestatariContoCorrente 
            : CCollection<IntestatarioContoCorrente>
        {

            [NonSerialized] private ContoCorrente m_ContoCorrente;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CIntestatariContoCorrente()
            {
                m_ContoCorrente = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="conto"></param>
            public CIntestatariContoCorrente(ContoCorrente conto) : this()
            {
                Load(conto);
            }

            /// <summary>
            /// Restituisce un riferimento al conto corrente
            /// </summary>
            public ContoCorrente ContoCorrente
            {
                get
                {
                    return m_ContoCorrente;
                }
            }

            /// <summary>
            /// Imposta il conto corrente
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetContoCorrente(ContoCorrente value)
            {
                m_ContoCorrente = value;
                if (value is object)
                {
                    foreach (IntestatarioContoCorrente c in this)
                        c.SetContoCorrente(value);
                }
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, IntestatarioContoCorrente value)
            {
                if (m_ContoCorrente is object)
                    value.SetContoCorrente(m_ContoCorrente);

                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, IntestatarioContoCorrente oldValue, IntestatarioContoCorrente newValue)
            {
                if (m_ContoCorrente is object)
                    newValue.SetContoCorrente(m_ContoCorrente);

                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica gli intestatari
            /// </summary>
            /// <param name="conto"></param>
            protected void Load(ContoCorrente conto)
            {
                if (conto is null)
                    throw new ArgumentNullException("conto");
                Clear();
                m_ContoCorrente = conto;
                if (DBUtils.GetID(conto, 0) == 0)
                    return;

                using (var cursor = new IntestatarioContoCorrenteCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.IDContoCorrente.Value = DBUtils.GetID(conto, 0);
                    while (cursor.Read())
                    {
                        this.Add(cursor.Item);
                    }
                }
            }

            
        }
    }
}