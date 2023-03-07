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
        public class CQSFilter 
            : IDMDXMLSerializable
        {
            private int m_IDCliente;
            [NonSerialized]  private Anagrafica.CPersona m_Cliente;
            private int m_IDRichiesta;
            [NonSerialized] private CRichiestaFinanziamento m_Richiesta;
            [NonSerialized] private CQSPDConsulenza m_Consulenza;
            private int m_IDConsulenza;
            [NonSerialized] private CConsulentePratica m_Consulente;
            private int m_IDConsulente;
            private string m_TipoFonte;
            private int m_IDFonte;
            [NonSerialized] private IFonte m_Fonte;

            public CQSFilter()
            {
                DMDObject.IncreaseCounter(this);
                m_IDCliente = 0;
                m_Cliente = null;
                m_IDRichiesta = 0;
                m_Richiesta = null;
                m_TipoFonte = "";
                m_IDFonte = 0;
                m_Fonte = null;
                m_Consulenza = null;
                m_IDConsulenza = 0;
                m_Consulente = null;
                m_IDConsulente = 0;
            }

            public int IDCliente
            {
                get
                {
                    return DBUtils.GetID(m_Cliente, m_IDCliente);
                }

                set
                {
                    m_IDCliente = value;
                    m_Cliente = null;
                }
            }

            public Anagrafica.CPersona Cliente
            {
                get
                {
                    if (m_Cliente is null)
                        m_Cliente = Anagrafica.Persone.GetItemById(m_IDCliente);
                    return m_Cliente;
                }

                set
                {
                    m_Cliente = value;
                    m_IDCliente = DBUtils.GetID(value);
                }
            }

            public int IDRichiesta
            {
                get
                {
                    return DBUtils.GetID(m_Richiesta, m_IDRichiesta);
                }

                set
                {
                    m_IDRichiesta = value;
                    m_Richiesta = null;
                }
            }

            public CRichiestaFinanziamento Richiesta
            {
                get
                {
                    if (m_Richiesta is null)
                        m_Richiesta = RichiesteFinanziamento.GetItemById(m_IDRichiesta);
                    return m_Richiesta;
                }

                set
                {
                    m_Richiesta = value;
                    m_IDRichiesta = DBUtils.GetID(value);
                }
            }

            public int IDConsulente
            {
                get
                {
                    return DBUtils.GetID(m_Consulente, m_IDConsulente);
                }

                set
                {
                    m_IDConsulente = value;
                    m_Consulente = null;
                }
            }

            public CConsulentePratica Consulente
            {
                get
                {
                    if (m_Consulente is null)
                        m_Consulente = Consulenti.GetItemById(m_IDConsulente);
                    return m_Consulente;
                }

                set
                {
                    m_Consulente = value;
                    m_IDConsulente = DBUtils.GetID(value);
                }
            }

            public int IDConsulenza
            {
                get
                {
                    return DBUtils.GetID(m_Consulenza, m_IDConsulenza);
                }

                set
                {
                    m_IDConsulenza = value;
                    m_Consulenza = null;
                }
            }

            public CQSPDConsulenza Consulenza
            {
                get
                {
                    if (m_Consulenza is null)
                        m_Consulenza = Consulenze.GetItemById(m_IDConsulenza);
                    return m_Consulenza;
                }

                set
                {
                    m_Consulenza = value;
                    m_IDConsulenza = DBUtils.GetID(value);
                }
            }

            public string TipoFonte
            {
                get
                {
                    return m_TipoFonte;
                }

                set
                {
                    m_TipoFonte = value;
                    m_Fonte = null;
                }
            }

            public int IDFonte
            {
                get
                {
                    return DBUtils.GetID((Databases.IDBObjectBase)m_Fonte, m_IDFonte);
                }

                set
                {
                    m_IDFonte = value;
                    m_Fonte = null;
                }
            }

            public IFonte Fonte
            {
                get
                {
                    if (m_Fonte is null && !string.IsNullOrEmpty(m_TipoFonte))
                        m_Fonte = Anagrafica.Fonti.GetItemById(m_TipoFonte, m_TipoFonte, m_IDFonte);
                    return m_Fonte;
                }

                set
                {
                    m_Fonte = value;
                    m_IDFonte = DBUtils.GetID((Databases.IDBObjectBase)value);
                }
            }

            public void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDCliente":
                        {
                            m_IDCliente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDRichiesta":
                        {
                            m_IDRichiesta = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoFonte":
                        {
                            m_TipoFonte = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDFonte":
                        {
                            m_IDFonte = DMD.Integers.ValueOf(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "IDConsulenza":
                        {
                            m_IDConsulenza = DMD.Integers.ValueOf(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "IDConsulente":
                        {
                            m_IDConsulente = DMD.Integers.ValueOf(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }
                }
            }

            public void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("IDRichiesta", IDRichiesta);
                writer.WriteAttribute("TipoFonte", TipoFonte);
                writer.WriteAttribute("IDFonte", IDFonte);
                writer.WriteAttribute("IDConsulenza", IDConsulenza);
                writer.WriteAttribute("IDConsulente", IDConsulente);
            }

            ~CQSFilter()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}