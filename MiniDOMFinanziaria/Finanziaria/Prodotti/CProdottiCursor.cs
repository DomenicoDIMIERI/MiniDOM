using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{

    [Serializable]
    public partial class Finanziaria
    {

        [Serializable]
        public class CProdottiCursor 
            : Databases.DBObjectCursor<CCQSPDProdotto>
        {
            private DBCursorField<int> m_CessionarioID = new DBCursorField<int>("Cessionario");
            private DBCursorStringField m_NomeCessionario = new DBCursorStringField("NomeCessionario");
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<int> m_GruppoProdottiID = new DBCursorField<int>("GruppoProdotti");
            private DBCursorStringField m_TipoRapporto = new DBCursorStringField("IdTipoRapporto");
            private DBCursorStringField m_TipoContratto = new DBCursorStringField("IdTipoContratto");
            private DBCursorField<bool> m_Visibile = new DBCursorField<bool>("Visibile");
            private DBCursorField<int> m_IDStatoIniziale = new DBCursorField<int>("IDStatoIniziale");
            private DBCursorField<ProdottoFlags> m_Flags = new DBCursorField<ProdottoFlags>("Flags");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorStringField m_Categoria = new DBCursorStringField("Idcategoria");
            private DBCursorField<int> m_IDListino = new DBCursorField<int>("IDListino");
            private bool m_OnlyValid;

            public CProdottiCursor()
            {
                m_OnlyValid = false;
            }

            protected override void OnInitialize(object item)
            {
                {
                    var withBlock = (CCQSPDProdotto)item;
                    withBlock.Nome = "Nuovo prodotto";
                }

                base.OnInitialize(item);
            }

            public DBCursorField<int> IDListino
            {
                get
                {
                    return m_IDListino;
                }
            }

            public DBCursorField<DateTime> DataInizio
            {
                get
                {
                    return m_DataInizio;
                }
            }

            public DBCursorField<DateTime> DataFine
            {
                get
                {
                    return m_DataFine;
                }
            }

            public DBCursorField<ProdottoFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            public DBCursorField<int> IDStatoIniziale
            {
                get
                {
                    return m_IDStatoIniziale;
                }
            }

            public bool OnlyValid
            {
                get
                {
                    return m_OnlyValid;
                }

                set
                {
                    if (m_OnlyValid == value)
                        return;
                    m_OnlyValid = value;
                    Reset1();
                }
            }

            public DBCursorField<bool> Visible
            {
                get
                {
                    return m_Visibile;
                }
            }

            public DBCursorField<int> CessionarioID
            {
                get
                {
                    return m_CessionarioID;
                }
            }

            public DBCursorStringField NomeCessionario
            {
                get
                {
                    return m_NomeCessionario;
                }
            }

            public DBCursorField<int> GruppoProdottiID
            {
                get
                {
                    return m_GruppoProdottiID;
                }
            }

            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            public DBCursorStringField Categoria
            {
                get
                {
                    return m_Categoria;
                }
            }

            public override string GetTableName()
            {
                return "tbl_Prodotti";
            }

            protected override Sistema.CModule GetModule()
            {
                return Prodotti.Module;
            }

            public DBCursorStringField TipoRapporto
            {
                get
                {
                    return m_TipoRapporto;
                }
            }

            public DBCursorStringField TipoContratto
            {
                get
                {
                    return m_TipoContratto;
                }
            }

            protected override CKeyCollection<Databases.DBCursorField> GetWhereFields()
            {
                var ret = base.GetWhereFields();
                ret.Remove(m_IDListino);
                return ret;
            }

            public override string GetWherePart()
            {
                string wherePart = base.GetWherePart();
                if (m_OnlyValid)
                {
                    wherePart = DMD.Strings.Combine(wherePart, "(([DataInizio] Is Null) Or ([DataInizio]<=" + DBUtils.DBDate(DMD.DateUtils.ToDay()) + "))", " AND ");
                    wherePart = DMD.Strings.Combine(wherePart, "(([DataFine] Is Null) Or ([DataFine]>=" + DBUtils.DBDate(DMD.DateUtils.ToDay()) + "))", " AND ");
                }

                if (m_IDListino.IsSet())
                {
                    string tmpSQL = "SELECT DISTINCT(Prodotto) FROM tbl_PreventivatoriXProdotto WHERE " + m_IDListino.GetSQL("Preventivatore") + " AND [Stato]=1";
                    wherePart = DMD.Strings.Combine(wherePart, " [ID] In (" + tmpSQL + ")", " AND ");
                }

                return wherePart;
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CCQSPDProdotto();
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("OnlyValid", m_OnlyValid);
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "OnlyValid":
                        {
                            m_OnlyValid = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
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