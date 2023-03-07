using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{
    public partial class CAnagraficaModuleHandler
    {
        [Serializable]
        public class CPopUpInfoPersona : IDMDXMLSerializable
        {
            public Anagrafica.CPersona Persona;
            public CCollection Estinzioni;
            public CustomerCalls.CContattoUtente UltimaChiamata;

            public CPopUpInfoPersona()
            {
                DMDObject.IncreaseCounter(this);
                Persona = null;
                Estinzioni = null;
                UltimaChiamata = null;
            }

            public CPopUpInfoPersona(Anagrafica.CPersona persona)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                Persona = persona;
                UltimaChiamata = CustomerCalls.CRM.GetUltimoContatto(persona);
                Estinzioni = new CCollection();
                Estinzioni.AddRange(Finanziaria.Estinzioni.GetEstinzioniByPersona(persona));
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Persona":
                        {
                            Persona = (Anagrafica.CPersona)fieldValue;
                            break;
                        }

                    case "Estinzioni":
                        {
                            Estinzioni = (CCollection)fieldValue;
                            break;
                        }

                    case "UltimaChiamata":
                        {
                            UltimaChiamata = (CustomerCalls.CContattoUtente)fieldValue;
                            break;
                        }
                }
            }

            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
            {
                writer.WriteTag("Persona", Persona);
                writer.WriteTag("UltimaChiamata", UltimaChiamata);
                writer.WriteTag("Estinzioni", Estinzioni);
            }

           

            ~CPopUpInfoPersona()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}