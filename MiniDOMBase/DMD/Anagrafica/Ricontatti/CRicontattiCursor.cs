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

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella degli appuntamenti
        /// </summary>
        [Serializable]
        public class CRicontattiCursor 
            : minidom.Databases.DBObjectCursorPO<CRicontatto>
        {

            private DBCursorField<DateTime> m_DataPrevista = new DBCursorField<DateTime>("DataPrevista");
            //private CCursorFieldDBDate m_DataPrevista = new CCursorFieldDBDate("DataPrevistaStr");
            private DBCursorField<int> m_IDAssegnatoA = new DBCursorField<int>("IDAssegnatoA");
            private DBCursorStringField m_NomeAssegnatoA = new DBCursorStringField("NomeAssegnatoA");
            private DBCursorStringField m_Note = new DBCursorStringField("Note");
            private DBCursorField<StatoRicontatto> m_StatoRicontatto = new DBCursorField<StatoRicontatto>("StatoRicontatto");
            private DBCursorField<DateTime> m_DataRicontatto = new DBCursorField<DateTime>("DataRicontatto");
            private DBCursorField<int> m_IDOperatore = new DBCursorField<int>("IDOperatore");
            private DBCursorStringField m_NomeOperatore = new DBCursorStringField("NomeOperatore");
            private DBCursorStringField m_TipoContatto = new DBCursorStringField("TipoContatto");
            private DBCursorField<int> m_IDContatto = new DBCursorField<int>("IDContatto");
            private DBCursorField<int> m_IDPersona = new DBCursorField<int>("IDPersona");
            private DBCursorStringField m_NomePersona = new DBCursorStringField("NomePersona");
            private DBCursorStringField m_SourceName = new DBCursorStringField("SourceName");
            private DBCursorStringField m_SourceParam = new DBCursorStringField("SourceParam");
            private DBCursorField<int> m_Promemoria = new DBCursorField<int>("Promemoria");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("NFlags");
            private DBCursorStringField m_Categoria = new DBCursorStringField("Categoria");
            private DBCursorStringField m_DettaglioStato = new DBCursorStringField("DettaglioStato");
            private DBCursorStringField m_DettaglioStato1 = new DBCursorStringField("DettaglioStato1");
            private DBCursorStringField m_TipoAppuntamento = new DBCursorStringField("TipoAppuntamento");
            private DBCursorStringField m_NumeroOIndirizzo = new DBCursorStringField("NumeroOIndirizzo");
            private DBCursorField<int> m_IDRicontattoPrecedente = new DBCursorField<int>("IDRicPrec");
            private DBCursorField<int> m_IDRicontattoSuccessivo = new DBCursorField<int>("IDRicSucc");
            private DBCursorField<int> m_Priorita = new DBCursorField<int>("Priorita");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRicontattiCursor()
            {

            }

            /// <summary>
            /// Campo Priorita
            /// </summary>
            public DBCursorField<int> Priorita
            {
                get
                {
                    return m_Priorita;
                }
            }

            /// <summary>
            /// Campo IDRicontattoPrecedente
            /// </summary>
            public DBCursorField<int> IDRicontattoPrecedente
            {
                get
                {
                    return m_IDRicontattoPrecedente;
                }
            }

            /// <summary>
            /// Campo IDRicontattoSuccessivo
            /// </summary>
            public DBCursorField<int> IDRicontattoSuccessivo
            {
                get
                {
                    return m_IDRicontattoSuccessivo;
                }
            }

            /// <summary>
            /// Campo TipoAppuntamento
            /// </summary>
            public DBCursorStringField TipoAppuntamento
            {
                get
                {
                    return m_TipoAppuntamento;
                }
            }

            /// <summary>
            /// Campo NumeroOIndirizzo
            /// </summary>
            public DBCursorStringField NumeroOIndirizzo
            {
                get
                {
                    return m_NumeroOIndirizzo;
                }
            }

            /// <summary>
            /// Campo Categoria
            /// </summary>
            public DBCursorStringField Categoria
            {
                get
                {
                    return m_Categoria;
                }
            }

            /// <summary>
            /// Campo Flags
            /// </summary>
            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            /// <summary>
            /// Campo Promemoria
            /// </summary>
            public DBCursorField<int> Promemoria
            {
                get
                {
                    return m_Promemoria;
                }
            }

            //public CCursorFieldDBDate DataPrevista
            //{
            //    get
            //    {
            //        return m_DataPrevista;
            //    }
            //}

            /// <summary>
            /// Campo DataPrevista
            /// </summary>
            public DBCursorField<DateTime> DataPrevista
            {
                get
                {
                    return m_DataPrevista;
                }
            }

            // Public ReadOnly Property DataPrevista As DBCursorField(Of Date)
            // Get
            // Return Me.m_DataPrevista
            // End Get
            // End Property


            /// <summary>
            /// Campo IDAssegnatoA
            /// </summary>
            public DBCursorField<int> IDAssegnatoA
            {
                get
                {
                    return m_IDAssegnatoA;
                }
            }

            /// <summary>
            /// Campo NomeAssegnatoA
            /// </summary>
            public DBCursorStringField NomeAssegnatoA
            {
                get
                {
                    return m_NomeAssegnatoA;
                }
            }

            /// <summary>
            /// Campo Note
            /// </summary>
            public DBCursorStringField Note
            {
                get
                {
                    return m_Note;
                }
            }

            /// <summary>
            /// Campo StatoRicontatto
            /// </summary>
            public DBCursorField<StatoRicontatto> StatoRicontatto
            {
                get
                {
                    return m_StatoRicontatto;
                }
            }

            /// <summary>
            /// Campo DataRicontatto
            /// </summary>
            public DBCursorField<DateTime> DataRicontatto
            {
                get
                {
                    return m_DataRicontatto;
                }
            }

            /// <summary>
            /// Campo IDOperatore
            /// </summary>
            public DBCursorField<int> IDOperatore
            {
                get
                {
                    return m_IDOperatore;
                }
            }

            /// <summary>
            /// Campo NomeOperatore
            /// </summary>
            public DBCursorStringField NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }
            }

            /// <summary>
            /// Campo TipoContatto
            /// </summary>
            public DBCursorStringField TipoContatto
            {
                get
                {
                    return m_TipoContatto;
                }
            }

            /// <summary>
            /// Campo IDContatto
            /// </summary>
            public DBCursorField<int> IDContatto
            {
                get
                {
                    return m_IDContatto;
                }
            }

            /// <summary>
            /// Campo IDPersona
            /// </summary>
            public DBCursorField<int> IDPersona
            {
                get
                {
                    return m_IDPersona;
                }
            }

            /// <summary>
            /// Campo NomePersona
            /// </summary>
            public DBCursorStringField NomePersona
            {
                get
                {
                    return m_NomePersona;
                }
            }

            /// <summary>
            /// Campo SourceName
            /// </summary>
            public DBCursorStringField SourceName
            {
                get
                {
                    return m_SourceName;
                }
            }

            /// <summary>
            /// Campo SourceParam
            /// </summary>
            public DBCursorStringField SourceParam
            {
                get
                {
                    return m_SourceParam;
                }
            }

            /// <summary>
            /// Campo DettaglioStato
            /// </summary>
            public DBCursorStringField DettaglioStato
            {
                get
                {
                    return m_DettaglioStato;
                }
            }

            /// <summary>
            /// Campo DettaglioStato1
            /// </summary>
            public DBCursorStringField DettaglioStato1
            {
                get
                {
                    return m_DettaglioStato1;
                }
            }

            //public override object InstantiateNew(DBReader dbRis)
            //{
            //    return new CRicontatto();
            //}

            //public override string GetTableName()
            //{
            //    // If (Me.Stato.IsSet AndAlso Me.Stato.Value =ObjectStatus.OBJECT_VALID ) AndAlso (Me.StatoRicontatto = Anagrafica.StatoRicontatto.PROGRAMMATO OrElse Me.StatoRicontatto = Anagrafica.StatoRicontatto.RIMANDATO) AndAlso (Me.NomeLista = "") Then
            //    return "tbl_Ricontatti";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Ricontatti; //.Module;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return Ricontatti.Database;
            //}

            /// <summary>
            /// Limiti aggiuntivi
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldBase GetWherePartLimit()
            {
                var ret = base.GetWherePartLimit();
                if (!Module.UserCanDoAction("list"))
                {
                    if (Module.UserCanDoAction("list_own"))
                    {
                        var u = Sistema.Users.CurrentUser;
                        ret += Field("IDAssegnatoA").EQ(DBUtils.GetID(u, 0));
                    }
                }
                return ret;
            }
            
        }
    }
}