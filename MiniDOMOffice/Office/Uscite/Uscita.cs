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
using static minidom.Office;


namespace minidom
{
    public partial class Office
    {



        /// <summary>
        /// Rappresenta una uscita che comprende più commissioni
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class Uscita 
            : minidom.Databases.DBObjectPO, IComparable, ICloneable, IComparable<Uscita>
        {
            [NonSerialized]
            private Sistema.CUser m_Operatore;                    // Operatore che ha svolto la commissione
            private int m_IDOperatore;                // ID dell'operatore che ha svolto la commissione
            private string m_NomeOperatore;               // Nome dell'operatore che ha svolto la commissione
            private DateTime? m_OraUscita;        // Data ed ora di uscita (per svolgere la commissione)
            private DateTime? m_OraRientro;       // Data ed ora di rientro
            private double? m_DistanzaPercorsa;   // Distanza percorsa
            private int m_IDVeicoloUsato;
            [NonSerialized] private Veicolo m_VeicoloUsato;
            private string m_NomeVeicoloUsato;
            private double? m_LitriCarburante;
            private decimal? m_Rimborso;
            private string m_Descrizione;                 // Descrizione lunga
            private int m_IDDispositivo;
            [NonSerialized] private Dispositivo m_Dispositivo;
            [NonSerialized] private CommissioniPerUscitaCollection m_Commissioni;
            [NonSerialized] private LuoghiVisitatiPerUscitaCollection m_LuoghiVisitati;
            private Anagrafica.CIndirizzo m_IndirizzoPartenza;
            private Anagrafica.CIndirizzo m_IndirizzoRitorno;

            /// <summary>
            /// Costruttore
            /// </summary>
            public Uscita()
            {
                m_Operatore = null;
                m_IDOperatore = 0;
                m_NomeOperatore = DMD.Strings.vbNullString;
                m_OraUscita = default;
                m_OraRientro = default;
                m_DistanzaPercorsa = default;
                m_Commissioni = null;
                m_IDVeicoloUsato = 0;
                m_VeicoloUsato = null;
                m_LitriCarburante = default;
                m_Rimborso = default;
                m_NomeVeicoloUsato = DMD.Strings.vbNullString;
                m_Descrizione = DMD.Strings.vbNullString;
                m_IDDispositivo = 0;
                m_Dispositivo = null;
                m_LuoghiVisitati = null;
                m_IndirizzoPartenza = new Anagrafica.CIndirizzo();
                m_IndirizzoRitorno = new Anagrafica.CIndirizzo();
            }

            /// <summary>
            /// Indirizzo di partenza dell'uscita
            /// </summary>
            public Anagrafica.CIndirizzo IndirizzoDiPartenza
            {
                get
                {
                    return m_IndirizzoPartenza;
                }
            }

            /// <summary>
            /// Indirizzo di rientro
            /// </summary>
            public Anagrafica.CIndirizzo IndirizzoDiRitorno
            {
                get
                {
                    return m_IndirizzoRitorno;
                }
            }

            // Public Property GPSPartenza As GPSPosition
            // Get
            // Return Me.m_GPSPartenza
            // End Get
            // Set(value As GPSPosition)
            // Dim oldValue As GPSPosition = Me.m_GPSPartenza
            // If (oldValue = value) Then Exit Property
            // If (value Is Nothing) Then value = New GPSPosition
            // Me.m_GPSPartenza = value
            // Me.DoChanged("GPSPartenza", value, oldValue)
            // End Set
            // End Property

            /// <summary>
            /// Restituisce la collezione dei luoghi visitati durante l'uscita
            /// </summary>
            public LuoghiVisitatiPerUscitaCollection LuoghiVisitati
            {
                get
                {
                    if (m_LuoghiVisitati is null)
                        m_LuoghiVisitati = new LuoghiVisitatiPerUscitaCollection(this);
                    return m_LuoghiVisitati;
                }
            }

            /// <summary>
            /// ID del dispositivo associato all'uscita 
            /// </summary>
            public int IDDispositivo
            {
                get
                {
                    return DBUtils.GetID(m_Dispositivo, m_IDDispositivo);
                }

                set
                {
                    int oldValue = IDDispositivo;
                    if (oldValue == value)
                        return;
                    m_IDDispositivo = value;
                    m_Dispositivo = null;
                    DoChanged("IDDispositivo", value, oldValue);
                }
            }

            /// <summary>
            /// Dispositivo associato all'uscita
            /// </summary>
            public Dispositivo Dispositivo
            {
                get
                {
                    if (m_Dispositivo is null)
                        m_Dispositivo = Dispositivi.GetItemById(m_IDDispositivo);
                    return m_Dispositivo;
                }

                set
                {
                    var oldValue = m_Dispositivo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Dispositivo = value;
                    m_IDDispositivo = DBUtils.GetID(value, 0);
                    DoChanged("Dispositivo", value, oldValue);
                }
            }

            /// <summary>
            /// Descrizione dell'uscita
            /// </summary>
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
            /// Consumo di carburante stimato per completare il percorso
            /// </summary>
            public double? ConsumoStimato
            {
                get
                {
                    if ((m_DistanzaPercorsa.HasValue && VeicoloUsato is object && VeicoloUsato.KmALitro.HasValue && VeicoloUsato.KmALitro > 0) == true)
                    {
                        return (double?)(m_DistanzaPercorsa.Value / VeicoloUsato.KmALitro.Value);
                    }
                    else
                    {
                        return default;
                    }
                }
            }

            /// <summary>
            /// Commissioni svolte durante l'uscita
            /// </summary>
            public CommissioniPerUscitaCollection Commissioni
            {
                get
                {
                    if (m_Commissioni is null)
                        m_Commissioni = new CommissioniPerUscitaCollection(this);
                    return m_Commissioni;
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del veicolo usato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDVeicoloUsato
            {
                get
                {
                    return DBUtils.GetID(m_VeicoloUsato, m_IDVeicoloUsato);
                }

                set
                {
                    int oldValue = IDVeicoloUsato;
                    if (oldValue == value)
                        return;
                    m_IDVeicoloUsato = value;
                    m_VeicoloUsato = null;
                    DoChanged("IDVeicoloUsato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il veicolo usato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Veicolo VeicoloUsato
            {
                get
                {
                    if (m_VeicoloUsato is null)
                        m_VeicoloUsato = Veicoli.GetItemById(m_IDVeicoloUsato);
                    return m_VeicoloUsato;
                }

                set
                {
                    var oldValue = m_VeicoloUsato;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_VeicoloUsato = value;
                    m_IDVeicoloUsato = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeVeicoloUsato = value.Nome;
                    DoChanged("VeicoloUsato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del veicolo usato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeVeicoloUsato
            {
                get
                {
                    return m_NomeVeicoloUsato;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeVeicoloUsato;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeVeicoloUsato = value;
                    DoChanged("NomeVeicoloUsato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta i litri di carburante consumati
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double? LitriCarburante
            {
                get
                {
                    return m_LitriCarburante;
                }

                set
                {
                    var oldValue = m_LitriCarburante;
                    if (oldValue == value == true)
                        return;
                    m_LitriCarburante = value;
                    DoChanged("LitriCarburante", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il rimborso per l'uscita
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public decimal? Rimborso
            {
                get
                {
                    return m_Rimborso;
                }

                set
                {
                    var oldValue = m_Rimborso;
                    if (oldValue == value == true)
                        return;
                    m_Rimborso = value;
                    DoChanged("Rimborso", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'operatore che ha effettuato la commissione
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
                    var oldValue = m_Operatore;
                    if (ReferenceEquals(value, oldValue))
                        return;
                    m_Operatore = value;
                    m_IDOperatore = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeOperatore = value.Nominativo;
                    DoChanged("Operatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'operatore che ha effettuato la commissione
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
            /// Restituisce o imposta il nome dell'operatore che ha effettuate la commissione
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
            /// Restituisce o imposta la data e l'ora di uscita per la commissione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? OraUscita
            {
                get
                {
                    return m_OraUscita;
                }

                set
                {
                    var oldValue = m_OraUscita;
                    if (oldValue == value == true)
                        return;
                    m_OraUscita = value;
                    DoChanged("OraUscita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la durata in secondi (differenza tra ora ingresso ed ora uscita)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int? Durata
            {
                get
                {
                    if (m_OraUscita.HasValue && m_OraRientro.HasValue)
                    {
                        return (int?)Maths.Abs(DMD.DateUtils.DateDiff("s", m_OraRientro.Value, m_OraUscita.Value));
                    }
                    else
                    {
                        return default;
                    }
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di rientro
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? OraRientro
            {
                get
                {
                    return m_OraRientro;
                }

                set
                {
                    var oldValue = m_OraRientro;
                    if (oldValue == value == true)
                        return;
                    m_OraRientro = value;
                    DoChanged("OraRientro", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la distanza percorsa
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double? DistanzaPercorsa
            {
                get
                {
                    return m_DistanzaPercorsa;
                }

                set
                {
                    var oldValue = m_DistanzaPercorsa;
                    if (oldValue == value == true)
                        return;
                    m_DistanzaPercorsa = value;
                    DoChanged("DistanzaPercorsa", value, oldValue);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Uscite;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeUscite";
            }

            /// <summary>
            /// Restituisce true se l'oggetto é stato modificato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                return   base.IsChanged() 
                      || m_IndirizzoPartenza.IsChanged() 
                      || m_IndirizzoRitorno.IsChanged();
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                m_IndirizzoPartenza.SetChanged(false);
                m_IndirizzoRitorno.SetChanged(false);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDOperatore = reader.Read("IDOperatore", m_IDOperatore);
                m_NomeOperatore = reader.Read("NomeOperatore", m_NomeOperatore);
                m_OraUscita = reader.Read("OraUscita", m_OraUscita);
                m_OraRientro = reader.Read("OraRientro", m_OraRientro);
                m_DistanzaPercorsa = reader.Read("DistanzaPercorsa", m_DistanzaPercorsa);
                m_IDVeicoloUsato = reader.Read("IDVeicoloUsato", m_IDVeicoloUsato);
                m_NomeVeicoloUsato = reader.Read("NomeVeicoloUsato", m_NomeVeicoloUsato);
                m_LitriCarburante = reader.Read("LitriCarburante", m_LitriCarburante);
                m_Rimborso = reader.Read("Rimborso", m_Rimborso);
                m_Descrizione = reader.Read("Descrizione", m_Descrizione);
                m_IDDispositivo = reader.Read("IDDispositivo", m_IDDispositivo);
                m_IndirizzoPartenza.Nome = reader.Read("Indirizzo_Nome", m_IndirizzoPartenza.Nome);
                m_IndirizzoPartenza.ToponimoEVia = reader.Read("Indirizzo_Via", m_IndirizzoPartenza.ToponimoEVia);
                m_IndirizzoPartenza.Civico = reader.Read("Indirizzo_Civico", m_IndirizzoPartenza.Civico);
                m_IndirizzoPartenza.Citta = reader.Read("Indirizzo_Citta", m_IndirizzoPartenza.Citta);
                m_IndirizzoPartenza.Provincia = reader.Read("Indirizzo_Provincia", m_IndirizzoPartenza.Provincia);
                m_IndirizzoPartenza.CAP = reader.Read("Indirizzo_CAP", m_IndirizzoPartenza.CAP);
                m_IndirizzoPartenza.Latitude = reader.Read("Lat", m_IndirizzoPartenza.Latitude);
                m_IndirizzoPartenza.Longitude = reader.Read("Lng", m_IndirizzoPartenza.Longitude);
                m_IndirizzoPartenza.Altitude = reader.Read("Alt", m_IndirizzoPartenza.Altitude);
                m_IndirizzoPartenza.SetChanged(false);
                m_IndirizzoRitorno.Nome = reader.Read("IndirizzoR_Nome", m_IndirizzoRitorno.Nome);
                m_IndirizzoRitorno.ToponimoEVia = reader.Read("IndirizzoR_Via", m_IndirizzoRitorno.ToponimoEVia);
                m_IndirizzoRitorno.Civico = reader.Read("IndirizzoR_Civico", m_IndirizzoRitorno.Civico);
                m_IndirizzoRitorno.Citta = reader.Read("IndirizzoR_Citta", m_IndirizzoRitorno.Citta);
                m_IndirizzoRitorno.Provincia = reader.Read("IndirizzoR_Provincia", m_IndirizzoRitorno.Provincia);
                m_IndirizzoRitorno.CAP = reader.Read("IndirizzoR_CAP", m_IndirizzoRitorno.CAP);
                m_IndirizzoRitorno.Latitude = reader.Read("LatR", m_IndirizzoRitorno.Latitude);
                m_IndirizzoRitorno.Longitude = reader.Read("LngR", m_IndirizzoRitorno.Longitude);
                m_IndirizzoRitorno.Altitude = reader.Read("AltR", m_IndirizzoRitorno.Altitude);
                m_IndirizzoRitorno.SetChanged(false);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDOperatore", IDOperatore);
                writer.Write("NomeOperatore", m_NomeOperatore);
                writer.Write("OraUscita", m_OraUscita);
                writer.Write("OraRientro", m_OraRientro);
                writer.Write("DistanzaPercorsa", m_DistanzaPercorsa);
                writer.Write("IDVeicoloUsato", IDVeicoloUsato);
                writer.Write("NomeVeicoloUsato", m_NomeVeicoloUsato);
                writer.Write("LitriCarburante", m_LitriCarburante);
                writer.Write("Rimborso", m_Rimborso);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("IDDispositivo", IDDispositivo);
                writer.Write("Indirizzo_Nome", m_IndirizzoPartenza.Nome);
                writer.Write("Indirizzo_Via", m_IndirizzoPartenza.ToponimoEVia);
                writer.Write("Indirizzo_Civico", m_IndirizzoPartenza.Civico);
                writer.Write("Indirizzo_Citta", m_IndirizzoPartenza.Citta);
                writer.Write("Indirizzo_Provincia", m_IndirizzoPartenza.Provincia);
                writer.Write("Indirizzo_CAP", m_IndirizzoPartenza.CAP);
                writer.Write("Lat", m_IndirizzoPartenza.Latitude);
                writer.Write("Lng", m_IndirizzoPartenza.Longitude);
                writer.Write("Alt", m_IndirizzoPartenza.Altitude);
                writer.Write("IndirizzoR_Nome", m_IndirizzoRitorno.Nome);
                writer.Write("IndirizzoR_Via", m_IndirizzoRitorno.ToponimoEVia);
                writer.Write("IndirizzoR_Civico", m_IndirizzoRitorno.Civico);
                writer.Write("IndirizzoR_Citta", m_IndirizzoRitorno.Citta);
                writer.Write("IndirizzoR_Provincia", m_IndirizzoRitorno.Provincia);
                writer.Write("IndirizzoR_CAP", m_IndirizzoRitorno.CAP);
                writer.Write("LatR", m_IndirizzoRitorno.Latitude);
                writer.Write("LngR", m_IndirizzoRitorno.Longitude);
                writer.Write("AltR", m_IndirizzoRitorno.Altitude);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", m_NomeOperatore);
                writer.WriteAttribute("OraUscita", m_OraUscita);
                writer.WriteAttribute("OraRientro", m_OraRientro);
                writer.WriteAttribute("DistanzaPercorsa", m_DistanzaPercorsa);
                writer.WriteAttribute("IDVeicoloUsato", IDVeicoloUsato);
                writer.WriteAttribute("NomeVeicoloUsato", m_NomeVeicoloUsato);
                writer.WriteAttribute("LitriCarburante", m_LitriCarburante);
                writer.WriteAttribute("Rimborso", m_Rimborso);
                writer.WriteAttribute("IDDispositivo", IDDispositivo);
                base.XMLSerialize(writer);
                writer.WriteTag("Descrizione", m_Descrizione);
                writer.WriteTag("IndirizzoP", m_IndirizzoPartenza);
                writer.WriteTag("IndirizzoR", m_IndirizzoRitorno);
                writer.SetSetting("uscitaserialization", true);
                writer.WriteTag("Commissioni", Commissioni);
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

                    case "OraUscita":
                        {
                            m_OraUscita = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "OraRientro":
                        {
                            m_OraRientro = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DistanzaPercorsa":
                        {
                            m_DistanzaPercorsa = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "IDVeicoloUsato":
                        {
                            m_IDVeicoloUsato = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeVeicoloUsato":
                        {
                            m_NomeVeicoloUsato = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "LitriCarburante":
                        {
                            m_LitriCarburante = (double?)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Rimborso":
                        {
                            m_Rimborso = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDDispositivo":
                        {
                            m_IDDispositivo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Commissioni":
                        {
                            m_Commissioni = (CommissioniPerUscitaCollection)fieldValue;
                            m_Commissioni.SetUscita(this);
                            break;
                        }

                    case "IndirizzoP":
                        {
                            m_IndirizzoPartenza = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }

                    case "IndirizzoR":
                        {
                            m_IndirizzoRitorno = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Compara due oggetti in base alla data di uscita in ordine crescente
            /// </summary>
            /// <param name="b"></param>
            /// <returns></returns>
            public int CompareTo(Uscita b)
            {
                return DMD.DateUtils.Compare(OraUscita, b.OraUscita);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((Uscita)obj);
            }

            /// <summary>
            /// Inizializza le commissioni per l'uscita
            /// </summary>
            /// <param name="col"></param>
            /// <returns></returns>
            public CCollection<CommissionePerUscita> IniziaCommissioni(CCollection<Commissione> col)
            {
                var ret = new CCollection<CommissionePerUscita>();
                foreach (Commissione c in col)
                {
                    if (
                           c.StatoCommissione != StatoCommissione.NonIniziata 
                        && c.StatoCommissione != StatoCommissione.Rimandata
                        )
                    {
                        throw new InvalidOperationException("Commissione[" + DBUtils.GetID(c) + "] non in stato coerente");
                    }
                }

                foreach (Commissione c in col)
                {
                    c.StatoCommissione = StatoCommissione.Iniziata;
                    c.Operatore = Sistema.Users.CurrentUser;
                    c.OraUscita = OraUscita;
                    c.Save();
                    var cxu = Commissioni.Add(c, Operatore, "");
                    ret.Add(cxu);
                }

                return ret;
            }

            /// <summary>
            /// Restituisce una copia dell'oggetto
            /// </summary>
            /// <returns></returns>
            public Uscita Clone()
            {
                return (Uscita) this._Clone();
            }

            /// <summary>
            /// Clona l'oggetto
            /// </summary>
            /// <returns></returns>
            protected override Databases.DBObjectBase _Clone()
            {
                //TODO controllare Uscita.Clone
                var ret = (Uscita) base._Clone();

                return ret;
            }
        }
    }
}