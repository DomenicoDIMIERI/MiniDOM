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
        /// Cursore sulla tabella delle notifiche
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class NotificaCursor 
            : minidom.Databases.DBObjectCursorPO<Notifica>
        {

            private DBCursorField<DateTime> m_Data = new DBCursorField<DateTime>("Data");
            //private DBCursorFieldDBDate m_Data = new DBCursorFieldDBDate("DataStr");
            private DBCursorStringField m_Context = new DBCursorStringField("Context");
            private DBCursorStringField m_SourceName = new DBCursorStringField("SourceName");
            private DBCursorField<int> m_SourceID = new DBCursorField<int>("SourceID");
            private DBCursorField<int> m_TargetID = new DBCursorField<int>("TargetID");
            private DBCursorStringField m_TargetName = new DBCursorStringField("TargetName");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<DateTime> m_DataConsegna = new DBCursorField<DateTime>("DataConsegna");
            private DBCursorField<DateTime> m_DataLettura = new DBCursorField<DateTime>("DataLettura");
            private DBCursorField<StatoNotifica> m_StatoNotifica = new DBCursorField<StatoNotifica>("StatoNotifica");
            private DBCursorStringField m_Categoria = new DBCursorStringField("Categoria");

            /// <summary>
            /// Costruttore
            /// </summary>
            public NotificaCursor()
            {
            }


            /// <summary>
            /// Data
            /// </summary>
            public DBCursorField<DateTime> Data
            {
                get
                {
                    return this.m_Data;
                }
            }

            /// <summary>
            /// Categoria
            /// </summary>
            public DBCursorStringField Categoria
            {
                get
                {
                    return m_Categoria;
                }
            }

            /// <summary>
            /// Context
            /// </summary>
            public DBCursorStringField Context
            {
                get
                {
                    return m_Context;
                }
            }

            /// <summary>
            /// SourceName
            /// </summary>
            public DBCursorStringField SourceName
            {
                get
                {
                    return m_SourceName;
                }
            }

            /// <summary>
            /// SourceID
            /// </summary>
            public DBCursorField<int> SourceID
            {
                get
                {
                    return m_SourceID;
                }
            }

            /// <summary>
            /// TargetID
            /// </summary>
            public DBCursorField<int> TargetID
            {
                get
                {
                    return m_TargetID;
                }
            }

            /// <summary>
            /// TargetName
            /// </summary>
            public DBCursorStringField TargetName
            {
                get
                {
                    return m_TargetName;
                }
            }

            /// <summary>
            /// Descrizione
            /// </summary>
            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            /// <summary>
            /// DataConsegna
            /// </summary>
            public DBCursorField<DateTime> DataConsegna
            {
                get
                {
                    return m_DataConsegna;
                }
            }

            /// <summary>
            /// DataLettura
            /// </summary>
            public DBCursorField<DateTime> DataLettura
            {
                get
                {
                    return m_DataLettura;
                }
            }

            /// <summary>
            /// StatoNotifica
            /// </summary>
            public DBCursorField<StatoNotifica> StatoNotifica
            {
                get
                {
                    return m_StatoNotifica;
                }
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return Notifiche.Database;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Notifiche;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_SYSNotify";
            //}
        }
    }
}