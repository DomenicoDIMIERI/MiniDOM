using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CProdTabFinConstraint : CTableConstraint
        {
            private int m_OwnerID; // ID dell'oggetto relazione prodotto X tabella Finanziaria
            private CProdottoXTabellaFin m_Owner; // Oggetto relazione prodotto X tabella Finanziaria

            public CProdTabFinConstraint()
            {
                m_OwnerID = 0;
                m_Owner = null;
            }

            public override CModulesClass GetModule()
            {
                return null;
            }

            /// <summary>
        /// ID dell'oggetto relazione prodotto X tabella Finanziaria
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int OwnerID
            {
                get
                {
                    return DBUtils.GetID(m_Owner, m_OwnerID);
                }

                set
                {
                    int oldValue = OwnerID;
                    if (oldValue == value)
                        return;
                    m_OwnerID = value;
                    m_Owner = null;
                    DoChanged("OwnerID", value, oldValue);
                }
            }

            public CProdottoXTabellaFin Owner
            {
                get
                {
                    if (m_Owner is null)
                        m_Owner = TabelleFinanziarie.GetTabellaXProdottoByID(m_OwnerID);
                    return m_Owner;
                }

                set
                {
                    var oldValue = Owner;
                    if (oldValue == value)
                        return;
                    SetOwner(value);
                    DoChanged("Owner", value, oldValue);
                }
            }

            internal void SetOwner(CProdottoXTabellaFin value)
            {
                m_Owner = value;
                m_OwnerID = DBUtils.GetID(value);
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override string GetTableName()
            {
                return "tbl_FIN_ProdXTabFinConstr";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_OwnerID = reader.Read("Owner", m_OwnerID);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Owner", OwnerID);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("OwnerID", OwnerID);
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "OwnerID":
                        {
                            m_OwnerID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }
        }
    }
}