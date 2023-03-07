using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{

    [Serializable]
    public class TabellaProdottoRow 
        : IDMDXMLSerializable
    {
        public ProdottoRow[] rows;
        public CKeyCollection<Finanziaria.CTabellaFinanziaria> TabelleFinanziarie;
        public CKeyCollection<Finanziaria.CTabellaAssicurativa> TabelleAssicurative;

        public TabellaProdottoRow()
        {
            rows = null;
            TabelleFinanziarie = new CKeyCollection<Finanziaria.CTabellaFinanziaria>();
            TabelleAssicurative = new CKeyCollection<Finanziaria.CTabellaAssicurativa>();
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "row":
                    {
                        rows = (ProdottoRow[])DMD.Arrays.Convert<ProdottoRow>(fieldValue);
                        break;
                    }

                case "TabelleFinanziarie":
                    {
                        TabelleFinanziarie.Clear();
                        if (fieldValue is CKeyCollection)
                        {
                            {
                                var withBlock = (CKeyCollection)fieldValue;
                                foreach (string k in withBlock.Keys)
                                    TabelleFinanziarie.Add(k, (Finanziaria.CTabellaFinanziaria)withBlock[k]);
                            }
                        }

                        break;
                    }

                case "TabelleAssicurative":
                    {
                        TabelleAssicurative.Clear();
                        if (fieldValue is CKeyCollection)
                        {
                            {
                                var withBlock1 = (CKeyCollection)fieldValue;
                                foreach (string k in withBlock1.Keys)
                                    TabelleAssicurative.Add(k, (Finanziaria.CTabellaAssicurativa)withBlock1[k]);
                            }
                        }

                        break;
                    }
            }
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            TabelleFinanziarie.Clear();
            TabelleAssicurative.Clear();
            if (rows is object)
            {
                foreach (ProdottoRow row in rows)
                {
                    if (row.TabellaFinanziaria is object)
                    {
                        if (!TabelleFinanziarie.ContainsKey("T" + row.TabellaFinanziaria.IDTabella))
                        {
                            TabelleFinanziarie.Add("T" + row.TabellaFinanziaria.IDTabella, row.TabellaFinanziaria.Tabella);
                        }
                    }

                    if (row.TabellaAssicurativa is object)
                    {
                        if (!TabelleAssicurative.ContainsKey("T" + row.TabellaAssicurativa.IDRischioVita))
                        {
                            TabelleAssicurative.Add("T" + row.TabellaAssicurativa.IDRischioVita, row.TabellaAssicurativa.RischioVita);
                        }

                        if (!TabelleAssicurative.ContainsKey("T" + row.TabellaAssicurativa.IDRischioImpiego))
                        {
                            TabelleAssicurative.Add("T" + row.TabellaAssicurativa.IDRischioImpiego, row.TabellaAssicurativa.RischioImpiego);
                        }

                        if (!TabelleAssicurative.ContainsKey("T" + row.TabellaAssicurativa.IDRischioCredito))
                        {
                            TabelleAssicurative.Add("T" + row.TabellaAssicurativa.IDRischioCredito, row.TabellaAssicurativa.RischioCredito);
                        }
                    }
                }
            }

            writer.WriteTag("TabelleFinanziarie", TabelleFinanziarie);
            writer.WriteTag("TabelleAssicurative", TabelleAssicurative);
            writer.WriteTag("rows", rows);
        }
    }


    [Serializable]
    public class ProdottoRow 
        : IDMDXMLSerializable
    {
        public int Index;
        public int IDProdotto;
        public int IDGruppo;
        public Finanziaria.CProdottoXTabellaFin TabellaFinanziaria;
        public Finanziaria.CProdottoXTabellaAss TabellaAssicurativa;

        public ProdottoRow()
        {
            Index = 0;
            IDProdotto = 0;
            IDGruppo = 0;
            TabellaFinanziaria = null;
            TabellaAssicurativa = null;
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Index":
                    {
                        Index = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "IDProdotto":
                    {
                        IDProdotto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "IDGruppo":
                    {
                        IDGruppo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "TabellaFinanziaria":
                    {
                        TabellaFinanziaria = (Finanziaria.CProdottoXTabellaFin)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                        break;
                    }

                case "TabellaAssicurativa":
                    {
                        TabellaAssicurativa = (Finanziaria.CProdottoXTabellaAss)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                        break;
                    }
            }
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Index", Index);
            writer.WriteAttribute("IDGruppo", IDGruppo);
            writer.WriteTag("IDProdotto", IDProdotto);
            writer.WriteTag("TabellaFinanziaria", TabellaFinanziaria);
            writer.WriteTag("TabellaAssicurativa", TabellaAssicurativa);
        }
    }


    [Serializable]
    public class ProdottoRowFilter
        : IDMDXMLSerializable
    {
        public int IDCessionario;
        public int IDGruppo;
        public int IDProfilo;
        public int IDProdotto;

        public ProdottoRowFilter()
        {
            IDCessionario = 0;
            IDGruppo = 0;
            IDProfilo = 0;
            IDProdotto = 0;
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "IDCessionario":
                    {
                        IDCessionario = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "IDGruppo":
                    {
                        IDGruppo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "IDProfilo":
                    {
                        IDProfilo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "IDProdotto":
                    {
                        IDProdotto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }
            }
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("IDCessionario", IDCessionario);
            writer.WriteAttribute("IDGruppo", IDGruppo);
            writer.WriteAttribute("IDProfilo", IDProfilo);
            writer.WriteAttribute("IDProdotto", IDProdotto);
        }
    }



    public class CProdottiModuleHandler 
        : CBaseModuleHandler
    {
        public CProdottiModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CProdottiCursor();
        }
    }
}