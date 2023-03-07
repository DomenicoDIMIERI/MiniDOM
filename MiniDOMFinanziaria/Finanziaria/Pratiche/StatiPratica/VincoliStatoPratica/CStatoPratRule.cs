using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        public enum FlagsRegolaStatoPratica : int
        {
            None = 0,

            /// <summary>
        /// Flag valido per il passaggio di stato in annullato.
        /// Identifica la regola come annullata dal cliente
        /// </summary>
        /// <remarks></remarks>
            DaCliente = 1,

            /// <summary>
        /// Flag valido per il passaggio di stato in annullato.
        /// Identifica la regola come annullata dall'agenzia bocciata
        /// </summary>
        /// <remarks></remarks>
            Bocciata = 2,

            /// <summary>
        /// Flag valido per il passaggio di stato in annullato.
        /// Identifica la regola come annullata (non fattibile)
        /// </summary>
        /// <remarks></remarks>
            NonFattibile = 4
        }

        /// <summary>
    /// Definisce una regola di passaggio di stato (dallo stato a cui appartiene ad uno stato successivo)
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CStatoPratRule : Databases.DBObject, IComparable
        {
            private string m_Nome; // [TEXT]     Nome della regola
            private string m_Descrizione;  // [TEXT]     Descrizione estesa della regola
            private int m_IDSource;  // [INT]      ID dello stato a cui appartiene la regola
            [NonSerialized]
            private CStatoPratica m_Source; // [CStatoPratica]    Stato a cui appartiene la regola
            private int m_IDTarget;  // [INT]      ID dello stato verso cui porta la regola
            [NonSerialized]
            private CStatoPratica m_Target; // [CStatoPratica]    Stato verso cui porta la regola
            private int m_Order;
            private bool m_Attivo;
            private FlagsRegolaStatoPratica m_Flags;

            public CStatoPratRule()
            {
                m_Nome = "";
                m_Descrizione = "";
                m_IDSource = 0;
                m_Source = null;
                m_IDTarget = 0;
                m_Target = null;
                m_Order = 0;
                m_Attivo = true;
                m_Flags = FlagsRegolaStatoPratica.None;
            }

            public FlagsRegolaStatoPratica Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    var oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            public int Order
            {
                get
                {
                    return m_Order;
                }

                set
                {
                    int oldValue = m_Order;
                    if (oldValue == value)
                        return;
                    m_Order = value;
                    DoChanged("Order", value, oldValue);
                }
            }

            public bool Attivo
            {
                get
                {
                    return m_Attivo;
                }

                set
                {
                    if (m_Attivo == value)
                        return;
                    m_Attivo = value;
                    DoChanged("Attivo", value, !value);
                }
            }

            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            public int IDSource
            {
                get
                {
                    return DBUtils.GetID(m_Source, m_IDSource);
                }

                set
                {
                    int oldValue = IDSource;
                    if (oldValue == value)
                        return;
                    m_IDSource = value;
                    m_Source = null;
                    DoChanged("IDSource", value, oldValue);
                }
            }

            public CStatoPratica Source
            {
                get
                {
                    if (m_Source is null)
                        m_Source = StatiPratica.GetItemById(m_IDSource);
                    return m_Source;
                }

                set
                {
                    var oldValue = m_Source;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Source = value;
                    m_IDSource = DBUtils.GetID(value);
                    DoChanged("Source", value, oldValue);
                }
            }

            protected internal void SetSource(CStatoPratica value)
            {
                m_Source = value;
                m_IDSource = DBUtils.GetID(value);
            }

            public CStatoPratica Target
            {
                get
                {
                    if (m_Target is null)
                        m_Target = StatiPratica.GetItemById(m_IDTarget);
                    return m_Target;
                }

                set
                {
                    var oldValue = m_Target;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Target = value;
                    m_IDTarget = DBUtils.GetID(value);
                    DoChanged("Target", value, oldValue);
                }
            }

            public int IDTarget
            {
                get
                {
                    return DBUtils.GetID(m_Target, m_IDTarget);
                }

                set
                {
                    int oldValue = IDTarget;
                    if (oldValue == value)
                        return;
                    m_IDTarget = value;
                    m_Target = null;
                    DoChanged("IDTarget", value, oldValue);
                }
            }

            public override CModulesClass GetModule()
            {
                return StatiPratRules.Module;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override string GetTableName()
            {
                return "tbl_PraticheSTR";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome",  m_Nome);
                m_Descrizione = reader.Read("Descrizione",  m_Descrizione);
                m_IDSource = reader.Read("IDSource",  m_IDSource);
                m_IDTarget = reader.Read("IDTarget",  m_IDTarget);
                m_Order = reader.Read("Order",  m_Order);
                m_Attivo = reader.Read("Attivo",  m_Attivo);
                m_Flags = reader.Read("Flags",  m_Flags);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("IDSource", IDSource);
                writer.Write("IDTarget", IDTarget);
                writer.Write("Order", m_Order);
                writer.Write("Attivo", m_Attivo);
                writer.Write("Flags", m_Flags);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("IDSource", IDSource);
                writer.WriteAttribute("IDTarget", IDTarget);
                writer.WriteAttribute("Order", m_Order);
                writer.WriteAttribute("Attivo", m_Attivo);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                base.XMLSerialize(writer);
                writer.WriteTag("Descrizione", m_Descrizione);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDSource":
                        {
                            m_IDSource = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDTarget":
                        {
                            m_IDTarget = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Order":
                        {
                            m_Order = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Attivo":
                        {
                            m_Attivo = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (FlagsRegolaStatoPratica)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public override string ToString()
            {
                return m_Nome;
            }

            public int CompareTo(CStatoPratRule obj)
            {
                int ret = m_Order - obj.m_Order;
                if (ret == 0)
                    ret = DMD.Strings.Compare(m_Nome, obj.m_Nome, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CStatoPratRule)obj);
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                if (Source is object)
                {
                    lock (Source)
                    {
                        var s1 = Source.StatiSuccessivi.GetItemById(DBUtils.GetID(this));
                        int i = -1;
                        if (s1 is object)
                            i = Source.StatiSuccessivi.IndexOf(s1);
                        if (Stato == ObjectStatus.OBJECT_VALID)
                        {
                            if (i >= 0)
                            {
                                Source.StatiSuccessivi[i] = this;
                            }
                            else
                            {
                                Source.StatiSuccessivi.Add(this);
                            }

                            Source.StatiSuccessivi.Sort();
                        }
                        else if (i >= 0)
                            Source.StatiSuccessivi.RemoveAt(i);
                    }
                }

                StatiPratRules.UpdateCached(this);
            }
        }
    }
}