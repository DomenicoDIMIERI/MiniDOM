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
        /// Oggetto usato per la sincronizzazione su db
        /// </summary>
        [Serializable]
        public sealed class CSecurityToken
            : Databases.DBObject 
        {
            internal string m_TokenID;
            internal string m_TokenClass;
            internal string m_TokenSourceName;
            internal int m_TokenSourceID;
            internal string m_TokenName;
            internal string m_Valore;
            internal string m_Session;
            internal int m_UsatoDaID;
            [NonSerialized] internal CUser m_UsatoDa;
            internal DateTime? m_UsatoIl;
            internal string m_Dettaglio;
            internal DateTime? m_ExpireTime;
            internal int m_ExpireCount;
            internal int m_UseCount;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CSecurityToken()
            {
                m_TokenID = "";
                m_TokenName = "";
                m_Valore = "";
                m_Session = "";
                m_UsatoDaID = 0;
                m_UsatoDa = null;
                m_UsatoIl = default;
                m_Dettaglio = "";
                m_ExpireTime = default;
                m_ExpireCount = 1;
                m_UseCount = 0;
                m_TokenSourceName = "";
                m_TokenSourceID = 0;
                m_TokenClass = "";
            }

            /// <summary>
            /// Restituisce o imposta il tipo dell'oggetto
            /// </summary>
            public string TokenClass
            {
                get
                {
                    return m_TokenClass;
                }

                set
                {
                    string oldValue = m_TokenClass;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TokenClass = value;
                    DoChanged("TokenClass", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la sorgente
            /// </summary>
            public string TokenSourceName
            {
                get
                {
                    return m_TokenSourceName;
                }

                set
                {
                    string oldValue = m_TokenSourceName;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TokenSourceName = value;
                    DoChanged("TokenSourceName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id della sorgente
            /// </summary>
            public int TokenSourceID
            {
                get
                {
                    return m_TokenSourceID;
                }

                set
                {
                    int oldValue = m_TokenSourceID;
                    if (oldValue == value)
                        return;
                    m_TokenSourceID = value;
                    DoChanged("TokenSourceID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la stringa che identifica questo oggetto univoco
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TokenID
            {
                get
                {
                    return m_TokenID;
                }
            }

            /// <summary>
            /// Imposta il token
            /// </summary>
            /// <param name="value"></param>
            internal void SetToken(string value)
            {
                m_TokenID = value;
            }

            /// <summary>
            /// Restituisce o imposta il nome del token
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TokenName
            {
                get
                {
                    return m_TokenName;
                }

                set
                {
                    string oldValue = m_TokenName;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TokenName = value;
                    DoChanged("TokenName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il valore del token
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TokenValue
            {
                get
                {
                    return m_Valore;
                }

                set
                {
                    string oldValue = m_Valore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Valore = value;
                    DoChanged("TokenValue", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della sessione utente in cui è stato creato il token
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SessionID
            {
                get
                {
                    return m_Session;
                }

                set
                {
                    string oldValue = m_Session;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Session = value;
                    DoChanged("SessionID", value, oldValue);
                }
            }
               
            /// <summary>
            /// Restituisce l'ID dell'utente che ha usato il token per la prima volta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int UsatoDaID
            {
                get
                {
                    return DBUtils.GetID(m_UsatoDa, m_UsatoDaID);
                }
            }

            /// <summary>
            /// Restituisce l'utente che ha usato il token per la prima volta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CUser UsatoDa
            {
                get
                {
                    if (m_UsatoDa is null)
                        m_UsatoDa = Users.GetItemById(m_UsatoDaID);
                    return m_UsatoDa;
                }
            }

            /// <summary>
            /// Imposta l'utente che ha usato il token
            /// </summary>
            /// <param name="user"></param>
            internal void SetUsatoDa(CUser user)
            {
                m_UsatoDa = user;
                m_UsatoDaID = DBUtils.GetID(user, 0);
            }

            /// <summary>
            /// Restitusice la data della prima volta in cui è stato usato il token
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? UsatoIl
            {
                get
                {
                    return m_UsatoIl;
                }
            }

            /// <summary>
            /// Imposta la data di utilizzo del token
            /// </summary>
            /// <param name="value"></param>
            internal void SetUsatoIl(DateTime? value)
            {
                m_UsatoIl = value;
            }

            /// <summary>
            /// Restituisce o imposta una stringa descrittiva dell'utilizzo del token
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Dettaglio
            {
                get
                {
                    return m_Dettaglio;
                }

                set
                {
                    string oldValue = m_Dettaglio;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Dettaglio = value;
                    DoChanged("Dettaglio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la data oltra la quale il token non sarà più utilizzabile
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? ExpireTime
            {
                get
                {
                    return m_ExpireTime;
                }
            }

            /// <summary>
            /// Imposta la data di scadenza del token
            /// </summary>
            /// <param name="value"></param>
            internal void SetExpireTime(DateTime? value)
            {
                m_ExpireTime = value;
            }

            /// <summary>
            /// Restituisce il numero di volte in cui il token può essere utilizzato.
            /// Se 0 il token può essere utilizzato infinite volte
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int ExpireCount
            {
                get
                {
                    return m_ExpireCount;
                }
            }

            /// <summary>
            /// Imposta il numero massimo di utilizzi del token
            /// </summary>
            /// <param name="value"></param>
            internal void SetExpireCount(int value)
            {
                m_ExpireCount = value;
            }

            /// <summary>
            /// Restituisce il numero di volte in cui il token è stato utilizzato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int UseCount
            {
                get
                {
                    return m_UseCount;
                }
            }

            /// <summary>
            /// Imposta il numero di utilizzi del token
            /// </summary>
            /// <param name="value"></param>
            internal void SetUseCount(int value)
            {
                m_UseCount = value;
            }

            /// <summary>
            /// Utilizza il token ed aggiunge il dettaglio.
            /// Se il token è scaduto o non è più utilizzabile viene generato un errore
            /// </summary>
            /// <param name="dettaglio"></param>
            /// <remarks></remarks>
            public void Usa(string dettaglio)
            {
                lock (ASPSecurity.Lock)
                {
                    if ((m_ExpireTime.HasValue && m_ExpireTime < DMD.DateUtils.Now()) == true)
                    {
                        throw new InvalidOperationException("Il token è scaduto");
                    }
                    else if (m_ExpireCount > 0 && m_UseCount >= m_ExpireCount)
                    {
                        throw new InvalidOperationException("Superato il numero di utilizzi del token");
                    }

                    if (m_UsatoDaID == 0)
                    {
                        m_UsatoDa = Users.CurrentUser;
                        m_UsatoDaID = DBUtils.GetID(m_UsatoDa, 0);
                        m_UsatoIl = DMD.DateUtils.Now();
                    }

                    m_UseCount += 1;
                    m_Dettaglio = DMD.Strings.Combine(m_Dettaglio, dettaglio, DMD.Strings.vbNewLine);
                    Save(true);
                }
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
                return minidom.Sistema.ASPSecurity;
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_TokenID;
            }

            /// <summary>
            /// Restituisce l'id dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_TokenID);
            }

            /// <summary>
            /// Restituisce true se gli oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(minidom.Databases.DBObject obj)
            {
                return (obj is CSecurityToken) && this.Equals((CSecurityToken)obj);
            }

            /// <summary>
            /// Restituisce true se gli oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public bool Equals(CSecurityToken obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_TokenID, obj.m_TokenID)
                    && DMD.Strings.EQ(this.m_TokenClass, obj.m_TokenClass)
                    && DMD.Strings.EQ(this.m_TokenSourceName, obj.m_TokenSourceName)
                    && DMD.Integers.EQ(this.m_TokenSourceID, obj.m_TokenSourceID)
                    && DMD.Strings.EQ(this.m_TokenName, obj.m_TokenName)
                    && DMD.Strings.EQ(this.m_Valore, obj.m_Valore)
                    && DMD.Strings.EQ(this.m_Session, obj.m_Session)
                    && DMD.Integers.EQ(this.m_UsatoDaID, obj.m_UsatoDaID)
                    && DMD.DateUtils.EQ(this.m_UsatoIl, obj.m_UsatoIl)
                    && DMD.Strings.EQ(this.m_Dettaglio, obj.m_Dettaglio)
                    && DMD.DateUtils.EQ(this.m_ExpireTime, obj.m_ExpireTime)
                    && DMD.Integers.EQ(this.m_ExpireCount, obj.m_ExpireCount)
                    && DMD.Integers.EQ(this.m_UseCount, obj.m_UseCount)
                    ;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_SecurityTokens";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_TokenID = reader.Read("Token", m_TokenID);
                m_TokenName = reader.Read("TokenName", m_TokenName);
                m_Valore = reader.Read("Valore", m_Valore);
                m_Session = reader.Read("Session", m_Session);
                m_UsatoDaID = reader.Read("UsatoDa", m_UsatoDaID);
                m_UsatoIl = reader.Read("UsatoIl", m_UsatoIl);
                m_Dettaglio = reader.Read("Dettaglio", m_Dettaglio);
                m_ExpireTime = reader.Read("ExpireTime", m_ExpireTime);
                m_ExpireCount = reader.Read("ExpireCount", m_ExpireCount);
                m_UseCount = reader.Read("UseCount", m_UseCount);
                m_TokenSourceName = reader.Read("TokenSourceName", m_TokenSourceName);
                m_TokenSourceID = reader.Read("TokenSourceID", m_TokenSourceID);
                m_TokenClass = reader.Read("TokenClass", m_TokenClass);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Token", m_TokenID);
                writer.Write("TokenName", m_TokenName);
                writer.Write("Valore", m_Valore);
                writer.Write("Session", m_Session);
                writer.Write("UsatoDa", m_UsatoDaID);
                writer.Write("UsatoIl", m_UsatoIl);
                writer.Write("Dettaglio", m_Dettaglio);
                writer.Write("ExpireTime", m_ExpireTime);
                writer.Write("ExpireCount", m_ExpireCount);
                writer.Write("UseCount", m_UseCount);
                writer.Write("TokenSourceName", m_TokenSourceName);
                writer.Write("TokenSourceID", m_TokenSourceID);
                writer.Write("TokenClass", m_TokenClass);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Token", typeof(string), 255);
                c = table.Fields.Ensure("TokenName", typeof(string), 255);
                c = table.Fields.Ensure("Valore", typeof(string), 0);
                c = table.Fields.Ensure("Session", typeof(string), 255);
                c = table.Fields.Ensure("UsatoDa", typeof(int), 1);
                c = table.Fields.Ensure("UsatoIl", typeof(DateTime), 1);
                c = table.Fields.Ensure("Dettaglio", typeof(string), 0);
                c = table.Fields.Ensure("ExpireTime", typeof(DateTime), 1);
                c = table.Fields.Ensure("ExpireCount", typeof(int), 1);
                c = table.Fields.Ensure("UseCount", typeof(int), 1);
                c = table.Fields.Ensure("TokenSourceName", typeof(string), 255);
                c = table.Fields.Ensure("TokenSourceID", typeof(int), 1);
                c = table.Fields.Ensure("TokenClass", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxToken", new string[] { "Token" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxTokenName", new string[] { "TokenName" }, DBFieldConstraintFlags.Unique);
                c = table.Constraints.Ensure("idxTokenValue", new string[] { "Valore" }, DBFieldConstraintFlags.Unique);
                c = table.Constraints.Ensure("idxSession", new string[] { "Session" }, DBFieldConstraintFlags.Unique);
                c = table.Constraints.Ensure("idxUsatoDa", new string[] { "UsatoDa", "UsatoIl" }, DBFieldConstraintFlags.Unique);
                c = table.Constraints.Ensure("idxExpire", new string[] { "ExpireTime", "ExpireCount", "UseCount" }, DBFieldConstraintFlags.Unique);
                c = table.Constraints.Ensure("idxTokenSource", new string[] { "TokenSourceName", "TokenSourceID", "TokenClass" }, DBFieldConstraintFlags.Unique);
                c = table.Constraints.Ensure("idxDettaglio", new string[] { "Dettaglio" }, DBFieldConstraintFlags.Unique);
 
            }

            
            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("TokenID", m_TokenID);
                writer.WriteAttribute("TokenName", m_TokenName);
                writer.WriteAttribute("Valore", m_Valore);
                writer.WriteAttribute("Session", m_Session);
                writer.WriteAttribute("UsatoDa", m_UsatoDaID);
                writer.WriteAttribute("UsatoIl", m_UsatoIl);
                writer.WriteAttribute("ExpireTime", m_ExpireTime);
                writer.WriteAttribute("ExpireCount", m_ExpireCount);
                writer.WriteAttribute("UseCount", m_UseCount);
                writer.WriteAttribute("TokenSourceName", m_TokenSourceName);
                writer.WriteAttribute("TokenSourceID", m_TokenSourceID);
                writer.WriteAttribute("TokenClass", m_TokenClass);
                base.XMLSerialize(writer);
                writer.WriteTag("Dettaglio", m_Dettaglio);
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
                    case "TokenID": m_TokenID = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "TokenName":
                        {
                            m_TokenName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Valore":
                        {
                            m_Valore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Session":
                        {
                            m_Session = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                     
                    case "UsatoDa":
                        {
                            m_UsatoDaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "UsatoIl":
                        {
                            m_UsatoIl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Dettaglio":
                        {
                            m_Dettaglio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ExpireTime":
                        {
                            m_ExpireTime = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ExpireCount":
                        {
                            m_ExpireCount = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "UseCount":
                        {
                            m_UseCount = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TokenSourceName":
                        {
                            m_TokenSourceName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TokenSourceID":
                        {
                            m_TokenSourceID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TokenClass":
                        {
                            m_TokenClass = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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