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
        /// Cursore di <see cref="CTicket"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CTicketCursor
            : minidom.Databases.DBObjectCursorPO<CTicket>
        {
            private DBCursorField<int> m_IDApertoDa = new DBCursorField<int>("ApertoDa");
            private DBCursorStringField m_NomeApertoDa = new DBCursorStringField("NomeApertoDa");
            private DBCursorField<int> m_IDCliente = new DBCursorField<int>("IDCliente");
            private DBCursorStringField m_NomeCliente = new DBCursorStringField("NomeCliente");
            private DBCursorField<int> m_IDInCaricoA = new DBCursorField<int>("InCaricoA");
            private DBCursorStringField m_NomeInCaricoA = new DBCursorStringField("InCaricoANome");
            private DBCursorStringField m_Categoria = new DBCursorStringField("Categoria");
            private DBCursorStringField m_Sottocategoria = new DBCursorStringField("Sottocategoria");
            private DBCursorStringField m_Messaggio = new DBCursorStringField("Messaggio");
            private DBCursorField<TicketStatus> m_StatoSegnalazione = new DBCursorField<TicketStatus>("StatoSegnalazione");
            private DBCursorField<PriorityEnum> m_Priorita = new DBCursorField<PriorityEnum>("Priorita");
            private DBCursorField<int> m_IDSupervisore = new DBCursorField<int>("IDSupervisore");
            private DBCursorStringField m_NomeSupervisore = new DBCursorStringField("NomeSupervisore");
            private DBCursorStringField m_Canale = new DBCursorStringField("Canale");
            private DBCursorField<DateTime> m_DataRichiesta = new DBCursorField<DateTime>("DataRichiesta");
            private DBCursorField<DateTime> m_DataPresaInCarico = new DBCursorField<DateTime>("DataPresaInCarico");
            private DBCursorField<DateTime> m_DataChiusura = new DBCursorField<DateTime>("DataChiusura");
            private DBCursorField<int> m_IDContesto = new DBCursorField<int>("IDContesto");
            private DBCursorStringField m_TipoContesto = new DBCursorStringField("TipoContesto");
            private DBCursorField<int> m_IDPostazione = new DBCursorField<int>("IDPostazione");
            private DBCursorStringField m_NomePostazione = new DBCursorStringField("NomePostazione");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTicketCursor()
            {
            }

            /// <summary>
            /// IDPostazione
            /// </summary>
            public DBCursorField<int> IDPostazione
            {
                get
                {
                    return m_IDPostazione;
                }
            }

            /// <summary>
            /// NomePostazione
            /// </summary>
            public DBCursorStringField NomePostazione
            {
                get
                {
                    return m_NomePostazione;
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
            /// IDApertoDa
            /// </summary>
            public DBCursorField<int> IDApertoDa
            {
                get
                {
                    return m_IDApertoDa;
                }
            }

            /// <summary>
            /// NomeApertoDa
            /// </summary>
            public DBCursorStringField NomeApertoDa
            {
                get
                {
                    return m_NomeApertoDa;
                }
            }

            /// <summary>
            /// IDInCaricoA
            /// </summary>
            public DBCursorField<int> IDInCaricoA
            {
                get
                {
                    return m_IDInCaricoA;
                }
            }

            /// <summary>
            /// NomeInCaricoA
            /// </summary>
            public DBCursorStringField NomeInCaricoA
            {
                get
                {
                    return m_NomeInCaricoA;
                }
            }

            /// <summary>
            /// IDSupervisore
            /// </summary>
            public DBCursorField<int> IDSupervisore
            {
                get
                {
                    return m_IDSupervisore;
                }
            }

            /// <summary>
            /// NomeSupervisore
            /// </summary>
            public DBCursorStringField NomeSupervisore
            {
                get
                {
                    return m_NomeSupervisore;
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
            /// Sottocategoria
            /// </summary>
            public DBCursorStringField Sottocategoria
            {
                get
                {
                    return m_Sottocategoria;
                }
            }

            /// <summary>
            /// Canale
            /// </summary>
            public DBCursorStringField Canale
            {
                get
                {
                    return m_Canale;
                }
            }

            /// <summary>
            /// Messaggio
            /// </summary>
            public DBCursorStringField Messaggio
            {
                get
                {
                    return m_Messaggio;
                }
            }

            /// <summary>
            /// StatoSegnalazione
            /// </summary>
            public DBCursorField<TicketStatus> StatoSegnalazione
            {
                get
                {
                    return m_StatoSegnalazione;
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
            /// DataPresaInCarico
            /// </summary>
            public DBCursorField<DateTime> DataPresaInCarico
            {
                get
                {
                    return m_DataPresaInCarico;
                }
            }

            /// <summary>
            /// DataChiusura
            /// </summary>
            public DBCursorField<DateTime> DataChiusura
            {
                get
                {
                    return m_DataChiusura;
                }
            }

            /// <summary>
            /// Priorita
            /// </summary>
            public DBCursorField<PriorityEnum> Priorita
            {
                get
                {
                    return m_Priorita;
                }
            }
  

            /// <summary>
            /// Inizializza i parametri per un nuovo oggetto
            /// </summary>
            /// <param name="item"></param>
            protected override void OnInitialize(CTicket item)
            {
                item.ApertoDa = Sistema.Users.CurrentUser;
                item.DataRichiesta = DMD.DateUtils.Now();
                base.OnInitialize(item);
            }

            /// <summary>
            /// Prepara i vincoli relativi alla sicurezza
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldBase GetWherePartLimit()
            {
                if (!Module.UserCanDoAction("list") && Module.UserCanDoAction("list_category"))
                {
                    var categories = minidom.Office.TicketCategories.GetUserAllowedCategories(Sistema.Users.CurrentUser);
                    if (categories.Count == 0)
                        return base.GetWherePartLimit();
                    var buff = DBCursorField.False;
                    foreach (var cat in categories)
                    {
                        if (string.IsNullOrEmpty(cat.Sottocategoria))
                        {
                            buff += this.Field("Categoria").EQ(cat.Categoria);
                        }
                        else
                        {
                            buff += this.Field("Categoria").EQ(cat.Categoria) * this.Field("Sottocategoria").EQ(cat.Sottocategoria);
                        }
                    }

                    return base.GetWherePartLimit() + buff;
                }
                else
                {
                    return base.GetWherePartLimit();
                }
            }
        }
    }
}