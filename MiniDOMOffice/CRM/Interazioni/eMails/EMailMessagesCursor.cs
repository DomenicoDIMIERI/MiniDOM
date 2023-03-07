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
        /// Cursore di oggetti <see cref="CEMailMessage"/>
        /// </summary>
        [Serializable]
        public class EMailMessagesCursor 
            : CContattoUtenteCursor
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public EMailMessagesCursor()
            {
                base.ClassName.Value = "EMailMessage";
            }

            private new object ClassName
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
                return new CEMailMessage();
            }


            /// <summary>
            /// Oggetto corrente
            /// </summary>
            public new CEMailMessage Item
            {
                get
                {
                    return (CEMailMessage)base.Item;
                }

                set
                {
                    base.Item = value;
                }
            }

            /// <summary>
            /// Aggiunge
            /// </summary>
            /// <returns></returns>
            public new CEMailMessage Add()
            {
                return (CEMailMessage)base.Add();
            }

        }
    }
}