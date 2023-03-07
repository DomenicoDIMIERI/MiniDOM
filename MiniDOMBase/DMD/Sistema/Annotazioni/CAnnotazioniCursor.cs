using System;
using DMD;
using DMD.Databases;
using minidom.repositories;
using static minidom.Sistema;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Cursore sulla tabella delle annotazioni
        /// </summary>
        [Serializable]
        public class CAnnotazioniCursor
            : minidom.Databases.DBObjectCursor<CAnnotazione>
        {
            private DBCursorField<int> m_OwnerID; // ID della persona associata
            private DBCursorStringField m_OwnerType;
            private DBCursorStringField m_Valore;
            private DBCursorField<int> m_IDContesto = new DBCursorField<int>("IDContesto");
            private DBCursorStringField m_TipoContesto = new DBCursorStringField("TipoContesto");
            private DBCursorStringField m_SourceName = new DBCursorStringField("SourceName");
            private DBCursorStringField m_SourceParam = new DBCursorStringField("SourceParam");
            // Private m_IDProduttore As New DBCursorField(Of Integer)("IDProduttore")


            /// <summary>
            /// Costruttore
            /// </summary>
            public CAnnotazioniCursor()
            {
                m_OwnerID = new DBCursorField<int>("OwnerID");
                m_OwnerType = new DBCursorStringField("OwnerType");
                m_Valore = new DBCursorStringField("Valore");
            }


            // Public ReadOnly Property IDProduttore As DBCursorField(Of Integer)
            // Get
            // Return Me.m_IDProduttore
            // End Get
            // End Property

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
            /// IDContesto
            /// </summary>
            public DBCursorField<int> IDContesto
            {
                get
                {
                    return m_IDContesto;
                }
            }

            /// <summary>
            /// TipoContesto
            /// </summary>
            public DBCursorStringField TipoContesto
            {
                get
                {
                    return m_TipoContesto;
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
            /// SourceParam
            /// </summary>
            public DBCursorStringField SourceParam
            {
                get
                {
                    return m_SourceParam;
                }
            }

            //public override object InstantiateNew(DBReader dbRis)
            //{
            //    var ret = new CAnnotazione();
            //    // ret.Produttore = Anagrafica.Aziende.AziendaPrincipale 
            //    return ret;
            //}

            //public override string GetTableName()
            //{
            //    return "tbl_Annotazioni";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Annotazioni;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return Annotazioni.Database;
            //}
        }
    }
}