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
using static minidom.Office;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Cursore di oggetti <see cref="SMSMessage"/>
        /// </summary>
        [Serializable]
        public class SMSMessageCursor 
            : CContattoUtenteCursor
        {

            // Private m_Numero As New DBCursorFieldObj(Of String)("Numero")

            /// <summary>
            /// Costruttore
            /// </summary>
            public SMSMessageCursor()
            {
                base.ClassName.Value = "SMSMessage";
            }

            /// <summary>
            /// Nasconde
            /// </summary>
            private object ClassName
            {
                get
                {
                    return null;
                }
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CContattoUtente InstantiateNewT(DBReader dbRis)
            {
                return new SMSMessage();
            }

            /// <summary>
            /// Oggetto corrente
            /// </summary>
            public new SMSMessage Item
            {
                get
                {
                    return (SMSMessage)base.Item;
                }

                set
                {
                    base.Item = value;
                }
            }

            /// <summary>
            /// Aggiunge un nuovo elemento
            /// </summary>
            /// <returns></returns>
            public new SMSMessage Add()
            {
                return (SMSMessage)base.Add();
            }
        }
    }
}