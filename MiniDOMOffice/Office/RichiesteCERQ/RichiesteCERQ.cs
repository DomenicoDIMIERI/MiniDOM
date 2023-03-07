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
        /// Repository di <see cref="RichiestaCERQ"/>
        /// </summary>
        /// <remarks></remarks>
        public partial class CRichiesteCERQClass 
            : CModulesClass<RichiestaCERQ>
        {
         
            /// <summary>
            /// Evento generato quando viene effettuata una nuova richiesta ad un cessionario
            /// </summary>
            /// <remarks></remarks>
            public event ItemEventHandler<RichiestaCERQ> RichiestaStatusChanged;
                
            /// <summary>
            /// Costruttore
            /// </summary>
            public CRichiesteCERQClass() 
                : base("modCQSPDRicCERQ", typeof(RichiestaCERQCursor))
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


                    if (persona1 is Anagrafica.CPersonaFisica)
                    {
                        // Tabella tbl_CQSPDRichCERQ 
                        using (var cursor = new RichiestaCERQCursor())
                        {
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.IDCliente.Value = mi.IDPersona2;
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                rec = new CMergePersonaRecord();
                                rec.NomeTabella = "tbl_CQSPDRichCERQ";
                                rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                                rec.FieldName = "IDCliente";
                                mi.TabelleModificate.Add(rec);

                                cursor.Item.Cliente = (CPersonaFisica)mi.Persona1;
                                cursor.Item.Save();
                            }
                        }


                         
                    }
                    else
                    {
                        // Tabella tbl_CQSPDRichCERQ 
                        using (var cursor = new RichiestaCERQCursor())
                        {
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.IDAmministrazione.Value = mi.IDPersona2;
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                rec = new CMergePersonaRecord();
                                rec.NomeTabella = "tbl_CQSPDRichCERQ";
                                rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                                rec.FieldName = "IDAmministrazione";
                                mi.TabelleModificate.Add(rec);

                                cursor.Item.Amministrazione = (CAzienda)mi.Persona1;
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
                        var items = mi.GetAffectedRecorsIDs("tbl_CQSPDRichCERQ", "IDCliente");
                        using (var cursor = new RichiestaCERQCursor())
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
                    else
                    {
                        var items = mi.GetAffectedRecorsIDs("tbl_CQSPDRichCERQ", "IDAmministrazione");
                        using (var cursor = new RichiestaCERQCursor())
                        {
                            cursor.ID.ValueIn(items);
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                cursor.Item.Amministrazione = (CAzienda)mi.Persona2;
                                cursor.Item.Save();
                            }
                        }
                    }
                }
            }

            internal void doRichiestaCreata(ItemEventArgs<RichiestaCERQ> e)
            {
                RichiestaStatusChanged?.Invoke(this, e);
                //Module.DispatchEvent(new Sistema.EventDescription("richiesta_creata", e.ToString(), e));
            }

            internal void doRichiestaInviata(ItemEventArgs<RichiestaCERQ> e)
            {
                RichiestaCERQ richiesta = (RichiestaCERQ)e.Item;
                Anagrafica.CPersona cliente = richiesta.Cliente;
                if (cliente is object)
                {
                    var info = CustomerCalls.CRM.GetContattoInfo(cliente);
                    info.AggiornaOperazione(richiesta, "Richiesta " + richiesta.TipoRichiesta);
                }

                RichiestaStatusChanged?.Invoke(this, e);
                //Module.DispatchEvent(new Sistema.EventDescription("richiesta_inviata", e.ToString(), e));
            }

            internal void doRichiestaRifiutata(ItemEventArgs<RichiestaCERQ> e)
            {
                RichiestaCERQ richiesta = (RichiestaCERQ)e.Item;
                Anagrafica.CPersona cliente = richiesta.Cliente;
                if (cliente is object)
                {
                    var info = CustomerCalls.CRM.GetContattoInfo(cliente);
                    info.AggiornaOperazione(richiesta, "Richiesta Rifiutata " + richiesta.TipoRichiesta);
                }

                RichiestaStatusChanged?.Invoke(this, e);
                //Module.DispatchEvent(new Sistema.EventDescription("richiesta_rifiutata", e.ToString(), e));
            }

            internal void doRichiestaAnnullata(ItemEventArgs<RichiestaCERQ> e)
            {
                RichiestaCERQ richiesta = (RichiestaCERQ)e.Item;
                Anagrafica.CPersona cliente = richiesta.Cliente;
                if (cliente is object)
                {
                    var info = CustomerCalls.CRM.GetContattoInfo(cliente);
                    info.AggiornaOperazione(richiesta, "Richiesta Annullata " + richiesta.TipoRichiesta);
                }

                RichiestaStatusChanged?.Invoke(this, e);
                //Module.DispatchEvent(new Sistema.EventDescription("richiesta_annullata", e.ToString(), e));
            }

            internal void doRichiestaRitirata(ItemEventArgs<RichiestaCERQ> e)
            {
                RichiestaCERQ richiesta = (RichiestaCERQ)e.Item;
                Anagrafica.CPersona cliente = richiesta.Cliente;
                if (cliente is object)
                {
                    var info = CustomerCalls.CRM.GetContattoInfo(cliente);
                    info.AggiornaOperazione(richiesta, "Richiesta Completata " + richiesta.TipoRichiesta);
                }

                RichiestaStatusChanged?.Invoke(this, e);
                //Module.DispatchEvent(new Sistema.EventDescription("richiesta_ritirata", e.ToString(), e));
            }

            /// <summary>
            /// Restituisce le richieste relative ad una persona
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            public CCollection<RichiestaCERQ> GetRichiesteByPersona(CPersona persona)
            {
                var ret = new CCollection<RichiestaCERQ>();
                if (persona is null)
                    throw new ArgumentNullException("persona");

                using (var cursor = new RichiestaCERQCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDCliente.Value = DBUtils.GetID(persona, 0);
                    cursor.DataPrevista.SortOrder = SortEnum.SORT_DESC;
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }
                }
                return ret;
            }

        }

        private static CRichiesteCERQClass m_RichiesteCERQ = null;

        /// <summary>
        /// Repository di <see cref="RichiestaCERQ"/>
        /// </summary>
        public static CRichiesteCERQClass RichiesteCERQ
        {
            get
            {
                if (m_RichiesteCERQ is null)
                    m_RichiesteCERQ = new CRichiesteCERQClass();
                return m_RichiesteCERQ;
            }
        }
    }
}