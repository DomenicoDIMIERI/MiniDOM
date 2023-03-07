using System;
using DMD;
using DMD.Databases;
using minidom.repositories;
using static minidom.Sistema;

namespace minidom
{
    public partial class Anagrafica
    {


        /// <summary>
        /// Cursore sulla tabella delle voci di manutenzione
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class VociManutenzioneCursor 
            : minidom.Databases.DBObjectCursorPO<VoceManutenzione>
        {
            private DBCursorField<int> m_IDManutenzione = new DBCursorField<int>("IDManutenzione");
            private DBCursorStringField m_Categoria1 = new DBCursorStringField("Categoria1");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<int> m_IDOggettoRimosso = new DBCursorField<int>("IDOggettoRimosso");
            private DBCursorStringField m_NomeOggettoRimosso = new DBCursorStringField("NomeOggettoRimosso");
            private DBCursorField<int> m_IDOggetto = new DBCursorField<int>("IDOggetto");
            private DBCursorStringField m_NomeOggetto = new DBCursorStringField("NomeOggetto");
            private DBCursorField<decimal> m_ValoreImponibile = new DBCursorField<decimal>("ValoreImponibile");
            private DBCursorField<decimal> m_ValoreIvato = new DBCursorField<decimal>("ValoreIvato");
            private DBCursorField<AzioneManutenzione> m_Azione = new DBCursorField<AzioneManutenzione>("Azione");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public VociManutenzioneCursor()
            {
            }

            /// <summary>
            /// Azione
            /// </summary>
            public DBCursorField<AzioneManutenzione> Azione
            {
                get
                {
                    return m_Azione;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// IDManutenzione
            /// </summary>
            public DBCursorField<int> IDManutenzione
            {
                get
                {
                    return m_IDManutenzione;
                }
            }

            /// <summary>
            /// Categoria1
            /// </summary>
            public DBCursorStringField Categoria1
            {
                get
                {
                    return m_Categoria1;
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
            /// IDOggettoRimosso
            /// </summary>
            public DBCursorField<int> IDOggettoRimosso
            {
                get
                {
                    return m_IDOggettoRimosso;
                }
            }

            /// <summary>
            /// NomeOggettoRimosso
            /// </summary>
            public DBCursorStringField NomeOggettoRimosso
            {
                get
                {
                    return m_NomeOggettoRimosso;
                }
            }

            /// <summary>
            /// IDOggetto
            /// </summary>
            public DBCursorField<int> IDOggetto
            {
                get
                {
                    return m_IDOggetto;
                }
            }

            /// <summary>
            /// NomeOggetto
            /// </summary>
            public DBCursorStringField NomeOggetto
            {
                get
                {
                    return m_NomeOggetto;
                }
            }

            /// <summary>
            /// ValoreImponibile
            /// </summary>
            public DBCursorField<decimal> ValoreImponibile
            {
                get
                {
                    return m_ValoreImponibile;
                }
            }

            /// <summary>
            /// ValoreIvato
            /// </summary>
            public DBCursorField<decimal> ValoreIvato
            {
                get
                {
                    return m_ValoreIvato;
                }
            }

            //public override object InstantiateNew(DBReader dbRis)
            //{
            //    return new VoceManutenzione();
            //}

            //public override string GetTableName()
            //{
            //    return "tbl_ManutenzioniVoci";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Manutenzioni.Voci; //.Module;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}
        }
    }
}