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
        /// Argomenti dell'evento 
        /// </summary>
        [Serializable]
        public class PrimaNotaSottoSogliaEventArgs 
            : DMDEventArgs
        {
            [NonSerialized]  private CUfficio m_Ufficio;
            private decimal m_Soglia;

            /// <summary>
            /// Costruttore
            /// </summary>
            public PrimaNotaSottoSogliaEventArgs()
            {

            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="ufficio"></param>
            /// <param name="soglia"></param>
            public PrimaNotaSottoSogliaEventArgs(CUfficio ufficio, decimal soglia)
            {
                DMDObject.IncreaseCounter(this);
                m_Ufficio = ufficio;
                m_Soglia = soglia;
            }

        
            /// <summary>
            /// Ufficio
            /// </summary>
            public CUfficio Ufficio
            {
                get
                {
                    return m_Ufficio;
                }
            }

            /// <summary>
            /// Soglia
            /// </summary>
            public decimal Soglia
            {
                get
                {
                    return m_Soglia;
                }
            }
        }

        /// <summary>
        /// Firma dell'evento SottoSoglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void SottoSogliaEventHandler(object sender, minidom.Office.PrimaNotaSottoSogliaEventArgs e);

    }
}