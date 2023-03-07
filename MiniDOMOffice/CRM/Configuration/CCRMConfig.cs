using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class CustomerCalls
    {
        /// <summary>
        /// Flag della configurazione del CRM
        /// </summary>
        [Flags]
        public enum CRMFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Segnala le chiamate senza risposta ai supervisori
            /// </summary>
            SEGNALA_NONRISPONDE = 1,

            /// <summary>
            /// Segnala le chiamate fatte a persone la cui data di ricontatto non é prossima
            /// </summary>
            SEGNALA_RICONTATTITROPPOLONTANI = 2,

            /// <summary>
            /// Inva ai supervisori le statistiche giornaliere sugli operatori
            /// </summary>
            INVIA_STATISTICHE_GIORNALIERE = 4
        }

        /// <summary>
        /// Classe che rappresenta la configurazione del CRM
        /// </summary>
        /// <remarks></remarks>
        public class CCRMConfig 
            : DMD.XML.DMDBaseXMLObject 
        {
            private string m_FromAddress;
            private string m_FromDisplayName;
            private CRMFlags m_Flags;
            private string m_TargetAddress;
            private int m_MinutiRicontatto;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCRMConfig()
            {
                m_FromAddress = "";
                m_FromDisplayName = "";
                m_Flags = CRMFlags.None;
                m_TargetAddress = "";
                m_MinutiRicontatto = 15;
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il sistema deve segnalare le telefonate senza risposta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool SegnalaNonRisponde
            {
                get
                {
                    return DMD.RunTime.TestFlag(m_Flags, CRMFlags.SEGNALA_NONRISPONDE);
                }

                set
                {
                    if (SegnalaNonRisponde == value)
                        return;
                    m_Flags = DMD.RunTime.SetFlag(m_Flags, CRMFlags.SEGNALA_NONRISPONDE, value);
                    DoChanged("SegnalaNonRisponde", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il sistema deve segnalare la programmazioni du un ricontatto oltre il limite di MinutiRicontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool SegnalaRicontattiTroppoLontani
            {
                get
                {
                    return DMD.RunTime.TestFlag(m_Flags, CRMFlags.SEGNALA_RICONTATTITROPPOLONTANI);
                }

                set
                {
                    if (SegnalaRicontattiTroppoLontani == value)
                        return;
                    m_Flags = DMD.RunTime.SetFlag(m_Flags, CRMFlags.SEGNALA_RICONTATTITROPPOLONTANI, value);
                    DoChanged("SegnalaRicontattiTroppoLontani", value, !value);
                }
            }


            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il sistema deve segnalare la programmazioni du un ricontatto oltre il limite di MinutiRicontatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool InviaStatisticheGiornaliere
            {
                get
                {
                    return DMD.RunTime.TestFlag(m_Flags, CRMFlags.INVIA_STATISTICHE_GIORNALIERE);
                }

                set
                {
                    if (InviaStatisticheGiornaliere == value)
                        return;
                    m_Flags = DMD.RunTime.SetFlag(m_Flags, CRMFlags.INVIA_STATISTICHE_GIORNALIERE, value);
                    DoChanged("InviaStatisticheGiornaliere", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo e-mail utilizzato per inviare le statistiche e gli alert del CRM
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string FromAddress
            {
                get
                {
                    return m_FromAddress;
                }

                set
                {
                    string oldValue = m_FromAddress;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_FromAddress = value;
                    DoChanged("FromAddress", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome visualizzato come mittente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string FromDisplayName
            {
                get
                {
                    return m_FromDisplayName;
                }

                set
                {
                    string oldValue = m_FromDisplayName;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_FromDisplayName = value;
                    DoChanged("FromDisplayName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'indirizzo e-mail a cui inviare le segnalazioni e le statistiche
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TargetAddress
            {
                get
                {
                    return m_TargetAddress;
                }

                set
                {
                    string oldValue = m_TargetAddress;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TargetAddress = value;
                    DoChanged("TargetAddress ", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta flags aggiuntivi
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CRMFlags Flags
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
            /// Restituisce o imposta i minuti oltre i quali il nuovo ricontatto programmato deve essere segnalato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int MinutiRicontatto
            {
                get
                {
                    return m_MinutiRicontatto;
                }

                set
                {
                    int oldValue = m_MinutiRicontatto;
                    if (oldValue == value)
                        return;
                    m_MinutiRicontatto = value;
                    DoChanged("MinutiRicontatto", value, oldValue);
                }
            }

          

            //protected override bool LoadFromRecordset(DBReader reader)
            //{
            //    this.m_FromAddress = reader.Read("FromAddress", this.m_FromAddress);
            //    this.m_FromDisplayName = reader.Read("FromDisplayName", this.m_FromDisplayName);
            //    this.m_Flags = reader.Read("Flags", this.m_Flags);
            //    this.m_TargetAddress = reader.Read("TargetAddress", this.m_TargetAddress);
            //    this.m_MinutiRicontatto = reader.Read("MinutiRicontatto", this.m_MinutiRicontatto);
            //    return base.LoadFromRecordset(reader);
            //}

            //protected override bool SaveToRecordset(DBWriter writer)
            //{
            //    writer.Write("FromAddress", m_FromAddress);
            //    writer.Write("FromDisplayName", m_FromDisplayName);
            //    writer.Write("Flags", m_Flags);
            //    writer.Write("TargetAddress", m_TargetAddress);
            //    writer.Write("MinutiRicontatto", m_MinutiRicontatto);
            //    return base.SaveToRecordset(writer);
            //}

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("FromAddress", m_FromAddress);
                writer.WriteAttribute("FromDisplayName", m_FromDisplayName);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                writer.WriteAttribute("TargetAddress", m_TargetAddress);
                writer.WriteAttribute("MinutiRicontatto", m_MinutiRicontatto);
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
                    case "FromAddress":
                        {
                            m_FromAddress = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "FromDisplayName":
                        {
                            m_FromDisplayName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (CRMFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TargetAddress":
                        {
                            m_TargetAddress = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MinutiRicontatto":
                        {
                            m_MinutiRicontatto = DMD.Integers.ValueOf(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
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
            /// Carica la configurazione dalle impostazioni di sistema
            /// </summary>
            public void Load()
            {
                //string dbSQL = "SELECT * FROM [" + GetTableName() + "] ORDER BY [ID] ASC";
                //var dbRis = GetConnection().ExecuteReader(dbSQL);
                //if (dbRis.Read())
                //{
                //    GetConnection().Load(this, dbRis);
                //}

                //dbRis.Dispose();
                var str = minidom.Sistema.ApplicationContext.Settings.GetItemByKey("CRM.Configuration", "");
                var tmp = DMD.XML.Utils.Deserialize<CCRMConfig>(str);
                if (tmp is object)
                    DMD.RunTime.CopyFrom(this, tmp, false);
            }

            /// <summary>
            /// Salva la configurazione
            /// </summary>
            public void Save()
            {
                //base.Save();
                var str = DMD.XML.Utils.Serialize(this);
                minidom.Sistema.ApplicationContext.Settings.SetItemByKey("CRM.Configuration", str);
                minidom.CustomerCalls.CRM.SetConfig(this);
            }
        }
    }
}