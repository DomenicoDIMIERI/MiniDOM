using DMD.Databases.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Provider predefinito delle fonti
        /// </summary>
        /// <remarks></remarks>
        public sealed class FontiDefaultProvider 
            : IFonteProvider
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public FontiDefaultProvider()
            {
                ////DMDObject.IncreaseCounter(this);
            }

            /// <summary>
            /// Restituisce l'array delle fonti di tipo specifico
            /// </summary>
            /// <param name="tipo"></param>
            /// <param name="onlyValid"></param>
            /// <returns></returns>
            public IFonte[] GetItemsAsArray(string tipo, bool onlyValid = true)
            {
                tipo = DMD.Strings.Trim(tipo);

                var ret = new List<IFonte>();                 
                foreach(var f in minidom.Anagrafica.Fonti.LoadAll())
                {
                    if (string.IsNullOrEmpty(tipo) || DMD.Strings.EQ(f.Tipo, tipo, true))
                        ret.Add(f);
                }

                return ret.ToArray();
            }

            /// <summary>
            /// Restituisce la fonte in base all'id
            /// </summary>
            /// <param name="tipo"></param>
            /// <param name="id"></param>
            /// <returns></returns>
            public IFonte GetItemById(string tipo, int id)
            {
                return minidom.Anagrafica.Fonti.GetItemById(id);
            }

            /// <summary>
            /// Restituisce la fonte in base al nome
            /// </summary>
            /// <param name="tipo"></param>
            /// <param name="name"></param>
            /// <returns></returns>
            public IFonte GetItemByName(string tipo, string name)
            {
                return minidom.Anagrafica.Fonti.GetItemByName(name);
            }

            /// <summary>
            /// Restituisce i tipi di fonte supportati
            /// </summary>
            /// <returns></returns>
            public string[] GetSupportedNames()
            {
                var items = new Dictionary<string, string>();
                foreach (var f in minidom.Anagrafica.Fonti.LoadAll())
                {
                    string t = f.Tipo;
                    if (items.ContainsKey(t) == false)
                        items.Add(t, t);
                }

                string[] ret = null;
                if (items.Count > 0)
                {
                    ret = new string[items.Count];
                    items.Keys.CopyTo(ret, 0);
                    Array.Sort(ret);
                }

                items = null;
                return ret;
            }

            /// <summary>
            /// Distruttore
            /// </summary>
            ~FontiDefaultProvider()
            {
                ////DMDObject.DecreaseCounter(this);
            }
        }
    }
}