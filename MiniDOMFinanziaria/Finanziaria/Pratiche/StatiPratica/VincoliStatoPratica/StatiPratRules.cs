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
using static minidom.Finanziaria;


namespace minidom
{
    namespace repositories
    {
        /// <summary>
        /// Repository di oggetti di tipo <see cref="CStatoPratRule"/>
        /// </summary>
        /// <remarks></remarks>
        public sealed class CStatiPratRulesClass
            : CModulesClass<CStatoPratRule>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CStatiPratRulesClass() 
                : base("modCQSPDStPrtRul", typeof(CStatoPratRuleCursor), -1)
            {
            }
        }

    }

    public partial class Finanziaria
    {


        
      
        private static CStatiPratRulesClass m_StatiPratRules = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CStatoPratRule"/>
        /// </summary>
        public static CStatiPratRulesClass StatiPratRules
        {
            get
            {
                if (m_StatiPratRules is null)
                    m_StatiPratRules = new CStatiPratRulesClass();
                return m_StatiPratRules;
            }
        }
    }
}