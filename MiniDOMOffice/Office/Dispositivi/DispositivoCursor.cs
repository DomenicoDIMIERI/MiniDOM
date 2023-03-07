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
        /// Cursore dir <see cref="Dispositivo"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class DispositivoCursor 
            : minidom.Databases.DBObjectCursorPO<Dispositivo>
        {
            private DBCursorField<int> m_UserID = new DBCursorField<int>("UserID");
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Tipo = new DBCursorStringField("Tipo");
            private DBCursorStringField m_Modello = new DBCursorStringField("Modello");
            private DBCursorStringField m_Classe = new DBCursorStringField("Classe"); //TODO mappare in JS?
            private DBCursorStringField m_Seriale = new DBCursorStringField("Seriale");
            private DBCursorStringField m_IconURL = new DBCursorStringField("IconURL");
            private DBCursorField<DateTime> m_DataAcquisto = new DBCursorField<DateTime>("DataAcquisto");
            private DBCursorField<DateTime> m_DataDismissione = new DBCursorField<DateTime>("DataDismissione");
            private DBCursorField<StatoDispositivo> m_StatoDispositivo = new DBCursorField<StatoDispositivo>("StatoDispositivo");

            /// <summary>
            /// Costruttore
            /// </summary>
            public DispositivoCursor()
            {
            }

            /// <summary>
            /// UserID
            /// </summary>
            public DBCursorField<int> UserID
            {
                get
                {
                    return m_UserID;
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
            /// Classe
            /// </summary>
            public DBCursorStringField Classe
            {
                get
                {
                    return m_Classe;
                }
            }

            /// <summary>
            /// Tipo
            /// </summary>
            public DBCursorStringField Tipo
            {
                get
                {
                    return m_Tipo;
                }
            }

            /// <summary>
            /// Modello
            /// </summary>
            public DBCursorStringField Modello
            {
                get
                {
                    return m_Modello;
                }
            }

            /// <summary>
            /// Seriale
            /// </summary>
            public DBCursorStringField Seriale
            {
                get
                {
                    return m_Seriale;
                }
            }

            /// <summary>
            /// IconURL
            /// </summary>
            public DBCursorStringField IconURL
            {
                get
                {
                    return m_IconURL;
                }
            }

            /// <summary>
            /// DataAcquisto
            /// </summary>
            public DBCursorField<DateTime> DataAcquisto
            {
                get
                {
                    return m_DataAcquisto;
                }
            }

            /// <summary>
            /// DataDismissione
            /// </summary>
            public DBCursorField<DateTime> DataDismissione
            {
                get
                {
                    return m_DataDismissione;
                }
            }

            /// <summary>
            /// StatoDispositivo
            /// </summary>
            public DBCursorField<StatoDispositivo> StatoDispositivo
            {
                get
                {
                    return m_StatoDispositivo;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Dispositivi;
            }

           
        }
    }
}