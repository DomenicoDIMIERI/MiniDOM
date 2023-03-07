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
    public partial class CustomerCalls
    {

        /// <summary>
        /// Cursore sulla tabella dei telegrammi
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CTelegrammiCursor 
            : CContattoUtenteCursor 
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTelegrammiCursor()
            {
                base.ClassName.Value = "CTelegramma";
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
                return new CTelegramma();
            }

            /// <summary>
            /// Restituisce l'oggetto corrente
            /// </summary>
            public new CTelegramma Item
            {
                get
                {
                    return (CTelegramma)base.Item;
                }

                set
                {
                    base.Item = value;
                }
            }

            /// <summary>
            /// Aggiunge una nuova riga
            /// </summary>
            /// <returns></returns>
            public new CTelegramma Add()
            {
                return (CTelegramma)base.Add();
            }
        }
    }
}