using System;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Consensi informati
        /// </summary>
        [Serializable]
        public class ConsensoInformato 
            : Databases.DBObjectBase
        {
            private int m_IDPersona;
            [NonSerialized] private CPersona m_Persona;
            private string m_NomePersona;
            private DateTime? m_DataConsenso;
            private bool m_Consenso;
            private bool m_Richiesto;
            private string m_NomeDocumento;
            private string m_DescrizioneDocumento;
            private string m_LinkDocumentoVisionato;
            private string m_LinkDocumentoFirmato;

            /// <summary>
            /// Costruttore
            /// </summary>
            public ConsensoInformato()
            {
                this.m_IDPersona = 0;
                this.m_Persona = null;
                this.m_NomePersona = "";
                this.m_DataConsenso = default;
                this.m_Consenso = false;
                this.m_Richiesto = false;
                this.m_NomeDocumento = "";
                this.m_DescrizioneDocumento = "";
                this.m_LinkDocumentoVisionato = "";
                this.m_LinkDocumentoFirmato = "";
                this.m_Flags = 0;
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona che ha dato o rifiutato il consenso
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDPersona
            {
                get
                {
                    return DBUtils.GetID(m_Persona, m_IDPersona);
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
            /// Restituisce o imposta la persona che ha dato o rifiutato il consenso
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CPersona Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = minidom.Anagrafica.Persone.GetItemById(m_IDPersona);
                    return m_Persona;
                }

                set
                {
                    var oldValue = m_Persona;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Persona = value;
                    m_IDPersona = DBUtils.GetID(value, this.m_IDPersona);
                    if (value is object)
                        m_NomePersona = value.Nominativo;
                    DoChanged("Persona", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta 
            /// </summary>
            /// <param name="value"></param>
            internal void SetPersona(CPersona value)
            {
                m_Persona = value;
                m_IDPersona = DBUtils.GetID(value, this.m_IDPersona);
            }

            /// <summary>
            /// Restituisce o imposta il nome della persona che ha dato o rifiutato il consenso
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
                    string oldValue = m_NomePersona;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePersona = value;
                    DoChanged("NomePersona", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o imposta un valore booleano vero se la persona ha dato il consenso
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Consenso
            {
                get
                {
                    return m_Consenso;
                }

                set
                {
                    if (m_Consenso == value)
                        return;
                    m_Consenso = value;
                    DoChanged("Consenso", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica che il consenso è necessario
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Richiesto
            {
                get
                {
                    return m_Richiesto;
                }

                set
                {
                    if (m_Richiesto == value)
                        return;
                    m_Richiesto = value;
                    DoChanged("Richiesto", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data del consenso o della negazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataConsenso
            {
                get
                {
                    return m_DataConsenso;
                }

                set
                {
                    var oldValue = m_DataConsenso;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataConsenso = value;
                    DoChanged("DataConsenso", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del doucmento che descrive l'informativa
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeDocumento
            {
                get
                {
                    return m_NomeDocumento;
                }

                set
                {
                    string oldValue = m_NomeDocumento;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeDocumento = value;
                    DoChanged("NomeDocumento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la descrizione del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string DescrizioneDocumento
            {
                get
                {
                    return m_DescrizioneDocumento;
                }

                set
                {
                    string oldValue = m_DescrizioneDocumento;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_DescrizioneDocumento = value;
                    DoChanged("DescrizioneDocumento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il link del documento che ha visualizzato l'utente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string LinkDocumentoVisionato
            {
                get
                {
                    return m_LinkDocumentoVisionato;
                }

                set
                {
                    string oldValue = m_LinkDocumentoVisionato;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_LinkDocumentoVisionato = value;
                    DoChanged("LinkDocumentoVisionato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il link al documento firmato inviato dal cliente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string LinkDocumentoFirmato
            {
                get
                {
                    return m_LinkDocumentoFirmato;
                }

                set
                {
                    string oldValue = m_LinkDocumentoFirmato;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_LinkDocumentoFirmato = value;
                    DoChanged("LinkDocumentoFirmato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.NomePersona, " - ", this.NomeDocumento, " - ", this.DataConsenso);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.NomePersona, this.NomeDocumento, this.DataConsenso);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is ConsensoInformato) && this.Equals((ConsensoInformato)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(ConsensoInformato obj)
            {
                return base.Equals(obj)
                     && DMD.Integers.EQ(this.IDPersona, obj.IDPersona)
                     && DMD.Strings.EQ(this.m_NomePersona, obj.m_NomePersona)
                     && DMD.DateUtils.EQ(this.m_DataConsenso, obj.m_DataConsenso)
                    && DMD.Booleans.EQ(this.m_Consenso, obj.m_Consenso)
                    && DMD.Booleans.EQ(this.m_Richiesto, obj.m_Richiesto)
                    && DMD.Strings.EQ(this.m_NomeDocumento, obj.m_NomeDocumento)
                    && DMD.Strings.EQ(this.m_DescrizioneDocumento, obj.m_DescrizioneDocumento)
                    && DMD.Strings.EQ(this.m_LinkDocumentoVisionato, obj.m_LinkDocumentoVisionato)
                    && DMD.Strings.EQ(this.m_LinkDocumentoFirmato, obj.m_LinkDocumentoFirmato)
                    ;            
                    //private CKeyCollection m_Parameters;
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.ConsensiInformati; //.Module;
            }

            /// <summary>
            /// Discriminante
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_PersoneConsensi";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDPersona = reader.Read("IDPersona",  m_IDPersona);
                m_NomePersona = reader.Read("NomePersona",  m_NomePersona);
                m_DataConsenso = reader.Read("DataConsenso",  m_DataConsenso);
                m_Consenso = reader.Read("Consenso",  m_Consenso);
                m_Richiesto = reader.Read("Richiesto",  m_Richiesto);
                m_NomeDocumento = reader.Read("NomeDocumento",  m_NomeDocumento);
                m_DescrizioneDocumento = reader.Read("DescrizioneDocumento",  m_DescrizioneDocumento);
                m_LinkDocumentoVisionato = reader.Read("LinkVisionato",  m_LinkDocumentoVisionato);
                m_LinkDocumentoFirmato = reader.Read("LinkFirmato",  m_LinkDocumentoFirmato);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel DB
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDPersona", IDPersona);
                writer.Write("NomePersona", m_NomePersona);
                writer.Write("DataConsenso", m_DataConsenso);
                writer.Write("Consenso", m_Consenso);
                writer.Write("Richiesto", m_Richiesto);
                writer.Write("NomeDocumento", m_NomeDocumento);
                writer.Write("DescrizioneDocumento", m_DescrizioneDocumento);
                writer.Write("LinkVisionato", m_LinkDocumentoVisionato);
                writer.Write("LinkFirmato", m_LinkDocumentoFirmato);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDPersona", typeof(int), 1);
                c = table.Fields.Ensure("NomePersona", typeof(string), 255);
                c = table.Fields.Ensure("DataConsenso", typeof(DateTime), 1);
                c = table.Fields.Ensure("Consenso", typeof(bool), 1);
                c = table.Fields.Ensure("Richiesto", typeof(bool), 1);
                c = table.Fields.Ensure("NomeDocumento", typeof(string), 255);
                c = table.Fields.Ensure("DescrizioneDocumento", typeof(string), 0);
                c = table.Fields.Ensure("LinkVisionato", typeof(string), 255);
                c = table.Fields.Ensure("LinkFirmato", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxPersona", new string[] { "IDPersona", "NomePersona" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataConsenso", "Consenso", "Richiesto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDocumento", new string[] { "NomeDocumento", "LinkVisionato", "LinkFirmato" }, DBFieldConstraintFlags.None);

            }

            /// <summary>
            /// Serializzazione XML
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteAttribute("NomePersona", m_NomePersona);
                writer.WriteAttribute("DataConsenso", m_DataConsenso);
                writer.WriteAttribute("Consenso", m_Consenso);
                writer.WriteAttribute("Richiesto", m_Richiesto);
                writer.WriteAttribute("NomeDocumento", m_NomeDocumento);
                writer.WriteAttribute("LinkVisionato", m_LinkDocumentoVisionato);
                writer.WriteAttribute("LinkFirmato", m_LinkDocumentoFirmato);
                base.XMLSerialize(writer);
                writer.WriteTag("DescrizioneDocumento", m_DescrizioneDocumento);
            }

            /// <summary>
            /// Deserializzazione XML
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
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

                    case "DataConsenso":
                        {
                            m_DataConsenso = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Consenso":
                        {
                            m_Consenso = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Richiesto":
                        {
                            m_Richiesto = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "NomeDocumento":
                        {
                            m_NomeDocumento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "LinkVisionato":
                        {
                            m_LinkDocumentoVisionato = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "LinkFirmato":
                        {
                            m_LinkDocumentoFirmato = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DescrizioneDocumento":
                        {
                            m_DescrizioneDocumento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
}