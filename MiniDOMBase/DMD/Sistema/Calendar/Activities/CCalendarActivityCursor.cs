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
        /// Cursore sulle attività del calendario
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CCalendarActivityCursor
            : minidom.Databases.DBObjectCursor<CCalendarActivity>
        {
            private DBCursorStringField m_Categoria = new DBCursorStringField("Categoria");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<StatoAttivita> m_StatoAttivita = new DBCursorField<StatoAttivita>("StatoAttivita");
            private DBCursorField<int> m_IDOperatore = new DBCursorField<int>("Operatore");
            private DBCursorStringField m_NomeOperatore = new DBCursorStringField("NomeOperatore");
            private DBCursorField<int> m_IDAssegnatoA = new DBCursorField<int>("IDAssegnatoA");
            private DBCursorStringField m_NomeAssegnatoA = new DBCursorStringField("NomeAssegnatoA");
            private DBCursorStringField m_Note = new DBCursorStringField("Note");
            private DBCursorStringField m_Luogo = new DBCursorStringField("Luogo");
            private DBCursorField<int> m_Promemoria = new DBCursorField<int>("Promemoria");
            private DBCursorField<int> m_Ripetizione = new DBCursorField<int>("Ripetizione");
            private DBCursorField<bool> m_GiornataIntera = new DBCursorField<bool>("GiornataIntera");
            private DBCursorField<CalendarActivityFlags> m_Flags = new DBCursorField<CalendarActivityFlags>("Flags");
            private DBCursorField<int> m_IDPersona = new DBCursorField<int>("IDPersona");
            private DBCursorStringField m_NomePersona = new DBCursorStringField("NomePersona");
            private DBCursorStringField m_ProviderName = new DBCursorStringField("ProviderName");
            private DBCursorField<int> m_Priorita = new DBCursorField<int>("Priorita");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCalendarActivityCursor()
            {
            }

            /// <summary>
            /// Priorita
            /// </summary>
            public DBCursorField<int> Priorita
            {
                get
                {
                    return m_Priorita;
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
            /// StatoAttivita
            /// </summary>
            public DBCursorField<StatoAttivita> StatoAttivita
            {
                get
                {
                    return m_StatoAttivita;
                }
            }

            /// <summary>
            /// IDOperatore
            /// </summary>
            public DBCursorField<int> IDOperatore
            {
                get
                {
                    return m_IDOperatore;
                }
            }

            /// <summary>
            /// NomeOperatore
            /// </summary>
            public DBCursorStringField NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }
            }

            /// <summary>
            /// IDAssegnatoA
            /// </summary>
            public DBCursorField<int> IDAssegnatoA
            {
                get
                {
                    return m_IDAssegnatoA;
                }
            }

            /// <summary>
            /// NomeAssegnatoA
            /// </summary>
            public DBCursorStringField NomeAssegnatoA
            {
                get
                {
                    return m_NomeAssegnatoA;
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
            /// Luogo
            /// </summary>
            public DBCursorStringField Luogo
            {
                get
                {
                    return m_Luogo;
                }
            }

            /// <summary>
            /// Promemoria
            /// </summary>
            public DBCursorField<int> Promemoria
            {
                get
                {
                    return m_Promemoria;
                }
            }

            /// <summary>
            /// Ripetizione
            /// </summary>
            public DBCursorField<int> Ripetizione
            {
                get
                {
                    return m_Ripetizione;
                }
            }

            /// <summary>
            /// GiornataIntera
            /// </summary>
            public DBCursorField<bool> GiornataIntera
            {
                get
                {
                    return m_GiornataIntera;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<CalendarActivityFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// IDPersona
            /// </summary>
            public DBCursorField<int> IDPersona
            {
                get
                {
                    return m_IDPersona;
                }
            }

            /// <summary>
            /// NomePersona
            /// </summary>
            public DBCursorStringField NomePersona
            {
                get
                {
                    return m_NomePersona;
                }
            }

            /// <summary>
            /// ProviderName
            /// </summary>
            public DBCursorStringField ProviderName
            {
                get
                {
                    return m_ProviderName;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Calendar; //.Module;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_CalendarActivities";
            //}

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}
        }
    }
}