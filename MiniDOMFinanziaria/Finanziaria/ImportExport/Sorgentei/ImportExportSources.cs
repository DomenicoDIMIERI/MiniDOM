using minidom.internals;

namespace minidom
{
    namespace internals
    {
        public sealed class CImportExportSourcesClass : CModulesClass<Finanziaria.CImportExportSource>
        {
            internal CImportExportSourcesClass() : base("modCQSPDImportExportSrc", typeof(Finanziaria.CImportExportSourceCursor), -1)
            {
            }

            public Finanziaria.CImportExportSource GetItemByName(string name)
            {
                name = DMD.Strings.Trim(name);
                if (string.IsNullOrEmpty(name))
                    return null;
                var items = LoadAll();
                foreach (Finanziaria.CImportExportSource src in items)
                {
                    if ((src.Name ?? "") == (name ?? ""))
                        return src;
                }

                return null;
            }
        }
    }

    public partial class Finanziaria
    {
        private static CImportExportSourcesClass m_ImportExportSources = null;

        public static CImportExportSourcesClass ImportExportSources
        {
            get
            {
                if (m_ImportExportSources is null)
                    m_ImportExportSources = new CImportExportSourcesClass();
                return m_ImportExportSources;
            }
        }
    }
}