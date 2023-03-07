using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {


        /// <summary>
    /// Definizione di una singola riga dei TEG massimi
    /// </summary>
    /// <remarks></remarks>
        public class CRigaTEGMax : Databases.DBObject
        {
            private int m_TabellaID; // ID della tabella a cui appartiene la riga
            private CTabellaTEGMax m_Tabella; // Oggetto tabella a cui appartiene la riga
            private double m_ValoreSoglia;  // Limite superiore di validità della riga della tabella
            private double[] m_Coefficienti;  // Valori soglia del TEG espressi per durata

            public CRigaTEGMax()
            {
                m_TabellaID = 0;
                m_Tabella = null;
                m_ValoreSoglia = 0d;
                m_Coefficienti = new double[11];
            }

            public override CModulesClass GetModule()
            {
                return null;
            }

            public int IDTabella
            {
                get
                {
                    return DBUtils.GetID(m_Tabella, m_TabellaID);
                }

                set
                {
                    int oldValue = IDTabella;
                    if (oldValue == value)
                        return;
                    m_TabellaID = value;
                    m_Tabella = null;
                    DoChanged("IDTabella", value, oldValue);
                }
            }

            public CTabellaTEGMax Tabella
            {
                get
                {
                    if (m_Tabella is null)
                        m_Tabella = TabelleTEGMax.GetItemById(m_TabellaID);
                    return m_Tabella;
                }

                set
                {
                    var oldValue = Tabella;
                    if (oldValue == value)
                        return;
                    m_Tabella = value;
                    m_TabellaID = DBUtils.GetID(value);
                    DoChanged("Tabella", value, oldValue);
                }
            }

            /// <summary>
        /// Limite superiore di validità della riga della tabella
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double ValoreSoglia
            {
                get
                {
                    return m_ValoreSoglia;
                }

                set
                {
                    double oldValue = m_ValoreSoglia;
                    if (oldValue == value)
                        return;
                    m_ValoreSoglia = value;
                    DoChanged("ValoreSoglia", value, oldValue);
                }
            }

            /// <summary>
        /// Valori soglia del TEG espressi per durata
        /// </summary>
        /// <param name="durata"></param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double get_Coefficiente(int durata)
            {
                return m_Coefficienti[(int)Maths.Floor(durata / 12d)];
            }

            public void set_Coefficiente(int durata, double value)
            {
                double oldValue = m_Coefficienti[(int)Maths.Floor(durata / 12d)];
                if (oldValue == value)
                    return;
                m_Coefficienti[(int)Maths.Floor(durata / 12d)] = value;
                DoChanged("Coefficiente", value, oldValue);
            }

            public override string GetTableName()
            {
                return "tbl_FIN_TEGMaxI";
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Tabella", this.IDTabella);
                writer.WriteAttribute("Soglia", this.m_ValoreSoglia);
                base.XMLSerialize(writer);
                writer.WriteTag("Coefficienti", this.m_Coefficienti);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Tabella":
                        {
                            this.m_TabellaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Soglia":
                        {
                            this.m_ValoreSoglia = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Coefficienti":
                        {
                            this.m_Coefficienti = (double[])DMD.Arrays.Convert<double>(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_TabellaID = reader.Read("Tabella", this.m_TabellaID);
                this.m_ValoreSoglia = reader.Read("ValoreSoglia", this.m_ValoreSoglia);
                for (int i = 1; i <= 10; i++)
                    this.m_Coefficienti[i] = reader.Read("Coeff" + i * 12, this.m_Coefficienti[i]);

                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                int i;
                writer.Write("Tabella", DBUtils.GetID(this.m_Tabella, this.m_TabellaID));
                writer.Write("ValoreSoglia", this.m_ValoreSoglia);
                for (i = 1; i <= 10; i++)
                    writer.Write("Coeff" + i * 12, this.m_Coefficienti[i]);
                return base.SaveToRecordset(writer);
            }

            public override string ToString()
            {
                return m_ValoreSoglia.ToString();
            }

            public bool Check(COffertaCQS offerta)
            {
                throw new NotImplementedException();
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}