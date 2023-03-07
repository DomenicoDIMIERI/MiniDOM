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
        /// Cursore sulla tabella dei setting su db
        /// </summary>
        [Serializable]
        public class CSettingRecordCursor
            : Databases.DBObjectCursorBase<CSettingRecord>
        {
            private DBCursorField<int> m_OwnerID = new DBCursorField<int>("OwnerID");
            private DBCursorStringField m_OwnerType = new DBCursorStringField("OwnerType");
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Valore = new DBCursorStringField("Valore");
            private DBCursorField<TypeCode> m_TipoValore = new DBCursorField<TypeCode>("TipoValore");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CSettingRecordCursor()
            {
            }

            /// <summary>
            /// OwnerID
            /// </summary>
            public DBCursorField<int> OwnerID
            {
                get
                {
                    return m_OwnerID;
                }
            }

            /// <summary>
            /// OwnerType
            /// </summary>
            public DBCursorStringField OwnerType
            {
                get
                {
                    return m_OwnerType;
                }
            }

            /// <summary>
            /// Nome
            /// </summary>
            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            /// <summary>
            /// Valore
            /// </summary>
            public DBCursorStringField Valore
            {
                get
                {
                    return m_Valore;
                }
            }

            /// <summary>
            /// TipoValore
            /// </summary>
            public DBCursorField<TypeCode> TipoValore
            {
                get
                {
                    return m_TipoValore;
                }
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Settings;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_Settings";
            //}
        }
    }
}