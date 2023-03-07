using System;
using DMD;
using DMD.Net.Mail;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Sistema
    {
 

       

        /// <summary>
        /// Confgurazione del sistema di invio/ricezione email
        /// </summary>
        [Serializable]
        public class CEmailConfig 
            : DMD.XML.DMDBaseXMLObject
        {
            private string m_Name;
            private CPOP3EmailConfig pop3Config;
            private CSMTPEmailConfig smtpConfig;
 

            /// <summary>
            /// Costruttore
            /// </summary>
            public CEmailConfig()
            {
                this.m_Name = string.Empty;
                this.pop3Config = new CPOP3EmailConfig();
                this.smtpConfig = new CSMTPEmailConfig();
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_Name;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Name);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(object obj)
            {
                return (obj is CEmailConfig) && this.Equals((CEmailConfig)obj);
            }


            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CEmailConfig obj)
            {
                return base.Equals(obj)
                       && DMD.Strings.EQ(this.m_Name, obj.m_Name)
                       && this.SMTPConfig.Equals(obj.SMTPConfig)
                       && this.POP3Config.Equals(obj.POP3Config);
            }

            /// <summary>
            /// Restituisce o imposta il nome della configurazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Name
            {
                get
                {
                    return m_Name;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Name;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Name = value;
                    DoChanged("Name", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la configurazione POP3
            /// </summary>
            public CPOP3EmailConfig POP3Config
            {
                get
                {
                    if (this.pop3Config is null) this.pop3Config = new CPOP3EmailConfig();
                    return this.pop3Config;
                }
                set
                {
                    var oldValue = this.pop3Config;
                    if (value == oldValue) return;
                    this.pop3Config = value;
                    this.DoChanged("POP3Config", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la configurazione SMTP
            /// </summary>
            public CSMTPEmailConfig SMTPConfig
            {
                get
                {
                    if (this.smtpConfig is null) this.smtpConfig = new CSMTPEmailConfig();
                    return this.smtpConfig;
                }
                set
                {
                    var oldValue = this.smtpConfig;
                    if (value == smtpConfig) return;
                    this.smtpConfig = value;
                    this.DoChanged("SMTPConfig", value, oldValue);
                }
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
                    case "Name": this.m_Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "POP3Config": this.pop3Config = (CPOP3EmailConfig)fieldValue; break;
                    case "SMTPConfig": this.smtpConfig = (CSMTPEmailConfig)fieldValue; break;
                    default: base.SetFieldInternal(fieldName, fieldValue); break;
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Name", this.m_Name);
                base.XMLSerialize(writer);
                writer.WriteTag("POP3Config", this.POP3Config);
                writer.WriteTag("SMTPConfig", this.SMTPConfig);
            }

             

        }
    }
}