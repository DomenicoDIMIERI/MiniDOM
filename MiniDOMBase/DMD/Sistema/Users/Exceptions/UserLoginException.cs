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
using DMD.Exceptions;

namespace minidom
{

    /// <summary>
    /// Eccezione generata quando si verifica un errore in fase di login
    /// </summary>
    [Serializable]
    public class UserLoginException 
        : RuntimeException
    {
        private string m_UserName;

        /// <summary>
        /// Costruttore
        /// </summary>
        public UserLoginException() 
            : base("Errore generico di accesso")
        {
            //DMDObject.IncreaseCounter(this);
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="userName"></param>
        public UserLoginException(string userName) 
            : this(userName, "Errore generico di accesso")
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        public UserLoginException(string userName, string message) 
            : base(message)
        {
            m_UserName = userName;
            //DMDObject.IncreaseCounter(this);
        }

        /// <summary>
        /// UserName dell'utente che ha tentato l'accesso
        /// </summary>
        public string UserName
        {
            get
            {
                return m_UserName;
            }
        }

       
    }
}