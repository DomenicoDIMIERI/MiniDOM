using System;
using System.Collections;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        public enum SortPreventivoBy : int
        {
            /// <summary>
        /// Visualizza in alto le offerte che favoriscono il cliente
        /// </summary>
        /// <remarks></remarks>
            SORTBY_NETTO = 0,

            /// <summary>
        /// Visualizza in alto le offerte che favoriscono il guadagno aziendale
        /// </summary>
        /// <remarks></remarks>
            SORTBY_RICARICOTOTALE = 1
        }

        /// <summary>
        /// Preventivatore comparativo
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CPreventivo 
            : Databases.DBObjectPO, IComparer
        {
            private string m_Nome; // [TEXT] Nome del cliente
            private string m_Cognome; // [TEXT] Cognome del cliente
            private string m_Sesso; // [TEXT*1] M o F : Sesso del cliente
            private DateTime? m_DataNascita; // [DATE] Data di nascita del cliente
            private string m_NatoAComune; // [TEXT] Comune di nascita del cliente
            private string m_NatoAProvincia; // [TEXT] Provincia di nascita del cliente
            private string m_CodiceFiscale; // [TEXT] Codice fiscale del cliente
            private string m_PartitaIVA; // [TEXT] Partita IVA del cliente
            private DateTime? m_DataAssunzione; // [DATE] Data di assunzione del cliente
            private DateTime? m_DataDecorrenza; // [DATE] Data di decorrenza del preventivo
            private string m_TipoContratto;   // [TEXT*1] ID del tipo contratto
                                              // Private m_TipoContrattoEx As TipoContratto '[CCQSPDTipoContratto] Oggetto tipo contratto
            private string m_TipoRapporto; // [TEXT*1] ID del tipo rapporto
                                           // Private m_TipoRapportoEx As TipoRapporto '[CCQSPDTipoRapporto] Oggetto tipo rapporto
            private int m_Durata; // [INT]  Numero di mesi della durata del finanziamento
            private decimal m_Rata; // [DOUBLE] Rata mensile
            private double m_Provvigionale; // [DOUBLE] Percentuale di provvigione sul montante lordo
            private bool m_CaricaAlMassimo;  // [BOOL] Se vero calcola la provvigione massima applicabile per ogni offerta
            private int m_ProfiloID; // [INT]  ID del profilo di preventivazione utilizzato
            [NonSerialized] private CProfilo m_Profilo; // [CCQSPDListino] Profilo di preventivazione utilizzato
            private string m_NomeProfilo;   // [TEXT] Nome del profilo di preventivazione utilizzato
            private SortPreventivoBy m_SortBy; // [INT] SORTBY_NETTO visualizza per prima le offerte più vantaggiose per il cliente, SORTBY_RICARICOTOTALE visualizza per prime le offerte più vantaggiose per l'azienda
            private decimal m_StipendioLordo; // [DOUBLE] Stipendio lordo percepito dal cliente    
            private decimal m_StipendioNetto; // [DOUBLE] Stipendio netto percepito dal cliente
            private int m_NumeroMensilita; // [INT] Numero di mensilità
            private decimal m_TFR; // [DOUBLE] TFR maturato dal cliente
            private int m_AmministrazioneID; // [INT] ID dell'amministrazione per cui lavora il cliente
            [NonSerialized] private Anagrafica.CAzienda m_Amministrazione; // [CAzienda] Oggetto ammnistrazione per cui lavora il cliente
            private string m_eMail; // [TEXT] e-mail del cliente
            private string m_Telefono; // [TEXT] Telefono del cliente
            private string m_Fax; // [TEXT] Fax del cliente
            [NonSerialized] private CCQSPDOfferte m_Offerte; // [CCQSPDOfferte] Oggetto che racchiude le offerte calcolate per questo preventivo

            public CPreventivo()
            {
                m_Nome = "";
                m_Cognome = "";
                m_Sesso = "";
                m_DataNascita = DateUtils.DateSerial(DMD.DateUtils.Year(DMD.DateUtils.Now()) - 18, 1, 1);
                m_NatoAComune = "";
                m_NatoAProvincia = "";
                m_CodiceFiscale = "";
                m_PartitaIVA = "";
                m_DataAssunzione = DateUtils.DateSerial(DMD.DateUtils.Year(DMD.DateUtils.Now()), 1, 1);
                m_DataDecorrenza = DMD.DateUtils.GetNextMonthFirstDay(DMD.DateUtils.Now());
                if (DMD.DateUtils.Day(DMD.DateUtils.Now()) >= 15)
                    m_DataDecorrenza = DMD.DateUtils.GetNextMonthFirstDay((DateTime)m_DataDecorrenza);
                m_TipoContratto = "C";
                // Me.m_TipoContrattoEx = Nothing
                m_TipoRapporto = "";
                // Me.m_TipoRapportoEx = Nothing
                m_Durata = 120;
                m_Rata = 0m;
                m_Provvigionale = 0d;
                m_CaricaAlMassimo = false;
                m_ProfiloID = 0;
                m_Profilo = null;
                m_NomeProfilo = "";
                m_SortBy = SortPreventivoBy.SORTBY_NETTO;
                m_StipendioLordo = 0m;
                m_StipendioNetto = 0m;
                m_NumeroMensilita = 13;
                m_TFR = 0m;
                m_AmministrazioneID = 0;
                m_Amministrazione = null;
                m_eMail = "";
                m_Telefono = "";
                m_Fax = "";
                m_Offerte = null;
            }

            public override CModulesClass GetModule()
            {
                return Preventivi.Module;
            }

            /// <summary>
        /// Restituisce o imposta l'e-mail del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string eMail
            {
                get
                {
                    return m_eMail;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_eMail;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_eMail = Strings.Trim(value);
                    DoChanged("eMail", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il telefono del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Telefono
            {
                get
                {
                    return m_Telefono;
                }

                set
                {
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    string oldValue = m_Telefono;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Telefono = value;
                    DoChanged("Telefono", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il fax dl cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Fax
            {
                get
                {
                    return m_Fax;
                }

                set
                {
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    string oldValue = m_Fax;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Fax = value;
                    DoChanged("Fax", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del comune di nascita del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NatoAComune
            {
                get
                {
                    return m_NatoAComune;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NatoAComune;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NatoAComune = value;
                    DoChanged("NatoAComune", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome della provincia di nascita del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NatoAProvincia
            {
                get
                {
                    return m_NatoAProvincia;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NatoAProvincia;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NatoAProvincia = value;
                    DoChanged("NatoAProvincia", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il codice fiscale del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string CodiceFiscale
            {
                get
                {
                    return m_CodiceFiscale;
                }

                set
                {
                    value = Sistema.Formats.ParseCodiceFiscale(value);
                    string oldValue = m_CodiceFiscale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceFiscale = value;
                    DoChanged("CodiceFiscale", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la partita iva del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string PartitaIVA
            {
                get
                {
                    return m_PartitaIVA;
                }

                set
                {
                    value = Sistema.Formats.ParsePartitaIVA(value);
                    string oldValue = m_PartitaIVA;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_PartitaIVA = value;
                    DoChanged("PartitaIVA", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il profilo di preventivazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CProfilo Profilo
            {
                get
                {
                    if (m_Profilo is null)
                        m_Profilo = Profili.GetItemById(m_ProfiloID);
                    return m_Profilo;
                }

                set
                {
                    var oldValue = Profilo;
                    if (oldValue == value)
                        return;
                    m_Profilo = value;
                    m_ProfiloID = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeProfilo = value.ProfiloVisibile;
                    DoChanged("Profilo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del profilo di preventivazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDProfilo
            {
                get
                {
                    return DBUtils.GetID(m_Profilo, m_ProfiloID);
                }

                set
                {
                    int oldValue = IDProfilo;
                    if (oldValue == value)
                        return;
                    m_ProfiloID = value;
                    m_Profilo = null;
                    DoChanged("IDProfilo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeProfilo
            {
                get
                {
                    return m_NomeProfilo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeProfilo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeProfilo = value;
                    DoChanged("NomeProfilo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore che indica se visualizzare le offerte favorendo il cliente o l'azienda
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public SortPreventivoBy SortBy
            {
                get
                {
                    return m_SortBy;
                }

                set
                {
                    var oldValue = m_SortBy;
                    if (oldValue == value)
                        return;
                    m_SortBy = value;
                    DoChanged("SortBy", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta lo stipendio lordo del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal StipendioLordo
            {
                get
                {
                    return m_StipendioLordo;
                }

                set
                {
                    decimal oldValue = m_StipendioLordo;
                    if (oldValue == value)
                        return;
                    m_StipendioLordo = value;
                    DoChanged("StipendioLordo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta lo stipendio netto del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal StipendioNetto
            {
                get
                {
                    return m_StipendioNetto;
                }

                set
                {
                    decimal oldValue = m_StipendioNetto;
                    if (oldValue == value)
                        return;
                    m_StipendioNetto = value;
                    DoChanged("StipendioNetto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il numero di mensilità percepite dal cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroMensilita
            {
                get
                {
                    return m_NumeroMensilita;
                }

                set
                {
                    int oldValue = m_NumeroMensilita;
                    if (oldValue == value)
                        return;
                    m_NumeroMensilita = value;
                    DoChanged("NumeroMensilita", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il TFR maturato dal cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal TFR
            {
                get
                {
                    return m_TFR;
                }

                set
                {
                    decimal oldValue = m_TFR;
                    if (oldValue == value)
                        return;
                    m_TFR = value;
                    DoChanged("TFR", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'amministrazione per cui lavora il cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDAmministrazione
            {
                get
                {
                    return DBUtils.GetID(m_Amministrazione, m_AmministrazioneID);
                }

                set
                {
                    int oldValue = IDAmministrazione;
                    if (oldValue == value)
                        return;
                    m_Amministrazione = null;
                    m_AmministrazioneID = value;
                    DoChanged("IDAmministrazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un oggetto CAzienda che rappresenta l'amministrazione per cui lavora il cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Anagrafica.CAzienda Amministrazione
            {
                get
                {
                    if (m_Amministrazione is null)
                        m_Amministrazione = Anagrafica.Aziende.GetItemById(m_AmministrazioneID);
                    return m_Amministrazione;
                }

                set
                {
                    var oldValue = Amministrazione;
                    if (oldValue == value)
                        return;
                    m_Amministrazione = value;
                    m_AmministrazioneID = DBUtils.GetID(value);
                    DoChanged("Amministrazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di nascita del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataNascita
            {
                get
                {
                    return m_DataNascita;
                }

                set
                {
                    var oldValue = m_DataNascita;
                    if (oldValue == value == true)
                        return;
                    m_DataNascita = value;
                    DoChanged("DataNascita", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di assunzione del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataAssunzione
            {
                get
                {
                    return m_DataAssunzione;
                }

                set
                {
                    var oldValue = m_DataAssunzione;
                    if (oldValue == value == true)
                        return;
                    m_DataAssunzione = value;
                    DoChanged("DataAssunzione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di decorrenza dell'offerta
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataDecorrenza
            {
                get
                {
                    return m_DataDecorrenza;
                }

                set
                {
                    var oldValue = m_DataDecorrenza;
                    if (oldValue == value == true)
                        return;
                    m_DataDecorrenza = value;
                    DoChanged("DataDecorrenza", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce il nominativo del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Nominativo
            {
                get
                {
                    return Strings.Trim(DMD.Strings.ToNameCase(m_Nome) + " " + Strings.UCase(m_Cognome));
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il cognome del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Cognome
            {
                get
                {
                    return m_Cognome;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Cognome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Cognome = value;
                    DoChanged("Cognome", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del tipo contratto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string TipoContratto
            {
                get
                {
                    return m_TipoContratto;
                }

                set
                {
                    value = Strings.UCase(Strings.Left(Strings.Trim(value), 1));
                    string oldValue = m_TipoContratto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoContratto = value;
                    DoChanged("TipoContratto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del tipo rapporto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string TipoRapporto
            {
                get
                {
                    return m_TipoRapporto;
                }

                set
                {
                    value = Strings.UCase(Strings.Left(Strings.Trim(value), 1));
                    string oldValue = m_TipoRapporto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoRapporto = value;
                    DoChanged("TipoRapporto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il sesso del cliente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Sesso
            {
                get
                {
                    return m_Sesso;
                }

                set
                {
                    value = Strings.UCase(Strings.Left(Strings.Trim(value), 1));
                    string oldValue = m_Sesso;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Sesso = value;
                    DoChanged("Sesso", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisc o imposta la durata (in mesi) del finanziamento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int Durata
            {
                get
                {
                    return m_Durata;
                }

                set
                {
                    int oldValue = m_Durata;
                    if (oldValue == value)
                        return;
                    m_Durata = value;
                    DoChanged("Durata", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il valore della rata mensile
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal Rata
            {
                get
                {
                    return m_Rata;
                }

                set
                {
                    decimal oldValue = m_Rata;
                    if (oldValue == value)
                        return;
                    m_Rata = value;
                    DoChanged("Rata", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce il momentate lordo del finanziamento
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal MontanteLordo
            {
                get
                {
                    return Rata * Durata;
                }
            }

            /// <summary>
        /// Restituisce o imposta la provvigione percentuale sul momentante lordo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double Provvigionale
            {
                get
                {
                    return m_Provvigionale;
                }

                set
                {
                    double oldValue = m_Provvigionale;
                    if (oldValue == value)
                        return;
                    m_Provvigionale = value;
                    DoChanged("Provvigionale", value, oldValue);
                }
            }

            /// <summary>
        /// Se vero indica di calcolare tutte le offerte caricando al massimo possibile
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool CaricaAlMassimo
            {
                get
                {
                    return m_CaricaAlMassimo;
                }

                set
                {
                    if (m_CaricaAlMassimo == value)
                        return;
                    m_CaricaAlMassimo = value;
                    DoChanged("CaricaAlMassimo", value, !value);
                }
            }

            /// <summary>
        /// Restituisce l'elenco delle offerte calcolate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCQSPDOfferte Offerte
            {
                get
                {
                    if (m_Offerte is null)
                        m_Offerte = new CCQSPDOfferte(this);
                    return m_Offerte;
                }
            }

            /// <summary>
        /// Forza il ricalcolo
        /// </summary>
        /// <remarks></remarks>
            public void ForceRecalc()
            {
                Offerte.Clear();
                ResetID();
                Calcola();
            }

            /// <summary>
        /// Ricacola
        /// </summary>
        /// <remarks></remarks>
            public void Calcola()
            {
                // Resettiamo le offerte
                Offerte.Clear();
                var prodotti = Profilo.GetProdottiValidi();
                // Per ogni prodotto
                foreach (CCQSPDProdotto prodotto in prodotti)
                {
                    // Se tipo contratto ed il tipo rapporto coincidono
                    if ((prodotto.IdTipoContratto ?? "") == (TipoContratto ?? "") && (prodotto.IdTipoRapporto ?? "") == (TipoRapporto ?? ""))
                    {
                        // Per ogni tabella Finanziaria
                        foreach (CProdottoXTabellaFin relFin in prodotto.TabelleFinanziarieRelations)
                        {
                            // e per ogni tripla di tabelle assicurative
                            foreach (CProdottoXTabellaAss relAss in prodotto.TabelleAssicurativeRelations)
                            {
                                foreach (CProdottoXTabellaSpesa ts in prodotto.TabelleSpese)
                                {
                                    var offerta = new COffertaCQS();
                                    offerta.Preventivo = this;
                                    offerta.Prodotto = prodotto;
                                    offerta.TabellaFinanziariaRel = relFin;
                                    offerta.TabellaAssicurativaRel = relAss;
                                    offerta.TabellaSpese = ts;
                                    offerta.Provvigionale.set_PercentualeSu(MontanteLordo, (float?)Provvigionale);
                                    offerta.Calcola();
                                    Offerte.Add(offerta);
                                    offerta.Save();
                                }
                            }
                        }
                    }
                }

                Offerte.Comparer = this;
                Offerte.Sort();
            }

            int IComparer.Compare(object a, object b)
            {
                return Compare((COffertaCQS)a, (COffertaCQS)b);
            }

            /// <summary>
        /// Compara due offerte
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public int Compare(COffertaCQS a, COffertaCQS b)
            {
                int CompareRet = default;
                if (a.ErrorCode == 0 & b.ErrorCode != 0)
                    return -1;
                if (a.ErrorCode != 0 & b.ErrorCode == 0)
                    return 1;
                switch (SortBy)
                {
                    case SortPreventivoBy.SORTBY_RICARICOTOTALE:
                        {
                            // If (a.GuadagnoAziendale + a.ValoreProvvigioni < b.GuadagnoAziendale + b.ValoreProvvigioni) Then Return 1
                            // If (a.GuadagnoAziendale + a.ValoreProvvigioni > b.GuadagnoAziendale + b.ValoreProvvigioni) Then Return -1
                            if (a.NettoRicavo < b.NettoRicavo)
                                return 1;
                            if (a.NettoRicavo > b.NettoRicavo)
                                return -1;
                            return 0;
                        }

                    default:
                        {
                            if (a.NettoRicavo < b.NettoRicavo)
                                return 1;
                            if (a.NettoRicavo > b.NettoRicavo)
                                return -1;
                            // If (a.GuadagnoAziendale + a.ValoreProvvigioni < b.GuadagnoAziendale + b.ValoreProvvigioni) Then Return 1
                            // If (a.GuadagnoAziendale + a.ValoreProvvigioni > b.GuadagnoAziendale + b.ValoreProvvigioni) Then Return -1
                            CompareRet = 0;
                            break;
                        }
                }

                return CompareRet;
            }

            public override string GetTableName()
            {
                return "tbl_Preventivi";
            }

            protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            {
                bool ret = base.SaveToDatabase(dbConn, force);
                if (ret & m_Offerte is object)
                    m_Offerte.Save(force);
                return ret;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_SortBy = reader.Read("SortBy", this.m_SortBy);
                this.m_DataNascita = reader.Read("DataNascita", this.m_DataNascita);
                this.m_DataAssunzione = reader.Read("DataAssunzione", this.m_DataAssunzione);
                this.m_DataDecorrenza = reader.Read("DataDecorrenza", this.m_DataDecorrenza);
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.m_Cognome = reader.Read("Cognome", this.m_Cognome);
                this.m_TipoContratto = reader.Read("TipoContratto", this.m_TipoContratto);
                this.m_TipoRapporto = reader.Read("TipoRapporto", this.m_TipoRapporto);
                this.m_Sesso = reader.Read("Sesso", this.m_Sesso);
                this.m_Durata = reader.Read("Durata", this.m_Durata);
                this.m_Rata = reader.Read("Rata", this.m_Rata);
                this.m_Provvigionale = reader.Read("Provvigionale", this.m_Provvigionale);
                this.m_CaricaAlMassimo = reader.Read("CaricaAlMassimo", this.m_CaricaAlMassimo);
                this.m_ProfiloID = reader.Read("Profilo", this.m_ProfiloID);
                this.m_NomeProfilo = reader.Read("NomeProfilo", this.m_NomeProfilo);
                this.m_StipendioLordo = reader.Read("StipendioLordo", this.m_StipendioLordo);
                this.m_StipendioNetto = reader.Read("StipendioNetto", this.m_StipendioNetto);
                this.m_NumeroMensilita = reader.Read("NumeroMensilita", this.m_NumeroMensilita);
                this.m_TFR = reader.Read("TFR", this.m_TFR);
                this.m_AmministrazioneID = reader.Read("Amministrazione", this.m_AmministrazioneID);
                this.m_NatoAComune = reader.Read("NatoAComune", this.m_NatoAComune);
                this.m_NatoAProvincia = reader.Read("NatoAProvincia", this.m_NatoAProvincia);
                this.m_CodiceFiscale = reader.Read("CodiceFiscale", this.m_CodiceFiscale);
                // reader.Read("PartitaIVA", Me.m_PartitaIVA)
                // reader.Read("eMail", Me.m_eMail)
                // reader.Read("Telefono", Me.m_Telefono)
                // reader.Read("Fax", Me.m_Fax)
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("SortBy", m_SortBy);
                writer.Write("DataNascita", m_DataNascita);
                writer.Write("DataAssunzione", m_DataAssunzione);
                writer.Write("DataDecorrenza", m_DataDecorrenza);
                writer.Write("Nome", m_Nome);
                writer.Write("Cognome", m_Cognome);
                writer.Write("TipoContratto", m_TipoContratto);
                writer.Write("TipoRapporto", m_TipoRapporto);
                writer.Write("Sesso", m_Sesso);
                writer.Write("Durata", m_Durata);
                writer.Write("Rata", m_Rata);
                writer.Write("Provvigionale", m_Provvigionale);
                writer.Write("CaricaAlMassimo", m_CaricaAlMassimo);
                writer.Write("Profilo", IDProfilo);
                writer.Write("NomeProfilo", m_NomeProfilo);
                writer.Write("StipendioLordo", m_StipendioLordo);
                writer.Write("StipendioNetto", m_StipendioNetto);
                writer.Write("NumeroMensilita", m_NumeroMensilita);
                writer.Write("TFR", m_TFR);
                writer.Write("Amministrazione", IDAmministrazione);
                writer.Write("NatoAComune", m_NatoAComune);
                writer.Write("NatoAProvincia", m_NatoAProvincia);
                writer.Write("CodiceFiscale", m_CodiceFiscale);
                // writer.Write("PartitaIVA", Me.m_PartitaIVA)
                // writer.Write("eMail", Me.m_eMail)
                // writer.Write("Telefono", Me.m_Telefono)
                // writer.Write("Fax", Me.m_Fax)
                return base.SaveToRecordset(writer);
            }

            // ---------------------------
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Cognome", m_Cognome);
                writer.WriteAttribute("Sesso", m_Sesso);
                writer.WriteAttribute("DataNascita", m_DataNascita);
                writer.WriteAttribute("NatoAComune", m_NatoAComune);
                writer.WriteAttribute("NatoAProvincia", m_NatoAProvincia);
                writer.WriteAttribute("CodiceFiscale", m_CodiceFiscale);
                writer.WriteAttribute("PartitaIVA", m_PartitaIVA);
                writer.WriteAttribute("DataAssunzione", m_DataAssunzione);
                writer.WriteAttribute("DataDecorrenza", m_DataDecorrenza);
                writer.WriteAttribute("TipoContratto", m_TipoContratto);
                writer.WriteAttribute("TipoRapporto", m_TipoRapporto);
                writer.WriteAttribute("Durata", m_Durata);
                writer.WriteAttribute("Rata", m_Rata);
                writer.WriteAttribute("Provvigionale", m_Provvigionale);
                writer.WriteAttribute("CaricaAlMassimo", m_CaricaAlMassimo);
                writer.WriteAttribute("ProfiloID", IDProfilo);
                writer.WriteAttribute("NomeProfilo", m_NomeProfilo);
                writer.WriteAttribute("SortBy", (int?)m_SortBy);
                writer.WriteAttribute("StipendioLordo", m_StipendioLordo);
                writer.WriteAttribute("StipendioNetto", m_StipendioNetto);
                writer.WriteAttribute("NumeroMensilita", m_NumeroMensilita);
                writer.WriteAttribute("TFR", m_TFR);
                writer.WriteAttribute("AmministrazioneID", IDAmministrazione);
                writer.WriteAttribute("eMail", m_eMail);
                writer.WriteAttribute("Telefono", m_Telefono);
                writer.WriteAttribute("Fax", m_Fax);
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Cognome":
                        {
                            m_Cognome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Sesso":
                        {
                            m_Sesso = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataNascita":
                        {
                            m_DataNascita = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "NatoAComune":
                        {
                            m_NatoAComune = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NatoAProvincia":
                        {
                            m_NatoAProvincia = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CodiceFiscale":
                        {
                            m_CodiceFiscale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PartitaIVA":
                        {
                            m_PartitaIVA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataAssunzione":
                        {
                            m_DataAssunzione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataDecorrenza":
                        {
                            m_DataDecorrenza = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "TipoContratto":
                        {
                            m_TipoContratto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoRapporto":
                        {
                            m_TipoRapporto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Durata":
                        {
                            m_Durata = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Rata":
                        {
                            m_Rata = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Provvigionale":
                        {
                            m_Provvigionale = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "CaricaAlMassimo":
                        {
                            m_CaricaAlMassimo = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "ProfiloID":
                        {
                            m_ProfiloID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeProfilo":
                        {
                            m_NomeProfilo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SortBy":
                        {
                            m_SortBy = (SortPreventivoBy)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StipendioLordo":
                        {
                            m_StipendioLordo = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "StipendioNetto":
                        {
                            m_StipendioNetto = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NumeroMensilita":
                        {
                            m_NumeroMensilita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TFR":
                        {
                            m_TFR = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "AmministrazioneID":
                        {
                            m_AmministrazioneID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "eMail":
                        {
                            m_eMail = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Telefono":
                        {
                            m_Telefono = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Fax":
                        {
                            m_Fax = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Offerte":
                        {
                            if (fieldValue is CCQSPDOfferte)
                            {
                                m_Offerte = (CCQSPDOfferte)fieldValue;
                                m_Offerte.SetOwner(this);
                            }
                            else
                            {
                                m_Offerte = null;
                            }

                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }
        }
    }
}