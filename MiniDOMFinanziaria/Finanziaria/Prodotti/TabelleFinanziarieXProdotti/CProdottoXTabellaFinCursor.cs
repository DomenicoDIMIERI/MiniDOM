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
    public partial class Finanziaria
    {


        /// <summary>
        /// Cursore che consente di recuperare tutte le tabelle finanziarie associate ad un prodotto
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CProdottoXTabellaFinCursor 
            : Databases.DBObjectCursor<CProdottoXTabellaFin>
        {
            private DBCursorField<int> m_ProdottoID;
            private DBCursorField<int> m_TabellaFinanziariaID;

            public CProdottoXTabellaFinCursor()
            {
                m_ProdottoID = new DBCursorField<int>("Prodotto");
                m_TabellaFinanziariaID = new DBCursorField<int>("Tabella");
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Finanziaria.TabelleFinanziarie; // modCQSPDTblFinanz
            }

            /// <summary>
            /// ProdottoID
            /// </summary>
            public DBCursorField<int> ProdottoID
            {
                get
                {
                    return m_ProdottoID;
                }
            }

            /// <summary>
            /// TabellaFinanziariaID
            /// </summary>
            public DBCursorField<int> TabellaFinanziariaID
            {
                get
                {
                    return m_TabellaFinanziariaID;
                }
            }
             

            
        }
    }
}