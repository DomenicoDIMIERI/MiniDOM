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
using static minidom.Contabilita;

namespace minidom
{
    public partial class Contabilita
    {


        /// <summary>
        /// Rappresenta una movimentazione contabile
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class VoceDiPagamento 
            : Databases.DBObjectPO
        {
            private string m_Descrizione;
            private decimal? m_Quantita;
            private string m_NomeValuta;
            private DateTime? m_DataOperazione;
            private DateTime? m_DataEffettiva;
            private object m_Source;
            private string m_SourceType;
            private int m_SourceID;
            private string m_SourceParams;
            [NonSerialized] private Anagrafica.ContoCorrente m_CCOrigine;
            private int m_IDCCOrigine;
            private string m_NomeCCOrigine;
            [NonSerialized] private Anagrafica.ContoCorrente m_CCDestinazione;
            private int m_IDCCDestinazione;
            private string m_NomeCCDestinazione;
            private object m_MetodoDiPagamento;
            private string m_TipoMetodoDiPagamento;
            private int m_IDMetodoDiPagamento;
            private string m_NomeMetodoDiPagamento;

            /// <summary>
            /// Costruttore
            /// </summary>
            public VoceDiPagamento()
            {
                m_Descrizione = "";
                m_Quantita = default;
                m_NomeValuta = "";
                m_DataOperazione = default;
                m_DataEffettiva = default;
                m_Source = null;
                m_SourceType = "";
                m_SourceID = 0;
                m_SourceParams = "";
                m_CCOrigine = null;
                m_IDCCOrigine = 0;
                m_NomeCCOrigine = "";
                m_CCDestinazione = null;
                m_IDCCDestinazione = 0;
                m_NomeCCDestinazione = "";
                m_MetodoDiPagamento = null;
                m_TipoMetodoDiPagamento = "";
                m_IDMetodoDiPagamento = 0;
                m_NomeMetodoDiPagamento = "";
            }

            /// <summary>
            /// Restituisce o imposta una stringa che descrive la movimentazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la quantità di denaro movimentata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public decimal? Quantita
            {
                get
                {
                    return m_Quantita;
                }

                set
                {
                    var oldValue = m_Quantita;
                    if (oldValue == value == true)
                        return;
                    m_Quantita = value;
                    DoChanged("Quantita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della valuta utilizzata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeValuta
            {
                get
                {
                    return m_NomeValuta;
                }

                set
                {
                    string oldValue = m_NomeValuta;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeValuta = value;
                    DoChanged("NomeValuta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data dell'operazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataOperazione
            {
                get
                {
                    return m_DataOperazione;
                }

                set
                {
                    var oldValue = m_DataOperazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataOperazione = value;
                    DoChanged("DataOperazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data effettiva
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataEffettiva
            {
                get
                {
                    return m_DataEffettiva;
                }

                set
                {
                    var oldValue = m_DataEffettiva;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataEffettiva = value;
                    DoChanged("DataEffettiva", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il documento o l'oggetto che ha causato la movimentazione di denaro
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public object Source
            {
                get
                {
                    if (m_Source is null)
                        m_Source = minidom.Sistema.ApplicationContext.GetItemByTypeAndId(m_SourceType, m_SourceID);
                    return m_Source;
                }

                set
                {
                    var oldValue = Source;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Source = value;
                    m_SourceType = "";
                    if (value is object)
                        m_SourceType = DMD.RunTime.vbTypeName(value);
                    m_SourceID = DBUtils.GetID(value, 0);
                    DoChanged("Source", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la sorgente
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetSource(object value)
            {
                m_Source = value;
                m_SourceID = DBUtils.GetID(value, 0);
                m_SourceType = "";
                if (value is object)
                    m_SourceType = DMD.RunTime.vbTypeName(value);
            }

            /// <summary>
            /// Restituisec o imposta il tipo del documento o l'oggetto che ha causato la movimentazione di denaro
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SourceType
            {
                get
                {
                    return m_SourceType;
                }

                set
                {
                    string oldValue = m_SourceType;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SourceType = value;
                    m_Source = null;
                    DoChanged("SourceType", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del documento o dell'oggetto che ha causato la movimentazione di denaro
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int SourceID
            {
                get
                {
                    return DBUtils.GetID(m_Source, m_SourceID);
                }

                set
                {
                    int oldValue = SourceID;
                    if (oldValue == value)
                        return;
                    m_SourceID = value;
                    m_Source = null;
                    DoChanged("SourceID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta dei parametri aggiuntivi per il documento o l'oggetto che ha causato la movimentazione di denaro
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SourceParams
            {
                get
                {
                    return m_SourceParams;
                }

                set
                {
                    string oldValue = m_SourceParams;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SourceParams = value;
                    DoChanged("SourceParams", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta i parametri sorgente
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetSourceParams(string value)
            {
                m_SourceParams = value;
            }

             

            /// <summary>
            /// Restituisce o imposta il conto corrente di origine (da cui vengono prelevati i soldi)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.ContoCorrente CCOrigine
            {
                get
                {
                    if (m_CCOrigine is null)
                        m_CCOrigine = Anagrafica.ContiCorrente.GetItemById(m_IDCCOrigine);
                    return m_CCOrigine;
                }

                set
                {
                    var oldValue = CCOrigine;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_CCOrigine = value;
                    m_IDCCOrigine = DBUtils.GetID(value, 0);
                    m_NomeCCOrigine = "";
                    if (value is object)
                        m_NomeCCOrigine = value.Nome;
                    DoChanged("CCOrigine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del conto corrente da cui vengono prelevati i soldi
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDCCOrigine
            {
                get
                {
                    return DBUtils.GetID(m_CCOrigine, m_IDCCOrigine);
                }

                set
                {
                    int oldValue = IDCCOrigine;
                    if (oldValue == value)
                        return;
                    m_IDCCOrigine = value;
                    m_CCOrigine = null;
                    DoChanged("IDCCOrigine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del conto corrente di origine
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeCCOrigine
            {
                get
                {
                    return m_NomeCCOrigine;
                }

                set
                {
                    string oldValue = m_NomeCCOrigine;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCCOrigine = value;
                    DoChanged("NomeCCOrigine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il conto corrente destinazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.ContoCorrente CCDestinazione
            {
                get
                {
                    if (m_CCDestinazione is null)
                        m_CCDestinazione = Anagrafica.ContiCorrente.GetItemById(m_IDCCDestinazione);
                    return m_CCDestinazione;
                }

                set
                {
                    var oldValue = CCDestinazione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_CCDestinazione = value;
                    m_IDCCDestinazione = DBUtils.GetID(value, 0);
                    m_NomeCCDestinazione = "";
                    if (value is object)
                        m_NomeCCDestinazione = value.Nome;
                    DoChanged("CCDestinazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del conto corrente destinazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDCCDestinazione
            {
                get
                {
                    return DBUtils.GetID(m_CCDestinazione, m_IDCCDestinazione);
                }

                set
                {
                    int oldValue = IDCCDestinazione;
                    if (oldValue == value)
                        return;
                    m_IDCCDestinazione = value;
                    m_CCDestinazione = null;
                    DoChanged("IDCCDestinazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del conto corrente di destinazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeCCDestinazione
            {
                get
                {
                    return m_NomeCCDestinazione;
                }

                set
                {
                    string oldValue = m_NomeCCDestinazione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCCDestinazione = value;
                    DoChanged("NomeCCDestinazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il metodo di pagamento utilizzato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public object MetodoDiPagamento
            {
                get
                {
                    if (m_MetodoDiPagamento is null)
                        m_MetodoDiPagamento = minidom.Sistema.ApplicationContext.GetItemByTypeAndId(m_TipoMetodoDiPagamento, m_IDMetodoDiPagamento);
                    return m_MetodoDiPagamento;
                }

                set
                {
                    var oldValue = MetodoDiPagamento;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_MetodoDiPagamento = value;
                    m_IDMetodoDiPagamento = DBUtils.GetID(value, 0);
                    m_TipoMetodoDiPagamento = "";
                    m_NomeMetodoDiPagamento = "";
                    if (value is object)
                    {
                        m_TipoMetodoDiPagamento = DMD.RunTime.vbTypeName(value);
                        m_NomeMetodoDiPagamento = ((Anagrafica.IMetodoDiPagamento)value).NomeMetodo;
                    }

                    DoChanged("MetodoDiPagamento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo del metodo di pagamento utilizzato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TipoMetodoDiPagamento
            {
                get
                {
                    return m_TipoMetodoDiPagamento;
                }

                set
                {
                    string oldValue = m_TipoMetodoDiPagamento;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoMetodoDiPagamento = value;
                    m_MetodoDiPagamento = null;
                    DoChanged("TipoMetodoDiPagamento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del metodo di pagamento utilizzato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDMetodoDiPagamento
            {
                get
                {
                    return DBUtils.GetID((Databases.IDBObjectBase)m_MetodoDiPagamento, m_IDMetodoDiPagamento);
                }

                set
                {
                    int oldValue = IDMetodoDiPagamento;
                    if (oldValue == value)
                        return;
                    m_IDMetodoDiPagamento = value;
                    m_MetodoDiPagamento = null;
                    DoChanged("IDMetodoDiPagamento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del metodo di pagamento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeMetodoDiPagamento
            {
                get
                {
                    return m_NomeMetodoDiPagamento;
                }

                set
                {
                    string oldValue = m_NomeMetodoDiPagamento;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeMetodoDiPagamento = value;
                    DoChanged("NomeMetodoDiPagamento", value, oldValue);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Contabilita.VociDiPagamento;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeVociPagamento";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Descrizione = reader.Read("Descrizione", m_Descrizione);
                m_Quantita = reader.Read("Quantita", m_Quantita);
                m_NomeValuta = reader.Read("NomeValuta", m_NomeValuta);
                m_DataOperazione = reader.Read("DataOperazione", m_DataOperazione);
                m_DataEffettiva = reader.Read("DataEffettiva", m_DataEffettiva);
                m_SourceType = reader.Read("SourceType", m_SourceType);
                m_SourceID = reader.Read("SourceID", m_SourceID);
                m_SourceParams = reader.Read("SourceParams", m_SourceParams);
                m_IDCCOrigine = reader.Read("IDCCOrigine", m_IDCCOrigine);
                m_NomeCCOrigine = reader.Read("NomeCCOrigine", m_NomeCCOrigine);
                m_IDCCDestinazione = reader.Read("IDCCDestinazione", m_IDCCDestinazione);
                m_NomeCCDestinazione = reader.Read("NomeCCDestinazione", m_NomeCCDestinazione);
                m_TipoMetodoDiPagamento = reader.Read("TipoMetodoDiPagamento", m_TipoMetodoDiPagamento);
                m_IDMetodoDiPagamento = reader.Read("IDMetodotoDiPagamento", m_IDMetodoDiPagamento);
                m_NomeMetodoDiPagamento = reader.Read("NomeMetodoDiPagamento", m_NomeMetodoDiPagamento);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("Quantita", m_Quantita);
                writer.Write("NomeValuta", m_NomeValuta);
                writer.Write("DataOperazione", m_DataOperazione);
                writer.Write("DataEffettiva", m_DataEffettiva);
                writer.Write("SourceType", m_SourceType);
                writer.Write("SourceID", SourceID);
                writer.Write("SourceParams", m_SourceParams);
                writer.Write("IDCCOrigine", IDCCOrigine);
                writer.Write("NomeCCOrigine", m_NomeCCOrigine);
                writer.Write("IDCCDestinazione", IDCCDestinazione);
                writer.Write("NomeCCDestinazione", m_NomeCCDestinazione);
                writer.Write("TipoMetodoDiPagamento", m_TipoMetodoDiPagamento);
                writer.Write("IDMetodotoDiPagamento", IDMetodoDiPagamento);
                writer.Write("NomeMetodoDiPagamento", m_NomeMetodoDiPagamento);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Descrizione", typeof(string), 255);
                c = table.Fields.Ensure("Quantita", typeof(decimal), 1);
                c = table.Fields.Ensure("NomeValuta", typeof(string), 255);
                c = table.Fields.Ensure("DataOperazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataEffettiva", typeof(DateTime), 1);
                c = table.Fields.Ensure("SourceType", typeof(string), 255);
                c = table.Fields.Ensure("SourceID", typeof(int), 1);
                c = table.Fields.Ensure("SourceParams", typeof(string), 255);
                c = table.Fields.Ensure("IDCCOrigine", typeof(int), 1);
                c = table.Fields.Ensure("NomeCCOrigine", typeof(string), 255);
                c = table.Fields.Ensure("IDCCDestinazione", typeof(int), 1);
                c = table.Fields.Ensure("NomeCCDestinazione", typeof(string), 255);
                c = table.Fields.Ensure("TipoMetodoDiPagamento", typeof(string), 255);
                c = table.Fields.Ensure("IDMetodotoDiPagamento", typeof(int), 1);
                c = table.Fields.Ensure("NomeMetodoDiPagamento", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxDescrizione", new string[] { "Descrizione", "DataOperazione", "IDCCOrigine", "IDCCDestinazione", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxQuantita", new string[] { "Quantita", "DataEffettiva", "NomeValuta" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSource", new string[] { "SourceType", "SourceID", "SourceParams" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxContiCC", new string[] { "NomeCCOrigine", "NomeCCDestinazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxMetodoPag", new string[] { "IDMetodotoDiPagamento", "TipoMetodoDiPagamento", "NomeMetodoDiPagamento" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("Quantita", m_Quantita);
                writer.WriteAttribute("NomeValuta", m_NomeValuta);
                writer.WriteAttribute("DataOperazione", m_DataOperazione);
                writer.WriteAttribute("DataEffettiva", m_DataEffettiva);
                writer.WriteAttribute("SourceType", m_SourceType);
                writer.WriteAttribute("SourceID", SourceID);
                writer.WriteAttribute("SourceParams", m_SourceParams);
                writer.WriteAttribute("IDCCOrigine", IDCCOrigine);
                writer.WriteAttribute("NomeCCOrigine", m_NomeCCOrigine);
                writer.WriteAttribute("IDCCDestinazione", IDCCDestinazione);
                writer.WriteAttribute("NomeCCDestinazione", m_NomeCCDestinazione);
                writer.WriteAttribute("TipoMetodoDiPagamento", m_TipoMetodoDiPagamento);
                writer.WriteAttribute("IDMetodoDiPagamento", IDMetodoDiPagamento);
                writer.WriteAttribute("NomeMetodoDiPagamento", m_NomeMetodoDiPagamento);
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
                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Quantita":
                        {
                            m_Quantita = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NomeValuta":
                        {
                            m_NomeValuta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataOperazione":
                        {
                            m_DataOperazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataEffettiva":
                        {
                            m_DataEffettiva = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "SourceType":
                        {
                            m_SourceType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceID":
                        {
                            m_SourceID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "SourceParams":
                        {
                            m_SourceParams = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                  
                    case "IDCCOrigine":
                        {
                            m_IDCCOrigine = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCCOrigine":
                        {
                            m_NomeCCOrigine = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDCCDestinazione":
                        {
                            m_IDCCDestinazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCCDestinazione":
                        {
                            m_NomeCCDestinazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoMetodoDiPagamento":
                        {
                            m_TipoMetodoDiPagamento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDMetodoDiPagamento":
                        {
                            m_IDMetodoDiPagamento = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeMetodoDiPagamento":
                        {
                            m_NomeMetodoDiPagamento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
                return m_Descrizione;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Descrizione);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is VoceDiPagamento) && this.Equals((VoceDiPagamento)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(VoceDiPagamento obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                    && DMD.Decimals.EQ(this.m_Quantita, obj.m_Quantita)
                    && DMD.Strings.EQ(this.m_NomeValuta, obj.m_NomeValuta)
                    && DMD.DateUtils.EQ(this.m_DataOperazione, obj.m_DataOperazione)
                    && DMD.DateUtils.EQ(this.m_DataEffettiva, obj.m_DataEffettiva)
                    && DMD.Strings.EQ(this.m_SourceType, obj.m_SourceType)
                    && DMD.Integers.EQ(this.m_SourceID, obj.m_SourceID)
                    && DMD.Strings.EQ(this.m_SourceParams, obj.m_SourceParams)
                    && DMD.Integers.EQ(this.m_IDCCOrigine, obj.m_IDCCOrigine)
                    && DMD.Strings.EQ(this.m_NomeCCOrigine, obj.m_NomeCCOrigine)
                    && DMD.Integers.EQ(this.m_IDCCDestinazione, obj.m_IDCCDestinazione)
                    && DMD.Strings.EQ(this.m_NomeCCDestinazione, obj.m_NomeCCDestinazione)
                    && DMD.Strings.EQ(this.m_TipoMetodoDiPagamento, obj.m_TipoMetodoDiPagamento)
                    && DMD.Integers.EQ(this.m_IDMetodoDiPagamento, obj.m_IDMetodoDiPagamento)
                    && DMD.Strings.EQ(this.m_NomeMetodoDiPagamento, obj.m_NomeMetodoDiPagamento)
                    ;
            }
        }
    }
}