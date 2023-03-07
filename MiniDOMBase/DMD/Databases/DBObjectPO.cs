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
    public partial class Databases
    {


        /// <summary>
        /// Interfaccia implementata dagli oggetti che sono associati ad un punto operativo
        /// </summary>
        public interface IDBPOObject
        {
            /// <summary>
            /// Restituisce o imposta l'id del punto operativo
            /// </summary>
            int IDPuntoOperativo { get; set; }

            /// <summary>
            /// Restituisce o imposta il punto operativo
            /// </summary>
            CUfficio PuntoOperativo { get; set; }

            /// <summary>
            /// Restituisce o imposta il nome del punto operativo
            /// </summary>
            string NomePuntoOperativo { get; set; }
        }

        /// <summary>
        /// Estende l'oggetto DBObject con informazioni sul punto operativo a cui è assegnato l'oggetto
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public abstract class DBObjectPO 
            : minidom.Databases.DBObject, IDBPOObject
        {
            private int m_IDPuntoOperativo;
            private string m_NomePuntoOperativo;
            [NonSerialized] private CUfficio m_PuntoOperativo;

            /// <summary>
            /// Costruttore
            /// </summary>
            public DBObjectPO()
            {
                m_IDPuntoOperativo = 0;
                m_NomePuntoOperativo = DMD.Strings.vbNullString;
                m_PuntoOperativo = null;
            }

            /// <summary>
            /// Restituisce o imposta l'ID del punto operativo a cui è assegnato l'oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDPuntoOperativo
            {
                get
                {
                    return DBUtils.GetID(m_PuntoOperativo, m_IDPuntoOperativo);
                }

                set
                {
                    int oldValue = IDPuntoOperativo;
                    if (value == oldValue)
                        return;
                    m_IDPuntoOperativo = value;
                    m_PuntoOperativo = null;
                    this.DoChanged("IDPuntoOperativo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del punto operativo a cui è assegnato l'oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomePuntoOperativo
            {
                get
                {
                    return m_NomePuntoOperativo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomePuntoOperativo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePuntoOperativo = value;
                    DoChanged("NomePuntoOperativo", value, oldValue);
                }
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDPuntoOperativo = reader.Read("IDPuntoOperativo", m_IDPuntoOperativo);
                m_NomePuntoOperativo = reader.Read("NomePuntoOperativo", m_NomePuntoOperativo);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel DB
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDPuntoOperativo", IDPuntoOperativo);
                writer.Write("NomePuntoOperativo", m_NomePuntoOperativo);
                return base.SaveToRecordset(writer);
            }

           /// <summary>
           /// Prepara i campi della tabella
           /// </summary>
           /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);
                var c = table.Fields.Ensure("IDPuntoOperativo", typeof(int), 0);
                c = table.Fields.Ensure("NomePuntoOperativo", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli sulla tabella
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxPuntoOperativo", new string[] { "IDPuntoOperativo", "NomePuntoOperativo" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione XML
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("m_IDPuntoOperativo", IDPuntoOperativo);
                writer.WriteAttribute("m_NomePuntoOperativo", m_NomePuntoOperativo);
                base.XMLSerialize(writer);
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
                    case "m_IDPuntoOperativo":
                        {
                            m_IDPuntoOperativo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "m_NomePuntoOperativo":
                        {
                            m_NomePuntoOperativo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Restituisce o imposta l'oggetto Ufficio che rappresenta il punto operativo a cui è assegnato l'oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CUfficio PuntoOperativo
            {
                get
                {
                    if (m_PuntoOperativo is null)
                        m_PuntoOperativo = Anagrafica.Uffici.GetItemById(m_IDPuntoOperativo);
                    return m_PuntoOperativo;
                }

                set
                {
                    var oldValue = m_PuntoOperativo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PuntoOperativo = value;
                    m_IDPuntoOperativo = DBUtils.GetID(value, 0);
                    m_NomePuntoOperativo = "";
                    if (value is object)
                        m_NomePuntoOperativo = value.Nome;
                    DoChanged("PuntoOperativo", value, oldValue);
                }
            }

            // Public Overrides Sub InitializeFrom(value As Object)
            // MyBase.InitializeFrom(value)
            // End Sub
        }
    }
}