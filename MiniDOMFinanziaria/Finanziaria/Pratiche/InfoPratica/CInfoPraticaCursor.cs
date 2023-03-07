using System;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Cursor sulla tabella delle informazioni aggiuntive sulle pratice
    /// </summary>
    /// <remarks></remarks>
        public class CInfoPraticaCursor : Databases.DBObjectCursorBase<CInfoPratica>
        {
            private DBCursorField<int> m_IDPratica = new DBCursorField<int>("IDPratica");
            private DBCursorField<int> m_IDCommerciale = new DBCursorField<int>("IDCommerciale");
            // Private m_IDConsulente As New DBCursorField(Of Integer)("IDConsulente")
            private DBCursorField<int> m_IDDistributore = new DBCursorField<int>("IDDistributore");
            private DBCursorField<bool> m_Trasferita = new DBCursorField<bool>("Trasferita");
            private DBCursorStringField m_TrasferitoDaURL = new DBCursorStringField("TrasferitoDaURL");
            private DBCursorField<DateTime> m_DataTrasferimento = new DBCursorField<DateTime>("DataTrasferimento");
            private DBCursorStringField m_TrasferitoA = new DBCursorStringField("TrasferitoA");
            private DBCursorField<int> m_IDTrasferitoDa = new DBCursorField<int>("IDTrasferitoDa");
            private DBCursorField<int> m_IDPraticaTrasferita = new DBCursorField<int>("IDPraticaTrasferita");
            private DBCursorField<DateTime> m_DataAggiornamentoPT = new DBCursorField<DateTime>("DataAggiornamentoPT");
            private DBCursorField<int> m_EsitoAggiornamentoPT = new DBCursorField<int>("EsitoAggiornamentoPT");
            private DBCursorField<decimal> m_Costo = new DBCursorField<decimal>("Costo");
            private DBCursorField<int> m_IDPraticaDiRiferimento = new DBCursorField<int>("IDPraticaDiRiferimento");
            private DBCursorField<int> m_IDCorrezione = new DBCursorField<int>("IDCorrezione");
            private DBCursorStringField m_MotivoSconto = new DBCursorStringField("MotivoSconto");
            private DBCursorStringField m_MotivoScontoDettaglio = new DBCursorStringField("MotivoScontoDettaglio");
            private DBCursorField<int> m_IDScontoAutorizzatoDa = new DBCursorField<int>("IDScontoAutorizzatoDa");
            private DBCursorField<DateTime> m_ScontoAutorizzatoIl = new DBCursorField<DateTime>("ScontoAutorizzatoIl");
            private DBCursorStringField m_ScontoAutorizzatoNote = new DBCursorStringField("ScontoAutorizzatoNote");
            private DBCursorField<int> m_IDCorrettaDa = new DBCursorField<int>("IDCorrettaDa");
            private DBCursorField<DateTime> m_DataCorrezione = new DBCursorField<DateTime>("DataCorrezione");
            private DBCursorStringField m_NoteAmministrative = new DBCursorStringField("NoteAmministrative");
            private DBCursorField<decimal> m_ValoreUpFront = new DBCursorField<decimal>("ValoreUpFront");
            private DBCursorField<decimal> m_ValoreProvvTAN = new DBCursorField<decimal>("ValoreProvvTAN");
            private DBCursorField<decimal> m_ValoreProvvAGG = new DBCursorField<decimal>("ValoreProvvAGG");
            private DBCursorField<decimal> m_ValoreProvvTOT = new DBCursorField<decimal>("ValoreProvvTOT");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");

            public CInfoPraticaCursor()
            {
            }

            public DBCursorField<decimal> ValoreUpFront
            {
                get
                {
                    return m_ValoreUpFront;
                }
            }

            public DBCursorField<decimal> ValoreProvvTAN
            {
                get
                {
                    return m_ValoreProvvTAN;
                }
            }

            public DBCursorField<decimal> ValoreProvvAGG
            {
                get
                {
                    return m_ValoreProvvAGG;
                }
            }

            public DBCursorField<decimal> ValoreProvvTOT
            {
                get
                {
                    return m_ValoreProvvTOT;
                }
            }

            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            public DBCursorField<int> IDCorrettaDa
            {
                get
                {
                    return m_IDCorrettaDa;
                }
            }

            public DBCursorField<DateTime> DataCorrezione
            {
                get
                {
                    return m_DataCorrezione;
                }
            }

            public DBCursorField<int> IDPratica
            {
                get
                {
                    return m_IDPratica;
                }
            }

            public DBCursorField<int> IDCommerciale
            {
                get
                {
                    return m_IDCommerciale;
                }
            }


            // Public ReadOnly Property IDConsulente As DBCursorField(Of Integer)
            // Get
            // Return Me.m_IDConsulente
            // End Get
            // End Property

            public DBCursorField<bool> Trasferita
            {
                get
                {
                    return m_Trasferita;
                }
            }

            public DBCursorStringField TrasferitoDaURL
            {
                get
                {
                    return m_TrasferitoDaURL;
                }
            }

            public DBCursorField<DateTime> DataTrasferimento
            {
                get
                {
                    return m_DataTrasferimento;
                }
            }

            public DBCursorStringField TrasferitoA
            {
                get
                {
                    return m_TrasferitoA;
                }
            }

            public DBCursorField<int> IDTrasferitoDa
            {
                get
                {
                    return m_IDTrasferitoDa;
                }
            }

            public DBCursorField<int> IDPraticaTrasferita
            {
                get
                {
                    return m_IDPraticaTrasferita;
                }
            }

            public DBCursorField<DateTime> DataAggiornamentoPratica
            {
                get
                {
                    return m_DataAggiornamentoPT;
                }
            }

            public DBCursorField<int> EsitoAggiornamentoPratica
            {
                get
                {
                    return m_EsitoAggiornamentoPT;
                }
            }

            public DBCursorField<decimal> Costo
            {
                get
                {
                    return m_Costo;
                }
            }

            public DBCursorField<int> IDPraticaDiRiferimento
            {
                get
                {
                    return m_IDPraticaDiRiferimento;
                }
            }

            public DBCursorField<int> IDDistributore
            {
                get
                {
                    return m_IDDistributore;
                }
            }

            public DBCursorField<int> IDCorrezione
            {
                get
                {
                    return m_IDCorrezione;
                }
            }

            public DBCursorStringField MotivoSconto
            {
                get
                {
                    return m_MotivoSconto;
                }
            }

            public DBCursorStringField MotivoScontoDettaglio
            {
                get
                {
                    return m_MotivoScontoDettaglio;
                }
            }

            public DBCursorField<int> IDScontoAutorizzatoDa
            {
                get
                {
                    return m_IDScontoAutorizzatoDa;
                }
            }

            public DBCursorField<DateTime> ScontoAutorizzatoIl
            {
                get
                {
                    return m_ScontoAutorizzatoIl;
                }
            }

            public DBCursorStringField ScontoAutorizzatoNote
            {
                get
                {
                    return m_ScontoAutorizzatoNote;
                }
            }

            protected override Sistema.CModule GetModule()
            {
                return null;
            }

            public override string GetTableName()
            {
                return "tbl_PraticheInfo";
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CInfoPratica();
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public DBCursorStringField NoteAmministrative
            {
                get
                {
                    return m_NoteAmministrative;
                }
            }
        }
    }
}