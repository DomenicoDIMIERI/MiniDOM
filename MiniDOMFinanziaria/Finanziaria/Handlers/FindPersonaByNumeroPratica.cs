using System.Collections;
using DMD;

namespace minidom
{
    public partial class Finanziaria
    {
        public class FindPersonaByNumeroPratica : Anagrafica.FindPersonaHandler
        {
            public FindPersonaByNumeroPratica()
            {
            }

            public override bool CanHandle(string param, Anagrafica.CRMFindParams filter)
            {
                return false; // IsNumeric(param) And Len(param) >= 4
            }

            public override void Find(string param, Anagrafica.CRMFindParams filter, CCollection<Anagrafica.CPersonaInfo> ret)
            {
                param = Strings.LCase(Strings.Trim(param));
                if (Strings.Len(Strings.Trim(param)) < 3)
                    return;
                var list = new ArrayList();
                string dbSQL = "SELECT [Cliente] FROM [tbl_Pratiche] WHERE [StatRichD_Params] Like '" + param + "%' AND [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
                var dbRis = Database.ExecuteReader(dbSQL);
                int id;
                if (dbRis.Read())
                {
                    id = Sistema.Formats.ToInteger(dbRis["Cliente"]);
                    if (id != 0)
                        list.Add(id);
                }

                dbRis.Dispose();
                dbSQL = "SELECT [IDPersona] FROM [tbl_Estinzioni] WHERE [Numero] Like '" + param + "%' AND [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
                dbRis = Database.ExecuteReader(dbSQL);
                if (dbRis.Read())
                {
                    id = Sistema.Formats.ToInteger(dbRis["IDPersona"]);
                    if (id != 0)
                        list.Add(id);
                }

                dbRis.Dispose();
                if (list.Count == 0)
                    return;
                var cursor = new Anagrafica.CPersonaFisicaCursor();
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.IgnoreRights = filter.ignoreRights;
                int[] arr = (int[])list.ToArray(typeof(int));
                cursor.ID.ValueIn(arr);
                if (filter.IDPuntoOperativo != 0)
                    cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                if (filter.tipoPersona.HasValue)
                    cursor.TipoPersona.Value = filter.tipoPersona.Value;
                if (filter.flags.HasValue)
                {
                    cursor.PFlags.Value = filter.flags.Value;
                    cursor.PFlags.Operator = Databases.OP.OP_ALLBITAND;
                }

                int cnt = 0;
                while (!cursor.EOF() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                {
                    ret.Add(new Anagrafica.CPersonaInfo(cursor.Item));
                    cursor.MoveNext();
                    cnt += 1;
                }

                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }
            }

            public override string GetHandledCommand()
            {
                return "Numero Pratica";
            }
        }
    }
}