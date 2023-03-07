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
using static minidom.Office;

namespace minidom
{
    public partial class Office
    {
        /// <summary>
        /// Tipi di elementi supportati in un template
        /// </summary>
        public enum TemplateItemTypes
        {
            /// <summary>
            /// L'elemento non é renderizzabile
            /// </summary>
            NoOperation = 0,

            /// <summary>
            /// Etichetta
            /// </summary>
            TEXTOUT = 1,

            /// <summary>
            /// Bordi del Rettangolo
            /// </summary>
            DRAWRECT = 2,

            /// <summary>
            /// Riempimento del rettangolo
            /// </summary>
            FILLRECT = 3,

            /// <summary>
            /// Bordo dell'ellisse
            /// </summary>
            DRAWELLIPSE = 4,

            /// <summary>
            /// Rimempimento dell'ellisse
            /// </summary>
            FILLELLIPSE = 5,

            /// <summary>
            /// Immagine
            /// </summary>
            DRAWIMAGE = 6,

            /// <summary>
            /// Genera una nuova pagina
            /// </summary>
            NEWPAGE = 7,

            /// <summary>
            /// Campo data
            /// </summary>
            DATAFIELD = 8,

            /// <summary>
            /// Campo espressione
            /// </summary>
            EXPRESSION = 9,

            /// <summary>
            /// Vai alla pagina
            /// </summary>
            GOTOPAGE = 10
        }


        /// <summary>
        /// Elemento generato da un template
        /// </summary>
        [Serializable]
        public class TemplateItem 
            : minidom.Databases.DBObjectBase
        {
            private TemplateItemTypes m_ItemType;
            private string m_Color;
            private CRectangle m_Bounds;
            private string m_Text;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public TemplateItem()
            {
                m_ItemType = TemplateItemTypes.NoOperation;
                m_Color = "";
                m_Bounds = new CRectangle(0d, 0d, 0d, 0d);
                m_Text = "";
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="type"></param>
            /// <param name="text"></param>
            /// <param name="bounds"></param>
            public TemplateItem(TemplateItemTypes type, string text, CRectangle bounds) : this()
            {
                m_ItemType = type;
                m_Text = text;
                m_Bounds = bounds;
                m_Color = "#000000";
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="type"></param>
            /// <param name="text"></param>
            /// <param name="bounds"></param>
            /// <param name="color"></param>
            public TemplateItem(TemplateItemTypes type, string text, CRectangle bounds, string color) : this()
            {
                m_ItemType = type;
                m_Text = text;
                m_Bounds = bounds;
                m_Color = color;
            }
             
            /// <summary>
            /// Tipo dell'elemento
            /// </summary>
            public TemplateItemTypes ItemType
            {
                get
                {
                    return m_ItemType;
                }

                set
                {
                    var oldValue = m_ItemType;
                    if (oldValue == value)
                        return;
                    m_ItemType = value;
                    DoChanged("ItemType", value, oldValue);
                }
            }

            /// <summary>
            /// Colore
            /// </summary>
            public string Color
            {
                get
                {
                    return m_Color;
                }

                set
                {
                    string oldValue = m_Color;
                    value = DMD.Strings.Trim(value);
                    if (oldValue.Equals(value))
                        return;
                    m_Color = value;
                    DoChanged("Color", value, oldValue);
                }
            }

            /// <summary>
            /// Rettangolo che contiene l'elemento
            /// </summary>
            public CRectangle Bounds
            {
                get
                {
                    return m_Bounds;
                }

                set
                {
                    var oldValue = m_Bounds;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Bounds = value;
                    DoChanged("Bounds", value, oldValue);
                }
            }

            /// <summary>
            /// Testo
            /// </summary>
            public string Text
            {
                get
                {
                    return m_Text;
                }

                set
                {
                    string oldValue = m_Text;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Text = value;
                    DoChanged("Text", value, oldValue);
                }
            }

            /// <summary>
            /// Estringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(
                        Enum.GetName(typeof(TemplateItemTypes), m_ItemType) , "/" ,
                        m_Bounds.ToString() , "/" ,
                        m_Text
                        );
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return null;
            }

            public override string GetTableName()
            {
                return "tbl_DocumentiTemplateItems";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_ItemType = reader.Read("ItemType", this.m_ItemType);
                this.m_Color = reader.Read("Color", this.m_Color);
                float x = default, y = default, w = default, h = default;
                x = reader.Read("x", x);
                y = reader.Read("y", y);
                w = reader.Read("w", w);
                h = reader.Read("h", h);
                m_Bounds = new CRectangle(x, y, w, h);
                this.m_Text = reader.Read("Text", this.m_Text);
                string txt = reader.Read("Params", "");
                if (!string.IsNullOrEmpty(txt))
                    this.m_Parameters = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(txt);

                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("ItemType", m_ItemType);
                writer.Write("Color", m_Color);
                writer.Write("x", Bounds.Left);
                writer.Write("y", Bounds.Top);
                writer.Write("w", Bounds.Width);
                writer.Write("h", Bounds.Height);
                writer.Write("Text", m_Text);
                writer.Write("Params", DMD.XML.Utils.Serializer.Serialize(Parameters));
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("ItemType", (int?)m_ItemType);
                writer.WriteAttribute("Color", m_Color);
                base.XMLSerialize(writer);
                writer.WriteTag("Bounds", Bounds);
                writer.WriteTag("Params", Parameters);
                writer.WriteTag("Text", m_Text);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "ItemType":
                        {
                            m_ItemType = (TemplateItemTypes)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Color":
                        {
                            m_Color = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Bounds":
                        {
                            m_Bounds = (CRectangle)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                            break;
                        }

                    case "Text":
                        {
                            m_Text = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Params":
                        {
                            m_Parameters = (CKeyCollection)fieldValue;
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