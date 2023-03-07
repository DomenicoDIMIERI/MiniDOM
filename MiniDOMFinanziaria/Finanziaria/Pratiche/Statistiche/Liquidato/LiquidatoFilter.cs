using System.Collections;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Filtro applicabile alle statistiche sul liquidato
    /// </summary>
    /// <remarks></remarks>
        public class LiquidatoFilter 
            : IDMDXMLSerializable
        {
            public CCollection<int> PuntiOperativi;
            public CCollection<int> Anni;
            public int ChartWidth;
            public int ChartHeight;

            public LiquidatoFilter()
            {
                DMDObject.IncreaseCounter(this);
                PuntiOperativi = new CCollection<int>();
                Anni = new CCollection<int>();
                ChartWidth = 0;
                ChartHeight = 0;
            }

            public void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "PO":
                        {
                            PuntiOperativi.Clear();
                            PuntiOperativi.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Anni":
                        {
                            Anni.Clear();
                            Anni.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "ChartWidth":
                        {
                            ChartWidth = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ChartHeight":
                        {
                            ChartHeight = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                }
            }

            public void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("ChartWidth", ChartWidth);
                writer.WriteAttribute("ChartHeight", ChartWidth);
                writer.WriteTag("PO", PuntiOperativi);
                writer.WriteTag("Anni", Anni);
            }

            ~LiquidatoFilter()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}