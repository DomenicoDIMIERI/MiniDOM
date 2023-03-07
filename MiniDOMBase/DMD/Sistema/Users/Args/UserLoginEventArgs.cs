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
    /// Argomento dell'evento UserLogin
    /// </summary>
    [Serializable]
    public class UserLoginEventArgs 
        : UserLogEventArgs
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public UserLoginEventArgs()
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="user"></param>
        public UserLoginEventArgs(Sistema.CUser user)
            : this(user, "Login di [" + user.UserName + "]")
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        public UserLoginEventArgs(Sistema.CUser user, string message) 
            : base(user, message)
        {
        }
    }
}