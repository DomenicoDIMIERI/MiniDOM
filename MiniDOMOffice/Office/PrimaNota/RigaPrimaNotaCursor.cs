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

namespace minidom
{
    public partial class Office
    {


        /// <summary>
        /// Cursore di <see cref="RigaPrimaNota"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class RigaPrimaNotaCursor 
            : minidom.Databases.DBObjectCursorPO<RigaPrimaNota>
        {
            private DBCursorField<DateTime> m_Data = new DBCursorField<DateTime>("Data");
            private DBCursorStringField m_DescrizioneMovimento = new DBCursorStringField("DescrizioneMovimento");
            private DBCursorField<decimal> m_Entrate = new DBCursorField<decimal>("Entrate");
            private DBCursorField<decimal> m_Uscite = new DBCursorField<decimal>("Uscite");

            /// <summary>
            /// Costruttore
            /// </summary>
            public RigaPrimaNotaCursor()
            {
            }

            /// <summary>
            /// Data
            /// </summary>
            public DBCursorField<DateTime> Data
            {
                get
                {
                    return m_Data;
                }
            }

            /// <summary>
            /// DescrizioneMovimento
            /// </summary>
            public DBCursorStringField DescrizioneMovimento
            {
                get
                {
                    return m_DescrizioneMovimento;
                }
            }

            /// <summary>
            /// Entrate
            /// </summary>
            public DBCursorField<decimal> Entrate
            {
                get
                {
                    return m_Entrate;
                }
            }

            /// <summary>
            /// Uscite
            /// </summary>
            public DBCursorField<decimal> Uscite
            {
                get
                {
                    return m_Uscite;
                }
            }
 
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.PrimaNota;
            }
             
        }
    }
}