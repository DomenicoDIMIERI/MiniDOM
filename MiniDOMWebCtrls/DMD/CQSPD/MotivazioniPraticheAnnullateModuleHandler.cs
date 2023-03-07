using System;
using DMD.XML;

namespace minidom.Forms
{
    public class MPANNHSTATSITEM 
        : IComparable, IDMDXMLSerializable
    {
        public int Count = 0;
        public decimal ML = 0m;
        public string Motivo = "";

        public MPANNHSTATSITEM()
        {
            DMDObject.IncreaseCounter(this);
        }

        public int CompareTo(object obj)
        {
            MPANNHSTATSITEM other = (MPANNHSTATSITEM)obj;
            if (ML > other.ML)
            {
                return 1;
            }
            else if (ML < other.ML)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        protected virtual void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Count":
                    {
                        Count = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "ML":
                    {
                        ML = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                        break;
                    }

                case "Motivo":
                    {
                        Motivo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }
            }
        }

        protected virtual void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Count", Count);
            writer.WriteAttribute("ML", ML);
            writer.WriteAttribute("Motivo", Motivo);
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            this.XMLSerialize(writer);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            this.SetFieldInternal(fieldName, fieldValue);
        }


        ~MPANNHSTATSITEM()
        {
            DMDObject.DecreaseCounter(this);
        }
    }

    public class MotivazioniPraticheAnnullateModuleHandler : CQSPDBaseStatsHandler
    {
        public MotivazioniPraticheAnnullateModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CPraticheCQSPDCursor();
        }
    }
}