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
    public partial class Store
    {

        /// <summary>
        /// Oggetto che consente di impostare il cursore degli articoli
        /// </summary>
        [Serializable]
        public class AttributoArticoloItem 
            : DMD.XML.DMDBaseXMLObject
        {
            /// <summary>
            /// Nome dell'attributo
            /// </summary>
            public string Nome;

            /// <summary>
            /// Tipo dell'attributo
            /// </summary>
            public TypeCode? Tipo;

            /// <summary>
            /// Valore dell'attributo
            /// </summary>
            public string ValoreFormattato;

            /// <summary>
            /// Unità di misura
            /// </summary>
            public string UnitaDiMisura;

            /// <summary>
            /// Costruttore
            /// </summary>
            public AttributoArticoloItem()
            {
                Nome = "";
                Tipo = default;
                ValoreFormattato = "";
                UnitaDiMisura = "";
            }

            /// <summary>
            /// Restituisce true se l'oggetto é impostato
            /// </summary>
            /// <returns></returns>
            public bool IsSet()
            {
                return !string.IsNullOrEmpty(ValoreFormattato);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Nome":
                        {
                            Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Tipo":
                        {
                            Tipo = (TypeCode?)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ValoreFormattato":
                        {
                            ValoreFormattato = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "UnitaDiMisura":
                        {
                            UnitaDiMisura = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    default:
                        base.SetFieldInternal(fieldName, fieldValue);
                        break;
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", Nome);
                writer.WriteAttribute("Tipo", (int?)Tipo);
                writer.WriteAttribute("ValoreFormattato", ValoreFormattato);
                writer.WriteAttribute("UnitaDiMisura", UnitaDiMisura);
                base.XMLSerialize(writer);
            }

           
        }

        /// <summary>
        /// Cursore di oggetti di tipo <see cref="Articolo"/>
        /// </summary>
        [Serializable]
        public class ArticoloCursor 
            : Databases.DBObjectCursor<Articolo>
        {
            private DBCursorStringField m_Nome = new DBCursorStringField("Nome");
            private DBCursorField<ArticoloFlags> m_Flags = new DBCursorField<ArticoloFlags>("Flags");
            private DBCursorStringField m_Marca = new DBCursorStringField("Marca");
            private DBCursorStringField m_Modello = new DBCursorStringField("Modello");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorStringField m_IconURL = new DBCursorStringField("IconURL");
            private DBCursorStringField m_CategoriaPrincipale = new DBCursorStringField("CategoriaPrincipale");
            private DBCursorStringField m_UnitaBase = new DBCursorStringField("UnitaBase");
            private DBCursorField<int> m_DecimaliValuta = new DBCursorField<int>("DecimaliValuta");
            private DBCursorField<int> m_DecimaliQuantita = new DBCursorField<int>("DecimaliQuantita");
            private DBCursorStringField m_TipoCodice = new DBCursorStringField("TipoCodice");
            private DBCursorStringField m_ValoreCodice = new DBCursorStringField("ValoreCodice");
            private DBCursorStringField m_ProductPage = new DBCursorStringField("ProductPage");
            private DBCursorStringField m_SupportPage = new DBCursorStringField("SupportPage");
            private CCollection<AttributoArticoloItem> m_Attributi = new CCollection<AttributoArticoloItem>();

            /// <summary>
            /// Costruttore
            /// </summary>
            public ArticoloCursor()
            {
            }

            /// <summary>
            /// ProductPage
            /// </summary>
            public DBCursorStringField ProductPage
            {
                get
                {
                    return m_ProductPage;
                }
            }

            /// <summary>
            /// SupportPage
            /// </summary>
            public DBCursorStringField SupportPage
            {
                get
                {
                    return m_SupportPage;
                }
            }

            /// <summary>
            /// Attributi
            /// </summary>
            public CCollection<AttributoArticoloItem> Attributi
            {
                get
                {
                    return m_Attributi;
                }
            }

            /// <summary>
            /// TipoCodice
            /// </summary>
            public DBCursorStringField TipoCodice
            {
                get
                {
                    return m_TipoCodice;
                }
            }

            /// <summary>
            /// ValoreCodice
            /// </summary>
            public DBCursorStringField ValoreCodice
            {
                get
                {
                    return m_ValoreCodice;
                }
            }

            /// <summary>
            /// UnitaBase
            /// </summary>
            public DBCursorStringField UnitaBase
            {
                get
                {
                    return m_UnitaBase;
                }
            }

            /// <summary>
            /// DecimaliValuta
            /// </summary>
            public DBCursorField<int> DecimaliValuta
            {
                get
                {
                    return m_DecimaliValuta;
                }
            }

            /// <summary>
            /// DecimaliQuantita
            /// </summary>
            public DBCursorField<int> DecimaliQuantita
            {
                get
                {
                    return m_DecimaliQuantita;
                }
            }

            /// <summary>
            /// Nome
            /// </summary>
            public DBCursorStringField Nome
            {
                get
                {
                    return m_Nome;
                }
            }

            /// <summary>
            /// CategoriaPrincipale
            /// </summary>
            public DBCursorStringField CategoriaPrincipale
            {
                get
                {
                    return m_CategoriaPrincipale;
                }
            }

            /// <summary>
            /// Marca
            /// </summary>
            public DBCursorStringField Marca
            {
                get
                {
                    return m_Marca;
                }
            }

            /// <summary>
            /// Modello
            /// </summary>
            public DBCursorStringField Modello
            {
                get
                {
                    return m_Modello;
                }
            }

            /// <summary>
            /// Descrizione
            /// </summary>
            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            /// <summary>
            /// IconURL
            /// </summary>
            public DBCursorStringField IconURL
            {
                get
                {
                    return m_IconURL;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<ArticoloFlags> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

          
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Store.Articoli;
            }

            /// <summary>
            /// Restituisce i campi del cursore che devono essere usati per la clausola where
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldsCollection GetCursorFields()
            {
                var ret = base.GetCursorFields();
                ret.RemoveByKey("TipoCodice");
                ret.RemoveByKey("ValoreCodice");
                return ret;
            }

            
         
            /// <summary>
            /// Restituisce i campi del cursore che devono essere usati per costruire la clausola sort
            /// </summary>
            /// <returns></returns>
            public override List<DBCursorField> GetSortFields()
            {
                var ret = base.GetSortFields();
                ret.Remove(this.m_TipoCodice);
                ret.Remove(this.m_ValoreCodice);
                return ret;
            }

            /// <summary>
            /// Genera la clausola where
            /// </summary>
            /// <returns></returns>
            public override DBCursorFieldBase GetWherePart()
            {
                DBCursorFieldBase ret = base.GetWherePart();
                if (m_ValoreCodice.IsSet() || m_TipoCodice.IsSet())
                {
                    var list = new List<int>();
                    using (var cursor = new CodiceArticoloCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        if (this.m_ValoreCodice.IsSet())
                        {
                            cursor.Valore.Values = this.m_ValoreCodice.Values;
                            cursor.Valore.Operator = this.m_ValoreCodice.Operator;
                            cursor.Valore.IncludeNulls = this.m_ValoreCodice.IncludeNulls;
                        }
                        if (this.m_TipoCodice.IsSet())
                        {
                            cursor.Tipo.Values = this.m_TipoCodice.Values;
                            cursor.Tipo.Operator = this.m_TipoCodice.Operator;
                            cursor.Tipo.IncludeNulls = this.m_TipoCodice.IncludeNulls;
                        }
                        while (cursor.Read())
                        {
                            list.Add(cursor.Item.IDArticolo);
                        }
                    }

                    ret *= Field("ID").In(list.ToArray());
                }

                return ret;
            }

            //TODO Cursore sugli attributi

            //public override string GetSQL()
            //{
            //    string ret = base.GetSQL();
            //    string tmpSQL;
            //    if (IsAttributoSet())
            //    {
            //        int t = 0;
            //        int[] totalids = null;
            //        foreach (AttributoArticoloItem attr in m_Attributi)
            //        {
            //            if (attr.IsSet())
            //            {
            //                tmpSQL = "[Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
            //                if (!string.IsNullOrEmpty(DMD.Strings.Trim(attr.Nome)))
            //                    tmpSQL = DMD.Strings.Combine(tmpSQL, "[NomeAttributo]=" + DBUtils.DBString(DMD.Strings.Trim(attr.Nome)), " AND ");
            //                if (attr.Tipo.HasValue)
            //                    tmpSQL = DMD.Strings.Combine(tmpSQL, "[TipoAttributo]=" + DBUtils.DBNumber(attr.Tipo), " AND ");
            //                if (!string.IsNullOrEmpty(DMD.Strings.Trim(attr.ValoreFormattato)))
            //                    tmpSQL = DMD.Strings.Combine(tmpSQL, "[ValoreAttributo]=" + DBUtils.DBString(DMD.Strings.Trim(attr.ValoreFormattato)), " AND ");
            //                if (!string.IsNullOrEmpty(DMD.Strings.Trim(attr.UnitaDiMisura)))
            //                    tmpSQL = DMD.Strings.Combine(tmpSQL, "[UnitaDiMisura]=" + DBUtils.DBString(DMD.Strings.Trim(attr.UnitaDiMisura)), " AND ");
            //                ret = "SELECT [T" + t + "].* FROM (" + ret + ") AS [T" + t + "] INNER JOIN (";
            //                ret += "SELECT [IDArticolo] AS [ID] FROM [tbl_OfficeArticoliAttributi] WHERE " + tmpSQL;
            //                ret += " GROUP BY [IDArticolo]) AS [T" + (t + 1) + "] ";
            //                ret += " ON [T" + t + "].[ID]=[T" + (t + 1) + "].[ID]";
            //                t += 2;
            //            }
            //        }
            //    }

            //    return ret;
            //}

            private bool IsAttributoSet()
            {
                foreach (AttributoArticoloItem attr in m_Attributi)
                {
                    if (attr.IsSet())
                        return true;
                }

                return false;
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                base.XMLSerialize(writer);
                writer.WriteTag("Attributi", m_Attributi);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Attributi":
                        {
                            m_Attributi.Clear();
                            m_Attributi.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }
        }
    }
}