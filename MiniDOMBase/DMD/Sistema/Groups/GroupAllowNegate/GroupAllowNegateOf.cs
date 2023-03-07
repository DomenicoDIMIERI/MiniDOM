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
    /// Classe base che consente di definire una relazione generica tra un gruppo ed un oggetto
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class GroupAllowNegate<T> 
        : Databases.DBObjectBase
    {
        private int m_ItemID;
        [NonSerialized] private T m_Item;
        private int m_GroupID;
        [NonSerialized] private CGroup m_Group;
        private bool m_Allow;
        private bool m_Negate;

        /// <summary>
        /// Costruttore
        /// </summary>
        public GroupAllowNegate()
        {
            this.m_ItemID = 0;
            this.m_Item = default;
            this.m_GroupID = 0;
            this.m_Group = null;
            this.m_Allow = false;
            this.m_Negate = false;
        }

        /// <summary>
        /// Restituisce una stringa che rappresenta l'oggetto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Strings.ConcatArray(this.GroupName, " ", this.Allow, ", " , this.Negate);
        }

        /// <summary>
        /// Restituisce il codice hash dell'oggetto
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCalculator.Calculate(this.m_GroupID, this.m_ItemID);
        }

        /// <summary>
        /// Restituisce true se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public sealed override bool Equals(Databases.DBObjectBase obj)
        {
            return (obj is GroupAllowNegate<T>) && this.Equals((GroupAllowNegate<T>)obj);
        }

        /// <summary>
        /// Restituisce true se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Equals(GroupAllowNegate<T> obj)
        {
            return base.Equals(obj)
                && DMD.Integers.EQ(this.m_ItemID, obj.m_ItemID)
                && DMD.Integers.EQ(this.m_GroupID, obj.m_GroupID)
                && DMD.Booleans.EQ(this.m_Allow, obj.m_Allow)
                 && DMD.Booleans.EQ(this.m_Negate, obj.m_Negate)
                ;
        }

        /// <summary>
        /// Restituisce o imposta l'ID dell'oggetto
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
        /// Restituisce o imposta l'id del gruppo
        /// </summary>
        public int GroupID
        {
            get
            {
                return DBUtils.GetID(m_Group, m_GroupID);
            }

            set
            {
                int oldValue = GroupID;
                if (oldValue == value)
                    return;
                m_GroupID = value;
                m_Group = null;
                DoChanged("GroupID", value, oldValue);
            }
        }

        /// <summary>
        /// Restituisce o imposta il gruppo
        /// </summary>
        public Sistema.CGroup Group
        {
            get
            {
                if (m_Group is null)
                    m_Group = Sistema.Groups.GetItemById(m_GroupID);
                return m_Group;
            }

            set
            {
                var oldValue = m_Group;
                if (ReferenceEquals(oldValue, value))
                    return;
                m_Group = value;
                m_GroupID = DBUtils.GetID(value, 0);
                DoChanged("Group", value, oldValue);
            }
        }

        /// <summary>
        /// Restituisce il nome del gruppo
        /// </summary>
        public string GroupName
        {
            get
            {
                if (Group is null)
                    return "Group[" + GroupID + "]";
                return Group.GroupName;
            }
        }

        /// <summary>
        /// Restituisce o imposta un valore booleano che indica se l'oggetto è abilitato esplicitamente per il gruppo
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
        /// Restituisce o imposta un valore booleano che indica se l'oggetto è abilitato esplicitamente per il gruppo
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
        /// Restituisce il nome del campo id che identifica l'oggetto nella tabella
        /// </summary>
        /// <returns></returns>
        protected abstract string GetItemFieldName();

        /// <summary>
        /// Restituisce il nome del campo id del gruppo nella tabella
        /// </summary>
        /// <returns></returns>
        protected virtual string GetGroupFieldName()
        {
            return "Gruppo";
        }

        /// <summary>
        /// Restituisce il nome della tabella
        /// </summary>
        /// <returns></returns>
        public abstract override string GetTableName();

        /// <summary>
        /// Carica dal database
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override bool LoadFromRecordset(DBReader reader)
        {
            m_ItemID = reader.Read(GetItemFieldName(), m_ItemID);
            m_GroupID = reader.Read(GetGroupFieldName(), m_GroupID);
            m_Allow = reader.Read("Allow", m_Allow);
            m_Negate = reader.Read("Negate", m_Negate);
            return base.LoadFromRecordset(reader);
        }

        /// <summary>
        /// Salva nel database
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected override bool SaveToRecordset(DBWriter writer)
        {
            writer.Write(GetItemFieldName(), ItemID);
            writer.Write(GetGroupFieldName(), GroupID);
            writer.Write("Allow", m_Allow);
            writer.Write("Negate", m_Negate);
            return base.SaveToRecordset(writer);
        }

        /// <summary>
        /// Prepara i campi
        /// </summary>
        /// <param name="table"></param>
        protected override void PrepareDBSchemaFields(DBTable table)
        {
            base.PrepareDBSchemaFields(table);

            var c = table.Fields.Ensure(this.getIDFieldName(), typeof(int), 1);
            c = table.Fields.Ensure(this.GetGroupFieldName(), typeof(int), 1);
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

            var c = table.Constraints.Ensure("idxGrpAllowNegate", new string[] { this.getIDFieldName(), this.GetGroupFieldName() }, DBFieldConstraintFlags.PrimaryKey);

        }

        /// <summary>
        /// Serializzazione xml
        /// </summary>
        /// <param name="writer"></param>
        protected override void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute(GetItemFieldName(), ItemID);
            writer.WriteAttribute(GetGroupFieldName(), GroupID);
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

                case var case1 when case1 == (GetGroupFieldName() ?? ""):
                    {
                        m_GroupID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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