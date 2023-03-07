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
    /// Eccezione generata quando l'utente effettua l'accesso con una password scaduta
    /// </summary>
    [Serializable]
    public class UserForcePwdPasswordException 
        : UserLoginException
    {
        /// <summary>
        /// Costruttore
        /// </summary>
        public UserForcePwdPasswordException() 
            : this(DMD.Strings.vbNullString, "Cambiamento password richiesto")
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="userName"></param>
        public UserForcePwdPasswordException(string userName) 
            : this(userName, "Cambiamento password richiesto")
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        public UserForcePwdPasswordException(string userName, string message) 
            : base(userName, message)
        {
        }
    }
}