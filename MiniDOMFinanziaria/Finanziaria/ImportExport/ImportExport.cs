using System;
using minidom.internals;

namespace minidom
{
    namespace internals
    {
        public sealed class CImportExportClass : CModulesClass<Finanziaria.CImportExport>
        {
            internal CImportExportClass() : base("modCQSPDImportExport", typeof(Finanziaria.CImportExportCursor))
            {
            }

            public Finanziaria.CImportExport GetItemByKey(Finanziaria.CImportExportSource src, string sharedKey)
            {
                if (src is null)
                    throw new ArgumentNullException("src");
                if (string.IsNullOrEmpty(sharedKey))
                    return null;
                var cursor = new Finanziaria.CImportExportCursor();
                cursor.IgnoreRights = true;
                cursor.SourceID.Value = DBUtils.GetID(src);
                cursor.SharedKey.Value = sharedKey;
                cursor.ID.SortOrder = SortEnum.SORT_DESC;
                var ret = cursor.Item;
                cursor.Dispose();
                return ret;
            }
        }
    }

    public partial class Finanziaria
    {
        private static CImportExportClass m_ImportExport = null;

        public static CImportExportClass ImportExport
        {
            get
            {
                if (m_ImportExport is null)
                    m_ImportExport = new CImportExportClass();
                return m_ImportExport;
            }
        }
    }
}