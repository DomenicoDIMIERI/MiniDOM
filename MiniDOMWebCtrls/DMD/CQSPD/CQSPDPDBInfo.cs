using DMD.XML;

namespace minidom.Forms
{
    public class CQSPDPDBInfo 
        : IDMDXMLSerializable
    {
        public int IDPratica;
        public string NominativoCliente;
        public string NomeProdotto;
        public int IDStatoPratica;
        public int GiorniContatto;
        public int GiorniStatoAttuale;
        public decimal Netto;
        public int Durata;
        public decimal Rata;
        public decimal Spread;
        public decimal UpFront;
        public CKeyCollection Attributi = new CKeyCollection();

        public CQSPDPDBInfo()
        {
            DMDObject.IncreaseCounter(this);
            IDPratica = 0;
            NominativoCliente = "";
            NomeProdotto = "";
            IDStatoPratica = 0;
            GiorniContatto = 0;
            GiorniStatoAttuale = 0;
            Netto = 0m;
            Durata = 0;
            Rata = 0m;
            Spread = 0m;
            UpFront = 0m;
        }

        protected void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("IDPratica", IDPratica);
            writer.WriteAttribute("NominativoCliente", NominativoCliente);
            writer.WriteAttribute("NomeProdotto", NomeProdotto);
            writer.WriteAttribute("IDStatoPratica", IDStatoPratica);
            writer.WriteAttribute("GiorniContatto", GiorniContatto);
            writer.WriteAttribute("GiorniStatoAttuale", GiorniStatoAttuale);
            writer.WriteAttribute("Netto", Netto);
            writer.WriteAttribute("Durata", Durata);
            writer.WriteAttribute("Rata", Rata);
            writer.WriteAttribute("Spread", Spread);
            writer.WriteAttribute("UpFront", UpFront);
            writer.WriteTag("Attributi", Attributi);
        }

        protected void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "IDPratica":
                    {
                        IDPratica = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "NominativoCliente":
                    {
                        NominativoCliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "NomeProdotto":
                    {
                        NomeProdotto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "IDStatoPratica":
                    {
                        IDStatoPratica = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "GiorniContatto":
                    {
                        GiorniContatto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "GiorniStatoAttuale":
                    {
                        GiorniStatoAttuale = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "Netto":
                    {
                        Netto = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "Durata":
                    {
                        Durata = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "Rata":
                    {
                        Rata = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "Spread":
                    {
                        Spread = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "UpFront":
                    {
                        UpFront = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "Attributi":
                    {
                        Attributi = (CKeyCollection)fieldValue;
                        break;
                    }
            }
        }

        ~CQSPDPDBInfo()
        {
            DMDObject.DecreaseCounter(this);
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            this.XMLSerialize(writer);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            this.SetFieldInternal(fieldName, fieldValue);
        }

    }
}