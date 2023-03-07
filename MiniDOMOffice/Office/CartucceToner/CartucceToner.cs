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
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CartucciaToner"/>
        /// </summary>
        [Serializable]
        public sealed class CartucceTonerClass 
            : CModulesClass<minidom.Office.CartucciaToner>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CartucceTonerClass() 
                : base("modCartucceToner", typeof(minidom.Office.CartucceTonerCursor), 0)
            {
            }
        }
    }

    public partial class Office
    {
        private static CartucceTonerClass m_CartucceToners = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CartucciaToner"/>
        /// </summary>
        public static CartucceTonerClass CartucceToners
        {
            get
            {
                if (m_CartucceToners is null)
                    m_CartucceToners = new CartucceTonerClass();
                return m_CartucceToners;
            }
        }
    }
}