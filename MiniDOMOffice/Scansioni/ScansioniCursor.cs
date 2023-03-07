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
        /// Cursore di <see cref="Scansione"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class ScansioneCursor 
            : minidom.Databases.DBObjectCursorPO<Scansione>
        {
            private DBCursorStringField m_NomeDispositivo = new DBCursorStringField("NomeDispositivo");
            private DBCursorStringField m_NomeDocumento = new DBCursorStringField("NomeDocumento");
            private DBCursorStringField m_MetodoRicezione = new DBCursorStringField("MetodoRicezione");
            private DBCursorStringField m_ParametriScansione = new DBCursorStringField("ParametriScansione");
            private DBCursorField<DateTime> m_DataInvio = new DBCursorField<DateTime>("DataInvio");
            private DBCursorField<DateTime> m_DataRicezione = new DBCursorField<DateTime>("DataRicezione");
            private DBCursorField<DateTime> m_DataElaborazione = new DBCursorField<DateTime>("DataElaborazione");
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorField<int> m_IDAttachment = new DBCursorField<int>("IDAttachment");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<int> m_IDInviataDa = new DBCursorField<int>("IDInviataDa");
            private DBCursorStringField m_NomeInviataDa = new DBCursorStringField("NomeInviataDa");
            private DBCursorField<int> m_IDInviataA = new DBCursorField<int>("IDInviataA");
            private DBCursorStringField m_NomeInviataA = new DBCursorStringField("NomeInviataA");
            private DBCursorField<int> m_IDElaborataDa = new DBCursorField<int>("IDElaborataDa");
            private DBCursorStringField m_NomeElaborataDa = new DBCursorStringField("NomeElaborataDa");

            /// <summary>
            /// Costruttore
            /// </summary>
            public ScansioneCursor()
            {
            }

            /// <summary>
            /// NomeDispositivo
            /// </summary>
            public DBCursorStringField NomeDispositivo
            {
                get
                {
                    return m_NomeDispositivo;
                }
            }

            /// <summary>
            /// NomeDocumento
            /// </summary>
            public DBCursorStringField NomeDocumento
            {
                get
                {
                    return m_NomeDocumento;
                }
            }

            /// <summary>
            /// MetodoRicezione
            /// </summary>
            public DBCursorStringField MetodoRicezione
            {
                get
                {
                    return m_MetodoRicezione;
                }
            }

            /// <summary>
            /// ParametriScansione
            /// </summary>
            public DBCursorStringField ParametriScansione
            {
                get
                {
                    return m_ParametriScansione;
                }
            }

            /// <summary>
            /// DataInvio
            /// </summary>
            public DBCursorField<DateTime> DataInvio
            {
                get
                {
                    return m_DataInvio;
                }
            }

            /// <summary>
            /// DataRicezione
            /// </summary>
            public DBCursorField<DateTime> DataRicezione
            {
                get
                {
                    return m_DataRicezione;
                }
            }

            /// <summary>
            /// DataElaborazione
            /// </summary>
            public DBCursorField<DateTime> DataElaborazione
            {
                get
                {
                    return m_DataElaborazione;
                }
            }

            /// <summary>
            /// IDCliente
            /// </summary>
            public DBCursorField<int> IDCliente
            {
                get
                {
                    return m_IDCliente;
                }
            }

            /// <summary>
            /// NomeCliente
            /// </summary>
            public DBCursorStringField NomeCliente
            {
                get
                {
                    return m_NomeCliente;
                }
            }

            /// <summary>
            /// IDAttachment
            /// </summary>
            public DBCursorField<int> IDAttachment
            {
                get
                {
                    return m_IDAttachment;
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
            /// IDInviataDa
            /// </summary>
            public DBCursorField<int> IDInviataDa
            {
                get
                {
                    return m_IDInviataDa;
                }
            }

            /// <summary>
            /// NomeInviataDa
            /// </summary>
            public DBCursorStringField NomeInviataDa
            {
                get
                {
                    return m_NomeInviataDa;
                }
            }

            /// <summary>
            /// IDInviataA
            /// </summary>
            public DBCursorField<int> IDInviataA
            {
                get
                {
                    return m_IDInviataA;
                }
            }

            /// <summary>
            /// NomeInviataA
            /// </summary>
            public DBCursorStringField NomeInviataA
            {
                get
                {
                    return m_NomeInviataA;
                }
            }

            /// <summary>
            /// IDElaborataDa
            /// </summary>
            public DBCursorField<int> IDElaborataDa
            {
                get
                {
                    return m_IDElaborataDa;
                }
            }

            /// <summary>
            /// NomeElaborataDa
            /// </summary>
            public DBCursorStringField NomeElaborataDa
            {
                get
                {
                    return m_NomeElaborataDa;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Scansioni;
            }
             
        }
    }
}