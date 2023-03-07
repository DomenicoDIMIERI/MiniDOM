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
using static minidom.Store;


namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti <see cref="Articolo"/>
        /// </summary>
        [Serializable]
        public partial class CArticoliClass
            : CModulesClass<Articolo>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CArticoliClass() 
                : base("modOfficeArticoli", typeof(ArticoloCursor), 0)
            {
            }

            //protected override CModule CreateModuleInfo()
            //{
            //    var m = base.CreateModuleInfo();
            //    m.Parent = minidom.Office.Module;
            //    m.Visible = true;
            //    m.Save();
            //    return m;
            //}

            /// <summary>
            /// Cerca gli articoli in base al codice articolo o alla descrizione
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public CCollection<Articolo> Find(string text)
            {
                var ret = new CCollection<Articolo>();
                text = Strings.Trim(text);
                if (string.IsNullOrEmpty(text))
                    return ret;

                using (var cursor = new ArticoloCursor())
                {
                    cursor.ValoreCodice.Value = text;
                    cursor.ValoreCodice.Operator = OP.OP_LIKE;
                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }
                }

                if (ret.Count == 0)
                {
                    using (var cursor = new ArticoloCursor())
                    {
                        cursor.Clear();
                        cursor.Nome.Value = text;
                        cursor.Nome.Operator = OP.OP_LIKE;
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

    public partial class Store
    {
        private static CArticoliClass m_Articoli = null;

        /// <summary>
        /// Repository di oggetti <see cref="Articolo"/>
        /// </summary>
        public static CArticoliClass Articoli
        {
            get
            {
                if (m_Articoli is null)
                    m_Articoli = new CArticoliClass();
                return m_Articoli;
            }
        }
    }
}