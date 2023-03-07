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
    public partial class CustomerCalls
    {

        /// <summary>
        /// Cursore su oggetti di tipo <see cref="BlackListAddress"/>
        /// </summary>
        [Serializable]
        public class BlackListAddressCursor 
            : minidom.Databases.DBObjectCursorBase<BlackListAddress>
        {
            private DBCursorField<BlackListType> m_TipoRegola = new DBCursorField<BlackListType>("Tipo");
            private DBCursorStringField m_TipoContatto = new DBCursorStringField("TipoContatto");
            private DBCursorStringField m_ValoreContatto = new DBCursorStringField("Valore");
            private DBCursorField<DateTime> m_DataBlocco = new DBCursorField<DateTime>("DataBlocco");
            private DBCursorField<int> m_IDBloccatoDa = new DBCursorField<int>("IDBloccatoDa");
            private DBCursorStringField m_NomeBloccatoDa = new DBCursorStringField("NomeBloccatoDa");
            private DBCursorStringField m_MotivoBlocco = new DBCursorStringField("MotivoBlocco");

            /// <summary>
            /// Costruttore
            /// </summary>
            public BlackListAddressCursor()
            {
            }

            /// <summary>
            /// TipoRegola
            /// </summary>
            public DBCursorField<BlackListType> TipoRegola
            {
                get
                {
                    return m_TipoRegola;
                }
            }

            /// <summary>
            /// ValoreContatto
            /// </summary>
            public DBCursorStringField ValoreContatto
            {
                get
                {
                    return m_ValoreContatto;
                }
            }

            /// <summary>
            /// TipoContatto
            /// </summary>
            public DBCursorStringField TipoContatto
            {
                get
                {
                    return m_TipoContatto;
                }
            }

            /// <summary>
            /// DataBlocco
            /// </summary>
            public DBCursorField<DateTime> DataBlocco
            {
                get
                {
                    return m_DataBlocco;
                }
            }

            /// <summary>
            /// IDBloccatoDa
            /// </summary>
            public DBCursorField<int> IDBloccatoDa
            {
                get
                {
                    return m_IDBloccatoDa;
                }
            }

            /// <summary>
            /// NomeBloccatoDa
            /// </summary>
            public DBCursorStringField NomeBloccatoDa
            {
                get
                {
                    return m_NomeBloccatoDa;
                }
            }

            /// <summary>
            /// MotivoBlocco
            /// </summary>
            public DBCursorStringField MotivoBlocco
            {
                get
                {
                    return m_MotivoBlocco;
                }
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.CustomerCalls.BlackListAdresses;
            }

            
        }
    }
}