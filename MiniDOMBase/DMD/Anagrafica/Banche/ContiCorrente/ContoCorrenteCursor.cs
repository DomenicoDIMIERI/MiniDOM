using System;
using DMD;
using DMD.XML;
using DMD.Databases;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella dei conti corrente
        /// </summary>
        [Serializable]
        public class ContoCorrenteCursor 
            : minidom.Databases.DBObjectCursor<ContoCorrente>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Numero = new DBCursorStringField("Numero");
            private DBCursorStringField m_IBAN = new DBCursorStringField("IBAN");
            private DBCursorStringField m_SWIFT = new DBCursorStringField("SWIFT");
            private DBCursorField<int> m_IDBanca = new DBCursorField<int>("IDBanca");
            private DBCursorStringField m_NomeBanca = new DBCursorStringField("NomeBanca");
            private DBCursorField<DateTime> m_DataApertura = new DBCursorField<DateTime>("DataApertura");
            private DBCursorField<DateTime> m_DataChiusura = new DBCursorField<DateTime>("DataChiusura");
            private DBCursorField<decimal> m_Saldo = new DBCursorField<decimal>("Saldo");
            private DBCursorField<decimal> m_SaldoDisponibile = new DBCursorField<decimal>("SaldoDisponibile");
            private DBCursorField<StatoContoCorrente> m_StatoContoCorrente = new DBCursorField<StatoContoCorrente>("StatoContoCorrente");
            private DBCursorField<ContoCorrenteFlags> m_Flags = new DBCursorField<ContoCorrenteFlags>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public ContoCorrenteCursor()
            {
            }

            /// <summary>
            /// Campo Nome
            /// </summary>
            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            /// <summary>
            /// Campo Numero
            /// </summary>
            public DBCursorStringField Numero
            {
                get
                {
                    return m_Numero;
                }
            }

            /// <summary>
            /// Campo IBAN
            /// </summary>
            public DBCursorStringField IBAN
            {
                get
                {
                    return m_IBAN;
                }
            }

            /// <summary>
            /// Campo SWIFT
            /// </summary>
            public DBCursorStringField SWIFT
            {
                get
                {
                    return m_SWIFT;
                }
            }

            /// <summary>
            /// Campo IDBanca
            /// </summary>
            public DBCursorField<int> IDBanca
            {
                get
                {
                    return m_IDBanca;
                }
            }

            /// <summary>
            /// Campo NomeBanca
            /// </summary>
            public DBCursorStringField NomeBanca
            {
                get
                {
                    return m_NomeBanca;
                }
            }

            /// <summary>
            /// Campo DataApertura
            /// </summary>
            public DBCursorField<DateTime> DataApertura
            {
                get
                {
                    return m_DataApertura;
                }
            }

            /// <summary>
            /// Campo DataChiusura
            /// </summary>
            public DBCursorField<DateTime> DataChiusura
            {
                get
                {
                    return m_DataChiusura;
                }
            }

            /// <summary>
            /// Campo Saldo
            /// </summary>
            public DBCursorField<decimal> Saldo
            {
                get
                {
                    return m_Saldo;
                }
            }

            /// <summary>
            /// Campo SaldoDisponibile
            /// </summary>
            public DBCursorField<decimal> SaldoDisponibile
            {
                get
                {
                    return m_SaldoDisponibile;
                }
            }

            /// <summary>
            /// Campo StatoContoCorrente
            /// </summary>
            public DBCursorField<StatoContoCorrente> StatoContoCorrente
            {
                get
                {
                    return m_StatoContoCorrente;
                }
            }

            /// <summary>
            /// Campo Flags
            /// </summary>
            public DBCursorField<ContoCorrenteFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}

            //protected override Sistema.CModule GetModule()
            //{
            //    return ContiCorrente.Module;
            //}

            //public override string GetTableName()
            //{
            //    return "tbl_ContiCorrenti";
            //}
        }
    }
}