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
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella degli indirizzi
        /// </summary>
        [Serializable]
        public class CIndirizziCursor 
            : minidom.Databases.DBObjectCursor<CIndirizzo>
        {
            private DBCursorField<int> m_PersonaID; // ID della persona associata
            private DBCursorStringField m_Nome; // Nome dell'indirizzo
            private DBCursorStringField m_Citta; // Nome della città
            private DBCursorStringField m_Provincia; // Nome della provincia
            private DBCursorStringField m_CAP; // Codice di avviamento postale
            private DBCursorStringField m_Toponimo;
            private DBCursorStringField m_Via; // Via o piazza
            private DBCursorStringField m_Civico; // Numero civico
            private DBCursorStringField m_Note; // Note
            private DBCursorField<int> m_Flags;  
            private DBCursorField<double> m_Lat;
            private DBCursorField<double> m_Lng;
            private DBCursorField<double> m_Alt;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CIndirizziCursor()
            {
                m_PersonaID = new DBCursorField<int>("Persona");
                m_Nome = new DBCursorStringField("Nome");
                m_Citta = new DBCursorStringField("Citta");
                m_Provincia = new DBCursorStringField("Provincia");
                m_CAP = new DBCursorStringField("CAP");
                m_Toponimo = new DBCursorStringField("Toponimo");
                m_Via = new DBCursorStringField("Via");
                m_Civico = new DBCursorStringField("Civico");
                m_Note = new DBCursorStringField("Note");
                m_Flags = new DBCursorField<int>("Flags");
                m_Lat = new DBCursorField<double>("Lat");
                m_Lng = new DBCursorField<double>("Lng");
                m_Alt = new DBCursorField<double>("Alt");
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<int> Flags
            {
                get
                {
                    return this.m_Flags;
                }
            }

            /// <summary>
            /// Lat
            /// </summary>
            public DBCursorField<double> Lat
            {
                get
                {
                    return this.m_Lat;
                }
            }

            /// <summary>
            /// Lng
            /// </summary>
            public DBCursorField<double> Lng
            {
                get
                {
                    return this.m_Lng;
                }
            }

            /// <summary>
            /// Alt
            /// </summary>
            public DBCursorField<double> Alt
            {
                get
                {
                    return this.m_Alt;
                }
            }

            /// <summary>
            /// PersonaID
            /// </summary>
            public DBCursorField<int> PersonaID
            {
                get
                {
                    return m_PersonaID;
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
            /// Toponimo
            /// </summary>
            public DBCursorStringField Toponimo
            {
                get
                {
                    return m_Toponimo;
                }
            }

            /// <summary>
            /// Citta
            /// </summary>
            public DBCursorStringField Citta
            {
                get
                {
                    return m_Citta;
                }
            }

            /// <summary>
            /// Provincia
            /// </summary>
            public DBCursorStringField Provincia
            {
                get
                {
                    return m_Provincia;
                }
            }

            /// <summary>
            /// CAP
            /// </summary>
            public DBCursorStringField CAP
            {
                get
                {
                    return m_CAP;
                }
            }

            /// <summary>
            /// Via
            /// </summary>
            public DBCursorStringField Via
            {
                get
                {
                    return m_Via;
                }
            }

            /// <summary>
            /// Civico
            /// </summary>
            public DBCursorStringField Civico
            {
                get
                {
                    return m_Civico;
                }
            }

            /// <summary>
            /// Note
            /// </summary>
            public DBCursorStringField Note
            {
                get
                {
                    return m_Note;
                }
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CIndirizzo InstantiateNewT(DBReader dbRis)
            {
                return new CIndirizzo();
            }

            //public override string GetTableName()
            //{
            //    return "tbl_Indirizzi";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Luoghi.Indirizzi; //.Module;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}
        }
    }
}