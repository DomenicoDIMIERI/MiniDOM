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
        /// Interfaccia implementata dagli oggetti salvabili nel db
        /// </summary>
        public interface IDBObjectBase
          : IDBMinObject
        {

            /// <summary>
            /// restituisce ID dell'oggetto
            /// </summary>
            int ID { get; }

            /// <summary>
            /// Imposta l'id dell'oggetto
            /// </summary>
            /// <param name="newID"></param>
            void SetID(int newID);
            
            /// <summary>
            /// Resetta l'id dell'oggetto
            /// </summary>
            void ResetID();
            
            /// <summary>
            /// Salva nel writer
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            bool SaveToRecordset(DBWriter writer);

            /// <summary>
            /// Carica dal reader
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            bool LoadFromRecordset(DBReader reader);
            
            /// <summary>
            /// Restituisce il nome del discriminator
            /// </summary>
            /// <returns></returns>
            string GetTableName();
        }

        ///// <summary>
        ///// Firma dell'evento PropertyChangedEvent
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //public delegate void PropertyChangedEventHandler(object sender, DMDPropertyChangedEventArgs e);


        /// <summary>
        /// Rappresenta un oggetto memorizzato in una tabella con informazioni sullo stato, su data e utente che ha creato l'oggetto e sull'ultima modifica
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public abstract class DBObjectBase
            : DMD.Databases.DBObject , IDBObjectBase, DMD.XML.IDMDXMLSerializable, ICloneable
        {
             



            [NonSerialized] private CCollection<DMDPropertyChangedEventArgs> m_Changes = new CCollection<DMDPropertyChangedEventArgs>();

            /// <summary>
            /// Flags
            /// </summary>
            protected int m_Flags;


            private CKeyCollection m_Parameters;

            /// <summary>
            /// Costruttore
            /// </summary>
            public DBObjectBase()
            {
                this.m_Flags = 0;
                this.m_Parameters = null;
            }

            /// <summary>
            /// Restituisce o imposta dei flag aggiuntivi
            /// </summary>
            public int Flags
            {
                get
                {
                    return this.m_Flags;
                }
                set
                {
                    int oldValue = this.m_Flags;
                    if (oldValue == value) return;
                    this.m_Flags = value;
                    this.DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Parametri aggiuntivi
            /// </summary>
            public CKeyCollection Parameters
            {
                get
                {
                    if (this.m_Parameters is null) this.m_Parameters = new CKeyCollection();
                    return this.m_Parameters;
                }
            }

            /// <summary>
            /// Resetta l'id
            /// </summary>
            void IDBObjectBase.ResetID()
            {
                DBUtils.SetID(this, 0);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(DMDBaseObject obj)
            {
                return (obj is DBObjectBase) && this.Equals((DBObjectBase)obj);
            }
            
            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(DBObjectBase obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_Flags, obj.m_Flags)
                    //&& CollectionUtils.EQAnyOrder(this.Parameters, obj.Parameters)
                    ;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

             

            /// <summary>
            /// Genera l'evento PropChanged
            /// </summary>
            /// <param name="e"></param>
            protected override void OnPropChanged(DMDPropertyChangedEventArgs e)
            {
                this.m_Changes.Add(e);
                base.OnPropChanged(e);
            }

            /// <summary>
            /// Restituisce il repository
            /// </summary>
            /// <returns></returns>
            public abstract CModulesClass GetModule();

            /// <summary>
            /// Restituisce un riferimento alla connessione
            /// </summary>
            /// <returns></returns>
            protected virtual DBConnection GetConnection()
            {
                var m = this.GetModule();
                return (m is null) ? Sistema.ApplicationContext.APPConn : m.Database;
            }

            /// <summary>
            /// Salva l'oggetto nel repository
            /// </summary>
            /// <param name="force"></param>
            public virtual void Save(bool force = false)
            {
                this.Save(this.GetConnection(), force);
            }

            /// <summary>
            /// Rimuove l'oggetto dal repository
            /// </summary>
            /// <param name="force"></param>
            public virtual void Delete(bool force = false)
            {
                this.Delete(this.GetConnection(), force);
            }

            /// <summary>
            /// Restituisce il nome del campo ID
            /// </summary>
            /// <returns></returns>
            protected override string getIDFieldName()
            {
                return "ID";
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            protected override string getTableName()
            {
                return this.GetTableName();
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public abstract string GetTableName();

            /// <summary>
            /// Genera l'evento OnAfterDelete
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterDelete(DMDEventArgs e)
            {
                base.OnAfterDelete(e);
                var ie = new ItemEventArgs(this);
                var repo = this.GetModule();
                if (repo is object)
                {
                    if (repo.Index is object)
                        repo.Index.Unindex(this);
                    repo.doOnItemDeleted(ie);
                }
            }

            /// <summary>
            /// Genera l'evento OnAfterChange
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterChange(DMDEventArgs e)
            {
                base.OnAfterChange(e);
                var ie = new ItemEventArgs(this);
                var repo = this.GetModule();
                if (repo is object)
                {
                    if (repo.Index is object)
                        repo.Index.Index(this);
                    repo.doOnItemModified(ie);
                }
            }

            /// <summary>
            /// Genera l'evento OnAfterCreate
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterCreate(DMDEventArgs e)
            {
                base.OnAfterCreate(e);
                var ie = new ItemEventArgs(this);
                var repo = this.GetModule();
                if (repo is object)
                {
                    if (repo.Index is object)
                        repo.Index.Index(this);
                    repo.doOnItemCreated(ie);
                }
            }
             
            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected virtual bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Flags", this.m_Flags);
                writer.Write("Parameters", DMD.XML.Utils.Serialize(this.Parameters));
                this.DBWrite(writer);
                return true;
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected virtual bool LoadFromRecordset(DBReader reader)
            {
                this.m_Flags = reader.Read("Flags", this.m_Flags);
                string tmp = reader.Read("Parameters", "");
                if (!string.IsNullOrEmpty(tmp))
                    this.m_Parameters = DMD.XML.Utils.Deserialize<CKeyCollection>(tmp);
                this.DBRead(reader);
                return true;
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);
                var c = table.Fields.Ensure("Flags", typeof(int), 1);
                c = table.Fields.Ensure("Parameters", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                //var c = table.Constraints.Ensure("idxFlags", new string[] { "Flags" }, DBFieldConstraintFlags.None);
            }

            bool IDBObjectBase.SaveToRecordset(DBWriter writer)
            {
                return this.SaveToRecordset(writer);
            }

            bool IDBObjectBase.LoadFromRecordset(DBReader reader)
            {
                return this.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Restituisce l'ID dell'oggetto
            /// </summary>
            public int ID
            {
                get
                {
                    return DBUtils.GetID(this, 0);
                }
            }

            void IDBObjectBase.SetID(int value)
            {
                DBUtils.SetID(this, value);
            }

            /// <summary>
            /// Serializzazione XML
            /// </summary>
            /// <param name="writer"></param>
            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("m_ID", this.ID);
                writer.WriteAttribute("m_Changed", this.IsChanged());
                writer.WriteAttribute("Flags", this.m_Flags);
                // writer.WriteTag("Changes", Me.m_Changes)
                writer.WriteTag("Parameters", this.Parameters);
            }

            /// <summary>
            /// Deserializzazione XML
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "m_ID":
                        {
                            DBUtils.SetID(this, (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue));
                            break;
                        }

                    case "m_Changed":
                        {
                            this.SetChanged(DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true);
                            break;
                        }

                    case "Changes":
                        {
                            this.m_Changes.Clear();
                            if (fieldValue is IEnumerable)
                            {
                                this.m_Changes.AddRange((IEnumerable)fieldValue);
                            } // Throw New ArgumentOutOfRangeException(fieldName)

                            break;
                        }
                    case "Flags":
                        {
                            this.m_Flags = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);  
                            break;
                        }
                    case "Parameters":
                        this.m_Parameters = (CKeyCollection)fieldValue;
                        break;
                    default:
                        {
                            break;
                        }
                }
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue);  }
            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer);  }

            /// <summary>
            /// Clonea l'oggetto
            /// </summary>
            /// <returns></returns>
            protected virtual DBObjectBase _Clone()
            {
                var ret = (DBObjectBase) this.MemberwiseClone();
                ret.m_Parameters = null;
                foreach(var k in this.Parameters.Keys)
                {
                    ret.Parameters.Add(k, this.Parameters[k]);
                }
                return ret;
            }

            object ICloneable.Clone()
            {
                return this._Clone();
            }
        }
    }
}