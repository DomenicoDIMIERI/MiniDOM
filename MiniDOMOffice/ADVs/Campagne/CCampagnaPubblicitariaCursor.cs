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
    public partial class ADV
    {

        /// <summary>
        /// Cursore sulla tabella <see cref="CCampagnaPubblicitaria"/>
        /// </summary>
        [Serializable]
        public class CCampagnaPubblicitariaCursor 
            : minidom.Databases.DBObjectCursor<CCampagnaPubblicitaria>
        {
            private DBCursorStringField m_NomeCampagna = new DBCursorStringField("NomeCampagna");
            private DBCursorStringField m_Titolo = new DBCursorStringField("Titolo");
            private DBCursorStringField m_Testo = new DBCursorStringField("Testo");
            // Private m_TipoTesto As New DBCursorFieldObj(Of String)("TipoTesto")
            private DBCursorField<bool> m_UsaListaDinamica = new DBCursorField<bool>("UsaListaDinamica");
            private DBCursorStringField m_ParametriLista = new DBCursorStringField("ParametriLista");
            private DBCursorField<int> m_IDListaDestinatari = new DBCursorField<int>("IDListaDestinatari");
            private DBCursorField<bool> m_Attiva = new DBCursorField<bool>("Attiva");
            private DBCursorStringField m_NomeMittente = new DBCursorStringField("NomeMittente");
            private DBCursorStringField m_IndirizzoMittente = new DBCursorStringField("IndirizzoMittente");
            private DBCursorField<StatoCampagnaPubblicitaria> m_StatoCampagna = new DBCursorField<StatoCampagnaPubblicitaria>("StatoCampagna");
            private DBCursorField<bool> m_RichiediConfermaDiLettura = new DBCursorField<bool>("ConfermaLettura");
            private DBCursorField<bool> m_RichiediConfermaDiRecapito = new DBCursorField<bool>("ConfermaRecapito");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorStringField m_FileDaUtilizzare = new DBCursorStringField("FileDaUtilizzare");
            private DBCursorStringField m_ListaCC = new DBCursorStringField("ListaCC");
            private DBCursorStringField m_ListaCCN = new DBCursorStringField("ListaCCN");
            private DBCursorStringField m_ListaNO = new DBCursorStringField("ListaNO");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCampagnaPubblicitariaCursor()
            {
            }

            /// <summary>
            /// ListaNO
            /// </summary>
            public DBCursorStringField ListaNO
            {
                get
                {
                    return m_ListaNO;
                }
            }

            /// <summary>
            /// ListaCC
            /// </summary>
            public DBCursorStringField ListaCC
            {
                get
                {
                    return m_ListaCC;
                }
            }

            /// <summary>
            /// ListaCCN
            /// </summary>
            public DBCursorStringField ListaCCN
            {
                get
                {
                    return m_ListaCCN;
                }
            }

            /// <summary>
            /// FileDaUtilizzare
            /// </summary>
            public DBCursorStringField FileDaUtilizzare
            {
                get
                {
                    return m_FileDaUtilizzare;
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
            /// NomeCampagna
            /// </summary>
            public DBCursorStringField NomeCampagna
            {
                get
                {
                    return m_NomeCampagna;
                }
            }

            /// <summary>
            /// Titolo
            /// </summary>
            public DBCursorStringField Titolo
            {
                get
                {
                    return m_Titolo;
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
            /// UsaListaDinamica
            /// </summary>
            public DBCursorField<bool> UsaListaDinamica
            {
                get
                {
                    return m_UsaListaDinamica;
                }
            }

            /// <summary>
            /// ParametriLista
            /// </summary>
            public DBCursorStringField ParametriLista
            {
                get
                {
                    return m_ParametriLista;
                }
            }

            /// <summary>
            /// IDListaDestinatari
            /// </summary>
            public DBCursorField<int> IDListaDestinatari
            {
                get
                {
                    return m_IDListaDestinatari;
                }
            }

            /// <summary>
            /// Attiva
            /// </summary>
            public DBCursorField<bool> Attiva
            {
                get
                {
                    return m_Attiva;
                }
            }

            /// <summary>
            /// NomeMittente
            /// </summary>
            public DBCursorStringField NomeMittente
            {
                get
                {
                    return m_NomeMittente;
                }
            }

            /// <summary>
            /// IndirizzoMittente
            /// </summary>
            public DBCursorStringField IndirizzoMittente
            {
                get
                {
                    return m_IndirizzoMittente;
                }
            }

            /// <summary>
            /// StatoCampagna
            /// </summary>
            public DBCursorField<StatoCampagnaPubblicitaria> StatoCampagna
            {
                get
                {
                    return m_StatoCampagna;
                }
            }

            /// <summary>
            /// RichiediConfermaDiLettura
            /// </summary>
            public DBCursorField<bool> RichiediConfermaDiLettura
            {
                get
                {
                    return m_RichiediConfermaDiLettura;
                }
            }

            /// <summary>
            /// RichiediConfermaDiRecapito
            /// </summary>
            public DBCursorField<bool> RichiediConfermaDiRecapito
            {
                get
                {
                    return m_RichiediConfermaDiRecapito;
                }
            }

            // Public ReadOnly Property TipoTesto As DBCursorFieldObj(Of String)
            // Get
            // Return Me.m_TipoTesto
            // End Get
            // End Property

             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.ADV.Campagne;
            }

            
        }
    }
}