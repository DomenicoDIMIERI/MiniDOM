using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class WebSite
    {


        /// <summary>
        /// Oggetto che rappresenta un indirizzo IP da cui è possibile ricevere trasferimenti di pratiche
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CAllowedRemoteIPs 
            : Databases.DBObject
        {
            private string m_Name;
            private string m_RemoteIP;
            private bool m_Negate;

            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CAllowedRemoteIPs()
            {
                m_Name = "";
                m_RemoteIP = "";
                m_Negate = false;
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'oggetto
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }

                set
                {
                    value = Strings.Left(Strings.Trim(value), 255);
                    string oldValue = m_Name;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Name = value;
                    DoChanged("Name", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo rappresentato dall'oggetto
            /// </summary>
            public string RemoteIP
            {
                get
                {
                    return m_RemoteIP;
                }

                set
                {
                    value = Strings.Left(Strings.Trim(value), 64);
                    string oldValue = m_RemoteIP;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_RemoteIP = value;
                    DoChanged("RemoteIP", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore che indica se l'indirizzo è disabilitato
            /// </summary>
            public bool Negate
            {
                get
                {
                    return m_Negate;
                }

                set
                {
                    if (m_Negate == value)
                        return;
                    m_Negate = value;
                    DoChanged("Negate", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della tabella
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Pratiche_Allow";
            }

            /// <summary>
            /// Restituisce un riferimento alla connessione
            /// </summary>
            /// <returns></returns>
            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            /// <summary>
            /// Carica l'oggetto
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(Databases.DBReader reader)
            {
                this.m_Name = reader.Read("Name", this.m_Name);
                this.m_RemoteIP = reader.Read("RemoteIP", this.m_RemoteIP);
                this.m_Negate = reader.Read("Negate", this.m_Negate);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(Databases.DBWriter writer)
            {
                writer.Write("Name", m_Name);
                writer.Write("RemoteIP", m_RemoteIP);
                writer.Write("Negate", m_Negate);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("m_Name", m_Name);
                writer.WriteAttribute("m_RemoteIP", m_RemoteIP);
                writer.WriteAttribute("m_Negate", m_Negate);
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "m_Name":
                        {
                            m_Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_RemoteIP":
                        {
                            m_RemoteIP = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_Negate":
                        {
                            m_Negate = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
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
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.JoinW( (this.m_Negate)? "Nega ": "Consenti ", this.m_Name , " (" , this.m_RemoteIP ,")");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return DMD.HashCalculator.Calculate(this.m_Negate, this.m_Name, this.m_RemoteIP);
            }

            /// <summary>
            /// Restituisce il modulo gestore delle risorse
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Module;
            }
        }
    }
}