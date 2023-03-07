using System;
using System.Collections;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Provider degli eventi calendario da CRM
        /// </summary>
        public class CRMCalendarProvider 
            : Sistema.BaseCalendarActivitiesProvider
        {

            /// <summary>
            /// Informazioni relative alla persona
            /// </summary>
            private struct PersonaInfo 
                : IComparable
            {
                /// <summary>
                /// ID della persona 
                /// </summary>
                public int ID;

                /// <summary>
                /// ID del punto operativo
                /// </summary>
                public int PO;

                /// <summary>
                /// Dettaglio sullo stato della persona
                /// </summary>
                public string Stato;

                /// <summary>
                /// Costruttore
                /// </summary>
                /// <param name="c"></param>
                public PersonaInfo(Anagrafica.CPersonaFisica c)
                {
                    this.ID = DBUtils.GetID(c, 0);
                    this.PO = c.IDPuntoOperativo;
                    this.Stato = c.DettaglioEsito;
                }

                /// <summary>
                /// Costruttore
                /// </summary>
                /// <param name="id"></param>
                public PersonaInfo(int id)
                {
                    this.ID = id;
                    this.PO = 0;
                    this.Stato = "";
                }

                /// <summary>
                /// Compare due oggetti
                /// </summary>
                /// <param name="obj"></param>
                /// <returns></returns>
                public int CompareTo(PersonaInfo obj)
                {
                    return DMD.Arrays.Compare(this.ID, obj.ID);
                }

                int IComparable.CompareTo(object obj)
                {
                    return CompareTo((PersonaInfo)obj);
                }
            }

            private object listLock = new object();
            private PersonaInfo[] m_List;
            // Private m_statiValidi As String() = Nothing
            // {
            // "Da Contattare",
            // "Interessato",
            // "Non Interessato",
            // "Non Fattibile",
            // "Irrintracciabile",
            // "Attesa Busta Paga",
            // "Busta Paga Ricevuta",
            // "Sollecitare Appuntamento",
            // "Appuntamento Fissato",
            // "Certificato Stipendio/CUD da Richiedere",
            // "Certificato Stipendio/CUD Richiesto",
            // "Certificato Stipendio/CUD Ricevuto",
            // "Da Valutare",
            // "In Valutazione",
            // "Proposta",
            // "In Lavorazione",
            // "Proporre Delega",
            // "Rinnovabile"
            // }

            private string[] statiValidi = new[] { "Attesa Richiamata", "Da Contattare", "NON CONTATTARE", "Irrintracciabile", "Interessato", "Non Interessato", "Non Fattibile", "Studio di Fattibità", "Pratica", "Certificato di Stipendio/CUD", "Busta Paga" };

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRMCalendarProvider()
            {
                Anagrafica.PersonaCreated += handlePersona;
                Anagrafica.PersonaDeleted += handlePersona;
                Anagrafica.PersonaModified += handlePersona;
                m_List = null;
            }

            private bool IsInList(int id)
            {
                int localBinarySearch() { var argitem = new PersonaInfo(id); var ret = DMD.Arrays.BinarySearch(m_List, argitem); return ret; }

                return DMD.Arrays.Len(m_List) > 0 && localBinarySearch() >= 0;
            }

            private void AddToList(Anagrafica.CPersonaFisica c)
            {
                int i = -1;
                var info = new PersonaInfo(c);
                if (DMD.Arrays.Len(m_List) > 0)
                    i = DMD.Arrays.BinarySearch(m_List, info);

                if (i >= 0)
                {
                    this.m_List[i] = info;
                }
                else
                {
                    this.m_List = DMD.Arrays.Append(this.m_List, info);
                    DMD.Arrays.Sort(m_List);
                }
            }

            private void RemoveFromList(int id)
            {
                int i = -1;
                if (DMD.Arrays.Len(m_List) > 0)
                {
                    var argitem = new PersonaInfo(id);
                    i = DMD.Arrays.BinarySearch(m_List, argitem);
                }

                if (i >= 0)
                    m_List = DMD.Arrays.RemoveAt(this.m_List, i);
            }

            private bool CheckStato(Anagrafica.CPersonaFisica p)
            {
                return DMD.Arrays.IndexOf(statiValidi, p.DettaglioEsito) >= 0;
            }

            private void handlePersona(Anagrafica.PersonaEventArgs e)
            {
                var c = e.Persona;
                if (c is Anagrafica.CPersonaFisica)
                {
                    lock (listLock)
                    {
                        if (c.Stato == ObjectStatus.OBJECT_VALID && CheckStato((Anagrafica.CPersonaFisica)c))
                        {
                            AddToList((Anagrafica.CPersonaFisica)c);
                        }
                        else
                        {
                            RemoveFromList(DBUtils.GetID(c, 0));
                        }
                    }
                }
            }

            private CCollection<PersonaInfo> List
            {
                get
                {
                    lock (listLock)
                    {
                        if (m_List is null)
                        {
                            var tmp = new ArrayList();
                            using (var cursor = new Anagrafica.CPersonaCursor())
                            {
                                cursor.TipoPersona.Value = Anagrafica.TipoPersona.PERSONA_FISICA;
                                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                                cursor.DettaglioEsito.ValueIn(statiValidi);
                                cursor.ID.SortOrder = SortEnum.SORT_ASC;
                                cursor.IgnoreRights = true;
                                while (cursor.Read())
                                {
                                    tmp.Add(new PersonaInfo((Anagrafica.CPersonaFisica)cursor.Item));
                                }
                            }

                            m_List = (PersonaInfo[])tmp.ToArray(typeof(PersonaInfo));
                        }

                        var ret = new CCollection<PersonaInfo>();
                        if (m_List is object)
                            ret.AddRange(m_List);
                        return ret;
                    }
                }
            }

            private CCollection<PersonaInfo> GetList(string stato)
            {
                var u = Sistema.Users.CurrentUser;
                var ret = new CCollection<PersonaInfo>();
                foreach (PersonaInfo p in List)
                {
                    if ((p.Stato ?? "") == (stato ?? "") && (p.PO == 0 || u.Uffici.HasOffice(p.PO)))
                    {
                        ret.Add(p);
                    }
                }

                return ret;
            }

            /// <summary>
            /// Restituisce le persone attive
            /// </summary>
            /// <param name="nomeLista"></param>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <param name="ufficio"></param>
            /// <param name="operatore"></param>
            /// <returns></returns>
            public override CCollection<Sistema.CActivePerson> GetActivePersons(string nomeLista, DateTime? fromDate, DateTime? toDate, int ufficio = 0, int operatore = 0)
            {
                //TODO GetActivePersons
                return new CCollection<Sistema.CActivePerson>();
            }

            /// <summary>
            /// Restituisce le attività pendenti
            /// </summary>
            /// <returns></returns>
            public override CCollection<Sistema.ICalendarActivity> GetPendingActivities()
            {
                //TODO GetPendingActivities
                return new CCollection<Sistema.ICalendarActivity>();
            }

            /// <summary>
            /// Restituisce le scadenze
            /// </summary>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <returns></returns>
            public override CCollection<Sistema.ICalendarActivity> GetScadenze(DateTime? fromDate, DateTime? toDate)
            {
                //TODO GetScadenze
                return new CCollection<Sistema.ICalendarActivity>();
            }

            /// <summary>
            /// Restituisce l'elenco delle cose da fare
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public override CCollection<Sistema.ICalendarActivity> GetToDoList(Sistema.CUser user)
            {
                // SyncLock Me.listLock
                var ret = new CCollection<Sistema.ICalendarActivity>();
                Sistema.CCalendarActivity act;
                CCollection<PersonaInfo> items;
                foreach (string stato in statiValidi)
                {
                    items = GetList(stato);
                    if (items.Count > 0)
                    {
                        act = new Sistema.CCalendarActivity();
                        ((Sistema.ICalendarActivity)act).SetProvider(this);
                        act.Descrizione = "Ci sono " + items.Count + " clienti in stato: <b>" + stato + "</b>";
                        act.DataInizio = DMD.DateUtils.ToDay();
                        act.GiornataIntera = true;
                        act.Categoria = "Normale";
                        act.Flags = Sistema.CalendarActivityFlags.IsAction;
                        act.IconURL = "/widgets/images/activities/bustapagaatesa.png";
                        act.Note = "CRMShowClientiInStato" + DMD.Strings.vbLf + stato;
                        act.Stato = ObjectStatus.OBJECT_VALID;
                        act.Priorita = ret.Count;
                        ret.Add(act);
                    }
                }
                // 'If (CRM.CRMGroup.Members.Contains(Sistema.Users.CurrentUser)) Then
                // items = Me.GetList("Attesa Busta Paga")
                // If (items.Count > 0) Then
                // act = New CCalendarActivity
                // DirectCast(act, ICalendarActivity).SetProvider(Me)
                // act.Descrizione = "Ci sono " & items.Count & " clienti che devono inviare la busta paga"
                // act.DataInizio = Calendar.Now
                // act.GiornataIntera = True
                // act.Categoria = "Urgente"
                // act.Flags = CalendarActivityFlags.IsAction
                // act.IconURL = "/widgets/images/activities/bustapagaatesa.png"
                // act.Note = "CQSPDShowClientiInStatoAttesaBustaPaga"
                // act.Stato = ObjectStatus.OBJECT_VALID
                // ret.Add(act)
                // End If
                // 'End If

                // 'Dim c As CConsulentePratica = Finanziaria.Consulenti.GetItemByUser(user)
                // 'If (c IsNot Nothing) Then
                // items = Me.GetList("Busta Paga Ricevuta")
                // If (items.Count > 0) Then
                // act = New CCalendarActivity
                // DirectCast(act, ICalendarActivity).SetProvider(Me)
                // act.Descrizione = "Ci sono " & items.Count & " buste paga da valutare"
                // act.DataInizio = Calendar.Now
                // act.GiornataIntera = True
                // act.Categoria = "Urgente"
                // act.Flags = CalendarActivityFlags.IsAction
                // act.IconURL = "/widgets/images/activities/bustapagaricevuta.png"
                // act.Note = "CQSPDShowClientiInStatoBustaPagaRicevuta"
                // act.Stato = ObjectStatus.OBJECT_VALID
                // ret.Add(act)
                // End If

                // '-------------------
                // items = Me.GetList("Proporre Delega")

                // If (items.Count > 0) Then
                // act = New CCalendarActivity
                // DirectCast(act, ICalendarActivity).SetProvider(Me)
                // act.Descrizione = "Ci sono " & items.Count & " clienti a cui proporre la delega"
                // act.DataInizio = Calendar.Now
                // act.GiornataIntera = True
                // act.Categoria = "Urgente"
                // act.Flags = CalendarActivityFlags.IsAction
                // act.IconURL = "/widgets/images/activities/bustapagaricevuta.png"
                // act.Note = "CQSPDShowClientiInStatoClientiDelega"
                // act.Stato = ObjectStatus.OBJECT_VALID
                // ret.Add(act)
                // End If

                // '-------------------
                // items = Me.GetList("Rinnovabile")
                // If (items.Count > 0) Then
                // act = New CCalendarActivity
                // DirectCast(act, ICalendarActivity).SetProvider(Me)
                // act.Descrizione = "Ci sono " & items.Count & " clienti che possono rinnovare"
                // act.DataInizio = Calendar.Now
                // act.GiornataIntera = True
                // act.Categoria = "Urgente"
                // act.Flags = CalendarActivityFlags.IsAction
                // act.IconURL = "/widgets/images/activities/bustapagaricevuta.png"
                // act.Note = "CQSPDShowClientiInStatoClientiRinnovabili"
                // act.Stato = ObjectStatus.OBJECT_VALID
                // ret.Add(act)
                // End If
                // ' End If



                return ret;
                // End SyncLock
            }

            /// <summary>
            /// Salva l'attività
            /// </summary>
            /// <param name="item"></param>
            /// <param name="force"></param>
            public override void SaveActivity(Sistema.ICalendarActivity item, bool force = false)
            {
            }

            /// <summary>
            /// Elimina l'attività
            /// </summary>
            /// <param name="item"></param>
            /// <param name="force"></param>
            public override void DeleteActivity(Sistema.ICalendarActivity item, bool force = false)
            {
            }

            /// <summary>
            /// Restituisce il nome univoco del provider
            /// </summary>
            public override string UniqueName
            {
                get
                {
                    return "CRMCALPROVBASE";
                }
            }
        }
    }
}