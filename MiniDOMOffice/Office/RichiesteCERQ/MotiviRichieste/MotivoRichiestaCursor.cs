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
        /// Repository di <see cref="MotivoRichiesta"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class MotivoRichiestaCursor 
            : minidom.Databases.DBObjectCursor<MotivoRichiesta>
        {
            private DBCursorStringField m_Motivo = new DBCursorStringField("Motivo");
            private DBCursorField<MotivoRichiestaFlags> m_Flags = new DBCursorField<MotivoRichiestaFlags>("Flags");
            private DBCursorStringField m_HandlerName = new DBCursorStringField("NomeHandler");

            /// <summary>
            /// Costruttore
            /// </summary>
            public MotivoRichiestaCursor()
            { 
            }

            /// <summary>
            /// Motivo
            /// </summary>
            public DBCursorStringField Motivo
            {
                get
                {
                    return m_Motivo;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<MotivoRichiestaFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// HandlerName
            /// </summary>
            public DBCursorStringField HandlerName
            {
                get
                {
                    return m_HandlerName;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.RichiesteCERQ.MotiviRichieste;
            }
             
        }
    }
}