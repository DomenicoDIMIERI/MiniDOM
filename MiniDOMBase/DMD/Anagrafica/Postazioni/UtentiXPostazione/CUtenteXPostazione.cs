using System;
using DMD.XML;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using System.Collections;


namespace minidom
{
    public partial class Anagrafica
    {



        /// <summary>
        /// Relazione tra gli utenti e le postazioni di lavoro
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CUtenteXPostazione 
            : DMD.XML.DMDBaseXMLObject// minidom.Databases.DBObjectBase
        {
            private string m_UserName;
            private string m_UserType;
            private int m_IDUtente;
            [NonSerialized] private Sistema.CUser m_Utente;
            private int m_IDPostazione;
            [NonSerialized] private CPostazione m_Postazione;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CUtenteXPostazione()
            {
                m_UserName = "";
                m_UserType = "";
                m_IDUtente = 0;
                m_Utente = null;
                m_IDPostazione = 0;
                m_Postazione = null;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_UserName, this.m_UserType, this.m_IDUtente,m_IDUtente, this.m_IDPostazione);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(object obj)
            {
                return (obj is CUtenteXPostazione) && this.Equals((CUtenteXPostazione)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="o"></param>
            /// <returns></returns>
            public virtual bool Equals(CUtenteXPostazione o)
            {
                return DMD.Strings.EQ(this.m_UserName, o.m_UserName)
                        && DMD.Strings.EQ(this.m_UserType, o.m_UserType)
                        && DMD.Integers.EQ(this.m_IDUtente, o.m_IDUtente)
                        && DMD.Integers.EQ(this.m_IDPostazione, o.m_IDPostazione)
                        ;
            }

            
            /// <summary>
            /// Restituisce o imposta il nome dell'utente
            /// </summary>
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
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_UserName = value;
                    DoChanged("UserName", value, oldValue);
                }
            }

            
            /// <summary>
            /// Restituisce o imposta il tipo dell'utente (administrator, standard, ecc)
            /// </summary>
            public string UserType
            {
                get
                {
                    return m_UserType;
                }

                set
                {
                    string oldValue = m_UserType;
                    value = DMD.Strings.Trim(value);
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_UserType = value;
                    DoChanged("UserType", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente
            /// </summary>
            public int IDUtente
            {
                get
                {
                    return DBUtils.GetID(this.m_Utente, this.m_IDUtente);
                }

                set
                {
                    int oldValue = this.IDUtente;
                    if (oldValue == value)
                        return;
                    m_IDUtente = value;
                    m_Utente = null;
                    DoChanged("IDUtente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente
            /// </summary>
            public Sistema.CUser Utente
            {
                get
                {
                    if (m_Utente is null)
                        m_Utente = Sistema.Users.GetItemById(m_IDUtente);
                    return m_Utente;
                }

                set
                {
                    var oldValue = Utente;
                    if (oldValue == value)
                        return;
                    m_Utente = value;
                    m_IDUtente = DBUtils.GetID(value, 0);
                    DoChanged("Utente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della postazione
            /// </summary>
            public int IDPostazione
            {
                get
                {
                    return DBUtils.GetID(m_Postazione, m_IDPostazione);
                }

                set
                {
                    int oldValue = IDPostazione;
                    if (oldValue == value)
                        return;
                    m_IDPostazione = value;
                    m_Postazione = null;
                    DoChanged("IDPostazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la postazione
            /// </summary>
            public CPostazione Postazione
            {
                get
                {
                    if (m_Postazione is null)
                        m_Postazione = Postazioni.GetItemById(m_IDPostazione);
                    return m_Postazione;
                }

                set
                {
                    var oldValue = Postazione;
                    if (oldValue == value)
                        return;
                    m_Postazione = value;
                    m_IDPostazione = DBUtils.GetID(value, 0);
                    DoChanged("Postazione", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la postazione
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetPostazione(CPostazione value)
            {
                m_Postazione = value;
                m_IDPostazione = DBUtils.GetID(value, 0);
            }

            ///// <summary>
            ///// Restituisce un riferimento al gestore delle risorse
            ///// </summary>
            ///// <returns></returns>
            //public override CModulesClass GetModule()
            //{
            //    return null;
            //}

            ///// <summary>
            ///// Restituisce un riferimento al DB
            ///// </summary>
            ///// <returns></returns>
            //protected internal override DBConnection GetConnection()
            //{
            //    return Anagrafica.Postazioni.Database;
            //}

            ///// <summary>
            ///// Restituisce il nome della tabella
            ///// </summary>
            ///// <returns></returns>
            //public override string GetTableName()
            //{
            //    return "tbl_UtentiXPostazione";
            //}

            ///// <summary>
            ///// Carica dal DB
            ///// </summary>
            ///// <param name="reader"></param>
            ///// <returns></returns>
            //protected override bool LoadFromRecordset(DBReader reader)
            //{
            //    m_UserName = reader.Read("UserName", m_UserName);
            //    m_UserType = reader.Read("UserType", m_UserType);
            //    m_IDUtente = reader.Read("Utente",  m_IDUtente);
            //    m_IDPostazione = reader.Read("Postazione", m_IDPostazione);
            //    return base.LoadFromRecordset(reader);
            //}

            ///// <summary>
            ///// Salva nel DB
            ///// </summary>
            ///// <param name="writer"></param>
            ///// <returns></returns>
            //protected override bool SaveToRecordset(DBWriter writer)
            //{
            //    writer.Write("Utente", IDUtente);
            //    writer.Write("Postazione", IDPostazione);
            //    writer.Write("UserName", m_UserName);
            //    writer.Write("UserType", m_UserType);
            //    return base.SaveToRecordset(writer);
            //}

            ///// <summary>
            ///// Prepara lo schema
            ///// </summary>
            ///// <param name="table"></param>
            //protected override void PrepareDBSchemaFields(DBTable table)
            //{
            //    base.PrepareDBSchemaFields(table);
            //    var c = table.Fields.Ensure("Utente", typeof(int), 0);
            //    c = table.Fields.Ensure("Postazione", typeof(int), 0);
            //    c = table.Fields.Ensure("UserName", typeof(string), 255);
            //    c = table.Fields.Ensure("UserType", typeof(string), 255);
            //}

            ///// <summary>
            ///// Prepara i vincoli
            ///// </summary>
            ///// <param name="table"></param>
            //protected override void PrepareDBSchemaConstraints(DBTable table)
            //{
            //    base.PrepareDBSchemaConstraints(table);
            //    table.Constraints.Ensure("idxUtente", new string[] { "Utente", "UserName", "UserType" }, true);
            //    table.Constraints.Ensure("idxPostazione", new string[] { "Postazione" }, true);
            //}

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("UtenteXPostazione[", IDUtente , ", " , IDPostazione , "]");
            }

            

            /// <summary>
            /// Serializzazione XML
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDUtente", IDUtente);
                writer.WriteAttribute("IDPostazione", IDPostazione);
                writer.WriteAttribute("UserName", m_UserName);
                writer.WriteAttribute("UserType", m_UserType);
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
                    case "IDUtente":
                        {
                            m_IDUtente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDPostazione":
                        {
                            m_IDPostazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "UserName":
                        {
                            m_UserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "UserType":
                        {
                            m_UserType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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