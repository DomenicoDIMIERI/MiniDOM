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
    /// Argomenti dell'evento UserLogout
    /// </summary>
    [Serializable]
    public class UserLogoutEventArgs 
        : UserLogEventArgs
    {
        private Sistema.LogOutMethods m_Method;

        /// <summary>
        /// Costruttore
        /// </summary>
        public UserLogoutEventArgs()
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="user"></param>
        /// <param name="method"></param>
        /// <param name="params"></param>
        public UserLogoutEventArgs(Sistema.CUser user, Sistema.LogOutMethods method, string @params = DMD.Strings.vbNullString) : base(user, @params)
        {
            m_Method = method;
        }


        /// <summary>
        /// Metodo usato per la disconnessione
        /// </summary>
        public Sistema.LogOutMethods Method
        {
            get
            {
                return m_Method;
            }
        }
    }
}
