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
        /// Cursore di <see cref="EstrattoContributivo"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class EstrattiContributiviCursor 
            : minidom.Databases.DBObjectCursorPO<EstrattoContributivo>
        {
            private DBCursorField<int> m_IDRichiedente = new DBCursorField<int>("IDRichiedente");
            private DBCursorStringField m_NomeRichiedente = new DBCursorStringField("NomeRichiedente");
            private DBCursorField<DateTime> m_DataRichiesta = new DBCursorField<DateTime>("DataRichiesta");
            private DBCursorField<int> m_IDAssegnatoA = new DBCursorField<int>("IDAssegnatoA");
            private DBCursorStringField m_NomeAssegnatoA = new DBCursorStringField("NomeAssegnatoA");
            private DBCursorField<DateTime> m_DataAssegnazione = new DBCursorField<DateTime>("DataAssegnazione");
            private DBCursorField<StatoEstrattoContributivo> m_StatoRichiesta = new DBCursorField<StatoEstrattoContributivo>("StatoRichiesta");
            private DBCursorField<DateTime> m_DataCompletamento = new DBCursorField<DateTime>("DataCompletamento");
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorField<int> m_IDAmministrazione = new DBCursorField<int>("IDAmministrazione");
            private DBCursorStringField m_NomeAmministrazione = new DBCursorStringField("NomeAmministrazione");
            private DBCursorField<int> m_IDDelega = new DBCursorField<int>("IDDelega");
            private DBCursorField<int> m_IDDocumentoRiconoscimento = new DBCursorField<int>("IDDocRic");
            private DBCursorField<int> m_IDCodiceFiscale = new DBCursorField<int>("IDCF");
            private DBCursorStringField m_Note = new DBCursorStringField("Note");
            private DBCursorStringField m_SourceType = new DBCursorStringField("SourceType");
            private DBCursorField<int> m_SourceID = new DBCursorField<int>("SourceID");
            // Private m_IDAllegato As New DBCursorField(Of Integer)("IDAllegato")

            /// <summary>
            /// Costruttore
            /// </summary>
            public EstrattiContributiviCursor()
            {

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
            /// SourceID
            /// </summary>
            public DBCursorField<int> SourceID
            {
                get
                {
                    return m_SourceID;
                }
            }

            /// <summary>
            /// SourceType
            /// </summary>
            public DBCursorStringField SourceType
            {
                get
                {
                    return m_SourceType;
                }
            }

            /// <summary>
            /// IDRichiedente
            /// </summary>
            public DBCursorField<int> IDRichiedente
            {
                get
                {
                    return m_IDRichiedente;
                }
            }

            /// <summary>
            /// NomeRichiedente
            /// </summary>
            public DBCursorStringField NomeRichiedente
            {
                get
                {
                    return m_NomeRichiedente;
                }
            }

            /// <summary>
            /// DataRichiesta
            /// </summary>
            public DBCursorField<DateTime> DataRichiesta
            {
                get
                {
                    return m_DataRichiesta;
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
            /// DataAssegnazione
            /// </summary>
            public DBCursorField<DateTime> DataAssegnazione
            {
                get
                {
                    return m_DataAssegnazione;
                }
            }

            /// <summary>
            /// StatoRichiesta
            /// </summary>
            public DBCursorField<StatoEstrattoContributivo> StatoRichiesta
            {
                get
                {
                    return m_StatoRichiesta;
                }
            }

            /// <summary>
            /// DataCompletamento
            /// </summary>
            public DBCursorField<DateTime> DataCompletamento
            {
                get
                {
                    return m_DataCompletamento;
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
            /// IDAmministrazione
            /// </summary>
            public DBCursorField<int> IDAmministrazione
            {
                get
                {
                    return m_IDAmministrazione;
                }
            }

            /// <summary>
            /// NomeAmministrazione
            /// </summary>
            public DBCursorStringField NomeAmministrazione
            {
                get
                {
                    return m_NomeAmministrazione;
                }
            }

            /// <summary>
            /// IDDelega
            /// </summary>
            public DBCursorField<int> IDDelega
            {
                get
                {
                    return m_IDDelega;
                }
            }

            /// <summary>
            /// IDDocumentoRiconoscimento
            /// </summary>
            public DBCursorField<int> IDDocumentoRiconoscimento
            {
                get
                {
                    return m_IDDocumentoRiconoscimento;
                }
            }

            /// <summary>
            /// IDCodiceFiscale
            /// </summary>
            public DBCursorField<int> IDCodiceFiscale
            {
                get
                {
                    return m_IDCodiceFiscale;
                }
            }

            // Public ReadOnly Property IDAllegato As DBCursorField(Of Integer)
            // Get
            // Return Me.m_IDAllegato
            // End Get
            // End Property

            /// <summary>
            /// Controlla i limiti di accesso
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldBase GetWherePartLimit()
            {
                var ret = base.GetWherePartLimit();
                // If (ret <> vbNullString) AndAlso Me.Module.UserCanDoAction("list_assigned") Then
                // Dim wherePart As String = "[IDOperatore] = " & GetID(Users.CurrentUser)
                // ret = Strings.Combine(ret, wherePart, " OR ")
                // End If
                return ret;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.EstrattiContributivi;
            }

            /// <summary>
            /// Inizializza il nuovo oggetto
            /// </summary>
            /// <param name="item"></param>
            protected override void OnInitialize(EstrattoContributivo item)
            {
                base.OnInitialize(item);

                item.Richiedente = Sistema.Users.CurrentUser;
                // ret.OraUscita = Now
            }

        }
    }
}