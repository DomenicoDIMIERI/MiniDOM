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
    /// Evento generato quando l'utente tenta di effettuare l'accesso usando un account scaduto
    /// </summary>
    [Serializable]
    public class UserExpiredException 
        : UserLoginException
    {
        private Sistema.UserStatus m_UserStatus;

        /// <summary>
        /// Costrutore
        /// </summary>
        public UserExpiredException() 
            : this(DMD.Strings.vbNullString, "L'account utente è scaduto")
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="userName"></param>
        public UserExpiredException(string userName) : this(userName, "[" + userName + "] L'account utente è scaduto")
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        public UserExpiredException(string userName, string message) : base(userName, message)
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="stato"></param>
        public UserExpiredException(string userName, Sistema.UserStatus stato) : base(userName, "[" + userName + "] L'account utente è scaduto: " + Enum.GetName(typeof(Sistema.UserStatus), stato))
        {
            m_UserStatus = stato;
        }

        /// <summary>
        /// Restituisce lo stato dell'utente
        /// </summary>
        public Sistema.UserStatus UserStatus
        {
            get
            {
                return m_UserStatus;
            }
        }
    }
}