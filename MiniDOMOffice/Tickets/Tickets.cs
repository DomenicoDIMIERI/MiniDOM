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
    namespace repositories
    {

        /// <summary>
        /// Repository di <see cref="CTicket"/>
        /// </summary>
        [Serializable]
        public partial class CTicketsClass 
            : CModulesClass<CTicket>
        {

            [NonSerialized] private CGroup m_GruppoResponsabili = null;
            [NonSerialized] private CGroup m_GruppoEsclusi = null;


            /// <summary>
            /// Costruttore
            /// </summary>
            public CTicketsClass() 
                : base("modTickets", typeof(minidom.Office.CTicketCursor))
            {
            }

            /// <summary>
            /// Inizializza le risorse
            /// </summary>
            public override void Initialize()
            {
                base.Initialize();
                minidom.Anagrafica.PersonaMerged += this.HandlePersonaMerged;
                minidom.Anagrafica.PersonaUnMerged += this.HandlePersonaUnMerged;
            }

            /// <summary>
            /// Rilascia le risorse
            /// </summary>
            public override void Terminate()
            {
                minidom.Anagrafica.PersonaMerged -= this.HandlePersonaMerged;
                minidom.Anagrafica.PersonaUnMerged -= this.HandlePersonaUnMerged;
                base.Terminate();
            }

            /// <summary>
            /// Gestisce il merge delle persone
            /// </summary>
            /// <param name="e"></param>
            /// <param name="sender"></param>
            private void HandlePersonaMerged(object sender, MergePersonaEventArgs e)
            {
                lock (Anagrafica.@lock)
                {
                    var mi = e.MI;
                    var persona1 = mi.Persona1;
                    var persona2 = mi.Persona2;
                    CMergePersonaRecord rec;


                    if (persona1 is CPersonaFisica)
                    {
                        // Tabella tbl_CQSPDRichCERQ 
                        using (var cursor = new CTicketCursor())
                        {
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.IDCliente.Value = mi.IDPersona2;
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                rec = new CMergePersonaRecord();
                                rec.NomeTabella = "tbl_SupportTickets";
                                rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                                rec.FieldName = "IDCliente";
                                mi.TabelleModificate.Add(rec);

                                cursor.Item.Cliente = (CPersonaFisica)mi.Persona1;
                                cursor.Item.Save();
                            }
                        }



                    }
                     

                }
            }

            /// <summary>
            /// Gestisce l'evento Unmerge di una persona fisica o di un'azienda
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            protected virtual void HandlePersonaUnMerged(object sender, MergePersonaEventArgs e)
            {
                lock (this.cacheLock)
                {
                    var mi = e.MI;
                    if (mi.Persona1 is CPersonaFisica)
                    {
                        var items = mi.GetAffectedRecorsIDs("tbl_SupportTickets", "IDCliente");
                        using (var cursor = new CTicketCursor())
                        {
                            cursor.ID.ValueIn(items);
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                cursor.Item.Cliente = (CPersonaFisica)mi.Persona2;
                                cursor.Item.Save();
                            }
                        }
                    }
                     
                }
            }


            /// <summary>
            /// Restituisce il gruppo dei responsabili cioè gli utenti a cui vengono notificate tutte le richieste di assistenza
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CGroup GruppoResponsabili
            {
                get
                {
                    lock (this)
                    {
                        if (m_GruppoResponsabili is null)
                        {
                            m_GruppoResponsabili = Sistema.Groups.GetItemByName("Responsabili Supporto Tecnico");
                            if (m_GruppoResponsabili is null)
                            {
                                m_GruppoResponsabili = new Sistema.CGroup("Responsabili Supporto Tecnico");
                                m_GruppoResponsabili.Notes = "Gruppo a cui vengono notificate tutte le richieste di assistenza, qualsiasi sia la loro categoria.";
                                m_GruppoResponsabili.Stato = ObjectStatus.OBJECT_VALID;
                                m_GruppoResponsabili.Save();
                            }
                        }

                        return m_GruppoResponsabili;
                    }
                }
            }

            /// <summary>
            /// Restituisce il gruppo degli utenti a cui non inviare alcun tipo di richiesta di assistenza.
            /// Questo gruppo bypassa tutti gli altri
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CGroup GruppoEsclusi
            {
                get
                {
                    lock (this)
                    {
                        if (m_GruppoEsclusi is null)
                        {
                            m_GruppoEsclusi = Sistema.Groups.GetItemByName("Esclusi Supporto Tecnico");
                            if (m_GruppoEsclusi is null)
                            {
                                m_GruppoEsclusi = new Sistema.CGroup("Esclusi Supporto Tecnico");
                                m_GruppoEsclusi.Stato = ObjectStatus.OBJECT_VALID;
                                m_GruppoEsclusi.Notes = "Gruppo degli utenti per cui è bloccato l'invio delle notifiche delle richieste di assistenza";
                                m_GruppoEsclusi.Save();
                            }
                        }

                        return m_GruppoEsclusi;
                    }
                }
            }
             
            /// <summary>
            /// Restituisce i ticket aperti dall'utente
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public CCollection<CTicket> GetActiveItems(CUser user)
            {
                var ret = new CCollection<CTicket>();
                if (DBUtils.GetID(user, 0) == 0)
                    return ret;

                var categories = minidom.Office.TicketCategories.GetUserAllowedCategories(Sistema.Users.CurrentUser);

                using (var cursor = new minidom.Office.CTicketCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.StatoSegnalazione.ValueIn(new[] { minidom.Office.TicketStatus.APERTO, minidom.Office.TicketStatus.INLAVORAZIONE, minidom.Office.TicketStatus.INSERITO, minidom.Office.TicketStatus.RIAPERTO, minidom.Office.TicketStatus.SOSPESO });
                    if (categories.Count == 0)
                    {
                        cursor.IDApertoDa.Value = DBUtils.GetID(user, 0);
                    }
                    else
                    {
                        var wherePart = (DBCursorFieldBase) cursor.Field("ApertoDa").EQ(DBUtils.GetID(user, 0));
                        foreach (var cat in categories)
                        {
                            wherePart += cursor.Field("Categoria").EQ(cat.Categoria) * cursor.Field("Sottocategoria").EQ(cat.Sottocategoria);
                        }

                        cursor.WhereClauses *= wherePart;
                    }

                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }

                }
                return ret;
            }
        }
    }

    public partial class Office
    {
        private static CTicketsClass m_Tickets = null;

        /// <summary>
        /// Repository di <see cref="CTicket"/>
        /// </summary>
        public static CTicketsClass Tickets
        {
            get
            {
                if (m_Tickets is null)
                    m_Tickets = new CTicketsClass();
                return m_Tickets;
            }
        }
    }
}