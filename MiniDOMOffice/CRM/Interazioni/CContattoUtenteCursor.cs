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
using static minidom.CustomerCalls;


namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Cursore sulla tabella delle telefonate
        /// </summary>
        /// <remarks></remarks>
        public class CContattoUtenteCursor 
            : minidom.Databases.DBObjectCursorPO<CContattoUtente>
        {
            private DBCursorField<int> m_IDAzienda = new DBCursorField<int>("IDAzienda");
            private DBCursorStringField m_ClassName = new DBCursorStringField("ClassName");  // [TEXT] Nome del tipo di contatto ricercato
            private DBCursorField<int> m_IDPersona = new DBCursorField<int>("IDPersona");  // [int] ID della persona associata
            private DBCursorStringField m_NomePersona = new DBCursorStringField("NomePersona"); // [Text] Nome della persona
            private DBCursorField<int> m_IDContesto = new DBCursorField<int>("IDContesto"); // [int]  ID del contesto
            private DBCursorStringField m_TipoContesto = new DBCursorStringField("TipoContesto"); // [Text] Tipo del contesto
            private DBCursorField<bool> m_Ricevuta = new DBCursorField<bool>("Ricevuta"); // [Bool] Se vero indica che si tratta di una chiamata ricevuta. Se falso si tratta di una chiamata effettuata
            private DBCursorStringField m_Scopo = new DBCursorStringField("Scopo"); // [Text] Scopo della chiamata
                                                                                                                // Private m_Data As New DBCursorField(Of Date)("Data") '[Date] Data e ora della chiamata
            private DBCursorField<DateTime> m_Data = new DBCursorField<DateTime>("DataStr"); // [Date] Data e ora della chiamata
            private DBCursorField<int> m_IDOperatore = new DBCursorField<int>("IDOperatore"); // [Int]  ID dell'operatore che ha effettuato la chiamata o che ha risposto
            private DBCursorStringField m_NomeOperatore = new DBCursorStringField("NomeOperatore"); // [text] Nome dell'operatore
            private DBCursorStringField m_Note = new DBCursorStringField("Note"); // [text] Campo generico
            private DBCursorField<EsitoChiamata> m_Esito = new DBCursorField<EsitoChiamata>("Esito");
            private DBCursorStringField m_DettaglioEsito = new DBCursorStringField("DettaglioEsito");
            private DBCursorStringField m_MessageID = new DBCursorStringField("MessageID");
            private DBCursorStringField m_NumeroOIndirizzo = new DBCursorStringField("Numero");
            private DBCursorStringField m_NomeIndirizzo = new DBCursorStringField("Indirizzo_Nome");
            private DBCursorField<float> m_Durata = new DBCursorField<float>("Durata");
            private DBCursorField<float> m_Attesa = new DBCursorField<float>("Attesa");
            private DBCursorField<StatoConversazione> m_StatoConversazione = new DBCursorField<StatoConversazione>("StatoConversazione");
            private DBCursorField<int> m_IDAccoltoDa = new DBCursorField<int>("IDAccoltoDa");
            private DBCursorStringField m_NomeAccoltoDa = new DBCursorStringField("NomeAccoltoDa");
            private DBCursorField<DateTime> m_DataRicezione = new DBCursorField<DateTime>("DataRicezione");
            private DBCursorField<int> m_AttachmentID = new DBCursorField<int>("IDAttachment");
            private DBCursorField<int> m_IDPerContoDi = new DBCursorField<int>("IDPerContoDi");
            private DBCursorStringField m_NomePerContoDi = new DBCursorStringField("NomePerContoDi");
            private DBCursorField<decimal> m_Costo = new DBCursorField<decimal>("Costo");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CContattoUtenteCursor()
            {
            }

            /// <summary>
            /// Costo
            /// </summary>
            public DBCursorField<decimal> Costo
            {
                get
                {
                    return m_Costo;
                }
            }

            /// <summary>
            /// IDPerContoDi
            /// </summary>
            public DBCursorField<int> IDPerContoDi
            {
                get
                {
                    return m_IDPerContoDi;
                }
            }

            /// <summary>
            /// NomePerContoDi
            /// </summary>
            public DBCursorStringField NomePerContoDi
            {
                get
                {
                    return m_NomePerContoDi;
                }
            }

            /// <summary>
            /// Istanzia un nuovo contatto utente
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CContattoUtente InstantiateNewT(DBReader dbRis)
            {
                if (dbRis is null)
                    return new CTelefonata();
                var className = DBUtils.Read(dbRis, "ClassName", "");
                return (CContattoUtente)minidom.Sistema.ApplicationContext.CreateInstance(className);
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Telefonate";
            }

            //public DBCursorField<int> AttachmentID
            //{
            //    get
            //    {
            //        return m_AttachmentID;
            //    }
            //}

            /// <summary>
            /// ClassName
            /// </summary>
            public DBCursorStringField ClassName
            {
                get
                {
                    return m_ClassName;
                }
            }

            /// <summary>
            /// IDAccoltoDa
            /// </summary>
            public DBCursorField<int> IDAccoltoDa
            {
                get
                {
                    return m_IDAccoltoDa;
                }
            }

            /// <summary>
            /// NomeAccoltoDa
            /// </summary>
            public DBCursorStringField NomeAccoltoDa
            {
                get
                {
                    return m_NomeAccoltoDa;
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
            /// StatoConversazione
            /// </summary>
            public DBCursorField<StatoConversazione> StatoConversazione
            {
                get
                {
                    return m_StatoConversazione;
                }
            }

            /// <summary>
            /// IDAzienda
            /// </summary>
            public DBCursorField<int> IDAzienda
            {
                get
                {
                    return m_IDAzienda;
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
            /// Contesto
            /// </summary>
            public DBCursorStringField Contesto
            {
                get
                {
                    return m_TipoContesto;
                }
            }

            /// <summary>
            /// Durata
            /// </summary>
            public DBCursorField<float> Durata
            {
                get
                {
                    return m_Durata;
                }
            }

            /// <summary>
            /// Attesa
            /// </summary>
            public DBCursorField<float> Attesa
            {
                get
                {
                    return m_Attesa;
                }
            }

            /// <summary>
            /// Ricevuta
            /// </summary>
            public DBCursorField<bool> Ricevuta
            {
                get
                {
                    return m_Ricevuta;
                }
            }

            /// <summary>
            /// Scopo
            /// </summary>
            public DBCursorStringField Scopo
            {
                get
                {
                    return m_Scopo;
                }
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

            // Public ReadOnly Property Data As DBCursorField(Of Date)
            // Get
            // Return Me.m_Data
            // End Get
            // End Property

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
            /// Esito
            /// </summary>
            public DBCursorField<EsitoChiamata> Esito
            {
                get
                {
                    return m_Esito;
                }
            }

            /// <summary>
            /// DettaglioEsito
            /// </summary>
            public DBCursorStringField DettaglioEsito
            {
                get
                {
                    return m_DettaglioEsito;
                }
            }

            /// <summary>
            /// MessageID
            /// </summary>
            public DBCursorStringField MessageID
            {
                get
                {
                    return m_MessageID;
                }
            }

            /// <summary>
            /// NumeroOIndirizzo
            /// </summary>
            public DBCursorStringField NumeroOIndirizzo
            {
                get
                {
                    return m_NumeroOIndirizzo;
                }
            }

            /// <summary>
            /// NomeIndirizzo
            /// </summary>
            public DBCursorStringField NomeIndirizzo
            {
                get
                {
                    return m_NomeIndirizzo;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected sealed override CModulesClass GetModule()
            {
                return this.GetContattoRepository();
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public virtual CContattiRepository GetContattoRepository()
            {
                return minidom.CustomerCalls.CRM;
            }

            // Protected Overrides Function GetWhereFields() As CKeyCollection(Of DBCursorField)
            // Dim ret As CKeyCollection(Of DBCursorField) = MyBase.GetWhereFields()
            // ret.RemoveByKey("Note")
            // Return ret
            // End Function

            // Public Overrides Function GetWherePart() As String
            // Dim ret As String = MyBase.GetWherePart
            // If (ret = "(0<>0)") OrElse (ret = "0<>0") Then Return ret

            // If (Me.m_Note.IsSet) Then
            // Dim res As CCollection(Of internals.CIndexingService.CResult) = CustomerCalls.CRM.Index.Find(Me.m_Note.Value, -1)
            // Dim ids() As Integer = {}
            // For Each r As internals.CIndexingService.CResult In res
            // ids = Arrays.InsertSorted(ids, r.OwnerID)
            // Next
            // If (ids.Length = 0) Then Return "(0<>0)"
            // Dim buffer As New System.Text.StringBuilder
            // For Each id As Integer In ids
            // If (buffer.Length > 0) Then buffer.Append(",")
            // buffer.Append(id)
            // Next

            // Return Strings.Combine(ret, "[ID] In (" & buffer.ToString & ")", " AND ")
            // Else
            // Return ret
            // End If
            // End Function


        }
    }
}