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

    /// <summary>
    /// Argomenti di un evento del modulo utenti
    /// </summary>
    [Serializable]
    public class UserEventArgs 
        : DMDEventArgs
    {
        [NonSerialized]  private CUser m_User;

        /// <summary>
        /// Costruttore
        /// </summary>
        public UserEventArgs()
        {
            //DMDObject.IncreaseCounter(this);
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="user"></param>
        public UserEventArgs(CUser user)
            : this()
        {
            m_User = user;
        }

        /// <summary>
        /// Restituisce un riferimento all'utente
        /// </summary>
        public CUser User
        {
            get
            {
                return m_User;
            }
        }
 
    }
}