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
        // ------------------------------
        /// <summary>
        /// Rappresenta un documento caricato per una persona
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CDocCarPerPersona
            : minidom.Databases.DBObject
        {
            private int m_IDDocumento;
            [NonSerialized] private DocumentoCaricato m_Documento;
            private int m_IDPersona;
            [NonSerialized] private Anagrafica.CPersona m_Persona;
            private int m_IDAttachment;
            [NonSerialized] private Sistema.CAttachment m_Attachment;
            private DateTime m_DataCaricamento;
            private int m_IDOperatoreCaricamento;
            [NonSerialized] private Sistema.CUser m_OperatoreCaricamento;
            private string m_NomeOperatoreCaricamento;
            private string m_Annotazioni;
            private DateTime? m_DataRilascio;
            private DateTime? m_DataScadenza;
            private int m_IDRilasciatoDa;
            [NonSerialized] private Anagrafica.CAzienda m_RilasciatoDa;
            private string m_NomeRilasciatoDa;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CDocCarPerPersona()
            {
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.DocumentiCaricati.DocumentiPerPersona;
            }

            /// <summary>
            /// Restituisce
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDDocumento
            {
                get
                {
                    return DBUtils.GetID(m_Documento, m_IDDocumento);
                }

                set
                {
                    int oldValue = IDDocumento;
                    if (oldValue == value)
                        return;
                    m_IDDocumento = value;
                    m_Documento = null;
                    DoChanged("IDDocumento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la descrizione del documento caricato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DocumentoCaricato Documento
            {
                get
                {
                    if (m_Documento is null)
                        m_Documento = GDE.GetItemById(m_IDDocumento);
                    return m_Documento;
                }

                set
                {
                    var oldValue = Documento;
                    if (oldValue == value)
                        return;
                    m_Documento = value;
                    m_IDDocumento = DBUtils.GetID(value, 0);
                    DoChanged("Documento", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il rifeirmento al documento
            /// </summary>
            /// <param name="value"></param>
            protected virtual internal void SetDocumento(DocumentoCaricato value)
            {
                this.m_Documento = value;
                this.m_IDDocumento = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona a cui è associato il documento
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
            /// Restituisce o imposta la persona a cui è associato il documento
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
                    var oldValue = Persona;
                    if (oldValue == value)
                        return;
                    m_Persona = value;
                    m_IDPersona = DBUtils.GetID(value, 0);
                    DoChanged("Persona", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il riferimento alla persona
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetPersona(CPersona value)
            {
                this.m_Persona = value;
                this.m_IDPersona = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce o imposta la data di caricamento del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime DataCaricamento
            {
                get
                {
                    return m_DataCaricamento;
                }

                set
                {
                    var oldValue = m_DataCaricamento;
                    if (oldValue == value)
                        return;
                    m_DataCaricamento = value;
                    DoChanged("DataCaricamento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'operatore che ha caricato il documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDOperatoreCaricamento
            {
                get
                {
                    return DBUtils.GetID(m_OperatoreCaricamento, m_IDOperatoreCaricamento);
                }

                set
                {
                    int oldValue = IDOperatoreCaricamento;
                    if (oldValue == value)
                        return;
                    m_IDOperatoreCaricamento = value;
                    m_OperatoreCaricamento = null;
                    DoChanged("IDOperatoreCaricamento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'operatore che ha caricato il documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CUser OperatoreCaricamento
            {
                get
                {
                    if (m_OperatoreCaricamento is null)
                        m_OperatoreCaricamento = Sistema.Users.GetItemById(m_IDOperatoreCaricamento);
                    return m_OperatoreCaricamento;
                }

                set
                {
                    var oldValue = OperatoreCaricamento;
                    if (oldValue == value)
                        return;
                    m_OperatoreCaricamento = value;
                    m_IDOperatoreCaricamento = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeOperatoreCaricamento = value.Nominativo;
                    DoChanged("OperatoreCaricamento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'operatore che ha caricato il documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeOperatoreCaricamento
            {
                get
                {
                    return m_NomeOperatoreCaricamento;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeOperatoreCaricamento;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatoreCaricamento = value;
                    DoChanged("NomeOperatoreCaricamento", value, oldValue);
                }
            }

            /// <summary>
            /// Annotazioni
            /// </summary>
            public string Annotazioni
            {
                get
                {
                    return m_Annotazioni;
                }

                set
                {
                    string oldValue = m_Annotazioni;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Annotazioni = value;
                    DoChanged("Annotazioni", value, oldValue);
                }
            }

            /// <summary>
            /// DataRilascio
            /// </summary>
            public DateTime? DataRilascio
            {
                get
                {
                    return m_DataRilascio;
                }

                set
                {
                    var oldValue = m_DataRilascio;
                    if (oldValue == value == true)
                        return;
                    m_DataRilascio = value;
                    DoChanged("DataRilascio", value, oldValue);
                }
            }

            /// <summary>
            /// DataScadenza
            /// </summary>
            public DateTime? DataScadenza
            {
                get
                {
                    return m_DataScadenza;
                }

                set
                {
                    var oldValue = m_DataScadenza;
                    if (oldValue == value == true)
                        return;
                    m_DataScadenza = value;
                    DoChanged("DataScadenza", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id dell'ente che ha rilasciato il documento
            /// </summary>
            public int IDRilasciatoDa
            {
                get
                {
                    return DBUtils.GetID(m_RilasciatoDa, m_IDRilasciatoDa);
                }

                set
                {
                    int oldValue = IDRilasciatoDa;
                    if (oldValue == value)
                        return;
                    m_IDRilasciatoDa = value;
                    m_RilasciatoDa = null;
                    DoChanged("IDRilasciatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Ente che ha rilasciato il documento
            /// </summary>
            public Anagrafica.CAzienda RilasciatoDa
            {
                get
                {
                    if (m_RilasciatoDa is null)
                        m_RilasciatoDa = Anagrafica.Aziende.GetItemById(m_IDRilasciatoDa);
                    return m_RilasciatoDa;
                }

                set
                {
                    var oldValue = RilasciatoDa;
                    if (oldValue == value)
                        return;
                    m_RilasciatoDa = value;
                    m_IDRilasciatoDa = DBUtils.GetID(value, 0);
                    m_NomeRilasciatoDa = (value is object)? value.Nominativo : "";
                    DoChanged("RilasciatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Nome dell'ente che ha rilasciato il documento
            /// </summary>
            public string NomeRilasciatoDa
            {
                get
                {
                    return m_NomeRilasciatoDa;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeRilasciatoDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeRilasciatoDa = value;
                    DoChanged("NomeRilasciatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del file caricato sul server
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAttachment
            {
                get
                {
                    return DBUtils.GetID(m_Attachment, m_IDAttachment);
                }

                set
                {
                    int oldValue = IDAttachment;
                    if (oldValue == value)
                        return;
                    m_IDAttachment = value;
                    m_Attachment = null;
                    DoChanged("IDAttachment", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'oggetto caricato sul server
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CAttachment Attachment
            {
                get
                {
                    return m_Attachment;
                }

                set
                {
                    var oldValue = Attachment;
                    if (oldValue == value)
                        return;
                    m_Attachment = value;
                    m_IDAttachment = DBUtils.GetID(value, 0);
                    DoChanged("Attachment", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                var ret = new System.Text.StringBuilder(1024);
                if (Documento is object)
                {
                    ret.Append(Documento.Nome);
                }
                else
                {
                    ret.Append("ID Documento: " + IDDocumento);
                }

                if (Persona is object)
                {
                    ret.Append(", " + Persona.Nominativo);
                }
                else
                {
                    ret.Append(", ID persona: " + IDPersona);
                }

                ret.Append(", Rilasciato da: " + NomeRilasciatoDa);
                if (!DMD.RunTime.IsNull(DataRilascio))
                    ret.Append(" il: " + Sistema.Formats.FormatUserDate(DataRilascio));
                return ret.ToString();
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_IDPersona);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CDocCarPerPersona) && this.Equals((CDocCarPerPersona)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CDocCarPerPersona obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDDocumento, obj.m_IDDocumento)
                    && DMD.Integers.EQ(this.m_IDPersona, obj.m_IDPersona)
                    && DMD.Integers.EQ(this.m_IDAttachment, obj.m_IDAttachment)
                    && DMD.DateUtils.EQ(this.m_DataCaricamento, obj.m_DataCaricamento)
                    && DMD.Integers.EQ(this.m_IDOperatoreCaricamento, obj.m_IDOperatoreCaricamento)
                    && DMD.Strings.EQ(this.m_NomeOperatoreCaricamento, obj.m_NomeOperatoreCaricamento)
                    && DMD.Strings.EQ(this.m_Annotazioni, obj.m_Annotazioni)
                    && DMD.DateUtils.EQ(this.m_DataRilascio, obj.m_DataRilascio)
                    && DMD.DateUtils.EQ(this.m_DataScadenza, obj.m_DataScadenza)
                    && DMD.Integers.EQ(this.m_IDRilasciatoDa, obj.m_IDRilasciatoDa)
                    && DMD.Strings.EQ(this.m_NomeRilasciatoDa, obj.m_NomeRilasciatoDa)
                    ;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_DocCarPerPersona";
            }
             
            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_IDDocumento = reader.Read("IDDocumento", this.m_IDDocumento);
                this.m_IDPersona = reader.Read("IDPersona", this.m_IDPersona);
                this.m_IDAttachment = reader.Read("IDAttachment", this.m_IDAttachment);
                this.m_DataCaricamento = reader.Read("DataCaricamento", this.m_DataCaricamento);
                this.m_IDOperatoreCaricamento = reader.Read("IDOperatoreCaricamento", this.m_IDOperatoreCaricamento);
                this.m_NomeOperatoreCaricamento = reader.Read("NomeOperatoreCaricamento", this.m_NomeOperatoreCaricamento);
                this.m_Annotazioni = reader.Read("Annotazioni", this.m_Annotazioni);
                this.m_DataRilascio = reader.Read("DataRilascio", this.m_DataRilascio);
                this.m_DataScadenza = reader.Read("DataScadenza", this.m_DataScadenza);
                this.m_IDRilasciatoDa = reader.Read("IDRilasciatoDa", this.m_IDRilasciatoDa);
                this.m_NomeRilasciatoDa = reader.Read("NomeRilasciatoDa", this.m_NomeRilasciatoDa);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDDocumento", IDDocumento);
                writer.Write("IDPersona", IDPersona);
                writer.Write("IDAttachment", IDAttachment);
                writer.Write("DataCaricamento", m_DataCaricamento);
                writer.Write("IDOperatoreCaricamento", IDOperatoreCaricamento);
                writer.Write("NomeOperatoreCaricamento", m_NomeOperatoreCaricamento);
                writer.Write("Annotazioni", m_Annotazioni);
                writer.Write("DataRilascio", m_DataRilascio);
                writer.Write("DataScadenza", m_DataScadenza);
                writer.Write("IDRilasciatoDa", IDRilasciatoDa);
                writer.Write("NomeRilasciatoDa", m_NomeRilasciatoDa);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDDocumento", typeof(int), 1);
                c = table.Fields.Ensure("IDPersona", typeof(int), 1);
                c = table.Fields.Ensure("IDAttachment", typeof(int), 1);
                c = table.Fields.Ensure("DataCaricamento", typeof(DateUtils), 1);
                c = table.Fields.Ensure("IDOperatoreCaricamento", typeof(int), 1);
                c = table.Fields.Ensure("NomeOperatoreCaricamento", typeof(string), 255);
                c = table.Fields.Ensure("Annotazioni", typeof(string), 0);
                c = table.Fields.Ensure("DataRilascio", typeof(DateUtils), 1);
                c = table.Fields.Ensure("DataScadenza", typeof(DateUtils), 1);
                c = table.Fields.Ensure("IDRilasciatoDa", typeof(int), 1);
                c = table.Fields.Ensure("NomeRilasciatoDa", typeof(string), 255);
                 

            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxDocumento", new string[] { "IDDocumento" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPersona", new string[] { "IDPersona" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAttachments", new string[] { "IDAttachment", "DataScadenza" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxOperatore", new string[] { "IDOperatoreCaricamento", "NomeOperatoreCaricamento" , "DataCaricamento" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRilasciatoDa", new string[] { "IDRilasciatoDa", "NomeRilasciatoDa", "DataRilascio" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "", "NomeRilasciatoDa" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("Annotazioni", typeof(string), 0);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDDocumento", IDDocumento);
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteAttribute("IDAttachment", IDAttachment);
                writer.WriteAttribute("DataCaricamento", m_DataCaricamento);
                writer.WriteAttribute("IDOperatoreCaricamento", IDOperatoreCaricamento);
                writer.WriteAttribute("NomeOperatoreCaricamento", m_NomeOperatoreCaricamento);
                writer.WriteAttribute("DataRilascio", m_DataRilascio);
                writer.WriteAttribute("DataScadenza", m_DataScadenza);
                writer.WriteAttribute("IDRilasciatoDa", IDRilasciatoDa);
                writer.WriteAttribute("NomeRilasciatoDa", m_NomeRilasciatoDa);
                base.XMLSerialize(writer);
                writer.WriteTag("Annotazioni", m_Annotazioni);
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
                    case "IDDocumento":
                        {
                            m_IDDocumento = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDPersona":
                        {
                            m_IDPersona = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDAttachment":
                        {
                            m_IDAttachment = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataCaricamento":
                        {
                            m_DataCaricamento = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDOperatoreCaricamento":
                        {
                            m_IDOperatoreCaricamento = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatoreCaricamento":
                        {
                            m_NomeOperatoreCaricamento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Annotazioni":
                        {
                            m_Annotazioni = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataRilascio":
                        {
                            m_DataRilascio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataScadenza":
                        {
                            m_DataScadenza = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDRilasciatoDa":
                        {
                            m_IDRilasciatoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeRilasciatoDa":
                        {
                            m_NomeRilasciatoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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