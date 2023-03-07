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
        /// Pratiche caricate sotto un area manager
        /// </summary>
        [Serializable]
        public class CAMPraticheCollection
            : CPraticheCollection
        {
           
            [NonSerialized] private CAreaManager m_AreaManager;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAMPraticheCollection()
            {
                m_AreaManager = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="am"></param>
            public CAMPraticheCollection(CAreaManager am)
            {
                this.Initialize(am);
            }

            /// <summary>
            /// Carica la collezione
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            protected internal void Initialize(CAreaManager value)
            {
                if (value is null)
                    throw new ArgumentNullException("value");

                this.Clear();
                m_AreaManager = value;

                if (DBUtils.GetID(value, 0) == 0)
                    return;

                foreach(var tm in this.m_AreaManager.TeamManagers)
                { 
                    var pratiche = new CPraticheCollection(tm);
                    foreach(var pratica in pratiche)
                        this.Add(pratica);
                }
            }
        }
    }
}