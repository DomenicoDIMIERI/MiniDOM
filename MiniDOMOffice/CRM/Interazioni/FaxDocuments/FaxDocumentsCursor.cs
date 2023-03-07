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
        /// Cursore di oggetti di tipo <see cref="FaxDocument"/>
        /// </summary>
        [Serializable]
        public class FaxDocumentsCursor 
            : CContattoUtenteCursor
        {

           
            /// <summary>
            /// Costruttore
            /// </summary>
            public FaxDocumentsCursor()
            {
                base.ClassName.Value = "FAXDocument";
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
                return new FaxDocument();
            }

            /// <summary>
            /// Oggetto corrente
            /// </summary>
            public new FaxDocument Item
            {
                get
                {
                    return (FaxDocument)base.Item;
                }

                set
                {
                    base.Item = value;
                }
            }

            /// <summary>
            /// Aggiunge una riga
            /// </summary>
            /// <returns></returns>
            public new FaxDocument Add()
            {
                return (FaxDocument)base.Add();
            }
        }
    }
}