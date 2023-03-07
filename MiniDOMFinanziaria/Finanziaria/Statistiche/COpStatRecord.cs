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

        [Serializable]
        public class COpStatRecord 
            : IDMDXMLSerializable
        {
            private int m_IDPuntoOperativo;
            private int m_IDOperatore;
            private DateTime? m_FromDate;
            private DateTime? m_ToDate;
            private bool m_OnlyValid;
            private bool m_Changed;
            private bool m_IgnoreRights;
            private CStatisticheOperazione m_OutCalls = new CStatisticheOperazione();
            private CStatisticheOperazione m_InCalls = new CStatisticheOperazione();
            private CStatisticheOperazione m_OutDates = new CStatisticheOperazione();
            private CStatisticheOperazione m_InDates = new CStatisticheOperazione();
            private CStatisticheOperazione m_VisiteDistinteRicevute = new CStatisticheOperazione();
            private int m_PraticheCDN;
            private decimal m_PraticheCDM;
            private int m_PersoneContattate;
            private CStatisticheOperazione m_Consulenze = new CStatisticheOperazione();
            private CStatisticheOperazione m_Richieste = new CStatisticheOperazione();
            private double m_DurataConsulenze;
            private int m_ConteggioConsulenzeVisiteEsterne;
            private int m_ConteggioConsulenzeVisiteUfficio;
            private int m_ConteggioConsulenzeTelefonateRicevute;
            private int m_ConteggioConsulenzeTelefonateEffettuate;
            private CStatisticheOperazione m_PraticheConsulente = new CStatisticheOperazione();
            private CStatisticheOperazione m_PraticheConcluseConsulente = new CStatisticheOperazione();
            private CStatisticheOperazione m_PraticheAnnullateConsulente = new CStatisticheOperazione();
            private CStatisticheOperazione m_PraticheConcluseConsulenza = new CStatisticheOperazione();
            private CStatisticheOperazione m_PraticheAnnullateConsulenza = new CStatisticheOperazione();
            private CKeyCollection<CStatisticheOperazione> m_Infos;
            private int[] m_ArrOperatori;
            private int[] m_ArrConsulenti;

            /// <summary>
            /// Costruttore
            /// </summary>
            public COpStatRecord()
            {
                DMDObject.IncreaseCounter(this);
                m_OnlyValid = true;
                m_Changed = false;
                m_IgnoreRights = false;
                m_IDOperatore = 0;
                m_IDPuntoOperativo = 0;
                m_FromDate = DMD.DateUtils.ToDay();
                m_ToDate = DMD.DateUtils.DateAdd("d", 1d, m_FromDate);
                Reset();
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue);  }
            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer); }

            public void Invalidate()
            {
                m_Changed = true;
                Reset();
            }

            public void Validate()
            {
                if (m_Changed == true)
                    Calculate();
            }

            public CStatisticheOperazione Stato(CStatoPratica s)
            {
                Validate();
                return m_Infos.GetItemByKey("K" + DBUtils.GetID(s));
            }

            public CStatisticheOperazione PraticheConcluseConsulenza
            {
                get
                {
                    return m_PraticheConcluseConsulenza;
                }
            }

            public CStatisticheOperazione PraticheAnnullateConsulenza
            {
                get
                {
                    return m_PraticheAnnullateConsulenza;
                }
            }

            public CStatisticheOperazione PraticheConsulente
            {
                get
                {
                    return m_PraticheConsulente;
                }
            }

            public CStatisticheOperazione PraticheConcluseConsulente
            {
                get
                {
                    return m_PraticheConcluseConsulente;
                }
            }

            public CStatisticheOperazione PraticheAnnullateConsulente
            {
                get
                {
                    return m_PraticheAnnullateConsulente;
                }
            }

            public CStatisticheOperazione Consulenze
            {
                get
                {
                    Validate();
                    return m_Consulenze;
                }
            }

            public CStatisticheOperazione RichiesteDiFinanziamento
            {
                get
                {
                    Validate();
                    return m_Richieste;
                }
            }



            /// <summary>
        /// Restituisce il numero totale di consulenze esterne
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int ConteggioConsulenzeVisiteEsterne
            {
                get
                {
                    Validate();
                    return m_ConteggioConsulenzeVisiteEsterne;
                }
            }

            /// <summary>
        /// Restituisce il numero totale di consulenze in ufficio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int ConteggioConsulenzeVisiteUfficio
            {
                get
                {
                    Validate();
                    return m_ConteggioConsulenzeVisiteUfficio;
                }
            }

            /// <summary>
        /// Restituisce il numero totale di consulenze effettuate durante una telefonata ricevuta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int ConteggioConsulenzeTelefonateRicevute
            {
                get
                {
                    Validate();
                    return m_ConteggioConsulenzeTelefonateRicevute;
                }
            }

            /// <summary>
        /// Restituisce il numero totale di consulenze effettuate durante una telefonata effettuata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int ConteggioConsulenzeTelefonateEffettuate
            {
                get
                {
                    Validate();
                    return m_ConteggioConsulenzeTelefonateEffettuate;
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se l'oggetto deve eseguire i calcoli come superuser
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool IgnoreRights
            {
                get
                {
                    return m_IgnoreRights;
                }

                set
                {
                    if (m_IgnoreRights == value)
                        return;
                    m_IgnoreRights = value;
                    Invalidate();
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'operatore di cui restituire le statistiche
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDOperatore
            {
                get
                {
                    return m_IDOperatore;
                }

                set
                {
                    if (m_IDOperatore == value)
                        return;
                    m_IDOperatore = value;
                    Invalidate();
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del punto operativo a cui restringere le statistiche.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Se IDOperatore è 0 l'oggetto effettua la somma di tutti gli operatori appartenenti al punto operativo. Se, invece, IDOperatore è non 0 e IDPuntoOperativo è non zero l'oggetto restringe le statistiche dell'operatore al solo punto operativo specificato</remarks>
            public int IDPuntoOperativo
            {
                get
                {
                    return m_IDPuntoOperativo;
                }

                set
                {
                    if (m_IDPuntoOperativo == value)
                        return;
                    m_IDPuntoOperativo = value;
                    Invalidate();
                }
            }

            /// <summary>
        /// Restituisce o imposta la data iniziale dell'intervallo considerato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? FromDate
            {
                get
                {
                    return m_FromDate;
                }

                set
                {
                    if (DMD.DateUtils.Compare(value, m_FromDate) == 0)
                        return;
                    m_FromDate = value;
                    Invalidate();
                }
            }

            /// <summary>
        /// Restituisce o imposta la data finale dell'intervallo considerato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? ToDate
            {
                get
                {
                    return m_ToDate;
                }

                set
                {
                    if (DMD.DateUtils.Compare(m_ToDate, value) == 0)
                        return;
                    m_ToDate = value;
                    Invalidate();
                }
            }

            /// <summary>
        /// Restituisce o imposta il periodo considerato
        /// </summary>
        /// <param name="value"></param>
        /// <remarks></remarks>
            public void SetPeriodo(string value)
            {
                var intervallo = DMD.DateUtils.PeriodoToDates(value, m_FromDate, m_ToDate);
                m_FromDate = intervallo.Inizio;
                m_ToDate = intervallo.Fine;
                Invalidate();
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se considerare i soli utenti validi (quando IDPuntoOperativo è non nullo e IDOperatore è nullo)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool OnlyValid
            {
                get
                {
                    return m_OnlyValid;
                }

                set
                {
                    if (m_OnlyValid == value)
                        return;
                    m_OnlyValid = value;
                    Invalidate();
                }
            }

            /// <summary>
        /// Restituisce il numero di persone fisiche (diverse) contattate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int PersoneContattate
            {
                get
                {
                    Validate();
                    return m_PersoneContattate;
                }
            }

            public CStatisticheOperazione InCalls
            {
                get
                {
                    Validate();
                    return m_InCalls;
                }
            }

            public CStatisticheOperazione OutCalls
            {
                get
                {
                    Validate();
                    return m_OutCalls;
                }
            }

            public CStatisticheOperazione OutDates
            {
                get
                {
                    Validate();
                    return m_OutDates;
                }
            }

            public CStatisticheOperazione InDates
            {
                get
                {
                    Validate();
                    return m_InDates;
                }
            }

            public CStatisticheOperazione VisiteDistinteRicevute
            {
                get
                {
                    Validate();
                    return m_VisiteDistinteRicevute;
                }
            }



            /// <summary>
        /// Restituisce il numero di pratiche in cui la fonte è impostata come 'Contatto Diretto' e l'operatore appartiene al gruppo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int PraticheContattoDirettoN
            {
                get
                {
                    Validate();
                    return m_PraticheCDN;
                }
            }

            /// <summary>
        /// Restituisce il montante lordo delle pratiche in cui la fonte è impostata come 'Contatto diretto' e l'operatore appartiene al gruppo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal PraticheContattoDirettoM
            {
                get
                {
                    Validate();
                    return m_PraticheCDM;
                }
            }


            /// <summary>
        /// Calcola
        /// </summary>
        /// <remarks></remarks>
            public void Calculate()
            {
                m_Changed = false;
                Reset();
                if (IDOperatore == 0)
                {
                    AddData(GetUtenti());
                }
                else
                {
                    AddData(new int[] { IDOperatore });
                }
            }

            private void Reset()
            {
                m_OutCalls = new CStatisticheOperazione();
                m_InCalls = new CStatisticheOperazione();
                m_OutDates = new CStatisticheOperazione();
                m_InDates = new CStatisticheOperazione();
                m_VisiteDistinteRicevute = new CStatisticheOperazione();
                m_PraticheConsulente = new CStatisticheOperazione();
                m_PraticheConcluseConsulente = new CStatisticheOperazione();
                m_PraticheAnnullateConsulente = new CStatisticheOperazione();
                m_PraticheConcluseConsulenza = new CStatisticheOperazione();
                m_PraticheAnnullateConsulenza = new CStatisticheOperazione();
                m_Richieste = new CStatisticheOperazione();
                m_PraticheCDM = 0m;
                m_PraticheCDN = 0;
                m_PersoneContattate = 0;
                m_Consulenze = new CStatisticheOperazione();
                m_DurataConsulenze = 0d;
                m_ConteggioConsulenzeTelefonateEffettuate = 0;
                m_ConteggioConsulenzeTelefonateRicevute = 0;
                m_ConteggioConsulenzeVisiteEsterne = 0;
                m_ConteggioConsulenzeVisiteUfficio = 0;
                m_PraticheCDM = 0m;
                m_PraticheCDN = 0;
                m_PersoneContattate = 0;
                m_DurataConsulenze = 0d;
                m_ConteggioConsulenzeVisiteEsterne = 0;
                m_ConteggioConsulenzeVisiteUfficio = 0;
                m_ConteggioConsulenzeTelefonateRicevute = 0;
                m_ConteggioConsulenzeTelefonateEffettuate = 0;
                m_Infos = new CKeyCollection<CStatisticheOperazione>();
            }

            private int[] GetUtenti()
            {
                var ret = new ArrayList();
                Sistema.CUser user;
                int i;
                if (m_IDPuntoOperativo == 0)
                {
                    foreach (var currentUser in Sistema.Users.LoadAll())
                    {
                        user = currentUser;
                        if (user.Visible && (OnlyValid == false || user.UserStato == Sistema.UserStatus.USER_ENABLED))
                        {
                            ret.Add(DBUtils.GetID(user));
                        }
                    }
                }
                else
                {
                    var ufficio = Anagrafica.Uffici.GetItemById(m_IDPuntoOperativo);
                    var loopTo = ufficio.Utenti.Count - 1;
                    for (i = 0; i <= loopTo; i++)
                    {
                        user = ufficio.Utenti[i];
                        if (user is object && user.Visible && (OnlyValid == false || user.UserStato == Sistema.UserStatus.USER_ENABLED))
                        {
                            ret.Add(DBUtils.GetID(user));
                        }
                    }
                }

                return (int[])ret.ToArray(typeof(int));
            }

            private void AddData(int[] operatori)
            {
                m_ArrOperatori = operatori;
                m_ArrConsulenti = null;
                var idConsulenti = new ArrayList();
                
                string dbSQL;
                string opArray = DBUtils.MakeArrayStr(operatori);
                CStatisticheOperazione telStats;
                var t = DMD.DateUtils.Now();
                telStats = CustomerCalls.Telefonate.GetOutCallsStats(m_IDPuntoOperativo, operatori, m_FromDate, m_ToDate, m_IgnoreRights);
                Debug.Print("ContaTelefonateEffettuatePerData: " + (DMD.DateUtils.Now() - t));
                m_OutCalls.Numero += telStats.Numero;
                m_OutCalls.TotalLen += telStats.TotalLen;
                m_OutCalls.TotalWait += telStats.TotalWait;
                m_OutCalls.MinWait = (m_OutCalls.Numero > 0)? Maths.Min(m_OutCalls.MinWait , telStats.MinWait) : telStats.MinWait;
                m_OutCalls.MinLen = Maths.Min(m_OutCalls.MinLen, telStats.MinLen);
                m_OutCalls.MaxLen = Maths.Max(m_OutCalls.MaxLen, telStats.MaxLen);
                t = DMD.DateUtils.Now();
                telStats = CustomerCalls.Telefonate.GetInCallStats(m_IDPuntoOperativo, operatori, m_FromDate, m_ToDate, m_IgnoreRights);
                Debug.Print("ContaTelefonateRicevutePerData: " + (DMD.DateUtils.Now() - t));
                m_InCalls.Numero += telStats.Numero;
                m_InCalls.TotalLen += telStats.TotalLen;
                m_InCalls.TotalWait += telStats.TotalWait;
                m_InCalls.MinLen = Maths.Min(m_InCalls.MinLen, telStats.MinLen);
                m_InCalls.MaxLen = Maths.Max(m_InCalls.MaxLen, telStats.MaxLen);
                t = DMD.DateUtils.Now();
                // Me.m_PersoneContattate += CustomerCalls.Telefonate.ContaPersoneContattate(Me.m_IDPuntoOperativo, operatori, Me.m_FromDate, Me.m_ToDate, Me.m_IgnoreRights)


                telStats = Visite.GetOutVisitsStats(m_IDPuntoOperativo, operatori, m_FromDate, m_ToDate, m_IgnoreRights);
                m_OutDates.Numero += telStats.Numero;
                m_OutDates.TotalLen += telStats.TotalLen;
                m_OutDates.TotalWait += telStats.TotalWait;
                m_OutDates.MinLen = Maths.Min(m_OutDates.MinLen, telStats.MinLen);
                m_OutDates.MaxLen = Maths.Max(m_OutDates.MaxLen, telStats.MaxLen);
                telStats = Visite.GetInVisitsStats(m_IDPuntoOperativo, operatori, m_FromDate, m_ToDate, m_IgnoreRights);
                m_InDates.Numero += telStats.Numero;
                m_InDates.TotalLen += telStats.TotalLen;
                m_InDates.TotalWait += telStats.TotalWait;
                m_InDates.MinLen = Maths.Min(m_InDates.MinLen, telStats.MinLen);
                m_InDates.MaxLen = Maths.Max(m_InDates.MaxLen, telStats.MaxLen);


                // 'Pratiche come consulente
                if (operatori.Length == 0)
                    idConsulenti.Add(0);
                for (int i = 0, loopTo = DMD.Arrays.UBound(operatori); i <= loopTo; i++)
                {
                    var consulente = Consulenti.GetItemByUser(operatori[i]);
                    if (DBUtils.GetID(consulente) != 0)
                        idConsulenti.Add(DBUtils.GetID(consulente));
                }

                if (idConsulenti.Count > 0)
                {
                    m_ArrConsulenti = (int[])idConsulenti.ToArray(typeof(int));
                    string opConsulente = DBUtils.MakeArrayStr(m_ArrConsulenti);

                    // Contiamo le consulenze
                    t = DMD.DateUtils.Now();
                    dbSQL = "SELECT Count(*) As [Num], Sum([tbl_CQSPDConsulenze].[Durata]) As [DurataTotale] FROM [tbl_CQSPDConsulenze] WHERE [IDConsulente] In (" + opConsulente + ") And [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
                    if (m_FromDate.HasValue) dbSQL += " AND [DataConsulenza]>=" + DBUtils.DBDate(m_FromDate);
                    if (m_ToDate.HasValue) dbSQL += " And [DataConsulenza]<" + DBUtils.DBDate(m_ToDate);
                    using (var dbRis = Database.ExecuteReader(dbSQL))
                    {
                        if (dbRis.Read())
                        {
                            m_Consulenze.Numero += Sistema.Formats.ToInteger(dbRis["Num"]);
                            m_Consulenze.TotalLen += Sistema.Formats.ToDouble(dbRis["DurataTotale"]);
                        }

                    }
                    Debug.Print("Pratiche come consulente: " + (DMD.DateUtils.Now() - t));


                    // Contiamo le pratiche concluse 
                    using (var cursor = new CPraticheCQSPDCursor())
                    {

                        cursor.IgnoreRights = true;
                        cursor.Fields.Clear();
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IDStatoAttuale.ValueIn(new object[] { DBUtils.GetID(StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA)), DBUtils.GetID(StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA)) });
                        cursor.IDConsulente.ValueIn(m_ArrConsulenti);
                        cursor.StatoGenerico.Clear();
                        cursor.StatoGenerico.IDToStato = DBUtils.GetID(StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_PREVENTIVO));
                        cursor.StatoGenerico.Inizio = m_FromDate;
                        cursor.StatoGenerico.Fine = m_ToDate;
                        cursor.Flags.Value = PraticaFlags.HIDDEN;
                        cursor.Flags.Operator = Databases.OP.OP_NE;
                        cursor.Flags.IncludeNulls = true;

                        dbSQL = "SELECT Count(*) As [Num], Sum([MontanteLordo]) As [ML] FROM (" + cursor.GetSQL() + ")";
                        using (var dbRis = Database.ExecuteReader(dbSQL))
                        {
                            if (dbRis.Read())
                            {
                                m_PraticheConcluseConsulenza.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
                                m_PraticheConcluseConsulenza.Valore = Sistema.Formats.ToValuta(dbRis["ML"]);
                            }
                        }

                        cursor.Reset1();

                        cursor.Fields.Clear();
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IDStatoAttuale.ValueIn(new object[] { DBUtils.GetID(StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)) });
                        cursor.StatoPreventivo.Clear();
                        cursor.StatoPreventivo.IDToStato = DBUtils.GetID(StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_PREVENTIVO));
                        cursor.StatoPreventivo.Inizio = m_FromDate;
                        cursor.StatoPreventivo.Fine = m_ToDate;
                        cursor.Flags.Value = PraticaFlags.HIDDEN;
                        cursor.Flags.Operator = Databases.OP.OP_NE;
                        cursor.Flags.IncludeNulls = true;

                        dbSQL = "SELECT Count(*) As [Num], Sum([MontanteLordo]) As [ML] FROM (" + cursor.GetSQL() + ")";
                        using (var dbRis = Database.ExecuteReader(dbSQL))
                        {
                            if (dbRis.Read())
                            {
                                m_PraticheAnnullateConsulenza.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
                                m_PraticheAnnullateConsulenza.Valore = Sistema.Formats.ToValuta(dbRis["ML"]);
                            }
                        }

                        cursor.Reset1();

                        // ------------------------
                        cursor.Fields.Clear();
                        cursor.WhereClauses.Clear();
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IDStatoAttuale.Clear();
                        // cursor.IDProduttore.Value = GetID(Anagrafica.Aziende.AziendaPrincipale)
                        cursor.IDConsulente.ValueIn(m_ArrConsulenti);
                        cursor.StatoPreventivo.Clear();
                        cursor.StatoPreventivo.IDToStato = DBUtils.GetID(StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_PREVENTIVO));
                        cursor.StatoPreventivo.Inizio = m_FromDate;
                        cursor.StatoPreventivo.Fine = m_ToDate;
                        cursor.Flags.Value = PraticaFlags.HIDDEN;
                        cursor.Flags.Operator = Databases.OP.OP_NE;
                        cursor.Flags.IncludeNulls = true;
                        dbSQL = "SELECT Count(*) As [Num], Sum([MontanteLordo]) As [ML] FROM (" + cursor.GetSQL() + ")";

                        using (var dbRis = Database.ExecuteReader(dbSQL))
                        {
                            if (dbRis.Read())
                            {
                                m_PraticheConsulente.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
                                m_PraticheConsulente.Valore = Sistema.Formats.ToValuta(dbRis["ML"]);
                            }
                        }

                        cursor.Reset1();

                        cursor.Fields.Clear();
                        cursor.WhereClauses.Clear();
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IDStatoAttuale.ValueIn(new object[] { DBUtils.GetID(StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA)), DBUtils.GetID(StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA)) });
                        // cursor.IDProduttore.Value = GetID(Anagrafica.Aziende.AziendaPrincipale)
                        cursor.IDConsulente.ValueIn(m_ArrConsulenti);
                        cursor.StatoPreventivo.Clear();
                        cursor.StatoPreventivo.IDToStato = DBUtils.GetID(StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_PREVENTIVO));
                        cursor.StatoPreventivo.Inizio = m_FromDate;
                        cursor.StatoPreventivo.Fine = m_ToDate;
                        cursor.Flags.Value = PraticaFlags.HIDDEN;
                        cursor.Flags.Operator = Databases.OP.OP_NE;
                        cursor.Flags.IncludeNulls = true;
                        dbSQL = "SELECT Count(*) As [Num], Sum([MontanteLordo]) As [ML] FROM (" + cursor.GetSQL() + ")";
                        using (var dbRis = Database.ExecuteReader(dbSQL))
                        {
                            if (dbRis.Read())
                            {
                                m_PraticheConcluseConsulente.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
                                m_PraticheConcluseConsulente.Valore = Sistema.Formats.ToValuta(dbRis["ML"]);
                            }
                        }
                        cursor.Reset1();
                        cursor.Fields.Clear();
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IDStatoAttuale.ValueIn(new object[] { DBUtils.GetID(StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)) });
                        // cursor.IDProduttore.Value = GetID(Anagrafica.Aziende.AziendaPrincipale)
                        cursor.IDConsulente.ValueIn(m_ArrConsulenti);
                        cursor.StatoPreventivo.Clear();
                        cursor.StatoPreventivo.IDToStato = DBUtils.GetID(StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_PREVENTIVO));
                        cursor.StatoPreventivo.Inizio = m_FromDate;
                        cursor.StatoPreventivo.Fine = m_ToDate;
                        cursor.Flags.Value = PraticaFlags.HIDDEN;
                        cursor.Flags.Operator = Databases.OP.OP_NE;
                        cursor.Flags.IncludeNulls = true;
                        dbSQL = "SELECT Count(*) As [Num], Sum([MontanteLordo]) As [ML] FROM (" + cursor.GetSQL() + ")";
                        using (var dbRis = Database.ExecuteReader(dbSQL))
                        {
                            if (dbRis.Read())
                            {
                                m_PraticheAnnullateConsulente.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
                                m_PraticheAnnullateConsulente.Valore = Sistema.Formats.ToValuta(dbRis["ML"]);
                            }
                        }
                    }
                }

                // Richieste registrate
                dbSQL = "SELECT Count(*) As [Num], Sum([Durata]) As [Durata] FROM [tbl_RichiesteFinanziamenti] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND  [IDPresaInCaricoDa] In (" + opArray + ") ";
                if (m_FromDate.HasValue)
                    dbSQL += " AND [Data]>=" + DBUtils.DBDate(m_FromDate);
                if (m_ToDate.HasValue)
                    dbSQL += " And [Data]<" + DBUtils.DBDate(m_ToDate);

                using (var dbRis = Database.ExecuteReader(dbSQL))
                {
                    if (dbRis.Read())
                    {
                        m_Richieste.Numero = Sistema.Formats.ToInteger(dbRis["Num"]);
                        m_Richieste.TotalLen = Sistema.Formats.ToInteger(dbRis["Durata"]);
                    }
                }

                // Pratiche inserite 
                foreach (CStatoPratica s in StatiPratica.GetSequenzaStandard())
                {
                    CStatisticheOperazione info;
                    // dbSQL = "SELECT Count(*) As [Num], Sum([tbl_Pratiche].[MontanteLordo]) As [ML] FROM [tbl_Pratiche] INNER JOIN [tbl_PraticheSTL] ON [tbl_Pratiche].[ID]=[tbl_PraticheSTL].[IDPratica] WHERE [tbl_Pratiche].[Stato]=" & ObjectStatus.OBJECT_VALID & " AND (([tbl_Pratiche].[Flags] AND " & PraticaFlags.HIDDEN & ")<>" & PraticaFlags.HIDDEN & ")  AND ([tbl_PraticheSTL].[IDOperatore] In (" & opArray & ")) "
                    // If (Me.m_FromDate.HasValue) Then dbSQL &= " AND [tbl_PraticheSTL].[Data]>=" & DBUtils.DBDate(Me.m_FromDate)
                    // If (Me.m_ToDate.HasValue) Then dbSQL &= " And [tbl_PraticheSTL].[Data]<" & DBUtils.DBDate(Me.m_ToDate)
                    // dbSQL &= " AND [tbl_PraticheSTL].[IDToStato] = " & GetID(s)
                    // For Each opid In operatori
                    using (var cursor = new CPraticheCQSPDCursor())
                    {
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        if (IDPuntoOperativo != 0)
                            cursor.IDPuntoOperativo.Value = IDPuntoOperativo;
                        cursor.StatoGenerico.IDToStato = DBUtils.GetID(s);
                        cursor.StatoGenerico.Inizio = m_FromDate;
                        cursor.StatoGenerico.Fine = m_ToDate;
                        if (IDOperatore != 0)
                            cursor.StatoPreventivo.IDOperatore = IDOperatore;
                        dbSQL = "SELECT Count(*) As [Num], Sum([tbl_Pratiche].[MontanteLordo]) As [ML] FROM (" + cursor.GetSQL() + ")";
                    }

                    using (var dbRis = Database.ExecuteReader(dbSQL))
                    {
                        if (dbRis.Read())
                        {
                            info = m_Infos.GetItemByKey("K" + DBUtils.GetID(s));
                            if (info is null)
                            {
                                info = new CStatisticheOperazione();
                                info.Numero = 0;
                                info.Valore = 0;
                                m_Infos.Add("K" + DBUtils.GetID(s), info);
                            }
                            // info.Stato = s
                            info.Numero += Sistema.Formats.ToInteger(dbRis["Num"]);
                            info.Valore += Sistema.Formats.ToValuta(dbRis["ML"]);
                        }
                    }

                }

                Debug.Print("Pratiche inserite: " + (DMD.DateUtils.Now() - t));
            }

            public int[] GetArrayOperatori()
            {
                Validate();
                return m_ArrOperatori;
            }

            public int[] GetArrayConsulenti()
            {
                Validate();
                return m_ArrConsulenti;
            }

            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "PO":
                        {
                            m_IDPuntoOperativo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "OP":
                        {
                            m_IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DI":
                        {
                            m_FromDate = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "TD":
                        {
                            m_ToDate = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "OV":
                        {
                            m_OnlyValid = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "CH":
                        {
                            m_Changed = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "IR":
                        {
                            m_IgnoreRights = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "PraticheCDN":
                        {
                            m_PraticheCDN = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "PraticheCDM":
                        {
                            m_PraticheCDM = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "PersoneContattate":
                        {
                            m_PersoneContattate = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DurataConsulenze":
                        {
                            m_DurataConsulenze = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ConteggioConsulenzeVisiteEsterne":
                        {
                            m_ConteggioConsulenzeVisiteEsterne = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ConteggioConsulenzeVisiteUfficio":
                        {
                            m_ConteggioConsulenzeVisiteUfficio = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ConteggioConsulenzeTelefonateRicevute":
                        {
                            m_ConteggioConsulenzeTelefonateRicevute = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ConteggioConsulenzeTelefonateEffettuate":
                        {
                            m_ConteggioConsulenzeTelefonateEffettuate = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "OutCalls":
                        {
                            this.m_OutCalls = DMD.XML.Utils.Serializer.ToObject(fieldValue, this.m_OutCalls);
                            break;
                        }

                    case "InCalls":
                        {
                            m_InCalls = DMD.XML.Utils.Serializer.ToObject(fieldValue, this.m_InCalls);
                            break;
                        }

                    case "OutDates":
                        {
                            this.m_OutDates = DMD.XML.Utils.Serializer.ToObject(fieldValue, this.m_OutDates);
                            break;
                        }

                    case "InDates":
                        {
                            this.m_InDates = DMD.XML.Utils.Serializer.ToObject(fieldValue, this.m_InDates);
                            break;
                        }

                    case "VisiteDistinteRicevute":
                        {
                            m_VisiteDistinteRicevute = DMD.XML.Utils.Serializer.ToObject(fieldValue, this.m_VisiteDistinteRicevute);
                            break;
                        }

                    case "Consulenze":
                        {
                            m_Consulenze = DMD.XML.Utils.Serializer.ToObject(fieldValue, this.m_Consulenze);
                            break;
                        }

                    case "Richieste":
                        {
                            m_Richieste = DMD.XML.Utils.Serializer.ToObject(fieldValue, this.m_Richieste);
                            break;
                        }

                    case "PraticheConsulente":
                        {
                            m_PraticheConsulente = DMD.XML.Utils.Serializer.ToObject(fieldValue, this.m_PraticheConsulente);
                            break;
                        }

                    case "PraticheConcluseConsulente":
                        {
                            m_PraticheConcluseConsulente = DMD.XML.Utils.Serializer.ToObject(fieldValue, this.m_PraticheConcluseConsulente);
                            break;
                        }

                    case "PraticheAnnullateConsulente":
                        {
                            m_PraticheAnnullateConsulente = DMD.XML.Utils.Serializer.ToObject(fieldValue, this.m_PraticheAnnullateConsulente);
                            break;
                        }

                    case "PraticheConcluseConsulenza":
                        {
                            m_PraticheConcluseConsulenza = DMD.XML.Utils.Serializer.ToObject(fieldValue, this.m_PraticheConcluseConsulenza);
                            break;
                        }

                    case "PraticheAnnullateConsulenza":
                        {
                            m_PraticheAnnullateConsulenza = DMD.XML.Utils.Serializer.ToObject(fieldValue, this.m_PraticheAnnullateConsulenza);
                            break;
                        }

                    case "Infos":
                        {
                            m_Infos = (CKeyCollection<CStatisticheOperazione>)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            break;
                        }

                    case "ArrOperatori":
                        {
                            m_ArrOperatori = (int[])DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            break;
                        }

                    case "ArrConsulenti":
                        {
                            m_ArrConsulenti = (int[])DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            break;
                        }
                }
            }

            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("PO", IDPuntoOperativo);
                writer.WriteAttribute("OP", IDOperatore);
                writer.WriteAttribute("DI", m_FromDate);
                writer.WriteAttribute("TD", m_ToDate);
                writer.WriteAttribute("OV", m_OnlyValid);
                writer.WriteAttribute("CH", m_Changed);
                writer.WriteAttribute("IR", m_IgnoreRights);
                writer.WriteAttribute("PraticheCDN", m_PraticheCDN);
                writer.WriteAttribute("PraticheCDM", m_PraticheCDM);
                writer.WriteAttribute("PersoneContattate", m_PersoneContattate);
                writer.WriteAttribute("DurataConsulenze", m_DurataConsulenze);
                writer.WriteAttribute("ConteggioConsulenzeVisiteEsterne", m_ConteggioConsulenzeVisiteEsterne);
                writer.WriteAttribute("ConteggioConsulenzeVisiteUfficio", m_ConteggioConsulenzeVisiteUfficio);
                writer.WriteAttribute("ConteggioConsulenzeTelefonateRicevute", m_ConteggioConsulenzeTelefonateRicevute);
                writer.WriteAttribute("ConteggioConsulenzeTelefonateEffettuate", m_ConteggioConsulenzeTelefonateEffettuate);
                writer.WriteTag("OutCalls", m_OutCalls);
                writer.WriteTag("InCalls", m_InCalls);
                writer.WriteTag("OutDates", m_OutDates);
                writer.WriteTag("InDates", m_InDates);
                writer.WriteTag("VisiteDistinteRicevute", m_VisiteDistinteRicevute);
                writer.WriteTag("Consulenze", m_Consulenze);
                writer.WriteTag("Richieste", m_Richieste);
                writer.WriteTag("PraticheConsulente", m_PraticheConsulente);
                writer.WriteTag("PraticheConcluseConsulente", m_PraticheConcluseConsulente);
                writer.WriteTag("PraticheAnnullateConsulente", m_PraticheAnnullateConsulente);
                writer.WriteTag("PraticheConcluseConsulenza", m_PraticheConcluseConsulenza);
                writer.WriteTag("PraticheAnnullateConsulenza", m_PraticheAnnullateConsulenza);
                writer.WriteTag("Infos", m_Infos);
                writer.WriteTag("ArrOperatori", m_ArrOperatori);
                writer.WriteTag("ArrConsulenti", m_ArrConsulenti);
            }

            ~COpStatRecord()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}