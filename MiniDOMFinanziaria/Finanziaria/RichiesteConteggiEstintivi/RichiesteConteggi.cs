using System;
using DMD;
using minidom;
using minidom.internals;

namespace minidom
{
    namespace internals
    {



        /// <summary>
    /// Rappresenta una richiesta di conteggio estintivo già presente sul gestionale esterno
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CRichiesteConteggiClass : CModulesClass<Finanziaria.CRichiestaConteggio>
        {
            public event SegnalataEventHandler Segnalata;

            public delegate void SegnalataEventHandler(object sender, ItemEventArgs e);

            public event PresaInCaricoEventHandler PresaInCarico;

            public delegate void PresaInCaricoEventHandler(object sender, ItemEventArgs e);

            // Public Event Richiesta(ByVal sender As Object, ByVal e As ItemEventArgs)


            public CRichiesteConteggiClass() : base("modCQSPDRichContEst", typeof(Finanziaria.CRichiestaConteggioCursor), 0)
            {
            }

            internal void doOnSegnalata(ItemEventArgs e)
            {
                Finanziaria.CRichiestaConteggio richiesta = (Finanziaria.CRichiestaConteggio)e.Item;
                var cliente = richiesta.Cliente;
                if (cliente is object)
                {
                    if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                    {
                        cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                        cliente.Save();
                    }

                    CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                    info.isClienteInAcquisizione = false;
                    info.isClienteAcquisito = true;
                    info.AggiungiAttenzione(richiesta, "Richiesta CE non ancora presa in carico", "Richiesta CE " + DBUtils.GetID(richiesta));
                    info.AggiornaOperazione(richiesta, "Richiesta CE Segnalata");
                }

                Segnalata?.Invoke(this, e);
                Module.DispatchEvent(new Sistema.EventDescription("segnalata", "Richiesta CE Segnalata", e.Item));
            }

            internal void doOnPresaInCarico(ItemEventArgs e)
            {
                Finanziaria.CRichiestaConteggio richiesta = (Finanziaria.CRichiestaConteggio)e.Item;
                var cliente = richiesta.Cliente;
                if (cliente is object)
                {
                    if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                    {
                        cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                        cliente.Save();
                    }

                    CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                    info.isClienteInAcquisizione = false;
                    info.isClienteAcquisito = true;
                    info.RimuoviAttenzione(richiesta, "Richiesta CE " + DBUtils.GetID(richiesta));
                    info.AggiornaOperazione(richiesta, "Richiesta CE Presa in carico da " + richiesta.PresaInCaricoDaNome);
                }

                PresaInCarico?.Invoke(this, e);
                Module.DispatchEvent(new Sistema.EventDescription("presaincarico", "Richiesta CE Presa In Carico", e.Item));
            }

            public string ParseTemplate(string template, Finanziaria.CRichiestaConteggio richiesta, CKeyCollection context)
            {
                Sistema.CUser currentUser = (Sistema.CUser)context["CurrentUser"];
                string baseURL = DMD.Strings.CStr(context["BaseURL"]);
                template = Strings.Replace(template, "%%ID%%", DBUtils.GetID(richiesta).ToString());
                template = Strings.Replace(template, "%%NUMEROPRATICA%%", richiesta.NumeroPratica);
                template = Strings.Replace(template, "%%USERNAME%%", currentUser.Nominativo);
                template = Strings.Replace(template, "%%NOMINATIVOCLIENTE%%", richiesta.NomeCliente);
                template = Strings.Replace(template, "%%NOMEISTITUTO%%", richiesta.NomeIstituto);
                template = Strings.Replace(template, "%%IDCLIENTE%%", richiesta.IDCliente.ToString());
                template = Strings.Replace(template, "%%NOMEPRESAINCARICODA%%", richiesta.PresaInCaricoDaNome);
                template = Strings.Replace(template, "%%BASEURL%%", baseURL);
                return template;
            }

            protected internal new void doItemCreated(ItemEventArgs e)
            {
                base.doItemCreated(e);
            }

            protected internal new void doItemDeleted(ItemEventArgs e)
            {
                base.doItemDeleted(e);
            }

            protected internal new void doItemModified(ItemEventArgs e)
            {
                base.doItemModified(e);
            }

            /// <summary>
        /// Restituisce tutte le richieste di conteggio effettuate per il cliente specificato
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
            public CCollection<Finanziaria.CRichiestaConteggio> GetRichiesteByPersona(Anagrafica.CPersona value)
            {
                if (value is null)
                    throw new ArgumentNullException("Persona");
                var ret = new CCollection<Finanziaria.CRichiestaConteggio>();
                if (DBUtils.GetID(value) == 0)
                    return ret;
                using (var cursor = new Finanziaria.CRichiestaConteggioCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDCliente.Value = DBUtils.GetID(value);
                    cursor.DataRichiesta.SortOrder = SortEnum.SORT_ASC;
                    cursor.IgnoreRights = true;
                    while (!cursor.EOF())
                    {
                        var r = cursor.Item;
                        r.SetCliente(value);
                        ret.Add(r);
                        cursor.MoveNext();
                    }
                }

                return ret;
            }

            /// <summary>
        /// Restituisce l'ultima richiesta di finanziamento fatta dal cliente specificato
        /// </summary>
        /// <param name="p">[in] Persona</param>
        /// <returns></returns>
        /// <remarks></remarks>
            public Finanziaria.CRichiestaConteggio GetUltimaRichiesta(Anagrafica.CPersonaFisica p)
            {
                if (p is null)
                    throw new ArgumentNullException("persona");
                if (DBUtils.GetID(p) == 0)
                    return null;
                using (var cursor = new Finanziaria.CRichiestaConteggioCursor())
                {
                    cursor.IDCliente.Value = DBUtils.GetID(p);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.DataRichiesta.SortOrder = SortEnum.SORT_DESC;
                    cursor.PageSize = 1;
                    cursor.IgnoreRights = true;
                    var ret = cursor.Item;
                    if (ret is object)
                        ret.SetCliente(p);
                    return ret;
                }
            }

            public override void Initialize()
            {
                base.Initialize();
                Databases.CDBTable table;
                Databases.CDBEntityField col;
                table = Finanziaria.Database.Tables.GetItemByKey("tbl_RichiesteFinanziamentiC");
                col = table.Fields.Alter("IDEstinzione", typeof(int));
                col = table.Fields.Alter("IDDOCConteggio", typeof(int));
                col = table.Fields.Alter("StatoRichiestaConteggio", typeof(int));
                table.Update();
            }
        }
    }

    public partial class Finanziaria
    {
        private static CRichiesteConteggiClass m_RichiesteConteggi = null;

        public static CRichiesteConteggiClass RichiesteConteggi
        {
            get
            {
                if (m_RichiesteConteggi is null)
                    m_RichiesteConteggi = new CRichiesteConteggiClass();
                return m_RichiesteConteggi;
            }
        }
    }
}