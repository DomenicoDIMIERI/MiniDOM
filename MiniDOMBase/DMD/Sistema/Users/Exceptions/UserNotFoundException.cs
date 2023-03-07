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
    /// Eccezione generata quando l'utente tenta di effettuare l'accesso con un account inesistente
    /// </summary>
    [Serializable]
    public class UserNotFoundException 
        : UserLoginException
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public UserNotFoundException() 
            : this(DMD.Strings.vbNullString, "Utente inesistente")
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="userName"></param>
        public UserNotFoundException(string userName) 
            : this(userName, "[" + userName + "] Utente inesistente")
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        public UserNotFoundException(string userName, string message) 
            : base(userName, message)
        {
        }
    }
}