
using DMD.Databases;
using System.Collections.Generic;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Rappresenta una sorgente di fonti
        /// </summary>
        public class DistributoriFonteProvider 
            : IFonteProvider
        {

            /// <summary>
            /// Restituisce la fonte in base all'id
            /// </summary>
            /// <param name="nome"></param>
            /// <param name="id"></param>
            /// <returns></returns>
            public IFonte GetItemById(string nome, int id)
            {
                return Distributori.GetItemById(id);
            }

            /// <summary>
            /// Restituisce la fonte in base all'id
            /// </summary>
            /// <param name="tipo"></param>
            /// <param name="name"></param>
            /// <returns></returns>
            public IFonte GetItemByName(string tipo, string name)
            {
                return Distributori.GetItemByName(name);
            }

            /// <summary>
            /// Restituisce le fonti in base al tipo
            /// </summary>
            /// <param name="nome"></param>
            /// <param name="onlyValid"></param>
            /// <returns></returns>
            public IFonte[] GetItemsAsArray(string nome, bool onlyValid = true)
            {
                var ret = new List<IFonte>();
                using (var cursor = new CDistributoriCursor())
                {
                    cursor.OnlyValid = onlyValid;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Nome.SortOrder = SortEnum.SORT_ASC;
                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }
                }
                return ret.ToArray();
            }

            /// <summary>
            /// Restituisce i tipo di fonte supportati
            /// </summary>
            /// <returns></returns>
            public string[] GetSupportedNames()
            {
                return new string[] { "Distributori" };
            }
        }
    }
}