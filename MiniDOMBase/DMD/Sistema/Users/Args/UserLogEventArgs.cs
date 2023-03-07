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
    /// Argomenti degli eventi di accesso (login e logout) di un utente
    /// </summary>
    [Serializable]
    public abstract class UserLogEventArgs 
        : UserEventArgs
    {
        private string m_Params;

        /// <summary>
        /// Costruttore
        /// </summary>
        public UserLogEventArgs()
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="user"></param>
        /// <param name="params"></param>
        public UserLogEventArgs(CUser user, string @params = DMD.Strings.vbNullString)
            : base(user)
        {
            m_Params = @params;
        }

        /// <summary>
        /// Parametri di accesso
        /// </summary>
        public string Params
        {
            get
            {
                return m_Params;
            }
        }
    }
}