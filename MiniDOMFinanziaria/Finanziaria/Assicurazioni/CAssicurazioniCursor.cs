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
        /// Cursore sulla tabella delle assicurazioni
        /// </summary>
        [Serializable]
        public class CAssicurazioniCursor 
            : minidom.Databases.DBObjectCursor<CAssicurazione>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("descrizione");
            private DBCursorField<int> m_MeseScattoEta = new DBCursorField<int>("mesescattoeta");
            private DBCursorField<int> m_MeseScattoAnzianita = new DBCursorField<int>("mesescattoanzianita");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAssicurazioniCursor()
            {
            }

            /// <summary>
            /// Nome
            /// </summary>
            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            /// <summary>
            /// Descrizione
            /// </summary>
            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            /// <summary>
            /// MeseScattoEta
            /// </summary>
            public DBCursorField<int> MeseScattoEta
            {
                get
                {
                    return m_MeseScattoEta;
                }
            }

            /// <summary>
            /// MeseScattoAnzianita
            /// </summary>
            public DBCursorField<int> MeseScattoAnzianita
            {
                get
                {
                    return m_MeseScattoAnzianita;
                }
            }
           
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Finanziaria.Assicurazioni;
            }


            //public override string GetTableName()
            //{
            //    return "tbl_Assicurazioni";
            //}

            //public override object InstantiateNew(DBReader dbRis)
            //{
            //    return new CAssicurazione();
            //}
        }
    }
}