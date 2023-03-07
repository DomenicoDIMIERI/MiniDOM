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
       
        [Serializable]
        public class FinestraLavorazioneXML 
            : IDMDXMLSerializable
        {
            public FinestraLavorazione W;
            private CCollection<CStatoLavorazionePratica> StatiLavorazione;
            private CCollection<COffertaCQS> Offerte;

            public FinestraLavorazioneXML()
            {
                DMDObject.IncreaseCounter(this);
                W = null;
                StatiLavorazione = new CCollection<CStatoLavorazionePratica>();
                Offerte = new CCollection<COffertaCQS>();
            }

            public FinestraLavorazioneXML(FinestraLavorazione w) : this()
            {
                if (w is null)
                    throw new ArgumentNullException("w");
                W = w;
            }

            public void Prepara()
            {
                StatiLavorazione.Clear();
                Offerte.Clear();
                AggiungiOfferteSF(W.StudioDiFattibilita);
                AggiungiOfferteP(W.CQS);
                AggiungiOfferteP(W.PD);
                AggiungiOfferteP(W.CQSI);
                AggiungiOfferteP(W.PDI);
            }

            public void Sincronizza()
            {
                SincronizzaSF(W.StudioDiFattibilita);
                SincronizzaP(W.CQS);
                SincronizzaP(W.PD);
                SincronizzaP(W.CQSI);
                SincronizzaP(W.PDI);
            }

            private void SincronizzaSF(CQSPDConsulenza sf)
            {
                if (sf is null)
                    return;
                sf.SetOffertaCQS(Offerte.GetItemById(sf.IDOffertaCQS));
                sf.SetOffertaPD(Offerte.GetItemById(sf.IDOffertaPD));
            }

            private void SincronizzaP(CPraticaCQSPD p)
            {
                if (p is null)
                    return;
                var col = new CStatiLavorazionePraticaCollection();
                foreach (var stl in StatiLavorazione)
                {
                    if (stl.IDPratica == DBUtils.GetID(p))
                    {
                        stl.SetPratica(p);
                        stl.SetOfferta(Offerte.GetItemById(stl.IDOfferta));
                        col.Add(stl);
                    }
                }

                col.SetPratica(p);
                col.Sort();
                p.SetStatiDiLavorazione(col);
            }

            private void AggiungiOfferteSF(CQSPDConsulenza sf)
            {
                if (sf is null)
                    return;
                if (Offerte.GetItemById(sf.IDOffertaCQS) is null && sf.OffertaCQS is object)
                    Offerte.Add(sf.OffertaCQS);
                if (Offerte.GetItemById(sf.IDOffertaPD) is null && sf.OffertaPD is object)
                    Offerte.Add(sf.OffertaPD);
            }

            private void AggiungiOfferteP(CPraticaCQSPD p)
            {
                if (p is null)
                    return;
                foreach (CStatoLavorazionePratica sl in p.StatiDiLavorazione)
                {
                    if (StatiLavorazione.GetItemById(DBUtils.GetID(sl)) is null)
                        StatiLavorazione.Add(sl);
                    if (Offerte.GetItemById(sl.IDOfferta) is null && sl.Offerta is object)
                        Offerte.Add(sl.Offerta);
                }
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "W":
                        {
                            W = (FinestraLavorazione)fieldValue;
                            break;
                        }

                    case "RF":
                        {
                            W.SetRichiestaFinanziamento((CRichiestaFinanziamento)fieldValue);
                            break;
                        }

                    case "SF":
                        {
                            W.SetStudioDiFattibilita((CQSPDConsulenza)fieldValue);
                            break;
                        }

                    case "CQS":
                        {
                            W.SetCQS((CPraticaCQSPD)fieldValue);
                            break;
                        }

                    case "PD":
                        {
                            W.SetPD((CPraticaCQSPD)fieldValue);
                            break;
                        }

                    case "CQSI":
                        {
                            W.SetCQSI((CPraticaCQSPD)fieldValue);
                            break;
                        }

                    case "PDI":
                        {
                            W.SetPDI((CPraticaCQSPD)fieldValue);
                            break;
                        }

                    case "Offerte":
                        {
                            Offerte.Clear();
                            Offerte.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "STLP":
                        {
                            StatiLavorazione.Clear();
                            StatiLavorazione.AddRange((IEnumerable)fieldValue);
                            break;
                        }
                }
            }

            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
            {
                writer.WriteTag("W", W);
                writer.WriteTag("RF", W.RichiestaFinanziamento);
                writer.WriteTag("SF", W.StudioDiFattibilita);
                writer.WriteTag("CQS", W.CQS);
                writer.WriteTag("PD", W.PD);
                writer.WriteTag("CQSI", W.CQSI);
                writer.WriteTag("PDI", W.PDI);
                writer.WriteTag("STLP", StatiLavorazione);
                writer.WriteTag("Offerte", Offerte);
            }

            ~FinestraLavorazioneXML()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}