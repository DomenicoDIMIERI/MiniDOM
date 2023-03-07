using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;
using System.ComponentModel;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Collezione di CAP assegnati ad un comune
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CIntervalliCAP 
            : CCollection<CIntervalloCAP>
        {

            [NonSerialized] private CComune m_Comune;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CIntervalliCAP()
            {
                m_Comune = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="comune"></param>
            public CIntervalliCAP(CComune comune)
            {
                if (comune is null)
                    throw new ArgumentNullException("comune");
                m_Comune = comune;
                Load();
            }

            /// <summary>
            /// Restituisce un riferimento al comune
            /// </summary>
            [Browsable(false)]
            public CComune Comune
            {
                get
                {
                    return m_Comune;
                }
            }

            /// <summary>
            /// Imposta il comune
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetComune(CComune value)
            {
                m_Comune = value;
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    var tmp = this[i];
                    tmp.SetComune(value);
                }
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CIntervalloCAP value)
            {
                if (m_Comune is object)
                    value.SetComune(m_Comune);

                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CIntervalloCAP oldValue, CIntervalloCAP newValue)
            {
                if (m_Comune is object)
                    newValue.SetComune(m_Comune);

                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica gli intervalli
            /// </summary>
            public void Load()
            {
                Clear();
                if (DBUtils.GetID(m_Comune, 0) == 0)
                    return;

                var items = minidom.Anagrafica.Luoghi.Comuni.IntervalliCAP.LoadAll();
                foreach(var item in items)
                {
                    if (item.IDComune == DBUtils.GetID(this.m_Comune, 0))
                        this.Add(item);                     
                }
                 
            }
        }
    }
}