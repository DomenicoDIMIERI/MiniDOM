using System.Collections;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{
    public class OfficeSituazionePersona 
        : IDMDXMLSerializable
    {
        private int m_IDPersona = 0;
        public CCollection<Office.Commissione> Commissioni;
        public CCollection<Office.RichiestaCERQ> Richieste;
        // Public Uscite As CCollection(Of Uscita)

        public OfficeSituazionePersona()
        {
        }

        public OfficeSituazionePersona(int idPersona)
        {
            m_IDPersona = idPersona;
            Commissioni = Office.Commissioni.GetCommissioniByPersona(idPersona);
            Richieste = Office.RichiesteCERQ.GetRichiesteByPersona(idPersona);
            // Me.Uscite = Office.RichiesteCERQ.GetRichiesteByPersona(idPersona)
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

                case "Commissioni":
                    {
                        Commissioni = new CCollection<Office.Commissione>();
                        if (fieldValue is IEnumerable)
                            Commissioni.AddRange((IEnumerable)fieldValue);
                        break;
                    }

                case "Richieste":
                    {
                        Richieste = new CCollection<Office.RichiestaCERQ>();
                        if (fieldValue is IEnumerable)
                            Richieste.AddRange((IEnumerable)fieldValue);
                        break;
                    }
            }
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("IDPersona", m_IDPersona);
            writer.WriteTag("Commissioni", Commissioni);
            writer.WriteTag("Richieste", Richieste);
        }
    }
}