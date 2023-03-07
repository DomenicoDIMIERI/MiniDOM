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
using static minidom.CustomerCalls;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CEMailMessage"/>
        /// </summary>
        [Serializable]
        public sealed class CEMailMessageslass
            : CContattiRepository<CEMailMessage>
        {


            /// <summary>
            /// Costruttore
            /// </summary>
            public CEMailMessageslass()
                : base("modContattiEMail", typeof(EMailMessagesCursor), 0)
            {
            }

             

        }

    }

    
    public partial class CustomerCalls
    {  

        private static CEMailMessageslass m_EMailMessages = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CEMailMessage"/>
        /// </summary>
        public static CEMailMessageslass EMailMessages
        {
            get
            {
                if (m_EMailMessages is null)
                    m_EMailMessages = new CEMailMessageslass();
                return m_EMailMessages;
            }
        }
    }
}