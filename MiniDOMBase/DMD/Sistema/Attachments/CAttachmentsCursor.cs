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
        /// Cursore sulla tabella degli allegati
        /// </summary>
        [Serializable]
        public class CAttachmentsCursor 
            : minidom.Databases.DBObjectCursor<CAttachment>
        {
            private DBCursorField<int> m_OwnerID = new DBCursorField<int>("OwnerID");
            private DBCursorStringField m_OwnerType = new DBCursorStringField("OwnerType");
            private DBCursorStringField m_Tipo = new DBCursorStringField("Tipo"); // Tipo del documento allegato
            private DBCursorField<AttachmentStatus> m_StatoDocumento = new DBCursorField<AttachmentStatus>("StatoDocumento"); // Valore che indica lo stato del documento (0 NON VALIDATO, 1 VALIDATO, 2 NON LEGGIBILE, 3 NON VALIDO ...)         
            private DBCursorField<int> m_VerificatoDaID = new DBCursorField<int>("VerificatoDaID"); // ID dell'utente che ha verificato il documento
            private DBCursorStringField m_NomeVerificatoDa = new DBCursorStringField("NomeVerificatoDa"); // Nome dell'utente che ha verificato il documento
            private DBCursorField<DateTime> m_VerificatoIl = new DBCursorField<DateTime>("VerificatoIl"); // Data di verifica
            private DBCursorStringField m_Testo = new DBCursorStringField("Testo"); // Testo visualizzato
            private DBCursorStringField m_URL = new DBCursorStringField("URL");
            private DBCursorField<int> m_IDContesto = new DBCursorField<int>("IDContesto");
            private DBCursorStringField m_TipoContesto = new DBCursorStringField("TipoContesto");
            private DBCursorStringField m_Parametro = new DBCursorStringField("Parametro");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<int> m_IDDocumento = new DBCursorField<int>("IDDocumento");
            private DBCursorField<int> m_IDRilasciatoDa = new DBCursorField<int>("IDRilasciatoDa");
            private DBCursorStringField m_NomeRilasciatoDa = new DBCursorStringField("RilasciatoDa");
            // Private m_IDProduttore As New DBCursorField(Of Integer)("IDProduttore")
            private DBCursorStringField m_Categoria = new DBCursorStringField("Categoria");
            private DBCursorStringField m_SottoCategoria = new DBCursorStringField("SottoCategoria");
            private DBCursorField<AttachmentFlags> m_Flags = new DBCursorField<AttachmentFlags>("Flags");
            private DBCursorField<int> m_ParentID = new DBCursorField<int>("ParentID");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAttachmentsCursor()
            {
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<AttachmentFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// ParentID
            /// </summary>
            public DBCursorField<int> ParentID
            {
                get
                {
                    return m_ParentID;
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
            /// SottoCategoria
            /// </summary>
            public DBCursorStringField SottoCategoria
            {
                get
                {
                    return m_SottoCategoria;
                }
            }

            // Public ReadOnly Property IDProduttore As DBCursorField(Of Integer)
            // Get
            // Return Me.m_IDProduttore
            // End Get
            // End Property

            /// <summary>
            /// IDContesto
            /// </summary>
            public DBCursorField<int> IDContesto
            {
                get
                {
                    return m_IDContesto;
                }
            }

            /// <summary>
            /// TipoContesto
            /// </summary>
            public DBCursorStringField TipoContesto
            {
                get
                {
                    return m_TipoContesto;
                }
            }

            /// <summary>
            /// NomeRilasciatoDa
            /// </summary>
            public DBCursorStringField NomeRilasciatoDa
            {
                get
                {
                    return m_NomeRilasciatoDa;
                }
            }

            /// <summary>
            /// IDRilasciatoDa
            /// </summary>
            public DBCursorField<int> IDRilasciatoDa
            {
                get
                {
                    return m_IDRilasciatoDa;
                }
            }

            /// <summary>
            /// OwnerID
            /// </summary>
            public DBCursorField<int> OwnerID
            {
                get
                {
                    return m_OwnerID;
                }
            }

            /// <summary>
            /// OwnerType
            /// </summary>
            public DBCursorStringField OwnerType
            {
                get
                {
                    return m_OwnerType;
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
            /// StatoDocumento
            /// </summary>
            public DBCursorField<AttachmentStatus> StatoDocumento
            {
                get
                {
                    return m_StatoDocumento;
                }
            }

            /// <summary>
            /// VerificatoDaID
            /// </summary>
            public DBCursorField<int> VerificatoDaID
            {
                get
                {
                    return m_VerificatoDaID;
                }
            }

            /// <summary>
            /// NomeVerificatoDa
            /// </summary>
            public DBCursorStringField NomeVerificatoDa
            {
                get
                {
                    return m_NomeVerificatoDa;
                }
            }

            /// <summary>
            /// VerificatoIl
            /// </summary>
            public DBCursorField<DateTime> VerificatoIl
            {
                get
                {
                    return m_VerificatoIl;
                }
            }

            /// <summary>
            /// Testo
            /// </summary>
            public DBCursorStringField Testo
            {
                get
                {
                    return m_Testo;
                }
            }

            /// <summary>
            /// URL
            /// </summary>
            public DBCursorStringField URL
            {
                get
                {
                    return m_URL;
                }
            }

            /// <summary>
            /// Parametro
            /// </summary>
            public DBCursorStringField Parametro
            {
                get
                {
                    return m_Parametro;
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
            /// IDDocumento
            /// </summary>
            public DBCursorField<int> IDDocumento
            {
                get
                {
                    return m_IDDocumento;
                }
            }

            //public override string GetTableName()
            //{
            //    return "tbl_Attachments";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.Attachments; //.Module;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return Attachments.Database;
            //}
        }
    }
}