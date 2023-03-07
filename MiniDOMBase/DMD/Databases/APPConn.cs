using System;

namespace minidom
{
    public partial class Databases
    {
        private static CDBConnection m_AppConn;
        private static CDBConnection m_LOGConn;

        /// <summary>
        /// Restituisce l'oggetto Connessione al DB principale dell'applicazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static CDBConnection APPConn
        {
            get
            {
                if (m_AppConn is null)
                {
                    m_AppConn = new COleDBConnection();
                    m_AppConn.ConnectionOpened += APPConnOpenHandler;
                }

                return m_AppConn;
            }

            set
            {
                m_AppConn = value;
                m_AppConn.ConnectionOpened += APPConnOpenHandler;
            }
        }

        /// <summary>
        /// Restituisce l'oggetto Connessione al DB dei log dell'applicazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static CDBConnection LOGConn
        {
            get
            {
                if (m_LOGConn is null)
                {
                    m_LOGConn = new COleDBConnection();
                    m_LOGConn.ConnectionOpened += LogConnOpenHandler;
                }

                return m_LOGConn;
            }

            set
            {
                m_LOGConn = value;
                m_LOGConn.ConnectionOpened += LogConnOpenHandler;
            }
        }

        private static void APPConnOpenHandler(object sender, EventArgs e)
        {
            Sistema.Initialize();
        }

        private static void LogConnOpenHandler(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Restituisce ver se l'oggetto riporta che il campo ha subito modifiche da quando è stato caricato
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsFieldChanged(object obj, string fieldName)
        {
            DBObjectBase tmp = (DBObjectBase)obj;
            return tmp.IsFieldChanged(fieldName);
        }
    }
}