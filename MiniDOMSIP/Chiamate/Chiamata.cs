using System;
using DMD;
using DMD.Databases;
using DMD.XML;
using minidom.repositories;

namespace minidom.PBX
{

    /// <summary>
    /// Flag di stato di una chiamata
    /// </summary>
    public enum StatoChiamata : int
    {
        /// <summary>
        /// Nessuno
        /// </summary>
        None = 0,

        /// <summary>
        /// Composizione della chiamata in uscita
        /// </summary>
        Dialling = 1,

        /// <summary>
        /// Il telefono sta squillando 
        /// </summary>
        Ringing = 2,

        /// <summary>
        /// La cornetta é stata alzata e la conversazione é in corso
        /// </summary>
        Speaking = 3,

        /// <summary>
        ///  La chiamata é stata messa in pausa
        /// </summary>
        Paused = 4,

        /// <summary>
        /// La cornetta é stata alzata e la conversazione é terminata
        /// </summary>
        Answered = 5,

        /// <summary>
        /// La cornetta non é stata alzata e la chiamata é terminata
        /// </summary>
        NotAnswered = 6
    }


    /// <summary>
    /// Chiamata sul centralino
    /// </summary>
    [Serializable]
    public class Chiamata 
        : minidom.Databases.DBObjectBase
    {
        private string m_ServerIP;
        private string m_ServerName;
        private string m_Channel;
        private string m_SourceNumber;
        private string m_TargetNumber;
        private DateTime? m_StartTime;
        private DateTime? m_AnswerTime;
        private DateTime? m_EndTime;
        private StatoChiamata m_StatoChiamata;
        private int m_Direzione;
        private int m_IDTelefonata;
        [NonSerialized] private CTelefonata m_Telefonata;

        /// <summary>
        /// Costruttore
        /// </summary>
        public Chiamata()
        {
            m_ServerIP = "";
            m_ServerName = "";
            m_Channel = "";
            m_SourceNumber = "";
            m_TargetNumber = "";
            m_StartTime = default;
            m_AnswerTime = default;
            m_EndTime = default;
            m_StatoChiamata = StatoChiamata.None;
            m_Direzione = 0;
            m_Flags = 0;
            m_IDTelefonata = 0;
            m_Telefonata = null;
        }

        /// <summary>
        /// Restituisce una stringa che rappresenta l'oggetto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DMD.Strings.ConcatArray(this.m_SourceNumber,"|", this.TargetNumber, "@", this.m_ServerName , " ", this.m_StartTime);
        }

        /// <summary>
        /// Restituisce il codice hash dell'oggetto
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCalculator.Calculate(this.m_ServerName, this.m_StartTime, this.m_TargetNumber);
        }

        /// <summary>
        /// Restituisce true se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Databases.DBObjectBase obj)
        {
            return (obj is Chiamata) && this.Equals((Chiamata)obj);
        }

        /// <summary>
        /// Restituisce true se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Equals(Chiamata obj)
        {
            return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_ServerIP, obj.m_ServerIP)
                    && DMD.Strings.EQ(this.m_ServerName, obj.m_ServerName)
                    && DMD.Strings.EQ(this.m_Channel, obj.m_Channel)
                    && DMD.Strings.EQ(this.m_SourceNumber, obj.m_SourceNumber)
                    && DMD.Strings.EQ(this.m_TargetNumber, obj.m_TargetNumber)
                    && DMD.DateUtils.EQ(this.m_StartTime, obj.m_StartTime)
                    && DMD.DateUtils.EQ(this.m_AnswerTime, obj.m_AnswerTime)
                    && DMD.DateUtils.EQ(this.m_EndTime, obj.m_EndTime)
                    && DMD.Integers.EQ((int)this.m_StatoChiamata, (int)obj.m_StatoChiamata)
                    && DMD.Integers.EQ(this.m_Direzione, obj.m_Direzione)
                    && DMD.Integers.EQ(this.m_IDTelefonata, obj.m_IDTelefonata)
                    ;
     }
    
        /// <summary>
        /// ID della telefonata
        /// </summary>
        public int IDTelefonata
        {
            get
            {
                return DBUtils.GetID(m_Telefonata, m_IDTelefonata);
            }

            set
            {
                int oldValue = IDTelefonata;
                if (oldValue == value)
                    return;
                m_IDTelefonata = value;
                m_Telefonata = null;
                DoChanged("IDTelefonata", value, oldValue);
            }
        }

        /// <summary>
        /// Telefonata
        /// </summary>
        public CustomerCalls.CTelefonata Telefonata
        {
            get
            {
                if (m_Telefonata is null)
                    m_Telefonata = minidom.CustomerCalls.Telefonate.GetItemById(m_IDTelefonata);
                return m_Telefonata;
            }

            set
            {
                var oldValue = m_Telefonata;
                if (ReferenceEquals(oldValue, value))
                    return;
                m_Telefonata = value;
                m_IDTelefonata = DBUtils.GetID(value, 0);
                DoChanged("Telefonata", value, oldValue);
            }
        }

        /// <summary>
        /// Server ip
        /// </summary>
        public string ServerIP
        {
            get
            {
                return m_ServerIP;
            }

            set
            {
                value = DMD.Strings.Trim(value);
                string oldValue = m_ServerIP;
                if ((oldValue ?? "") == (value ?? ""))
                    return;
                m_ServerIP = value;
                DoChanged("ServerIP", value, oldValue);
            }
        }

        /// <summary>
        /// Server name
        /// </summary>
        public string ServerName
        {
            get
            {
                return m_ServerName;
            }

            set
            {
                value = DMD.Strings.Trim(value);
                string oldValue = m_ServerName;
                if ((oldValue ?? "") == (value ?? ""))
                    return;
                m_ServerName = value;
                DoChanged("ServerName", value, oldValue);
            }
        }

        /// <summary>
        /// Channel
        /// </summary>
        public string Channel
        {
            get
            {
                return m_Channel;
            }

            set
            {
                value = DMD.Strings.Trim(value);
                string oldValue = m_Channel;
                if ((value ?? "") == (oldValue ?? ""))
                    return;
                m_Channel = value;
                DoChanged("Channel", value, oldValue);
            }
        }

        /// <summary>
        /// Numero del chiamante
        /// </summary>
        public string SourceNumber
        {
            get
            {
                return m_SourceNumber;
            }

            set
            {
                value = DMD.Strings.Trim(value);
                string oldValue = m_SourceNumber;
                if ((oldValue ?? "") == (value ?? ""))
                    return;
                m_SourceNumber = value;
                DoChanged("SourceNumber", value, oldValue);
            }
        }

        /// <summary>
        /// Numero del destinatario
        /// </summary>
        public string TargetNumber
        {
            get
            {
                return m_TargetNumber;
            }

            set
            {
                value = DMD.Strings.Trim(value);
                string oldValue = m_TargetNumber;
                if ((oldValue ?? "") == (value ?? ""))
                    return;
                m_TargetNumber = value;
                DoChanged("TargetNumber", value, oldValue);
            }
        }

        /// <summary>
        /// Ora di inizio
        /// </summary>
        public DateTime? StartTime
        {
            get
            {
                return m_StartTime;
            }

            set
            {
                var oldValue = m_StartTime;
                if (DMD.DateUtils.Compare(value, oldValue) == 0)
                    return;
                m_StartTime = value;
                DoChanged("StartTime", value, oldValue);
            }
        }

        /// <summary>
        /// Ora della risposta
        /// </summary>
        public DateTime? AnswerTime
        {
            get
            {
                return m_AnswerTime;
            }

            set
            {
                var oldValue = m_AnswerTime;
                if (DMD.DateUtils.Compare(value, oldValue) == 0)
                    return;
                m_AnswerTime = value;
                DoChanged("AnswerTime", value, oldValue);
            }
        }

        /// <summary>
        /// Ora di termine
        /// </summary>
        public DateTime? EndTime
        {
            get
            {
                return m_EndTime;
            }

            set
            {
                var oldValue = m_EndTime;
                if (DMD.DateUtils.Compare(value, oldValue) == 0)
                    return;
                m_EndTime = value;
                DoChanged("EndTime", value, oldValue);
            }
        }

        /// <summary>
        /// Stato della chiamata
        /// </summary>
        public StatoChiamata StatoChiamata
        {
            get
            {
                return m_StatoChiamata;
            }

            set
            {
                var oldValue = m_StatoChiamata;
                if (oldValue == value)
                    return;
                m_StatoChiamata = value;
                DoChanged("StatoChiamata", value, oldValue);
            }
        }

        /// <summary>
        /// Direzione che indica se la chiamata é in ingresso o in uscita
        /// </summary>
        public int Direzione
        {
            get
            {
                return m_Direzione;
            }

            set
            {
                int oldValue = m_Direzione;
                if (oldValue == value)
                    return;
                m_Direzione = value;
                DoChanged("Direzione", value, oldValue);
            }
        }

          
        /// <summary>
        /// Repository
        /// </summary>
        /// <returns></returns>
        public override CModulesClass GetModule()
        {
            return minidom.PBX.Chiamate.Chiamate;
        }

        /// <summary>
        /// Discriminator
        /// </summary>
        /// <returns></returns>
        public override string GetTableName()
        {
            return "tbl_Telefonate";
        }

        /// <summary>
        /// Carica dal db
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override bool LoadFromRecordset(DBReader reader)
        {
            m_ServerIP = reader.Read("ServerIP", this.m_ServerIP);
            m_ServerName = reader.Read("ServerName", this.m_ServerName);
            m_Channel = reader.Read("Channel", this.m_Channel);
            m_SourceNumber = reader.Read("SourceNumber", this.m_SourceNumber);
            m_TargetNumber = reader.Read("TargetNumber", this.m_TargetNumber);
            m_StartTime = reader.Read("StartTime", this.m_StartTime);
            m_AnswerTime = reader.Read("AnswerTime", this.m_AnswerTime);
            m_EndTime = reader.Read("EndTime", this.m_EndTime);
            m_StatoChiamata = reader.Read("StatoChiamata", this.m_StatoChiamata);
            m_Direzione = reader.Read("Direzione", this.m_Direzione);
            m_IDTelefonata = reader.Read("IDTelefonata", this.m_IDTelefonata);
            return base.LoadFromRecordset(reader);
        }

        /// <summary>
        /// Salva nel db
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected override bool SaveToRecordset(DBWriter writer)
        {
            writer.Write("ServerIP", m_ServerIP);
            writer.Write("ServerName", m_ServerName);
            writer.Write("Channel", m_Channel);
            writer.Write("SourceNumber", m_SourceNumber);
            writer.Write("TargetNumber", m_TargetNumber);
            writer.Write("StartTime", m_StartTime);
            writer.Write("AnswerTime", m_AnswerTime);
            writer.Write("EndTime", m_EndTime);
            writer.Write("StatoChiamata", m_StatoChiamata);
            writer.Write("Direzione", m_Direzione);
            writer.Write("IDTelefonata", IDTelefonata);
            return base.SaveToRecordset(writer);
        }

        /// <summary>
        /// Prepara lo schema
        /// </summary>
        /// <param name="table"></param>
        protected override void PrepareDBSchemaFields(DBTable table)
        {
            base.PrepareDBSchemaFields(table);

            var c = table.Fields.Ensure("ServerIP", typeof(string), 255);
            c = table.Fields.Ensure("ServerName", typeof(string), 255);
            c = table.Fields.Ensure("Channel", typeof(string), 255);
            c = table.Fields.Ensure("SourceNumber", typeof(string), 255);
            c = table.Fields.Ensure("TargetNumber", typeof(string), 255);
            c = table.Fields.Ensure("StartTime", typeof(DateTime), 1);
            c = table.Fields.Ensure("AnswerTime", typeof(DateTime), 1);
            c = table.Fields.Ensure("EndTime", typeof(DateTime), 1);
            c = table.Fields.Ensure("StatoChiamata", typeof(int), 1);
            c = table.Fields.Ensure("Direzione", typeof(string), 0);
            c = table.Fields.Ensure("IDTelefonata", typeof(int), 1);
             
        }

        /// <summary>
        /// Prepara i vincoli
        /// </summary>
        /// <param name="table"></param>
        protected override void PrepareDBSchemaConstraints(DBTable table)
        {
            base.PrepareDBSchemaConstraints(table);

            var c = table.Constraints.Ensure("idxServer", new string[] { "ServerIP", "ServerName", "Channel" }, DBFieldConstraintFlags.None);
            c = table.Constraints.Ensure("idxNumbers", new string[] { "SourceNumber", "TargetNumber" }, DBFieldConstraintFlags.None);
            c = table.Constraints.Ensure("idxDates", new string[] { "StartTime", "EndTime", "AnswerTime" }, DBFieldConstraintFlags.None);
            c = table.Constraints.Ensure("idxStatoChiamata", new string[] { "StatoChiamata", "IDTelefonata", "Direzione" }, DBFieldConstraintFlags.None);
             

        }

        /// <summary>
        /// Serializzazione xml
        /// </summary>
        /// <param name="writer"></param>
        protected override void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("ServerIP", m_ServerIP);
            writer.WriteAttribute("ServerName", m_ServerName);
            writer.WriteAttribute("Channel", m_Channel);
            writer.WriteAttribute("SourceNumber", m_SourceNumber);
            writer.WriteAttribute("TargetNumber", m_TargetNumber);
            writer.WriteAttribute("StartTime", m_StartTime);
            writer.WriteAttribute("AnswerTime", m_AnswerTime);
            writer.WriteAttribute("EndTime", m_EndTime);
            writer.WriteAttribute("StatoChiamata", (int?)m_StatoChiamata);
            writer.WriteAttribute("Direzione", m_Direzione);
            
            writer.WriteAttribute("IDTelefonata", IDTelefonata);
            base.XMLSerialize(writer);
            
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
                case "ServerIP":
                    {
                        m_ServerIP = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "ServerName":
                    {
                        m_ServerName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Channel":
                    {
                        m_Channel = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "SourceNumber":
                    {
                        m_SourceNumber = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "TargetNumber":
                    {
                        m_TargetNumber = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "StartTime":
                    {
                        m_StartTime = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                        break;
                    }

                case "AnswerTime":
                    {
                        m_AnswerTime = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                        break;
                    }

                case "EndTime":
                    {
                        m_EndTime = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                        break;
                    }

                case "StatoChiamata":
                    {
                        m_StatoChiamata = (StatoChiamata)DMD.Integers.ValueOf(DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue));
                        break;
                    }

                case "Direzione":
                    {
                        m_Direzione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

               
                case "IDTelefonata":
                    {
                        m_IDTelefonata = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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