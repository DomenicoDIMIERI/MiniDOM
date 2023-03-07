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

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Evento relativo ad un modulo
        /// </summary>
        [Serializable]
        public class ModuleEventArgs
            : DMDEventArgs
        {
            
            [NonSerialized] private CModule m_Module;

            /// <summary>
            /// Costruttore
            /// </summary>
            public ModuleEventArgs()
            {
                //DMDObject.IncreaseCounter(this);
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="m"></param>
            public ModuleEventArgs(CModule m) : this()
            {
                m_Module = m;
            }

            /// <summary>
            /// Restituisce un riferimento al modulo
            /// </summary>
            public CModule Module
            {
                get
                {
                    return m_Module;
                }
            }

            //~ModuleEventArgs()
            //{
            //    //DMDObject.DecreaseCounter(this);
            //}
        }
    }
}