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
using static minidom.Office;
namespace minidom.PBX
{

    /// <summary>
    /// Cursore di <see cref="DMDSIPConfig"/>
    /// </summary>
    [Serializable]
    public sealed class DMDSIPConfigCursor 
        : minidom.Databases.DBObjectCursorBase<DMDSIPConfig>
    {
        private DBCursorStringField m_IDPostazione = new DBCursorStringField("IDPostazione");
        private DBCursorStringField m_IDMacchina = new DBCursorStringField("IDMacchina");
        private DBCursorStringField m_IDUtente = new DBCursorStringField("IDUtente");
        private DBCursorField<bool> m_Attiva = new DBCursorField<bool>("Attiva");
        private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
        private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
        private DBCursorField<DMDSIPConfigFlags> m_Flags = new DBCursorField<DMDSIPConfigFlags>("Flags");
        private DBCursorStringField m_UploadServer = new DBCursorStringField("UploadServer");
        private DBCursorStringField m_NotifyServer = new DBCursorStringField("NotifyServer");
        private DBCursorStringField m_ServerName = new DBCursorStringField("ServerName");
        private DBCursorStringField m_UserName = new DBCursorStringField("UserName");
        private DBCursorField<int> m_RemoveLogAfterNDays = new DBCursorField<int>("RemoveLogAfterNDays");
        private DBCursorStringField m_MinVersion = new DBCursorStringField("MinVersion");
        private DBCursorStringField m_MaxVersion = new DBCursorStringField("MaxVersion");

        /// <summary>
        /// Costruttore
        /// </summary>
        public DMDSIPConfigCursor()
        {
        }

        /// <summary>
        /// MinVersion
        /// </summary>
        public DBCursorStringField MinVersion
        {
            get
            {
                return m_MinVersion;
            }
        }

        /// <summary>
        /// MaxVersion
        /// </summary>
        public DBCursorStringField MaxVersion
        {
            get
            {
                return m_MaxVersion;
            }
        }

        /// <summary>
        /// RemoveLogAfterNDays
        /// </summary>
        public DBCursorField<int> RemoveLogAfterNDays
        {
            get
            {
                return m_RemoveLogAfterNDays;
            }
        }

        /// <summary>
        /// ServerName
        /// </summary>
        public DBCursorStringField ServerName
        {
            get
            {
                return m_ServerName;
            }
        }

        /// <summary>
        /// UserName
        /// </summary>
        public DBCursorStringField UserName
        {
            get
            {
                return m_UserName;
            }
        }

        /// <summary>
        /// UploadServer
        /// </summary>
        public DBCursorStringField UploadServer
        {
            get
            {
                return m_UploadServer;
            }
        }

        /// <summary>
        /// NotifyServer
        /// </summary>
        public DBCursorStringField NotifyServer
        {
            get
            {
                return m_NotifyServer;
            }
        }

        /// <summary>
        /// IDPostazione
        /// </summary>
        public DBCursorStringField IDPostazione
        {
            get
            {
                return m_IDPostazione;
            }
        }

        /// <summary>
        /// IDMacchina
        /// </summary>
        public DBCursorStringField IDMacchina
        {
            get
            {
                return m_IDMacchina;
            }
        }

        /// <summary>
        /// IDUtente
        /// </summary>
        public DBCursorStringField IDUtente
        {
            get
            {
                return m_IDUtente;
            }
        }

        /// <summary>
        /// Attiva
        /// </summary>
        public DBCursorField<bool> Attiva
        {
            get
            {
                return m_Attiva;
            }
        }

        /// <summary>
        /// DataInizio
        /// </summary>
        public DBCursorField<DateTime> DataInizio
        {
            get
            {
                return m_DataInizio;
            }
        }

        /// <summary>
        /// DataFine
        /// </summary>
        public DBCursorField<DateTime> DataFine
        {
            get
            {
                return m_DataFine;
            }
        }

        /// <summary>
        /// Flags
        /// </summary>
        public DBCursorField<DMDSIPConfigFlags> Flags
        {
            get
            {
                return m_Flags;
            }
        }
 
        /// <summary>
        /// Repository
        /// </summary>
        /// <returns></returns>
        protected override CModulesClass GetModule()
        {
            return DMDSIPApp.Configs;
        }
         
    }
}