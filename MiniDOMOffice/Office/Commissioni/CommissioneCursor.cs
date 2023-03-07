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
        /// Cursore sulla tabella delle commissioni
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CommissioneCursor 
            : minidom.Databases.DBObjectCursorPO<Commissione>
        {
            private DBCursorField<StatoCommissione> m_StatoCommissione = new DBCursorField<StatoCommissione>("StatoCommissione");
            private DBCursorField<int> m_IDOperatore = new DBCursorField<int>("IDOperatore");
            private DBCursorStringField m_NomeOperatore = new DBCursorStringField("NomeOperatore");
            private DBCursorField<DateTime> m_DataPrevista = new DBCursorField<DateTime>("DataPrevista");
            private DBCursorField<DateTime> m_OraUscita = new DBCursorField<DateTime>("OraUscita");
            private DBCursorField<DateTime> m_OraRientro = new DBCursorField<DateTime>("OraRientro");
            private DBCursorStringField m_Motivo = new DBCursorStringField("Motivo");
            // Private m_Luogo As New DBCursorFieldObj(Of String)("Luogo")
            private DBCursorField<int> m_IDAzienda = new DBCursorField<int>("IDAzienda");
            private DBCursorStringField m_NomeAzienda = new DBCursorStringField("NomeAzienda");
            private DBCursorField<int> m_IDPersonaIncontrata = new DBCursorField<int>("IDPersona");
            private DBCursorStringField m_NomePersonaIncontrata = new DBCursorStringField("NomePersona");
            private DBCursorStringField m_Esito = new DBCursorStringField("Esito");
            private DBCursorField<DateTime> m_Scadenzario = new DBCursorField<DateTime>("Scadenzario");
            private DBCursorStringField m_NoteScadenzario = new DBCursorStringField("NoteScadenzario");
            private DBCursorField<int> m_IDRichiesta = new DBCursorField<int>("IDRichiesta");
            private DBCursorField<double> m_DistanzaPercorsa = new DBCursorField<double>("DistanzaPercorsa");
            private DBCursorField<int> m_ContextID = new DBCursorField<int>("ContextID");
            private DBCursorStringField m_ContextType = new DBCursorStringField("ContextType");
            private DBCursorField<int> m_IDAssegnataDa = new DBCursorField<int>("IDAssegnataDa");
            private DBCursorStringField m_NomeAssegnataDa = new DBCursorStringField("NomeAssegnataDa");
            private DBCursorField<DateTime> m_AssegnataIl = new DBCursorField<DateTime>("AssegnataIl");
            private DBCursorField<int> m_IDAssegnataA = new DBCursorField<int>("IDAssegnataA");
            private DBCursorStringField m_NomeAssegnataA = new DBCursorStringField("NomeAssegnataA");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorStringField m_SourceType = new DBCursorStringField("SourceType");
            private DBCursorField<int> m_SourceID = new DBCursorField<int>("SourceID");
            private DBCursorStringField m_Presso = new DBCursorStringField("Presso");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CommissioneCursor()
            {

            }

            /// <summary>
            /// Presso
            /// </summary>
            public DBCursorStringField Presso
            {
                get
                {
                    return m_Presso;
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
            /// IDAssegnataDa
            /// </summary>
            public DBCursorField<int> IDAssegnataDa
            {
                get
                {
                    return m_IDAssegnataDa;
                }
            }

            /// <summary>
            /// NomeAssegnataDa
            /// </summary>
            public DBCursorStringField NomeAssegnataDa
            {
                get
                {
                    return m_NomeAssegnataDa;
                }
            }

            /// <summary>
            /// IDAssegnataA
            /// </summary>
            public DBCursorField<int> IDAssegnataA
            {
                get
                {
                    return m_IDAssegnataA;
                }
            }

            /// <summary>
            /// NomeAssegnataA
            /// </summary>
            public DBCursorStringField NomeAssegnataA
            {
                get
                {
                    return m_NomeAssegnataA;
                }
            }

            /// <summary>
            /// AssegnataIl
            /// </summary>
            public DBCursorField<DateTime> AssegnataIl
            {
                get
                {
                    return m_AssegnataIl;
                }
            }

            /// <summary>
            /// StatoCommissione
            /// </summary>
            public DBCursorField<StatoCommissione> StatoCommissione
            {
                get
                {
                    return m_StatoCommissione;
                }
            }

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
            /// ContextID
            /// </summary>
            public DBCursorField<int> ContextID
            {
                get
                {
                    return m_ContextID;
                }
            }

            /// <summary>
            /// ContextType
            /// </summary>
            public DBCursorStringField ContextType
            {
                get
                {
                    return m_ContextType;
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
            /// DataPrevista
            /// </summary>
            public DBCursorField<DateTime> DataPrevista
            {
                get
                {
                    return m_DataPrevista;
                }
            }

            /// <summary>
            /// OraUscita
            /// </summary>
            public DBCursorField<DateTime> OraUscita
            {
                get
                {
                    return m_OraUscita;
                }
            }

            /// <summary>
            /// OraRientro
            /// </summary>
            public DBCursorField<DateTime> OraRientro
            {
                get
                {
                    return m_OraRientro;
                }
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

            // Public ReadOnly Property Luogo As DBCursorFieldObj(Of String)
            // Get
            // Return Me.m_Luogo
            // End Get
            // End Property

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
            /// NomeAzienda
            /// </summary>
            public DBCursorStringField NomeAzienda
            {
                get
                {
                    return m_NomeAzienda;
                }
            }

            /// <summary>
            /// IDPersonaIncontrata
            /// </summary>
            public DBCursorField<int> IDPersonaIncontrata
            {
                get
                {
                    return m_IDPersonaIncontrata;
                }
            }

            /// <summary>
            /// NomePersonaIncontrata
            /// </summary>
            public DBCursorStringField NomePersonaIncontrata
            {
                get
                {
                    return m_NomePersonaIncontrata;
                }
            }

            /// <summary>
            /// Esito
            /// </summary>
            public DBCursorStringField Esito
            {
                get
                {
                    return m_Esito;
                }
            }

            /// <summary>
            /// Scadenzario
            /// </summary>
            public DBCursorField<DateTime> Scadenzario
            {
                get
                {
                    return m_Scadenzario;
                }
            }

            /// <summary>
            /// NoteScadenzario
            /// </summary>
            public DBCursorStringField NoteScadenzario
            {
                get
                {
                    return m_NoteScadenzario;
                }
            }

            /// <summary>
            /// IDRichiesta
            /// </summary>
            public DBCursorField<int> IDRichiesta
            {
                get
                {
                    return m_IDRichiesta;
                }
            }

            /// <summary>
            /// DistanzaPercorsa
            /// </summary>
            public DBCursorField<double> DistanzaPercorsa
            {
                get
                {
                    return m_DistanzaPercorsa;
                }
            }

            /// <summary>
            /// Applica i limiti di autorizzazione
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldBase GetWherePartLimit()
            {
                var ret = base.GetWherePartLimit();
                if (!Module.UserCanDoAction("list"))
                {
                    var u = Sistema.Users.CurrentUser;
                    var uid = DBUtils.GetID(u, 0);
                    if (Module.UserCanDoAction("list_assigned"))
                    {
                        ret += (this.Field("IDOperatore").EQ(uid) + this.Field("IDAssegnataA").EQ(uid) );
                    }

                    if (Module.UserCanDoAction("list_own"))
                    {
                        ret += this.Field("IDAssegnataDa").EQ(uid);
                    }
                }

                return ret;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Commissioni;
            }
             

            /// <summary>
            /// Inizializza il nuovo oggetto
            /// </summary>
            /// <param name="item"></param>
            protected override void OnInitialize(Commissione item)
            {
                base.OnInitialize(item);
                var u = Sistema.Users.CurrentUser;
                item.AssegnataA = u;
                item.AssegnataDa = u;
                item.AssegnataA = u;
                item.AssegnataIl = DMD.DateUtils.Now();
                item.DataPrevista = item.AssegnataIl;
                item.PuntoOperativo = Anagrafica.Uffici.GetCurrentPO();
                 
            }
        }
    }
}