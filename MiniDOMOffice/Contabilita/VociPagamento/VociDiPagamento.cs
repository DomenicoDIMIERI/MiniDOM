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
using static minidom.Contabilita;
namespace minidom
{
 
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="VoceDiPagamento"/>
        /// </summary>
        public class VociDiPagamentoClass
            : CModulesClass<VoceDiPagamento>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public VociDiPagamentoClass() 
                : base("modOfficeVociDiPagamento", typeof(VoceDiPagamentoCursor), 0)
            {
            }
        }
    }

    public partial class Contabilita
    {
        private static VociDiPagamentoClass m_VociDiPagamento = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="VoceDiPagamento"/>
        /// </summary>
        public static VociDiPagamentoClass VociDiPagamento
        {
            get
            {
                if (m_VociDiPagamento is null)
                    m_VociDiPagamento = new VociDiPagamentoClass();
                return m_VociDiPagamento;
            }
        }
    }
}