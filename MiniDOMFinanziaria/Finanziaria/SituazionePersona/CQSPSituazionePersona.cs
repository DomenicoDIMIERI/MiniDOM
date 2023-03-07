using System;
using System.Collections;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CQSPSituazionePersona : IDMDXMLSerializable
        {
            private Anagrafica.CPersona m_Persona = null;
            private int m_IDPersona = 0;
            public CCollection<CEstinzione> AltriPrestiti;
            public CCollection<CRichiestaConteggio> RichiesteConteggi;
            public CCollection<CRichiestaFinanziamento> RichiesteDiFinanziamento;
            public CCollection<CQSPDStudioDiFattibilita> StudiDiFattibilita;
            public CCollection<CQSPDConsulenza> Consulenze;
            public CCollection<COffertaCQS> Offerte;
            public CCollection<CPraticaCQSPD> Pratiche;
            // Public StatiDiLavorazione As CCollection(Of CStatoLavorazionePratica)
            public CCollection<CRichiestaApprovazione> RichiesteApprovazione;
            public CCollection<EstinzioneXEstintore> Estinzioni;
            public CCollection<FinestraLavorazione> FinestreLavorazione;

            public CQSPSituazionePersona()
            {
                DMDObject.IncreaseCounter(this);
                m_Persona = null;
                m_IDPersona = 0;
                RichiesteConteggi = new CCollection<CRichiestaConteggio>();
                AltriPrestiti = new CCollection<CEstinzione>();
                RichiesteDiFinanziamento = new CCollection<CRichiestaFinanziamento>();
                StudiDiFattibilita = new CCollection<CQSPDStudioDiFattibilita>();
                Consulenze = new CCollection<CQSPDConsulenza>();
                Offerte = new CCollection<COffertaCQS>();
                Pratiche = new CCollection<CPraticaCQSPD>();
                // Me.StatiDiLavorazione = New CCollection(Of CStatoLavorazionePratica)
                RichiesteApprovazione = new CCollection<CRichiestaApprovazione>();
                Estinzioni = new CCollection<EstinzioneXEstintore>();
                FinestreLavorazione = new CCollection<FinestraLavorazione>();
            }

            public CQSPSituazionePersona(Anagrafica.CPersona persona) : this()
            {
                m_Persona = persona;
                m_IDPersona = DBUtils.GetID(persona);
            }

            public int IDPersona
            {
                get
                {
                    return DBUtils.GetID(m_Persona, m_IDPersona);
                }

                set
                {
                    if (IDPersona == value)
                        return;
                    m_IDPersona = value;
                    m_Persona = null;
                }
            }

            public Anagrafica.CPersona Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = Anagrafica.Persone.GetItemById(m_IDPersona);
                    return m_Persona;
                }

                set
                {
                    m_Persona = value;
                    m_IDPersona = DBUtils.GetID(value);
                }
            }

            public void Load()
            {
                AltriPrestiti = Finanziaria.Estinzioni.GetEstinzioniByPersona(Persona);
                RichiesteDiFinanziamento = RichiesteFinanziamento.GetRichiesteByPersona(Persona);
                RichiesteConteggi = Finanziaria.RichiesteConteggi.GetRichiesteByPersona(Persona);
                Consulenze = Finanziaria.Consulenze.GetConsulenzeByPersona(Persona);
                // Me.Offerte = Finanziaria.Offerte.GetOfferteByPersona(Me.Persona)
                Pratiche = Finanziaria.Pratiche.GetPraticheByPersona((Anagrafica.CPersonaFisica)Persona);
                StudiDiFattibilita = Finanziaria.StudiDiFattibilita.GetStudiDiFattibilitaByPersona(Persona);
                FinestreLavorazione = FinestreDiLavorazione.GetFinestreByPersona(Persona);
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDPersona":
                        {
                            m_IDPersona = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "AltriPrestiti":
                        {
                            AltriPrestiti.Clear();
                            AltriPrestiti.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "RichiesteDiFinanziamento":
                        {
                            RichiesteDiFinanziamento.Clear();
                            RichiesteDiFinanziamento.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "StudiDiFattibilita":
                        {
                            StudiDiFattibilita.Clear();
                            StudiDiFattibilita.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Consulenze":
                        {
                            Consulenze.Clear();
                            Consulenze.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Offerte":
                        {
                            Offerte.Clear();
                            Offerte.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Pratiche":
                        {
                            Pratiche.Clear();
                            Pratiche.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "RichiesteConteggi":
                        {
                            RichiesteConteggi.Clear();
                            RichiesteConteggi.AddRange((IEnumerable)fieldValue);
                            break;
                        }
                    // Case "StatiDiLavorazione" : Me.StatiDiLavorazione.Clear() : Me.StatiDiLavorazione.AddRange(fieldValue)
                    case "RichiesteApprovazione":
                        {
                            RichiesteApprovazione.Clear();
                            RichiesteApprovazione.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Estinzioni":
                        {
                            Estinzioni.Clear();
                            Estinzioni.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "FinestreLavorazione":
                        {
                            FinestreLavorazione.Clear();
                            FinestreLavorazione.AddRange((IEnumerable)fieldValue);
                            break;
                        }
                }
            }

            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
            {
                Prepara();
                Controlla();
                // writer.Settings.SetValueBool("CPraticaCQSPD_SerializeStatiLavorazione", False)
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteTag("AltriPrestiti", AltriPrestiti);
                writer.WriteTag("Estinzioni", Estinzioni);
                writer.WriteTag("RichiesteDiFinanziamento", RichiesteDiFinanziamento);
                writer.WriteTag("StudiDiFattibilita", StudiDiFattibilita);
                writer.WriteTag("Consulenze", Consulenze);
                writer.WriteTag("Offerte", Offerte);
                writer.WriteTag("Pratiche", Pratiche);
                // writer.WriteTag("StatiDiLavorazione", Me.StatiDiLavorazione)
                writer.WriteTag("RichiesteApprovazione", RichiesteApprovazione);
                writer.WriteTag("FinestreLavorazione", FinestreLavorazione);
                writer.WriteTag("RichiesteConteggi", RichiesteConteggi);
            }

            public void Prepara()
            {
                foreach (CRichiestaConteggio ricc in RichiesteConteggi)
                {
                    if (FinestreLavorazione.GetItemById(ricc.IDFinestraLavorazione) is null && ricc.FinestraLavorazione is object)
                        FinestreLavorazione.Add(ricc.FinestraLavorazione);
                }

                foreach (CRichiestaFinanziamento rich in RichiesteDiFinanziamento)
                {
                    if (FinestreLavorazione.GetItemById(rich.IDFinestraLavorazione) is null && rich.FinestraLavorazione is object)
                        FinestreLavorazione.Add(rich.FinestraLavorazione);
                }

                foreach (CQSPDConsulenza cons in Consulenze)
                {
                    foreach (EstinzioneXEstintore e in cons.Estinzioni)
                    {
                        if (AltriPrestiti.GetItemById(e.IDEstinzione) is null && e.Estinzione is object)
                            AltriPrestiti.Add(e.Estinzione);
                        Estinzioni.Add(e);
                    }

                    if (StudiDiFattibilita.GetItemById(cons.IDStudioDiFattibilita) is null && cons.StudioDiFattibilita is object)
                        StudiDiFattibilita.Add(cons.StudioDiFattibilita);
                    if (Offerte.GetItemById(cons.IDOffertaCQS) is null && cons.OffertaCQS is object)
                        Offerte.Add(cons.OffertaCQS);
                    if (Offerte.GetItemById(cons.IDOffertaPD) is null && cons.OffertaPD is object)
                        Offerte.Add(cons.OffertaPD);
                    if (RichiesteDiFinanziamento.GetItemById(cons.IDRichiesta) is null && cons.Richiesta is object)
                        RichiesteDiFinanziamento.Add(cons.Richiesta);
                    if (RichiesteApprovazione.GetItemById(cons.IDRichiestaApprovazione) is null && cons.RichiestaApprovazione is object)
                        RichiesteApprovazione.Add(cons.RichiestaApprovazione);
                    if (FinestreLavorazione.GetItemById(cons.IDFinestraLavorazione) is null && cons.FinestraLavorazione is object)
                        FinestreLavorazione.Add(cons.FinestraLavorazione);
                }

                foreach (CPraticaCQSPD prat in Pratiche)
                {
                    foreach (EstinzioneXEstintore e in prat.Estinzioni)
                    {
                        if (AltriPrestiti.GetItemById(e.IDEstinzione) is null && e.Estinzione is object)
                            AltriPrestiti.Add(e.Estinzione);
                        Estinzioni.Add(e);
                    }

                    if (Offerte.GetItemById(prat.IDOffertaCorrente) is null && prat.OffertaCorrente is object)
                        Offerte.Add(prat.OffertaCorrente);
                    if (RichiesteDiFinanziamento.GetItemById(prat.IDRichiestaApprovazione) is null && prat.RichiestaApprovazione is object)
                        RichiesteApprovazione.Add(prat.RichiestaApprovazione);
                    // If Me.StatiDiLavorazione.GetItemById(prat.IDStatoDiLavorazioneAttuale) Is Nothing AndAlso prat.StatoDiLavorazioneAttuale IsNot Nothing Then Me.StatiDiLavorazione.Add(prat.StatoDiLavorazioneAttuale)
                    foreach (CStatoLavorazionePratica stl in prat.StatiDiLavorazione)
                    {
                        // Me.StatiDiLavorazione.Add(stl)
                        if (Offerte.GetItemById(stl.IDOfferta) is null && stl.Offerta is object)
                            Offerte.Add(stl.Offerta);
                    }

                    if (FinestreLavorazione.GetItemById(prat.IDFinestraLavorazione) is null && prat.FinestraLavorazione is object)
                        FinestreLavorazione.Add(prat.FinestraLavorazione);
                }
            }

            private void Controlla()
            {
                Sincronizza();
                foreach (CPraticaCQSPD p in Pratiche)
                {
                    if (p.RichiestaDiFinanziamento is null)
                        p.RichiestaDiFinanziamento = FindRichiesta(p);
                    if (p.Consulenza is null)
                        p.Consulenza = FindConsulenza(p);
                    if (p.IsChanged())
                        p.Save();
                }

                foreach (CQSPDConsulenza c in Consulenze)
                {
                    if (c.StudioDiFattibilita is null)
                        c.StudioDiFattibilita = FindSF(c);
                    if (c.Richiesta is null)
                        c.Richiesta = FindRichiesta(c);
                    if (c.IsChanged())
                        c.Save();
                }

                foreach (CQSPDStudioDiFattibilita s in StudiDiFattibilita)
                {
                    if (s.Richiesta is null)
                        s.Richiesta = FindRichiesta(s);
                    if (s.IsChanged())
                        s.Save();
                }
            }

            private CQSPDConsulenza FindConsulenza(CPraticaCQSPD p)
            {
                if (p.FinestraLavorazione is object && p.FinestraLavorazione.StudioDiFattibilita is object)
                    return p.FinestraLavorazione.StudioDiFattibilita;
                var dp = p.DataDecorrenza;
                CQSPDConsulenza c = null;
                if (dp.HasValue == false)
                    dp = p.CreatoIl;
                foreach (var currentC in Consulenze)
                {
                    c = currentC;
                    var dc = c.DataConsulenza;
                    if (dc.HasValue == false)
                        dc = c.CreatoIl;
                    int diff = (int)DMD.DateUtils.DateDiff(DateTimeInterval.Day, dc.Value, dp.Value);
                    if (diff >= 0 && diff <= 30)
                        return c;
                }

                c = new CQSPDConsulenza();
                c.Cliente = (Anagrafica.CPersonaFisica)Persona;
                // If p.Prodotto IsNot Nothing AndAlso p.Prodotto.IdTipoContratto = "D" Then
                // c.OffertaPD = p.OffertaCorrente
                // Else
                // c.OffertaCQS = p.OffertaCorrente
                // End If
                c.Stato = ObjectStatus.OBJECT_VALID;
                c.DataConsulenza = dp.Value;
                c.DataConferma = dp.Value;
                // c.DataDecorrenza = p.DataDecorrenza
                c.DataProposta = dp.Value;
                c.Descrizione = "Consulenza Creata automaticamente dalla pratica " + p.NumeroPratica;
                c.FinestraLavorazione = p.FinestraLavorazione;
                c.Eta = (float)Persona.get_Eta(dp.Value);
                c.Consulente = p.Consulente;
                c.Azienda = p.Impiego.Azienda;
                c.DataAssunzione = p.Impiego.DataAssunzione;
                c.Richiesta = p.RichiestaDiFinanziamento;
                c.PuntoOperativo = p.PuntoOperativo;
                c.StatoConsulenza = StatiConsulenza.ACCETTATA;
                c.Save();
                Consulenze.Add(c);
                return c;
            }

            private CQSPDStudioDiFattibilita FindSF(CQSPDConsulenza c)
            {
                var dp = c.DataConsulenza;
                if (dp.HasValue == false)
                    dp = c.CreatoIl;
                CQSPDStudioDiFattibilita s = null;
                foreach (var currentS in StudiDiFattibilita)
                {
                    s = currentS;
                    var dc = s.Data;
                    int diff = (int)DMD.DateUtils.DateDiff(DateTimeInterval.Day, dc, dp.Value);
                    if (diff >= 0 && diff <= 30)
                        return s;
                }

                s = new CQSPDStudioDiFattibilita();
                s.Cliente = Persona;
                s.Stato = ObjectStatus.OBJECT_VALID;
                s.Data = dp.Value;
                s.PuntoOperativo = c.PuntoOperativo;
                s.Richiesta = c.Richiesta;
                s.Save();
                StudiDiFattibilita.Add(s);
                return s;
            }

            private CRichiestaFinanziamento FindRichiesta(CPraticaCQSPD p)
            {
                var dp = p.DataDecorrenza;
                CRichiestaFinanziamento r = null;
                if (dp.HasValue == false)
                    dp = p.CreatoIl;
                foreach (var currentR in RichiesteDiFinanziamento)
                {
                    r = currentR;
                    var dc = r.Data;
                    int diff = (int)DMD.DateUtils.DateDiff(DateTimeInterval.Day, dc, dp.Value);
                    if (diff >= 0 && diff <= 30)
                        return r;
                }

                r = new CRichiestaFinanziamento();
                r.Cliente = (Anagrafica.CPersonaFisica)Persona;
                r.Stato = ObjectStatus.OBJECT_VALID;
                r.Data = dp.Value;
                r.PuntoOperativo = p.PuntoOperativo;
                r.Note = "Richiesta Creata automaticamente dalla pratica " + p.NumeroPratica;
                r.FinestraLavorazione = p.FinestraLavorazione;
                r.StatoRichiesta = StatoRichiestaFinanziamento.EVASA;
                // r.AssegnatoA = p.Stato
                r.Save();
                RichiesteDiFinanziamento.Add(r);
                return r;
            }

            private CRichiestaFinanziamento FindRichiesta(CQSPDConsulenza c)
            {
                if (c.FinestraLavorazione is object && c.FinestraLavorazione.RichiestaFinanziamento is object)
                    return c.FinestraLavorazione.RichiestaFinanziamento;
                var dp = c.DataConsulenza;
                if (dp.HasValue == false)
                    dp = c.CreatoIl;
                CRichiestaFinanziamento r = null;
                foreach (var currentR in RichiesteDiFinanziamento)
                {
                    r = currentR;
                    var dc = r.Data;
                    int diff = (int)DMD.DateUtils.DateDiff(DateTimeInterval.Day, dc, dp.Value);
                    if (diff >= 0 && diff <= 30)
                        return r;
                }

                r = new CRichiestaFinanziamento();
                r.Cliente = (Anagrafica.CPersonaFisica)Persona;
                r.Stato = ObjectStatus.OBJECT_VALID;
                r.Data = dp.Value;
                r.PuntoOperativo = c.PuntoOperativo;
                r.Note = "Richiesta Creata automaticamente dalla consulenza del " + Sistema.Formats.FormatUserDateTime(dp);
                r.StatoRichiesta = StatoRichiestaFinanziamento.EVASA;
                r.Save();
                RichiesteDiFinanziamento.Add(r);
                return r;
            }

            private CRichiestaFinanziamento FindRichiesta(CQSPDStudioDiFattibilita s)
            {
                DateTime? dp = s.Data;
                if (dp.HasValue == false)
                    dp = s.CreatoIl;
                CRichiestaFinanziamento r = null;
                foreach (var currentR in RichiesteDiFinanziamento)
                {
                    r = currentR;
                    var dc = r.Data;
                    int diff = (int)DMD.DateUtils.DateDiff(DateTimeInterval.Day, dc, dp.Value);
                    if (diff >= 0 && diff <= 30)
                        return r;
                }

                r = new CRichiestaFinanziamento();
                r.Cliente = (Anagrafica.CPersonaFisica)Persona;
                r.Stato = ObjectStatus.OBJECT_VALID;
                r.Data = dp.Value;
                r.PuntoOperativo = s.PuntoOperativo;
                r.Note = "Richiesta Creata automaticamente lo studio di fattibilità del " + Sistema.Formats.FormatUserDateTime(dp);
                r.StatoRichiesta = StatoRichiestaFinanziamento.EVASA;
                r.Save();
                RichiesteDiFinanziamento.Add(r);
                return r;
            }

            public void Sincronizza()
            {
                foreach (CRichiestaConteggio ricc in RichiesteConteggi)
                {
                    ricc.SetCliente(Persona);
                    ricc.SetFinestraLavorazione(FinestreLavorazione.GetItemById(ricc.IDFinestraLavorazione));
                }

                foreach (CEstinzione e in AltriPrestiti)
                {
                    e.SetPersona(Persona);
                    e.SetPratica(Pratiche.GetItemById(e.IDPratica));
                }

                foreach (EstinzioneXEstintore ep in Estinzioni)
                    ep.SetEstinzione(AltriPrestiti.GetItemById(ep.IDEstinzione));
                foreach (CRichiestaFinanziamento rich in RichiesteDiFinanziamento)
                {
                    rich.SetCliente((Anagrafica.CPersonaFisica)Persona);
                    rich.SetFinestraLavorazione(FinestreLavorazione.GetItemById(rich.IDFinestraLavorazione));
                }

                foreach (CQSPDStudioDiFattibilita studiof in StudiDiFattibilita)
                {
                    studiof.SetCliente(Persona);
                    studiof.SetRichiesta(RichiesteDiFinanziamento.GetItemById(studiof.IDRichiesta));
                }

                foreach (CQSPDConsulenza cons in Consulenze)
                {
                    cons.SetCliente(Persona);
                    cons.SetRichiesta(RichiesteDiFinanziamento.GetItemById(cons.IDRichiesta));
                    cons.SetStudioDiFattibilita(StudiDiFattibilita.GetItemById(cons.IDStudioDiFattibilita));
                    cons.SetRichiestaApprovazione(RichiesteApprovazione.GetItemById(cons.IDRichiestaApprovazione));
                    cons.SetOffertaCQS(Offerte.GetItemById(cons.IDOffertaCQS));
                    cons.SetOffertaPD(Offerte.GetItemById(cons.IDOffertaPD));
                    cons.SetFinestraLavorazione(FinestreLavorazione.GetItemById(cons.IDFinestraLavorazione));
                    var col = new CEstinzioniXEstintoreCollection();
                    col.SetEstintore(cons);
                    foreach (var ep in Estinzioni)
                    {
                        if (ep.TipoEstintore == "CQSPDConsulenza" && ep.IDEstintore == DBUtils.GetID(cons))
                        {
                            ep.SetEstintore(cons);
                            col.Add(ep);
                        }
                    }

                    cons.SetEstinzioni(col);
                }

                foreach (COffertaCQS off in Offerte)
                {
                    off.SetCliente((Anagrafica.CPersonaFisica)Persona);
                    off.SetPratica(Pratiche.GetItemById(off.IDPratica));
                }
                // For Each stl As CStatoLavorazionePratica In Me.StatiDiLavorazione
                // stl.SetOfferta(Me.Offerte.GetItemById(stl.IDOfferta))
                // Next
                foreach (CPraticaCQSPD prat in Pratiche)
                {
                    prat.SetCliente((Anagrafica.CPersonaFisica)Persona);
                    prat.SetConsulenza(Consulenze.GetItemById(prat.IDConsulenza));
                    prat.SetRichiestaDiFinanziamento(RichiesteDiFinanziamento.GetItemById(prat.IDRichiestaDiFinanziamento));
                    prat.SetRichiestaApprovazione(RichiesteApprovazione.GetItemById(prat.IDRichiestaApprovazione));
                    prat.SetOffertaCorrente(Offerte.GetItemById(prat.IDOffertaCorrente));
                    // prat.SetStatoDiLavorazioneAttuale(Me.StatiDiLavorazione.GetItemById(prat.IDStatoDiLavorazioneAttuale))
                    prat.SetFinestraLavorazione(FinestreLavorazione.GetItemById(prat.IDFinestraLavorazione));
                    // Dim col As New CStatiLavorazionePraticaCollection
                    // col.SetPratica(prat)
                    foreach (CStatoLavorazionePratica stl in prat.StatiDiLavorazione)
                    {
                        // If stl.IDPratica = GetID(prat) Then
                        stl.SetPratica(prat);
                        stl.SetOfferta(Offerte.GetItemById(stl.IDOfferta));
                        // col.Add(stl)
                        // End If
                    }
                    // col.Sort()
                    // prat.SetStatiDiLavorazione(col)

                    var col1 = new CEstinzioniXEstintoreCollection();
                    col1.SetEstintore(prat);
                    foreach (var ep in Estinzioni)
                    {
                        if (ep.TipoEstintore == "CPraticaCQSPD" && ep.IDEstintore == DBUtils.GetID(prat))
                        {
                            ep.SetEstintore(prat);
                            col1.Add(ep);
                        }
                    }

                    prat.SetEstinzioni(col1);
                }

                foreach (FinestraLavorazione fl in FinestreLavorazione)
                {
                    var altriPrestiti = new CCollection<CEstinzione>();
                    foreach (CEstinzione ap in AltriPrestiti)
                    {
                        var di = fl.DataInizioLavorazione;
                        if (di.HasValue == false)
                            di = fl.DataInizioLavorabilita;
                        if (ap.IsInCorsoOFutura((DateTime)di))
                        {
                            altriPrestiti.Add(ap);
                        }
                    }

                    fl.SetAltriPrestiti(altriPrestiti);
                    fl.SetCQS(Pratiche.GetItemById(fl.IDCQS));
                    fl.SetPD(Pratiche.GetItemById(fl.IDPD));
                    fl.SetCQSI(Pratiche.GetItemById(fl.IDCQSI));
                    fl.SetPDI(Pratiche.GetItemById(fl.IDPDI));
                    fl.SetRichiestaFinanziamento(RichiesteDiFinanziamento.GetItemById(fl.IDRichiestaFinanziamento));
                    fl.SetStudioDiFattibilita(Consulenze.GetItemById(fl.IDStudioDiFattibilita));
                }
            }

            ~CQSPSituazionePersona()
            {
                DMDObject.DecreaseCounter(this);
            }

            public string[] CanDeleteAltroPrestito(CEstinzione e)
            {
                var ret = new ArrayList();
                if (e.Stato == ObjectStatus.OBJECT_VALID)
                {
                    foreach (EstinzioneXEstintore esti in Estinzioni)
                    {
                        if (esti.Stato == ObjectStatus.OBJECT_VALID && !string.IsNullOrEmpty(esti.TipoEstintore) && esti.IDEstintore != 0 && esti.IDEstinzione == DBUtils.GetID(e))
                        {
                            ret.Add(esti.TipoEstintore + "[" + esti.IDEstintore + "]");
                        }
                    }
                }

                return (string[])ret.ToArray(typeof(string));
            }
        }
    }
}