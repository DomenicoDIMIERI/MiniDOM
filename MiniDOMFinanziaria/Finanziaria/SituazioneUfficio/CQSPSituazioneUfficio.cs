using System;
using System.Collections;
using DMD;
using DMD.XML;
using minidom;
using static minidom.CustomerCalls;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {

        // Public Class CQSPSituazioneUfficioOp
        // Public Op As Object
        // Public Childs As New CCollection(Of CQSPSituazioneUfficioOp)

        // End Class
    [Serializable]
        public class CQSPSituazioneUfficioClienteInfo
        {
            [NonSerialized] public Anagrafica.CPersona Cliente;
            public CCollection<CVisita> VisiteRicevute;
            public CCollection<CRichiestaFinanziamento> Richieste;
            public CCollection<CQSPDConsulenza> Consulenze;
            public CCollection<CPraticaCQSPD> PraticheInCorso;
            public CCollection<CPraticaCQSPD> PraticheConcluse;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CQSPSituazioneUfficioClienteInfo()
            {
                DMDObject.IncreaseCounter(this);
                Cliente = null;
                VisiteRicevute = new CCollection<CVisita>();
                Richieste = new CCollection<CRichiestaFinanziamento>();
                Consulenze = new CCollection<CQSPDConsulenza>();
                PraticheInCorso = new CCollection<CPraticaCQSPD>();
                PraticheConcluse = new CCollection<CPraticaCQSPD>();
            }

            public void Load(CQSPSituazioneUfficio info, Anagrafica.CPersona cliente)
            {
                Cliente = cliente;
                foreach (CVisita v in info.VisiteRicevute)
                {
                    if (v.IDPersona == DBUtils.GetID(Cliente))
                        VisiteRicevute.Add(v);
                }

                foreach (CRichiestaFinanziamento r in info.RichiestePendenti)
                {
                    if (r.IDCliente == DBUtils.GetID(Cliente))
                        Richieste.Add(r);
                }

                foreach (CQSPDConsulenza c in info.ConsulenzePendenti)
                {
                    if (c.IDCliente == DBUtils.GetID(Cliente))
                        Consulenze.Add(c);
                }

                foreach (CPraticaCQSPD p in info.PraticheInCorso)
                {
                    if (p.IDCliente == DBUtils.GetID(Cliente))
                        PraticheInCorso.Add(p);
                }

                foreach (CPraticaCQSPD p in info.PraticheConcluse)
                {
                    if (p.IDCliente == DBUtils.GetID(Cliente))
                        PraticheConcluse.Add(p);
                }
            }

            public CVisita GetVisitaPrecedente(Anagrafica.CPersona cliente, DateTime data, CCollection<CVisita> cache)
            {
                CVisita lastV = default;
                int idCliente = DBUtils.GetID(cliente);
                foreach (CVisita v in cache)
                {
                    if (v.IDPersona == idCliente)
                    {
                        if (v.Data > data)
                            break;
                        lastV = v;
                    }
                }

                if (lastV is null)
                {
                    using (var cursor = new CVisiteCursor())
                    {
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.Ricevuta.Value = true;
                        cursor.Data.Value = data;
                        cursor.Data.Operator = Databases.OP.OP_LE;
                        cursor.IDPersona.Value = idCliente;
                        lastV = cursor.Item;
                    }
                    if (lastV is object)
                    {
                        cache.Add(lastV);
                        cache.Sort();
                    }
                }

                return lastV;
            }

            ~CQSPSituazioneUfficioClienteInfo()
            {
                DMDObject.DecreaseCounter(this);
            }
        }

        [Serializable]
        public class CQSPSituazioneUfficio
            : IDMDXMLSerializable
        {
            private const int INCLUDIVISITEFINOAGG = 180; // Include le visite degli ultimi 30 giorni
            [NonSerialized] private Anagrafica.CUfficio m_Ufficio = null;
            private int m_IDUfficio = 0;

            // Public Ops As CCollection(Of CQSPSituazioneUfficioOp)
            public CCollection<CVisita> VisiteRicevute;
            public CCollection<CRichiestaFinanziamento> RichiestePendenti;
            public CCollection<CQSPDConsulenza> ConsulenzePendenti;
            public CCollection<CPraticaCQSPD> PraticheInCorso;
            public CCollection<CPraticaCQSPD> PraticheConcluse;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CQSPSituazioneUfficio()
            {
                DMDObject.IncreaseCounter(this);
                m_Ufficio = null;
                m_IDUfficio = 0;

                // Me.Ops = New CCollection(Of CQSPSituazioneUfficioOp)
                VisiteRicevute = new CCollection<CVisita>();
                RichiestePendenti = new CCollection<CRichiestaFinanziamento>();
                ConsulenzePendenti = new CCollection<CQSPDConsulenza>();
                PraticheInCorso = new CCollection<CPraticaCQSPD>();
                PraticheConcluse = new CCollection<CPraticaCQSPD>();
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="ufficio"></param>
            public CQSPSituazioneUfficio(Anagrafica.CUfficio ufficio) : this()
            {
                m_Ufficio = ufficio;
                m_IDUfficio = DBUtils.GetID(ufficio);
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'ufficio
            /// </summary>
            public int IDUfficio
            {
                get
                {
                    return DBUtils.GetID(m_Ufficio, m_IDUfficio);
                }

                set
                {
                    if (IDUfficio == value)
                        return;
                    m_IDUfficio = value;
                    m_Ufficio = null;
                }
            }

            /// <summary>
            /// Restituisce o imposta un riferimento all'ufficio
            /// </summary>
            public Anagrafica.CUfficio Ufficio
            {
                get
                {
                    if (m_Ufficio is null)
                        m_Ufficio = Anagrafica.Uffici.GetItemById(m_IDUfficio);
                    return m_Ufficio;
                }

                set
                {
                    m_Ufficio = value;
                    m_IDUfficio = DBUtils.GetID(value);
                }
            }

            /// <summary>
            /// Carica le statistiche
            /// </summary>
            /// <param name="dataInizio"></param>
            /// <param name="dataFine"></param>
            public void Load(DateTime? dataInizio, DateTime? dataFine)
            {
                var d1 = DMD.DateUtils.DateAdd(DateTimeInterval.Day, -INCLUDIVISITEFINOAGG, DMD.DateUtils.ToDay());
                var d2 = DMD.DateUtils.DateAdd(DateTimeInterval.Second, 24 * 3600 - 1, DMD.DateUtils.ToDay());
                VisiteRicevute = CustomerCalls.Visite.GetVisiteRicevute(Ufficio, default, d1, d2);
                RichiestePendenti = RichiesteFinanziamento.GetRichiestePendenti(Ufficio, null, d1, d2);
                ConsulenzePendenti = Consulenze.GetConsulenzeInCorso(Ufficio, null, d2, d2);
                PraticheInCorso = Pratiche.GetPraticheInCorso(Ufficio, null, default, default);
                PraticheConcluse = Pratiche.GetPraticheInCorso(Ufficio, null, dataInizio, dataFine);
                Prepara();
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDUfficio":
                        {
                            m_IDUfficio = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "VisiteRicevute":
                        {
                            VisiteRicevute.Clear();
                            VisiteRicevute.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "RichiestePendenti":
                        {
                            RichiestePendenti.Clear();
                            RichiestePendenti.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "ConsulenzePendenti":
                        {
                            ConsulenzePendenti.Clear();
                            ConsulenzePendenti.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "PraticheInCorso":
                        {
                            PraticheInCorso.Clear();
                            PraticheInCorso.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "PraticheConcluse":
                        {
                            PraticheConcluse.Clear();
                            PraticheConcluse.AddRange((IEnumerable)fieldValue);
                            break;
                        }
                }
            }

            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDUfficio", IDUfficio);
                writer.WriteTag("VisiteRicevute", VisiteRicevute);
                writer.WriteTag("RichiestePendenti", RichiestePendenti);
                writer.WriteTag("ConsulenzePendenti", ConsulenzePendenti);
                writer.WriteTag("PraticheInCorso", PraticheInCorso);
                writer.WriteTag("PraticheConcluse", PraticheConcluse);
            }

            public CCollection<Anagrafica.CPersona> GetClienti()
            {
                var clienti = new CKeyCollection<Anagrafica.CPersona>();
                foreach (CVisita v in VisiteRicevute)
                {
                    if (v.IDPersona != 0 && clienti.GetItemByKey("K" + v.IDPersona) is null && v.Persona is object)
                    {
                        clienti.Add("K" + v.IDPersona, v.Persona);
                    }
                }

                foreach (CRichiestaFinanziamento r in RichiestePendenti)
                {
                    if (r.IDCliente != 0 && clienti.GetItemByKey("K" + r.IDCliente) is null && r.Cliente is object)
                    {
                        clienti.Add("K" + r.IDCliente, r.Cliente);
                    }
                }

                foreach (CQSPDConsulenza c in ConsulenzePendenti)
                {
                    if (c.IDCliente != 0 && clienti.GetItemByKey("K" + c.IDCliente) is null && c.Cliente is object)
                    {
                        clienti.Add("K" + c.IDCliente, c.Cliente);
                    }
                }

                foreach (CPraticaCQSPD p in PraticheInCorso)
                {
                    if (p.IDCliente != 0 && clienti.GetItemByKey("K" + p.IDCliente) is null && p.Cliente is object)
                    {
                        clienti.Add("K" + p.IDCliente, p.Cliente);
                    }
                }

                foreach (CPraticaCQSPD p in PraticheConcluse)
                {
                    if (p.IDCliente != 0 && clienti.GetItemByKey("K" + p.IDCliente) is null && p.Cliente is object)
                    {
                        clienti.Add("K" + p.IDCliente, p.Cliente);
                    }
                }

                return new CCollection<Anagrafica.CPersona>(clienti);
            }

            public void Prepara()
            {
                // Me.Ops = New CCollection(Of CQSPSituazioneUfficioOp)

                // Ordiniamo le visite per data
                VisiteRicevute.Sort();

                // Dim clienti As CCollection(Of CPersona) = Me.GetClienti
                // Dim cliente As CPersona
                // Dim v As CVisita
                // Dim r As CRichiestaFinanziamento
                // Dim c As CQSPDConsulenza
                // Dim p As CPraticaCQSPD


                // Dim visiteXcliente As New CKeyCollection(Of CCollection(Of CVisita))
                // Dim richiesteXvisita As New CKeyCollection(Of CCollection(Of CRichiestaFinanziamento))
                // Dim consulenzeXRichiesta As New CKeyCollection(Of CCollection(Of CQSPDConsulenza))
                // Dim praticheXconsulenza As New CKeyCollection(Of CCollection(Of CPraticaCQSPD))


                // For Each cliente In clienti
                // For Each r In Me.RichiestePendenti
                // If (r.IDCliente = GetID(cliente)) Then
                // v = Nothing
                // If (r.TipoContesto = "CVisita") Then
                // v = Me.VisiteRicevute.GetItemById(r.IDContesto)
                // If (v Is Nothing) Then
                // v = CustomerCalls.Visite.GetItemById(r.IDContesto)
                // If (v IsNot Nothing) Then
                // Me.VisiteRicevute.Add(v)
                // Me.VisiteRicevute.Sort()
                // End If
                // End If
                // End If
                // If (v Is Nothing) Then
                // v = Me.GetVisitaPrecedente(cliente, r.Data, Me.VisiteRicevute)
                // End If
                // End If
                // Next
                // Next

                // 'For Each v In Me.VisiteRicevute
                // '    Dim richieste As CCollection(Of CRichiestaFinanziamento) = richiesteXvisita.GetItemByKey("K" & GetID(v))
                // '    If (richieste Is Nothing) Then
                // '        richieste = New CCollection(Of CRichiestaFinanziamento)
                // '        richiesteXvisita.Add("K" & GetID(v), richieste)
                // '    End If
                // '    For Each r In Me.RichiestePendenti
                // '        If r.
                // '    Next Then
                // 'Next

            }

            public void Sincronizza()
            {
                // For Each e As CEstinzione In Me.AltriPrestiti
                // 'e.SetPersona(Me.Persona)
                // e.SetPratica(Me.PraticheInCorso.GetItemById(e.IDPratica))
                // Next
                // For Each ep As EstinzioneXEstintore In Me.Estinzioni
                // ep.SetEstinzione(Me.AltriPrestiti.GetItemById(ep.IDEstinzione))
                // Next
                // 'For Each rich As CRichiestaFinanziamento In Me.RichiesteDiFinanziamento
                // '    rich.m_(Me.Persona)
                // 'Next
                // For Each studiof As CQSPDStudioDiFattibilita In Me.StudiDiFattibilita
                // 'studiof.SetCliente(Me.Persona)
                // studiof.SetRichiesta(Me.RichiesteDiFinanziamento.GetItemById(studiof.IDRichiesta))
                // Next
                // For Each cons As CQSPDConsulenza In Me.Consulenze
                // cons.SetCliente(Me.Persona)
                // cons.SetRichiesta(Me.RichiesteDiFinanziamento.GetItemById(cons.IDRichiesta))
                // cons.SetStudioDiFattibilita(Me.StudiDiFattibilita.GetItemById(cons.IDStudioDiFattibilita))
                // cons.SetRichiestaApprovazione(Me.RichiesteApprovazione.GetItemById(cons.IDRichiestaApprovazione))
                // cons.SetOffertaCQS(Me.Offerte.GetItemById(cons.IDOffertaCQS))
                // cons.SetOffertaPD(Me.Offerte.GetItemById(cons.IDOffertaPD))
                // Dim col As New CEstinzioniXEstintoreCollection
                // col.SetEstintore(cons)
                // For Each ep In Me.Estinzioni
                // If ep.TipoEstintore = "CQSPDConsulenza" AndAlso ep.IDEstintore = GetID(cons) Then
                // ep.SetEstintore(cons)
                // col.Add(ep)
                // End If
                // Next
                // cons.SetEstinzioni(col)
                // Next
                // For Each off As COffertaCQS In Me.Offerte
                // off.SetCliente(Me.Persona)
                // off.SetPratica(Me.Pratiche.GetItemById(off.IDPratica))
                // Next
                // For Each stl As CStatoLavorazionePratica In Me.StatiDiLavorazione
                // stl.SetOfferta(Me.Offerte.GetItemById(stl.IDOfferta))
                // Next
                // For Each prat As CPraticaCQSPD In Me.Pratiche
                // prat.SetCliente(Me.Persona)
                // prat.SetConsulenza(Me.Consulenze.GetItemById(prat.IDConsulenza))
                // prat.SetRichiestaDiFinanziamento(Me.RichiesteDiFinanziamento.GetItemById(prat.IDRichiestaDiFinanziamento))
                // prat.SetRichiestaApprovazione(Me.RichiesteApprovazione.GetItemById(prat.IDRichiestaApprovazione))
                // prat.SetOffertaCorrente(Me.Offerte.GetItemById(prat.IDOffertaCorrente))
                // prat.SetStatoDiLavorazioneAttuale(Me.StatiDiLavorazione.GetItemById(prat.IDStatoDiLavorazioneAttuale))
                // Dim col As New CStatiLavorazionePraticaCollection
                // col.SetPratica(prat)
                // For Each stl As CStatoLavorazionePratica In Me.StatiDiLavorazione
                // If stl.IDPratica = GetID(prat) Then
                // stl.SetPratica(prat)
                // col.Add(stl)
                // End If
                // Next
                // col.Sort()
                // prat.SetStatiDiLavorazione(col)

                // Dim col1 As New CEstinzioniXEstintoreCollection
                // col1.SetEstintore(prat)
                // For Each ep In Me.Estinzioni
                // If ep.TipoEstintore = "CPraticaCQSPD" AndAlso ep.IDEstintore = GetID(prat) Then
                // ep.SetEstintore(prat)
                // col1.Add(ep)
                // End If
                // Next
                // prat.SetEstinzioni(col1)
                // Next
            }

            ~CQSPSituazioneUfficio()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}