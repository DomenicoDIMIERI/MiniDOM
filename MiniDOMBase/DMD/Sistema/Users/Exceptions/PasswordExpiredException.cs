using System;


namespace minidom
{

    /// <summary>
    /// Eccezione generata quando un utente tenta di effettuare il login con una password scaduta
    /// </summary>
    [Serializable]
    public class PasswordExpiredException 
        : UserLoginException
    {
        private DateTime? m_ExpireDate;

        /// <summary>
        /// Costruttore
        /// </summary>
        public PasswordExpiredException() 
            : this(DMD.Strings.vbNullString, "Password scaduta", default)
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="userName"></param>
        public PasswordExpiredException(string userName) 
            : this(userName, "[" + userName + "] Password scaduta", default)
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        /// <param name="expireDate"></param>
        public PasswordExpiredException(string userName, string message, DateTime? expireDate) 
            : base(userName, message)
        {
            m_ExpireDate = expireDate;
        }

        /// <summary>
        /// Restituisce la data in cui la password è scaduta
        /// </summary>
        public DateTime? ExpireDate
        {
            get
            {
                return m_ExpireDate;
            }
        }
    }
}