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
    /// Cursore sulla tabella di oggetti GroupAllowNegateCursor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class GroupAllowNegateCursor<T> 
        : minidom.Databases.DBObjectCursorBase<GroupAllowNegate<T>>
    {
        private DBCursorField<int> m_ItemID = null;
        private DBCursorField<int> m_GroupID = null;
        private DBCursorField<bool> m_Allow = new DBCursorField<bool>("Allow");
        private DBCursorField<bool> m_Negate = new DBCursorField<bool>("Negate");

        /// <summary>
        /// Costruttore
        /// </summary>
        public GroupAllowNegateCursor()
        {
        }

        /// <summary>
        /// ItemID
        /// </summary>
        public DBCursorField<int> ItemID
        {
            get
            {
                if (m_ItemID is null)
                    m_ItemID = new DBCursorField<int>(GetItemFieldName());
                return m_ItemID;
            }
        }

        /// <summary>
        /// GroupID
        /// </summary>
        public DBCursorField<int> GroupID
        {
            get
            {
                if (m_GroupID is null)
                    m_GroupID = new DBCursorField<int>(GetGroupFieldName());
                return m_GroupID;
            }
        }

        /// <summary>
        /// Allow
        /// </summary>
        public DBCursorField<bool> Allow
        {
            get
            {
                return m_Allow;
            }
        }

        /// <summary>
        /// Allow
        /// </summary>
        public DBCursorField<bool> Negate
        {
            get
            {
                return m_Negate;
            }
        }

        /// <summary>
        /// Restituisce il nome del campo id dell'oggetto
        /// </summary>
        /// <returns></returns>
        protected abstract string GetItemFieldName();

        /// <summary>
        /// Restituisce il nome del campo id del gruppo
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
    }
}