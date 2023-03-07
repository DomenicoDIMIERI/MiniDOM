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

        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        // Rappresnta un prodotto utilizzabile per il calcolo dei preventivi o per la compilazione dei rapportini
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        public enum SpeseProdotto : int
        {
            SPESE_NESSUNA = 0,
            SPESE_FISSE = 1
        }

        public enum ProvvigioneErogataDa : int
        {
            Direttamente = 0,
            BancaFinanziatrice = 1
        }

        public enum TipoProdotto : int
        {
            PRODOTTO_IN_BANDO = 1,
            PRODOTTO_PICCOLE_ATC = 2,
            PRODOTTO_ORDINARIO = 2
        }

        [Flags]
        public enum ProdottoFlags : int
        {
            None = 0,

            /// <summary>
        /// Se vero indica che il prodotto è utilizzabile solo se approvato
        /// </summary>
        /// <remarks></remarks>
            RequireApprovation = 1,

            /// <summary>
        /// Nasconde il campo provvigione massima nell'interfaccia del caricamento della pratica/offerta
        /// </summary>
        /// <remarks></remarks>
            HidePMax = 2
        }

        [Serializable]
        public class CCQSPDProdotto 
            : Databases.DBObject, IComparable, ICloneable
        {
            private bool m_Visibile;
            private int m_CessionarioID;
            [NonSerialized] private CCQSPDCessionarioClass m_Cessionario;
            private string m_NomeCessionario;
            private int m_GruppoProdottiID;
            [NonSerialized] private CGruppoProdotti m_GruppoProdotti;
            private string m_IdTipoContratto;
            private string m_IdTipoRapporto;
            private string m_Categoria;
            private string m_Nome;
            private string m_Descrizione;
            private TEGCalcFlag m_TipoCalcoloTEG;
            private double m_ProvvigioneMassima;
            private bool m_InBando;
            private CCollection<CAssicurazione> m_Assicurazioni;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            private ProvvigioneErogataDa m_ProvvigioneErogataDa; // [INT] 0=Direttamente, 1=Dalla banca finanziatrice
            private CStatoPratica m_StatoIniziale;
            private CTabelleFinanziarieProdottoCollection m_TabelleFinRelations; // Collezione contenente la definizione delle relazioni con le tabelle finanziarie
            private CTabelleAssicurativeProdottoCollection m_TabelleAssRelations;  // Collezione contenente la definizione delle relazioni con le tabelle finanziarie
            private CProdottoXConvenzioneCollection m_Convenzioni;  // Collezione contenente la definizione delle relazioni con le tabelle finanziarie
            private TEGCalcFlag m_TipoCalcoloTAEG;
            private int m_IDTabellaSpese; // [INT] ID della tabella spese
            private CTabellaSpese m_TabellaSpese; // [CTabellaSpese] Oggetto che definisce le spese per il prodotto
            private int m_IDTabellaTAEG; // [INT] ID della tabella utilizzata per i tassi soglia TAEG
            private CTabellaTEGMax m_TabellaTAEG;          // [CTabellaTEGMax] Tabella utilizzata per i tassi soglia TAEG
            private int m_IDTabellaTEG; // [INT] ID della tabella utilizzata per i tassi soglia TEG
            private CTabellaTEGMax m_TabellaTEG; // [CTabellaTEGMax] Tabella utilizzata per i tassi soglia TEG
            private int m_IDStatoIniziale;
            private ProdottoFlags m_Flags;
            private CKeyCollection m_Attributi;
            private CProdottoXTabellaSpesaCollection m_TabelleSpese;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCQSPDProdotto()
            {
                m_TabelleFinRelations = null;
                m_TabelleAssRelations = null;
                // Me.m_Banca = Nothing
                m_InBando = false;
                m_GruppoProdotti = null;
                m_Assicurazioni = null;
                m_TipoCalcoloTEG = TEGCalcFlag.CALCOLOTEG_SPESEALL;
                m_TipoCalcoloTAEG = TEGCalcFlag.CALCOLOTEG_SPESEALL;
                m_IDTabellaSpese = 0;
                m_TabellaSpese = null;
                m_Convenzioni = null;
                m_Visibile = true;
                m_CessionarioID = 0;
                m_Cessionario = null;
                m_GruppoProdottiID = 0;
                m_GruppoProdotti = null;
                m_DataInizio = default;
                m_DataFine = default;
                m_ProvvigioneErogataDa = 0;
                m_IDTabellaTEG = 0;
                m_TabellaTEG = null;
                m_IDTabellaTAEG = 0;
                m_TabellaTAEG = null;
                m_IDStatoIniziale = 0;
                m_StatoIniziale = null;
                m_Flags = ProdottoFlags.None;
                m_Attributi = new CKeyCollection();
                m_TabelleSpese = null;
            }

            public CProdottoXTabellaSpesaCollection TabelleSpese
            {
                get
                {
                    lock (this)
                    {
                        if (m_TabelleSpese is null)
                            m_TabelleSpese = new CProdottoXTabellaSpesaCollection(this);
                        return m_TabelleSpese;
                    }
                }
            }

            public CKeyCollection Attributi
            {
                get
                {
                    return m_Attributi;
                }
            }

            /// <summary>
        /// Restituisce o imposta dei flags per il prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public ProdottoFlags Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    var oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta lo stato iniziale da cui partono le pratiche caricate con questo prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CStatoPratica StatoIniziale
            {
                get
                {
                    if (m_StatoIniziale is null)
                        m_StatoIniziale = StatiPratica.GetItemById(m_IDStatoIniziale);
                    return m_StatoIniziale;
                }

                set
                {
                    var oldValue = m_StatoIniziale;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_StatoIniziale = value;
                    m_IDStatoIniziale = DBUtils.GetID(value);
                    DoChanged("StatoIniziale", value, oldValue);
                }
            }

            public CStatoPratica GetStatoIniziale()
            {
                if (StatoIniziale is object)
                {
                    return StatoIniziale;
                }
                else if (GruppoProdotti is object)
                {
                    return GruppoProdotti.StatoIniziale;
                }
                else
                {
                    return null;
                }
            }

            public int IDStatoIniziale
            {
                get
                {
                    return DBUtils.GetID(m_StatoIniziale, m_IDStatoIniziale);
                }

                set
                {
                    int oldValue = IDStatoIniziale;
                    if (oldValue == value)
                        return;
                    m_IDStatoIniziale = value;
                    m_StatoIniziale = null;
                    DoChanged("IDStatoIniziale", value, oldValue);
                }
            }

            public override CModulesClass GetModule()
            {
                return Prodotti.Module;
            }

            /// <summary>
        /// Restituisce o imposta l'ID della tabella utilizzata per il calcolo del TEG massimo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDTabellaTEG
            {
                get
                {
                    return DBUtils.GetID(m_TabellaTEG, m_IDTabellaTEG);
                }

                set
                {
                    int oldValue = IDTabellaTEG;
                    if (oldValue == value)
                        return;
                    m_IDTabellaTEG = value;
                    m_TabellaTEG = null;
                    DoChanged("IDTabellaTEG", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'oggetto tabella utilizzata per il calcolo del TEG massimo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CTabellaTEGMax TabellaTEG
            {
                get
                {
                    if (m_TabellaTEG is null)
                        m_TabellaTEG = TabelleTEGMax.GetItemById(m_IDTabellaTEG);
                    return m_TabellaTEG;
                }

                set
                {
                    var oldValue = TabellaTEG;
                    if (oldValue == value)
                        return;
                    m_TabellaTEG = value;
                    m_IDTabellaTEG = DBUtils.GetID(value);
                    DoChanged("TabellaTEG", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della tabella utilizzata per il calcolo del TAEG massimo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDTabellaTAEG
            {
                get
                {
                    return DBUtils.GetID(m_TabellaTAEG, m_IDTabellaTAEG);
                }

                set
                {
                    int oldValue = IDTabellaTAEG;
                    if (oldValue == value)
                        return;
                    m_IDTabellaTAEG = value;
                    m_TabellaTAEG = null;
                    DoChanged("IDTabellaTAEG", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'oggetto tabella utilizzata per il calcolo del TAEG massimo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CTabellaTEGMax TabellaTAEG
            {
                get
                {
                    if (m_TabellaTAEG is null)
                        m_TabellaTAEG = TabelleTEGMax.GetItemById(m_IDTabellaTAEG);
                    return m_TabellaTAEG;
                }

                set
                {
                    var oldValue = TabellaTAEG;
                    if (oldValue == value)
                        return;
                    m_TabellaTAEG = value;
                    m_IDTabellaTAEG = DBUtils.GetID(value);
                    DoChanged("TabellaTAEG", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID della tabella utilizzata per il calcolo delle spese
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDTabellaSpese
            {
                get
                {
                    return DBUtils.GetID(m_TabellaSpese, m_IDTabellaSpese);
                }

                set
                {
                    int oldValue = IDTabellaSpese;
                    if (value == oldValue)
                        return;
                    m_IDTabellaSpese = value;
                    m_TabellaSpese = null;
                    DoChanged("IDTabellaSpese", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'oggetto tabella utilizzata per il calcolo del TEG massimo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CTabellaSpese TabellaSpese
            {
                get
                {
                    if (m_TabellaSpese is null)
                        m_TabellaSpese = Finanziaria.TabelleSpese.GetItemById(m_IDTabellaSpese);
                    return m_TabellaSpese;
                }

                set
                {
                    var oldValue = TabellaSpese;
                    if (oldValue == value)
                        return;
                    m_TabellaSpese = value;
                    m_IDTabellaSpese = DBUtils.GetID(value);
                    DoChanged("TabellaSpese", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un intero che indica le spese da includere nel calcolo del TAEG
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public TipoCalcoloTEG TipoCalcoloTAEG
            {
                get
                {
                    return (TipoCalcoloTEG)m_TipoCalcoloTAEG;
                }

                set
                {
                    TipoCalcoloTEG oldValue = (TipoCalcoloTEG)m_TipoCalcoloTAEG;
                    if (oldValue == value)
                        return;
                    m_TipoCalcoloTAEG = (TEGCalcFlag)value;
                    DoChanged("TipoCalcoloTAEG", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore intero che indica chi eroga la provvigione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public ProvvigioneErogataDa ProvvigioneErogataDa
            {
                get
                {
                    return m_ProvvigioneErogataDa;
                }

                set
                {
                    var oldValue = m_ProvvigioneErogataDa;
                    if (oldValue == value)
                        return;
                    m_ProvvigioneErogataDa = value;
                    DoChanged("ProvvigioneErogataDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del cessionario che eroga il prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int CessionarioID
            {
                get
                {
                    return DBUtils.GetID(m_Cessionario, m_CessionarioID);
                }

                set
                {
                    int oldValue = CessionarioID;
                    if (oldValue == value)
                        return;
                    m_CessionarioID = value;
                    m_Cessionario = null;
                    DoChanged("CessionarioID", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'oggetto Cessionario che eroga il prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCQSPDCessionarioClass Cessionario
            {
                get
                {
                    if (m_Cessionario is null)
                        m_Cessionario = Cessionari.GetItemById(m_CessionarioID);
                    return m_Cessionario;
                }

                set
                {
                    var oldValue = Cessionario;
                    if (oldValue == value)
                        return;
                    m_Cessionario = value;
                    m_CessionarioID = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeCessionario = value.Nome;
                    DoChanged("Cessionario", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del cessionario che eroga il prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeCessionario
            {
                get
                {
                    return m_NomeCessionario;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeCessionario;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCessionario = value;
                    DoChanged("NomeCessionario", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del gruppo di prodotti
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int GruppoProdottiID
            {
                get
                {
                    return DBUtils.GetID(m_GruppoProdotti, m_GruppoProdottiID);
                }

                set
                {
                    int oldValue = GruppoProdottiID;
                    if (oldValue == value)
                        return;
                    m_GruppoProdottiID = value;
                    m_GruppoProdotti = null;
                    DoChanged("GruppoProdottiID", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'oggetto GruppoProdotti
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CGruppoProdotti GruppoProdotti
            {
                get
                {
                    if (m_GruppoProdotti is null)
                        m_GruppoProdotti = GruppiProdotto.GetItemById(m_GruppoProdottiID);
                    return m_GruppoProdotti;
                }

                set
                {
                    var oldValue = GruppoProdotti;
                    if (oldValue == value)
                        return;
                    m_GruppoProdotti = value;
                    m_GruppoProdottiID = DBUtils.GetID(value);
                    DoChanged("GruppoProdotti", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se il prodotto è visibile nell'area preventivi
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Visibile
            {
                get
                {
                    return m_Visibile;
                }

                set
                {
                    if (m_Visibile == value)
                        return;
                    m_Visibile = value;
                    DoChanged("Visibile", value, !value);
                }
            }

            // 'Restituisce una collezione di relazioni tra il prodotto e le tabelle finanziarie
            // Property Get TabelleFinanziarieCorrelate
            // If (m_TabelleFinanziarieCorrelate Is Nothing) Then
            // Set m_TabelleFinanziarieCorrelate = New CProdottoXTabelleFinanziarieCorrelate
            // Call m_TabelleFinanziarieCorrelate.Initialize(Me)
            // End If
            // Set TabelleFinanziarieCorrelate = m_TabelleFinanziarieCorrelate
            // End Property

            // Restituisce un oggetto CTabelleFinanziarieProdottoCollection contenete la definizione
            // delle relazioni tra questo prodotto e le tabelle finanziarie. Per ogni relazione
            // sono specificati anche i vincoli di validità
            public CTabelleFinanziarieProdottoCollection TabelleFinanziarieRelations
            {
                get
                {
                    if (m_TabelleFinRelations is null)
                        m_TabelleFinRelations = new CTabelleFinanziarieProdottoCollection(this);
                    return m_TabelleFinRelations;
                }
            }

            // 'Restituisce una collezione di relazioni tra il prodotto e le trie di tabelle assicurative (vita, impiego, credito) 
            // Property Get TabelleAssicurativeCorrelate
            // If (m_TabelleAssicurativeCorrelate Is Nothing) Then
            // Set m_TabelleAssicurtiveCorrelate = New CProdottoXTabelleAssicurativeCorrelate
            // Call m_TabelleAssicurativeCorrelate.Initialize(Me)
            // End If
            // Set TabelleAssicurativeCorrelate = m_TabelleAssicurativeCorrelate
            // End Property

            // Restituisce un oggetto CTabelleAssicurativeProdottoCollection contenete la definizione
            // delle relazioni tra questo prodotto e le triple di tabelle assicurative (vita, impiego, crdito). Per ogni relazione
            // sono specificati anche i vincoli di validità
            public CTabelleAssicurativeProdottoCollection TabelleAssicurativeRelations
            {
                get
                {
                    lock (this)
                    {
                        if (m_TabelleAssRelations is null)
                            m_TabelleAssRelations = new CTabelleAssicurativeProdottoCollection(this);
                        return m_TabelleAssRelations;
                    }
                }
            }

            public CProdottoXConvenzioneCollection Convenzioni
            {
                get
                {
                    if (m_Convenzioni is null)
                        m_Convenzioni = new CProdottoXConvenzioneCollection(this);
                    return m_Convenzioni;
                }
            }

            internal void InvalidateConvenzioni()
            {
                m_Convenzioni = null;
            }

            // '
            // Property Get Assicurazioni
            // If (m_Assicurazioni Is Nothing) Then
            // Set m_Assicurazioni = New CFsbPrev_Assicurazioni
            // Call m_Assicurazioni.Initialize(Me)
            // End If
            // Set Assicurazioni = m_Assicurazioni
            // End Property

            /// <summary>
        /// Restituisce o imposta il massimo caricabile per il prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double ProvvigioneMassima
            {
                get
                {
                    return m_ProvvigioneMassima;
                }

                set
                {
                    double oldValue = m_ProvvigioneMassima;
                    if (oldValue == value)
                        return;
                    m_ProvvigioneMassima = value;
                    DoChanged("ProvvigioneMassima", value, oldValue);
                }
            }

            public double? CalcolaProvvigioneMassima(COffertaCQS o, CCollection<EstinzioneXEstintore> estinzioni)
            {
                CProdottoXTabellaFin tblFin = null;
                if (o.TabellaFinanziariaRel is object)
                {
                    tblFin = o.TabellaFinanziariaRel;
                }
                else if (TabelleFinanziarieRelations.Count > 0)
                {
                    tblFin = TabelleFinanziarieRelations[0];
                }

                bool isRinnovo = false;
                bool isEstinzione = false;
                foreach (EstinzioneXEstintore e in estinzioni)
                {
                    if (e.Selezionata)
                    {
                        isEstinzione = true;
                        if (e.Stato == ObjectStatus.OBJECT_VALID && (e.NomeCessionario ?? "") == (o.NomeCessionario ?? ""))
                        {
                            isRinnovo = true;
                        }
                    }
                }

                if (tblFin is object && tblFin.Tabella is object)
                {
                    if (isRinnovo)
                    {
                        return tblFin.Tabella.ProvvMaxConRinnovi;
                    }
                    else if (isEstinzione)
                    {
                        return tblFin.Tabella.ProvvMaxConEstinzioni;
                    }
                    else
                    {
                        return tblFin.Tabella.ProvvMax;
                    }
                }
                else
                {
                    return default;
                }
            }

            public double? CalcolaProvvigioneTAN(COffertaCQS o, CCollection<EstinzioneXEstintore> estinzioni)
            {
                CProdottoXTabellaFin tblFin = null;
                if (o.TabellaFinanziariaRel is object)
                {
                    tblFin = o.TabellaFinanziariaRel;
                }
                else if (TabelleFinanziarieRelations.Count > 0)
                {
                    tblFin = TabelleFinanziarieRelations[0];
                }

                bool isRinnovo = false;
                bool isEstinzione = false;
                for (int i = 0, loopTo = estinzioni.Count - 1; i <= loopTo; i++)
                {
                    var e = estinzioni[i];
                    if (e.Selezionata)
                    {
                        isEstinzione = true;
                        if (e.Stato == ObjectStatus.OBJECT_VALID && (e.NomeCessionario ?? "") == (o.NomeCessionario ?? ""))
                        {
                            isRinnovo = true;
                        }
                    }
                }

                double? pTAN = default;
                CTabellaFinanziaria tabella = null;
                if (tblFin is object)
                {
                    tabella = tblFin.Tabella;
                }

                if (tabella is object)
                {
                    if (isRinnovo)
                    {
                        pTAN = tabella.ProvvTANR;
                    }
                    else if (isEstinzione)
                    {
                        pTAN = tabella.ProvvTANE;
                    }
                    else
                    {
                        pTAN = tabella.ScontoVisibile;
                    }
                }

                return pTAN;
            }

            public double? CalcolaRappel(COffertaCQS o, CCollection<EstinzioneXEstintore> estinzioni)
            {
                CProdottoXTabellaFin tblFin = null;
                if (o.TabellaFinanziariaRel is object)
                {
                    tblFin = o.TabellaFinanziariaRel;
                }
                else if (TabelleFinanziarieRelations.Count > 0)
                {
                    tblFin = TabelleFinanziarieRelations[0];
                }

                bool isRinnovo = false;
                bool isEstinzione = false;
                if (estinzioni is object)
                {
                    for (int i = 0, loopTo = estinzioni.Count - 1; i <= loopTo; i++)
                    {
                        var e = estinzioni[i];
                        if (e.Selezionata)
                        {
                            isEstinzione = true;
                            if (e.Stato == ObjectStatus.OBJECT_VALID && (e.NomeCessionario ?? "") == (o.NomeCessionario ?? ""))
                            {
                                isRinnovo = true;
                            }
                        }
                    }
                }

                CTabellaFinanziaria tariffa = null;
                if (tblFin is object)
                {
                    tariffa = tblFin.Tabella;
                }

                var spese = TabellaSpese;
                double? rappel = default;
                if (tariffa is object && Sistema.TestFlag(tariffa.Flags, TabellaFinanziariaFlags.ForzaRappel))
                {
                    rappel = tariffa.Sconto;
                }
                else if (spese is object && o.Durata > 0)
                {
                    rappel = spese.get_Rappel(o.Durata);
                }

                return rappel;
            }

            public double? CalcolaRiduzioneProvvigionale(COffertaCQS o, CCollection<EstinzioneXEstintore> estinzioni)
            {
                CProdottoXTabellaFin tblFin = null;
                if (o.TabellaFinanziariaRel is object)
                {
                    tblFin = o.TabellaFinanziariaRel;
                }
                else if (TabelleFinanziarieRelations.Count > 0)
                {
                    tblFin = TabelleFinanziarieRelations[0];
                }

                bool isRinnovo = false;
                bool isEstinzione = false;
                if (estinzioni is object)
                {
                    for (int i = 0, loopTo = estinzioni.Count - 1; i <= loopTo; i++)
                    {
                        var e = estinzioni[i];
                        if (e.Selezionata)
                        {
                            isEstinzione = true;
                            if (e.Stato == ObjectStatus.OBJECT_VALID && (e.NomeCessionario ?? "") == (o.NomeCessionario ?? ""))
                            {
                                isRinnovo = true;
                            }
                        }
                    }
                }

                CTabellaFinanziaria tariffa = null;
                if (tblFin is object)
                    tariffa = tblFin.Tabella;
                if (tariffa is object)
                {
                    if (isRinnovo)
                    {
                        return tariffa.ProvvMaxConRinnovi;
                    }
                    else if (isEstinzione)
                    {
                        return tariffa.ProvvMaxConEstinzioni;
                    }
                }

                return 0.0d;
            }



            /// <summary>
        /// Restituisce o imposta una stringa che indica il tipo di contratto a cui appartiene il prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string IdTipoContratto
            {
                get
                {
                    return m_IdTipoContratto;
                }

                set
                {
                    value = Strings.UCase(Strings.Left(Strings.Trim(value), 1));
                    string oldValue = m_IdTipoContratto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IdTipoContratto = value;
                    DoChanged("IdTipoContratto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta una stringa che indica il tipo di rapporto a cui appartiene il prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string IdTipoRapporto
            {
                get
                {
                    return m_IdTipoRapporto;
                }

                set
                {
                    value = Strings.UCase(Strings.Left(Strings.Trim(value), 1));
                    string oldValue = m_IdTipoRapporto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IdTipoRapporto = value;
                    DoChanged("IdTipoRapporto", value, oldValue);
                }
            }

            // Property Get TipoContratto
            // If (m_TipoContratto Is Nothing) Then Set m_TipoContratto = FsbPrev.GetTipoContrattoById(m_IdTipoContratto)
            // Set TipoContratto = m_TipoContratto
            // End Property
            // Property Set TipoContratto(value)
            // Set m_TipoContratto = value
            // m_IdTipoContratto = value.ID
            // End Property

            // Property Get TipoRapporto
            // If (m_TipoRapporto Is Nothing) Then Set m_TipoRapporto = FsbPrev.GetTipoRapportoById(m_IdTipoRapporto)
            // Set TipoRapporto = m_TipoRapporto
            // End Property
            // Property Set TipoRapporto(value)
            // Set m_TipoRapporto = value
            // m_IdTipoRapporto = value.ID
            // End Property

            public string Categoria
            {
                get
                {
                    return m_Categoria;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Categoria;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Categoria = value;
                    DoChanged("Categoria", value, oldValue);
                }
            }

            // Property Get Banca
            // If (m_Banca Is Nothing) Then Set m_Banca = FsbPrev.GetBancaById(m_IDBanca)
            // Set Banca = m_Banca
            // End Property
            // Property Set Banca(value)
            // Set m_Banca = value
            // m_IDBanca = DBUtils.GetID(value, 0)
            // End Property

            /// <summary>
        /// Restituisce o imposta il nome del prodotto
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
        /// Restituisce o imposta la descrizione del prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore intero che indica le spese incluse nel calcolo del TEG
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public TipoCalcoloTEG TipoCalcoloTeg
            {
                get
                {
                    return (TipoCalcoloTEG)m_TipoCalcoloTEG;
                }

                set
                {
                    TipoCalcoloTEG oldValue = (TipoCalcoloTEG)m_TipoCalcoloTEG;
                    if (oldValue == value)
                        return;
                    m_TipoCalcoloTEG = (TEGCalcFlag)value;
                    DoChanged("TipoCalcoloTeg", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce il TEG massimo per l'offerta
        /// </summary>
        /// <param name="offerta"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public double GetMaxTEG(COffertaCQS offerta)
            {
                double GetMaxTEGRet = default;
                if (TabellaTEG is object)
                {
                    return TabellaTEG.Calculate(offerta);
                }
                else
                {
                    GetMaxTEGRet = 0d;
                }

                return GetMaxTEGRet;
            }

            /// <summary>
        /// Restituisce il TAEG massimo per l'offerta
        /// </summary>
        /// <param name="offerta"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public double GetMaxTAEG(COffertaCQS offerta)
            {
                if (TabellaTAEG is object)
                {
                    return TabellaTAEG.Calculate(offerta);
                }
                else
                {
                    return 0d;
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di inizio validità del prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataInizio
            {
                get
                {
                    return m_DataInizio;
                }

                set
                {
                    var oldValue = m_DataInizio;
                    if (oldValue == value == true)
                        return;
                    m_DataInizio = value;
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di fine validità del prodotto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataFine
            {
                get
                {
                    return m_DataFine;
                }

                set
                {
                    var oldValue = m_DataFine;
                    if (oldValue == value == true)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            public bool IsValid()
            {
                return IsValid(DMD.DateUtils.Now());
            }

            public bool IsValid(DateTime al)
            {
                return DMD.DateUtils.CheckBetween(al, m_DataInizio, m_DataFine);
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se le pratiche
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool RichiedeApprovazione
            {
                get
                {
                    return Sistema.TestFlag(m_Flags, ProdottoFlags.RequireApprovation);
                }

                set
                {
                    if (RichiedeApprovazione == value)
                        return;
                    m_Flags = Sistema.SetFlag(m_Flags, ProdottoFlags.RequireApprovation, value);
                    DoChanged("RichiedeApprovazione", value, !value);
                }
            }

            public override bool IsChanged()
            {
                bool ret = base.IsChanged();
                if (!ret && m_TabelleFinRelations is object)
                    ret = DBUtils.IsChanged(m_TabelleFinRelations);
                if (!ret && m_TabelleAssRelations is object)
                    ret = DBUtils.IsChanged(m_TabelleAssRelations);
                return ret;
            }

            protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            {
                bool ret = base.SaveToDatabase(dbConn, force);
                if (ret & m_TabelleFinRelations is object)
                    m_TabelleFinRelations.Save(force);
                if (ret & m_TabelleAssRelations is object)
                    m_TabelleAssRelations.Save(force);
                Prodotti.UpdateCached(this);
                return ret;
            }

            protected override bool DropFromDatabase(Databases.CDBConnection dbConn, bool force)
            {
                TabelleFinanziarieRelations.Delete(force);
                TabelleAssicurativeRelations.Delete(force);
                return base.DropFromDatabase(dbConn, force);
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override string GetTableName()
            {
                return "tbl_Prodotti";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_CessionarioID = reader.Read("Cessionario", this.m_CessionarioID);
                m_NomeCessionario = reader.Read("NomeCessionario", this.m_NomeCessionario);
                m_GruppoProdottiID = reader.Read("GruppoProdotti", this.m_GruppoProdottiID);
                m_IdTipoContratto = reader.Read("IdTipoContratto", this.m_IdTipoContratto);
                m_IdTipoRapporto = reader.Read("IdTipoRapporto", this.m_IdTipoRapporto);
                m_Categoria = reader.Read("Idcategoria", this.m_Categoria);
                // reader.Read("IDBanca", Me.m_IDBanca)
                m_Nome = reader.Read("nome", this.m_Nome);
                m_Descrizione = reader.Read("descrizione", this.m_Descrizione);
                m_TipoCalcoloTEG = reader.Read("TipoCalcoloTeg", this.m_TipoCalcoloTEG);
                m_TipoCalcoloTAEG = reader.Read("TipoCalcoloTAEG", this.m_TipoCalcoloTAEG);
                m_Visibile = reader.Read("Visibile", this.m_Visibile);
                m_IDTabellaSpese = reader.Read("IDTabellaSpese", this.m_IDTabellaSpese);
                m_IDTabellaTEG = reader.Read("IDTabellaTEG", this.m_IDTabellaTEG);
                m_IDTabellaTAEG = reader.Read("IDTabellaTAEG", this.m_IDTabellaTAEG);
                m_DataInizio = reader.Read("DataInizio", this.m_DataInizio);
                m_DataFine = reader.Read("DataFine", this.m_DataFine);
                m_ProvvigioneErogataDa = reader.Read("ProvvigioneErogataDa", this.m_ProvvigioneErogataDa);
                m_IDStatoIniziale = reader.Read("IDStatoIniziale", this.m_IDStatoIniziale);
                m_Flags = reader.Read("Flags", this.m_Flags);
                string attributiString = reader.Read("Attributi", "");
                if (!string.IsNullOrEmpty(attributiString))
                {
                    m_Attributi = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(attributiString);
                }

                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Visibile", m_Visibile);
                writer.Write("Cessionario", DBUtils.GetID(m_Cessionario, m_CessionarioID));
                writer.Write("NomeCessionario", m_NomeCessionario);
                writer.Write("GruppoProdotti", DBUtils.GetID(m_GruppoProdotti, m_GruppoProdottiID));
                writer.Write("IDTabellaSpese", IDTabellaSpese);
                writer.Write("IDTabellaTEG", IDTabellaTEG);
                writer.Write("IDTabellaTAEG", IDTabellaTAEG);
                writer.Write("IdTipoContratto", m_IdTipoContratto);
                writer.Write("IdTipoRapporto", m_IdTipoRapporto);
                writer.Write("Idcategoria", m_Categoria);
                // writer.Write("IDBanca", GetID(Me.m_Banca, Me.m_IDBanca))
                writer.Write("nome", m_Nome);
                writer.Write("descrizione", m_Descrizione);
                writer.Write("TipoCalcoloTeg", m_TipoCalcoloTEG);
                writer.Write("TipoCalcolotaeg", m_TipoCalcoloTAEG);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("ProvvigioneErogataDa", m_ProvvigioneErogataDa);
                writer.Write("IDStatoIniziale", IDStatoIniziale);
                writer.Write("Flags", m_Flags);
                writer.Write("Attributi", DMD.XML.Utils.Serializer.Serialize(Attributi));
                return base.SaveToRecordset(writer);
            }

            public override string ToString()
            {
                return m_Nome;
            }

            public int CompareTo(CCQSPDProdotto item)
            {
                return DMD.Strings.Compare(Nome, item.Nome, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CCQSPDProdotto)obj);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Visibile", m_Visibile);
                writer.WriteAttribute("CessionarioID", CessionarioID);
                writer.WriteAttribute("NomeCessionario", m_NomeCessionario);
                writer.WriteAttribute("GruppoProdottiID", GruppoProdottiID);
                writer.WriteAttribute("IdTipoContratto", m_IdTipoContratto);
                writer.WriteAttribute("IdTipoRapporto", m_IdTipoRapporto);
                writer.WriteAttribute("Categoria", m_Categoria);
                // writer.WriteTag("IDBanca", Me.m_IDBanca)
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("TipoCalcoloTEG", (int?)m_TipoCalcoloTEG);
                writer.WriteAttribute("ProvvigioneMassima", m_ProvvigioneMassima);
                writer.WriteAttribute("InBando", m_InBando);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("ProvvigioneErogataDa", (int?)m_ProvvigioneErogataDa);
                writer.WriteAttribute("TipoCalcoloTAEG", (int?)m_TipoCalcoloTAEG);
                writer.WriteAttribute("IDTabellaSpese", IDTabellaSpese);
                writer.WriteAttribute("IDTabellaTAEG", IDTabellaTAEG);
                writer.WriteAttribute("IDTabellaTEG", IDTabellaTEG);
                writer.WriteAttribute("IDStatoIniziale", IDStatoIniziale);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                base.XMLSerialize(writer);
                // writer.WriteTag("TabelleFinanziarieRelations", Me.TabelleFinanziarieRelations)
                // writer.WriteTag("TabelleAssicurativeRelations", Me.TabelleAssicurativeRelations)
                // writer.WriteTag("TabelleSpese", Me.TabelleSpese)
                writer.WriteTag("Attributi", Attributi);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Visibile":
                        {
                            m_Visibile = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "CessionarioID":
                        {
                            m_CessionarioID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCessionario":
                        {
                            m_NomeCessionario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "GruppoProdottiID":
                        {
                            m_GruppoProdottiID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IdTipoContratto":
                        {
                            m_IdTipoContratto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IdTipoRapporto":
                        {
                            m_IdTipoRapporto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Categoria":
                        {
                            m_Categoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    // Case "IDBanca" : Me.m_IDBanca = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoCalcoloTEG":
                        {
                            m_TipoCalcoloTEG = (TEGCalcFlag)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ProvvigioneMassima":
                        {
                            m_ProvvigioneMassima = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "InBando":
                        {
                            m_InBando = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "DataInizio":
                        {
                            m_DataInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFine":
                        {
                            m_DataFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ProvvigioneErogataDa":
                        {
                            m_ProvvigioneErogataDa = (ProvvigioneErogataDa)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoCalcoloTAEG":
                        {
                            m_TipoCalcoloTAEG = (TEGCalcFlag)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDTabellaSpese":
                        {
                            m_IDTabellaSpese = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDTabellaTAEG":
                        {
                            m_IDTabellaTAEG = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDTabellaTEG":
                        {
                            m_IDTabellaTEG = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDStatoIniziale":
                        {
                            m_IDStatoIniziale = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (ProdottoFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Attributi":
                        {
                            m_Attributi = (CKeyCollection)fieldValue;
                            break;
                        }

                    case "TabelleFinanziarieRelations":
                        {
                            m_TabelleFinRelations = new CTabelleFinanziarieProdottoCollection();
                            m_TabelleFinRelations.SetProdotto(this);
                            m_TabelleFinRelations.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "TabelleAssicurativeRelations":
                        {
                            m_TabelleAssRelations = new CTabelleAssicurativeProdottoCollection();
                            m_TabelleAssRelations.SetProdotto(this);
                            m_TabelleAssRelations.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "TabelleSpese":
                        {
                            m_TabelleSpese = new CProdottoXTabellaSpesaCollection();
                            m_TabelleSpese.SetProdotto(this);
                            m_TabelleSpese.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Convenzioni":
                        {
                            m_Convenzioni = new CProdottoXConvenzioneCollection();
                            m_Convenzioni.SetProdotto(this);
                            m_Convenzioni.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public void InvalidateTabelleSpese()
            {
                lock (this)
                    m_TabelleSpese = null;
            }

            public void InvalidateAssicurazioni()
            {
                lock (this)
                    m_TabelleAssRelations = null;
            }

            public override void InitializeFrom(object value)
            {
                base.InitializeFrom(value);
                m_TabelleAssRelations = null;
                m_TabelleFinRelations = null;
                m_TabelleSpese = null;
                m_Convenzioni = null;
            }

            public object Clone()
            {
                return MemberwiseClone();
            }

            protected override void ResetID()
            {
                base.ResetID();
                m_Assicurazioni = null;
                m_TabelleFinRelations = null;
                m_TabelleAssRelations = null;
                m_Convenzioni = null;
                m_Attributi = new CKeyCollection();
                m_TabelleSpese = null;
            }
        }
    }
}