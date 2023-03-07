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
        /// Interfaccia implementata dagli oggetti creati da un utente
        /// </summary>
        public interface IDBObject 
            : IDBObjectBase
        {
            /// <summary>
            /// Restituisce l'utente che ha creato l'oggetto
            /// </summary>
            Sistema.CUser CreatoDa { get; }

            /// <summary>
            /// Restituisce l'id dell'utente che ha creato l'oggetto
            /// </summary>
            int CreatoDaId { get; }

            /// <summary>
            /// Restituisce la data di creazione
            /// </summary>
            DateTime CreatoIl { get; }

            /// <summary>
            /// Restituisce l'utente che ha effettuato l'utima modifica
            /// </summary>
            Sistema.CUser ModificatoDa { get; }

            /// <summary>
            /// Restituisce l'id dell'utente che ha effettuato l'ultima modifica
            /// </summary>
            int ModificatoDaId { get; }

            /// <summary>
            /// Restituisce la data dell'ultima modifica
            /// </summary>
            DateTime ModificatoIl { get; }

            /// <summary>
            /// Restituisce o imposta lo stato dell'oggetto
            /// </summary>
            ObjectStatus Stato { get; set; }

            /// <summary>
            /// Forza l'utente di creazione
            /// </summary>
            /// <param name="user"></param>
            void ForceUser(Sistema.CUser user);
        }

        /// <summary>
        /// Rappresenta un oggetto memorizzato in una tabella con informazioni sullo stato, su data e utente che ha creato l'oggetto e sull'ultima modifica
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public abstract class DBObject 
            : minidom.Databases.DBObjectBase, IDBObject
        {
            private int m_CreatoDaID; // ID dell'utente che ha creato la persona
            [NonSerialized] private Sistema.CUser m_CreatoDa; // Oggetto CUser dell'utente che ha creato la persona
            private DateTime m_CreatoIl; // Data e ora di creazione dell'oggetto
            private int m_ModificatoDaID; // ID dell'utente che ha modificato la persona
            [NonSerialized] private Sistema.CUser m_ModificatoDa; // Oggetto CUser dell'utente che ha modificato la persona
            private DateTime m_ModificatoIl;
            private ObjectStatus m_Stato; // Stato dell'oggetto (0 = temporaneo, 1 = ok, 2 eliminato)
            [NonSerialized] private ObjectStatus m_StatoOld; // Stato dell'oggetto (0 = temporaneo, 1 = ok, 2 eliminato)
            

            /// <summary>
            /// Costruttore
            /// </summary>
            public DBObject()
            {
                m_CreatoDaID = 0;
                m_CreatoDa = null;
                m_CreatoIl = default;
                m_ModificatoDaID = 0;
                m_ModificatoDa = null;
                m_ModificatoIl = default;
                m_Stato = ObjectStatus.OBJECT_TEMP;
                m_StatoOld = m_Stato;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(base.GetHashCode(), this.CreatoDaId, this.m_CreatoIl, this.ModificatoDaId, this.m_ModificatoIl, this.m_Stato);
            }

            /// <summary>
            /// Restituisce true se l'oggetto è uguale
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override sealed bool Equals(DBObjectBase obj)
            {
                return this.Equals((DBObject)obj);
            }

            /// <summary>
            /// Restituisce true se l'oggetto è uguale
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(DBObject obj)
            {
                return base.Equals(obj) &&
                          Integers.EQ(this.CreatoDaId, obj.CreatoDaId)
                       && Integers.EQ(this.ModificatoDaId, obj.ModificatoDaId)
                       && DateUtils.EQ(this.m_CreatoIl, obj.m_CreatoIl)
                       && DateUtils.EQ(this.m_ModificatoIl, obj.m_ModificatoIl)
                       && Integers.EQ((int)this.m_Stato, (int)obj.m_Stato)
                       ;

            }

            
            Sistema.CUser IDBObject.CreatoDa { get { return this.CreatoDa; } }

            int IDBObject.CreatoDaId { get { return this.CreatoDaId; } }
            DateTime IDBObject.CreatoIl { get { return this.CreatoIl; } }
            Sistema.CUser IDBObject.ModificatoDa { get { return this.ModificatoDa; } }
            int IDBObject.ModificatoDaId { get { return this.ModificatoDaId; } }
            DateTime IDBObject.ModificatoIl { get { return this.ModificatoIl; } }
            ObjectStatus IDBObject.Stato { get { return this.Stato; } set { this.Stato = value; } }
            void IDBObject.ForceUser(Sistema.CUser user) { this.ForceUser(user);  }

            /// <summary>
            /// Restituisce l'utente che ha creato l'oggetto
            /// </summary>
            public Sistema.CUser CreatoDa
            {
                get
                {
                    if (m_CreatoDa is null)
                        m_CreatoDa = Sistema.Users.GetItemById(m_CreatoDaID);
                    return m_CreatoDa;
                }
            }

            /// <summary>
            /// Restituisce l'ID dell'utente che ha creato l'oggetto
            /// </summary>
            public int CreatoDaId
            {
                get
                {
                    return DBUtils.GetID(m_CreatoDa, m_CreatoDaID);
                }
            }

            /// <summary>
            /// Restituisce la data di creazione dell'oggetto
            /// </summary>
            public DateTime CreatoIl
            {
                get
                {
                    return m_CreatoIl;
                }
            }

            /// <summary>
            /// Restituisce l'utente che ha effettuato l'ultima modifica
            /// </summary>
            public Sistema.CUser ModificatoDa
            {
                get
                {
                    if (m_ModificatoDa is null)
                        m_ModificatoDa = Sistema.Users.GetItemById(m_ModificatoDaID);
                    return m_ModificatoDa;
                }
            }

            /// <summary>
            /// Restituisce l'id dell'utente che ha effettuato l'ultima modifica
            /// </summary>
            public int ModificatoDaId
            {
                get
                {
                    return DBUtils.GetID(m_ModificatoDa, m_ModificatoDaID);
                }
            }

            /// <summary>
            /// Restituisce la data dell'ultima modifica
            /// </summary>
            public DateTime ModificatoIl
            {
                get
                {
                    return m_ModificatoIl;
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato dell'oggetto
            /// </summary>
            public ObjectStatus Stato
            {
                get
                {
                    return m_Stato;
                }

                set
                {
                    var oldValue = m_Stato;
                    if (oldValue == value)
                        return;
                    m_Stato = value;
                    DoChanged("Stato", value, oldValue);
                }
            }

            /// <summary>
            /// OnBeforeSave
            /// </summary>
            /// <param name="e"></param>
            protected override void OnBeforeSave(DMDEventArgs e)
            {
                var tmp = this.m_StatoOld;

                base.OnBeforeSave(e);

                if (tmp == ObjectStatus.OBJECT_TEMP & this.m_Stato == ObjectStatus.OBJECT_VALID)
                {
                    this.OnBeforeCreate(e);
                }
                else if (tmp == ObjectStatus.OBJECT_VALID & m_Stato == ObjectStatus.OBJECT_DELETED)
                {
                    this.OnBeforeDelete(e);
                }
                else if (m_Stato == ObjectStatus.OBJECT_VALID)
                {
                    this.OnBeforeChange(e);
                }
            }

            /// <summary>
            /// OnAfterSave
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterSave(DMDEventArgs e)
            {
                var tmp = this.m_StatoOld;
                this.m_StatoOld = m_Stato;

                base.OnAfterSave(e);

                if (tmp == ObjectStatus.OBJECT_TEMP & this.m_Stato == ObjectStatus.OBJECT_VALID)
                {
                    this.OnAfterCreate(e);
                }
                else if (tmp == ObjectStatus.OBJECT_VALID & m_Stato == ObjectStatus.OBJECT_DELETED)
                {
                    this.OnAfterDelete(e);
                }
                else if (m_Stato == ObjectStatus.OBJECT_VALID)
                {
                    this.OnAfterChange(e);
                }

                
            }

            /// <summary>
            /// Forza l'utente che ha creato l'utente
            /// </summary>
            /// <param name="value"></param>
            public void ForceUser(Sistema.CUser value)
            {
                m_CreatoDaID = DBUtils.GetID(value, 0);
                m_CreatoDa = value;
                m_CreatoIl = DMD.DateUtils.Now();
                m_ModificatoDaID = m_CreatoDaID;
                m_ModificatoDa = m_CreatoDa;
                m_ModificatoIl = m_CreatoIl;
                SetChanged(true);
            }

            /// <summary>
            /// Forza l'utente che ha creato l'utente e la data di creazione
            /// </summary>
            /// <param name="value"></param>
            /// <param name="creatoIl"></param>
            public void ForceUser(Sistema.CUser value, DateTime creatoIl)
            {
                m_CreatoDaID = DBUtils.GetID(value, 0);
                m_CreatoDa = value;
                m_CreatoIl = creatoIl;
                m_ModificatoDaID = m_CreatoDaID;
                m_ModificatoDa = m_CreatoDa;
                m_ModificatoIl = m_CreatoIl;
                SetChanged(true);
            }

            /// <summary>
            /// Carica dal database
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_CreatoDaID = reader.Read("CreatoDa", m_CreatoDaID);
                m_CreatoIl = reader.Read("CreatoIl", m_CreatoIl);
                m_ModificatoDaID = reader.Read("ModificatoDa", m_ModificatoDaID);
                m_ModificatoIl = reader.Read("ModificatoIl", m_ModificatoIl);
                m_Stato = reader.Read("Stato", m_Stato);
                m_StatoOld = m_Stato;
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel writer
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                m_ModificatoDa = Sistema.Users.CurrentUser;
                m_ModificatoDaID = DBUtils.GetID(m_ModificatoDa, 0);
                m_ModificatoIl = DMD.DateUtils.Now();
                if (m_CreatoDaID == 0)
                {
                    m_CreatoDa = m_ModificatoDa;
                    m_CreatoDaID = m_ModificatoDaID;
                    m_CreatoIl = m_ModificatoIl;
                }

                writer.Write("CreatoDa", DBUtils.GetID(m_CreatoDa, m_CreatoDaID));
                writer.Write("CreatoIl", m_CreatoIl);
                writer.Write("ModificatoDa", DBUtils.GetID(m_ModificatoDa, m_ModificatoDaID));
                writer.Write("ModificatoIl", m_ModificatoIl);
                writer.Write("Stato", m_Stato);
                return base.SaveToRecordset(writer);
            }

          
            /// <summary>
            /// Prepara i campi della tabella
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("CreatoDa", typeof(int), 0);
                c = table.Fields.Ensure("CreatoIl", typeof(DateTime), 0);
                c = table.Fields.Ensure("ModificatoDa", typeof(int), 0);
                c = table.Fields.Ensure("ModificatoIl", typeof(DateTime), 0);
                c = table.Fields.Ensure("Stato", typeof(int), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxCreatoDa", new string[] { "CreatoDa", "CreatoIl" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxModificatoDa", new string[] { "ModificatoDa", "ModificatoIl" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxStato", new string[] { "Stato" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione XML
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("m_CreatoDaID", m_CreatoDaID);
                writer.WriteAttribute("m_CreatoIl", m_CreatoIl);
                writer.WriteAttribute("m_ModificatoDaID", m_ModificatoDaID);
                writer.WriteAttribute("m_ModificatoIl", m_ModificatoIl);
                writer.WriteAttribute("m_Stato", (int?)m_Stato);
                writer.WriteAttribute("StatoOld", (int?)m_StatoOld);
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
                    case "m_CreatoDaID":
                        {
                            m_CreatoDaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "m_CreatoIl":
                        {
                            m_CreatoIl = Sistema.Formats.ToDate(DMD.XML.Utils.Serializer.DeserializeDate(fieldValue));
                            break;
                        }

                    case "m_ModificatoDaID":
                        {
                            m_ModificatoDaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "m_ModificatoIl":
                        {
                            m_ModificatoIl = Sistema.Formats.ToDate(DMD.XML.Utils.Serializer.DeserializeDate(fieldValue));
                            break;
                        }

                    case "m_Stato":
                        {
                            m_Stato = (ObjectStatus)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoOld":
                        {
                            m_StatoOld = (ObjectStatus)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
            /// Copia i valori dall'oggetto
            /// </summary>
            /// <param name="value"></param>
            protected virtual void _CopyFrom(object value)
            {
                var f1 = DMD.RunTime.GetAllFields(value.GetType());
                var f2 = DMD.RunTime.GetAllFields(typeof(DBObject));
                var f3 = DMD.RunTime.GetAllFields(typeof(DBObjectBase));
                foreach (System.Reflection.FieldInfo f in f1)
                {
                    if (!f.IsInitOnly && DMD.Arrays.IndexOf(f2, f) < 0 && DMD.Arrays.IndexOf(f3, f) < 0)
                    {
                        f.SetValue(this, f.GetValue(value));
                    }
                }
            }

            

            /// <summary>
            /// Elimina l'oggetto impostando lo stato su OBJECT_DELETED
            /// </summary>
            /// <param name="force"></param>
            public override void Delete(bool force = false)
            {
                Stato = ObjectStatus.OBJECT_DELETED;
                Save();
            }
        }
    }
}