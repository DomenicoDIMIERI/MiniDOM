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
        /// Cursore di oggetti <see cref="CRMStatistichePersona"/>
        /// </summary>
        [Serializable]
        public class CRMStatistichePersonaCursor
            : minidom.Databases.DBObjectCursorBase<CRMStatistichePersona>
        {
            private DBCursorField<int> m_IDPersona = new DBCursorField<int>("IDPersona");
            private DBCursorField<int> m_IDUltimaVisita = new DBCursorField<int>("IDUltimaVisita");
            private DBCursorField<DateTime> m_DataUltimaVisita = new DBCursorField<DateTime>("UltimaVisita");
            private DBCursorField<int> m_UltimoContattoNoID = new DBCursorField<int>("IDTelefonata");
            private DBCursorField<int> m_UltimoContattoOkID = new DBCursorField<int>("IDTelefonataOk");
            private DBCursorField<DateTime> m_DataUltimoContattoNo = new DBCursorField<DateTime>("DataUltimaTelefonata");
            private DBCursorField<DateTime> m_DataUltimoContattoOk = new DBCursorField<DateTime>("DataUltimaTelefonataOk");
            private DBCursorField<int> m_ConteggioRisp = new DBCursorField<int>("ConteggioRisp");
            private DBCursorField<int> m_ConteggioNoRispo = new DBCursorField<int>("ConteggioNoRispo");
            private DBCursorField<int> m_ConteggioTelefonate = new DBCursorField<int>("[ConteggioRisp] + [ConteggioNoRispo]");
            private DBCursorField<double> m_FrequenzaRisposta = new DBCursorField<double>("IIF([ConteggioRisp] + [ConteggioNoRispo]>0, [ConteggioRisp]/([ConteggioRisp] + [ConteggioNoRispo]), 0)");
            private DBCursorField<PersonFlags> m_Flags = new DBCursorField<PersonFlags>("Flags");
            private DBCursorStringField m_Note = new DBCursorStringField("Note");
            private DBCursorStringField m_NomePersona = new DBCursorStringField("NomePersona");
            private DBCursorField<int> m_IDPuntoOperativo = new DBCursorField<int>("IDPuntoOperativo");
            private DBCursorStringField m_MotivoProssimoRicontatto = new DBCursorStringField("MotivoProssimoRicontatto");
            private DBCursorField<DateTime> m_DataProssimoRicontatto = new DBCursorField<DateTime>("DataProssimoRicontatto");
            private DBCursorStringField m_IconURL = new DBCursorStringField("IconURL");
            // Private m_DataAggiornamento As New DBCursorField(Of Date)("DataAggiornamento")
            private DBCursorField<DateTime> m_DataAggiornamento = new DBCursorField<DateTime>("DataAggiornamento1");
            private DBCursorField<DateTime> m_DataUltimaOperazione = new DBCursorField<DateTime>("DataUltimaOperazione");
            private DBCursorField<DateTime> m_DataUltimoContatto = new DBCursorField<DateTime>("IIF([DataUltimaTelefonata]>[DataUltimaTelefonataOk],[DataUltimaTelefonata],[DataUltimaTelefonataOk])");
            private DBCursorField<int> m_Gravita = new DBCursorField<int>("Gravita");
            private DBCursorField<int> m_ConteggioCondizioni = new DBCursorField<int>("ContaCondizioni");
            private DBCursorStringField m_DettaglioEsito = new DBCursorStringField("DettaglioEsito");
            private DBCursorStringField m_DettaglioEsito1 = new DBCursorStringField("DettaglioEsito1");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRMStatistichePersonaCursor()
            {
            }

            /// <summary>
            /// ConteggioTelefonate
            /// </summary>
            public DBCursorField<int> ConteggioTelefonate
            {
                get
                {
                    return m_ConteggioTelefonate;
                }
            }

            /// <summary>
            /// FrequenzaRisposta
            /// </summary>
            public DBCursorField<double> FrequenzaRisposta
            {
                get
                {
                    return m_FrequenzaRisposta;
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
            /// DettaglioEsito1
            /// </summary>
            public DBCursorStringField DettaglioEsito1
            {
                get
                {
                    return m_DettaglioEsito1;
                }
            }

            /// <summary>
            /// IDUltimaVisita
            /// </summary>
            public DBCursorField<int> IDUltimaVisita
            {
                get
                {
                    return m_IDUltimaVisita;
                }
            }

            /// <summary>
            /// DataUltimaVisita
            /// </summary>
            public DBCursorField<DateTime> DataUltimaVisita
            {
                get
                {
                    return m_DataUltimaVisita;
                }
            }

            /// <summary>
            /// Gravita
            /// </summary>
            public DBCursorField<int> Gravita
            {
                get
                {
                    return m_Gravita;
                }
            }

            /// <summary>
            /// ConteggioCondizioni
            /// </summary>
            public DBCursorField<int> ConteggioCondizioni
            {
                get
                {
                    return m_ConteggioCondizioni;
                }
            }

            /// <summary>
            /// DataUltimaOperazione
            /// </summary>
            public DBCursorField<DateTime> DataUltimaOperazione
            {
                get
                {
                    return m_DataUltimaOperazione;
                }
            }

            /// <summary>
            /// DataUltimoContatto
            /// </summary>
            public DBCursorField<DateTime> DataUltimoContatto
            {
                get
                {
                    return m_DataUltimoContatto;
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
            /// MotivoProssimoRicontatto
            /// </summary>
            public DBCursorStringField MotivoProssimoRicontatto
            {
                get
                {
                    return m_MotivoProssimoRicontatto;
                }
            }

            /// <summary>
            /// DataProssimoRicontatto
            /// </summary>
            public DBCursorField<DateTime> DataProssimoRicontatto
            {
                get
                {
                    return m_DataProssimoRicontatto;
                }
            }

            // Public ReadOnly Property DataAggiornamento As DBCursorField(Of Date)
            // Get
            // Return Me.m_DataAggiornamento
            // End Get
            // End Property

            /// <summary>
            /// DataAggiornamento
            /// </summary>
            public DBCursorField<DateTime> DataAggiornamento
            {
                get
                {
                    return m_DataAggiornamento;
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
            /// UltimoContattoOkID
            /// </summary>
            public DBCursorField<int> UltimoContattoOkID
            {
                get
                {
                    return m_UltimoContattoOkID;
                }
            }

            /// <summary>
            /// UltimoContattoNoID
            /// </summary>
            public DBCursorField<int> UltimoContattoNoID
            {
                get
                {
                    return m_UltimoContattoNoID;
                }
            }

            /// <summary>
            /// DataUltimoContattoOk
            /// </summary>
            public DBCursorField<DateTime> DataUltimoContattoOk
            {
                get
                {
                    return m_DataUltimoContattoOk;
                }
            }

            /// <summary>
            /// DataUltimoContattoNo
            /// </summary>
            public DBCursorField<DateTime> DataUltimoContattoNo
            {
                get
                {
                    return m_DataUltimoContattoNo;
                }
            }

            /// <summary>
            /// ConteggioRisp
            /// </summary>
            public DBCursorField<int> ConteggioRisp
            {
                get
                {
                    return m_ConteggioRisp;
                }
            }

            /// <summary>
            /// ConteggioNoRispo
            /// </summary>
            public DBCursorField<int> ConteggioNoRispo
            {
                get
                {
                    return m_ConteggioNoRispo;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<PersonFlags> Flags
            {
                get
                {
                    return m_Flags;
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
            /// IDPuntoOperativo
            /// </summary>
            public DBCursorField<int> IDPuntoOperativo
            {
                get
                {
                    return m_IDPuntoOperativo;
                }
            }

          
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.CustomerCalls.Statistiche.StatistichePersona;
            }

            //public override string GetTableName()
            //{
            //    return "tbl_UltimaChiamata";
            //}

            // Protected Overrides Function GetWhereFields() As CKeyCollection(Of DBCursorField)
            // Dim ret As CKeyCollection(Of DBCursorField) = MyBase.GetWhereFields()
            // Dim f As DBCursorField(Of Date) = ret.GetItemByKey("DataAggiornamento")
            // ret.Remove(f)
            // Dim field As New DBCursorFieldObj(Of String)("DataAggiornamento1")
            // ret.Add("DataAggiornamento1", field)
            // If f.IsSet Then
            // field.Value = DBUtils.ToDBDateStr(f.Value)
            // field.Value1 = DBUtils.ToDBDateStr(f.Value1)
            // field.SortOrder = f.SortOrder
            // field.IncludeNulls = f.IncludeNulls
            // field.Operator = f.Operator
            // End If
            // Return ret
            // End Function

            // Protected Overrides Function GetSortFields() As CKeyCollection(Of DBCursorField)
            // Dim ret As CKeyCollection(Of DBCursorField) = MyBase.GetWhereFields()
            // Dim f As DBCursorField(Of Date) = ret.GetItemByKey("DataAggiornamento")
            // ret.Remove(f)
            // Dim field As New DBCursorFieldObj(Of String)("DataAggiornamento1")
            // ret.Add("DataAggiornamento1", field)
            // If f.IsSet Then
            // field.SortOrder = f.SortOrder
            // End If
            // Return ret
            // End Function
 
        }
    }
}