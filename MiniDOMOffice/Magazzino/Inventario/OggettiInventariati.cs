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
        /// Repository di oggetti <see cref="OggettoInventariato"/>
        /// </summary>
        [Serializable]
        public partial class COggettiInventariatiClass 
            : CModulesClass<OggettoInventariato>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public COggettiInventariatiClass() 
                : base("modOfficeOggettiInventariati", typeof(OggettoInventariatoCursor), 0)
            {
            }

            //protected override Sistema.CModule CreateModuleInfo()
            //{
            //    var m = base.CreateModuleInfo();
            //    m.Parent = Module;
            //    m.Visible = true;
            //    m.Save();
            //    return m;
            //}

            /// <summary>
            /// Restituisce una stringa che descrive il formato
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public string FormatStatoAttuale(StatoOggettoInventariato value)
            {
                switch (value)
                {
                    case StatoOggettoInventariato.Sconosciuto: return "Sconosciuto";
                    case StatoOggettoInventariato.Funzionante: return "Funzionante";
                    case StatoOggettoInventariato.ConQualcheDifetto: return "Con qualche difetto";
                    case StatoOggettoInventariato.DaRiparare: return "Da Riparare";
                    case StatoOggettoInventariato.ValutazioneRiparazione: return "Valutare Riparazione";
                    case StatoOggettoInventariato.NonRiparabile: return "Non riparabile";
                    case StatoOggettoInventariato.Dismesso: return "Dismesso";
                    default: return DMD.Strings.Combine("[", value, "]");
                }
            }

            /// <summary>
            /// Restituisce la collezione degli oggetti che rispettano il pattern
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public CCollection<OggettoInventariato> Find(string text)
            {
                var ret = new CCollection<OggettoInventariato>();
                text = Strings.Trim(text);

                if (string.IsNullOrEmpty(text))
                    return ret;

                using (var cursor = new OggettoInventariatoCursor())
                {
                    // cerciahimo prima in base al codice
                    cursor.WhereClauses *= (
                                             cursor.Field ("Seriale").StartsWith(text) 
                                           + cursor.Field("CodiceRFID").StartWidth(text) 
                                           + cursor.Field("Codice").StartWidth(text)
                                           );
                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }

                    cursor.Reset1();
                    cursor.WhereClauses = DBCursorField.True;
                    if (ret.Count == 0)
                    {
                        // Se non abbiamo risultati cerchiamo in base al nome
                        cursor.Clear();
                        cursor.Nome.Value = text;
                        cursor.Nome.Operator = OP.OP_LIKE;
                        while (cursor.Read())
                        {
                            ret.Add(cursor.Item);
                        }
                    }

                    cursor.Reset1();
                    if (cursor.Count() == 0)
                    {
                        // Se non abbiamo risultati cerchiamo il base al nome articolo
                        cursor.Clear();
                        cursor.NomeArticolo.Value = text;
                        cursor.NomeArticolo.Operator = OP.OP_LIKE;
                        while (cursor.Read())
                        {
                            ret.Add(cursor.Item);
                        }
                    }

                }

                return ret;
            }

            /// <summary>
            /// Restituisce l'oggetto in base al nome
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public OggettoInventariato GetItemByName(string name)
            {
                OggettoInventariato ret = null;

                name = DMD.Strings.Trim(name);
                
                if (string.IsNullOrEmpty(name))
                    return null;

                using (var cursor = new OggettoInventariatoCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.Nome.Value = name;
                    cursor.StatoAttuale.Value = StatoOggettoInventariato.Funzionante;
                    ret = cursor.Item;
                    cursor.Reset1();
                    if (ret is null)
                    {
                        cursor.StatoAttuale.Clear();
                        cursor.StatoAttuale.SortOrder = SortEnum.SORT_ASC;
                        ret = cursor.Item;
                    }

                }

                return ret;
            }
        }
    }

    public partial class Store
    {
        private static COggettiInventariatiClass m_OggettiInventariati = null;

        /// <summary>
        /// Repository di oggetti <see cref="OggettoInventariato"/>
        /// </summary>
        public static COggettiInventariatiClass OggettiInventariati
        {
            get
            {
                if (m_OggettiInventariati is null)
                    m_OggettiInventariati = new COggettiInventariatiClass();
                return m_OggettiInventariati;
            }
        }
    }
}