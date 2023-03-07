using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Finanziaria;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CObiettivoPratica"/>
        /// </summary>
        [Serializable]
        public class CObiettivoPraticaClass 
            : CModulesClass<Finanziaria.CObiettivoPratica>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CObiettivoPraticaClass() 
                : base("modCQSPDObiettiviPratica", typeof(Finanziaria.CObiettivoPraticaCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce tutti gli obiettivi attivi e validi alla data indicata
            /// </summary>
            /// <param name="d"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Finanziaria.CObiettivoPratica> GetObiettiviAl(DateTime d)
            {
                var ret = new CCollection<Finanziaria.CObiettivoPratica>();
                lock (this)
                {
                    foreach (Finanziaria.CObiettivoPratica o in LoadAll())
                    {
                        if (o.Stato == ObjectStatus.OBJECT_VALID && o.IsValid(d))
                        {
                            ret.Add(o);
                        }
                    }
                }

                ret.Sort();
                return ret;
            }

            /// <summary>
            /// Restituisce gli obiettivi validi alla data per l'ufficio specificato
            /// </summary>
            /// <param name="po"></param>
            /// <param name="d"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Finanziaria.CObiettivoPratica> GetObiettiviAl(Anagrafica.CUfficio po, DateTime d)
            {
                var ret = new CCollection<Finanziaria.CObiettivoPratica>();
                if (po is null)
                    throw new ArgumentNullException("po");
                lock (this)
                {
                    foreach (Finanziaria.CObiettivoPratica o in LoadAll())
                    {
                        if (
                            o.Stato == ObjectStatus.OBJECT_VALID 
                            && 
                            (o.IDPuntoOperativo == 0 || o.IDPuntoOperativo == DBUtils.GetID(po, 0)
                            ) && o.IsValid(d))
                        {
                            ret.Add(o);
                        }
                    }
                }

                ret.Sort();
                return ret;
            }

            /// <summary>
            /// Restituisce l'elemento in base al nome
            /// </summary>
            /// <param name="nome"></param>
            /// <returns></returns>
            public Finanziaria.CObiettivoPratica GetItemByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                if (string.IsNullOrEmpty(nome))
                    return null;
                foreach (Finanziaria.CObiettivoPratica o in LoadAll())
                {
                    if (DMD.Strings.Compare(o.Nome, nome, true) == 0)
                        return o;
                }

                return null;
            }
        }
    }

    public partial class Finanziaria
    {
        private static CObiettivoPraticaClass m_Obiettivi;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CObiettivoPratica"/>
        /// </summary>
        public static CObiettivoPraticaClass Obiettivi
        {
            get
            {
                if (m_Obiettivi is null)
                    m_Obiettivi = new CObiettivoPraticaClass();
                return m_Obiettivi;
            }
        }
    }
}