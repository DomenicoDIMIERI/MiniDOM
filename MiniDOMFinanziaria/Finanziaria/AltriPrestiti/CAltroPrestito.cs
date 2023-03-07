using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Tipologie di estinzione
        /// </summary>
        /// <remarks></remarks>
        public enum TipoEstinzione : int
        {
            ESTINZIONE_NO = 0,    // Sconosciuto
            ESTINZIONE_CESSIONEDELQUINTO = 1,    // Cessione del quinto
            ESTINZIONE_PRESTITODELEGA = 2,    // Delegazione di pagamento
            ESTINZIONE_PRESTITOPERSONALE = 3,    // Prestito personale
            ESTINZIONE_PIGNORAMENTO = 4,    // Pignoramento
            ESTINZIONE_MUTUO = 5,
            ESTINZIONE_PROTESTI = 6,
            ESTINZIONE_ASSICURAZIONE = 7,
            ESTINZIONE_ALIMENTI = 8,
            ESTINZIONE_CQP = 9,
            ESTINZIONE_PICCOLO_PRESTITO = 10
        }

        [Serializable]
        public class CAltroPrestito : Databases.DBObject
        {
            private TipoEstinzione m_Tipo; // [INT]      Un valore intero che indica la tipologia di estinzione
            private int m_IDIstituto; // [INT]      ID dell'istituto con cui il cliente ha stipulato il contratto da estinguere
            [NonSerialized] private CCQSPDCessionarioClass m_Istituto; // [Object]   Oggetto istituto con cui il cliente ha stipulato il contratto
            private string m_NomeIstituto; // [TEXT]     Nome dell'istituto
            private DateTime? m_DataInizio; // [Date]     Data di inizio del prestito
            private DateTime? m_Scadenza; // [Date]     Data di scadenza del prestito
            private decimal? m_Rata; // [Double]   
            private int m_Durata; // [INT]      Durata in numero di rate
            private double m_TAN; // [Double]    
            private int m_NumeroRateInsolute; // [int] Numero di rate insolute
            private double m_Penale; // [Double] Percentuale da corrispondere come penale per estinzione anticipata
            private int m_NumeroRatePagate;
            private bool m_Calculated;
            private decimal m_AbbuonoInteressi;
            private double m_PenaleEstinzione;
            private decimal m_SpeseAccessorie;
            private int m_IDPersona;
            [NonSerialized] private Anagrafica.CPersona m_Persona;
            private string m_NomePersona;

            public CAltroPrestito()
            {
                m_Tipo = TipoEstinzione.ESTINZIONE_NO;
                m_IDIstituto = 0;
                m_Istituto = null;
                m_NomeIstituto = "";
                m_DataInizio = default;
                m_Scadenza = default;
                m_Rata = 0;
                m_Durata = 0;
                m_TAN = 0d;
                m_NumeroRatePagate = 0;
                m_Calculated = false;
                m_AbbuonoInteressi = 0m;
                m_PenaleEstinzione = 1d;
                m_SpeseAccessorie = 0m;
                m_IDPersona = 0;
                m_Persona = null;
                m_NomePersona = "";
            }

            /// <summary>
        /// Restituisce o imposta l'ID della persona a cui appartiene questo oggetto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDPersona
            {
                get
                {
                    return Databases.GetID(m_Persona, m_IDPersona);
                }

                set
                {
                    int oldValue = IDPersona;
                    if (oldValue == value)
                        return;
                    m_IDPersona = value;
                    m_Persona = null;
                    DoChanged("IDPersona", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la persona a cui appartiene questo oggetto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public Anagrafica.CPersona Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = Anagrafica.Persone.GetItemById(m_IDPersona);
                    return m_Persona;
                }

                set
                {
                    var oldValue = m_Persona;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Persona = value;
                    m_IDPersona = Databases.GetID(value);
                    if (value is object)
                        m_NomePersona = value.Nominativo;
                    DoChanged("Persona", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome della persona a cui appartiene l'oggetto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomePersona
            {
                get
                {
                    return m_NomePersona;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomePersona;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersona = value;
                    DoChanged("NomePersona", value, oldValue);
                }
            }

            public void Validate()
            {
                if (m_Calculated == false)
                    Calculate();
            }

            public void Invalidate()
            {
                m_Calculated = false;
            }

            public override Sistema.CModule GetModule()
            {
                return AltriPrestiti.Module;
            }

            public double PenaleEstinzione
            {
                get
                {
                    return m_PenaleEstinzione;
                }

                set
                {
                    if (value < 0d)
                        throw new ArgumentOutOfRangeException("La penale di estinzione non può assumere un valore negativo");
                    double oldValue = m_PenaleEstinzione;
                    if (oldValue == value)
                        return;
                    m_PenaleEstinzione = value;
                    Invalidate();
                    DoChanged("PenaleEstinzione", value, oldValue);
                }
            }

            public decimal SpeseAccessorie
            {
                get
                {
                    return m_SpeseAccessorie;
                }

                set
                {
                    if (value < 0m)
                        throw new ArgumentOutOfRangeException("Le spese accessorie non possono assumere un valore negativo");
                    decimal oldValue = m_SpeseAccessorie;
                    if (oldValue == value)
                        return;
                    m_SpeseAccessorie = value;
                    Invalidate();
                    DoChanged("SpeseAccessorie", value, oldValue);
                }
            }

            public int NumeroRatePagate
            {
                get
                {
                    return m_NumeroRatePagate;
                }

                set
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException("Il numero di rate pagate non può essere negativo");
                    int oldValue = m_NumeroRatePagate;
                    if (oldValue == value)
                        return;
                    m_NumeroRatePagate = value;
                    Invalidate();
                    DoChanged("NumeroRatePagate", value, oldValue);
                }
            }

            public int NumeroRateResidue
            {
                get
                {
                    return Durata - NumeroRatePagate;
                }

                set
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException("Il numero di rate residue non può essere negativo");
                    int oldValue = NumeroRateResidue;
                    if (oldValue == value)
                        return;
                    m_NumeroRatePagate = Durata - value;
                    Invalidate();
                    DoChanged("NumeroRateResidue", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il numero di rate già maturate ma non  ancora pagate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int NumeroRateInsolute
            {
                get
                {
                    return m_NumeroRateInsolute;
                }

                set
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException("Il numero di rate insolute non può essere negativo");
                    int oldValue = m_NumeroRateInsolute;
                    if (oldValue == value)
                        return;
                    m_NumeroRateInsolute = value;
                    DoChanged("NumeroRateInsolute", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il tipo del prestito
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public TipoEstinzione Tipo
            {
                get
                {
                    return m_Tipo;
                }

                set
                {
                    var oldValue = m_Tipo;
                    if (oldValue == value)
                        return;
                    m_Tipo = value;
                    DoChanged("Tipo", value, oldValue);
                }
            }

            public string TipoEx
            {
                get
                {
                    return Estinzioni.FormatTipo(Tipo);
                }
            }

            public int IDIstituto
            {
                get
                {
                    return Databases.GetID(m_Istituto, m_IDIstituto);
                }

                set
                {
                    int oldValue = IDIstituto;
                    if (oldValue == value)
                        return;
                    m_IDIstituto = value;
                    m_Istituto = null;
                    DoChanged("IDIstituto", value, oldValue);
                }
            }

            public CCQSPDCessionarioClass Istituto
            {
                get
                {
                    if (m_Istituto is null)
                        m_Istituto = Cessionari.GetItemById(m_IDIstituto);
                    return m_Istituto;
                }

                set
                {
                    var oldValue = Istituto;
                    if (oldValue == value)
                        return;
                    m_Istituto = value;
                    m_IDIstituto = Databases.GetID(value);
                    if (value is object)
                        m_NomeIstituto = value.Nome;
                    DoChanged("Istituto", value, oldValue);
                }
            }

            public string NomeIstituto
            {
                get
                {
                    return m_NomeIstituto;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeIstituto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeIstituto = value;
                    DoChanged("NomeIstituto", value, oldValue);
                }
            }

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
                    Invalidate();
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            public DateTime? Scadenza
            {
                get
                {
                    return m_Scadenza;
                }

                set
                {
                    var oldValue = m_Scadenza;
                    if (oldValue == value == true)
                        return;
                    if (m_Scadenza == value == true)
                        return;
                    m_Scadenza = value;
                    Invalidate();
                    DoChanged("Scadenza", value, oldValue);
                }
            }

            public decimal? Rata
            {
                get
                {
                    return m_Rata;
                }

                set
                {
                    var oldValue = m_Rata;
                    if (oldValue == value == true)
                        return;
                    m_Rata = value;
                    Invalidate();
                    DoChanged("Rata", value, oldValue);
                }
            }

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
                    Invalidate();
                    DoChanged("Durata", value, oldValue);
                }
            }

            public double TAN
            {
                get
                {
                    return m_TAN;
                }

                set
                {
                    double oldValue = m_TAN;
                    if (oldValue == value)
                        return;
                    m_TAN = value;
                    Invalidate();
                    DoChanged("TAN", value, oldValue);
                }
            }

            public decimal? DebitoIniziale
            {
                get
                {
                    if (m_Rata.HasValue)
                        return m_Rata.Value * m_Durata;
                    return default;
                }

                set
                {
                    var oldValue = DebitoIniziale;
                    if (oldValue == value == true)
                        return;
                    if (m_Durata <= 0)
                        m_Durata = 1;
                    m_Rata = value / m_Durata;
                    DoChanged("DebitoIniziale", value, oldValue);
                }
            }

            public decimal? DebitoResiduo
            {
                get
                {
                    if (DebitoIniziale.HasValue && m_Rata.HasValue)
                    {
                        return DebitoIniziale.Value - m_Rata.Value * m_NumeroRatePagate;
                    }
                    else
                    {
                        return default;
                    }
                }

                set
                {
                    var oldValue = DebitoResiduo;
                    if (oldValue == value == true)
                        return;
                    m_NumeroRatePagate = (int)((DebitoIniziale - value) / m_Rata.Value);
                    DoChanged("DebitoResiduo", value, oldValue);
                }
            }

            public decimal AbbuonoInteressi
            {
                get
                {
                    Validate();
                    return m_AbbuonoInteressi;
                }
            }

            public decimal CapitaleDaRimborsare
            {
                get
                {
                    if (DebitoResiduo.HasValue)
                    {
                        return DebitoResiduo.Value - AbbuonoInteressi;
                    }
                    else
                    {
                        return 0m;
                    }
                }
            }

            public decimal ValorePenale
            {
                get
                {
                    return (decimal)((double)CapitaleDaRimborsare * m_PenaleEstinzione / 100d);
                }
            }

            public decimal TotaleDaRimborsare
            {
                get
                {
                    return CapitaleDaRimborsare + ValorePenale + SpeseAccessorie + ValoreQuoteInsolute;
                }
            }

            public decimal ValoreQuoteInsolute
            {
                get
                {
                    Validate();
                    if (m_Rata.HasValue)
                    {
                        return m_NumeroRateInsolute * m_Rata.Value;
                    }
                    else
                    {
                        return 0m;
                    }
                }
            }

            public bool Calculate()
            {
                // Dim Dk As Decimal   'Debito residuo per k=1..n-1
                decimal Ik; // Interesse in ciascun periodo k=1...n
                int k;
                double i;

                // Me.m_DebitoIniziale = Me.m_Rata.Value * Me.m_Durata
                // Me.m_DebitoResiduo = Me.m_DebitoIniziale - Me.m_Quota * Me.m_NumeroRatePagate

                i = m_TAN / 100d / 12d;
                m_AbbuonoInteressi = 0m;
                var loopTo = m_Durata;
                for (k = m_NumeroRatePagate + 1; k <= loopTo; k++)
                {
                    Ik = (decimal)((double)m_Rata.Value * (1d - 1d / Math.Pow(1d + i, m_Durata - k + 1)));
                    m_AbbuonoInteressi = m_AbbuonoInteressi + Ik;
                }

                m_Calculated = true;
                return true;
            }

            // ---------------------------------------------------------------
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("m_Tipo", (int?)m_Tipo);
                writer.WriteAttribute("m_IDIstituto", IDIstituto);
                writer.WriteAttribute("m_NomeIstituto", m_NomeIstituto);
                writer.WriteAttribute("m_DataInizio", m_DataInizio);
                writer.WriteAttribute("m_Scadenza", m_Scadenza);
                writer.WriteAttribute("m_Rata", m_Rata);
                writer.WriteAttribute("m_Durata", m_Durata);
                writer.WriteAttribute("m_TAN", m_TAN);
                writer.WriteAttribute("NumeroRatePagate", m_NumeroRatePagate);
                writer.WriteAttribute("NumeroRateInsolute", m_NumeroRateInsolute);
                writer.WriteAttribute("Calculated", m_Calculated);
                writer.WriteAttribute("AbbuonoInteressi", m_AbbuonoInteressi);
                writer.WriteAttribute("PenaleEstinzione", m_PenaleEstinzione);
                writer.WriteAttribute("SpeseAccessorie", m_SpeseAccessorie);
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteAttribute("NomePersona", m_NomePersona);
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "m_Tipo":
                        {
                            m_Tipo = (TipoEstinzione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "m_IDIstituto":
                        {
                            m_IDIstituto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "m_NomeIstituto":
                        {
                            m_NomeIstituto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_DataInizio":
                        {
                            m_DataInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "m_Scadenza":
                        {
                            m_Scadenza = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "m_Rata":
                        {
                            m_Rata = (decimal?)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "m_Durata":
                        {
                            m_Durata = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "m_TAN":
                        {
                            m_TAN = (double)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NumeroRateInsolute":
                        {
                            m_NumeroRateInsolute = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NumeroRatePagate":
                        {
                            m_NumeroRatePagate = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Calculated":
                        {
                            m_Calculated = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "AbbuonoInteressi":
                        {
                            m_AbbuonoInteressi = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "PenaleEstinzione":
                        {
                            m_PenaleEstinzione = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "SpeseAccessorie":
                        {
                            m_SpeseAccessorie = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "IDPersona":
                        {
                            m_IDPersona = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePersona":
                        {
                            m_NomePersona = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDAltriPrestiti";
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(Databases.DBReader reader)
            {
                this.m_Tipo = reader.Read("Tipo", this.m_Tipo);
                this.m_IDIstituto = reader.Read("Istituto", this.m_IDIstituto);
                this.m_NomeIstituto = reader.Read("NomeIstituto", this.m_NomeIstituto);
                this.m_DataInizio = reader.Read("DataInizio", this.m_DataInizio);
                this.m_Scadenza = reader.Read("Scadenza", this.m_Scadenza);
                this.m_Rata = reader.Read("Rata", this.m_Rata);
                this.m_Durata = reader.Read("Durata", this.m_Durata);
                this.m_TAN = reader.Read("TAN", this.m_TAN);
                this.m_NumeroRatePagate = reader.Read("NumeroRatePagate", this.m_NumeroRatePagate);
                this.m_NumeroRateInsolute = reader.Read("NumeroRateResidue", this.m_NumeroRateInsolute);
                this.m_Calculated = reader.Read("Calculated", this.m_Calculated);
                this.m_AbbuonoInteressi = reader.Read("AbbuonoInteressi", this.m_AbbuonoInteressi);
                this.m_PenaleEstinzione = reader.Read("PenaleEstinzione", this.m_PenaleEstinzione);
                this.m_SpeseAccessorie = reader.Read("SpeseAccessorie", this.m_SpeseAccessorie);
                this.m_IDPersona = reader.Read("IDPersona", this.m_IDPersona);
                this.m_NomePersona = reader.Read("NomePersona", this.m_NomePersona);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(Databases.DBWriter writer)
            {
                writer.Write("Tipo", m_Tipo);
                writer.Write("Istituto", IDIstituto);
                writer.Write("NomeIstituto", m_NomeIstituto);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("Scadenza", m_Scadenza);
                writer.Write("Rata", m_Rata);
                writer.Write("Durata", m_Durata);
                writer.Write("TAN", m_TAN);
                writer.Write("NumeroRatePagate", m_NumeroRatePagate);
                writer.Write("NumeroRateResidue", m_NumeroRateInsolute);
                writer.Write("Calculated", m_Calculated);
                writer.Write("AbbuonoInteressi", m_AbbuonoInteressi);
                writer.Write("PenaleEstinzione", m_PenaleEstinzione);
                writer.Write("SpeseAccessorie", m_SpeseAccessorie);
                writer.Write("IDPersona", IDPersona);
                writer.Write("NomePersona", m_NomePersona);
                return base.SaveToRecordset(writer);
            }

            public override string ToString()
            {
                return DMD.Strings.JoinW(
                            AltriPrestiti.FormatTipo(Tipo),
                            ", Rata: ", Sistema.Formats.FormatValuta(Rata),
                            ", Rate residue: ", NumeroRateResidue
                            );
            }

            public bool InCorso()
            {
                return DMD.DateUtils.CheckBetween(DMD.DateUtils.Now(), DataInizio, Scadenza);
            }

            internal void SetPersona(Anagrafica.CPersona value)
            {
                m_Persona = value;
                m_IDPersona = Databases.GetID(value);
                m_NomePersona = value.Nominativo;
            }

            protected override void OnCreate(SystemEvent e)
            {
                if (m_DataInizio.HasValue && m_Durata > 0)
                {
                    ProgrammaRicontatto();
                }

                base.OnCreate(e);
            }

            protected override void OnModified(SystemEvent e)
            {
                if (m_DataInizio.HasValue && m_Durata > 0)
                {
                    ProgrammaRicontatto();
                }

                base.OnModified(e);
            }

            protected override void OnDelete(SystemEvent e)
            {
                AnnullaRicontatto();
                base.OnDelete(e);
            }

            /// <summary>
        /// Programma il ricontatto opportuno
        /// </summary>
        /// <remarks></remarks>
            public void ProgrammaRicontatto()
            {
                // If (Me.InCorso) Then
                DateTime dataRinnovo;
                float percRinnovo;
                int giorniAnticipo;
                int numMesi;
                string motivo;
                string nomeLista;
                var r = Anagrafica.ListeRicontatto.Items.GetRicontattoBySource(this);
                if (r is object && r.StatoRicontatto == Anagrafica.StatoRicontatto.EFFETTUATO)
                    return;
                if (this.GetPraticheEstinte().Count > 0)
                {
                    if (r is object)
                    {
                        r.StatoRicontatto = Anagrafica.StatoRicontatto.ANNULLATO;
                        r.Save();
                    }
                }
                else
                {
                    switch (Tipo)
                    {
                        case TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO:
                        case TipoEstinzione.ESTINZIONE_PRESTITODELEGA:
                        case TipoEstinzione.ESTINZIONE_CQP:
                            {
                                nomeLista = "Altri Prestiti Rinnovabili";
                                percRinnovo = (float)GetModule().Settings.GetValueDouble("Rinnovo_PercDurata", 40d);
                                giorniAnticipo = GetModule().Settings.GetValueInt("Rinnovo_GiorniAnticipo", 30);
                                numMesi = (int)Math.Floor(Durata * percRinnovo / 100f);
                                dataRinnovo = DMD.DateUtils.DateAdd(DateTimeInterval.Month, numMesi, DataInizio.Value);
                                dataRinnovo = DMD.DateUtils.DateAdd(DateTimeInterval.Day, -giorniAnticipo, dataRinnovo);
                                motivo = "Finanziamento rinnovabile: raggiunto il " + Sistema.Formats.FormatInteger(percRinnovo) + "% del prestito " + ToString();
                                if (r is object)
                                {
                                    r.Note = motivo;
                                    r.NomeLista = nomeLista;
                                    r.DataPrevista = dataRinnovo;
                                    r.Save();
                                }
                                else
                                {
                                    r = (Anagrafica.ListaRicontattoItem)Anagrafica.Ricontatti.ProgrammaRicontatto(Persona, dataRinnovo, motivo, DMD.RunTime.vbTypeName(this), Databases.GetID(this).ToString(), nomeLista, Persona.PuntoOperativo, Sistema.Users.CurrentUser);
                                }

                                break;
                            }

                        default:
                            {
                                if (r is object)
                                {
                                    r.StatoRicontatto = Anagrafica.StatoRicontatto.ANNULLATO;
                                    r.Save();
                                }

                                break;
                            }
                    }
                }
            }

            public void AnnullaRicontatto()
            {
                var r = Anagrafica.Ricontatti.GetRicontattoBySource(this);
                if (r is object)
                {
                    r.StatoRicontatto = Anagrafica.StatoRicontatto.ANNULLATO;
                    r.Save();
                }
            }

            /// <summary>
        /// Ottiene l'elenco delle estinzioni di questo oggetto. L'elenco non contiene le estinzioni relative alle pratiche annullate
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<CEstinzione> GetPraticheEstinte()
            {
                var ret = new CCollection<CEstinzione>();
                using (var cursor = new CEstinzioniCursor()) { 
                    cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
                    // cursor.IDAltroPrestito.Value = GetID(Me)
                    while (!cursor.EOF())
                    {
                        if (cursor.Item.Pratica.StatoAttuale.MacroStato.HasValue)
                        {
                            switch (cursor.Item.Pratica.StatoAttuale.MacroStato)
                            {
                                case StatoPraticaEnum.STATO_ANNULLATA:
                                    {
                                        break;
                                    }

                                default:
                                    {
                                        ret.Add(cursor.Item);
                                        break;
                                    }
                            }
                        }

                        cursor.MoveNext();
                    }
                }

                return ret;
            }
        }
    }
}