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
        /// Cursore di oggetti di tipo <see cref="CTelefonata"/>
        /// </summary>
        [Serializable]
        public class CTelefonateCursor
            : CContattoUtenteCursor
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTelefonateCursor()
            {
                base.ClassName.Value = "CTelefonata";
            }

            /// <summary>
            /// Sovrascrivo
            /// </summary>
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
                return new CTelefonata();
            }

            /// <summary>
            /// Restituisce l'elemento corrente
            /// </summary>
            public new CTelefonata Item
            {
                get
                {
                    return (CTelefonata)base.Item;
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
            public new CTelefonata Add()
            {
                return (CTelefonata)base.Add();
            }
        }
    }
}