using System;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CAltriPrestitiCursor : Databases.DBObjectCursor<CAltroPrestito>
        {
            private Databases.CCursorField<TipoEstinzione> m_Tipo = new Databases.CCursorField<TipoEstinzione>("Tipo");
            private Databases.CCursorField<int> m_IDIstituto = new Databases.CCursorField<int>("IDIstituto");
            private Databases.CCursorFieldObj<string> m_NomeIstituto = new Databases.CCursorFieldObj<string>("NomeIstituto");
            private Databases.CCursorField<DateTime> m_DataInizio = new Databases.CCursorField<DateTime>("DataInizio");
            private Databases.CCursorField<DateTime> m_Scadenza = new Databases.CCursorField<DateTime>("Scadenza");
            private Databases.CCursorField<decimal> m_Rata = new Databases.CCursorField<decimal>("Rata");
            private Databases.CCursorField<int> m_Durata = new Databases.CCursorField<int>("Durata");
            private Databases.CCursorField<double> m_TAN = new Databases.CCursorField<double>("TAN");
            private Databases.CCursorField<int> m_IDPratica = new Databases.CCursorField<int>("IDPratica");
            private Databases.CCursorField<DateTime> m_DecorrenzaPratica = new Databases.CCursorField<DateTime>("DecorrenzaPratica");
            private Databases.CCursorField<int> m_IDPersona = new Databases.CCursorField<int>("IDPersona");
            private Databases.CCursorFieldObj<string> m_NomePersona = new Databases.CCursorFieldObj<string>("NomePersona");

            public CAltriPrestitiCursor()
            {
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public Databases.CCursorField<int> IDPersona
            {
                get
                {
                    return m_IDPersona;
                }
            }

            public Databases.CCursorFieldObj<string> NomePersona
            {
                get
                {
                    return m_NomePersona;
                }
            }

            public Databases.CCursorField<DateTime> DecorrenzaPratica
            {
                get
                {
                    return m_DecorrenzaPratica;
                }
            }

            public Databases.CCursorField<int> IDPratica
            {
                get
                {
                    return m_IDPratica;
                }
            }

            public Databases.CCursorField<TipoEstinzione> Tipo
            {
                get
                {
                    return m_Tipo;
                }
            }

            public Databases.CCursorField<int> IDIstituto
            {
                get
                {
                    return m_IDIstituto;
                }
            }

            public Databases.CCursorFieldObj<string> NomeIstituto
            {
                get
                {
                    return m_NomeIstituto;
                }
            }

            public Databases.CCursorField<DateTime> DataInizio
            {
                get
                {
                    return m_DataInizio;
                }
            }

            public Databases.CCursorField<DateTime> Scadenza
            {
                get
                {
                    return m_Scadenza;
                }
            }

            public Databases.CCursorField<decimal> Rata
            {
                get
                {
                    return m_Rata;
                }
            }

            public Databases.CCursorField<int> Durata
            {
                get
                {
                    return m_Durata;
                }
            }

            public Databases.CCursorField<double> TAN
            {
                get
                {
                    return m_TAN;
                }
            }

            public override object InstantiateNew(Databases.DBReader dbRis)
            {
                return new CEstinzione();
            }

            public override string GetTableName()
            {
                return "tbl_CQSPDAltriPrestiti";
            }

            protected override Sistema.CModule GetModule()
            {
                return AltriPrestiti.Module;
            }
        }
    }
}