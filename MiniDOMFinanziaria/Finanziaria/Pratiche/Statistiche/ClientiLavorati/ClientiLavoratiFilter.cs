using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class ClientiLavoratiFilter
            : IDMDXMLSerializable
        {
            public int IDPuntoOperativo;
            public int IDConsulente;
            public DateTime? DataInizio;
            public DateTime? DataFine;
            public string TipoFonte;
            public int IDFonte;

            public ClientiLavoratiFilter()
            {
                DMDObject.IncreaseCounter(this);
                IDPuntoOperativo = 0;
                IDConsulente = 0;
                DataInizio = default;
                DataFine = default;
                TipoFonte = "";
                IDFonte = 0;
            }

            public void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDPuntoOperativo":
                        {
                            IDPuntoOperativo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDConsulente":
                        {
                            IDConsulente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataInizio":
                        {
                            DataInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFine":
                        {
                            DataFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "TipoFonte":
                        {
                            TipoFonte = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDFonte":
                        {
                            IDFonte = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                }
            }

            public void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDPuntoOperativo", IDPuntoOperativo);
                writer.WriteAttribute("IDConsulente", IDConsulente);
                writer.WriteAttribute("DataInizio", DataInizio);
                writer.WriteAttribute("DataFine", DataFine);
                writer.WriteAttribute("TipoFonte", TipoFonte);
                writer.WriteAttribute("IDFonte", IDFonte);
            }

            ~ClientiLavoratiFilter()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}