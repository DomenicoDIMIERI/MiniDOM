using System;
using DMD;
using DMD.Databases;
using minidom;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella delle carte di credito
        /// </summary>
        [Serializable]
        public class CartaDiCreditoCursor 
            : minidom.Databases.DBObjectCursor<CartaDiCredito>
        {
            private DBCursorStringField m_Name = new DBCursorStringField("Name");
            private DBCursorField<int> m_IDContoCorrente = new DBCursorField<int>("IDContoCorrente");
            private DBCursorStringField m_NomeConto = new DBCursorStringField("NomeConto");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorStringField m_CircuitoCarta = new DBCursorStringField("CircuitoCarta");
            private DBCursorStringField m_CodiceVerifica = new DBCursorStringField("CodiceVerifica");
            private DBCursorStringField m_NomeIntestatario = new DBCursorStringField("NomeIntestatario");
            private DBCursorStringField m_NumeroCarta = new DBCursorStringField("NumeroCarta");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CartaDiCreditoCursor()
            {
            }

            /// <summary>
            /// Campo Name
            /// </summary>
            public DBCursorStringField Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// Campo IDContoCorrente
            /// </summary>
            public DBCursorField<int> IDContoCorrente
            {
                get
                {
                    return m_IDContoCorrente;
                }
            }

            /// <summary>
            /// Campo NomeConto
            /// </summary>
            public DBCursorStringField NomeConto
            {
                get
                {
                    return m_NomeConto;
                }
            }

            /// <summary>
            /// Campo DataInizio
            /// </summary>
            public DBCursorField<DateTime> DataInizio
            {
                get
                {
                    return m_DataInizio;
                }
            }

            /// <summary>
            /// Campo
            /// </summary>
            public DBCursorField<DateTime> DataFine
            {
                get
                {
                    return m_DataFine;
                }
            }

            /// <summary>
            /// Campo Flags
            /// </summary>
            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// Campo CircuitoCarta
            /// </summary>
            public DBCursorStringField CircuitoCarta
            {
                get
                {
                    return m_CircuitoCarta;
                }
            }

            /// <summary>
            /// Campo CodiceVerifica
            /// </summary>
            public DBCursorStringField CodiceVerifica
            {
                get
                {
                    return m_CodiceVerifica;
                }
            }

            /// <summary>
            /// Campo NomeIntestatario
            /// </summary>
            public DBCursorStringField NomeIntestatario
            {
                get
                {
                    return m_NomeIntestatario;
                }
            }

            /// <summary>
            /// Campo NumeroCarta
            /// </summary>
            public DBCursorStringField NumeroCarta
            {
                get
                {
                    return m_NumeroCarta;
                }
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.CarteDiCredito; //.Module;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_CarteDiCredito";
            //}
        }
    }
}