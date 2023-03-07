using System;
using minidom;
using DMD.XML;

namespace minidom
{
    public partial class Finanziaria
    {
        public enum StatoTrattativa : int
        {
            /// <summary>
        /// 'L'operatore non ha proposto il prodotto
        /// </summary>
        /// <remarks></remarks>
            STATO_NONPROPOSTO = 0,

            /// <summary>
        /// L'operatore ha proposto il prodotto ed il produttore ha formulato le sue richieste
        /// </summary>
        /// <remarks></remarks>
            STATO_PROPOSTA = 1,

            /// <summary>
        /// L'operatore ha proposto il prodotto e si sta attendendo l'esito della valutazione
        /// </summary>
        /// <remarks></remarks>
            STATO_ATTESAAPPROVAZIONE = 3,

            /// <summary>
        /// Il supervisore ha approvato le richieste del produttore
        /// </summary>
        /// <remarks></remarks>
            STATO_APPROVATO = 5,

            /// <summary>
        /// Il supervisore non ha approvato le richieste del produttore
        /// </summary>
        /// <remarks></remarks>
            STATO_NONAPPROVATO = 7,

            /// <summary>
        /// Il produttore ha accettato l'offerta così come è stata approvata dal supervisore
        /// </summary>
        /// <remarks></remarks>
            STATO_ACCETTATO = 9,

            /// <summary>
        /// Il produttore non ha accettato l'ultima offerta
        /// </summary>
        /// <remarks></remarks>
            STATO_NONACCETTATO = 11,

            /// <summary>
        /// La trattativa é stata precedentemente accettata ma é scaduta o non più valida
        /// </summary>
            STATO_RITIRATA = 15
        }

        [Flags]
        public enum TrattativaCollaboratoreFlags : int
        {
            None = 0,

            /// <summary>
        /// La provvigione é visibile solo agli utenti privilegiati
        /// </summary>
            Nascosta = 1,

            /// <summary>
        /// La provvigione si applica solo in caso di produzione diretta del collaboratore
        /// </summary>
            SoloDirettaCollaboratore = 2
        }

        [Serializable]
        public class CTrattativaCollaboratore : Databases.DBObject
        {
            private string m_Nome;
            private bool m_Richiesto;
            [NonSerialized]
            private CCollaboratore m_Collaboratore;
            private int m_IDCollaboratore;
            private string m_NomeCollaboratore;
            private int m_IDCessionario;
            [NonSerialized]
            private CCQSPDCessionarioClass m_Cessionario;
            private string m_NomeCessionario;
            private int m_IDProdotto;
            [NonSerialized]
            private CCQSPDProdotto m_Prodotto;
            private string m_NomeProdotto;
            private StatoTrattativa m_StatoTrattativa;
            [NonSerialized]
            private Sistema.CUser m_PropostoDa;
            private int m_IDPropostoDa;
            private DateTime? m_PropostoIl;
            private double? m_SpreadProposto;
            private double? m_SpreadRichiesto;
            private double? m_SpreadApprovato;
            private double? m_SpreadFidelizzazione;
            private decimal? m_ValoreBase;
            private decimal? m_ValoreMax;
            private string m_Formula;
            private CQSPDTipoProvvigioneEnum m_TipoCalcolo;
            private int m_IDApprovatoDa;
            [NonSerialized]
            private Sistema.CUser m_ApprovatoDa;
            private DateTime? m_ApprovatoIl;
            private string m_Note;
            private TrattativaCollaboratoreFlags m_Flags;
            private CKeyCollection m_Attributi;

            public CTrattativaCollaboratore()
            {
                m_Nome = "";
                m_Richiesto = false;
                m_Collaboratore = null;
                m_IDCollaboratore = 0;
                m_NomeCollaboratore = "";
                m_IDCessionario = 0;
                m_Cessionario = null;
                m_NomeCessionario = "";
                m_IDProdotto = 0;
                m_Prodotto = null;
                m_NomeProdotto = "";
                m_StatoTrattativa = StatoTrattativa.STATO_ACCETTATO;
                m_PropostoDa = null;
                m_IDPropostoDa = 0;
                m_PropostoIl = default;
                m_SpreadProposto = default;
                m_SpreadRichiesto = default;
                m_SpreadApprovato = default;
                m_SpreadFidelizzazione = default;
                m_ValoreBase = default;
                m_ValoreMax = default;
                m_Formula = "";
                m_TipoCalcolo = CQSPDTipoProvvigioneEnum.PercentualeSuMontanteLordo;
                m_IDApprovatoDa = 0;
                m_ApprovatoDa = null;
                m_ApprovatoIl = default;
                m_Note = "";
                m_Flags = TrattativaCollaboratoreFlags.None;
                m_Attributi = null;
            }

            public override CModulesClass GetModule()
            {
                return TrattativeCollaboratore.Module;
            }

            /// <summary>
        /// Restituisce o imposta il nome della trattativa
        /// </summary>
        /// <returns></returns>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta una percentuale di fidelizzazione
        /// </summary>
        /// <returns></returns>
            public double? SpreadFidelizzazione
            {
                get
                {
                    return m_SpreadFidelizzazione;
                }

                set
                {
                    var oldValue = m_SpreadFidelizzazione;
                    if (oldValue == value == true)
                        return;
                    m_SpreadFidelizzazione = value;
                    DoChanged("SpreadFidelizzazione", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la provvigione massima in euro
        /// </summary>
        /// <returns></returns>
            public decimal? ValoreMax
            {
                get
                {
                    return m_ValoreMax;
                }

                set
                {
                    var oldValue = m_ValoreMax;
                    if (oldValue == value == true)
                        return;
                    m_ValoreMax = value;
                    DoChanged("ValoreMax", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il valore base riconosciuto per il prodotto
        /// </summary>
        /// <returns></returns>
            public decimal? ValoreBase
            {
                get
                {
                    return m_ValoreBase;
                }

                set
                {
                    var oldValue = m_ValoreBase;
                    if (value == oldValue == true)
                        return;
                    m_ValoreBase = value;
                    DoChanged("ValoreBase", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la formula con cui viene calcolata la provvigione
        /// </summary>
        /// <returns></returns>
            public string Formula
            {
                get
                {
                    return m_Formula;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Formula;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Formula = value;
                    DoChanged("Formula", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il tipo di calcolo utilizzato per il prodotto
        /// </summary>
        /// <returns></returns>
            public CQSPDTipoProvvigioneEnum TipoCalcolo
            {
                get
                {
                    return m_TipoCalcolo;
                }

                set
                {
                    var oldValue = m_TipoCalcolo;
                    if (oldValue == value)
                        return;
                    m_TipoCalcolo = value;
                    DoChanged("TipoCalcolo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta dei flags aggiuntivi
        /// </summary>
        /// <returns></returns>
            public TrattativaCollaboratoreFlags Flags
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
        /// Restituisce dei parametri aggiuntivi
        /// </summary>
        /// <returns></returns>
            public CKeyCollection Attributi
            {
                get
                {
                    if (m_Attributi is null)
                        m_Attributi = new CKeyCollection();
                    return m_Attributi;
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore che indica se il prodotto
        /// </summary>
        /// <returns></returns>
            public bool Richiesto
            {
                get
                {
                    return m_Richiesto;
                }

                set
                {
                    if (m_Richiesto == value)
                        return;
                    m_Richiesto = value;
                    DoChanged("Richiesto", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del collaboratore associato
        /// </summary>
        /// <returns></returns>
            public int IDCollaboratore
            {
                get
                {
                    return DBUtils.GetID(m_Collaboratore, m_IDCollaboratore);
                }

                set
                {
                    int oldValue = IDCollaboratore;
                    if (oldValue == value)
                        return;
                    m_IDCollaboratore = value;
                    m_Collaboratore = null;
                    DoChanged("IDCollaboratore", value, oldValue);
                }
            }

            /// <summary>
        /// Restituosce o imposta il collaboratore associato
        /// </summary>
        /// <returns></returns>
            public CCollaboratore Collaboratore
            {
                get
                {
                    if (m_Collaboratore is null)
                        m_Collaboratore = Collaboratori.GetItemById(m_IDCollaboratore);
                    return m_Collaboratore;
                }

                set
                {
                    var oldValue = Collaboratore;
                    if (oldValue == value)
                        return;
                    m_Collaboratore = value;
                    m_IDCollaboratore = DBUtils.GetID(value);
                    m_NomeCollaboratore = "";
                    if (value is object)
                        m_NomeCollaboratore = value.NomePersona;
                    DoChanged("Collaboratore", value, oldValue);
                }
            }

            protected internal void SetCollaboratore(CCollaboratore value)
            {
                m_Collaboratore = value;
                m_IDCollaboratore = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta il nome del collaboratore
        /// </summary>
        /// <returns></returns>
            public string NomeCollaboratore
            {
                get
                {
                    return m_NomeCollaboratore;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeCollaboratore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCollaboratore = value;
                    DoChanged("NomeCollaboratore", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del prodotto associato
        /// </summary>
        /// <returns></returns>
            public int IDProdotto
            {
                get
                {
                    return DBUtils.GetID(m_Prodotto, m_IDProdotto);
                }

                set
                {
                    int oldValue = IDProdotto;
                    if (oldValue == value)
                        return;
                    m_IDProdotto = value;
                    m_Prodotto = null;
                    DoChanged("IDProdotto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il prodotto associato
        /// </summary>
        /// <returns></returns>
            public CCQSPDProdotto Prodotto
            {
                get
                {
                    if (m_Prodotto is null)
                        m_Prodotto = Prodotti.GetItemById(m_IDProdotto);
                    return m_Prodotto;
                }

                set
                {
                    var oldValue = Prodotto;
                    if (oldValue == value)
                        return;
                    m_Prodotto = value;
                    m_IDProdotto = DBUtils.GetID(value);
                    m_NomeProdotto = "";
                    if (value is object)
                        m_NomeProdotto = value.Descrizione;
                    DoChanged("Prodotto", value, oldValue);
                }
            }

            protected internal void SetProdotto(CCQSPDProdotto value)
            {
                m_Prodotto = value;
                m_IDProdotto = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta il nome del prodotto
        /// </summary>
        /// <returns></returns>
            public string NomeProdotto
            {
                get
                {
                    return m_NomeProdotto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeProdotto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeProdotto = value;
                    DoChanged("NomeProdotto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta delle note aggiuntive
        /// </summary>
        /// <returns></returns>
            public string Note
            {
                get
                {
                    return m_Note;
                }

                set
                {
                    string oldValue = m_Note;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Note = value;
                    DoChanged("Note", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del cessionario
        /// </summary>
        /// <returns></returns>
            public int IDCessionario
            {
                get
                {
                    return DBUtils.GetID(m_Cessionario, m_IDCessionario);
                }

                set
                {
                    int oldValue = IDCessionario;
                    if (oldValue == value)
                        return;
                    m_IDCessionario = value;
                    m_Cessionario = null;
                    DoChanged("IDCessionario", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il cessionario
        /// </summary>
        /// <returns></returns>
            public CCQSPDCessionarioClass Cessionario
            {
                get
                {
                    if (m_Cessionario is null)
                        m_Cessionario = Cessionari.GetItemById(m_IDCessionario);
                    return m_Cessionario;
                }

                set
                {
                    var oldValue = Cessionario;
                    if (oldValue == value)
                        return;
                    m_Cessionario = value;
                    m_IDCessionario = DBUtils.GetID(value);
                    m_NomeCessionario = "";
                    if (value is object)
                        m_NomeCessionario = value.Nome;
                    DoChanged("Cessionario", value, oldValue);
                }
            }

            protected internal void SetCessionario(CCQSPDCessionarioClass value)
            {
                m_Cessionario = value;
                m_IDCessionario = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce o imposta il nome del cessionario
        /// </summary>
        /// <returns></returns>
            public string NomeCessionario
            {
                get
                {
                    return m_NomeCessionario;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeCessionario;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCessionario = value;
                    DoChanged("NomeCessionario", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data in cui il prodotto é stato proposto/richiesto
        /// </summary>
        /// <returns></returns>
            public DateTime? PropostoIl
            {
                get
                {
                    return m_PropostoIl;
                }

                set
                {
                    var oldValue = m_PropostoIl;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_PropostoIl = value;
                    DoChanged("PropostoIl", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'utente che ha proposto il prodotto o che ne ha registrato la richiesta
        /// </summary>
        /// <returns></returns>
            public int IDPropostoDa
            {
                get
                {
                    return DBUtils.GetID(m_PropostoDa, m_IDPropostoDa);
                }

                set
                {
                    int oldValue = IDPropostoDa;
                    if (oldValue == value)
                        return;
                    m_PropostoDa = null;
                    m_IDPropostoDa = value;
                    DoChanged("IDPropostoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha proposto il prodotto o che ne ha registrato la richiesta
        /// </summary>
        /// <returns></returns>
            public Sistema.CUser PropostoDa
            {
                get
                {
                    if (m_PropostoDa is null)
                        m_PropostoDa = Sistema.Users.GetItemById(m_IDPropostoDa);
                    return m_PropostoDa;
                }

                set
                {
                    var oldValue = PropostoDa;
                    if (oldValue == value)
                        return;
                    m_PropostoDa = value;
                    m_IDPropostoDa = DBUtils.GetID(value);
                    DoChanged("PropostoDa", value, oldValue);
                }
            }

            protected internal void SetPropostoDa(Sistema.CUser value)
            {
                m_PropostoDa = value;
                m_IDPropostoDa = DBUtils.GetID(value);
            }

            /// <summary>
        /// Restituisce il nome dell'utente che ha proposto il prodotto o che ne ha registrato la richiesta
        /// </summary>
        /// <returns></returns>
            public string NomePropostoDa
            {
                get
                {
                    if (PropostoDa is null)
                        return "UserID: " + IDPropostoDa;
                    return PropostoDa.Nominativo;
                }
            }

            /// <summary>
        /// Restituisce o imposta lo stato della trattativa pre il prodotto
        /// </summary>
        /// <returns></returns>
            public StatoTrattativa StatoTrattativa
            {
                get
                {
                    return m_StatoTrattativa;
                }

                set
                {
                    var oldValue = m_StatoTrattativa;
                    if (oldValue == value)
                        return;
                    m_StatoTrattativa = value;
                    DoChanged("StatoTrattativa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta una stringa che indica lo stato della trattativa per il prodotto
        /// </summary>
        /// <returns></returns>
            public string StatoTrattativaEx
            {
                get
                {
                    return TrattativeCollaboratore.FormatStato(StatoTrattativa);
                }

                set
                {
                    StatoTrattativa = TrattativeCollaboratore.ParseStato(value);
                }
            }

            /// <summary>
        /// Restituisce o imposta la provvigione max in percentuale (proposta)
        /// </summary>
        /// <returns></returns>
            public double? SpreadProposto
            {
                get
                {
                    return m_SpreadProposto;
                }

                set
                {
                    var oldValue = m_SpreadProposto;
                    if (oldValue == value == true)
                        return;
                    m_SpreadProposto = value;
                    DoChanged("SpreadProposto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la provvigione max in percentuale (richiesta)
        /// </summary>
        /// <returns></returns>
            public double? SpreadRichiesto
            {
                get
                {
                    return m_SpreadRichiesto;
                }

                set
                {
                    var oldValue = m_SpreadRichiesto;
                    if (oldValue == value == true)
                        return;
                    m_SpreadRichiesto = value;
                    DoChanged("SpreadRichiesto", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la provvigione max in % (concordata)
        /// </summary>
        /// <returns></returns>
            public double? SpreadApprovato
            {
                get
                {
                    return m_SpreadApprovato;
                }

                set
                {
                    var oldValue = m_SpreadApprovato;
                    if (oldValue == value == true)
                        return;
                    m_SpreadApprovato = value;
                    DoChanged("SpreadApprovato", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'utente che ha convalidato la proposta
        /// </summary>
        /// <returns></returns>
            public Sistema.CUser ApprovatoDa
            {
                get
                {
                    if (m_ApprovatoDa is null)
                        m_ApprovatoDa = Sistema.Users.GetItemById(m_IDApprovatoDa);
                    return m_ApprovatoDa;
                }

                set
                {
                    var oldValue = ApprovatoDa;
                    if (oldValue == value)
                        return;
                    m_ApprovatoDa = value;
                    m_IDApprovatoDa = DBUtils.GetID(value);
                    DoChanged("ApprovatoDa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID dell'utente che ha convalidato la proposta
        /// </summary>
        /// <returns></returns>
            public int IDApprovatoDa
            {
                get
                {
                    return DBUtils.GetID(m_ApprovatoDa, m_IDApprovatoDa);
                }

                set
                {
                    int oldValue = IDApprovatoDa;
                    if (oldValue == value)
                        return;
                    m_IDApprovatoDa = value;
                    m_ApprovatoDa = null;
                    DoChanged("IDApprovatoDa", value, oldValue);
                }
            }

            public string NomeApprovatoDa
            {
                get
                {
                    if (ApprovatoDa is null)
                        return "UserID: " + m_IDApprovatoDa;
                    return ApprovatoDa.Nominativo;
                }
            }


            /// <summary>
        /// Restituisce o imposta la data in cui il prodotto é stato assegnato o negato al collaboratore
        /// </summary>
        /// <returns></returns>
            public DateTime? ApprovatoIl
            {
                get
                {
                    return m_ApprovatoIl;
                }

                set
                {
                    var oldValue = m_ApprovatoIl;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_ApprovatoIl = value;
                    DoChanged("ApprovatoIl", value, oldValue);
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override void Save(bool force = false)
            {
                base.Save(force);
                Collaboratori.InvalidateTrattative();
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDCollabBUI";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", this. m_Nome);
                m_Richiesto = reader.Read("Richiesto", this.m_Richiesto);
                m_IDCollaboratore = reader.Read("IDCollaboratore", this.m_IDCollaboratore);
                m_NomeCollaboratore = reader.Read("NomeCollaboratore", this.m_NomeCollaboratore);
                m_IDCessionario = reader.Read("IDCessionario", this.m_IDCessionario);
                m_NomeCessionario = reader.Read("NomeCessionario", this.m_NomeCessionario);
                m_IDProdotto = reader.Read("IDProdotto", this.m_IDProdotto);
                m_NomeProdotto = reader.Read("NomeProdotto", this.m_NomeProdotto);
                m_StatoTrattativa = reader.Read("StatoTrattativa", this.m_StatoTrattativa);
                m_SpreadProposto = reader.Read("SpreadProposto", this.m_SpreadProposto);
                m_SpreadRichiesto = reader.Read("SpreadRichiesto", this.m_SpreadRichiesto);
                m_SpreadApprovato = reader.Read("SpreadApprovato", this.m_SpreadApprovato);
                m_SpreadFidelizzazione = reader.Read("SpreadFidelizzazione", this.m_SpreadFidelizzazione);
                m_Flags = reader.Read("Flags", this.m_Flags);
                m_ValoreBase = reader.Read("ValoreBase", this.m_ValoreBase);
                m_ValoreMax = reader.Read("ValoreMax", this.m_ValoreMax);
                m_Formula = reader.Read("Formula", this.m_Formula);
                string tmp = reader.Read("Attributi", "");
                if (!string.IsNullOrEmpty(tmp))
                    m_Attributi = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);

                m_IDPropostoDa = reader.Read("IDPropostoDa", this.m_IDPropostoDa);
                m_PropostoIl = reader.Read("PropostoIl", this.m_PropostoIl);
                m_IDApprovatoDa = reader.Read("IDApprovatoDa", this.m_IDApprovatoDa);
                m_ApprovatoIl = reader.Read("ApprovatoIl", this.m_ApprovatoIl);
                m_Note = reader.Read("Note", this.m_Note);
                m_TipoCalcolo = reader.Read("TipoCalcolo", this.m_TipoCalcolo);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("Richiesto", m_Richiesto);
                writer.Write("IDCollaboratore", IDCollaboratore);
                writer.Write("NomeCollaboratore", m_NomeCollaboratore);
                writer.Write("IDCessionario", IDCessionario);
                writer.Write("NomeCessionario", m_NomeCessionario);
                writer.Write("IDProdotto", IDProdotto);
                writer.Write("NomeProdotto", m_NomeProdotto);
                writer.Write("StatoTrattativa", m_StatoTrattativa);
                writer.Write("SpreadProposto", m_SpreadProposto);
                writer.Write("SpreadRichiesto", m_SpreadRichiesto);
                writer.Write("SpreadApprovato", m_SpreadApprovato);
                writer.Write("SpreadFidelizzazione", m_SpreadFidelizzazione);
                writer.Write("Flags", m_Flags);
                writer.Write("ValoreBase", m_ValoreBase);
                writer.Write("ValoreMax", m_ValoreMax);
                writer.Write("Formula", m_Formula);
                writer.Write("Attributi", DMD.XML.Utils.Serializer.Serialize(Attributi));
                writer.Write("IDPropostoDa", IDPropostoDa);
                writer.Write("PropostoIl", m_PropostoIl);
                writer.Write("IDApprovatoDa", IDApprovatoDa);
                writer.Write("ApprovatoIl", m_ApprovatoIl);
                writer.Write("TipoCalcolo", m_TipoCalcolo);
                writer.Write("Note", m_Note);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Richiesto", m_Richiesto);
                writer.WriteAttribute("IDCollaboratore", IDCollaboratore);
                writer.WriteAttribute("NomeCollaboratore", m_NomeCollaboratore);
                writer.WriteAttribute("IDCessionario", IDCessionario);
                writer.WriteAttribute("NomeCessionario", m_NomeCessionario);
                writer.WriteAttribute("IDProdotto", IDProdotto);
                writer.WriteAttribute("NomeProdotto", m_NomeProdotto);
                writer.WriteAttribute("StatoTrattativa", (int?)m_StatoTrattativa);
                writer.WriteAttribute("SpreadProposto", m_SpreadProposto);
                writer.WriteAttribute("SpreadRichiesto", m_SpreadRichiesto);
                writer.WriteAttribute("SpreadApprovato", m_SpreadApprovato);
                writer.WriteAttribute("SpreadFidelizzazione", m_SpreadFidelizzazione);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                writer.WriteAttribute("ValoreBase", m_ValoreBase);
                writer.WriteAttribute("ValoreMax", m_ValoreMax);
                writer.WriteAttribute("IDPropostoDa", IDPropostoDa);
                writer.WriteAttribute("PropostoIl", m_PropostoIl);
                writer.WriteAttribute("IDApprovatoDa", IDApprovatoDa);
                writer.WriteAttribute("ApprovatoIl", m_ApprovatoIl);
                writer.WriteAttribute("TipoCalcolo", (int?)m_TipoCalcolo);
                base.XMLSerialize(writer);
                writer.WriteTag("Attributi", Attributi);
                writer.WriteTag("Formula", m_Formula);
                writer.WriteTag("Note", m_Note);
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

                    case "Richiesto":
                        {
                            m_Richiesto = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "IDCollaboratore":
                        {
                            m_IDCollaboratore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCollaboratore":
                        {
                            m_NomeCollaboratore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDCessionario":
                        {
                            m_IDCessionario = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCessionario":
                        {
                            m_NomeCessionario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDProdotto":
                        {
                            m_IDProdotto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeProdotto":
                        {
                            m_NomeProdotto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoTrattativa":
                        {
                            m_StatoTrattativa = (StatoTrattativa)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "SpreadProposto":
                        {
                            m_SpreadProposto = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "SpreadRichiesto":
                        {
                            m_SpreadRichiesto = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "SpreadApprovato":
                        {
                            m_SpreadApprovato = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "SpreadFidelizzazione":
                        {
                            m_SpreadFidelizzazione = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (TrattativaCollaboratoreFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ValoreBase":
                        {
                            m_ValoreBase = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ValoreMax":
                        {
                            m_ValoreMax = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Formula":
                        {
                            m_Formula = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDPropostoDa":
                        {
                            m_IDPropostoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "PropostoIl":
                        {
                            m_PropostoIl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDApprovatoDa":
                        {
                            m_IDApprovatoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ApprovatoIl":
                        {
                            m_ApprovatoIl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Attributi":
                        {
                            m_Attributi = (CKeyCollection)fieldValue;
                            break;
                        }

                    case "TipoCalcolo":
                        {
                            m_TipoCalcolo = (CQSPDTipoProvvigioneEnum)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public override string ToString()
            {
                var ret = new System.Text.StringBuilder();
                ret.Append("{ ");
                ret.Append(NomeCollaboratore);
                ret.Append(", ");
                ret.Append(NomeProdotto);
                ret.Append(" }");
                return ret.ToString();
            }
        }
    }
}