using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    namespace repositories
    {


        /// <summary>
        /// Repository di oggetti di tipo <see cref="CCanale"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CCanaliClass 
            : CModulesClass<Anagrafica.CCanale>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCanaliClass() 
                : base("modCanali", typeof(Anagrafica.CCanaleCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce il canale per nome
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public Anagrafica.CCanale GetItemByName(string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                foreach (Anagrafica.CCanale ret in LoadAll())
                {
                    if (DMD.Strings.Compare(ret.Nome, value, true) == 0)
                        return ret;
                }

                return null;
            }

            /// <summary>
            /// Restituisce il canaler per tipo e nome
            /// </summary>
            /// <param name="tipo"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public Anagrafica.CCanale GetItemByName(string tipo, string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                foreach (Anagrafica.CCanale ret in LoadAll())
                {
                    if (
                            DMD.Strings.Compare(ret.Tipo, tipo, true) == 0 
                        &&  DMD.Strings.Compare(ret.Nome, value, true) == 0
                        )
                        return ret;
                }

                return null;
            }

            /// <summary>
            /// Restituisce un array contenente tutti i tipi di canale registrati
            /// </summary>
            /// <param name="onlyValid"></param>
            /// <returns></returns>
            public string[] GetTipiCanale(bool onlyValid = true)
            {
                var ret = DMD.Arrays.Empty<string>();
                foreach (Anagrafica.CCanale item in LoadAll())
                {
                    if (DMD.Arrays.BinarySearch(ret, item.Tipo) < 0)
                        ret = DMD.Arrays.InsertSorted(ret, item.Tipo);
                }

                return ret;
            }
        }
    }

    public partial class Anagrafica
    {
        private static CCanaliClass m_Canali = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CCanale"/>
        /// </summary>
        public static CCanaliClass Canali
        {
            get
            {
                if (m_Canali is null)
                    m_Canali = new CCanaliClass();
                return m_Canali;
            }
        }
    }
}