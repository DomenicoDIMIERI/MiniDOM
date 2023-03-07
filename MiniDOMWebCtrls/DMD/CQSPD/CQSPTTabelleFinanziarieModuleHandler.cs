using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{
    public class CQSPTTabelleFinanziarieModuleHandler : CBaseModuleHandler
    {
        public CQSPTTabelleFinanziarieModuleHandler()
        {
        }

        public override object GetInternalItemById(int id)
        {
            return Finanziaria.TabelleFinanziarie.GetItemById(id);
        }

        public string GetTabellaXProdottoByID(object renderer)
        {
            int id = (int)this.n2int(renderer, "id");
            var item = Finanziaria.TabelleFinanziarie.GetTabellaXProdottoByID(id);
            if (item is null)
                return "";
            return DMD.XML.Utils.Serializer.Serialize(item, XMLSerializeMethod.Document);
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CTabelleFinanziarieCursor();
        }

        public string CreateElencoUnusedTblFin(object renderer)
        {
            int idProdotto = (int)this.n2int(renderer, "pid");
            var writer = new System.Text.StringBuilder();
            if (idProdotto != 0)
            {

                using (var dbRis = Finanziaria.Database.ExecuteReader("SELECT * FROM [tbl_FIN_TblFin] WHERE ([Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString() + ") And Not [ID] In (SELECT [Tabella] FROM [tbl_FIN_ProdXTabFin] WHERE ([Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString() + ") And ([Prodotto]=" + idProdotto + ") GROUP BY [Tabella]) ORDER BY [Nome] ASC"))
                {
                    while (dbRis.Read())
                    {
                        writer.Append("<option value=\"");
                        writer.Append(Sistema.Formats.ToInteger(dbRis["ID"]));
                        writer.Append("\">");
                        writer.Append(Sistema.Formats.ToString(dbRis["Nome"]));
                        writer.Append(" (");
                        writer.Append(Sistema.Formats.ToString(dbRis["NomeCessionario"]));
                        writer.Append(")</option>");
                    }
                }
            }

            return writer.ToString();
        }

        public string CreateElencoUsedTblFin(object renderer)
        {
            // Dim items As CCollection = Finanziaria.TabelleFinanziarie.GetUsed(idProdotto)
            var writer = new System.Text.StringBuilder();
            int idProdotto = (int)this.n2int(renderer, "pid");
            if (idProdotto != 0)
            {
                // dbRis = Finanziaria.Database.ExecuteReader("SELECT * FROM [tbl_FIN_TblFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And [ID] In (SELECT DISTINCT [Tabella] FROM [tbl_FIN_ProdXTabFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And ([Prodotto]=" & idProdotto & ")) ORDER BY [Nome] ASC")
                using (var dbRis = Finanziaria.Database.ExecuteReader("SELECT * FROM [tbl_FIN_TblFin] WHERE ([Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString() + ") And [ID] In (SELECT [Tabella] FROM [tbl_FIN_ProdXTabFin] WHERE ([Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString() + ") And ([Prodotto]=" + idProdotto + ") GROUP BY [Tabella]) ORDER BY [Nome] ASC"))
                {
                    while (dbRis.Read())
                    {
                        writer.Append("<option value=\"");
                        writer.Append(Sistema.Formats.ToInteger(dbRis["ID"]));
                        writer.Append("\">");
                        writer.Append(Sistema.Formats.ToString(dbRis["Nome"]));
                        writer.Append(" (");
                        writer.Append(Sistema.Formats.ToString(dbRis["NomeCessionario"]));
                        writer.Append(")</option>");
                    }
                }
            }

            return writer.ToString();
        }
    }
}