using DMD.XML;

namespace minidom.Forms
{
    public class CQSPDDashInfo 
        : IDMDXMLSerializable
    {
        public int Count;
        public decimal SommaMontanteLordo;
        public decimal SommaProvvigioneTotale;
        public decimal SommaSpread;
        public decimal SommaUpFront;
        public decimal SommaRunning;
        public CCollection<CQSPDPDBInfo> Pratiche;

        public CQSPDDashInfo()
        {
            DMDObject.IncreaseCounter(this);
            Count = 0;
            SommaMontanteLordo = 0m;
            SommaProvvigioneTotale = 0m;
            SommaSpread = 0m;
            SommaUpFront = 0m;
            SommaRunning = 0m;
            Pratiche = new CCollection<CQSPDPDBInfo>();
        }

        protected void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Count", Count);
            writer.WriteAttribute("SommaMontanteLordo", SommaMontanteLordo);
            writer.WriteAttribute("SommaProvvigioneTotale", SommaProvvigioneTotale);
            writer.WriteAttribute("SommaSpread", SommaSpread);
            writer.WriteAttribute("SommaUpFront", SommaUpFront);
            writer.WriteAttribute("SommaRunning", SommaRunning);
            writer.WriteTag("Pratiche", Pratiche);
        }

        protected void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Pratiche":
                    {
                        Pratiche = (CCollection<CQSPDPDBInfo>)fieldValue;
                        break;
                    }

                case "Count":
                    {
                        Count = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "SommaMontanteLordo":
                    {
                        SommaMontanteLordo = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "SommaProvvigioneTotale":
                    {
                        SommaProvvigioneTotale = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "SommaRunning":
                    {
                        SommaRunning = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "SommaSpread":
                    {
                        SommaSpread = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "SommaUpFront":
                    {
                        SommaUpFront = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }
            }
        }

        ~CQSPDDashInfo()
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