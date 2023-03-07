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

    /// <summary>
    /// Oggetto base da cui derivano le relzioni di tipo allow/negate tra un oggetto generico ed un utente
    /// </summary>
    /// <typeparam name="T"></typeparam>

    [Serializable]
    public abstract class UserAllowNegate<T> 
        : Databases.DBObjectBase
    {
        private int m_ItemID;
        [NonSerialized] private T m_Item;
        private int m_UserID;
        [NonSerialized] private Sistema.CUser m_User;
        private bool m_Allow;
        private bool m_Negate;

        /// <summary>
        /// Costruttore
        /// </summary>
        public UserAllowNegate()
        {
            this.m_ItemID = 0;
            this.m_Item = default;
            this.m_UserID = 0;
            this.m_User = null;
            this.m_Allow = false;
            this.m_Negate = false;
        }

        /// <summary>
        /// Restituisce o imposta l'id dell'oggetto
        /// </summary>
        public int ItemID
        {
            get
            {
                return DBUtils.GetID(m_Item, m_ItemID);
            }

            set
            {
                int oldValue = ItemID;
                if (oldValue == value)
                    return;
                m_Item = default;
                m_ItemID = value;
                DoChanged("ItemID", value, oldValue);
            }
        }

        /// <summary>
        /// Restituisce o imposta l'oggetto
        /// </summary>
        public T Item
        {
            get
            {
                return m_Item;
            }

            set
            {
                object oldValue = m_Item;
                object newValue = value;
                if (ReferenceEquals(oldValue, newValue))
                    return;
                m_Item = value;
                m_ItemID = DBUtils.GetID(value, 0);
                DoChanged("Item", value, oldValue);
            }
        }

        /// <summary>
        /// Imposta l'oggetto
        /// </summary>
        /// <param name="item"></param>
        protected internal void SetItem(T item)
        {
            m_Item = item;
            m_ItemID = DBUtils.GetID(item, 0);
        }

        /// <summary>
        /// Restituisce o imposta l'id dell'utente
        /// </summary>
        public int UserID
        {
            get
            {
                return DBUtils.GetID(m_User, m_UserID);
            }

            set
            {
                int oldValue = UserID;
                if (oldValue == value)
                    return;
                m_UserID = value;
                m_User = null;
                DoChanged("UserID", value, oldValue);
            }
        }

        /// <summary>
        /// Restituisce o imposta l'utente
        /// </summary>
        public CUser User
        {
            get
            {
                if (m_User is null)
                    m_User = Sistema.Users.GetItemById(m_UserID);
                return m_User;
            }

            set
            {
                var oldValue = m_User;
                if (ReferenceEquals(oldValue, value))
                    return;
                m_User = value;
                m_UserID = DBUtils.GetID(value, 0);
                DoChanged("User", value, oldValue);
            }
        }

        /// <summary>
        /// Imposta l'utente
        /// </summary>
        /// <param name="value"></param>
        protected internal virtual void SetUser(CUser value)
        {
            this.m_User = value;
            this.m_UserID = DBUtils.GetID(value, 0);
        }

        /// <summary>
        /// Nominativo dell'utente
        /// </summary>
        public string Nominativo
        {
            get
            {
                if (User is null)
                    return "User[" + UserID + "]";
                return User.UserName + " (" + User.Nominativo + ")";
            }
        }

        /// <summary>
        /// Restituisce una stringa che rappresenta l'oggetto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Strings.ConcatArray(this.Nominativo, " ", this.Allow, ", ", this.Negate);
        }

        /// <summary>
        /// Restituisce il codice hash dell'oggetto
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCalculator.Calculate(this.m_ItemID, this.m_UserID);
        }

        /// <summary>
        /// Restituisce true se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public sealed override bool Equals(Databases.DBObjectBase obj)
        {
            return (obj is UserAllowNegate<T>) && this.Equals((UserAllowNegate<T>)obj);
        }

        /// <summary>
        /// Restituisce true se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Equals(UserAllowNegate<T> obj)
        {
            return base.Equals(obj)
                && DMD.Integers.EQ(this.m_ItemID, obj.m_ItemID)
                && DMD.Integers.EQ(this.m_UserID, obj.m_UserID)
                && DMD.Booleans.EQ(this.m_Allow, obj.m_Allow)
                && DMD.Booleans.EQ(this.m_Negate, obj.m_Negate)
                ;
        }

        /// <summary>
        /// Restituisce o imposta un valore booleano che indica se la risorsa é consentita all'utente
        /// </summary>
        public bool Allow
        {
            get
            {
                return m_Allow;
            }

            set
            {
                if (m_Allow == value)
                    return;
                m_Allow = value;
                DoChanged("Allow", m_Allow, !value);
            }
        }

        /// <summary>
        /// Restituisce o imposta un valore booleano che indica se la risorsa é negata all'utente
        /// </summary>
        public bool Negate
        {
            get
            {
                return m_Negate;
            }

            set
            {
                if (m_Negate == value)
                    return;
                m_Negate = value;
                DoChanged("Negate", m_Negate, !value);
            }
        }

        /// <summary>
        /// Restituisce il nome del campo id dell'oggetto nella tabella
        /// </summary>
        /// <returns></returns>
        protected abstract string GetItemFieldName();

        /// <summary>
        /// Restituisce il nome del campo id dell'utente nella tabella
        /// </summary>
        /// <returns></returns>
        protected virtual string GetUserFieldName()
        {
            return "Utente";
        }

        /// <summary>
        /// Restitusice il nome della tabella
        /// </summary>
        /// <returns></returns>
        public abstract override string GetTableName();


        /// <summary>
        /// Carica dal db
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override bool LoadFromRecordset(DBReader reader)
        {
            this.m_ItemID = reader.Read(GetItemFieldName(), this. m_ItemID);
            this.m_UserID = reader.Read(GetUserFieldName(), this.m_UserID);
            this.m_Allow = reader.Read("Allow", this.m_Allow);
            this.m_Negate = reader.Read("Negate", this.m_Negate);
            return base.LoadFromRecordset(reader);
        }

        /// <summary>
        /// Salva nel db
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected override bool SaveToRecordset(DBWriter writer)
        {
            writer.Write(GetItemFieldName(), ItemID);
            writer.Write(GetUserFieldName(), UserID);
            writer.Write("Allow", m_Allow);
            writer.Write("Negate", this.m_Negate);
            return base.SaveToRecordset(writer);
        }

        /// <summary>
        /// Prepara lo schema
        /// </summary>
        /// <param name="table"></param>
        protected override void PrepareDBSchemaFields(DBTable table)
        {
            base.PrepareDBSchemaFields(table);

            var c = table.Fields.Ensure(this.GetItemFieldName(), typeof(string), 255);
            c = table.Fields.Ensure(this.GetUserFieldName(), typeof(string), 255);
            c = table.Fields.Ensure("Allow", typeof(bool), 1);
            c = table.Fields.Ensure("Negate", typeof(bool), 1);

        }

        /// <summary>
        /// Prepara i vincoli
        /// </summary>
        /// <param name="table"></param>
        protected override void PrepareDBSchemaConstraints(DBTable table)
        {
            base.PrepareDBSchemaConstraints(table);

            var c = table.Constraints.Ensure("idxUserAllowNegate", new string[] { this.GetItemFieldName(), this.GetUserFieldName() }, DBFieldConstraintFlags.PrimaryKey);
        }

        /// <summary>
        /// Serializzazione xml
        /// </summary>
        /// <param name="writer"></param>
        protected override void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute(GetItemFieldName(), ItemID);
            writer.WriteAttribute(GetUserFieldName(), UserID);
            writer.WriteAttribute("Allow", m_Allow);
            writer.WriteAttribute("Negate", m_Negate);
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
                case var @case when @case == (GetItemFieldName() ?? ""):
                    {
                        m_ItemID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case var case1 when case1 == (GetUserFieldName() ?? ""):
                    {
                        m_UserID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "Allow":
                    {
                        m_Allow = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                        break;
                    }
                case "Negate":
                    {
                        m_Negate = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
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