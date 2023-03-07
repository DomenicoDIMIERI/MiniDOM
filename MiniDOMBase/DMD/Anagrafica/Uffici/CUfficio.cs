using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;


namespace minidom
{
    public partial class Anagrafica
    {
        /// <summary>
        /// Flag validi per un ufficio
        /// </summary>
        [Flags]
        public enum UfficioFlags : int
        {
            /// <summary>
            /// Nessuno
            /// </summary>
            None = 0,

            /// <summary>
            /// Attivo
            /// </summary>
            Attivo = 1,

            /// <summary>
            /// Sede legale
            /// </summary>
            SedeLegale = 2,

            /// <summary>
            /// Sede operativa
            /// </summary>
            SedeOperativa = 4
        }

        /// <summary>
        /// Classe che rappresenta un ufficio di una azienda
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CUfficio
            : Databases.DBObject, IComparable, IComparable<CUfficio>
        {
            private string m_Nome;
            private int m_IDAzienda;
            private string m_NomeAzienda;
            [NonSerialized] private CAzienda m_Azienda;
            [NonSerialized] private CUtentiXUfficioCollection m_Utenti;
            private CIndirizzo m_Indirizzo;
            private string m_Telefono;
            private string m_Telefono1;
            private string m_Fax;
            private string m_Fax1;
            private string m_EMail;
            private string m_PEC;
            private string m_WebSite;
            private int m_MinimoRicontatti;
            private bool m_Attivo;
            private DateTime? m_DataApertura;
            private DateTime? m_DataChiusura;
            private int m_IDResponsabile;
            [NonSerialized] private Sistema.CUser m_Responsabile;
            private string m_NomeResponsabile;
            private string m_CodiceFiscale;
            [NonSerialized] private CContattiPerPersonaCollection m_Recapiti;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CUfficio()
            {
                m_Nome = "";
                m_Utenti = null;
                m_IDAzienda = 0;
                m_Azienda = null;
                m_NomeAzienda = "";
                m_Indirizzo = new CIndirizzo();
                m_Utenti = null;
                m_MinimoRicontatti = 0;
                m_Attivo = true;
                m_DataApertura = default;
                m_DataChiusura = default;
                m_Telefono = "";
                m_Telefono1 = "";
                m_Fax = "";
                m_Fax1 = "";
                m_EMail = "";
                m_PEC = "";
                m_WebSite = "";
                m_IDResponsabile = 0;
                m_Responsabile = null;
                m_NomeResponsabile = "";
                m_Flags = (int)UfficioFlags.Attivo;
                m_CodiceFiscale = "";
                m_Recapiti = null;
               
            }

             

            /// <summary>
            /// Recapiti dell'ufficio
            /// </summary>
            public CContattiPerPersonaCollection Recapiti
            {
                get
                {
                    if (m_Recapiti is null)
                        m_Recapiti = new CContattiPerPersonaCollection(Azienda, this);
                    return m_Recapiti;
                }
            }

            /// <summary>
            /// Codice fiscale (se la sede è diversa dalla sede legale)
            /// </summary>
            public string CodiceFiscale
            {
                get
                {
                    return m_CodiceFiscale;
                }

                set
                {
                    string oldValue = m_CodiceFiscale;
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceFiscale = value;
                    DoChanged("CodiceFiscale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di telefono principale dell'ufficio
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
                    string oldValue = m_Telefono;
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Telefono = value;
                    DoChanged("Telefono", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di telefono secondario dell'ufficio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Telefono1
            {
                get
                {
                    return m_Telefono1;
                }

                set
                {
                    string oldValue = m_Telefono1;
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Telefono1 = value;
                    DoChanged("Telefono1", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di fax principale dell'ufficio
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
                    string oldValue = m_Fax;
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Fax = value;
                    DoChanged("Fax", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di fax secondario dell'ufficio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Fax1
            {
                get
                {
                    return m_Fax1;
                }

                set
                {
                    string oldValue = m_Fax1;
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Fax1 = value;
                    DoChanged("Fax1", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'email principale dell'ufficio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string eMail
            {
                get
                {
                    return m_EMail;
                }

                set
                {
                    string oldValue = m_EMail;
                    value = Sistema.Formats.ParseEMailAddress(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_EMail = value;
                    DoChanged("eMail", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo PEC principale dell'ufficio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string PEC
            {
                get
                {
                    return m_PEC;
                }

                set
                {
                    string oldValue = m_PEC;
                    value = Sistema.Formats.ParseEMailAddress(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_PEC = value;
                    DoChanged("PEC", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo della pagina web associata all'ufficio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string WebSite
            {
                get
                {
                    return m_WebSite;
                }

                set
                {
                    string oldValue = m_WebSite;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_WebSite = value;
                    DoChanged("WebSite", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente responsabile dell'ufficio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDResponsabile
            {
                get
                {
                    return DBUtils.GetID(m_Responsabile, m_IDResponsabile);
                }

                set
                {
                    int oldValue = IDResponsabile;
                    if (oldValue == value)
                        return;
                    m_IDResponsabile = value;
                    m_Responsabile = null;
                    DoChanged("IDResponsabile", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente responsabile dell'ufficio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser Responsabile
            {
                get
                {
                    if (m_Responsabile is null)
                        m_Responsabile = Sistema.Users.GetItemById(m_IDResponsabile);
                    return m_Responsabile;
                }

                set
                {
                    var oldValue = Responsabile;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Responsabile = value;
                    m_IDResponsabile = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeResponsabile = value.Nominativo;
                    DoChanged("Responsabile", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente responsabile
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeResponsabile
            {
                get
                {
                    return m_NomeResponsabile;
                }

                set
                {
                    string oldValue = m_NomeResponsabile;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeResponsabile = value;
                    DoChanged("NomeResponsabile", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta i flag relativi all'ufficio
            /// </summary>
            public new UfficioFlags Flags
            {
                get
                {
                    return (UfficioFlags)this.m_Flags;
                }

                set
                {
                    var oldValue = (UfficioFlags) this.m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se l'ufficio è attivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Attivo
            {
                get
                {
                    return m_Attivo;
                }

                set
                {
                    if (m_Attivo == value)
                        return;
                    m_Attivo = value;
                    DoChanged("Attivo", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di apertura dell'ufficio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataApertura
            {
                get
                {
                    return m_DataApertura;
                }

                set
                {
                    var oldValue = m_DataApertura;
                    if (oldValue == value == true)
                        return;
                    m_DataApertura = value;
                    DoChanged("DataApertura", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di chiusura dell'ufficio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataChiusura
            {
                get
                {
                    return m_DataChiusura;
                }

                set
                {
                    var oldValue = m_DataChiusura;
                    if (oldValue == value == true)
                        return;
                    m_DataChiusura = value;
                    DoChanged("DataChiusura", value, oldValue);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Uffici; //.Module;
            }

            /// <summary>
            /// Restituisce o imposta il nome del punto operativo
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
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'azienda a cui appartiene il punto operativo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAzienda
            {
                get
                {
                    return DBUtils.GetID(m_Azienda, m_IDAzienda);
                }

                set
                {
                    int oldValue = IDAzienda;
                    if (oldValue == value)
                        return;
                    m_IDAzienda = value;
                    m_Azienda = null;
                    DoChanged("IDAzienda", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'azienda a cui appartiene l'ufficio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeAzienda
            {
                get
                {
                    return m_NomeAzienda;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeAzienda;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAzienda = value;
                    DoChanged("NomeAzienda", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'oggetto azienda a cui appartiene il punto operativo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CAzienda Azienda
            {
                get
                {
                    if (m_Azienda is null)
                    {
                        if (m_IDAzienda == DBUtils.GetID(Aziende.AziendaPrincipale, 0))
                        {
                            m_Azienda = Aziende.AziendaPrincipale;
                        }
                        else
                        {
                            m_Azienda = Aziende.GetItemById(m_IDAzienda);
                        }
                    }

                    return m_Azienda;
                }

                set
                {
                    var oldValue = Azienda;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    if (oldValue is object)
                    {
                        oldValue.InternalRemoveUfficio(this);
                    }

                    m_Azienda = value;
                    m_IDAzienda = DBUtils.GetID(value, 0);
                    if (value is object)
                    {
                        m_NomeAzienda = value.Nominativo;
                        value.InternalAddUfficio(this);
                    }

                    DoChanged("Azienda", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'azienda
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetAzienda(CAzienda value)
            {
                m_Azienda = value;
                m_IDAzienda = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce una collezione di CUser contenete gli utenti appartenenti all'ufficio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CUtentiXUfficioCollection Utenti
            {
                get
                {
                    if (m_Utenti is null)
                        m_Utenti = new CUtentiXUfficioCollection(this);
                    return m_Utenti;
                }
            }

            /// <summary>
            /// Restituisce o imposta il valore minimo dei ricontatti che un ufficio deve generare in un giorno feriale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int MinimoRicontatti
            {
                get
                {
                    return m_MinimoRicontatti;
                }

                set
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException("MinimoRicontatti deve essere non negativo");
                    int oldValue = m_MinimoRicontatti;
                    if (oldValue == value)
                        return;
                    m_MinimoRicontatti = value;
                    DoChanged("MinimoRicontatti", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'indirizzo dell'ufficio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CIndirizzo Indirizzo
            {
                get
                {
                    return m_Indirizzo;
                }
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_AziendaUffici";
            }

            /// <summary>
            /// Restituisce true se l'oggetto è stato modificato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                return base.IsChanged() || m_Indirizzo.IsChanged();
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                m_Indirizzo.SetChanged(false);
                if (IDAzienda == DBUtils.GetID(Aziende.AziendaPrincipale, 0))
                {
                    Aziende.AziendaPrincipale.InternalUpdateUfficio(this);
                }
            }

            /// <summary>
            /// OnAfterDelete
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterDelete(DMDEventArgs e)
            {
                base.OnAfterDelete(e);
                if (IDAzienda == DBUtils.GetID(Aziende.AziendaPrincipale, 0))
                {
                    Aziende.AziendaPrincipale.InternalUpdateUfficio(this);
                }
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", this.m_Nome);
                m_IDAzienda = reader.Read("IDAzienda", this.m_IDAzienda);
                m_NomeAzienda = reader.Read("NomeAzienda", this.m_NomeAzienda);
                m_MinimoRicontatti = reader.Read("MinimoRicontatti", this.m_MinimoRicontatti);
                m_Attivo = reader.Read("Attivo", this.m_Attivo);
                m_DataApertura = reader.Read("DataApertura", this.m_DataApertura);
                m_DataChiusura = reader.Read("DataChiusura", this.m_DataChiusura);
                m_Indirizzo.ToponimoEVia = reader.Read("Via", m_Indirizzo.ToponimoEVia);
                m_Indirizzo.Civico = reader.Read("Civico", m_Indirizzo.Civico);
                m_Indirizzo.CAP = reader.Read("CAP", m_Indirizzo.CAP);
                m_Indirizzo.Citta = reader.Read("Comune", m_Indirizzo.Citta);
                m_Indirizzo.Provincia = reader.Read("Provincia", m_Indirizzo.Provincia);
                m_Indirizzo.Latitude = reader.Read("Lat", m_Indirizzo.Latitude);
                m_Indirizzo.Longitude = reader.Read("Lng", m_Indirizzo.Longitude);
                m_Indirizzo.Altitude = reader.Read("Alt", m_Indirizzo.Altitude);
                m_Telefono = reader.Read("Telefono", this.m_Telefono);
                m_Telefono1 = reader.Read("Telefono1", this.m_Telefono1);
                m_Fax = reader.Read("Fax", this.m_Fax);
                m_Fax1 = reader.Read("Fax1", this.m_Fax1);
                m_EMail = reader.Read("EMail", this.m_EMail);
                m_PEC = reader.Read("PEC", this.m_PEC);
                m_WebSite = reader.Read("WebSite", this.m_WebSite);
                m_CodiceFiscale = reader.Read("CodiceFiscale", this.m_CodiceFiscale);
                m_IDResponsabile = reader.Read("IDResponsabile", this.m_IDResponsabile);
                m_NomeResponsabile = reader.Read("NomeResponsabile", this.m_NomeResponsabile);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", this.m_Nome);
                writer.Write("NomeAzienda", this.m_NomeAzienda);
                writer.Write("IDAzienda", this.IDAzienda);
                writer.Write("MinimoRicontatti", this.m_MinimoRicontatti);
                writer.Write("Attivo", this.m_Attivo);
                writer.Write("DataApertura", this.m_DataApertura);
                writer.Write("DataChiusura", this.m_DataChiusura);
                writer.Write("Via", this.m_Indirizzo.ToponimoEVia);
                writer.Write("Civico", this.m_Indirizzo.Civico);
                writer.Write("CAP", this.m_Indirizzo.CAP);
                writer.Write("Comune", this.m_Indirizzo.Citta);
                writer.Write("Provincia", this.m_Indirizzo.Provincia);
                writer.Write("Lat", this.m_Indirizzo.Latitude);
                writer.Write("Lng", this.m_Indirizzo.Longitude);
                writer.Write("Alt", this.m_Indirizzo.Altitude);
                writer.Write("Telefono", this.m_Telefono);
                writer.Write("Telefono1", this.m_Telefono1);
                writer.Write("Fax", this.m_Fax);
                writer.Write("Fax1", this.m_Fax1);
                writer.Write("EMail", this.m_EMail);
                writer.Write("PEC", this.m_PEC);
                writer.Write("WebSite", this.m_WebSite);
                writer.Write("IDResponsabile", this.IDResponsabile);
                writer.Write("NomeResponsabile", this.m_NomeResponsabile);
                writer.Write("CodiceFiscale", this.m_CodiceFiscale);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("NomeAzienda", typeof(string), 255);
                c = table.Fields.Ensure("IDAzienda", typeof(int), 1);
                c = table.Fields.Ensure("MinimoRicontatti", typeof(int), 1);
                c = table.Fields.Ensure("Attivo", typeof(bool), 1);
                c = table.Fields.Ensure("DataApertura", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataChiusura", typeof(DateTime), 1);
                c = table.Fields.Ensure("Via", typeof(string), 255);
                c = table.Fields.Ensure("Civico", typeof(string), 255);
                c = table.Fields.Ensure("CAP", typeof(string), 255);
                c = table.Fields.Ensure("Comune", typeof(string), 255);
                c = table.Fields.Ensure("Provincia", typeof(string), 255);
                c = table.Fields.Ensure("Nazione", typeof(string), 255);
                c = table.Fields.Ensure("Lat", typeof(double), 1);
                c = table.Fields.Ensure("Lng", typeof(double), 1);
                c = table.Fields.Ensure("Alt", typeof(double), 1);
                c = table.Fields.Ensure("Telefono", typeof(string), 255);
                c = table.Fields.Ensure("Telefono1", typeof(string), 255);
                c = table.Fields.Ensure("Fax", typeof(string), 255);
                c = table.Fields.Ensure("Fax1", typeof(string), 255);
                c = table.Fields.Ensure("EMail", typeof(string), 255);
                c = table.Fields.Ensure("PEC", typeof(string), 255);
                c = table.Fields.Ensure("WebSite", typeof(string), 255);
                c = table.Fields.Ensure("IDResponsabile", typeof(int), 255);
                c = table.Fields.Ensure("NomeResponsabile", typeof(string), 255);
                c = table.Fields.Ensure("CodiceFiscale", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "IDAzienda", "Nome", "NomeAzienda" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataApertura", "DataChiusura", "Flag", "Attivo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndirizzo1", new string[] { "Nazione", "Provincia", "Civico" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndirizzo2", new string[] { "CAP", "Comune", "Via" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndirizzo3", new string[] { "Lat", "Lng", "Alt" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRecapiti1", new string[] { "Telefono","Fax", "Telefono1", "Fax1" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRecapiti2", new string[] { "EMail", "PEC", "WebSite" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxResponsabile", new string[] { "IDResponsabile", "NomeResponsabile" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCF", new string[] { "CodiceFiscale" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("MinimoRicontatti", typeof(int), 1);                                
                //c = table.Fields.Ensure("Parameters", typeof(string), 0);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_Nome;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_NomeAzienda, this.m_Nome);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CUfficio) && this.Equals((CUfficio)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CUfficio obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && DMD.Integers.EQ(this.m_IDAzienda, obj.m_IDAzienda)
                    && DMD.Strings.EQ(this.m_NomeAzienda, obj.m_NomeAzienda)
                    && this.m_Indirizzo.Equals(obj.m_Indirizzo)
                    && DMD.Strings.EQ(this.m_Telefono, obj.m_Telefono)
                    && DMD.Strings.EQ(this.m_Telefono1, obj.m_Telefono1)
                    && DMD.Strings.EQ(this.m_Fax, obj.m_Fax)
                    && DMD.Strings.EQ(this.m_Fax1, obj.m_Fax1)
                    && DMD.Strings.EQ(this.m_EMail, obj.m_EMail)
                    && DMD.Strings.EQ(this.m_PEC, obj.m_PEC)
                    && DMD.Strings.EQ(this.m_WebSite, obj.m_WebSite)
                    && DMD.Integers.EQ(this.m_MinimoRicontatti, obj.m_MinimoRicontatti)
                    && DMD.Booleans.EQ(this.m_Attivo, obj.m_Attivo)
                    && DMD.DateUtils.EQ(this.m_DataApertura, obj.m_DataApertura)
                    && DMD.DateUtils.EQ(this.m_DataChiusura, obj.m_DataChiusura)
                    && DMD.Integers.EQ(this.m_IDResponsabile, obj.m_IDResponsabile)
                    && DMD.Strings.EQ(this.m_NomeResponsabile, obj.m_NomeResponsabile)
                    && DMD.Strings.EQ(this.m_CodiceFiscale, obj.m_CodiceFiscale)
                    ;
                    //[NonSerialized] private CContattiPerPersonaCollection m_Recapiti;
                    //private CKeyCollection m_Parameters;
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("IDAzienda", IDAzienda);
                writer.WriteAttribute("NomeAzienda", m_NomeAzienda);
                writer.WriteAttribute("MinimoRicontatti", m_MinimoRicontatti);
                writer.WriteAttribute("Attivo", m_Attivo);
                writer.WriteAttribute("DataApertura", m_DataApertura);
                writer.WriteAttribute("DataChiusura", m_DataChiusura);
                writer.WriteAttribute("Telefono", m_Telefono);
                writer.WriteAttribute("Telefono1", m_Telefono1);
                writer.WriteAttribute("Fax", m_Fax);
                writer.WriteAttribute("Fax1", m_Fax1);
                writer.WriteAttribute("eMail", m_EMail);
                writer.WriteAttribute("PEC", m_PEC);
                writer.WriteAttribute("WebSite", m_WebSite);
                writer.WriteAttribute("IDResponsabile", IDResponsabile);
                writer.WriteAttribute("NomeResponsabile", m_NomeResponsabile);
                writer.WriteAttribute("CodiceFiscale", m_CodiceFiscale);
                base.XMLSerialize(writer);
                writer.WriteTag("Indirizzo", m_Indirizzo);
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
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAzienda":
                        {
                            m_IDAzienda = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAzienda":
                        {
                            m_NomeAzienda = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Indirizzo":
                        {
                            m_Indirizzo = (CIndirizzo)fieldValue;
                            break;
                        }

                    case "MinimoRicontatti":
                        {
                            m_MinimoRicontatti = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Attivo":
                        {
                            m_Attivo = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "DataApertura":
                        {
                            m_DataApertura = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataChiusura":
                        {
                            m_DataChiusura = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Telefono":
                        {
                            m_Telefono = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Telefono1":
                        {
                            m_Telefono1 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Fax":
                        {
                            m_Fax = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Fax1":
                        {
                            m_Fax1 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "eMail":
                        {
                            m_EMail = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PEC":
                        {
                            m_PEC = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "WebSite":
                        {
                            m_WebSite = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    
                    case "IDResponsabile":
                        {
                            m_IDResponsabile = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeResponsabile":
                        {
                            m_NomeResponsabile = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CodiceFiscale":
                        {
                            m_CodiceFiscale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Restituisce true se l'ufficio è valido alla data corrente
            /// </summary>
            /// <returns></returns>
            public bool IsValid()
            {
                return IsValid(DMD.DateUtils.Now());
            }

            /// <summary>
            /// Restituisce true se l'ufficio è valido alla data specificata
            /// </summary>
            /// <param name="atDate"></param>
            /// <returns></returns>
            public bool IsValid(DateTime atDate)
            {
                return m_Attivo && DMD.DateUtils.CheckBetween(atDate, m_DataApertura, m_DataChiusura);
            }

            /// <summary>
            /// Compara due uffici
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(CUfficio other)
            {
                return DMD.Strings.Compare(m_Nome, other.m_Nome, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CUfficio)obj);
            }
        }
    }
}