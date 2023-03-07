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
    /// Cursore sulla tabella UserAllowNegateCursor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class UserAllowNegateCursor<T> 
        : minidom.Databases.DBObjectCursorBase<UserAllowNegate<T>>
    {
        private DBCursorField<int> m_ItemID = null;
        private DBCursorField<int> m_UserID = null;
        private DBCursorField<bool> m_Allow = new DBCursorField<bool>("Allow");
        private DBCursorField<bool> m_Negate = new DBCursorField<bool>("Negate");

        /// <summary>
        /// Costruttore
        /// </summary>
        public UserAllowNegateCursor()
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
        /// UserID
        /// </summary>
        public DBCursorField<int> UserID
        {
            get
            {
                if (m_UserID is null)
                    m_UserID = new DBCursorField<int>(GetUserFieldName());
                return m_UserID;
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
        /// Negate
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
        /// Restituisce il nome del campo id dell'utente
        /// </summary>
        /// <returns></returns>
        protected virtual string GetUserFieldName()
        {
            return "Utente";
        }

        /// <summary>
        /// Restituisce il nome della tabella
        /// </summary>
        /// <returns></returns>
        public abstract override string GetTableName();
    }
}