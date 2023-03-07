using System;
using DMD;
using DMD.XML;
using DMD.Databases;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Anagrafica
    {


        /// <summary>
        /// Cursore sulla tabella dei contatti
        /// </summary>
        [Serializable]
        public class CContattoCursor 
            : minidom.Databases.DBObjectCursor<CContatto>
        {
            private DBCursorField<int> m_PersonaID = new DBCursorField<int>("Persona");
            private DBCursorStringField m_Tipo = new DBCursorStringField("Tipo");
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Valore = new DBCursorStringField("Valore");
            private DBCursorField<bool> m_Validated = new DBCursorField<bool>("Validated");
            private DBCursorField<int> m_ValidatoDaID = new DBCursorField<int>("ValidatoDa");
            private DBCursorField<DateTime> m_ValidatoIl = new DBCursorField<DateTime>("ValidatoIl");
            private DBCursorField<int> m_Ordine = new DBCursorField<int>("Ordine");
            private DBCursorField<ContattoFlags> m_Flags = new DBCursorField<ContattoFlags>("Flags");
            private DBCursorStringField m_Commento = new DBCursorStringField("Commento");
            private DBCursorStringField m_TipoFonte = new DBCursorStringField("TipoFonte");
            private DBCursorField<int> m_IDFonte = new DBCursorField<int>("IDFonte");
            private DBCursorField<StatoRecapito> m_StatoRecapito = new DBCursorField<StatoRecapito>("StatoRecapito");
            private DBCursorField<int> m_IDUfficio = new DBCursorField<int>("IDUfficio");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CContattoCursor()
            {
            }

            /// <summary>
            /// Campo IDUfficio
            /// </summary>
            public DBCursorField<int> IDUfficio
            {
                get
                {
                    return m_IDUfficio;
                }
            }

            /// <summary>
            /// Campo IDFonte
            /// </summary>
            public DBCursorField<int> IDFonte
            {
                get
                {
                    return m_IDFonte;
                }
            }

            /// <summary>
            /// Campo TipoFonte
            /// </summary>
            public DBCursorStringField TipoFonte
            {
                get
                {
                    return m_TipoFonte;
                }
            }

            /// <summary>
            /// Campo ValidatoDaID
            /// </summary>
            public DBCursorField<int> ValidatoDaID
            {
                get
                {
                    return m_ValidatoDaID;
                }
            }

            /// <summary>
            /// Campo ValidatoIl
            /// </summary>
            public DBCursorField<DateTime> ValidatoIl
            {
                get
                {
                    return m_ValidatoIl;
                }
            }

            /// <summary>
            /// Campo Flags
            /// </summary>
            public DBCursorField<ContattoFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// Campo PersonaID
            /// </summary>
            public DBCursorField<int> PersonaID
            {
                get
                {
                    return m_PersonaID;
                }
            }

            /// <summary>
            /// Campo Nome
            /// </summary>
            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            /// <summary>
            /// Campo Tipo
            /// </summary>
            public DBCursorStringField Tipo
            {
                get
                {
                    return m_Tipo;
                }
            }

            /// <summary>
            /// Campo Valore
            /// </summary>
            public DBCursorStringField Valore
            {
                get
                {
                    return m_Valore;
                }
            }

            /// <summary>
            /// Campo Commento
            /// </summary>
            public DBCursorStringField Commento
            {
                get
                {
                    return m_Commento;
                }
            }

            /// <summary>
            /// Campo Ordine
            /// </summary>
            public DBCursorField<int> Ordine
            {
                get
                {
                    return m_Ordine;
                }
            }

            /// <summary>
            /// Campo Validated
            /// </summary>
            public DBCursorField<bool> Validated
            {
                get
                {
                    return m_Validated;
                }
            }

            /// <summary>
            /// Campo StatoRecapito
            /// </summary>
            public DBCursorField<StatoRecapito> StatoRecapito
            {
                get
                {
                    return m_StatoRecapito;
                }
            }

            //public override string GetTableName()
            //{
            //    return "tbl_Contatti";
            //}

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}

            //protected override Sistema.CModule GetModule()
            //{
            //    return null;
            //}
        }
    }
}