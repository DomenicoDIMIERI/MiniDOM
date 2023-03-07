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
        /// Cursore sulla tabella degli intervalli CAP
        /// </summary>
        /// <remarks></remarks>
        public class CIntervalloCAPCursor 
            : minidom.Databases.DBObjectCursorBase<CIntervalloCAP>
        {
            private DBCursorField<int> m_Da = new DBCursorField<int>("Da");
            private DBCursorField<int> m_A = new DBCursorField<int>("A");
            private DBCursorField<int> m_IDComune = new DBCursorField<int>("IDComune");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CIntervalloCAPCursor()
            {
            }

            /// <summary>
            /// Da
            /// </summary>
            public DBCursorField<int> Da
            {
                get
                {
                    return m_Da;
                }
            }

            /// <summary>
            /// A
            /// </summary>
            public DBCursorField<int> A
            {
                get
                {
                    return m_A;
                }
            }

            /// <summary>
            /// IDComune
            /// </summary>
            public DBCursorField<int> IDComune
            {
                get
                {
                    return m_IDComune;
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
                return minidom.Anagrafica.Luoghi.Comuni.IntervalliCAP; // Comuni.Module;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_LuoghiCAP";
            //}

            //protected internal override CDBConnection GetConnection()
            //{
            //    return Luoghi.Database;
            //}
        }
    }
}