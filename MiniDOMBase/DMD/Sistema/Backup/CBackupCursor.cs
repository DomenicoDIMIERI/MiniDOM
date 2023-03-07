using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Cursore sulla tabella dei backup
        /// </summary>
        [Serializable]
        public class CBackupCursor 
            : minidom.Databases.DBObjectCursor<CBackup>
        {
            private DBCursorStringField m_Name = new DBCursorStringField("Name");
            private DBCursorStringField m_FileName = new DBCursorStringField("FileName");
            private DBCursorField<DateTime> m_FileDate = new DBCursorField<DateTime>("FileDate");
            private DBCursorField<long> m_FileSize = new DBCursorField<long>("FileSize");
            private DBCursorStringField m_LogMessages = new DBCursorStringField("LogMessages");
            private DBCursorField<CompressionLevels> m_CompressionLevel = new DBCursorField<CompressionLevels>("CompressionLevel");
            private DBCursorField<CompressionMethods> m_CompressionMethod = new DBCursorField<CompressionMethods>("CompressionMethod");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CBackupCursor()
            {
            }

            /// <summary>
            /// CompressionLevel
            /// </summary>
            public DBCursorField<CompressionLevels> CompressionLevel
            {
                get
                {
                    return m_CompressionLevel;
                }
            }

            /// <summary>
            /// CompressionMethod
            /// </summary>
            public DBCursorField<CompressionMethods> CompressionMethod
            {
                get
                {
                    return m_CompressionMethod;
                }
            }

            /// <summary>
            /// LogMessages
            /// </summary>
            public DBCursorStringField LogMessages
            {
                get
                {
                    return m_LogMessages;
                }
            }

            /// <summary>
            /// Name
            /// </summary>
            public DBCursorStringField Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// FileName
            /// </summary>
            public DBCursorStringField FileName
            {
                get
                {
                    return m_FileName;
                }
            }

            /// <summary>
            /// FileDate
            /// </summary>
            public DBCursorField<DateTime> FileDate
            {
                get
                {
                    return m_FileDate;
                }
            }

            /// <summary>
            /// FileSize
            /// </summary>
            public DBCursorField<long> FileSize
            {
                get
                {
                    return m_FileSize;
                }
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Backups; //.Module;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_Backups";
            //}
        }
    }
}
