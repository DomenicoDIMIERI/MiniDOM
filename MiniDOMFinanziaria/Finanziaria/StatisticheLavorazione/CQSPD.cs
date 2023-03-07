using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CPraticaInfoLavorazione
            : IDMDXMLSerializable
        {
            public int m_IDPratica;
            [NonSerialized] public CPraticaCQSPD m_Pratica;
            public int IDPersona;
            [NonSerialized] public Anagrafica.CPersonaFisica Persona;
            public string NomePersona;
            public decimal Rata;
            public int Durata;
            public decimal NettoRicavo;
            public decimal NettoAllaMano;
            public float TAN;
            public float TAEG;

            public CPraticaInfoLavorazione()
            {
                m_IDPratica = 0;
                m_Pratica = null;
                IDPersona = 0;
                Persona = null;
                NomePersona = "";
                Rata = 0m;
                Durata = 0;
                NettoAllaMano = 0m;
                NettoRicavo = 0m;
                TAEG = 0f;
                TAN = 0f;
            }

            public CPraticaInfoLavorazione(CPraticaCQSPD p)
            {
                if (p is null)
                    throw new ArgumentNullException("p");
                m_IDPratica = DBUtils.GetID(p);
                m_Pratica = p;
                Persona = p.Cliente;
                IDPersona = DBUtils.GetID(Persona);
                NomePersona = Persona.Nominativo;
                Rata = p.OffertaCorrente.Rata;
                Durata = p.OffertaCorrente.Durata;
                NettoRicavo = p.OffertaCorrente.NettoRicavo;
                // Me.m_NettoAllaMano = p.OffertaCorrente.NettoRicavo - p.altr
                TAN = (float)p.OffertaCorrente.TAN;
                TAEG = (float)p.OffertaCorrente.TAEG;
            }

            public int IDPratica
            {
                get
                {
                    return DBUtils.GetID(m_Pratica, m_IDPratica);
                }

                set
                {
                    if (IDPratica == value)
                        return;
                    m_IDPratica = value;
                    m_Pratica = null;
                }
            }

            public CPraticaCQSPD Pratica
            {
                get
                {
                    if (m_Pratica is null)
                        m_Pratica = Pratiche.GetItemById(m_IDPratica);
                    return m_Pratica;
                }

                set
                {
                    m_Pratica = value;
                    m_IDPratica = DBUtils.GetID(value);
                }
            }

            public void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDPratica":
                        {
                            m_IDPratica = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDPersona":
                        {
                            IDPersona = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePersona":
                        {
                            NomePersona = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Rata":
                        {
                            Rata = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Durata":
                        {
                            Durata = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NettoRicavo":
                        {
                            NettoRicavo = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "NettoAllaMano":
                        {
                            NettoAllaMano = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TAN":
                        {
                            TAN = (float)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "TAEG":
                        {
                            TAEG = (float)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }
                }
            }

            public void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDPratica", IDPratica);
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteAttribute("NomePersona", NomePersona);
                writer.WriteAttribute("Rata", Rata);
                writer.WriteAttribute("Durata", Durata);
                writer.WriteAttribute("NettoRicavo", NettoRicavo);
                writer.WriteAttribute("NettoAllaMano", NettoAllaMano);
                writer.WriteAttribute("TAN", TAN);
                writer.WriteAttribute("TAEG", TAEG);
            }
        }

        [Serializable]
        public class CQSPDSTXANNSTATITEM 
            : IComparable, IDMDXMLSerializable
        {
            public int Anno;
            public int[] Conteggio;
            public decimal[] ML;

            public CQSPDSTXANNSTATITEM()
            {
                Anno = 0;
                Conteggio = new int[13];
                ML = new decimal[13];
            }

            public int CompareTo(object obj)
            {
                CQSPDSTXANNSTATITEM tmp = (CQSPDSTXANNSTATITEM)obj;
                int ret = Anno - tmp.Anno;
                return ret;
            }

            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Anno":
                        {
                            Anno = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Conteggio":
                        {
                            Conteggio = (int[])DMD.XML.Utils.Serializer.ToArray<int>(fieldValue);
                            break;
                        }

                    case "ML":
                        {
                            ML = (decimal[])DMD.XML.Utils.Serializer.ToArray<int>(fieldValue);
                            break;
                        }
                }
            }

            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Anno", Anno);
                writer.WriteTag("Conteggio", Conteggio);
                writer.WriteTag("ML", ML);
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue);  }
            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer);  }
        }


        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public enum InfoStatoEnum : int
        {
            CONTATTO = 0,
            LIQUIDATO = 1,
            ANNULLATO = 2,
            ALTRO = 3
        }

        public class InfoStato 
            : IDMDXMLSerializable
        {
            public string Descrizione;
            public InfoStatoEnum stato;
            public int Conteggio;
            public decimal Montante;

            public InfoStato(string descrizione, InfoStatoEnum stato, int conteggio, decimal montante)
            {
                DMDObject.IncreaseCounter(this);
                Descrizione = descrizione;
                this.stato = stato;
                Conteggio = conteggio;
                Montante = montante;
            }

            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Descrizione":
                        {
                            Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Stato":
                        {
                            stato = (InfoStatoEnum)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Conteggio":
                        {
                            Conteggio = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Montante":
                        {
                            Montante = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }
                }
            }

            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Descrizione", Descrizione);
                writer.WriteAttribute("Stato", (int?)stato);
                writer.WriteAttribute("Conteggio", Conteggio);
                writer.WriteAttribute("Montante", Montante);
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue); }
            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer); }

            ~InfoStato()
            {
                DMDObject.DecreaseCounter(this);
            }
        }


        public class CompareByAnnullato
            : IComparer, IComparer<CRigaStatisticaPerStato>
        {
            public CompareByAnnullato()
            {
                DMDObject.IncreaseCounter(this);
            }

            ~CompareByAnnullato()
            {
                DMDObject.DecreaseCounter(this);
            }

            public int Compare(CRigaStatisticaPerStato x, CRigaStatisticaPerStato y)
            {
                decimal v1 = x.get_Item(InfoStatoEnum.ANNULLATO).Montante;
                decimal v2 = y.get_Item(InfoStatoEnum.ANNULLATO).Montante;
                if (v1 < v2)
                    return 1;
                if (v1 > v2)
                    return -1;
                return 0;
            }

            int IComparer.Compare(object x, object y)
            {
                return Compare((CRigaStatisticaPerStato)x, (CRigaStatisticaPerStato)y);
            }
        }

        public class CompareByContatto : IComparer, IComparer<CRigaStatisticaPerStato>
        {
            public CompareByContatto()
            {
                DMDObject.IncreaseCounter(this);
            }

            ~CompareByContatto()
            {
                DMDObject.DecreaseCounter(this);
            }

            public int Compare(CRigaStatisticaPerStato x, CRigaStatisticaPerStato y)
            {
                decimal v1 = x.get_Item(InfoStatoEnum.CONTATTO).Montante;
                decimal v2 = y.get_Item(InfoStatoEnum.CONTATTO).Montante;
                if (v1 < v2)
                    return 1;
                if (v1 > v2)
                    return -1;
                return 0;
            }

            int IComparer.Compare(object x, object y)
            {
                return Compare((CRigaStatisticaPerStato)x, (CRigaStatisticaPerStato)y);
            }
        }

        public class CompareByLiquidato : IComparer, IComparer<CRigaStatisticaPerStato>
        {
            public CompareByLiquidato()
            {
                DMDObject.IncreaseCounter(this);
            }

            ~CompareByLiquidato()
            {
                DMDObject.DecreaseCounter(this);
            }

            public int Compare(CRigaStatisticaPerStato x, CRigaStatisticaPerStato y)
            {
                decimal v1 = x.get_Item(InfoStatoEnum.LIQUIDATO).Montante;
                decimal v2 = y.get_Item(InfoStatoEnum.LIQUIDATO).Montante;
                if (v1 < v2)
                    return 1;
                if (v1 > v2)
                    return -1;
                return 0;
            }

            int IComparer.Compare(object x, object y)
            {
                return Compare((CRigaStatisticaPerStato)x, (CRigaStatisticaPerStato)y);
            }
        }

        public class CompareByAltriStati : IComparer, IComparer<CRigaStatisticaPerStato>
        {
            public CompareByAltriStati()
            {
                DMDObject.IncreaseCounter(this);
            }

            ~CompareByAltriStati()
            {
                DMDObject.DecreaseCounter(this);
            }

            public int Compare(CRigaStatisticaPerStato x, CRigaStatisticaPerStato y)
            {
                decimal v1 = x.get_Item(InfoStatoEnum.ALTRO).Montante;
                decimal v2 = y.get_Item(InfoStatoEnum.ALTRO).Montante;
                if (v1 < v2)
                    return 1;
                if (v1 > v2)
                    return -1;
                return 0;
            }

            int IComparer.Compare(object x, object y)
            {
                return Compare((CRigaStatisticaPerStato)x, (CRigaStatisticaPerStato)y);
            }
        }

        public class CompareByKey : IComparer, IComparer<CRigaStatisticaPerStato>
        {
            public CompareByKey()
            {
                DMDObject.IncreaseCounter(this);
            }

            ~CompareByKey()
            {
                DMDObject.DecreaseCounter(this);
            }

            public int Compare(CRigaStatisticaPerStato x, CRigaStatisticaPerStato y)
            {
                return DMD.Strings.Compare(x.Descrizione, y.Descrizione, true);
            }

            int IComparer.Compare(object x, object y)
            {
                return Compare((CRigaStatisticaPerStato)x, (CRigaStatisticaPerStato)y);
            }
        }

        [Serializable]
        public class CRigaStatisticaPerStato 
            : IComparable, IDMDXMLSerializable
        {
            public string Descrizione = "";
            public InfoStato[] m_Items;
            public string Tag;
            public int Tag1;

            public CRigaStatisticaPerStato()
            {
                DMDObject.IncreaseCounter(this);
                m_Items = new InfoStato[4];
                m_Items[0] = new InfoStato("Contatto", InfoStatoEnum.CONTATTO, 0, 0m);
                m_Items[1] = new InfoStato("Liquidato", InfoStatoEnum.LIQUIDATO, 0, 0m);
                m_Items[2] = new InfoStato("Annullato", InfoStatoEnum.ANNULLATO, 0, 0m);
                m_Items[3] = new InfoStato("Altri Stati", InfoStatoEnum.ALTRO, 0, 0m);
            }

            public InfoStato get_Item(InfoStatoEnum stato)
            {
                switch (stato)
                {
                    case InfoStatoEnum.ALTRO:
                        {
                            return m_Items[3];
                        }

                    case InfoStatoEnum.ANNULLATO:
                        {
                            return m_Items[2];
                        }

                    case InfoStatoEnum.LIQUIDATO:
                        {
                            return m_Items[1];
                        }

                    default:
                        {
                            return m_Items[0];
                        }
                }
            }

            public int CompareTo(CRigaStatisticaPerStato b)
            {
                return DMD.Strings.Compare(Descrizione, b.Descrizione, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CRigaStatisticaPerStato)obj);
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue); }
            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer); }
            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Descrizione":
                        {
                            Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Tag":
                        {
                            Tag = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Tag1":
                        {
                            Tag1 = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Items":
                        {
                            m_Items = (InfoStato[])DMD.XML.Utils.Serializer.ToArray<InfoStato>(fieldValue);
                            break;
                        }
                }
            }

            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Descrizione", Descrizione);
                writer.WriteAttribute("Tag", Tag);
                writer.WriteAttribute("Tag1", Tag1);
                writer.WriteTag("Items", m_Items);
            }

            ~CRigaStatisticaPerStato()
            {
                DMDObject.DecreaseCounter(this);
            }
        }

        [Serializable]
        public class CRigaStatisticaPerStatoCollection : CKeyCollection<CRigaStatisticaPerStato>
        {
            public CRigaStatisticaPerStatoCollection()
            {
            }

            public new CRigaStatisticaPerStato Add(string descrizione)
            {
                var item = new CRigaStatisticaPerStato();
                item.Descrizione = descrizione;
                Add("" + descrizione, item);
                // MyBase.Sort()
                return item;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */


    }
}