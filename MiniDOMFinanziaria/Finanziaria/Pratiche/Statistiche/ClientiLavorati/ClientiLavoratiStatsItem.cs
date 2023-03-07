using System;
using System.Collections;
using System.Data;
using System.Linq;
using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using static minidom.CustomerCalls;

namespace minidom
{
    public partial class Finanziaria
    {
        public enum StatoLavorazione : int
        {
            /// <summary>
        /// Non è definito uno stato per questa persona
        /// </summary>
        /// <remarks></remarks>
            None = 0,

            /// <summary>
        /// L'utente è stato contattato ed ha risposto almeno una volta
        /// </summary>
        /// <remarks></remarks>
            InContatto = 2,

            /// <summary>
        /// Il cliente è interessato ad una operazione
        /// </summary>
        /// <remarks></remarks>
            Interessato = 3,

            /// <summary>
        /// Il cliente non è interessato ad alcuna operazione
        /// </summary>
        /// <remarks></remarks>
            NonInteressato = 4,


            /// <summary>
        /// La persona è stata visitata o è venuta in ufficio almeno una volta
        /// </summary>
        /// <remarks></remarks>
            Visita = 5,
            /// <summary>
        /// Il cliente ha richiesto un conteggio estintivo
        /// </summary>
        /// <remarks></remarks>
            RichiestaConteggioEstintivo = 6,

            /// <summary>
        /// La persona ha fatto almeno una richiesta di finanziamento
        /// </summary>
        /// <remarks></remarks>
            RichiestaFinanziamento = 7,

            /// <summary>
        /// La persona ha inviato la busta paga
        /// </summary>
        /// <remarks></remarks>
            BustaPaga = 8,

            /// <summary>
        /// E' stato fatto almeno uno studio di fattibilità
        /// </summary>
        /// <remarks></remarks>
            StudioDiFattibilita = 9,

            /// <summary>
        /// E' stata inserita almeno una pratica
        /// </summary>
        /// <remarks></remarks>
            Pratica = 10
        }

        public enum SottostatoLavorazione : int
        {
            None = 0,
            InAttesa = 1,
            InLavorazione = 2,
            NonFattibile = 3,
            RifiutatoDalCliente = 4,
            BocciatoAgenzia = 5,
            BocciatoIstituto = 6,
            Completato = 7
        }

        [Serializable]
        public class ClientiLavoratiStatsItem : Databases.DBObjectPO
        {
            private int m_IDCliente;
            private string m_NomeCliente;
            private Anagrafica.CPersona m_Cliente;
            private string m_IconaCliente;
            private StatoLavorazione m_StatoLavorazione;
            private SottostatoLavorazione m_SottostatoLavorazione;
            private DateTime? m_DataUltimoAggiornamento;
            private DateTime? m_DataInizioLavorazione;
            private DateTime? m_DataFineLavorazione;
            private int m_IDOperatore;
            private Sistema.CUser m_Operatore;
            private string m_NomeOperatore;
            private int m_Flags;
            private int[] m_Opeatori;
            private int[] m_Visite;
            private int[] m_RichiesteConteggi;
            private int[] m_RichiesteFinanziamenti;
            private int[] m_BustePaga;
            private int[] m_OfferteInserite;
            private int[] m_OfferteProposte;
            private int[] m_OfferteRifiutateCliente;
            private int[] m_OfferteBocciate;
            private int[] m_OfferteNonFattibili;
            private int[] m_PraticheInCorso;
            private int[] m_PraticheLiquidate;
            private int[] m_PraticheRifiutateCliente;
            private int[] m_PraticheBocciate;
            private int[] m_PraticheNonFattibili;

            public ClientiLavoratiStatsItem()
            {
                m_IDCliente = 0;
                m_NomeCliente = "";
                m_IconaCliente = "";
                m_Cliente = null;
                m_StatoLavorazione = StatoLavorazione.None;
                m_SottostatoLavorazione = SottostatoLavorazione.None;
                m_DataUltimoAggiornamento = default;
                m_DataInizioLavorazione = default;
                m_DataFineLavorazione = default;
                // Me.m_NumeroBustePaga = 0
                // Me.m_NumeroVisite = 0
                // Me.m_NumeroVisiteConsulenza = 0
                // Me.m_NumeroRichiesteFinanziamento = 0
                // Me.m_NumeroRichiesteConteggiEstintivi = 0
                // Me.m_NumeroStudiDiFattibilita = 0
                // Me.m_NumeroOfferteProposte = 0
                // Me.m_NumeroOfferteAccettate = 0
                // Me.m_NumeroOfferteRifiutate = 0
                // Me.m_NumeroOfferteNonFattibili = 0
                // Me.m_NumeroOfferteBocciate = 0
                // Me.m_NumeroPratiche = 0
                // Me.m_NumeroPraticheLiquidate = 0
                // Me.m_NumeroPraticheAnnullate = 0
                // Me.m_NumeroPraticheRifiutate = 0
                // Me.m_NumeroPraticheNonFattibili = 0
                // Me.m_NumeroPraticheBocciate = 0

                m_Flags = 0;
                m_Opeatori = null;
                m_Visite = null;
                m_RichiesteConteggi = null;
                m_RichiesteFinanziamenti = null;
                m_OfferteInserite = null;
                m_OfferteProposte = null;
                m_OfferteRifiutateCliente = null;
                m_OfferteBocciate = null;
                m_OfferteNonFattibili = null;
                m_PraticheInCorso = null;
                m_PraticheLiquidate = null;
                m_PraticheRifiutateCliente = null;
                m_PraticheBocciate = null;
                m_PraticheNonFattibili = null;
            }

            /// <summary>
        /// Restituisce o i imposta dei flags aggiuntivi
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    int oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce la collezione di tutti gli operatori che hanno avuto contatti con il cliente durante il periodo di lavorazione
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<Sistema.CUser> GetOperatori()
            {
                var ret = new CCollection<Sistema.CUser>();
                int len = DMD.Arrays.Len(m_Opeatori);
                for (int i = 0, loopTo = len - 1; i <= loopTo; i++)
                {
                    var user = Sistema.Users.GetItemById(m_Opeatori[i]);
                    if (user is object && user.Stato == ObjectStatus.OBJECT_VALID)
                        ret.Add(user);
                }

                return ret;
            }

            /// <summary>
        /// Restituisce o il numero di buste paga caricate per il cliente durante il periodo di lavorazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroBustePaga
            {
                get
                {
                    return DMD.Arrays.Len(m_BustePaga);
                }
            }

            /// <summary>
        /// Restituisce la collezione delle buste paga caricate
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<Sistema.CAttachment> GetBustePaga()
            {
                var ret = new CCollection<Sistema.CAttachment>();
                int len = NumeroBustePaga;
                for (int i = 0, loopTo = len - 1; i <= loopTo; i++)
                {
                    var a = Sistema.Attachments.GetItemById(m_BustePaga[i]);
                    if (a is object && a.Stato == ObjectStatus.OBJECT_VALID)
                        ret.Add(a);
                }

                return ret;
            }

            /// <summary>
        /// Restituisce  il numero totale delle visite ricevute o presso il cliente durante il periodo di lavorazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroVisite
            {
                get
                {
                    return DMD.Arrays.Len(m_Visite);
                }
            }

            /// <summary>
        /// Restituisce la collezione delle visite
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<CVisita> GetVisite()
            {
                var ret = new CCollection<CVisita>();
                int len = NumeroVisite;
                for (int i = 0, loopTo = len - 1; i <= loopTo; i++)
                {
                    CVisita visita = CustomerCalls.Visite.GetItemById(m_Visite[i]);
                    if (visita is object && visita.Stato == ObjectStatus.OBJECT_VALID)
                        ret.Add(visita);
                }

                return ret;
            }

            // ''' <summary>
            // ''' Restituisce  il numero totale di visite con scopo consulenza durante il periodo di lavorazione
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Property NumeroVisiteConsulenza As Integer
            // Get
            // Return Me.m_NumeroVisiteConsulenza
            // End Get
            // Set(value As Integer)
            // Dim oldValue As Integer = Me.m_NumeroVisiteConsulenza
            // If (oldValue = value) Then Exit Property
            // Me.m_NumeroVisiteConsulenza = value
            // Me.DoChanged("NumeroVisiteConsulenza", value, oldValue)
            // End Set
            // End Property

            /// <summary>
        /// Restituisce il numero di richieste di finanziamento registrate per il cliente durante il periodo di lavorazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroRichiesteFinanziamento
            {
                get
                {
                    return DMD.Arrays.Len(m_RichiesteFinanziamenti);
                }
            }

            /// <summary>
        /// Restituisce la collezione delle richieste di finanziamento
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<CRichiestaFinanziamento> GetRichiesteFinanziamenti()
            {
                var ret = new CCollection<CRichiestaFinanziamento>();
                int len = NumeroRichiesteFinanziamento;
                for (int i = 0, loopTo = len - 1; i <= loopTo; i++)
                {
                    var richiesta = RichiesteFinanziamento.GetItemById(m_RichiesteFinanziamenti[i]);
                    if (richiesta is object && richiesta.Stato == ObjectStatus.OBJECT_VALID)
                        ret.Add(richiesta);
                }

                return ret;
            }

            /// <summary>
        /// Restituisce il numero di richieste di conteggi estintivi fatti dal cliente durante il periodo di lavorazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroRichiesteConteggiEstintivi
            {
                get
                {
                    return DMD.Arrays.Len(m_RichiesteConteggi);
                }
            }

            public CCollection<CRichiestaConteggio> GetRichiesteConteggiEstintivi()
            {
                var ret = new CCollection<CRichiestaConteggio>();
                int len = NumeroRichiesteConteggiEstintivi;
                for (int i = 0, loopTo = len - 1; i <= loopTo; i++)
                {
                    var richiesta = RichiesteConteggi.GetItemById(m_RichiesteConteggi[i]);
                    if (richiesta is object && richiesta.Stato == ObjectStatus.OBJECT_VALID)
                        ret.Add(richiesta);
                }

                return ret;
            }

            /// <summary>
        /// Restituisce il numero di offerte che sono solo state inserite
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroOfferteInserite
            {
                get
                {
                    return DMD.Arrays.Len(m_OfferteInserite);
                }
            }

            public CCollection<CQSPDConsulenza> GetOfferteInserite()
            {
                var ret = new CCollection<CQSPDConsulenza>();
                int len = NumeroOfferteInserite;
                for (int i = 0, loopTo = len - 1; i <= loopTo; i++)
                {
                    var offerta = Consulenze.GetItemById(m_OfferteInserite[i]);
                    if (offerta is object && offerta.Stato == ObjectStatus.OBJECT_VALID)
                        ret.Add(offerta);
                }

                return ret;
            }

            /// <summary>
        /// Restituisce  il numero totale di studi di fattibilità proposti al cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroOfferteProposte
            {
                get
                {
                    return DMD.Arrays.Len(m_OfferteProposte);
                }
            }

            public CCollection<CQSPDConsulenza> GetOfferteProposte()
            {
                var ret = new CCollection<CQSPDConsulenza>();
                int len = NumeroOfferteProposte;
                for (int i = 0, loopTo = len - 1; i <= loopTo; i++)
                {
                    var offerta = Consulenze.GetItemById(m_OfferteProposte[i]);
                    if (offerta is object)
                        ret.Add(offerta);
                }

                return ret;
            }


            /// <summary>
        /// Restituisce o imposta il numero di offerte proposte al cliente e da esso rifiutate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroOfferteRifiutateCliente
            {
                get
                {
                    return DMD.Arrays.Len(m_OfferteRifiutateCliente);
                }
            }

            public CCollection<CQSPDConsulenza> GetOfferteRifiutateCliente()
            {
                var ret = new CCollection<CQSPDConsulenza>();
                int len = NumeroOfferteRifiutateCliente;
                for (int i = 0, loopTo = len - 1; i <= loopTo; i++)
                {
                    var offerta = Consulenze.GetItemById(m_OfferteRifiutateCliente[i]);
                    if (offerta is object && offerta.Stato == ObjectStatus.OBJECT_VALID)
                        ret.Add(offerta);
                }

                return ret;
            }

            /// <summary>
        /// Restituisce o imposta il numero di offerte studiate e marcate come non fattibili
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroOfferteNonFattibili
            {
                get
                {
                    return DMD.Arrays.Len(m_OfferteNonFattibili);
                }
            }

            public CCollection<CQSPDConsulenza> GetOfferteNonFattibili()
            {
                var ret = new CCollection<CQSPDConsulenza>();
                int len = NumeroOfferteNonFattibili;
                for (int i = 0, loopTo = len - 1; i <= loopTo; i++)
                {
                    var offerta = Consulenze.GetItemById(m_OfferteNonFattibili[i]);
                    if (offerta is object && offerta.Stato == ObjectStatus.OBJECT_VALID)
                        ret.Add(offerta);
                }

                return ret;
            }

            /// <summary>
        /// Restituisce o imposta il numero di offerte bocciate dall'agenzia
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroOfferteBocciate
            {
                get
                {
                    return DMD.Arrays.Len(m_OfferteBocciate);
                }
            }

            public CCollection<CQSPDConsulenza> GetOfferteBocciate()
            {
                var ret = new CCollection<CQSPDConsulenza>();
                int len = NumeroOfferteBocciate;
                for (int i = 0, loopTo = len - 1; i <= loopTo; i++)
                {
                    var offerta = Consulenze.GetItemById(m_OfferteBocciate[i]);
                    if (offerta is object && offerta.Stato == ObjectStatus.OBJECT_VALID)
                        ret.Add(offerta);
                }

                return ret;
            }

            /// <summary>
        /// Restituisce o imposta il numero di pratiche in corso
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroPraticheInCorso
            {
                get
                {
                    return DMD.Arrays.Len(m_PraticheInCorso);
                }
            }

            public CCollection<CPraticaCQSPD> GetPraticheInCorso()
            {
                var ret = new CCollection<CPraticaCQSPD>();
                int len = NumeroPraticheInCorso;
                for (int i = 0, loopTo = len - 1; i <= loopTo; i++)
                {
                    var pratica = Pratiche.GetItemById(m_PraticheInCorso[i]);
                    if (pratica is object && pratica.Stato == ObjectStatus.OBJECT_VALID)
                        ret.Add(pratica);
                }

                return ret;
            }

            /// <summary>
        /// Restituisce o imposta il numero di pratiche liquidate durante il periodo di lavorazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroPraticheLiquidate
            {
                get
                {
                    return DMD.Arrays.Len(m_PraticheLiquidate);
                }
            }

            public CCollection<CPraticaCQSPD> GetPraticheLiquidate()
            {
                var ret = new CCollection<CPraticaCQSPD>();
                int len = NumeroPraticheInCorso;
                for (int i = 0, loopTo = len - 1; i <= loopTo; i++)
                {
                    var pratica = Pratiche.GetItemById(m_PraticheLiquidate[i]);
                    if (pratica is object && pratica.Stato == ObjectStatus.OBJECT_VALID)
                        ret.Add(pratica);
                }

                return ret;
            }

            /// <summary>
        /// Restituisce o imposta il numero di pratiche rifiutate dal cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroPraticheRifiutateCliente
            {
                get
                {
                    return DMD.Arrays.Len(m_PraticheRifiutateCliente);
                }
            }

            public CCollection<CPraticaCQSPD> GetPraticheRifiutateCliente()
            {
                var ret = new CCollection<CPraticaCQSPD>();
                int len = NumeroPraticheRifiutateCliente;
                for (int i = 0, loopTo = len - 1; i <= loopTo; i++)
                {
                    var pratica = Pratiche.GetItemById(m_PraticheRifiutateCliente[i]);
                    if (pratica is object && pratica.Stato == ObjectStatus.OBJECT_VALID)
                        ret.Add(pratica);
                }

                return ret;
            }

            /// <summary>
        /// Restituisce o imposta il numero di pratiche non fattibili
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroPraticheNonFattibili
            {
                get
                {
                    return DMD.Arrays.Len(m_PraticheNonFattibili);
                }
            }

            public CCollection<CPraticaCQSPD> GetPraticheNonFattibili()
            {
                var ret = new CCollection<CPraticaCQSPD>();
                int len = NumeroPraticheNonFattibili;
                for (int i = 0, loopTo = len - 1; i <= loopTo; i++)
                {
                    var pratica = Pratiche.GetItemById(m_PraticheNonFattibili[i]);
                    if (pratica is object && pratica.Stato == ObjectStatus.OBJECT_VALID)
                        ret.Add(pratica);
                }

                return ret;
            }

            /// <summary>
        /// Restituisce o imposta il numero di pratiche bocciate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroPraticheBocciate
            {
                get
                {
                    return DMD.Arrays.Len(m_PraticheBocciate);
                }
            }

            public CCollection<CPraticaCQSPD> GetPraticheBocciate()
            {
                var ret = new CCollection<CPraticaCQSPD>();
                int len = NumeroPraticheBocciate;
                for (int i = 0, loopTo = len - 1; i <= loopTo; i++)
                {
                    var pratica = Pratiche.GetItemById(m_PraticheBocciate[i]);
                    if (pratica is object && pratica.Stato == ObjectStatus.OBJECT_VALID)
                        ret.Add(pratica);
                }

                return ret;
            }

            /// <summary>
        /// Restituisce o imposta un valore che indica lo stato di lavorazione del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public StatoLavorazione StatoLavorazione
            {
                get
                {
                    return m_StatoLavorazione;
                }

                set
                {
                    var oldValue = m_StatoLavorazione;
                    if (oldValue == value)
                        return;
                    m_StatoLavorazione = value;
                    DoChanged("StatoLavorazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore che indica il passaggio intermedio dello stato di lavorazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public SottostatoLavorazione SottostatoLavorazione
            {
                get
                {
                    return m_SottostatoLavorazione;
                }

                set
                {
                    var oldValue = m_SottostatoLavorazione;
                    if (oldValue == value)
                        return;
                    m_SottostatoLavorazione = value;
                    DoChanged("SottostatoLavorazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di inizio lavorazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataInizioLavorazione
            {
                get
                {
                    return m_DataInizioLavorazione;
                }

                set
                {
                    var oldValue = m_DataInizioLavorazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataInizioLavorazione = value;
                    DoChanged("DataInizioLavorazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di fine lavorazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataFineLavorazione
            {
                get
                {
                    return m_DataFineLavorazione;
                }

                set
                {
                    var oldValue = m_DataFineLavorazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataFineLavorazione = value;
                    DoChanged("DataFineLavorazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data dell'ultimo aggiornamento di stato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataUltimoAggiornamento
            {
                get
                {
                    return m_DataUltimoAggiornamento;
                }

                set
                {
                    var oldValue = m_DataUltimoAggiornamento;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataUltimoAggiornamento = value;
                    DoChanged("DataUltimoAggiornamento", value, oldValue);
                }
            }

            /// <summary>
        /// Somma delle pratiche rifiutate, bocciate e non fattibili
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroPraticheAnnullate
            {
                get
                {
                    return NumeroPraticheRifiutateCliente + NumeroPraticheNonFattibili + NumeroPraticheBocciate;
                }
            }

            /// <summary>
        /// Somma delle pratice in corso, liquidate ed annullate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroPratiche
            {
                get
                {
                    return NumeroPraticheInCorso + NumeroPraticheLiquidate + NumeroPraticheAnnullate;
                }
            }

            /// <summary>
        /// Somma delle offerte rifiutate, bocciate e non fattibili
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroOfferteAnnullate
            {
                get
                {
                    return NumeroOfferteBocciate + NumeroOfferteNonFattibili + NumeroOfferteRifiutateCliente;
                }
            }

            /// <summary>
        /// Somma delle offerte inserite, proposte  e annullate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroOfferte
            {
                get
                {
                    return NumeroOfferteInserite + NumeroOfferteProposte + NumeroOfferteAnnullate;
                }
            }

            public StatoLavorazione CalcolaStato()
            {
                if (NumeroPratiche > 0)
                {
                    return StatoLavorazione.Pratica;
                }
                else if (NumeroOfferte > 0)
                {
                    return StatoLavorazione.StudioDiFattibilita;
                }
                else if (NumeroRichiesteFinanziamento > 0)
                {
                    return StatoLavorazione.RichiestaFinanziamento;
                }
                else if (NumeroBustePaga > 0)
                {
                    return StatoLavorazione.BustaPaga;
                }
                else if (NumeroVisite > 0)
                {
                    return StatoLavorazione.Visita;
                }
                else if (NumeroRichiesteConteggiEstintivi > 0)
                {
                    return StatoLavorazione.RichiestaConteggioEstintivo;
                }
                else
                {
                    return StatoLavorazione.None;
                }
            }

            public SottostatoLavorazione CalcolaSottostato()
            {
                switch (CalcolaStato())
                {
                    case StatoLavorazione.RichiestaConteggioEstintivo:
                        {
                            return SottostatoLavorazione.InLavorazione;
                        }

                    case StatoLavorazione.Visita:
                        {
                            return SottostatoLavorazione.InLavorazione;
                        }

                    case StatoLavorazione.RichiestaFinanziamento:
                        {
                            return SottostatoLavorazione.InLavorazione;
                        }

                    case StatoLavorazione.BustaPaga:
                        {
                            return SottostatoLavorazione.InLavorazione;
                        }

                    case StatoLavorazione.StudioDiFattibilita:
                        {
                            if (NumeroOfferteProposte > 0)
                            {
                                return SottostatoLavorazione.InLavorazione;
                            }
                            else if (NumeroOfferteRifiutateCliente > 0)
                            {
                                return SottostatoLavorazione.RifiutatoDalCliente;
                            }
                            else if (NumeroOfferteBocciate > 0)
                            {
                                return SottostatoLavorazione.BocciatoAgenzia;
                            }
                            else
                            {
                                return SottostatoLavorazione.NonFattibile;
                            }

                            break;
                        }

                    case StatoLavorazione.Pratica:
                        {
                            if (NumeroPratiche == NumeroPraticheAnnullate + NumeroPraticheLiquidate)
                            {
                                if (NumeroPraticheLiquidate > 0)
                                {
                                    return SottostatoLavorazione.Completato;
                                }
                                else if (NumeroPraticheRifiutateCliente > 0)
                                {
                                    return SottostatoLavorazione.RifiutatoDalCliente;
                                }
                                else if (NumeroPraticheBocciate > 0)
                                {
                                    return SottostatoLavorazione.BocciatoAgenzia;
                                }
                                else
                                {
                                    return SottostatoLavorazione.NonFattibile;
                                }
                            }
                            else
                            {
                                return SottostatoLavorazione.InLavorazione;
                            }

                            break;
                        }

                    default:
                        {
                            return SottostatoLavorazione.None;
                        }
                }
            }

            public int IDCliente
            {
                get
                {
                    return DBUtils.GetID(m_Cliente, m_IDCliente);
                }

                set
                {
                    int oldValue = IDCliente;
                    if (oldValue == value)
                        return;
                    m_IDCliente = value;
                    m_Cliente = null;
                    DoChanged("IDCliente", value, oldValue);
                }
            }

            public Anagrafica.CPersona Cliente
            {
                get
                {
                    if (m_Cliente is null)
                        m_Cliente = Anagrafica.Persone.GetItemById(m_IDCliente);
                    return m_Cliente;
                }

                set
                {
                    var oldValue = m_Cliente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDCliente = DBUtils.GetID(value);
                    m_Cliente = value;
                    if (value is object)
                    {
                        m_NomeCliente = value.Nominativo;
                        m_IconaCliente = value.IconURL;
                        PuntoOperativo = value.PuntoOperativo;
                    }

                    DoChanged("Cliente", value, oldValue);
                }
            }

            public string NomeCliente
            {
                get
                {
                    return m_NomeCliente;
                }

                set
                {
                    string oldValue = m_NomeCliente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCliente = value;
                    DoChanged("NomeCliente", value, oldValue);
                }
            }

            public string IconaCliente
            {
                get
                {
                    return m_IconaCliente;
                }

                set
                {
                    string oldValue = m_IconaCliente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IconaCliente = value;
                    DoChanged("IconaCliente", value, oldValue);
                }
            }

            private Sistema.CUser CalcolaOperatore(CCollection<CPraticaCQSPD> pratiche, CCollection<CQSPDConsulenza> consulenze, CCollection<CRichiestaFinanziamento> richiestef, CCollection<CVisita> visite)




            {
                try
                {
                    Sistema.CUser op = null;
                    var p = GetUltimaPratica(pratiche);
                    if (p is object)
                    {
                        if (op is null && p.Consulente is object)
                            op = p.Consulente.User;
                        if (op is null && p.StatoLiquidata is object)
                            op = p.StatoLiquidata.Operatore;
                        if (op is null && p.StatoPraticaCaricata is object)
                            op = p.StatoPraticaCaricata.Operatore;
                        if (op is null && p.StatoDiLavorazioneAttuale is object)
                            op = p.StatoDiLavorazioneAttuale.Operatore;
                        if (op is null)
                            op = p.CreatoDa;
                    }

                    if (op is null)
                    {
                        var c = GetUltimaConsulenza(consulenze);
                        if (c is object)
                        {
                            if (c.Consulente is object)
                                op = c.Consulente.User;
                            if (op is null)
                                op = c.CreatoDa;
                        }
                    }

                    if (op is null)
                    {
                        var r = GetUltimaRichiesta(richiestef);
                        if (r is object)
                            op = r.AssegnatoA;
                    }

                    if (op is null)
                    {
                        var v = GetUltimaVisita(visite);
                        if (v is object)
                        {
                            op = v.Operatore;
                            if (op is null)
                                op = v.CreatoDa;
                        }
                    }

                    return op;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'operatore principale
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDOperatore
            {
                get
                {
                    return DBUtils.GetID(m_Operatore, m_IDOperatore);
                }

                set
                {
                    int oldValue = IDOperatore;
                    if (oldValue == value)
                        return;
                    m_IDOperatore = value;
                    m_Operatore = null;
                    DoChanged("IDOperatore", value, oldValue);
                }
            }

            /// <summary>
        /// Azzera tutte le statistiche
        /// </summary>
        /// <remarks></remarks>
            public void Azzera()
            {
                m_StatoLavorazione = StatoLavorazione.None;
                m_SottostatoLavorazione = SottostatoLavorazione.None;
                m_DataUltimoAggiornamento = default;
                m_IDOperatore = 0;
                m_Operatore = null;
                m_NomeOperatore = "";
                m_Flags = 0;
                m_Opeatori = null;
                m_Visite = null;
                m_RichiesteConteggi = null;
                m_RichiesteFinanziamenti = null;
                m_BustePaga = null;
                m_OfferteInserite = null;
                m_OfferteProposte = null;
                m_OfferteRifiutateCliente = null;
                m_OfferteBocciate = null;
                m_OfferteNonFattibili = null;
                m_PraticheInCorso = null;
                m_PraticheLiquidate = null;
                m_PraticheRifiutateCliente = null;
                m_PraticheBocciate = null;
                m_PraticheNonFattibili = null;
                SetChanged(true);
            }

            /// <summary>
        /// Aggiorna le statistiche per il cliente corrente
        /// </summary>
        /// <remarks></remarks>
            public void Ricalcola()
            {
                if (Cliente is null)
                    throw new ArgumentNullException("cliente");
                Azzera();
                var visite = TrovaVisite();
                foreach (CVisita visita in visite)
                    NotifyVisita(visita);
                var richiesteF = TrovaRichiesteFinanziamento();
                foreach (CRichiestaFinanziamento rich in richiesteF)
                    NotifyRichiestaFinanziamento(rich);
                var richiesteCE = TrovaRichiesteConteggi();
                foreach (CRichiestaConteggio rich in richiesteCE)
                    NotifyRichiestaConteggio(rich);
                var bustePaga = TrovaBustePaga();
                foreach (Sistema.CAttachment att in bustePaga)
                    NotifyBustaPaga(att);
                var studi = TrovaStudiDiFattibilita();
                foreach (CQSPDConsulenza cons in studi)
                    NotifyConsulenza(cons);
                var pratiche = TrovaPratiche();
                foreach (CPraticaCQSPD pratica in pratiche)
                    NotifyPratica(pratica);
            }

            /// <summary>
        /// Effettua la ricerca delle visite fatte o ricevute dal cliente durante il periodo di lavorazione
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<CVisita> TrovaVisite()
            {
                var ret = new CCollection<CVisita>();
                CVisita visita;
                string dbSQL;
                IDataReader dbRis;
                dbSQL = "SELECT * FROM [tbl_Telefonate] WHERE [ClassName]='CVisita' AND [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
                dbSQL = "AND ([IDPersona]=" + IDCliente + " OR [IDPerContoDi]=" + IDCliente + ")";
                if (DataInizioLavorazione.HasValue)
                    dbSQL += " AND [Data]>=" + DBUtils.DBDate(DataInizioLavorazione.Value);
                if (DataFineLavorazione.HasValue)
                    dbSQL += " AND [Data]<=" + DBUtils.DBDate(DataFineLavorazione.Value);
                if (IDPuntoOperativo != 0)
                    dbSQL += " AND [IDPuntoOperativo]=" + IDPuntoOperativo;
                dbRis = CRM.TelDB.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    visita = new CVisita();
                    CRM.TelDB.Load(visita, dbRis);
                    ret.Add(visita);
                }

                dbRis.Dispose();
                return ret;
            }

            /// <summary>
        /// Effettua la ricerca delle visite fatte o ricevute dal cliente durante il periodo di lavorazione
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<Sistema.CAttachment> TrovaBustePaga()
            {
                var ret = new CCollection<Sistema.CAttachment>();
                Sistema.CAttachment item;
                string dbSQL;
                IDataReader dbRis;
                dbSQL = "SELECT * FROM [tbl_Attachments] WHERE [OwnerType]=" + DBUtils.DBString(DMD.RunTime.vbTypeName(Cliente)) + " AND [OwnerID]=" + IDCliente + " AND [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [Tipo]='Busta Paga'";
                if (DataInizioLavorazione.HasValue)
                    dbSQL += " AND [DataInizio]>=" + DBUtils.DBDate(DataInizioLavorazione.Value);
                if (DataFineLavorazione.HasValue)
                    dbSQL += " AND [DataInizio]<=" + DBUtils.DBDate(DataFineLavorazione.Value);
                dbRis = Databases.APPConn.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    item = new Sistema.CAttachment();
                    Databases.APPConn.Load(item, dbRis);
                    ret.Add(item);
                }

                dbRis.Dispose();
                return ret;
            }

            /// <summary>
        /// Effettua le richieste di conteggi estintivi
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<CRichiestaConteggio> TrovaRichiesteConteggi()
            {
                var ret = new CCollection<CRichiestaConteggio>();
                CRichiestaConteggio richiesta;
                string dbSQL;
                IDataReader dbRis;
                dbSQL = "SELECT * FROM [tbl_RichiesteFinanziamentiC] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
                dbSQL += " AND [IDCliente]=" + IDCliente;
                if (IDPuntoOperativo != 0)
                    dbSQL += " AND [IDPuntoOperativo]=" + IDPuntoOperativo;
                if (DataInizioLavorazione.HasValue)
                    dbSQL += " AND [DataRichiesta]>=" + DBUtils.DBDate(DataInizioLavorazione.Value);
                if (DataFineLavorazione.HasValue)
                    dbSQL += " AND [DataRichiesta]<=" + DBUtils.DBDate(DataFineLavorazione.Value);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    richiesta = new CRichiestaConteggio();
                    Database.Load(richiesta, dbRis);
                    ret.Add(richiesta);
                }

                dbRis.Dispose();
                return ret;
            }

            /// <summary>
        /// Effettua la ricerca sulla tabella delle richieste di finanziamento nel periodo
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<CRichiestaFinanziamento> TrovaRichiesteFinanziamento()
            {
                var ret = new CCollection<CRichiestaFinanziamento>();
                CRichiestaFinanziamento richiesta;
                string dbSQL;
                IDataReader dbRis;
                dbSQL = "SELECT * FROM [tbl_RichiesteFinanziamenti] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
                dbSQL += " AND [IDCliente]=" + IDCliente;
                if (IDPuntoOperativo != 0)
                    dbSQL += " AND [IDPuntoOperativo]=" + IDPuntoOperativo;
                if (DataInizioLavorazione.HasValue)
                    dbSQL += " AND [Data]>=" + DBUtils.DBDate(DataInizioLavorazione.Value);
                if (DataFineLavorazione.HasValue)
                    dbSQL += " AND [Data]<=" + DBUtils.DBDate(DataFineLavorazione.Value);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    richiesta = new CRichiestaFinanziamento();
                    Database.Load(richiesta, dbRis);
                    ret.Add(richiesta);
                }

                dbRis.Dispose();
                return ret;
            }

            public CCollection<CQSPDConsulenza> TrovaStudiDiFattibilita()
            {
                var ret = new CCollection<CQSPDConsulenza>();
                string dbSQL;
                IDataReader dbRis;
                CQSPDConsulenza consulenza;
                dbSQL = "SELECT * FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
                dbSQL += " AND [IDCliente]=" + IDCliente;
                if (IDPuntoOperativo != 0)
                    dbSQL += " AND [IDPuntoOperativo]=" + IDPuntoOperativo;
                if (DataInizioLavorazione.HasValue)
                    dbSQL += " AND [DataConsulenza]>=" + DBUtils.DBDate(DataInizioLavorazione.Value);
                if (DataFineLavorazione.HasValue)
                    dbSQL += " AND [DataConsulenza]<=" + DBUtils.DBDate(DataFineLavorazione.Value);
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    consulenza = new CQSPDConsulenza();
                    Database.Load(consulenza, dbRis);
                    ret.Add(consulenza);
                }

                dbRis.Dispose();
                return ret;
            }

            public CCollection<CPraticaCQSPD> TrovaPratiche()
            {
                var ret = new CCollection<CPraticaCQSPD>();
                string dbSQL;
                IDataReader dbRis;
                CPraticaCQSPD pratica;
                if (DataInizioLavorazione.HasValue || DataFineLavorazione.HasValue)
                {
                    dbSQL = "";
                    dbSQL += "SELECT [tbl_Pratiche].* FROM [tbl_Pratiche] ";
                    dbSQL += " INNER JOIN ";
                    dbSQL += "(SELECT [IDPratica] FROM [tbl_PraticheSTL] WHERE ";
                    if (DataInizioLavorazione.HasValue)
                    {
                        dbSQL += " [Data]>=" + DBUtils.DBDate(DataInizioLavorazione.Value);
                        if (DataFineLavorazione.HasValue)
                        {
                            dbSQL += " AND [Data]<=" + DBUtils.DBDate(DataFineLavorazione.Value);
                        }
                    }
                    else
                    {
                        dbSQL += " [Data]<=" + DBUtils.DBDate(DataFineLavorazione.Value);
                    }

                    dbSQL += " GROUP BY [IDPratica]) AS [T1] ON [tbl_Pratiche].[ID] = [T1].[IDPratica]";
                    dbSQL += " WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
                }
                else
                {
                    dbSQL = "SELECT * FROM [tbl_Pratiche] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
                }

                if (IDPuntoOperativo != 0)
                    dbSQL += " AND [IDPuntoOperativo]=" + IDPuntoOperativo;
                dbSQL += " AND [Cliente]=" + IDCliente;
                dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    pratica = new CPraticaCQSPD();
                    Database.Load(pratica, dbRis);
                    ret.Add(pratica);
                }

                dbRis.Dispose();
                return ret;
            }


            /// <summary>
        /// Restituisce o imposta l'operatore principale
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Sistema.CUser Operatore
            {
                get
                {
                    lock (this)
                    {
                        if (m_Operatore is null)
                            m_Operatore = Sistema.Users.GetItemById(m_IDOperatore);
                        return m_Operatore;
                    }
                }

                set
                {
                    Sistema.CUser oldValue;
                    lock (this)
                    {
                        oldValue = Operatore;
                        if (ReferenceEquals(oldValue, value))
                            return;
                        m_Operatore = value;
                        m_IDOperatore = DBUtils.GetID(value);
                        if (value is object)
                            m_NomeOperatore = value.Nominativo;
                    }

                    DoChanged("Operatore", value, oldValue);
                }
            }

            public string NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }

                set
                {
                    string oldValue = m_NomeOperatore;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatore = value;
                    DoChanged("NomeOperatore", value, oldValue);
                }
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDCliente":
                        {
                            m_IDCliente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCliente":
                        {
                            m_NomeCliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IconaCliente":
                        {
                            m_IconaCliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoLavorazione":
                        {
                            m_StatoLavorazione = (StatoLavorazione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "SottostatoLavorazione":
                        {
                            m_SottostatoLavorazione = (SottostatoLavorazione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataInizioLavorazione":
                        {
                            m_DataInizioLavorazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFineLavorazione":
                        {
                            m_DataFineLavorazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataUltimoAggiornamento":
                        {
                            m_DataUltimoAggiornamento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDOperatore":
                        {
                            m_IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatore":
                        {
                            m_NomeOperatore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Operatori":
                        {
                            m_Opeatori = StrToArr(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "ArrayVisite":
                        {
                            m_Visite = StrToArr(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "ArrayRichiesteC":
                        {
                            m_RichiesteConteggi = StrToArr(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "ArrayRichiesteF":
                        {
                            m_RichiesteFinanziamenti = StrToArr(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "ArrayBustePaga":
                        {
                            m_BustePaga = StrToArr(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "ArrayOfferteInserite":
                        {
                            m_OfferteInserite = StrToArr(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "ArrayOfferteProposte":
                        {
                            m_OfferteProposte = StrToArr(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "ArrayOfferteRifiutate":
                        {
                            m_OfferteRifiutateCliente = StrToArr(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "ArrayOfferteBocciate":
                        {
                            m_OfferteBocciate = StrToArr(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "ArrayOfferteNonFatt":
                        {
                            m_OfferteNonFattibili = StrToArr(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "ArrayPraticheInCorso":
                        {
                            m_PraticheInCorso = StrToArr(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "ArrayPraticheLiquidate":
                        {
                            m_PraticheLiquidate = StrToArr(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "ArrayPraticheRifiutate":
                        {
                            m_PraticheRifiutateCliente = StrToArr(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "ArrayPraticheBocciate":
                        {
                            m_PraticheBocciate = StrToArr(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "ArrayPraticheNonFatt":
                        {
                            m_PraticheNonFattibili = StrToArr(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("IconaCliente", m_IconaCliente);
                writer.WriteAttribute("StatoLavorazione", (int?)m_StatoLavorazione);
                writer.WriteAttribute("SottostatoLavorazione", (int?)m_SottostatoLavorazione);
                writer.WriteAttribute("DataInizioLavorazione", m_DataInizioLavorazione);
                writer.WriteAttribute("DataFineLavorazione", m_DataFineLavorazione);
                writer.WriteAttribute("DataUltimoAggiornamento", m_DataUltimoAggiornamento);
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", m_NomeOperatore);
                writer.WriteAttribute("Flags", m_Flags);
                base.XMLSerialize(writer);
                writer.WriteTag("Operatori", ArrToStr(m_Opeatori));
                writer.WriteTag("ArrayVisite", ArrToStr(m_Visite));
                writer.WriteTag("ArrayRichiesteC", ArrToStr(m_RichiesteConteggi));
                writer.WriteTag("ArrayRichiesteF", ArrToStr(m_RichiesteFinanziamenti));
                writer.WriteTag("ArrayBustePaga", ArrToStr(m_BustePaga));
                writer.WriteTag("ArrayOfferteInserite", ArrToStr(m_OfferteInserite));
                writer.WriteTag("ArrayOfferteProposte", ArrToStr(m_OfferteProposte));
                writer.WriteTag("ArrayOfferteRifiutate", ArrToStr(m_OfferteRifiutateCliente));
                writer.WriteTag("ArrayOfferteBocciate", ArrToStr(m_OfferteBocciate));
                writer.WriteTag("ArrayOfferteNonFatt", ArrToStr(m_OfferteNonFattibili));
                writer.WriteTag("ArrayPraticheInCorso", ArrToStr(m_PraticheInCorso));
                writer.WriteTag("ArrayPraticheLiquidate", ArrToStr(m_PraticheLiquidate));
                writer.WriteTag("ArrayPraticheRifiutate", ArrToStr(m_PraticheRifiutateCliente));
                writer.WriteTag("ArrayPraticheBocciate", ArrToStr(m_PraticheBocciate));
                writer.WriteTag("ArrayPraticheNonFatt", ArrToStr(m_PraticheNonFattibili));
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override CModulesClass GetModule()
            {
                return Module;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDClientiLavorati";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDCliente = reader.Read("IDCliente",  m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente",  m_NomeCliente);
                m_IconaCliente = reader.Read("IconaCliente",  m_IconaCliente);
                m_StatoLavorazione = reader.Read("StatoLavorazione",  m_StatoLavorazione);
                m_SottostatoLavorazione = reader.Read("SottostatoLavorazione",  m_SottostatoLavorazione);
                m_DataInizioLavorazione = reader.Read("DataInizioLavorazione",  m_DataInizioLavorazione);
                m_DataFineLavorazione = reader.Read("DataFineLavorazione",  m_DataFineLavorazione);
                m_DataUltimoAggiornamento = reader.Read("DataUltimoAggiornamento",  m_DataUltimoAggiornamento);
                m_IDOperatore = reader.Read("IDOperatore",  m_IDOperatore);
                m_NomeOperatore = reader.Read("NomeOperatore",  m_NomeOperatore);
                m_Flags = reader.Read("Flags",  m_Flags);
                string argvalue = ""; m_Opeatori = StrToArr(reader.Read("Operatori", argvalue));
                string argvalue1 = ""; m_Visite = StrToArr(reader.Read("ArrayVisite", argvalue1));
                string argvalue2 = ""; m_RichiesteConteggi = StrToArr(reader.Read("ArrayRichiesteC", argvalue2));
                string argvalue3 = ""; m_RichiesteFinanziamenti = StrToArr(reader.Read("ArrayRichiesteF", argvalue3));
                string argvalue4 = ""; m_BustePaga = StrToArr(reader.Read("ArrayBustePaga",  argvalue4));
                string argvalue5 = ""; m_OfferteInserite = StrToArr(reader.Read("ArrayOfferteInserite",  argvalue5));
                string argvalue6 = ""; m_OfferteProposte = StrToArr(reader.Read("ArrayOfferteProposte",  argvalue6));
                string argvalue7 = ""; m_OfferteRifiutateCliente = StrToArr(reader.Read("ArrayOfferteRifiutate",  argvalue7));
                string argvalue8 = ""; m_OfferteBocciate = StrToArr(reader.Read("ArrayOfferteBocciate",  argvalue8));
                string argvalue9 = ""; m_OfferteNonFattibili = StrToArr(reader.Read("ArrayOfferteNonFatt",  argvalue9));
                string argvalue10 = ""; m_PraticheInCorso = StrToArr(reader.Read("ArrayPraticheInCorso",  argvalue10));
                string argvalue11 = ""; m_PraticheLiquidate = StrToArr(reader.Read("ArrayPraticheLiquidate",  argvalue11));
                string argvalue12 = ""; m_PraticheRifiutateCliente = StrToArr(reader.Read("ArrayPraticheRifiutate",  argvalue12));
                string argvalue13 = ""; m_PraticheBocciate = StrToArr(reader.Read("ArrayPraticheBocciate",  argvalue13));
                string argvalue14 = ""; m_PraticheNonFattibili = StrToArr(reader.Read("ArrayPraticheNonFatt",  argvalue14));
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                writer.Write("IconaCliente", m_IconaCliente);
                writer.Write("StatoLavorazione", m_StatoLavorazione);
                writer.Write("SottostatoLavorazione", m_SottostatoLavorazione);
                writer.Write("DataInizioLavorazione", m_DataInizioLavorazione);
                writer.Write("DataFineLavorazione", m_DataFineLavorazione);
                writer.Write("DataUltimoAggiornamento", m_DataUltimoAggiornamento);
                writer.Write("IDOperatore", IDOperatore);
                writer.Write("NomeOperatore", m_NomeOperatore);
                writer.Write("Flags", m_Flags);
                writer.Write("Operatori", ArrToStr(m_Opeatori));
                writer.Write("ArrayVisite", ArrToStr(m_Visite));
                writer.Write("ArrayRichiesteC", ArrToStr(m_RichiesteConteggi));
                writer.Write("ArrayRichiesteF", ArrToStr(m_RichiesteFinanziamenti));
                writer.Write("ArrayBustePaga", ArrToStr(m_BustePaga));
                writer.Write("Operatori", ArrToStr(m_Opeatori));
                writer.Write("ArrayVisite", ArrToStr(m_Visite));
                writer.Write("ArrayRichiesteC", ArrToStr(m_RichiesteConteggi));
                writer.Write("ArrayRichiesteF", ArrToStr(m_RichiesteFinanziamenti));
                writer.Write("ArrayBustePaga", ArrToStr(m_BustePaga));
                writer.Write("ArrayOfferteInserite", ArrToStr(m_OfferteInserite));
                writer.Write("ArrayOfferteProposte", ArrToStr(m_OfferteProposte));
                writer.Write("ArrayOfferteRifiutate", ArrToStr(m_OfferteRifiutateCliente));
                writer.Write("ArrayOfferteBocciate", ArrToStr(m_OfferteBocciate));
                writer.Write("ArrayOfferteNonFatt", ArrToStr(m_OfferteNonFattibili));
                writer.Write("ArrayPraticheInCorso", ArrToStr(m_PraticheInCorso));
                writer.Write("ArrayPraticheLiquidate", ArrToStr(m_PraticheLiquidate));
                writer.Write("ArrayPraticheRifiutate", ArrToStr(m_PraticheRifiutateCliente));
                writer.Write("ArrayPraticheBocciate", ArrToStr(m_PraticheBocciate));
                writer.Write("ArrayPraticheNonFatt", ArrToStr(m_PraticheNonFattibili));
                writer.Write("NumeroVisite", NumeroVisite);
                writer.Write("NumeroBustePaga", NumeroBustePaga);
                writer.Write("NumeroRichiesteFinanziamento", NumeroRichiesteFinanziamento);
                writer.Write("NumeroRichiesteConteggiEstintivi", NumeroRichiesteConteggiEstintivi);
                writer.Write("NumeroStudiDiFattibilita", NumeroOfferte);
                writer.Write("NumeroOfferteProposte", NumeroOfferteProposte);
                // writer.Write("NumeroOfferteAccettate", Me.NumeroOffertePropost)
                writer.Write("NumeroOfferteRifiutate", NumeroOfferteRifiutateCliente);
                writer.Write("NumeroOfferteNonFattibili", NumeroOfferteNonFattibili);
                writer.Write("NumeroOfferteBocciate", NumeroOfferteBocciate);
                writer.Write("NumeroPratiche", NumeroPratiche);
                writer.Write("NumeroPraticheLiquidate", NumeroPraticheLiquidate);
                writer.Write("NumeroPraticheAnnullate", NumeroPraticheAnnullate);
                writer.Write("NumeroPraticheRifiutate", NumeroPraticheRifiutateCliente);
                writer.Write("NumeroPraticheNonFattibili", NumeroPraticheNonFattibili);
                writer.Write("NumeroPraticheBocciate", NumeroPraticheBocciate);
                return base.SaveToRecordset(writer);
            }

            private int ContaPraticheLiquidate(CCollection<CPraticaCQSPD> items)
            {
                int cnt = 0;
                var stLiq = StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA);
                var stArch = StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA);
                foreach (CPraticaCQSPD p in items)
                {
                    if (ReferenceEquals(p.StatoAttuale, stLiq) || ReferenceEquals(p.StatoAttuale, stArch))
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            private int ContaVisiteConsulenza(CCollection<CVisita> items)
            {
                int cnt = 0;
                foreach (CVisita v in items)
                {
                    if (DMD.Strings.InStr(DMD.Strings.LCase(v.Scopo), "consulenza") > 0)
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            private string ContaPraticheAnnullate(CCollection<CPraticaCQSPD> items)
            {
                int cnt = 0;
                var stAnn = StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA);
                foreach (CPraticaCQSPD p in items)
                {
                    if (ReferenceEquals(p.StatoAttuale, stAnn))
                    {
                        cnt += 1;
                    }
                }

                return cnt.ToString();
            }

            private string ContaPraticheRifiutate(CCollection<CPraticaCQSPD> items)
            {
                int cnt = 0;
                var stAnn = StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA);
                foreach (CPraticaCQSPD p in items)
                {
                    if (ReferenceEquals(p.StatoAttuale, stAnn) && p.StatoDiLavorazioneAttuale.RegolaApplicata is object && Sistema.TestFlag(p.StatoDiLavorazioneAttuale.RegolaApplicata.Flags, FlagsRegolaStatoPratica.DaCliente))
                    {
                        cnt += 1;
                    }
                }

                return cnt.ToString();
            }

            private DateTime? GetData(CPraticaCQSPD r)
            {
                if (r.StatoDiLavorazioneAttuale is object)
                    return r.StatoDiLavorazioneAttuale.Data;
                return r.DataDecorrenza;
            }

            private CPraticaCQSPD GetUltimaPratica(CCollection<CPraticaCQSPD> items)
            {
                CPraticaCQSPD last = null;
                foreach (CPraticaCQSPD v in items)
                {
                    if (last is null || DMD.DateUtils.Compare(GetData(v), GetData(last)) > 0)
                    {
                        last = v;
                    }
                }

                return last;
            }

            private CVisita GetUltimaVisita(CCollection<CVisita> items)
            {
                CVisita last = default;
                foreach (CVisita v in items)
                {
                    if (last is null || v.Data > last.Data)
                    {
                        last = v;
                    }
                }

                return last;
            }

            private CRichiestaFinanziamento GetUltimaRichiesta(CCollection<CRichiestaFinanziamento> items)
            {
                CRichiestaFinanziamento last = null;
                foreach (CRichiestaFinanziamento v in items)
                {
                    if (last is null || v.Data > last.Data)
                    {
                        last = v;
                    }
                }

                return last;
            }

            private DateTime? GetData(CQSPDConsulenza c)
            {
                if (c.DataConsulenza.HasValue)
                    return c.DataConsulenza;
                if (c.DataProposta.HasValue)
                    return c.DataProposta;
                if (c.DataDecorrenza.HasValue)
                    return c.DataDecorrenza;
                return c.CreatoIl;
            }

            private CQSPDConsulenza GetUltimaConsulenza(CCollection<CQSPDConsulenza> items)
            {
                CQSPDConsulenza last = null;
                foreach (CQSPDConsulenza v in items)
                {
                    if (last is null || DMD.DateUtils.Compare(GetData(v), GetData(last)) > 0)
                    {
                        last = v;
                    }
                }

                return last;
            }

            // Private Function GetPraticheInCorso() As Boolean?
            // Dim cnt As Integer = 0
            // Dim stLiq As CStatoPratica = Finanziaria.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA)
            // Dim stArch As CStatoPratica = Finanziaria.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA)

            // For Each p As CPraticaCQSPD In Me.Pratiche
            // If (p.StatoAttuale Is stLiq OrElse p.StatoAttuale Is stArch) Then
            // cnt += 1
            // End If
            // Next

            // Return cnt
            // End Function

            // Private Function GetPraticheInCorso() As CCollection(Of CPraticaCQSPD)
            // Dim ret As New CCollection(Of CPraticaCQSPD)
            // Dim stAnn As CStatoPratica = Finanziaria.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)
            // Dim stLiq As CStatoPratica = Finanziaria.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA)
            // Dim stArch As CStatoPratica = Finanziaria.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA)

            // For Each p As CPraticaCQSPD In Me.Pratiche
            // If Not ((p.StatoAttuale Is stAnn) OrElse (p.StatoAttuale Is stLiq) OrElse (p.StatoAttuale Is stArch)) Then
            // ret.Add(p)
            // End If
            // Next

            // Return ret
            // End Function

            private int[] GetArrayIDPratiche(CCollection<CPraticaCQSPD> items)
            {
                var ret = new ArrayList();
                foreach (CPraticaCQSPD item in items)
                    ret.Add(DBUtils.GetID(item));
                return (int[])ret.ToArray(typeof(int));
            }

            private int[] GetArrayIDConsulenze(CCollection<CQSPDConsulenza> items)
            {
                var ret = new ArrayList();
                foreach (CQSPDConsulenza item in items)
                    ret.Add(DBUtils.GetID(item));
                return (int[])ret.ToArray(typeof(int));
            }

            private int[] GetArrayIDRichieste(CCollection<CRichiestaFinanziamento> items)
            {
                var ret = new ArrayList();
                foreach (CRichiestaFinanziamento item in items)
                    ret.Add(DBUtils.GetID(item));
                return (int[])ret.ToArray(typeof(int));
            }

            private int[] GetArrayIDVisite(CCollection<CVisita> items)
            {
                var ret = new ArrayList();
                foreach (CVisita item in items)
                    ret.Add(DBUtils.GetID(item));
                return (int[])ret.ToArray(typeof(int));
            }

            private int ContaPraticheBocciate(CCollection<CPraticaCQSPD> items)
            {
                int cnt = 0;
                var stAnn = StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA);
                foreach (CPraticaCQSPD p in items)
                {
                    if (ReferenceEquals(p.StatoAttuale, stAnn) && p.StatoDiLavorazioneAttuale.RegolaApplicata is object && Sistema.TestFlag(p.StatoDiLavorazioneAttuale.RegolaApplicata.Flags, FlagsRegolaStatoPratica.Bocciata))
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            private int ContaPraticheNonFattibili(CCollection<CPraticaCQSPD> items)
            {
                int cnt = 0;
                var stAnn = StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA);
                foreach (CPraticaCQSPD p in items)
                {
                    if (ReferenceEquals(p.StatoAttuale, stAnn) && p.StatoDiLavorazioneAttuale.RegolaApplicata is object && Sistema.TestFlag(p.StatoDiLavorazioneAttuale.RegolaApplicata.Flags, FlagsRegolaStatoPratica.NonFattibile))
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            private int ContaConsulenzeNonFattibili(CCollection<CQSPDConsulenza> items)
            {
                int cnt = 0;
                foreach (CQSPDConsulenza c in items)
                {
                    if (c.StatoConsulenza == StatiConsulenza.BOCCIATA)
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            private int ContaConsulenzeRifiutate(CCollection<CQSPDConsulenza> items)
            {
                int cnt = 0;
                foreach (CQSPDConsulenza c in items)
                {
                    if (c.StatoConsulenza == StatiConsulenza.RIFIUTATA)
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            private int ContaConsulenzeBocciate(CCollection<CQSPDConsulenza> items)
            {
                int cnt = 0;
                foreach (CQSPDConsulenza c in items)
                {
                    if (c.StatoConsulenza == StatiConsulenza.BOCCIATA)
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            private int ContaConsulenzeAccettate(CCollection<CQSPDConsulenza> items)
            {
                int cnt = 0;
                foreach (CQSPDConsulenza c in items)
                {
                    if (DMD.Booleans.ValueOf(StatiConsulenza.ACCETTATA))
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            private int ContaConsulenzeProposte(CCollection<CQSPDConsulenza> items)
            {
                int cnt = 0;
                foreach (CQSPDConsulenza c in items)
                {
                    if (c.StatoConsulenza == StatiConsulenza.PROPOSTA)
                    {
                        cnt += 1;
                    }
                }

                return cnt;
            }

            private string ArrToStr(int[] items)
            {
                var ret = new System.Text.StringBuilder();
                int len = DMD.Arrays.Len(items);
                for (int i = 0, loopTo = len - 1; i <= loopTo; i++)
                {
                    if (i > 0)
                        ret.Append(",");
                    ret.Append(items[i].ToString());
                }

                return ret.ToString();
            }

            private int[] StrToArr(string text)
            {
                text = DMD.Strings.Trim(text);
                if (string.IsNullOrEmpty(text))
                    return null;
                var ret = new ArrayList();
                var tmp = Strings.Split(text, ",");
                for (int i = 0, loopTo = DMD.Arrays.UBound(tmp); i <= loopTo; i++)
                {
                    if (Sistema.Formats.ToInteger(tmp[i]) != 0)
                    {
                        ret.Add(Sistema.Formats.ToInteger(tmp[i]));
                    }
                }

                return (int[])ret.ToArray(typeof(int));
            }

            /// <summary>
        /// Notifica a questo oggetto la visita del cliente
        /// </summary>
        /// <param name="visita"></param>
        /// <remarks></remarks>
            public void NotifyVisita(CVisita visita)
            {
                if (visita.Stato == ObjectStatus.OBJECT_VALID && DMD.DateUtils.CheckBetween(visita.Data, m_DataInizioLavorazione, m_DataFineLavorazione))
                {
                    m_Visite = AddIfNotIn(m_Visite, DBUtils.GetID(visita));
                    m_Opeatori = this.AddIfNotIn(m_Opeatori, visita.IDOperatore);
                    DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, visita.Data);
                    if (StatoLavorazione <= StatoLavorazione.Visita)
                    {
                        Operatore = visita.Operatore;
                    }

                    StatoLavorazione = CalcolaStato();
                    SottostatoLavorazione = CalcolaSottostato();
                }
                else
                {
                    m_Visite = RemoveIfIn(m_Visite, DBUtils.GetID(visita));
                }
            }


            /// <summary>
        /// Notifica a questo oggetto il caricamento della busta paga
        /// </summary>
        /// <param name="a"></param>
        /// <remarks></remarks>
            public void NotifyBustaPaga(Sistema.CAttachment a)
            {
                if (a.Stato == ObjectStatus.OBJECT_VALID && DMD.DateUtils.CheckBetween(a.DataInizio, m_DataInizioLavorazione, m_DataFineLavorazione))
                {
                    m_BustePaga = AddIfNotIn(m_BustePaga, DBUtils.GetID(a));
                    m_Opeatori = AddIfNotIn(m_Opeatori, a.CreatoDaId);
                    DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, a.DataInizio);
                    if (StatoLavorazione <= StatoLavorazione.BustaPaga)
                    {
                        Operatore = a.CreatoDa;
                    }

                    StatoLavorazione = CalcolaStato();
                    SottostatoLavorazione = CalcolaSottostato();
                }
                else
                {
                    m_BustePaga = RemoveIfIn(m_BustePaga, DBUtils.GetID(a));
                }
            }

            public void NotifyRichiestaFinanziamento(CRichiestaFinanziamento richiesta)
            {
                if (richiesta.Stato == ObjectStatus.OBJECT_VALID && DMD.DateUtils.CheckBetween(richiesta.Data, m_DataInizioLavorazione, m_DataFineLavorazione))
                {
                    m_RichiesteFinanziamenti = AddIfNotIn(m_RichiesteFinanziamenti, DBUtils.GetID(richiesta));
                    m_Opeatori = AddIfNotIn(m_Opeatori, richiesta.IDAssegnatoA);
                    DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, richiesta.Data);
                    if (StatoLavorazione <= StatoLavorazione.RichiestaFinanziamento)
                    {
                        Operatore = richiesta.AssegnatoA;
                    }

                    StatoLavorazione = CalcolaStato();
                    SottostatoLavorazione = CalcolaSottostato();
                }
                else
                {
                    m_RichiesteFinanziamenti = RemoveIfIn(m_RichiesteFinanziamenti, DBUtils.GetID(richiesta));
                }
            }

            private int[] AddIfNotIn(int[] items, int id)
            {
                if (id == 0)
                    return items;
                int i = DMD.Arrays.BinarySearch(items, id);
                if (i < 0)
                {
                    items = DMD.Arrays.Append(items, id);
                    Array.Sort(items);
                    SetChanged(true);
                }

                return items;
            }

            private int[] RemoveIfIn(int[] items, int id)
            {
                if (id == 0)
                    return items;
                int i = DMD.Arrays.BinarySearch(items, id);
                if (i >= 0)
                {
                    items = DMD.Arrays.RemoveAt(items, i);
                    SetChanged(true);
                }

                return items;
            }

            private bool IsIn(int[] items, int id)
            {
                if (items is null || id == 0)
                    return false;
                return DMD.Arrays.BinarySearch(items, id) >= 0;
            }

            public void NotifyConsulenza(CQSPDConsulenza consulenza)
            {
                if (consulenza is null)
                    throw new ArgumentNullException("consulenza");
                int id = DBUtils.GetID(consulenza);
                if (id == 0)
                    return;
                if (consulenza.Stato == ObjectStatus.OBJECT_VALID && DMD.DateUtils.CheckBetween(consulenza.DataConsulenza, m_DataInizioLavorazione, m_DataFineLavorazione))
                {
                    m_Opeatori = AddIfNotIn(m_Opeatori, consulenza.IDAnnullataDa);
                    m_Opeatori = AddIfNotIn(m_Opeatori, consulenza.IDConfermataDa);
                    m_Opeatori = AddIfNotIn(m_Opeatori, consulenza.IDInseritoDa);
                    m_Opeatori = AddIfNotIn(m_Opeatori, consulenza.IDPropostaDa);
                    if (consulenza.Consulente is object)
                    {
                        m_Opeatori = AddIfNotIn(m_Opeatori, consulenza.Consulente.IDUser);
                    }

                    switch (consulenza.StatoConsulenza)
                    {
                        case StatiConsulenza.INSERITA:
                            {
                                DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, consulenza.DataConsulenza);
                                m_OfferteInserite = AddIfNotIn(m_OfferteInserite, id);
                                m_OfferteProposte = RemoveIfIn(m_OfferteProposte, id);
                                m_OfferteRifiutateCliente = RemoveIfIn(m_OfferteRifiutateCliente, id);
                                m_OfferteBocciate = RemoveIfIn(m_OfferteBocciate, id);
                                m_OfferteNonFattibili = RemoveIfIn(m_OfferteNonFattibili, id);
                                break;
                            }

                        case StatiConsulenza.PROPOSTA:
                        case StatiConsulenza.ACCETTATA:
                            {
                                DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, consulenza.DataProposta);
                                DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, consulenza.DataConferma);
                                if (consulenza.DataAnnullamento.HasValue)
                                {
                                    m_OfferteInserite = RemoveIfIn(m_OfferteInserite, id);
                                    m_OfferteProposte = RemoveIfIn(m_OfferteProposte, id);
                                    m_OfferteRifiutateCliente = RemoveIfIn(m_OfferteRifiutateCliente, id);
                                    m_OfferteBocciate = AddIfNotIn(m_OfferteBocciate, id);
                                    m_OfferteNonFattibili = RemoveIfIn(m_OfferteNonFattibili, id);
                                }
                                else
                                {
                                    m_OfferteInserite = RemoveIfIn(m_OfferteInserite, id);
                                    m_OfferteProposte = AddIfNotIn(m_OfferteProposte, id);
                                    m_OfferteRifiutateCliente = RemoveIfIn(m_OfferteRifiutateCliente, id);
                                    m_OfferteBocciate = RemoveIfIn(m_OfferteBocciate, id);
                                    m_OfferteNonFattibili = RemoveIfIn(m_OfferteNonFattibili, id);
                                }

                                break;
                            }

                        case StatiConsulenza.RIFIUTATA:
                            {
                                m_OfferteInserite = RemoveIfIn(m_OfferteInserite, id);
                                m_OfferteProposte = RemoveIfIn(m_OfferteProposte, id);
                                m_OfferteRifiutateCliente = AddIfNotIn(m_OfferteRifiutateCliente, id);
                                m_OfferteBocciate = RemoveIfIn(m_OfferteBocciate, id);
                                m_OfferteNonFattibili = RemoveIfIn(m_OfferteNonFattibili, id);
                                DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, consulenza.DataConferma);
                                DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, consulenza.DataAnnullamento);
                                break;
                            }

                        case StatiConsulenza.NONFATTIBILE:
                            {
                                m_OfferteInserite = RemoveIfIn(m_OfferteInserite, id);
                                m_OfferteProposte = RemoveIfIn(m_OfferteProposte, id);
                                m_OfferteRifiutateCliente = RemoveIfIn(m_OfferteRifiutateCliente, id);
                                m_OfferteBocciate = RemoveIfIn(m_OfferteBocciate, id);
                                m_OfferteNonFattibili = AddIfNotIn(m_OfferteNonFattibili, id);
                                DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, consulenza.DataConferma);
                                DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, consulenza.DataAnnullamento);
                                break;
                            }

                        default:
                            {
                                m_OfferteInserite = RemoveIfIn(m_OfferteInserite, id);
                                m_OfferteProposte = RemoveIfIn(m_OfferteProposte, id);
                                m_OfferteRifiutateCliente = RemoveIfIn(m_OfferteRifiutateCliente, id);
                                m_OfferteBocciate = AddIfNotIn(m_OfferteBocciate, id);
                                m_OfferteNonFattibili = RemoveIfIn(m_OfferteNonFattibili, id);
                                DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, consulenza.DataConferma);
                                DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, consulenza.DataAnnullamento);
                                break;
                            }
                    }

                    if (StatoLavorazione <= StatoLavorazione.StudioDiFattibilita)
                    {
                        if (consulenza.Consulente is object)
                            Operatore = consulenza.Consulente.User;
                    }

                    StatoLavorazione = CalcolaStato();
                    SottostatoLavorazione = CalcolaSottostato();
                    switch (SottostatoLavorazione)
                    {
                        case SottostatoLavorazione.Completato:
                        case SottostatoLavorazione.BocciatoAgenzia:
                        case SottostatoLavorazione.BocciatoIstituto:
                        case SottostatoLavorazione.NonFattibile:
                        case SottostatoLavorazione.RifiutatoDalCliente:
                            {
                                DataUltimoAggiornamento = DataUltimoAggiornamento;
                                break;
                            }
                    }
                }
                else
                {
                    m_OfferteInserite = RemoveIfIn(m_OfferteInserite, id);
                    m_OfferteProposte = RemoveIfIn(m_OfferteProposte, id);
                    m_OfferteRifiutateCliente = RemoveIfIn(m_OfferteRifiutateCliente, id);
                    m_OfferteBocciate = RemoveIfIn(m_OfferteBocciate, id);
                    m_OfferteNonFattibili = RemoveIfIn(m_OfferteNonFattibili, id);
                }
            }

            private DateTime GetDataContatto(CPraticaCQSPD p)
            {
                DateTime? d = default;
                if (p.StatoPreventivo is object)
                    d = p.StatoPreventivo.Data;
                if (d.HasValue == false)
                {
                    foreach (CStatoLavorazionePratica stl in p.StatiDiLavorazione)
                        d = DMD.DateUtils.Min(d, stl.Data);
                }

                if (d.HasValue == false)
                    d = p.CreatoIl;
                return d.Value;
            }

            public void NotifyPratica(CPraticaCQSPD item)
            {
                if (item.Stato == ObjectStatus.OBJECT_VALID && DMD.DateUtils.CheckBetween(GetDataContatto(item), m_DataInizioLavorazione, m_DataFineLavorazione))
                {
                    if (item.StatoDiLavorazioneAttuale is object)
                        m_Opeatori = AddIfNotIn(m_Opeatori, item.StatoDiLavorazioneAttuale.IDOperatore);
                    switch (item.StatoAttuale.MacroStato)
                    {
                        case StatoPraticaEnum.STATO_LIQUIDATA: // 70:
                        case StatoPraticaEnum.STATO_ARCHIVIATA: // 80:
                            {
                                m_PraticheInCorso = RemoveIfIn(m_PraticheInCorso, DBUtils.GetID(item));
                                m_PraticheLiquidate = AddIfNotIn(m_PraticheLiquidate, DBUtils.GetID(item));
                                m_PraticheBocciate = RemoveIfIn(m_PraticheBocciate, DBUtils.GetID(item));
                                m_PraticheNonFattibili = RemoveIfIn(m_PraticheNonFattibili, DBUtils.GetID(item));
                                m_PraticheRifiutateCliente = RemoveIfIn(m_PraticheRifiutateCliente, DBUtils.GetID(item));
                                if (item.StatoLiquidata is object)
                                {
                                    DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, item.StatoLiquidata.Data);
                                }

                                break;
                            }

                        case StatoPraticaEnum.STATO_ANNULLATA: // 1000:
                            {
                                if (item.StatoDiLavorazioneAttuale.RegolaApplicata is object)
                                {
                                    if (Sistema.TestFlag(item.StatoDiLavorazioneAttuale.RegolaApplicata.Flags, FlagsRegolaStatoPratica.DaCliente))
                                    {
                                        m_PraticheInCorso = RemoveIfIn(m_PraticheInCorso, DBUtils.GetID(item));
                                        m_PraticheLiquidate = RemoveIfIn(m_PraticheLiquidate, DBUtils.GetID(item));
                                        m_PraticheBocciate = RemoveIfIn(m_PraticheBocciate, DBUtils.GetID(item));
                                        m_PraticheNonFattibili = RemoveIfIn(m_PraticheNonFattibili, DBUtils.GetID(item));
                                        m_PraticheRifiutateCliente = AddIfNotIn(m_PraticheRifiutateCliente, DBUtils.GetID(item));
                                    }
                                    else if (Sistema.TestFlag(item.StatoDiLavorazioneAttuale.RegolaApplicata.Flags, FlagsRegolaStatoPratica.Bocciata))
                                    {
                                        m_PraticheInCorso = RemoveIfIn(m_PraticheInCorso, DBUtils.GetID(item));
                                        m_PraticheLiquidate = RemoveIfIn(m_PraticheLiquidate, DBUtils.GetID(item));
                                        m_PraticheBocciate = AddIfNotIn(m_PraticheBocciate, DBUtils.GetID(item));
                                        m_PraticheNonFattibili = RemoveIfIn(m_PraticheNonFattibili, DBUtils.GetID(item));
                                        m_PraticheRifiutateCliente = RemoveIfIn(m_PraticheRifiutateCliente, DBUtils.GetID(item));
                                    }
                                    else
                                    {
                                        m_PraticheInCorso = RemoveIfIn(m_PraticheInCorso, DBUtils.GetID(item));
                                        m_PraticheLiquidate = RemoveIfIn(m_PraticheLiquidate, DBUtils.GetID(item));
                                        m_PraticheBocciate = RemoveIfIn(m_PraticheBocciate, DBUtils.GetID(item));
                                        m_PraticheNonFattibili = AddIfNotIn(m_PraticheNonFattibili, DBUtils.GetID(item));
                                        m_PraticheRifiutateCliente = RemoveIfIn(m_PraticheRifiutateCliente, DBUtils.GetID(item));
                                    }
                                }
                                else
                                {
                                    m_PraticheInCorso = RemoveIfIn(m_PraticheInCorso, DBUtils.GetID(item));
                                    m_PraticheLiquidate = RemoveIfIn(m_PraticheLiquidate, DBUtils.GetID(item));
                                    m_PraticheBocciate = RemoveIfIn(m_PraticheBocciate, DBUtils.GetID(item));
                                    m_PraticheNonFattibili = AddIfNotIn(m_PraticheNonFattibili, DBUtils.GetID(item));
                                    m_PraticheRifiutateCliente = RemoveIfIn(m_PraticheRifiutateCliente, DBUtils.GetID(item));
                                }

                                if (item.StatoAnnullata is object)
                                {
                                    DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, item.StatoAnnullata.Data);
                                }

                                break;
                            }

                        default:
                            {
                                m_PraticheInCorso = AddIfNotIn(m_PraticheInCorso, DBUtils.GetID(item));
                                m_PraticheLiquidate = RemoveIfIn(m_PraticheLiquidate, DBUtils.GetID(item));
                                m_PraticheBocciate = RemoveIfIn(m_PraticheBocciate, DBUtils.GetID(item));
                                m_PraticheNonFattibili = RemoveIfIn(m_PraticheNonFattibili, DBUtils.GetID(item));
                                m_PraticheRifiutateCliente = RemoveIfIn(m_PraticheRifiutateCliente, DBUtils.GetID(item));
                                break;
                            }
                    }

                    if (item.Consulente is object)
                        Operatore = item.Consulente.User;
                    if (item.StatoDiLavorazioneAttuale is object)
                    {
                        DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, item.StatoDiLavorazioneAttuale.Data);
                    }

                    StatoLavorazione = CalcolaStato();
                    SottostatoLavorazione = CalcolaSottostato();
                    switch (SottostatoLavorazione)
                    {
                        case SottostatoLavorazione.Completato:
                        case SottostatoLavorazione.BocciatoAgenzia:
                        case SottostatoLavorazione.BocciatoIstituto:
                        case SottostatoLavorazione.NonFattibile:
                        case SottostatoLavorazione.RifiutatoDalCliente:
                            {
                                DataUltimoAggiornamento = DataUltimoAggiornamento;
                                break;
                            }
                    }
                }
                else
                {
                    m_PraticheInCorso = RemoveIfIn(m_PraticheInCorso, DBUtils.GetID(item));
                    m_PraticheLiquidate = RemoveIfIn(m_PraticheLiquidate, DBUtils.GetID(item));
                    m_PraticheRifiutateCliente = RemoveIfIn(m_PraticheRifiutateCliente, DBUtils.GetID(item));
                    m_PraticheBocciate = RemoveIfIn(m_PraticheBocciate, DBUtils.GetID(item));
                    m_PraticheNonFattibili = RemoveIfIn(m_PraticheNonFattibili, DBUtils.GetID(item));
                }
            }

            public void NotifyRichiestaConteggio(CRichiestaConteggio richiesta)
            {
                if (richiesta.Stato == ObjectStatus.OBJECT_VALID && DMD.DateUtils.CheckBetween(richiesta.DataRichiesta, m_DataInizioLavorazione, m_DataFineLavorazione))
                {
                    m_RichiesteConteggi = AddIfNotIn(m_RichiesteConteggi, DBUtils.GetID(richiesta));
                    m_Opeatori = AddIfNotIn(m_Opeatori, richiesta.PresaInCaricoDaID);
                    DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, richiesta.DataRichiesta);
                }
                else
                {
                    m_RichiesteConteggi = RemoveIfIn(m_RichiesteConteggi, DBUtils.GetID(richiesta));
                }
            }

            private int[] MergeArrays(int[] arr1, int[] arr2)
            {
                int len2 = DMD.Arrays.Len(arr2);
                for (int i = 0, loopTo = len2 - 1; i <= loopTo; i++)
                    arr1 = AddIfNotIn(arr1, arr2[i]);
                return arr1;
            }

            public void MergeWith(ClientiLavoratiStatsItem item)
            {
                if (item is null)
                    throw new ArgumentNullException("item");
                m_DataInizioLavorazione = DMD.DateUtils.Min(m_DataInizioLavorazione, item.m_DataInizioLavorazione);
                m_DataFineLavorazione = DMD.DateUtils.Max(m_DataFineLavorazione, item.m_DataFineLavorazione);
                m_DataUltimoAggiornamento = DMD.DateUtils.Max(m_DataUltimoAggiornamento, item.m_DataUltimoAggiornamento);
                m_Opeatori = MergeArrays(m_Opeatori, item.m_Opeatori);
                m_Visite = MergeArrays(m_Visite, item.m_Visite);
                m_RichiesteConteggi = MergeArrays(m_RichiesteConteggi, item.m_RichiesteConteggi);
                m_RichiesteFinanziamenti = MergeArrays(m_RichiesteFinanziamenti, item.m_RichiesteFinanziamenti);
                m_BustePaga = MergeArrays(m_BustePaga, item.m_BustePaga);
                m_OfferteInserite = MergeArrays(m_OfferteInserite, item.m_OfferteInserite);
                m_OfferteProposte = MergeArrays(m_OfferteProposte, item.m_OfferteProposte);
                m_OfferteRifiutateCliente = MergeArrays(m_OfferteRifiutateCliente, item.m_OfferteRifiutateCliente);
                m_OfferteBocciate = MergeArrays(m_OfferteBocciate, item.m_OfferteBocciate);
                m_OfferteNonFattibili = MergeArrays(m_OfferteNonFattibili, item.m_OfferteNonFattibili);
                m_PraticheInCorso = MergeArrays(m_PraticheInCorso, item.m_PraticheInCorso);
                m_PraticheLiquidate = MergeArrays(m_PraticheLiquidate, item.m_PraticheLiquidate);
                m_PraticheRifiutateCliente = MergeArrays(m_PraticheRifiutateCliente, item.m_PraticheRifiutateCliente);
                m_PraticheBocciate = MergeArrays(m_PraticheBocciate, item.m_PraticheBocciate);
                m_PraticheNonFattibili = MergeArrays(m_PraticheNonFattibili, item.m_PraticheNonFattibili);
                StatoLavorazione = CalcolaStato();
                SottostatoLavorazione = CalcolaSottostato();
            }
        }
    }
}