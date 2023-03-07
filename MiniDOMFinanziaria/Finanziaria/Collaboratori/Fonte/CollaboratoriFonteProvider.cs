
namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Provider delle fonti per gli oggetti di tipo <see cref="CCollaboratore"/>
        /// </summary>
        public class CollaboratoriFonteProvider 
            : Anagrafica.IFonteProvider
        {

            /// <summary>
            /// Restituisce il collaboratore in base all'id
            /// </summary>
            /// <param name="nome"></param>
            /// <param name="id"></param>
            /// <returns></returns>
            public  IFonte GetItemById(string nome, int id)
            {
                return minidom.Finanziaria.Collaboratori.GetItemById(id);
            }

            /// <summary>
            /// Restituisce il collaboratore in base al nome
            /// </summary>
            /// <param name="tipo"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public IFonte GetItemByName(string tipo, string value)
            {
                return minidom.Finanziaria.Collaboratori.GetItemByName(value);
            }

            /// <summary>
            /// Restituisce l'elenco dei collaboratori
            /// </summary>
            /// <param name="nome"></param>
            /// <param name="onlyValid"></param>
            /// <returns></returns>
            public IFonte[] GetItemsAsArray(string nome, bool onlyValid = true)
            {
                var ret = new CCollection<IFonte>();
                using (var cursor = new CCollaboratoriCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.OnlyValid = onlyValid;
                    cursor.NomePersona.SortOrder = SortEnum.SORT_ASC;
                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }
                    // ret.Sort()
                }

                return ret.ToArray();
            }

            /// <summary>
            /// Restituisce il tipo di oggetti supportato
            /// </summary>
            /// <returns></returns>
            public string[] GetSupportedNames()
            {
                return new string[] { "Collaboratori" };
            }
        }
    }
}