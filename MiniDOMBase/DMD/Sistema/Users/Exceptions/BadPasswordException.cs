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
    /// Eccezione generata quando l'utente tenta di effettuare l'accesso utilizzando una password non corretta
    /// </summary>
    [Serializable]
    public class BadPasswordException
        : UserLoginException
    {
        private string m_BadPassword;

        /// <summary>
        /// Costruttore
        /// </summary>
        public BadPasswordException() 
            : this(DMD.Strings.vbNullString, "Password non corrispondente", "")
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="userName"></param>
        public BadPasswordException(string userName) 
            : this(userName, "[" + userName + "] Password non corrispondente", "")
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        /// <param name="pwd"></param>
        public BadPasswordException(string userName, string message, string pwd) 
            : base(userName, message)
        {
            m_BadPassword = pwd;
        }

        /// <summary>
        /// Quando abilitato restituisce la password inserita erroneamente
        /// </summary>
        public string BadPassword
        {
            get
            {
                return m_BadPassword;
            }
        }
    }
}