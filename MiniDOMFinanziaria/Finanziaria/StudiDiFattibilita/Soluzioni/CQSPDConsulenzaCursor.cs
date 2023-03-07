using System;
using System.Linq;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Cursore sulla tabella delle consulenze
    /// </summary>
    /// <remarks></remarks>
        public class CQSPDConsulenzaCursor : Databases.DBObjectCursorPO<CQSPDConsulenza>
        {
            private DBCursorField<int> m_IDStudioDiFattibilita = new DBCursorField<int>("IDGruppo");
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorField<int> m_IDRichiesta = new DBCursorField<int>("IDRichiesta");
            // Private m_IDOffertaCorrente As Integer          'ID dell'offerta corrente
            private DBCursorField<int> m_IDConsulente = new DBCursorField<int>("IDConsulente");
            private DBCursorStringField m_NomeConsulente = new DBCursorStringField("NomeConsulente");
            private DBCursorField<DateTime> m_DataConsulenza = new DBCursorField<DateTime>("DataConsulenza");
            private DBCursorField<DateTime> m_DataConferma = new DBCursorField<DateTime>("DataConferma");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<ConsulenzeFlags> m_Flags = new DBCursorField<ConsulenzeFlags>("Flags");
            private DBCursorField<StatiConsulenza> m_StatoConsulenza = new DBCursorField<StatiConsulenza>("StatoConsulenza");
            private DBCursorField<int> m_IDOffertaCQS = new DBCursorField<int>("IDOffertaCQS");
            private DBCursorField<int> m_IDOffertaPD = new DBCursorField<int>("IDOffertaPD");
            private DBCursorField<DateTime> m_DataProposta = new DBCursorField<DateTime>("DataProposta");
            private DBCursorField<int> m_IDPropostaDa = new DBCursorField<int>("IDPropostaDa");
            private DBCursorField<int> m_IDConfermataDa = new DBCursorField<int>("IDConfermataDa");
            private DBCursorField<int> m_IDContesto = new DBCursorField<int>("IDContesto");
            private DBCursorStringField m_TipoContesto = new DBCursorStringField("TipoContesto");
            private DBCursorField<double> m_Durata = new DBCursorField<double>("Durata");
            private DBCursorField<int> m_IDAzienda = new DBCursorField<int>("IDAzienda");
            private DBCursorField<int> m_IDInseritoDa = new DBCursorField<int>("IDInseritoDa");
            private DBCursorField<int> m_IDRichiestaApprovazione = new DBCursorField<int>("IDRichiestaApprovazione");
            private DBCursorStringField m_MotivoAnnullamento = new DBCursorStringField("MotivoAnnullamento");
            private DBCursorStringField m_DettaglioAnnullamento = new DBCursorStringField("DettaglioAnnullamento");
            private DBCursorField<int> m_IDAnnullataDa = new DBCursorField<int>("IDAnnullataDa");
            private DBCursorStringField m_NomeAnnullataDa = new DBCursorStringField("NomeAnnullataDa");
            private DBCursorField<DateTime> m_DataAnnullamento = new DBCursorField<DateTime>("DataAnnullamento");
            private DBCursorField<int> m_IDFinestraLavorazione = new DBCursorField<int>("IDFinestraLavorazione");
            private DBCursorField<int> m_IDUltimaVerifica = new DBCursorField<int>("IDUltimaVerifica");
            private DBCursorField<int> m_IDCollaboratore = new DBCursorField<int>("IDCollaboratore");
            private DBCursorField<int> m_IDCessionario = new DBCursorField<int>("IDCessionario");
            private DBCursorStringField m_NomeCessionario = new DBCursorStringField("NomeCessionario");
            private DBCursorField<int> m_IDProfilo = new DBCursorField<int>("IDProfilo");
            private DBCursorStringField m_NomeProfilo = new DBCursorStringField("NomeProfilo");
            private DBCursorField<int> m_IDProdotto = new DBCursorField<int>("IDProdotto");
            private DBCursorStringField m_NomeProdotto = new DBCursorStringField("NomeProdotto");
            private DBCursorStringField m_CategoriaProdotto = new DBCursorStringField("CategoriaProdotto");
            private DBCursorField<double> m_Rata = new DBCursorField<double>("Rata");
            private DBCursorField<int> m_NumeroRate = new DBCursorField<int>("NumeroRate");

            public CQSPDConsulenzaCursor()
            {
            }

            public DBCursorField<int> IDCessionario
            {
                get
                {
                    return m_IDCessionario;
                }
            }

            public DBCursorStringField NomeCessionario
            {
                get
                {
                    return m_NomeCessionario;
                }
            }

            public DBCursorField<int> IDProfilo
            {
                get
                {
                    return m_IDProfilo;
                }
            }

            public DBCursorStringField NomeProfilo
            {
                get
                {
                    return m_NomeProfilo;
                }
            }

            public DBCursorField<int> IDProdotto
            {
                get
                {
                    return m_IDProdotto;
                }
            }

            public DBCursorStringField NomeProdotto
            {
                get
                {
                    return m_NomeProdotto;
                }
            }

            public DBCursorStringField CategoriaProdotto
            {
                get
                {
                    return m_CategoriaProdotto;
                }
            }

            public DBCursorField<double> Rata
            {
                get
                {
                    return m_Rata;
                }
            }

            public DBCursorField<int> NumeroRate
            {
                get
                {
                    return m_NumeroRate;
                }
            }

            public DBCursorField<int> IDCollaboratore
            {
                get
                {
                    return m_IDCollaboratore;
                }
            }

            public DBCursorField<int> IDUltimaVerifica
            {
                get
                {
                    return m_IDUltimaVerifica;
                }
            }

            public DBCursorField<int> IDFinestraLavorazione
            {
                get
                {
                    return m_IDFinestraLavorazione;
                }
            }

            public DBCursorField<int> IDAnnullataDa
            {
                get
                {
                    return m_IDAnnullataDa;
                }
            }

            public DBCursorStringField NomeAnnullataDa
            {
                get
                {
                    return m_NomeAnnullataDa;
                }
            }

            public DBCursorField<DateTime> DataAnnullamento
            {
                get
                {
                    return m_DataAnnullamento;
                }
            }

            public DBCursorStringField MotivoAnnullamento
            {
                get
                {
                    return m_MotivoAnnullamento;
                }
            }

            public DBCursorStringField DettaglioAnnullamento
            {
                get
                {
                    return m_DettaglioAnnullamento;
                }
            }

            public DBCursorField<int> IDRichiestaApprovazione
            {
                get
                {
                    return m_IDRichiestaApprovazione;
                }
            }

            public DBCursorField<int> IDInseritoDa
            {
                get
                {
                    return m_IDInseritoDa;
                }
            }

            public DBCursorField<int> IDStudioDiFattibilita
            {
                get
                {
                    return m_IDStudioDiFattibilita;
                }
            }

            public DBCursorField<double> Durata
            {
                get
                {
                    return m_Durata;
                }
            }

            public DBCursorField<int> IDContesto
            {
                get
                {
                    return m_IDContesto;
                }
            }

            public DBCursorStringField TipoContesto
            {
                get
                {
                    return m_TipoContesto;
                }
            }

            public DBCursorField<DateTime> DataProposta
            {
                get
                {
                    return m_DataProposta;
                }
            }

            public DBCursorField<int> IDPropostaDa
            {
                get
                {
                    return m_IDPropostaDa;
                }
            }

            public DBCursorField<int> IDConfermataDa
            {
                get
                {
                    return m_IDConfermataDa;
                }
            }

            public DBCursorField<int> IDOffertaCQS
            {
                get
                {
                    return m_IDOffertaCQS;
                }
            }

            public DBCursorField<int> IDOffertaPD
            {
                get
                {
                    return m_IDOffertaPD;
                }
            }

            public DBCursorField<StatiConsulenza> StatoConsulenza
            {
                get
                {
                    return m_StatoConsulenza;
                }
            }

            public DBCursorField<int> IDCliente
            {
                get
                {
                    return m_IDCliente;
                }
            }

            public DBCursorStringField NomeCliente
            {
                get
                {
                    return m_NomeCliente;
                }
            }

            public DBCursorField<int> IDRichiesta
            {
                get
                {
                    return m_IDRichiesta;
                }
            }

            public DBCursorField<int> IDConsulente
            {
                get
                {
                    return m_IDConsulente;
                }
            }

            public DBCursorStringField NomeConsulente
            {
                get
                {
                    return m_NomeConsulente;
                }
            }

            public DBCursorField<DateTime> DataConsulenza
            {
                get
                {
                    return m_DataConsulenza;
                }
            }

            public DBCursorField<DateTime> DataConferma
            {
                get
                {
                    return m_DataConferma;
                }
            }

            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            public DBCursorField<ConsulenzeFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            public DBCursorField<int> IDAzienda
            {
                get
                {
                    return m_IDAzienda;
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CQSPDConsulenza();
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDConsulenze";
            }

            protected override Sistema.CModule GetModule()
            {
                return Consulenze.Module;
            }

            protected override CKeyCollection<Databases.DBCursorField> GetWhereFields()
            {
                var ret = new CKeyCollection<Databases.DBCursorField>(base.GetWhereFields());
                ret.Remove(m_IDCessionario);
                ret.Remove(m_NomeCessionario);
                ret.Remove(m_IDProfilo);
                ret.Remove(m_NomeProfilo);
                ret.Remove(m_IDProdotto);
                ret.Remove(m_NomeProdotto);
                ret.Remove(m_CategoriaProdotto);
                ret.Remove(m_Rata);
                ret.Remove(m_NumeroRate);
                return ret;
            }

            protected override CKeyCollection<Databases.DBCursorField> GetSortFields()
            {
                var ret = new CKeyCollection<Databases.DBCursorField>(base.GetWhereFields());
                ret.Remove(m_IDCessionario);
                ret.Remove(m_NomeCessionario);
                ret.Remove(m_IDProfilo);
                ret.Remove(m_NomeProfilo);
                ret.Remove(m_IDProdotto);
                ret.Remove(m_NomeProdotto);
                ret.Remove(m_CategoriaProdotto);
                ret.Remove(m_Rata);
                ret.Remove(m_NumeroRate);
                return ret;
            }

            private bool IsOffertaSet()
            {
                return m_IDCessionario.IsSet() || m_NomeCessionario.IsSet() || m_IDProfilo.IsSet() || m_NomeProfilo.IsSet() || m_IDProdotto.IsSet() || m_NomeProdotto.IsSet() || m_CategoriaProdotto.IsSet() || m_Rata.IsSet() || m_NumeroRate.IsSet();
            }

            public override string GetWherePart()
            {
                string ret = base.GetWherePart();
                if (IsOffertaSet())
                {
                    var arr = DMD.Arrays.Empty<int>();
                    using (var cursor = new CCQSPDOfferteCursor())
                    {
                        cursor.IDCessionario.InitFrom(m_IDCessionario);
                        cursor.NomeCessionario.InitFrom(m_NomeCessionario);
                        cursor.IDProfilo.InitFrom(m_IDProfilo);
                        cursor.NomeProfilo.InitFrom(m_NomeProfilo);
                        cursor.ProdottoID.InitFrom(m_IDProdotto);
                        cursor.NomeProdotto.InitFrom(m_NomeProdotto);
                        cursor.CategoriaProdotto.InitFrom(m_CategoriaProdotto);
                        cursor.Rata.InitFrom(m_Rata);
                        cursor.Durata.InitFrom(m_NumeroRate);
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IgnoreRights = true;
                        
                        string dbSQL = "SELECT [ID] FROM (" + cursor.GetSQL() + ")";
                        using (var dbRis = cursor.Connection.ExecuteReader(dbSQL))
                        {
                            while (dbRis.Read())
                            {
                                int id = Sistema.Formats.ToInteger(dbRis["ID"]);
                                arr = DMD.Arrays.Append<int>(arr, id);
                            }

                        }

                    }

                    if (arr.Length == 0)
                    {
                        ret = "(0<>0)";
                    }
                    else
                    {
                        string strj = DMD.Strings.Join(arr, ",");
                        ret = DMD.Strings.Combine(ret, "([IDOffertaCQS] In (" + strj + ") OR [IDOffertaPD] In (" + strj + "))", " AND ");
                    }
                }

                return ret;
            }
        }
    }
}