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
        /// Repository di oggetti di tipo <see cref="CCQSPDCessionarioClass"/>
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class CCessionariClass
            : CModulesClass<Finanziaria.CCQSPDCessionarioClass>
        {
            [NonSerialized]
            private Finanziaria.CCQSPDCessionarioClass _default = null;


            /// <summary>
            /// Costruttore
            /// </summary>
            public CCessionariClass() 
                : base("modCQSPDCessionari", typeof(Finanziaria.CCessionariCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce o imposta un riferimento al cessionario predefinito
            /// </summary>
            /// <returns></returns>
            public Finanziaria.CCQSPDCessionarioClass Default
            {
                get
                {
                    if (_default is null)
                    {
                        int idDefaultCQS = Sistema.ApplicationContext.Settings.GetValueInt("CQSPD.Cessionari.DefaultCessionarioID", 0);
                        _default = GetItemById(idDefaultCQS);
                    }

                    if (_default is null || _default.Stato != ObjectStatus.OBJECT_VALID || !_default.IsValid())
                    {
                        var items = LoadAll();
                        foreach (Finanziaria.CCQSPDCessionarioClass c in items)
                        {
                            if (c.UsabileInPratiche && c.IsValid())
                            {
                                _default = c;
                                break;
                            }
                        }
                    }

                    return _default;
                }

                set
                {
                    if (ReferenceEquals(_default, value))
                        return;
                    _default = value;
                }
            }


            /// <summary>
            /// Restituisce il cessionario in base al suo nome.
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Finanziaria.CCQSPDCessionarioClass GetItemByName(string value)
            {
                value = Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                foreach (Finanziaria.CCQSPDCessionarioClass ret in LoadAll())
                {
                    if (DMD.Strings.Compare(value, ret.Nome, true) == 0)
                        return ret;
                }

                return null;
            }



            /// <summary>
            /// Restituisce un array contenente l'elenco di tutti i cessionari attivi ed inattivi
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public Finanziaria.CCQSPDCessionarioClass[] GetArrayCessionari()
            {
                return GetAllCessionari().ToArray();
            }

            /// <summary>
            /// Restituisce un array base 0 contenente tutti gli oggetti CCessionario validi
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public Finanziaria.CCQSPDCessionarioClass[] GetArrayCessionariValidi()
            {
                var ret = new CCollection<Finanziaria.CCQSPDCessionarioClass>();
                foreach (Finanziaria.CCQSPDCessionarioClass c in LoadAll())
                {
                    if (c.IsValid())
                        ret.Add(c);
                }

                return ret.ToArray();
            }

            /// <summary>
            /// Restituisce la collezione di tutti i cessionari
            /// </summary>
            /// <returns></returns>
            public CCollection<Finanziaria.CCQSPDCessionarioClass> GetAllCessionari()
            {
                return new CCollection<Finanziaria.CCQSPDCessionarioClass>(LoadAll());
            }

            /// <summary>
            /// Aggionra l'oggetto nella cache
            /// </summary>
            /// <param name="item"></param>
            public override void UpdateCached(CCQSPDCessionarioClass item)
            {
                if (_default is object && DBUtils.GetID(_default, 0) == DBUtils.GetID(item, 0))
                {
                    _default = item;
                }

                base.UpdateCached(item);
            }
        }
    }

    public partial class Finanziaria
    {

        private static CCessionariClass m_Cessionari = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CCQSPDCessionarioClass"/>
        /// </summary>
        public static CCessionariClass Cessionari
        {
            get
            {
                if (m_Cessionari is null)
                    m_Cessionari = new CCessionariClass();
                return m_Cessionari;
            }
        }
    }
}