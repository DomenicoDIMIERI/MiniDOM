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
    /// Tipo di vincolo su una tabella (Finanziaria o assicurativa)
    /// </summary>
    /// <remarks></remarks>
        public enum TableContraints : int
        {
            CONSTR_NE = -3,        // Not equal
            CONSTR_LT = -2,        // Less than
            CONSTR_LE = -1,        // Less than or equal
            CONSTR_EQ = 0,         // Equal
            CONSTR_GE = 1,         // Greater than or equal
            CONSTR_GT = 2,         // Greater than
            CONSTR_LIKE = 3,       // Like
            CONSTR_ANY = 4,        // Contiene almeno uno tra
            CONSTR_ALL = 5,        // Contiene tutti
            CONSTR_BETWEEN = 6    // Tra
        }

        /// <summary>
    /// Vincolo su una tabella (Finanziaria o assicurativa)
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CTableConstraint : Databases.DBObject
        {
            private string m_Espressione; // Nome del campo o espressione
            private TypeCode m_TipoValore;
            private TableContraints m_TipoVincolo; // Tipo Vincolo: 
            private object m_Op1; // Limite inferiore del vincolo
            private object m_Op2; // Limite superiore del vincolo

            public CTableConstraint()
            {
                m_Espressione = "";
                m_TipoVincolo = 0;
            }

            /// <summary>
        /// Nome del campo o espressione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Espressione
            {
                get
                {
                    return m_Espressione;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Espressione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Espressione = Strings.Trim(value);
                    DoChanged("Espressione", value, oldValue);
                }
            }

            /// <summary>
        /// Tipo Vincolo:
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public TableContraints TipoVincolo
            {
                get
                {
                    return m_TipoVincolo;
                }

                set
                {
                    var oldValue = value;
                    if (oldValue == value)
                        return;
                    m_TipoVincolo = value;
                    DoChanged("TipoVincolo", value, oldValue);
                }
            }

            /// <summary>
        /// Tipo del valore del vincolo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public TypeCode TipoValore
            {
                get
                {
                    return m_TipoValore;
                }

                set
                {
                    var oldValue = m_TipoValore;
                    if (oldValue == value)
                        return;
                    m_TipoValore = value;
                    DoChanged("TipoValore", value, oldValue);
                }
            }

            /// <summary>
        /// Limite inferiore del vincolo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public object Op1
            {
                get
                {
                    return m_Op1;
                }

                set
                {
                    value = Sistema.Types.CastTo(value, TipoValore);
                    var oldValue = m_Op1;
                    
                    if (DMD.Arrays.Compare(value, oldValue) == 0)
                        return;

                    m_Op1 = value;
                    DoChanged("Op1", value, oldValue);
                }
            }

            public object Op2
            {
                get
                {
                    return m_Op2;
                }

                set
                {
                    value = Sistema.Types.CastTo(value, TipoValore);
                    var oldValue = m_Op2;
                    
                    if (DMD.Arrays.Compare(value, oldValue) == 0)
                        return;

                    m_Op2 = value;
                    DoChanged("Op2", value, oldValue);
                }
            }


            /// <summary>
        /// Controlla che la relazione sia applicazione
        /// </summary>
        /// <param name="offerta"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Check(COffertaCQS offerta)
            {
                object value;
                bool ret = true;
                value = Sistema.Types.CastTo(offerta.EvaluateExpression(Espressione), TipoValore);
                switch (TipoVincolo)
                {
                    case TableContraints.CONSTR_NE:
                        {
                            int localCompare() { var argb = Op1; var ret = DMD.Arrays.Compare(value, argb); Op1 = argb; return ret; }

                            ret = localCompare() != 0;
                            break;
                        }

                    case TableContraints.CONSTR_LT:
                        {
                            int localCompare1() { var argb = Op1; var ret = DMD.Arrays.Compare(value, argb); Op1 = argb; return ret; }

                            ret = localCompare1() < 0;
                            break;
                        }

                    case TableContraints.CONSTR_LE:
                        {
                            int localCompare2() { var argb = Op1; var ret = DMD.Arrays.Compare(value, argb); Op1 = argb; return ret; }

                            ret = localCompare2() <= 0;
                            break;
                        }

                    case TableContraints.CONSTR_EQ:
                        {
                            int localCompare3() { var argb = Op1; var ret = DMD.Arrays.Compare(value, argb); Op1 = argb; return ret; }

                            ret = localCompare3() == 0;
                            break;
                        }

                    case TableContraints.CONSTR_GE:
                        {
                            int localCompare4() { var argb = Op1; var ret = DMD.Arrays.Compare(value, argb); Op1 = argb; return ret; }

                            ret = localCompare4() >= 0;
                            break;
                        }

                    case TableContraints.CONSTR_GT:
                        {
                            int localCompare5() { var argb = Op1; var ret = DMD.Arrays.Compare(value, argb); Op1 = argb; return ret; }

                            ret = localCompare5() > 0;
                            break;
                        }

                    case TableContraints.CONSTR_LIKE:
                        {
                            ret = Strings.InStr(DMD.Strings.CStr(value), DMD.Strings.CStr(Op1)) > 0;
                            break;
                        }

                    case TableContraints.CONSTR_ANY:
                        {
                            ret = Strings.InStr(DMD.Strings.CStr(value), DMD.Strings.CStr(Op1)) > 0;
                            break;
                        }

                    case TableContraints.CONSTR_ALL:
                        {
                            int localCompare6() { var argb = Op1; var ret = DMD.Arrays.Compare(value, argb); Op1 = argb; return ret; }

                            ret = localCompare6() == 0;
                            break;
                        }

                    case TableContraints.CONSTR_BETWEEN:
                        {
                            int localCompare7() { var argb = Op1; var ret = DMD.Arrays.Compare(value, argb); Op1 = argb; return ret; }

                            int localCompare8() { var argb = Op2; var ret = DMD.Arrays.Compare(value, argb); Op2 = argb; return ret; }

                            ret = localCompare7() >= 0 & localCompare8() <= 0;
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }

                return ret;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Espressione = reader.Read("Espressione", this.m_Espressione);
                this.m_TipoVincolo = reader.Read("TipoVincolo", this.m_TipoVincolo);
                this.m_TipoValore = reader.Read("TipoValore", this.m_TipoValore);
                this.m_Op1 = Sistema.Types.CastTo(reader.Read("Op1", ""), m_TipoValore);
                this.m_Op2 = Sistema.Types.CastTo(reader.Read("Op2", ""), m_TipoValore);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Espressione", m_Espressione);
                writer.Write("TipoVincolo", m_TipoVincolo);
                writer.Write("TipoValore", m_TipoValore);
                writer.Write("Op1", Sistema.Formats.ToString(m_Op1));
                writer.Write("Op2", Sistema.Formats.ToString(m_Op2));
                return base.SaveToRecordset(writer);
            }

            public override string ToString()
            {
                string ret = Espressione;
                switch (TipoVincolo)
                {
                    case TableContraints.CONSTR_NE:
                        {
                            ret += " <> " + DMD.Strings.CStr(Op1);
                            break;
                        }

                    case TableContraints.CONSTR_LT:
                        {
                            ret += " < " + DMD.Strings.CStr(Op1);
                            break;
                        }

                    case TableContraints.CONSTR_LE:
                        {
                            ret += " <= " + DMD.Strings.CStr(Op1);
                            break;
                        }

                    case TableContraints.CONSTR_EQ:
                        {
                            ret += " = " + DMD.Strings.CStr(Op1);
                            break;
                        }

                    case TableContraints.CONSTR_GE:
                        {
                            ret += " >= " + DMD.Strings.CStr(Op1);
                            break;
                        }

                    case TableContraints.CONSTR_GT:
                        {
                            ret += " > " + DMD.Strings.CStr(Op1);
                            break;
                        }

                    case TableContraints.CONSTR_LIKE:
                        {
                            ret += " ~ " + DMD.Strings.CStr(Op1);
                            break;
                        }

                    case TableContraints.CONSTR_ANY:
                        {
                            ret += " Contiene almeno un " + DMD.Strings.CStr(Op1);
                            break;
                        }

                    case TableContraints.CONSTR_ALL:
                        {
                            ret += " Contiene tutti " + DMD.Strings.CStr(Op1);
                            break;
                        }

                    case TableContraints.CONSTR_BETWEEN:
                        {
                            ret += " tra " + DMD.Strings.CStr(Op1) + " e " + DMD.Strings.CStr(Op2);
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }

                return ret;
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Espressione", this.m_Espressione);
                writer.WriteAttribute("TipoValore", (int?)this.m_TipoValore);
                writer.WriteAttribute("TipoVincolo", (int?)this.m_TipoVincolo);
                writer.WriteAttribute("Op1", DMD.Strings.CStr(this.m_Op1));
                writer.WriteAttribute("Op2", DMD.Strings.CStr(this.m_Op2));
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Espressione":
                        {
                            m_Espressione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoValore":
                        {
                            m_TipoValore = (TypeCode)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoVincolo":
                        {
                            m_TipoVincolo = (TableContraints)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Op1":
                        {
                            m_Op1 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Op2":
                        {
                            m_Op2 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public override CModulesClass GetModule()
            {
                throw new NotImplementedException();
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDGenericContraint";
            }
        }
    }
}