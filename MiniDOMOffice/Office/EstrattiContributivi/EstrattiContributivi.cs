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
        /// Repository di oggetti <see cref="EstrattoContributivo"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CEstrattiContributiviClass 
            : CModulesClass<EstrattoContributivo>
        {
            /// <summary>
            /// Evento generato quando l'estratto contributivo viene richiesto
            /// </summary>
            public event ItemEventHandler<EstrattoContributivo> ItemRichiesto;

            /// <summary>
            /// Evento generato quando l'estratto contributivo viene preso in carico
            /// </summary>
            public event ItemEventHandler<EstrattoContributivo> ItemPresoInCarico;

            /// <summary>
            /// Evento generato quando l'estratto contributivo viene evaso
            /// </summary>
            public event ItemEventHandler<EstrattoContributivo> ItemEvaso;

            /// <summary>
            /// Evento generato quando l'estratto contributivo viene segnato come annullato 
            /// </summary>
            public event ItemEventHandler<EstrattoContributivo> ItemErrato;


            /// <summary>
            /// Evento generato quando l'estratto contributivo viene segnato come sospeso
            /// </summary>
            public event ItemEventHandler<EstrattoContributivo> ItemSospeso;



            private CGroup m_GruppoResponsabili = null;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CEstrattiContributiviClass() 
                : base("modOfficeEstrattiC", typeof(EstrattiContributiviCursor))
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
            protected virtual void HandlePersonaMerged(object sender, MergePersonaEventArgs e)
            {
                lock (Anagrafica.@lock)
                {
                    var mi = e.MI;
                    var persona1 = mi.Persona1;
                    var persona2 = mi.Persona2;
                    CMergePersonaRecord rec;


                    if (persona1 is CPersonaFisica)
                    {
                        // Tabella tbl_OfficeEstrattiC 
                        using (var cursor = new EstrattiContributiviCursor())
                        {
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.IDCliente.Value = mi.IDPersona2;
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                rec = new CMergePersonaRecord();
                                rec.NomeTabella = "tbl_OfficeEstrattiC";
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
                        using (var cursor = new EstrattiContributiviCursor())
                        {
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.IDAmministrazione.Value = mi.IDPersona2;
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                rec = new CMergePersonaRecord();
                                rec.NomeTabella = "tbl_OfficeEstrattiC";
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
            protected virtual void HandlePersonaUnMerged(object sender , MergePersonaEventArgs e)
            {
                lock (this.cacheLock)
                {
                    var mi = e.MI;
                    if (mi.Persona1 is CPersonaFisica)
                    {
                        var items = mi.GetAffectedRecorsIDs("tbl_OfficeEstrattiC", "IDCliente");
                        using (var cursor = new EstrattiContributiviCursor())
                        {
                            cursor.ID.ValueIn(items);
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                cursor.Item.Cliente = (CPersonaFisica) mi.Persona2;
                                cursor.Item.Save();
                            }
                        }
                    }
                    else
                    {
                        var items = mi.GetAffectedRecorsIDs("tbl_OfficeEstrattiC", "IDAmministrazione");
                        using (var cursor = new EstrattiContributiviCursor())
                        {
                            cursor.ID.ValueIn(items);
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                cursor.Item.Amministrazione = (CAzienda) mi.Persona2;
                                cursor.Item.Save();
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Restituisce il gruppo dei responsabili
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CGroup GruppoResponsabili
            {
                get
                {
                    if (m_GruppoResponsabili is null)
                    {
                        m_GruppoResponsabili = Sistema.Groups.GetItemByName("Responsabili Estratti Contributivi");
                        if (m_GruppoResponsabili is null)
                        {
                            m_GruppoResponsabili = new Sistema.CGroup("Responsabili Estratti Contributivi");
                            m_GruppoResponsabili.Stato = ObjectStatus.OBJECT_VALID;
                            m_GruppoResponsabili.Save();
                        }
                    }

                    return m_GruppoResponsabili;
                }
            }

            /// <summary>
            /// Genera l'evento ItemPresoInCarico
            /// </summary>
            /// <param name="e"></param>
            protected internal virtual void OnPresoInCarico(ItemEventArgs<EstrattoContributivo> e)
            {
                ItemPresoInCarico?.Invoke(this, e);
            }

            /// <summary>
            /// Genera l'evento ItemPresoInCarico
            /// </summary>
            /// <param name="e"></param>
            protected internal virtual void OnRichiesto(ItemEventArgs<EstrattoContributivo> e)
            {
                ItemRichiesto?.Invoke(this, e);
            }

            /// <summary>
            /// Genera l'evento ItemPresoInCarico
            /// </summary>
            /// <param name="e"></param>
            protected internal virtual void OnEvaso(ItemEventArgs<EstrattoContributivo> e)
            {
                ItemEvaso?.Invoke(this, e);
            }

            /// <summary>
            /// Genera l'evento ItemErrato
            /// </summary>
            /// <param name="e"></param>
            protected internal virtual void OnErrato(ItemEventArgs<EstrattoContributivo> e)
            {
                ItemErrato?.Invoke(this, e);
            }

            /// <summary>
            /// Genera l'evento ItemSospeso
            /// </summary>
            /// <param name="e"></param>
            protected internal virtual void OnSospeso(ItemEventArgs<EstrattoContributivo> e)
            {
                ItemSospeso?.Invoke(this, e);
            }

            //private Sistema.CModule InitModule()
            //{
            //    var ret = Sistema.Modules.GetItemByName("modOfficeEstrattiC");
            //    if (ret is null)
            //    {
            //        ret = new Sistema.CModule("modOfficeEstrattiC");
            //        ret.Description = "Estratti Contributivi";
            //        ret.DisplayName = "Estratti Contributivi";
            //        ret.Parent = Office.Module;
            //        ret.Stato = ObjectStatus.OBJECT_VALID;
            //        ret.Save();
            //        ret.InitializeStandardActions();
            //    }

            //    if (!Database.Tables.ContainsKey("tbl_OfficeEstrattiC"))
            //    {
            //        var table = Database.Tables.Add("tbl_OfficeEstrattiC");
            //        Databases.CDBEntityField field;
            //        field = table.Fields.Add("ID", TypeCode.Int32);
            //        field.AutoIncrement = true;
            //        table.Create();
            //    }

            //    string t = GruppoResponsabili.GroupName;
            //    return ret;
            //}


            /// <summary>
            /// Prepara il template
            /// </summary>
            /// <param name="template"></param>
            /// <param name="richiesta"></param>
            /// <param name="basePath"></param>
            /// <returns></returns>
            public string ParseTemplate(string template, EstrattoContributivo richiesta, string basePath)
            {
                string ret = template;
                ret = Strings.Replace(ret, "%%ID%%", DBUtils.GetID(richiesta).ToString());
                ret = Strings.Replace(ret, "%%NOMERICHIEDENTE%%", richiesta.NomeRichiedente);
                ret = Strings.Replace(ret, "%%NOMEASSEGNATOA%%", richiesta.NomeAssegnatoA);
                ret = Strings.Replace(ret, "%%NOMECLIENTE%%", richiesta.NomeCliente);
                ret = Strings.Replace(ret, "%%URLDELEGA%%", GetURL(richiesta.Delega, basePath));
                ret = Strings.Replace(ret, "%%URLDOCRIC%%", GetURL(richiesta.DocumentoRiconoscimento, basePath));
                ret = Strings.Replace(ret, "%%URLCODFISC%%", GetURL(richiesta.CodiceFiscale, basePath));
                // ret = Replace(ret, "%%URLESTRATTO%%", GetURL(richiesta.Allegato, basePath))
                ret = Strings.Replace(ret, "%%USERNAME%%", Sistema.Users.CurrentUser.Nominativo);
                ret = Strings.Replace(ret, "%%MESSAGES%%", CreateElencoMessaggi(new Sistema.CAnnotazioni(richiesta)));
                return ret;
            }

            private string CreateElencoMessaggi(Sistema.CAnnotazioni items)
            {
                string ret = "";
                foreach (Sistema.CAnnotazione a in items)
                    ret += Sistema.Formats.FormatUserDateTime(a.CreatoIl) + " <b>" + a.CreatoDa.Nominativo + "</b>: " + a.Valore + "<br/>";
                return ret;
            }

            private string GetURL(Sistema.CAttachment a, string bp)
            {
                if (a is null)
                    return "";
                string ret = Strings.Trim(bp);
                string url = a.URL;
                if (Strings.Right(ret, 1) != "/")
                    ret = ret + "/";
                if (Strings.Left(url, 1) == "/")
                    url = Strings.Mid(url, 2);
                return ret + url;
            }

             
        }


    }

    public partial class Office
    {


 
        private static CEstrattiContributiviClass m_EstrattiContributivi = null;

        /// <summary>
        /// Repository di oggetti <see cref="EstrattoContributivo"/>
        /// </summary>
        public static CEstrattiContributiviClass EstrattiContributivi
        {
            get
            {
                if (m_EstrattiContributivi is null)
                    m_EstrattiContributivi = new CEstrattiContributiviClass();
                return m_EstrattiContributivi;
            }
        }
    }
}