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

        /// <summary>
        /// Rappresenta uno stato di lavorazione di una pratica cioè un passaggio di stato
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CStatoLavorazionePratica 
            : Databases.DBObject, IComparable, ICloneable, IComparable<CStatoLavorazionePratica>
        {
            private StatoPraticaEnum? m_MacroStato; // Macrostato
            private int m_IDPratica;                  // ID della pratica che ha subito il passaggio
            [NonSerialized] private CPraticaCQSPD m_Pratica;                // Pratica che ha subito il passaggio
            private int m_IDFromStato;                // ID dello stato precedente
            [NonSerialized] private CStatoLavorazionePratica m_FromStato; // Stato precedente
            [NonSerialized] private CStatoPratRule m_RegolaApplicata;     // Regola applicata
            private int m_IDRegolaApplicata;          // ID della regola applicata
            private string m_NomeRegolaApplicata;         // Nome della regola applicata
            private int m_IDStatoPratica;             // ID dello stato successivo
            private string m_DescrizioneStato;            // Descrizione dello stato attuale
            [NonSerialized] private CStatoPratica m_StatoPratica;         // Stato successivo
            private DateTime? m_Data;             // Data ed ora del passaggio di stato
            private int m_IDOperatore;                // ID dell'operatore che ha effettuato il passaggio
            [NonSerialized] private CUser m_Operatore;                    // Operatore che ha effettuato il passaggio
            private string m_NomeOperatore;               // Nome dell'operatore che ha effettuato il passaggio
            private string m_Note;                        // Annotazioni sul passaggio
            private string m_Params;                      // Parametri del passaggio di stato
            private bool m_Forzato;                    // Vero se il passaggio è stato forzato a dispetto dei vincoli
            private string m_Attachment;                  // 
            private int m_IDOfferta;                  // ID dell'offerta corrente al momento del passaggio
            [NonSerialized] private COffertaCQS m_Offerta;                // Offerta corrente al momento del passaggio

            /// <summary>
            /// Costruttore
            /// </summary>
            public CStatoLavorazionePratica()
            {
                m_MacroStato = default;
                m_IDPratica = 0;
                m_Pratica = null;
                m_IDFromStato = 0;
                m_FromStato = null;
                m_IDStatoPratica = 0;
                m_StatoPratica = null;
                m_Data = default;
                m_IDOperatore = 0;
                m_Operatore = null;
                m_NomeOperatore = DMD.Strings.vbNullString;
                m_Note = DMD.Strings.vbNullString;
                m_Params = DMD.Strings.vbNullString;
                m_Forzato = false;
                m_Attachment = DMD.Strings.vbNullString;
                m_IDOfferta = 0;
                m_Offerta = null;
                m_DescrizioneStato = DMD.Strings.vbNullString;
                m_RegolaApplicata = null;
                m_IDRegolaApplicata = 0;
                m_NomeRegolaApplicata = "";
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="macroStato"></param>
            public CStatoLavorazionePratica(StatoPraticaEnum macroStato) 
                : this()
            {
                m_MacroStato = macroStato;
            }

            /// <summary>
            /// ID della regola applicata
            /// </summary>
            public int IDRegolaApplicata
            {
                get
                {
                    return DBUtils.GetID(m_RegolaApplicata, m_IDRegolaApplicata);
                }

                set
                {
                    int oldValue = IDRegolaApplicata;
                    if (oldValue == value)
                        return;
                    m_IDRegolaApplicata = value;
                    m_RegolaApplicata = null;
                    DoChanged("IDRegolaApplicata", value, oldValue);
                }
            }

            /// <summary>
            /// Regola applicata
            /// </summary>
            public CStatoPratRule RegolaApplicata
            {
                get
                {
                    if (m_RegolaApplicata is null)
                        m_RegolaApplicata = StatiPratRules.GetItemById(m_IDRegolaApplicata);
                    return m_RegolaApplicata;
                }

                set
                {
                    var oldValue = m_RegolaApplicata;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_RegolaApplicata = value;
                    m_IDRegolaApplicata = DBUtils.GetID(value);
                    m_NomeRegolaApplicata = "";
                    if (value is object)
                        m_NomeRegolaApplicata = value.Nome;
                    DoChanged("RegolaApplicata", value, oldValue);
                }
            }

            /// <summary>
            /// Nome della regola applicata
            /// </summary>
            public string NomeRegolaApplicata
            {
                get
                {
                    return m_NomeRegolaApplicata;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeRegolaApplicata;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeRegolaApplicata = value;
                    DoChanged("NomeRegolaApplicata", value, oldValue);
                }
            }

            /// <summary>
            /// Descrizione dello stato
            /// </summary>
            public string DescrizioneStato
            {
                get
                {
                    return m_DescrizioneStato;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_DescrizioneStato;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DescrizioneStato = value;
                    DoChanged("DescrizioneStato", value, oldValue);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return Finanziaria.StatiLavorazionePratica;
            }

            /// <summary>
            /// ID dello stato di provenienza
            /// </summary>
            public int IDFromStato
            {
                get
                {
                    return DBUtils.GetID(m_FromStato, m_IDFromStato);
                }

                set
                {
                    int oldValue = IDFromStato;
                    if (oldValue == value)
                        return;
                    m_IDFromStato = value;
                    m_FromStato = null;
                    DoChanged("IDFromStato", value, oldValue);
                }
            }

            /// <summary>
            /// Stato di provenienza
            /// </summary>
            public CStatoLavorazionePratica FromStato
            {
                get
                {
                    if (m_FromStato is null)
                        m_FromStato = StatiLavorazionePratica.GetItemById(m_IDFromStato);
                    return m_FromStato;
                }

                set
                {
                    var oldValue = m_FromStato;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_FromStato = value;
                    m_IDFromStato = DBUtils.GetID(value);
                    DoChanged("FromStato", value, oldValue);
                }
            }

            /// <summary>
            /// ID dello stato di destinazione
            /// </summary>
            public int IDStatoPratica
            {
                get
                {
                    return DBUtils.GetID(m_StatoPratica, m_IDStatoPratica);
                }

                set
                {
                    int oldValue = IDStatoPratica;
                    if (oldValue == value)
                        return;
                    m_IDStatoPratica = value;
                    m_StatoPratica = null;
                    DoChanged("IDStatoPratica", value, oldValue);
                }
            }

            /// <summary>
            /// Stato di destinazione
            /// </summary>
            public CStatoPratica StatoPratica
            {
                get
                {
                    if (m_StatoPratica is null)
                        m_StatoPratica = StatiPratica.GetItemById(m_IDStatoPratica);
                    return m_StatoPratica;
                }

                set
                {
                    var oldValue = m_StatoPratica;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_StatoPratica = value;
                    m_IDStatoPratica = DBUtils.GetID(value);
                    if (value is object)
                        m_DescrizioneStato = value.Nome;
                    DoChanged("StatoPratica", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore numerico che indica lo stato della pratica
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoPraticaEnum? MacroStato
            {
                get
                {
                    return m_MacroStato;
                }

                set
                {
                    var oldValue = m_MacroStato;
                    if (oldValue == value == true)
                        return;
                    m_MacroStato = value;
                    DoChanged("MacroStato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della pratica associata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDPratica
            {
                get
                {
                    return DBUtils.GetID(m_Pratica, m_IDPratica);
                }

                set
                {
                    int oldValue = IDPratica;
                    if (oldValue == value)
                        return;
                    m_IDPratica = value;
                    m_Pratica = null;
                    DoChanged("IDPratica", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'oggetto pratica a cui è associato lo stato di lavorazione corrente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CPraticaCQSPD Pratica
            {
                get
                {
                    if (m_Pratica is null)
                        m_Pratica = Pratiche.GetItemById(m_IDPratica);
                    return m_Pratica;
                }

                set
                {
                    var oldValue = Pratica;
                    if (oldValue == value)
                        return;
                    m_Pratica = value;
                    m_IDPratica = DBUtils.GetID(value);
                    DoChanged("Pratica", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data in cui la pratica è passata nello stato di lavorazione indicato da questo oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? Data
            {
                get
                {
                    return m_Data;
                }

                set
                {
                    var oldValue = m_Data;
                    if (oldValue == value == true)
                        return;
                    m_Data = value;
                    DoChanged("Data", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che ha messo la pratica nello stato di lavorazione rappresentato da questo oggetto
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
            /// Restituisce o imposta un oggetto CUser che rappresenta l'utente che ha messo la pratica nello stato di lavorazione rappresentato da questo oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CUser Operatore
            {
                get
                {
                    if (m_Operatore is null)
                        m_Operatore = Sistema.Users.GetItemById(m_IDOperatore);
                    return m_Operatore;
                }

                set
                {
                    var oldValue = Operatore;
                    if (oldValue == value)
                        return;
                    m_Operatore = value;
                    m_IDOperatore = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeOperatore = value.Nominativo;
                    DoChanged("Operatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'operatore che ha messo la pratica nello stato di lavorazione rappresentato da questo oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeOperatore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatore = value;
                    DoChanged("NomeOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituise o imposta una stringa usata come campo note
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
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
            /// Restituisce o imposta una stringa usata come campo parametri
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Params
            {
                get
                {
                    return m_Params;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Params;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Params = value;
                    DoChanged("Params", value, oldValue);
                }
            }

            /// <summary>
            /// URL dell'allegato
            /// </summary>
            public string Attachment
            {
                get
                {
                    return m_Attachment;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Attachment;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Attachment = value;
                    DoChanged("Attachment", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se lo stato è stato forzato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Forzato
            {
                get
                {
                    return m_Forzato;
                }

                set
                {
                    if (m_Forzato == value)
                        return;
                    m_Forzato = value;
                    DoChanged("Forzato", value, !value);
                }
            }

            /// <summary>
            /// ID dell'offerta attiva nel momento del passaggio di stato
            /// </summary>
            public int IDOfferta
            {
                get
                {
                    return DBUtils.GetID(m_Offerta, m_IDOfferta);
                }

                set
                {
                    int oldValue = IDOfferta;
                    if (value == oldValue)
                        return;
                    m_IDOfferta = value;
                    m_Offerta = null;
                    DoChanged("IDOfferta", value, oldValue);
                }
            }

            /// <summary>
            /// Offerta attiva nel momento del passaggio di stato
            /// </summary>
            public COffertaCQS Offerta
            {
                get
                {
                    if (m_Offerta is null)
                        m_Offerta = Offerte.GetItemById(m_IDOfferta);
                    return m_Offerta;
                }

                set
                {
                    if (ReferenceEquals(value, m_Offerta))
                        return;
                    m_Offerta = value;
                    m_IDOfferta = DBUtils.GetID(value);
                    DoChanged("Offerta", value);
                }
            }

            /// <summary>
            /// Imposta l'offerta attiva nel momento del passaggio di stato
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetOfferta(COffertaCQS value)
            {
                m_Offerta = value;
                m_IDOfferta = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("MacroStato", this.m_MacroStato);
                writer.WriteAttribute("IDPratica", this.IDPratica);
                writer.WriteAttribute("Data", this.m_Data);
                writer.WriteAttribute("IDOperatore", this.IDOperatore);
                writer.WriteAttribute("NomeOperatore", m_NomeOperatore);
                writer.WriteAttribute("Params", m_Params);
                writer.WriteAttribute("Attachment", m_Attachment);
                writer.WriteAttribute("Forzato", m_Forzato);
                writer.WriteAttribute("IDOfferta", IDOfferta);
                writer.WriteAttribute("IDFromStato", IDFromStato);
                writer.WriteAttribute("IDToStato", IDStatoPratica);
                writer.WriteAttribute("DescrizioneStato", m_DescrizioneStato);
                writer.WriteAttribute("IDRegolaApplicata", IDRegolaApplicata);
                base.XMLSerialize(writer);
                writer.WriteTag("Note", m_Note);
                // writer.WriteTag("StatoPratica", Me.StatoPratica)
                // writer.WriteTag("OffertaCorrente", Me.Offerta)
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "MacroStato":
                        {
                            m_MacroStato = (StatoPraticaEnum?)DMD.XML.Utils.Serializer.DeserializeNumber(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "IDPratica":
                        {
                            m_IDPratica = (int)DMD.XML.Utils.Serializer.DeserializeNumber(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Data":
                        {
                            m_Data = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDOperatore":
                        {
                            m_IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeNumber(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NomeOperatore":
                        {
                            m_NomeOperatore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Params":
                        {
                            m_Params = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Attachment":
                        {
                            m_Attachment = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Forzato":
                        {
                            m_Forzato = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "IDOfferta":
                        {
                            m_IDOfferta = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDFromStato":
                        {
                            m_IDFromStato = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDToStato":
                        {
                            m_IDStatoPratica = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DescrizioneStato":
                        {
                            m_DescrizioneStato = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDRegolaApplicata":
                        {
                            m_IDRegolaApplicata = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoPratica":
                        {
                            m_StatoPratica = (CStatoPratica)fieldValue;
                            break;
                        }

                    default:
                        {
                            // Case "OffertaCorrente" : Me.m_Offerta = fieldValue
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_PraticheSTL";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_MacroStato = reader.Read("MacroStato", this.m_MacroStato);
                m_IDPratica = reader.Read("IDPratica", m_IDPratica);
                m_Data = reader.Read("Data", m_Data);
                m_IDOperatore = reader.Read("IDOperatore", m_IDOperatore);
                m_NomeOperatore = reader.Read("NomeOperatore", m_NomeOperatore);
                m_Note = reader.Read("Note", m_Note);
                m_Params = reader.Read("Parameters", m_Params);
                // Me.m_Attachment = reader.Read("Attachment", Me.m_Attachment)
                m_Forzato = reader.Read("Forzato", m_Forzato);
                m_IDOfferta = reader.Read("IDOfferta", m_IDOfferta);
                m_IDFromStato = reader.Read("IDFromStato", m_IDFromStato);
                m_IDStatoPratica = reader.Read("IDToStato", m_IDStatoPratica);
                m_DescrizioneStato = reader.Read("DescrizioneStato", m_DescrizioneStato);
                m_IDRegolaApplicata = reader.Read("IDRegolaApplicata", m_IDRegolaApplicata);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("MacroStato", m_MacroStato);
                writer.Write("IDPratica", IDPratica);
                writer.Write("Data", m_Data);
                writer.Write("IDOperatore", IDOperatore);
                writer.Write("NomeOperatore", m_NomeOperatore);
                writer.Write("Note", m_Note);
                writer.Write("Parameters", m_Params);
                writer.Write("Forzato", m_Forzato);
                // writer.Write("Attachment", Me.m_Attachment)
                writer.Write("IDOfferta", IDOfferta);
                writer.Write("IDFromStato", IDFromStato);
                writer.Write("IDToStato", IDStatoPratica);
                writer.Write("DescrizioneStato", m_DescrizioneStato);
                writer.Write("IDRegolaApplicata", IDRegolaApplicata);
                // writer.Write("DataStr", DBUtils.ToDBDateStr(Me.m_Data))
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_DescrizioneStato;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_DescrizioneStato);
            }

            /// <summary>
            /// Imposta la pratica
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetPratica(CPraticaCQSPD value)
            {
                this.m_Pratica = value;
                this.m_IDPratica = DBUtils.GetID(value, 0);
            }

            int IComparable.CompareTo(object obj) { return CompareTo((CStatoLavorazionePratica)obj)); }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            public int CompareTo(CStatoLavorazionePratica item)
            {
                int ret = DMD.DateUtils.Compare(Data, item.Data);
                if (ret == 0) ret = DMD.Arrays.Compare(this.ID, item.ID);
                return ret;
            }

 

            //public override void InitializeFrom(object value)
            //{
            //    {
            //        var withBlock = (CStatoLavorazionePratica)value;
            //        m_MacroStato = withBlock.m_MacroStato;
            //        // Me.m_IDPratica = .m_IDPratica
            //        // Me.m_Pratica = .m_Pratica
            //        m_IDFromStato = withBlock.m_IDFromStato;
            //        m_FromStato = withBlock.m_FromStato;
            //        m_RegolaApplicata = withBlock.m_RegolaApplicata;
            //        m_IDRegolaApplicata = withBlock.m_IDRegolaApplicata;
            //        m_NomeRegolaApplicata = withBlock.m_NomeRegolaApplicata;
            //        m_IDStatoPratica = withBlock.m_IDStatoPratica;
            //        m_DescrizioneStato = withBlock.m_DescrizioneStato;
            //        m_StatoPratica = withBlock.m_StatoPratica;
            //        m_Data = withBlock.m_Data;
            //        m_IDOperatore = withBlock.m_IDOperatore;
            //        m_Operatore = withBlock.m_Operatore;
            //        m_NomeOperatore = withBlock.m_NomeOperatore;
            //        m_Note = withBlock.m_Note;
            //        m_Params = withBlock.m_Params;
            //        m_Forzato = withBlock.m_Forzato;
            //        m_Attachment = withBlock.m_Attachment;
            //        m_IDOfferta = withBlock.m_IDOfferta;
            //        m_Offerta = withBlock.m_Offerta;
            //    }

            //    base.InitializeFrom(value);
            //}

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CStatoLavorazionePratica obj)
            {
                return base.Equals(obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CStatoLavorazionePratica) && this.Equals((CStatoLavorazionePratica)obj);
            }

            /// <summary>
            /// Clona l'oggetto
            /// </summary>
            /// <returns></returns>
            public CStatoLavorazionePratica Clone()
            {
                return (CStatoLavorazionePratica)this._Clone();
            }
        }
    }
}