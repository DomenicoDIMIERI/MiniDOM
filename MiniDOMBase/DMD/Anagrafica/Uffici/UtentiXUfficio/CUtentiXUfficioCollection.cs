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
    public partial class Anagrafica
    {

        /// <summary>
        /// Collezione di utenti per un ufficio
        /// </summary>
        [Serializable]
        public class CUtentiXUfficioCollection 
            : CCollection<Sistema.CUser>
        {
            [NonSerialized] private CUfficio m_Ufficio;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUtentiXUfficioCollection()
            {
                m_Ufficio = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="ufficio"></param>
            public CUtentiXUfficioCollection(CUfficio ufficio) : this()
            {
                Load(ufficio);
            }

            /// <summary>
            /// Restituisce un riferimento all'ufficio
            /// </summary>
            public CUfficio Ufficio
            {
                get
                {
                    return m_Ufficio;
                }
            }

            /// <summary>
            /// Carica gli utenti
            /// </summary>
            /// <param name="value"></param>
            protected internal void Load(CUfficio value)
            {
                if (value is null)
                    throw new ArgumentNullException("Ufficio");
                
                this.Clear();

                m_Ufficio = value;
                if (DBUtils.GetID(value, 0) == 0)
                    return;
                
                foreach(var item in minidom.Anagrafica.Uffici.UfficiConsentiti)
                {
                    if (   item.IDUfficio == DBUtils.GetID(value, 0) 
                        && item.Utente is object 
                        && item.Utente.Stato == ObjectStatus.OBJECT_VALID
                        )
                    {
                        Add(item.Utente);
                    }
                }
            }
        }
    }
}