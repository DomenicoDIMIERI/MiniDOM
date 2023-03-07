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

namespace minidom.PBX
{

    /// <summary>
    /// Flag validi per una configurazione
    /// </summary>
    [Flags]
    public enum DMDSIPConfigFlags : int
    {
        /// <summary>
        /// Nessun flag
        /// </summary>
        None = 0,

        /// <summary>
        /// Se vero indica al programma di mostrare una finestra per le telefonate in ingresso
        /// </summary>
        ShowInCall = 4
    }

    /// <summary>
    /// Configurazione per un utente su una macchina
    /// </summary>
    [Serializable]
    public sealed class DMDSIPConfig 
        : Databases.DBObjectBase
    {
        private string m_IDPostazione;        // Stringa che identifica la postazione a cui fa riferimento la configurazione
        private string m_IDMacchina;          // Stringa che identifica la macchina a cui fa riferimento la configurazione
        private string m_IDUtente;            // Strings che identifica l'utente collegato alla macchina
        private bool m_Attiva;             // Se vero indica che la configurazione è attiva
        private DateTime? m_DataInizio;           // Data di inizio validità della configurazione
        private DateTime? m_DataFine;             // Data di fine validità della configurazione
        private CCollection<AsteriskServer> m_AsteriskServers;    // Collezione dei server asterisk
        private CCollection<DispositivoEsterno> m_Dispositivi;     // Collezione dei dispositivi configurati
        private CCollection<LineaEsterna> m_Linee;                 // Configurazione delle linee disponibili per il centralino
        private CCollection<string> m_CartelleMonitorate;  // Elenco delle cartelle monitorate
        private CCollection<string> m_CartelleEscluse;  // Elenco delle cartelle monitorate
        private string m_ServerName;          // Server da cui vengono scaricate le informazioni
        private string m_UserName;            // Utente che viene utilizzato per identificarsi sul server delle informazioni
        private string m_UploadServer;        // Server a cui vengono inviati i files caricati nelle cartelle monitorate
        private string m_NotifyServer;        // Server a cui vengono inviati gli screenshot
        //private DMDSIPConfigFlags m_Flags;
        private int m_RemoveLogAfterNDays;
        private string m_MinVersion;          // Versione minima del programma per cui è applicabile la configurazione
        private string m_MaxVersion;          // Versione massima del programma per cui è applicabile la configurazione

        /// <summary>
        /// Costruttore
        /// </summary>
        public DMDSIPConfig()
        {
            m_IDPostazione = "";
            m_IDMacchina = "";
            m_IDUtente = "";
            m_Attiva = true;
            m_DataInizio = default;
            m_DataFine = default;
            m_AsteriskServers = null;
            m_Dispositivi = null;
            m_Linee = null;
            m_CartelleMonitorate = null;
            m_CartelleEscluse = null;
            m_Flags = (int)DMDSIPConfigFlags.None;
            m_UploadServer = "";
            m_NotifyServer = "";
            m_ServerName = "";
            m_UserName = "";
            m_RemoveLogAfterNDays = 7;
            m_MinVersion = "";
            m_MaxVersion = "";
        }

        /// <summary>
        /// Restituisce rue se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public sealed override bool Equals(Databases.DBObjectBase obj)
        {
            return (obj is DMDSIPConfig) && this.Equals((DMDSIPConfig)obj);
        }

        /// <summary>
        /// Restituisce rue se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Equals(DMDSIPConfig obj)
        {
            return base.Equals(obj)
                && DMD.Strings.EQ(this.m_IDPostazione, obj.m_IDPostazione)
                && DMD.Strings.EQ(this.m_IDMacchina, obj.m_IDMacchina)
                && DMD.Strings.EQ(this.m_IDUtente, obj.m_IDUtente)
                && DMD.Booleans.EQ(this.m_Attiva, obj.m_Attiva)
                && DMD.DateUtils.EQ(this.m_DataInizio, obj.m_DataInizio)
                && DMD.DateUtils.EQ(this.m_DataFine, obj.m_DataFine)
                && DMD.Strings.EQ(this.m_ServerName, obj.m_ServerName)
                && DMD.Strings.EQ(this.m_UserName, obj.m_UserName)
                && DMD.Strings.EQ(this.m_UploadServer, obj.m_UploadServer)
                && DMD.Strings.EQ(this.m_NotifyServer, obj.m_NotifyServer)
                && DMD.Integers.EQ(this.m_RemoveLogAfterNDays, obj.m_RemoveLogAfterNDays)
                && DMD.Strings.EQ(this.m_MinVersion, obj.m_MinVersion)
                && DMD.Strings.EQ(this.m_MaxVersion, obj.m_MaxVersion)
            //private CCollection<AsteriskServer> m_AsteriskServers;    // Collezione dei server asterisk
            //private CCollection<DispositivoEsterno> m_Dispositivi;     // Collezione dei dispositivi configurati
            //private CCollection<LineaEsterna> m_Linee;                 // Configurazione delle linee disponibili per il centralino
            //private CCollection<string> m_CartelleMonitorate;  // Elenco delle cartelle monitorate
            //private CCollection<string> m_CartelleEscluse;  // Elenco delle cartelle monitorate
            ;

        }

        /// <summary>
        /// Se vero indica al programma di mostrare una finestra per le telefonate in ingresso
        /// </summary>
        /// <returns></returns>
        public bool ShowInCall
        {
            get
            {
                return Sistema.TestFlag(m_Flags, DMDSIPConfigFlags.ShowInCall);
            }

            set
            {
                if (ShowInCall == value)
                    return;
                m_Flags = Sistema.SetFlag(m_Flags, DMDSIPConfigFlags.ShowInCall, value);
                DoChanged("ShowInCall", value, !value);
            }
        }

        /// <summary>
        /// Restituisce o imposta la versione minima del programma per cui è applicabile la configurazione
        /// </summary>
        /// <returns></returns>
        public string MinVersion
        {
            get
            {
                return m_MinVersion;
            }

            set
            {
                string oldValue = m_MinVersion;
                value = DMD.Strings.Trim(value);
                if ((oldValue ?? "") == (value ?? ""))
                    return;
                m_MinVersion = value;
                DoChanged("MinVersion", value, oldValue);
            }
        }

        /// <summary>
        /// Restituisce o imposta la versione massima del programma per cui è applicabile la configurazione
        /// </summary>
        /// <returns></returns>
        public string MaxVersion
        {
            get
            {
                return m_MaxVersion;
            }

            set
            {
                string oldValue = m_MaxVersion;
                value = DMD.Strings.Trim(value);
                if ((oldValue ?? "") == (value ?? ""))
                    return;
                m_MaxVersion = value;
                DoChanged("MaxVersion", value, oldValue);
            }
        }


        /// <summary>
        /// Restituisce o imposta il numero di giorni in cui conservare i log.
        /// Un valore pari a 0 indica nessun log.
        /// Un valore negativo indica che i log non verranno mai eliminati
        /// </summary>
        /// <returns></returns>
        public int RemoveLogAfterNDays
        {
            get
            {
                return m_RemoveLogAfterNDays;
            }

            set
            {
                int oldValue = m_RemoveLogAfterNDays;
                if (oldValue == value)
                    return;
                m_RemoveLogAfterNDays = value;
                DoChanged("RemoveLogAfterNDays", value, oldValue);
            }
        }


        /// <summary>
        /// Server da cui vengono scaricate le informazioni
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ServerName
        {
            get
            {
                return m_ServerName;
            }

            set
            {
                string oldValue = m_ServerName;
                value = DMD.Strings.Trim(value);
                if ((oldValue ?? "") == (value ?? ""))
                    return;
                m_ServerName = value;
                DoChanged("ServerName", value, oldValue);
            }
        }

        /// <summary>
        /// Restituisce o imposta l'utente che viene utilizzato per identificarsi sul server delle informazioni
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string UserName
        {
            get
            {
                return m_UserName;
            }

            set
            {
                string oldValue = m_UserName;
                value = DMD.Strings.Trim(value);
                if ((oldValue ?? "") == (value ?? ""))
                    return;
                m_UserName = value;
                DoChanged("UserName", value, oldValue);
            }
        }

        /// <summary>
        /// Restituisce o imposta il server a cui vengono inviati i files delle cartelle monitorate
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string UploadServer
        {
            get
            {
                return m_UploadServer;
            }

            set
            {
                string oldValue = m_UploadServer;
                value = DMD.Strings.Trim(value);
                if ((oldValue ?? "") == (value ?? ""))
                    return;
                m_UploadServer = value;
                DoChanged("UploadServer", value, oldValue);
            }
        }

        /// <summary>
        /// Restituisce o imposta il server delle notifiche
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string NotifyServer
        {
            get
            {
                return m_NotifyServer;
            }

            set
            {
                string oldValue = m_NotifyServer;
                value = DMD.Strings.Trim(value);
                if ((oldValue ?? "") == (value ?? ""))
                    return;
                m_NotifyServer = value;
                DoChanged("NotifyServer", value, oldValue);
            }
        }

        /// <summary>
        /// Restituisce o imposta l'ID della postazione associata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string IDPostazione
        {
            get
            {
                return m_IDPostazione;
            }

            set
            {
                string oldValue = m_IDPostazione;
                value = DMD.Strings.Trim(value);
                if ((oldValue ?? "") == (value ?? ""))
                    return;
                m_IDPostazione = value;
                DoChanged("IDPostazione", value, oldValue);
            }
        }

        /// <summary>
        /// Restituisce o imposta l'ID della macchina associata
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string IDMacchina
        {
            get
            {
                return m_IDMacchina;
            }

            set
            {
                string oldValue = m_IDMacchina;
                value = DMD.Strings.Trim(value);
                if ((oldValue ?? "") == (value ?? ""))
                    return;
                m_IDMacchina = value;
                DoChanged("IDMacchina", value, oldValue);
            }
        }

        /// <summary>
        /// Restituisce o imposta l'ID dell'utente associato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string IDUtente
        {
            get
            {
                return m_IDUtente;
            }

            set
            {
                string oldValue = m_IDUtente;
                value = DMD.Strings.Trim(value);
                if ((oldValue ?? "") == (value ?? ""))
                    return;
                m_IDUtente = value;
                DoChanged("IDUtente", value, oldValue);
            }
        }

        /// <summary>
        /// Restituisce o imposta un valore booleano che indica se la configurazione è attiva
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Attiva
        {
            get
            {
                return m_Attiva;
            }

            set
            {
                if (m_Attiva == value)
                    return;
                m_Attiva = value;
                DoChanged("Attiva", value, !value);
            }
        }

        /// <summary>
        /// Data di inizio
        /// </summary>
        public DateTime? DataInizio
        {
            get
            {
                return m_DataInizio;
            }

            set
            {
                var oldValue = m_DataInizio;
                if (DMD.DateUtils.Compare(value, oldValue) == 0)
                    return;
                m_DataInizio = value;
                DoChanged("DataInizio", value, oldValue);
            }
        }

        /// <summary>
        /// Data di fine
        /// </summary>
        public DateTime? DataFine
        {
            get
            {
                return m_DataFine;
            }

            set
            {
                var oldValue = m_DataFine;
                if (DMD.DateUtils.Compare(value, oldValue) == 0)
                    return;
                m_DataFine = value;
                DoChanged("DataFine", value, oldValue);
            }
        }

        /// <summary>
        /// Servers
        /// </summary>
        public CCollection<AsteriskServer> AsteriskServers
        {
            get
            {
                if (m_AsteriskServers is null)
                    m_AsteriskServers = new CCollection<AsteriskServer>();
                return m_AsteriskServers;
            }
        }

        /// <summary>
        /// Dispositivi
        /// </summary>
        public CCollection<DispositivoEsterno> Dispositivi
        {
            get
            {
                if (m_Dispositivi is null)
                    m_Dispositivi = new CCollection<DispositivoEsterno>();
                return m_Dispositivi;
            }
        }

        /// <summary>
        /// Linee
        /// </summary>
        public CCollection<LineaEsterna> Linee
        {
            get
            {
                if (m_Linee is null)
                    m_Linee = new CCollection<LineaEsterna>();
                return m_Linee;
            }
        }

        /// <summary>
        /// Flag relativi alla configurazione
        /// </summary>
        public DMDSIPConfigFlags Flags
        {
            get
            {
                return (DMDSIPConfigFlags) base.Flags;
            }

            set
            {
                var oldValue = this.Flags;
                if (oldValue == value)
                    return;
                m_Flags = (int)value;
                DoChanged("Flags", value, oldValue);
            }
        }
         
        /// <summary>
        /// Restituisce una stringa che rappresenta l'oggetto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DMD.Strings.ConcatArray( m_IDMacchina , "." , m_IDPostazione , "." , m_IDUtente);
        }

        /// <summary>
        /// Restituisce il codice hash dell'oggetto
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCalculator.Calculate(this.m_IDUtente);
        }

        /// <summary>
        /// Restituisce la collezione delle cartelle che vengono monitorate per la creazione, modifica ed eliminazione di files
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CCollection<string> CartelleMonitorate
        {
            get
            {
                if (m_CartelleMonitorate is null)
                    m_CartelleMonitorate = new CCollection<string>();
                return m_CartelleMonitorate;
            }
        }

        /// <summary>
        /// Restituisce la collezione delle cartelle che vengono escluse dal monitoraggio dei files (le cartelle escluse hanno precedenza)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CCollection<string> CartelleEscluse
        {
            get
            {
                if (m_CartelleEscluse is null)
                    m_CartelleEscluse = new CCollection<string>();
                return m_CartelleEscluse;
            }
        }
         
        /// <summary>
        /// Repository
        /// </summary>
        /// <returns></returns>
        public override CModulesClass GetModule()
        {
            return DMDSIPApp.Configs;
        }

        /// <summary>
        /// Discriminator
        /// </summary>
        /// <returns></returns>
        public override string GetTableName()
        {
            return "tbl_DialTPConfig";
        }

        /// <summary>
        /// Carica dal db
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override bool LoadFromRecordset(DBReader reader)
        {
            m_IDPostazione = reader.Read("IDPostazione", this.m_IDPostazione);
            m_IDMacchina = reader.Read("IDMacchina", this.m_IDMacchina);
            m_IDUtente = reader.Read("IDUtente", this.m_IDUtente);
            m_Attiva = reader.Read("Attiva", this.m_Attiva);
            m_DataInizio = reader.Read("DataInizio", this.m_DataInizio);
            m_DataFine = reader.Read("DataFine", this.m_DataFine);
            m_UploadServer = reader.Read("UploadServer", this.m_UploadServer);
            m_NotifyServer = reader.Read("NotifyServer", this.m_NotifyServer);
            m_ServerName = reader.Read("ServerName", this.m_ServerName);
            m_UserName = reader.Read("UserName", this.m_UserName);
            m_RemoveLogAfterNDays = reader.Read("RemoveLogAfterNDays", this.m_RemoveLogAfterNDays);
            m_MinVersion = reader.Read("MinVersion", this.m_MinVersion);
            m_MaxVersion = reader.Read("MaxVersion", this.m_MaxVersion);
            string tmp = reader.Read("AsteriskServers", "");
            if (!string.IsNullOrEmpty(tmp))
            {
                m_AsteriskServers = new CCollection<AsteriskServer>();
                m_AsteriskServers.AddRange((CCollection)DMD.XML.Utils.Serializer.Deserialize(tmp));
            }

            tmp = reader.Read("Dispositivi", "");
            if (!string.IsNullOrEmpty(tmp))
            {
                m_Dispositivi = new CCollection<DispositivoEsterno>();
                m_Dispositivi.AddRange((CCollection)DMD.XML.Utils.Serializer.Deserialize(tmp));
            }

            tmp = reader.Read("Linee", "");
            if (!string.IsNullOrEmpty(tmp))
            {
                m_Linee = new CCollection<LineaEsterna>();
                m_Linee.AddRange((CCollection)DMD.XML.Utils.Serializer.Deserialize(tmp));
            }

            tmp = reader.Read("CartelleMonitorate", "");
            if (!string.IsNullOrEmpty(tmp))
            {
                m_CartelleMonitorate = new CCollection<string>();
                m_CartelleMonitorate.AddRange((CCollection)DMD.XML.Utils.Serializer.Deserialize(tmp));
            }

            tmp = reader.Read("CartelleEscluse", "");
            if (!string.IsNullOrEmpty(tmp))
            {
                m_CartelleEscluse = new CCollection<string>();
                m_CartelleEscluse.AddRange((CCollection)DMD.XML.Utils.Serializer.Deserialize(tmp));
            }

            return base.LoadFromRecordset(reader);
        }

        /// <summary>
        /// Salva nel db
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected override bool SaveToRecordset(DBWriter writer)
        {
            writer.Write("IDPostazione", m_IDPostazione);
            writer.Write("IDMacchina", m_IDMacchina);
            writer.Write("IDUtente", m_IDUtente);
            writer.Write("Attiva", m_Attiva);
            writer.Write("DataInizio", m_DataInizio);
            writer.Write("DataFine", m_DataFine);
            writer.Write("Flags", m_Flags);
            writer.Write("AsteriskServers", DMD.XML.Utils.Serializer.Serialize(AsteriskServers));
            writer.Write("Dispositivi", DMD.XML.Utils.Serializer.Serialize(Dispositivi));
            writer.Write("Linee", DMD.XML.Utils.Serializer.Serialize(Linee));
            writer.Write("CartelleMonitorate", DMD.XML.Utils.Serializer.Serialize(CartelleMonitorate));
            writer.Write("CartelleEscluse", DMD.XML.Utils.Serializer.Serialize(CartelleEscluse));
            writer.Write("UploadServer", m_UploadServer);
            writer.Write("NotifyServer", m_NotifyServer);
            writer.Write("ServerName", m_ServerName);
            writer.Write("UserName", m_UserName);
            writer.Write("RemoveLogAfterNDays", m_RemoveLogAfterNDays);
            writer.Write("MinVersion", m_MinVersion);
            writer.Write("MaxVersion", m_MaxVersion);
            return base.SaveToRecordset(writer);
        }

        /// <summary>
        /// Prepara lo schema
        /// </summary>
        /// <param name="table"></param>
        protected override void PrepareDBSchemaFields(DBTable table)
        {
            base.PrepareDBSchemaFields(table);

            var c = table.Fields.Ensure("IDPostazione", typeof(int), 1);
            c = table.Fields.Ensure("IDMacchina", typeof(int), 1);
            c = table.Fields.Ensure("IDUtente", typeof(int), 1);
            c = table.Fields.Ensure("Attiva", typeof(bool), 1);
            c = table.Fields.Ensure("DataInizio", typeof(DateTime), 1);
            c = table.Fields.Ensure("DataInizio", typeof(DateTime), 1);
            c = table.Fields.Ensure("AsteriskServers", typeof(string), 0);
            c = table.Fields.Ensure("Dispositivi", typeof(string), 0);
            c = table.Fields.Ensure("Linee", typeof(string), 0);
            c = table.Fields.Ensure("CartelleMonitorate", typeof(string), 0);
            c = table.Fields.Ensure("CartelleEscluse", typeof(string), 0);
            c = table.Fields.Ensure("UploadServer", typeof(string), 255);
            c = table.Fields.Ensure("NotifyServer", typeof(string), 255);
            c = table.Fields.Ensure("ServerName", typeof(string), 255);
            c = table.Fields.Ensure("UserName", typeof(string), 255);
            c = table.Fields.Ensure("RemoveLogAfterNDays", typeof(bool), 1);
            c = table.Fields.Ensure("MinVersion", typeof(string), 255);
            c = table.Fields.Ensure("MaxVersion", typeof(string), 255);
             
        }


        /// <summary>
        /// Prepara i campi
        /// </summary>
        /// <param name="table"></param>
        protected override void PrepareDBSchemaConstraints(DBTable table)
        {
            base.PrepareDBSchemaConstraints(table);

            var c = table.Constraints.Ensure("idxUtente", new string[] { "IDPostazione", "IDMacchina", "IDUtente", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
            c = table.Constraints.Ensure("idxDate", new string[] { "DataInizio", "DataInizio" }, DBFieldConstraintFlags.None);
            c = table.Constraints.Ensure("idxServers", new string[] { "UploadServer", "NotifyServer", "ServerName" }, DBFieldConstraintFlags.None);
            c = table.Constraints.Ensure("idxParams", new string[] { "Attiva", "UserName", "RemoveLogAfterNDays", "MinVersion", "MaxVersion" }, DBFieldConstraintFlags.None);


            //c = table.Fields.Ensure("AsteriskServers", typeof(string), 0);
            //c = table.Fields.Ensure("Dispositivi", typeof(string), 0);
            //c = table.Fields.Ensure("Linee", typeof(string), 0);
            //c = table.Fields.Ensure("CartelleMonitorate", typeof(string), 0);
            //c = table.Fields.Ensure("CartelleEscluse", typeof(string), 0);
              
        }


        /// <summary>
        /// Serializzazione xml
        /// </summary>
        /// <param name="writer"></param>
        protected override void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("IDPostazione", m_IDPostazione);
            writer.WriteAttribute("IDMacchina", m_IDMacchina);
            writer.WriteAttribute("IDUtente", m_IDUtente);
            writer.WriteAttribute("Attiva", m_Attiva);
            writer.WriteAttribute("DataInizio", m_DataInizio);
            writer.WriteAttribute("DataFine", m_DataFine);
            writer.WriteAttribute("UploadServer", m_UploadServer);
            writer.WriteAttribute("NotifyServer", m_NotifyServer);
            writer.WriteAttribute("ServerName", m_ServerName);
            writer.WriteAttribute("UserName", m_UserName);
            writer.WriteAttribute("RemoveLogAfterNDays", m_RemoveLogAfterNDays);
            writer.WriteAttribute("MinVersion", m_MinVersion);
            writer.WriteAttribute("MaxVersion", m_MaxVersion);
            base.XMLSerialize(writer);
            writer.WriteTag("AsteriskServers", AsteriskServers);
            writer.WriteTag("Dispositivi", Dispositivi);
            writer.WriteTag("Linee", Linee);
            writer.WriteTag("CartelleMonitorate", CartelleMonitorate);
            writer.WriteTag("CartelleEscluse", CartelleEscluse);             
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
                case "IDPostazione":
                    {
                        m_IDPostazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "IDMacchina":
                    {
                        m_IDMacchina = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "IDUtente":
                    {
                        m_IDUtente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Attiva":
                    {
                        m_Attiva = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
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
 
                case "UploadServer":
                    {
                        m_UploadServer = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "NotifyServer":
                    {
                        m_NotifyServer = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "ServerName":
                    {
                        m_ServerName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "UserName":
                    {
                        m_UserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "RemoveLogAfterNDays":
                    {
                        m_RemoveLogAfterNDays = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "MinVersion":
                    {
                        m_MinVersion = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "MaxVersion":
                    {
                        m_MaxVersion = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "AsteriskServers":
                    {
                        m_AsteriskServers = new CCollection<AsteriskServer>();
                        m_AsteriskServers.AddRange((CCollection)fieldValue);
                        break;
                    }

                case "Dispositivi":
                    {
                        m_Dispositivi = new CCollection<DispositivoEsterno>();
                        m_Dispositivi.AddRange((CCollection)fieldValue);
                        break;
                    }

                case "Linee":
                    {
                        m_Linee = new CCollection<LineaEsterna>();
                        m_Linee.AddRange((CCollection)fieldValue);
                        break;
                    }

                case "CartelleMonitorate":
                    {
                        m_CartelleMonitorate = new CCollection<string>();
                        m_CartelleMonitorate.AddRange((CCollection)fieldValue);
                        break;
                    }

                case "CartelleEscluse":
                    {
                        m_CartelleEscluse = new CCollection<string>();
                        m_CartelleEscluse.AddRange((CCollection)fieldValue);
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