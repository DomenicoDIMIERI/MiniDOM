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
    /// Curesore di oggetti di tipo <see cref="Chiamata"/>
    /// </summary>
    [Serializable]
    public sealed class ChiamateCursor 
        : minidom.Databases.DBObjectCursorBase<Chiamata>
    {
        private DBCursorStringField m_ServerIP = new DBCursorStringField("ServerIP");
        private DBCursorStringField m_ServerName = new DBCursorStringField("ServerName");
        private DBCursorStringField m_Channel = new DBCursorStringField("Channel");
        private DBCursorStringField m_SourceNumber = new DBCursorStringField("SourceNumber");
        private DBCursorStringField m_TargetNumber = new DBCursorStringField("TargetNumber");
        private DBCursorField<DateTime> m_StartTime = new DBCursorField<DateTime>("StartTime");
        private DBCursorField<DateTime> m_AnswerTime = new DBCursorField<DateTime>("AnswerTime");
        private DBCursorField<DateTime> m_EndTime = new DBCursorField<DateTime>("EndTime");
        private DBCursorField<StatoChiamata> m_StatoChiamata = new DBCursorField<StatoChiamata>("StatoChiamata");
        private DBCursorField<int> m_Direzione = new DBCursorField<int>("Direzione");
        private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
        private DBCursorField<int> m_IDTelefonata = new DBCursorField<int>("IDTelefonata");

        /// <summary>
        /// Costruttore
        /// </summary>
        public ChiamateCursor()
        {
        }

        /// <summary>
        /// IDTelefonata
        /// </summary>
        public DBCursorField<int> IDTelefonata
        {
            get
            {
                return m_IDTelefonata;
            }
        }

        /// <summary>
        /// ServerIP
        /// </summary>
        public DBCursorStringField ServerIP
        {
            get
            {
                return m_ServerIP;
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
        /// Channel
        /// </summary>
        public DBCursorStringField Channel
        {
            get
            {
                return m_Channel;
            }
        }

        /// <summary>
        /// SourceNumber
        /// </summary>
        public DBCursorStringField SourceNumber
        {
            get
            {
                return m_SourceNumber;
            }
        }

        /// <summary>
        /// TargetNumber
        /// </summary>
        public DBCursorStringField TargetNumber
        {
            get
            {
                return m_TargetNumber;
            }
        }

        /// <summary>
        /// StartTime
        /// </summary>
        public DBCursorField<DateTime> StartTime
        {
            get
            {
                return m_StartTime;
            }
        }

        /// <summary>
        /// AnswerTime
        /// </summary>
        public DBCursorField<DateTime> AnswerTime
        {
            get
            {
                return m_AnswerTime;
            }
        }

        /// <summary>
        /// EndTime
        /// </summary>
        public DBCursorField<DateTime> EndTime
        {
            get
            {
                return m_EndTime;
            }
        }

        /// <summary>
        /// StatoChiamata
        /// </summary>
        public DBCursorField<StatoChiamata> StatoChiamata
        {
            get
            {
                return m_StatoChiamata;
            }
        }

        /// <summary>
        /// Direzione
        /// </summary>
        public DBCursorField<int> Direzione
        {
            get
            {
                return m_Direzione;
            }
        }

        /// <summary>
        /// Flags
        /// </summary>
        public DBCursorField<int> Flags
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
            return minidom.PBX.Chiamate.Chiamate;
        }
         
    }
}