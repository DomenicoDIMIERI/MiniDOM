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
        /// Cursore sulla tabella degli appunti
        /// </summary>
        /// <remarks></remarks>
        public class CAppuntiCursor 
            : CContattoUtenteCursor
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAppuntiCursor()
            {
                base.ClassName.Value = "CAppunto";
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
                return new CAppunto();
            }

            /// <summary>
            /// Restituisce l'elemento corrente
            /// </summary>
            public new CAppunto Item
            {
                get
                {
                    return (CAppunto)base.Item;
                }

                set
                {
                    base.Item = value;
                }
            }

            /// <summary>
            /// Aggiunge un record
            /// </summary>
            /// <returns></returns>
            public new CAppunto Add()
            {
                return (CAppunto)base.Add();
            }
        }
    }
}