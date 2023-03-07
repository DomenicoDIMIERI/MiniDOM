using System;
using DMD;

namespace minidom
{
    public partial class Finanziaria
    {
        public sealed class CConsulenzeClass : CModulesClass<CQSPDConsulenza>
        {

            /// <summary>
        /// Evento generato quando viene inserita una nuova consulenza
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event ConsulenzaInseritaEventHandler ConsulenzaInserita;

            public delegate void ConsulenzaInseritaEventHandler(ItemEventArgs e);

            /// <summary>
        /// Evento generato quando la consulenza viene proposta al cliente
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event ConsulenzaPropostaEventHandler ConsulenzaProposta;

            public delegate void ConsulenzaPropostaEventHandler(ItemEventArgs e);

            /// <summary>
        /// Evento generato quando la consulenza viene accettata dal cliente
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event ConsulenzaAccettataEventHandler ConsulenzaAccettata;

            public delegate void ConsulenzaAccettataEventHandler(ItemEventArgs e);

            /// <summary>
        /// Evento generato quando la consulenza viene rifiutata dal cliente
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event ConsulenzaRifiutataEventHandler ConsulenzaRifiutata;

            public delegate void ConsulenzaRifiutataEventHandler(ItemEventArgs e);

            /// <summary>
        /// Evento generato quando la consulenza viene bocciata da un operatore o da un supervisore
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event ConsulenzaBocciataEventHandler ConsulenzaBocciata;

            public delegate void ConsulenzaBocciataEventHandler(ItemEventArgs e);

            /// <summary>
        /// Evento generato quando la consulenza viene eliminata
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event ConsulenzaEliminataEventHandler ConsulenzaEliminata;

            public delegate void ConsulenzaEliminataEventHandler(ItemEventArgs e);

            /// <summary>
        /// Evento generato quando una consulenza viene modificata
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event ConsulenzaModificataEventHandler ConsulenzaModificata;

            public delegate void ConsulenzaModificataEventHandler(ItemEventArgs e);

            /// <summary>
        /// Evento generato quando viene richiesta una valutazione della consulenza ad un supervisore
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event ConsulenzaRichiestaApprovazioneEventHandler ConsulenzaRichiestaApprovazione;

            public delegate void ConsulenzaRichiestaApprovazioneEventHandler(ItemEventArgs e);

            /// <summary>
        /// Evento generato quando un supervisore approva la consulenza
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event ConsulenzaApprovataEventHandler ConsulenzaApprovata;

            public delegate void ConsulenzaApprovataEventHandler(ItemEventArgs e);


            /// <summary>
        /// Evento generato quando un supervisore approva la consulenza
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event ConsulenzaNegataEventHandler ConsulenzaNegata;

            public delegate void ConsulenzaNegataEventHandler(ItemEventArgs e);
            /// <summary>
        /// Evento generato quando un supervisore prende in carico la richiesta di valutazione
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
            public event ConsulenzaPresaInCaricoEventHandler ConsulenzaPresaInCarico;

            public delegate void ConsulenzaPresaInCaricoEventHandler(ItemEventArgs e);

            internal CConsulenzeClass() : base("modConsulenzeCQS", typeof(CQSPDConsulenzaCursor))
            {
            }

            public CQSPDConsulenza GetUltimaConsulenzaProposta(Anagrafica.CPersonaFisica persona)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                return GetUltimaConsulenzaProposta(DBUtils.GetID(persona));
            }

            public CQSPDConsulenza GetUltimaConsulenzaProposta(int idPersona)
            {
                if (idPersona == 0)
                    return null;
                var cursor = new CQSPDConsulenzaCursor();
                CQSPDConsulenza ret;
                cursor.PageSize = 1;
                cursor.IDCliente.Value = idPersona;
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.StatoConsulenza.Value = StatiConsulenza.PROPOSTA;
                cursor.StatoConsulenza.Operator = Databases.OP.OP_GE;
                cursor.DataProposta.SortOrder = SortEnum.SORT_DESC;
                ret = cursor.Item;
                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }

                return ret;
            }

            public CCollection<CQSPDConsulenza> GetConsulenzeByPersona(Anagrafica.CPersona persona)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                var ret = new CCollection<CQSPDConsulenza>();
                if (DBUtils.GetID(persona) == 0)
                    return ret;
                var cursor = new CQSPDConsulenzaCursor();
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.IDCliente.Value = DBUtils.GetID(persona);
                cursor.DataConsulenza.SortOrder = SortEnum.SORT_DESC;
                cursor.IgnoreRights = true;
                while (!cursor.EOF())
                {
                    var q = cursor.Item;
                    q.SetCliente(persona);
                    ret.Add(q);
                    cursor.MoveNext();
                }

                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }

                return ret;
            }

            public CCollection<CQSPDConsulenza> GetConsulenzeByPersona(int idPersona)
            {
                return GetConsulenzeByPersona(Anagrafica.Persone.GetItemById(idPersona));
            }

            internal void doConsulenzaProposta(ItemEventArgs<CQSPDConsulenza> e)
            {
                var consulenza = e.Item;
                Anagrafica.CPersona cliente = consulenza.Cliente;
                // Dim info As CustomerCalls.CRMStatistichePersona = CustomerCalls.CRM.getp
                if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                info.AggiornaOperazione(consulenza, "Studio di fattibilità del " + Sistema.Formats.FormatUserDateTime(consulenza.DataConsulenza) + " proposto al cliente");
                ConsulenzaProposta?.Invoke(e);
            }

            internal void doConsulenzaAccettata(ItemEventArgs<CQSPDConsulenza> e)
            {
                var consulenza = e.Item;
                Anagrafica.CPersona cliente = consulenza.Cliente;
                // Dim info As CustomerCalls.CRMStatistichePersona = CustomerCalls.CRM.getp
                if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                info.AggiornaOperazione(consulenza, "Studio di fattibilità del " + Sistema.Formats.FormatUserDateTime(consulenza.DataConsulenza) + " accettato dal cliente");
                ConsulenzaAccettata?.Invoke(e);
            }

            internal void doConsulenzaRifiutata(ItemEventArgs<CQSPDConsulenza> e)
            {
                var consulenza = e.Item;
                Anagrafica.CPersona cliente = consulenza.Cliente;
                // Dim info As CustomerCalls.CRMStatistichePersona = CustomerCalls.CRM.getp
                if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                info.AggiornaOperazione(consulenza, "Studio di fattibilità del " + Sistema.Formats.FormatUserDateTime(consulenza.DataConsulenza) + " rifiutato dal cliente");
                ConsulenzaRifiutata?.Invoke(e);
            }

            internal void doConsulenzaInserita(ItemEventArgs<CQSPDConsulenza> e)
            {
                doItemCreated(e);
                var consulenza = e.Item;
                Anagrafica.CPersona cliente = consulenza.Cliente;
                // Dim info As CustomerCalls.CRMStatistichePersona = CustomerCalls.CRM.getp
                if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                info.AggiornaOperazione(consulenza, "Studio di fattibilità del " + Sistema.Formats.FormatUserDateTime(consulenza.DataConsulenza) + " registrato");
                ConsulenzaInserita?.Invoke(e);
            }

            internal void doConsulenzaBocciata(ItemEventArgs<CQSPDConsulenza> e)
            {
                var consulenza = e.Item;
                Anagrafica.CPersona cliente = consulenza.Cliente;
                // Dim info As CustomerCalls.CRMStatistichePersona = CustomerCalls.CRM.getp
                if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                info.AggiornaOperazione(consulenza, "Studio di fattibilità del " + Sistema.Formats.FormatUserDateTime(consulenza.DataConsulenza) + " registrato e non fattibilie");
                ConsulenzaBocciata?.Invoke(e);
            }

            internal void doConsulenzaEliminata(ItemEventArgs<CQSPDConsulenza> e)
            {
                doItemDeleted(e);
                ConsulenzaEliminata?.Invoke(e);
            }

            internal void doConsulenzaModificata(ItemEventArgs<CQSPDConsulenza> e)
            {
                doItemModified(e);
                ConsulenzaModificata?.Invoke(e);
            }

            private int GetNumeroOfferta(CQSPDConsulenza c)
            {
                if (c is null || c.StudioDiFattibilita is null)
                    return 1;
                return 1 + c.StudioDiFattibilita.Proposte.IndexOf(c);
            }

            private string FormatOfferta(COffertaCQS o)
            {
                if (o is null)
                    return null;
                return o.NomeProdotto + " - " + o.NomeCessionario + " (" + Sistema.Formats.FormatValuta(o.Rata) + "x" + Sistema.Formats.FormatInteger(o.Durata) + " = " + Sistema.Formats.FormatValuta(o.MontanteLordo) + ")";
            }

            public string FormatStatoEx(StatiConsulenza stato)
            {
                var items = new StatiConsulenza[] { StatiConsulenza.INSERITA, StatiConsulenza.PROPOSTA, StatiConsulenza.ACCETTATA, StatiConsulenza.RIFIUTATA, StatiConsulenza.BOCCIATA };
                var names = new string[] { "Solo Inserita", "Proposta al cliente", "Accettata dal cliente", "Rifiutata dal cliente", "Bocciata o Non fattibile" };
                return names[DMD.Arrays.IndexOf(items, stato)];
            }

            public string ParseTemplate(string template, CKeyCollection context)
            {
                string ret = template;
                CQSPDConsulenza consulenza = (CQSPDConsulenza)context["Consulenza"];
                Sistema.CUser currentUser = (Sistema.CUser)context["CurrentUser"];
                ret = Strings.Replace(ret, "%%NOMECONSULENTE%%", consulenza.NomeConsulente);
                ret = Strings.Replace(ret, "%%IDCLIENTE%%", consulenza.IDCliente.ToString());
                ret = Strings.Replace(ret, "%%NOMECLIENTE%%", consulenza.NomeCliente);
                ret = Strings.Replace(ret, "%%NUMEROOFFERTA%%", GetNumeroOfferta(consulenza).ToString());
                ret = Strings.Replace(ret, "%%ID%%", DBUtils.GetID(consulenza).ToString());
                ret = Strings.Replace(ret, "%%BASEURL%%", Sistema.ApplicationContext.BaseURL);
                ret = Strings.Replace(ret, "%%NOTE%%", consulenza.Descrizione);
                ret = Strings.Replace(ret, "%%USERNAME%%", currentUser.Nominativo);
                ret = Strings.Replace(ret, "%%DESCRIZIONECQS%%", FormatOfferta(consulenza.OffertaCQS));
                ret = Strings.Replace(ret, "%%DESCRIZIONEPD%%", FormatOfferta(consulenza.OffertaPD));
                ret = Strings.Replace(ret, "%%NETTOALLAMANO%%", Sistema.Formats.FormatValuta(consulenza.NettoRicavo - consulenza.SommaEstinzioni));
                ret = Strings.Replace(ret, "%%NETTORICAVO%%", Sistema.Formats.FormatValuta(consulenza.NettoRicavo));
                ret = Strings.Replace(ret, "%%SOMMAESTINZIONI%%", Sistema.Formats.FormatValuta(consulenza.SommaEstinzioni));
                ret = Strings.Replace(ret, "%%SOMMAPIGNORAMENTI%%", Sistema.Formats.FormatValuta(consulenza.SommaPignoramenti));
                ret = Strings.Replace(ret, "%%SOMMATRATTENUTE%%", Sistema.Formats.FormatValuta(consulenza.SommaTrattenuteVolontarie));
                ret = Strings.Replace(ret, "%%STATOCONSULENZA%%", Consulenze.FormatStato((StatiConsulenza)consulenza.Stato));
                ret = Strings.Replace(ret, "%%STATOCONSULENZAEX%%", Consulenze.FormatStatoEx((StatiConsulenza)consulenza.Stato));
                ret = Strings.Replace(ret, "%%DATAPROPOSTA%%", Sistema.Formats.FormatUserDateTime(consulenza.DataProposta));
                ret = Strings.Replace(ret, "%%DATACONFERMA%%", Sistema.Formats.FormatUserDateTime(consulenza.DataConferma));
                ret = Strings.Replace(ret, "%%DATAINSERIMENTO%%", Sistema.Formats.FormatUserDateTime(consulenza.CreatoIl));
                ret = Strings.Replace(ret, "%%MOTIVOANNULLAMENTO%%", consulenza.MotivoAnnullamento);
                ret = Strings.Replace(ret, "%%DETTAGLIOANNULLAMENTO%%", consulenza.DettaglioAnnullamento);
               // ret = ParseNib(ret, "%%DATACONSULENZA%%", (DateTime?)Sistema.Formats.FormatUserDateTime(consulenza.DataConsulenza));
                return ret;
            }

            private string ParseNib(string text, string nib, DateTime? value)
            {
                if (value.HasValue)
                {
                    return DMD.Strings.Replace(text, nib, Sistema.Formats.FormatUserDateTime(value));
                }
                else
                {
                    return DMD.Strings.Replace(text, nib, "");
                }
            }

            public CQSPDConsulenza GetUltimaConsulenza(int idPersona)
            {
                var cursor = new CQSPDConsulenzaCursor();
                try
                {
                    cursor.IDCliente.Value = idPersona;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.DataConsulenza.SortOrder = SortEnum.SORT_DESC;
                    // cursor.StatoConsulenza().setValue(StatiConsulenza.ACCETTATA);
                    cursor.PageSize = 1;
                    return cursor.Item;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    cursor.Dispose();
                }
            }

            public string FormatStato(StatiConsulenza stato)
            {
                var items = new StatiConsulenza[] { StatiConsulenza.INSERITA, StatiConsulenza.PROPOSTA, StatiConsulenza.ACCETTATA, StatiConsulenza.RIFIUTATA, StatiConsulenza.BOCCIATA };
                var names = new string[] { "Inserita", "Proposta", "Accettata", "Rifiutata", "Bocciata" };
                return names[DMD.Arrays.IndexOf(items, stato)];
            }

            protected internal void DoOnRequireApprovation(ItemEventArgs e)
            {
                CQSPDConsulenza consulenza = (CQSPDConsulenza)e.Item;
                Anagrafica.CPersona cliente = consulenza.Cliente;
                // Dim info As CustomerCalls.CRMStatistichePersona = CustomerCalls.CRM.getp
                if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                info.AggiornaOperazione(consulenza, "Studio di fattibilità del " + Sistema.Formats.FormatUserDateTime(consulenza.DataConsulenza) + " in richiesta approvazione");
                ConsulenzaRichiestaApprovazione?.Invoke(e);
            }

            protected internal void DoOnApprovata(ItemEventArgs e)
            {
                CQSPDConsulenza consulenza = (CQSPDConsulenza)e.Item;
                Anagrafica.CPersona cliente = consulenza.Cliente;
                // Dim info As CustomerCalls.CRMStatistichePersona = CustomerCalls.CRM.getp
                if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                info.AggiornaOperazione(consulenza, "Studio di fattibilità del " + Sistema.Formats.FormatUserDateTime(consulenza.DataConsulenza) + " approvato da " + Sistema.Users.CurrentUser.Nominativo);
                ConsulenzaApprovata?.Invoke(e);
            }

            protected internal void DoOnNegata(ItemEventArgs e)
            {
                CQSPDConsulenza consulenza = (CQSPDConsulenza)e.Item;
                Anagrafica.CPersona cliente = consulenza.Cliente;
                // Dim info As CustomerCalls.CRMStatistichePersona = CustomerCalls.CRM.getp
                if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                info.AggiornaOperazione(consulenza, "Studio di fattibilità del " + Sistema.Formats.FormatUserDateTime(consulenza.DataConsulenza) + " negato da " + Sistema.Users.CurrentUser.Nominativo);
                ConsulenzaNegata?.Invoke(e);
            }

            protected internal void DoOnRifiutata(ItemEventArgs e)
            {
                CQSPDConsulenza consulenza = (CQSPDConsulenza)e.Item;
                Anagrafica.CPersona cliente = consulenza.Cliente;
                // Dim info As CustomerCalls.CRMStatistichePersona = CustomerCalls.CRM.getp
                if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                info.AggiornaOperazione(consulenza, "Studio di fattibilità del " + Sistema.Formats.FormatUserDateTime(consulenza.DataConsulenza) + " rifiutato dal cliente");
                ConsulenzaBocciata?.Invoke(e);
            }

            protected internal void DoOnInCarico(ItemEventArgs e)
            {
                CQSPDConsulenza consulenza = (CQSPDConsulenza)e.Item;
                Anagrafica.CPersona cliente = consulenza.Cliente;
                // Dim info As CustomerCalls.CRMStatistichePersona = CustomerCalls.CRM.getp
                if ((!cliente.GetFlag(Anagrafica.PFlags.Cliente).HasValue || cliente.GetFlag(Anagrafica.PFlags.Cliente) == false) == true)
                {
                    cliente.SetFlag(Anagrafica.PFlags.Cliente, true);
                    cliente.Save();
                }

                CustomerCalls.CRMStatistichePersona info = CustomerCalls.CRM.GetContattoInfo(cliente);
                info.AggiornaOperazione(consulenza, "Studio di fattibilità del " + Sistema.Formats.FormatUserDateTime(consulenza.DataConsulenza) + " in supervisione");
                ConsulenzaPresaInCarico?.Invoke(e);
            }

            public CCollection<CQSPDConsulenza> GetConsulenzeInCorso(Anagrafica.CUfficio ufficio, Sistema.CUser operatore, DateTime? di, DateTime? df)
            {
                var cursor = new CQSPDConsulenzaCursor();
                var ret = new CCollection<CQSPDConsulenza>();
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.StatoConsulenza.Value = StatiConsulenza.ACCETTATA;
                if (ufficio is object)
                    cursor.IDPuntoOperativo.Value = DBUtils.GetID(ufficio);
                if (operatore is object)
                    cursor.IDConsulente.Value = DBUtils.GetID(operatore);
                if (di.HasValue)
                {
                    if (df.HasValue)
                    {
                        cursor.DataConsulenza.Between(di.Value, df.Value);
                    }
                    else
                    {
                        cursor.DataConsulenza.Value = di.Value;
                        cursor.DataConsulenza.Operator = Databases.OP.OP_GE;
                    }
                }
                else if (df.HasValue)
                {
                    cursor.DataConsulenza.Value = df.Value;
                    cursor.DataConsulenza.Operator = Databases.OP.OP_LE;
                }

                string dbSQL;
                dbSQL = "SELECT * FROM (SELECT [T1].*, [T2].[ID] AS [IDGrp] FROM (" + cursor.GetSQL() + ") AS [T1] INNER JOIN (SELECT [ID] FROM [tbl_CQSPDGrpConsulenze] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [CntPratiche]=0 GROUP BY [ID]) AS [T2] ON [T1].[IDGruppo]=[T2].[ID]) WHERE Not ([IDGrp] Is Null)";
                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }

                var dbRis = Database.ExecuteReader(dbSQL);
                while (dbRis.Read())
                {
                    var item = new CQSPDConsulenza();
                    Database.Load(item, dbRis);
                    ret.Add(item);
                }

                dbRis.Dispose();
                return ret;
            }
        }

        private static CConsulenzeClass m_Consulenze = null;

        public static CConsulenzeClass Consulenze
        {
            get
            {
                if (m_Consulenze is null)
                    m_Consulenze = new CConsulenzeClass();
                return m_Consulenze;
            }
        }
    }
}