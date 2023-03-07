using System;

namespace minidom.Forms
{
    public class COfficeDefPercorsiHandler : CBaseModuleHandler
    {
        public COfficeDefPercorsiHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.PercorsiDefinitiCursor();
        }

        public override object GetInternalItemById(int id)
        {
            return Office.PercorsiDefiniti.GetItemById(id);
        }

        public override CCollection<ExportableColumnInfo> GetExportableColumnsList()
        {
            var ret = base.GetExportableColumnsList();
            ret.Add(new ExportableColumnInfo("ID", "ID", TypeCode.Int32, true));
            ret.Add(new ExportableColumnInfo("Nome", "Nome", TypeCode.String, true));
            return ret;
        }

        public string GetPercorsiByPO(object renderer)
        {
            int idPO = (int)this.n2int(renderer, "po");
            // Dim cursor As New PercorsiDefinitiCursor
            try
            {
                var ret = new CCollection<Office.PercorsoDefinito>();
                // cursor.IgnoreRights = True
                // cursor.Nome.SortOrder = SortEnum.SORT_ASC
                // cursor.Attivo.Value = True
                // If (idPO <> 0) Then cursor.IDPuntoOperativo.Value = idPO
                // While Not cursor.EOF
                // ret.Add(cursor.Item)
                // cursor.MoveNext()
                // End While
                foreach (Office.PercorsoDefinito p in Office.PercorsiDefiniti.LoadAll())
                {
                    if (p.Stato == Databases.ObjectStatus.OBJECT_VALID && p.Attivo == true && (p.IDPuntoOperativo == 0 || p.IDPuntoOperativo == idPO))
                    {
                        ret.Add(p);
                    }
                }

                ret.Sort();
                return DMD.XML.Utils.Serializer.Serialize(ret);
            }
            catch (Exception ex)
            {
                Sistema.Events.NotifyUnhandledException(ex);
                throw;
            }
            finally
            {
                // If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            }
        }
    }
}