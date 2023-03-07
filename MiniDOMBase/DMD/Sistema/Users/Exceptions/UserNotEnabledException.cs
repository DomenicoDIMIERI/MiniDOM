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
    /// Eccezione generata quando l'utente tenta l'accesso con un account non abilitato
    /// </summary>
    [Serializable]
    public class UserNotEnabledException 
        : UserLoginException
    {
        private Sistema.UserStatus m_UserStatus;

        /// <summary>
        /// Costruttore
        /// </summary>
        public UserNotEnabledException() 
            : this(DMD.Strings.vbNullString, "Utente non abilitato")
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="userName"></param>
        public UserNotEnabledException(string userName)
            : this(userName, "[" + userName + "] Utente non abilitato")
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        public UserNotEnabledException(string userName, string message)
            : base(userName, message)
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="stato"></param>
        public UserNotEnabledException(string userName, Sistema.UserStatus stato) 
            : base(userName, "[" + userName + "] Utente non abilitato: " + Enum.GetName(typeof(Sistema.UserStatus), stato))
        {
            m_UserStatus = stato;
        }

        /// <summary>
        /// Stato dell'utente
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