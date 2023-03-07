using System;
using System.Collections;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CQSPDInfoAnalisiAnomalie
            : IComparable, IDMDXMLSerializable
        {
            public int IDCliente;
            public string NomeCliente;
            [NonSerialized] private Anagrafica.CPersonaFisica m_Cliente;
            public CCollection<OggettoAnomalo> Oggetti;

            public CQSPDInfoAnalisiAnomalie()
            {
                DMDObject.IncreaseCounter(this);
                IDCliente = 0;
                NomeCliente = "";
                m_Cliente = null;
                Oggetti = new CCollection<OggettoAnomalo>();
            }

            public Anagrafica.CPersonaFisica Cliente
            {
                get
                {
                    if (m_Cliente is null)
                        m_Cliente = (Anagrafica.CPersonaFisica)Anagrafica.Persone.GetItemById(IDCliente);
                    return m_Cliente;
                }

                set
                {
                    m_Cliente = value;
                    IDCliente = DBUtils.GetID(value);
                    if (value is null)
                    {
                        NomeCliente = "";
                    }
                    else
                    {
                        NomeCliente = value.Nominativo;
                    }
                }
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue); }
            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer); }

            public int CompareTo(object obj)
            {
                CQSPDInfoAnalisiAnomalie o = (CQSPDInfoAnalisiAnomalie)obj;
                return DMD.Strings.Compare(NomeCliente, o.NomeCliente, true);
            }

            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDCliente":
                        {
                            IDCliente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCliente":
                        {
                            NomeCliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Oggetti":
                        {
                            Oggetti.Clear();
                            Oggetti.AddRange((IEnumerable)fieldValue);
                            break;
                        }
                }
            }

            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", NomeCliente);
                writer.WriteTag("Oggetti", Oggetti);
            }

            ~CQSPDInfoAnalisiAnomalie()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}