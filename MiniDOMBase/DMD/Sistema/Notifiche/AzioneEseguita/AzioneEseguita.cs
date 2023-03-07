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

namespace minidom
{
    public partial class Sistema
    {



        /// <summary>
        /// Rappresenta il risultato di un'azione eseguita su una notifica
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class AzioneEseguita
            : Databases.DBObject
        {
            [NonSerialized] private Notifica m_Notifica;          // Notifica su cui è stata eseguita l'azione
            private int m_IDNotifica;         // ID della notifica su cui è stata eseguita l'azione
            [NonSerialized] private AzioneEseguibile m_Azione;      // Risultato dell'operazione
            private string m_AzioneType;          // Tipo dell'azione
            private DateTime m_DataEsecuzione;        // Data di esecuzione
            private string m_ActionParameters;          // Parametri di esecuzione (in form
            private string m_ActionResults;

            /// <summary>
            /// Costruttore
            /// </summary>
            public AzioneEseguita()
            {
                m_Notifica = null;
                m_IDNotifica = 0;
                m_Azione = null;
                m_AzioneType = DMD.Strings.vbNullString;
                m_DataEsecuzione = default;
                m_ActionParameters = DMD.Strings.vbNullString;
                m_ActionResults = DMD.Strings.vbNullString;
            }

            /// <summary>
            /// Restituisce l'ID della notifica associata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDNotifica
            {
                get
                {
                    return DBUtils.GetID(m_Notifica, m_IDNotifica);
                }

                internal set
                {
                    int oldValue = IDNotifica;
                    if (oldValue == value)
                        return;
                    m_IDNotifica = value;
                    m_Notifica = null;
                    DoChanged("IDNotifica", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la notifica associata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Notifica Notifica
            {
                get
                {
                    if (m_Notifica is null)
                        m_Notifica = Notifiche.GetItemById(m_IDNotifica);
                    return m_Notifica;
                }

                internal set
                {
                    var oldValue = m_Notifica;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Notifica = value;
                    m_IDNotifica = DBUtils.GetID(value, 0);
                    DoChanged("Notifica", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'azione eseguita
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public AzioneEseguibile Azione
            {
                get
                {
                    if (m_Azione is null && !string.IsNullOrEmpty(m_AzioneType))
                        m_Azione = (AzioneEseguibile)minidom.Sistema.ApplicationContext.CreateInstance(this.m_AzioneType);
                    return m_Azione;
                }

                internal set
                {
                    var oldValue = Azione;
                    if ((value is object) &&  (oldValue is object) && (oldValue.GetType() == value.GetType()))
                        return;
                    m_Azione = value;
                    m_AzioneType = DMD.RunTime.vbTypeName(value);
                    DoChanged("Azione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di esecuzione dell'handler
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime DataEsecuzione
            {
                get
                {
                    return m_DataEsecuzione;
                }

                internal set
                {
                    var oldValue = m_DataEsecuzione;
                    if (oldValue == value)
                        return;
                    m_DataEsecuzione = value;
                    DoChanged("DataEsecuzione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa descrittiva dei parametri di esecuzione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ActionParameters
            {
                get
                {
                    return m_ActionParameters;
                }

                internal set
                {
                    string oldValue = m_ActionParameters;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ActionParameters = value;
                    DoChanged("ActionParameters", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa descrittiva dei risultati
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ActionResults
            {
                get
                {
                    return m_ActionResults;
                }

                internal set
                {
                    // value = Trim(value)
                    string oldValue = m_ActionResults;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ActionResults = value;
                    DoChanged("ActionResults", value, oldValue);
                }
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDNotifica = reader.Read("Notifica", this.m_IDNotifica);
                m_AzioneType = reader.Read("Azione", this.m_AzioneType);
                m_DataEsecuzione = reader.Read("DataEsecuzione", this.m_DataEsecuzione);
                m_ActionParameters = reader.Read("ActionParameters", this.m_ActionParameters);
                m_ActionResults = reader.Read("ActionResults", this.m_ActionResults);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Notifica", IDNotifica);
                writer.Write("Azione", m_AzioneType);
                writer.Write("DataEsecuzione", m_DataEsecuzione);
                writer.Write("ActionParameters", m_ActionParameters);
                writer.Write("ActionResults", m_ActionResults);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Notifica", typeof(int), 1);
                c = table.Fields.Ensure("Azione", typeof(int), 1);
                c = table.Fields.Ensure("DataEsecuzione", typeof(DateTime), 1);
                c = table.Fields.Ensure("ActionParameters", typeof(string), 0);
                c = table.Fields.Ensure("ActionResults", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNotifica", new string[] { "Notifica", "Azione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataEsecuzione" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("ActionParameters", typeof(string), 0);
                //c = table.Fields.Ensure("ActionResults", typeof(string), 0);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Notifica", IDNotifica);
                writer.WriteAttribute("Azione", m_AzioneType);
                writer.WriteAttribute("DataEsecuzione", m_DataEsecuzione);
                base.XMLSerialize(writer);
                writer.WriteTag("ActionParameters", m_ActionParameters);
                writer.WriteTag("ActionResults", m_ActionResults);
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
                    case "Notifica":
                        {
                            m_IDNotifica = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Azione":
                        {
                            m_AzioneType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataEsecuzione":
                        {
                            m_DataEsecuzione = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ActionParameters":
                        {
                            m_ActionParameters = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ActionResults":
                        {
                            m_ActionResults = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
                return DMD.Strings.ConcatArray(this.m_AzioneType, " ", this.IDNotifica);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_AzioneType, this.m_IDNotifica);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is AzioneEseguita) && this.Equals((AzioneEseguita)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public bool Equals(AzioneEseguita obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDNotifica, obj.m_IDNotifica)
                    && DMD.Strings.EQ(this.m_AzioneType, obj.m_AzioneType)
                    && DMD.DateUtils.EQ(this.m_DataEsecuzione, obj.m_DataEsecuzione)
                    && DMD.Strings.EQ(this.m_ActionParameters, obj.m_ActionParameters)
                    && DMD.Strings.EQ(this.m_ActionResults, obj.m_ActionResults)
                    ;
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Notifiche.Database;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Notifiche.AzioniEseguiteRepository;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_SYSNotifyRes";
            }
        }
    }
}