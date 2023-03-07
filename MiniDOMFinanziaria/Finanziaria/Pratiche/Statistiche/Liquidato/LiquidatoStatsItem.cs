using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {
        public class LiquidatoStatsItem
            : IComparable, IDMDXMLSerializable
        {
            public int Anno;
            public int Mese;
            public int CaricatoCnt;
            public decimal CaricatoSum;
            public decimal CaricatoUpfront;
            public decimal CaricatoRunning;
            public decimal CaricatoSconto;
            public int RichiestaDeliberaCnt;
            public decimal RichiestaDeliberaSum;
            public decimal RichiestaDeliberaUpfront;
            public decimal RichiestaDeliberaRunning;
            public decimal RichiestaDeliberaSconto;
            public int LiquidatoCnt;
            public decimal LiquidatoSum;
            public decimal LiquidatoUpfront;
            public decimal LiquidatoRunning;
            public decimal LiquidatoSconto;

            public LiquidatoStatsItem()
            {
                DMDObject.IncreaseCounter(this);
            }

            public int CompareTo(object obj)
            {
                LiquidatoStatsItem o = (LiquidatoStatsItem)obj;
                int ret = -Anno + o.Anno;
                if (ret == 0)
                    ret = -Mese + o.Mese;
                return ret;
            }

            public void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Anno":
                        {
                            Anno = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Mese":
                        {
                            Mese = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "CaricatoCnt":
                        {
                            CaricatoCnt = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "CaricatoSum":
                        {
                            CaricatoSum = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "CaricatoUpfront":
                        {
                            CaricatoUpfront = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "CaricatoRunning":
                        {
                            CaricatoRunning = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "CaricatoSconto":
                        {
                            CaricatoSconto = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "RichiestaDeliberaCnt":
                        {
                            RichiestaDeliberaCnt = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "RichiestaDeliberaSum":
                        {
                            RichiestaDeliberaSum = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "RichiestaDeliberaUpfront":
                        {
                            RichiestaDeliberaUpfront = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "RichiestaDeliberaRunning":
                        {
                            RichiestaDeliberaRunning = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "RichiestaDeliberaSconto":
                        {
                            RichiestaDeliberaSconto = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "LiquidatoCnt":
                        {
                            LiquidatoCnt = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "LiquidatoSum":
                        {
                            LiquidatoSum = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "LiquidatoUpfront":
                        {
                            LiquidatoUpfront = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "LiquidatoRunning":
                        {
                            LiquidatoRunning = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "LiquidatoSconto":
                        {
                            LiquidatoSconto = (decimal)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }
                }
            }

            public void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Anno", Anno);
                writer.WriteAttribute("Mese", Mese);
                writer.WriteAttribute("CaricatoCnt", CaricatoCnt);
                writer.WriteAttribute("CaricatoSum", CaricatoSum);
                writer.WriteAttribute("CaricatoUpfront", CaricatoUpfront);
                writer.WriteAttribute("CaricatoRunning", CaricatoRunning);
                writer.WriteAttribute("CaricatoSconto", CaricatoSconto);
                writer.WriteAttribute("RichiestaDeliberaCnt", RichiestaDeliberaCnt);
                writer.WriteAttribute("RichiestaDeliberaSum", RichiestaDeliberaSum);
                writer.WriteAttribute("RichiestaDeliberaUpfront", RichiestaDeliberaUpfront);
                writer.WriteAttribute("RichiestaDeliberaRunning", RichiestaDeliberaRunning);
                writer.WriteAttribute("RichiestaDeliberaSconto", RichiestaDeliberaSconto);
                writer.WriteAttribute("LiquidatoCnt", LiquidatoCnt);
                writer.WriteAttribute("LiquidatoSum", LiquidatoSum);
                writer.WriteAttribute("LiquidatoUpfront", LiquidatoUpfront);
                writer.WriteAttribute("LiquidatoRunning", LiquidatoRunning);
                writer.WriteAttribute("LiquidatoSconto", LiquidatoSconto);
            }

            ~LiquidatoStatsItem()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}