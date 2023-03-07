using System;
using DMD;
using DMD.Databases;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using DMD.Databases.Collections;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti <see cref="ContoCorrente"/>
        /// </summary>
        [Serializable]
        public sealed class CContiCorrenteClass 
            : CModulesClass<Anagrafica.ContoCorrente>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CContiCorrenteClass() 
                : base("modContiCorrente", typeof(Anagrafica.ContoCorrenteCursor), 0)
            {
            }

            private CItestatariClass m_Intestatari = null;

            /// <summary>
            /// Repository di oggetti <see cref="IntestatarioContoCorrente"/>
            /// </summary>
            public CItestatariClass Intestatari
            {
                get
                {
                    if (this.m_Intestatari is null) this.m_Intestatari = new CItestatariClass();
                    return this.m_Intestatari;
                }
            }

            /// <summary>
            /// Interpreta il codice IBAN
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public string ParseIBAN(string value)
            {
                return DMD.Strings.Replace(value, " ", "");
            }

            /// <summary>
            /// Interpreta il numero di conto
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public string ParseNumero(string value)
            {
                return DMD.Strings.Replace(value, " ", "");
            }

            /// <summary>
            /// Restituisce l'elemento in base al nome
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Anagrafica.ContoCorrente GetItemByName(string name)
            {
                name = DMD.Strings.Trim(name);
                if (string.IsNullOrEmpty(name))
                    return null;
                
                using(var cursor = new Anagrafica.ContoCorrenteCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Nome.Value = name;
                    cursor.StatoContoCorrente.SortOrder = SortEnum.SORT_ASC;
                    var ret = cursor.Item;
                    return ret;
                }
                
            }

            /// <summary>
            /// Restituisce l'elemento in base al numero di conto
            /// </summary>
            /// <param name="banca"></param>
            /// <param name="numero"></param>
            /// <returns></returns>
            public Anagrafica.ContoCorrente GetItemByNumero(Anagrafica.CBanca banca, string numero)
            {
                if (DBUtils.GetID(banca, 0) == 0)
                    return null;
                numero = ParseNumero(numero);
                if (string.IsNullOrEmpty(numero))
                    return null;

                using (var cursor = new Anagrafica.ContoCorrenteCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Numero.Value = numero;
                    cursor.IDBanca.Value = DBUtils.GetID(banca, 0);
                    return cursor.Item;
                }                 
            }

            /// <summary>
            /// Restituisce tutti i conti correnti corrispondenti
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public CCollection<Anagrafica.ContoCorrente> Find(string text)
            {
                var ret = new CCollection<Anagrafica.ContoCorrente>();
                text = DMD.Strings.Trim(text);
                if (string.IsNullOrEmpty(text))
                    return ret;

                using (var cursor = new Anagrafica.ContoCorrenteCursor())
                {
                    cursor.Nome.Value = text + "%";
                    cursor.Nome.Operator = OP.OP_LIKE;
                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }

                    cursor.Reset1();
                    if (ret.Count == 0)
                    {
                        cursor.Clear();
                        cursor.Numero.Value = "%" + text + "%";
                        cursor.Numero.Operator = OP.OP_LIKE;
                        while (cursor.Read())
                        {
                            ret.Add(cursor.Item);
                        }
                    }

                }

                return ret;
            }
        }
    }

    public partial class Anagrafica
    {
        private static CContiCorrenteClass m_ContiCorrente = null;

        /// <summary>
        /// Repository di oggetti <see cref="ContoCorrente"/>
        /// </summary>
        public static CContiCorrenteClass ContiCorrente
        {
            get
            {
                if (m_ContiCorrente is null)
                    m_ContiCorrente = new CContiCorrenteClass();
                return m_ContiCorrente;
            }
        }
    }
}