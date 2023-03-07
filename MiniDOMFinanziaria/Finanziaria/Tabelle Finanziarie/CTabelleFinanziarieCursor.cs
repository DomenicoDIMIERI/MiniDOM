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
        /// Cursore per muoversi all'interno della tabella delle tabelle finanziarie
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CTabelleFinanziarieCursor 
            : Databases.DBObjectCursor<CTabellaFinanziaria>
        {
            private DBCursorField<int> m_CessionarioID = new DBCursorField<int>("Cessionario");
            private DBCursorStringField m_NomeCessionario = new DBCursorStringField("NomeCessionario");
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<bool> m_TANVariabile = new DBCursorField<bool>("TANVariabile");
            private DBCursorField<double> m_TAN = new DBCursorField<double>("TAN");
            private DBCursorField<DateTime> m_DataInizio = new DBCursorField<DateTime>("DataInizio");
            private DBCursorField<DateTime> m_DataFine = new DBCursorField<DateTime>("DataFine");
            private DBCursorField<bool> m_Visible = new DBCursorField<bool>("Visible");
            private DBCursorField<double> m_ProvvMax = new DBCursorField<double>("ProvvMax");
            private DBCursorField<double> m_ProvvMaxConRinnovi = new DBCursorField<double>("ProvvMaxRinn");
            private DBCursorField<double> m_ProvvMaxConEstinzioni = new DBCursorField<double>("ProvvMaxEst");
            private DBCursorField<double> m_Sconto = new DBCursorField<double>("Sconto");
            private DBCursorField<TabellaFinanziariaFlags> m_Flags = new DBCursorField<TabellaFinanziariaFlags>("Flags");
            private DBCursorField<decimal> m_UpFrontMax = new DBCursorField<decimal>("UpFrontMax");
            private DBCursorField<TipoCalcoloProvvigioni> m_TipoCalcoloProvvigioni = new DBCursorField<TipoCalcoloProvvigioni>("TipoCalcoloProvvigioni");
            private DBCursorStringField m_FormulaProvvigioni = new DBCursorStringField("FormulaProvvigioni");
            private DBCursorField<double> m_ScontoVisibile = new DBCursorField<double>("ScontoVisibile");
            private DBCursorField<double> m_ProvvAggVisib = new DBCursorField<double>("ProvvAggVisib");
            private DBCursorField<TipoCalcoloProvvigioni> m_TipoCalcoloProvvTAN = new DBCursorField<TipoCalcoloProvvigioni>("TipoCalcoloProvvTAN");
            private DBCursorField<double> m_ProvvTANR = new DBCursorField<double>("ProvvTANR");
            private DBCursorField<double> m_ProvvTANE = new DBCursorField<double>("ProvvTANE");
            private DBCursorStringField m_Categoria = new DBCursorStringField("Categoria");
            private bool m_OnlyValid;


            /// <summary>
            /// Costruttore
            /// </summary>
            public CTabelleFinanziarieCursor()
            {
                m_OnlyValid = false;
            }

            public DBCursorStringField Categoria
            {
                get
                {
                    return m_Categoria;
                }
            }

            /// <summary>
        /// Provvigione TAN in caso di rinnovi
        /// </summary>
        /// <returns></returns>
            public DBCursorField<double> ProvvTANR
            {
                get
                {
                    return m_ProvvTANR;
                }
            }

            /// <summary>
        /// Provvigione TAN in caso di estinzioni
        /// </summary>
        /// <returns></returns>
            public DBCursorField<double> ProvvTANE
            {
                get
                {
                    return m_ProvvTANE;
                }
            }

            public DBCursorField<TipoCalcoloProvvigioni> TipoCalcoloProvvTAN
            {
                get
                {
                    return m_TipoCalcoloProvvTAN;
                }
            }

            /// <summary>
        /// Provvigione aggiuntiva visibile
        /// </summary>
        /// <returns></returns>
            public DBCursorField<double> ProvvAggVisib
            {
                get
                {
                    return m_ProvvAggVisib;
                }
            }

            protected override Sistema.CModule GetModule()
            {
                return TabelleFinanziarie.Module; // ("modCQSPDTblFinanz")
            }

            public bool OnlyValid
            {
                get
                {
                    return m_OnlyValid;
                }

                set
                {
                    m_OnlyValid = value;
                }
            }

            public DBCursorField<TipoCalcoloProvvigioni> TipoCalcoloProvvigioni
            {
                get
                {
                    return m_TipoCalcoloProvvigioni;
                }
            }

            public DBCursorStringField FormulaProvvigioni
            {
                get
                {
                    return m_FormulaProvvigioni;
                }
            }

            public DBCursorField<double> Sconto
            {
                get
                {
                    return m_Sconto;
                }
            }

            public DBCursorField<TabellaFinanziariaFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            public DBCursorField<double> ProvvMax
            {
                get
                {
                    return m_ProvvMax;
                }
            }

            public DBCursorField<bool> Visible
            {
                get
                {
                    return m_Visible;
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

            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            public DBCursorField<bool> TANVariabile
            {
                get
                {
                    return m_TANVariabile;
                }
            }

            public DBCursorField<double> TAN
            {
                get
                {
                    return m_TAN;
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

            public DBCursorField<double> ProvvMaxConRinnovi
            {
                get
                {
                    return m_ProvvMaxConRinnovi;
                }
            }

            public DBCursorField<double> ProvvMaxConEstinzioni
            {
                get
                {
                    return m_ProvvMaxConEstinzioni;
                }
            }

            public override string GetTableName()
            {
                return "tbl_FIN_TblFin";
            }

            public override object InstantiateNew(DBReader dbRis)
            {
                return new CTabellaFinanziaria();
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

            public DBCursorField<double> ScontoVisibile
            {
                get
                {
                    return m_ScontoVisibile;
                }
            }

            public override string GetWherePart()
            {
                string wherePart = base.GetWherePart();
                if (m_OnlyValid)
                {
                    wherePart = DMD.Strings.Combine(wherePart, "(([DataInizio] Is Null) Or ([DataInizio]<=" + DBUtils.DBDate(DMD.DateUtils.ToDay()) + "))", " AND ");
                    wherePart = DMD.Strings.Combine(wherePart, "(([DataFine] Is Null) Or ([DataFine]>=" + DBUtils.DBDate(DMD.DateUtils.ToDay()) + "))", " AND ");
                }

                return wherePart;
            }
        }
    }
}