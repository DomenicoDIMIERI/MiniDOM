using DMD;
using DMD.Databases.Collections;

namespace minidom.Forms
{
    public enum TextAlignEnum : int
    {
        TEXTALIGN_DEFAULT = 0,
        TEXTALIGN_LEFT = 1,
        TEXTALIGN_RIGHT = 2,
        TEXTALIGN_CENTER = 3,
        TEXTALIGN_JUSTIFY = 4
    }

    public enum DockType : int
    {
        DOCK_NONE = 0,
        DOCK_LEFT = 1,
        DOCK_TOP = 2,
        DOCK_RIGHT = 3,
        DOCK_BOTTOM = 4,
        DOCK_FILL = 5
    }
     
    /// <summary>
    /// Attributi
    /// </summary>
    public class CWebControlAttributes 
        : CKeyCollection<string>
    {
        private WebControl m_Owner;

        /// <summary>
        /// Costruttore
        /// </summary>
        public CWebControlAttributes()
        {
            m_Owner = null;
        }

        private new void Add(object item)
        {
            base.Add(item);
        }

        private new void Insert(int index, object item)
        {
            base.Insert(index, item);
        }

        /// <summary>
        /// Accede all'elemento per nome
        /// </summary>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public new string this[string attrName]
        {
            get
            {
                attrName = Strings.LCase(Strings.Trim(attrName));
                int i = IndexOfKey(attrName);
                if (i < 0)
                    return DMD.Strings.vbNullString;
                return base[i];
            }

            set
            {
                attrName = Strings.LCase(Strings.Trim(attrName));
                int i = IndexOfKey(attrName);
                if (i < 0)
                {
                    Add(attrName, "" + value);
                }
                else
                {
                    base[i] = "" + value;
                }
            }
        }

        /// <summary>
        /// Accede all'elemento per indice
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public new string this[int i]
        {
            get
            {
                return base[i];
            }

            set
            {
                base[i] = "" + value;
            }
        }

        /// <summary>
        /// Imposta il proprietario
        /// </summary>
        /// <param name="owner"></param>
        protected internal virtual void SetOwner(WebControl owner)
        {
            m_Owner = owner;
        }
    }

    

    /// <summary>
    /// Collezione dei controlli appartenenti ad un controllo
    /// </summary>
    public class CWebControlsCollection 
        : CCollection<WebControl>
    {
        private WebControl m_Owner;

        public CWebControlsCollection()
        {
            m_Owner = null;
        }

        public WebControl Owner
        {
            get
            {
                return m_Owner;
            }
        }

        /// <summary>
        /// Imposta il contenitore
        /// </summary>
        /// <param name="value"></param>
        protected internal void SetOwner(WebControl value)
        {
            m_Owner = value;
        }

        /// <summary>
        /// OnInsert
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected override void OnInsert(int index, WebControl value)
        {
            if (m_Owner is object)
                value.SetParent(m_Owner);
            base.OnInsert(index, value);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CSearchStateItem
    {
        public string fieldName;
        public string fieldValue;

        /// <summary>
        /// Costruttore
        /// </summary>
        public CSearchStateItem()
        {
             
        }

        public void FromXML(string value)
        {
            string ret;
            ret = DMD.XML.Utils.Serializer.GetInnerTAG(value, "CSearchStateItem");
            fieldName = DMD.WebUtils.HtmlDecode(DMD.XML.Utils.Serializer.GetInnerTAG(ret, "name"));
            fieldValue = DMD.WebUtils.HtmlDecode(DMD.XML.Utils.Serializer.GetInnerTAG(ret, "value"));
        }
 
        /// <summary>
        /// Restituisce una stringa che rappresenta l'oggetto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string ret;
            ret = "";
            ret = ret + "<CSearchStateItem>";
            ret = ret + "<name>" + DMD.WebUtils.HtmlEncode(fieldName) + "</name>";
            ret = ret + "<value>" + DMD.WebUtils.HtmlEncode(fieldValue) + "</value>";
            ret = ret + "</CSearchStateItem>";
            return ret;
        }
    }

    public class CSearchState 
        : CCollection<CSearchStateItem>
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public CSearchState()
        {
        }

        /// <summary>
        /// Crea un nuovo oggetto CSearchStateItem e lo aggiunge in coda alla collezione.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public new CSearchStateItem Add(string fieldName, string fieldValue)
        {
            return SetValue(fieldName, fieldValue);
        }

        /// <summary>
        /// Restituisce l'indice dell'elemento
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public int IndexOfKey(string fieldName)
        {
            for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
            {
                if ((Strings.UCase(this[i].fieldName) ?? "") == (Strings.UCase(Strings.Trim(fieldName)) ?? ""))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Restituisce l'elemento
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string GetValue(string fieldName)
        {
            int i = IndexOfKey(fieldName);
            if (i >= 0)
            {
                return this[i].fieldValue;
            }
            else
            {
                return DMD.Strings.vbNullString;
            }
        }

        /// <summary>
        /// Imposta il valore
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        public CSearchStateItem SetValue(string fieldName, string fieldValue)
        {
            int i;
            CSearchStateItem item;
            i = IndexOfKey(fieldName);
            if (i >= 0)
            {
                item = base[i];
            }
            else
            {
                item = new CSearchStateItem();
                Add(item);
            }

            item.fieldName = Strings.Trim(fieldName);
            item.fieldValue = fieldValue;
            return item;
        }

        /// <summary>
        /// Rimuove il valore
        /// </summary>
        /// <param name="fieldName"></param>
        public void RemoveByKey(string fieldName)
        {
            int i;
            i = IndexOfKey(fieldName);
            base.RemoveAt(i);
        }

        /// <summary>
        /// Restituisce true se la collezione contiene il valore
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public bool ContainsKey(string fieldName)
        {
            return IndexOfKey(fieldName) >= 0;
        }

        /// <summary>
        /// Restituisce una stringa che rappresenta l'oggetto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string ret;
            ret = "<CSearchState count=\"" + Count + "\">";
            for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                ret = ret + base[i].ToString();
            ret = ret + "</CSearchState>";
            return ret;
        }

        /// <summary>
        /// Carica dal testo xml
        /// </summary>
        /// <param name="value"></param>
        public void FromXML(string value)
        {
            string tmp;
            int i;
            CSearchStateItem Item;
            Clear();
            tmp = DMD.XML.Utils.Serializer.GetInnerTAG(value, "CSearchState");
            i = Strings.InStr(tmp, "</CSearchStateItem>");
            while (i > 0)
            {
                Item = new CSearchStateItem();
                Item.FromXML(Strings.Left(tmp, i + Strings.Len("</CSearchStateItem>")));
                Add(Item);
                tmp = Strings.Mid(tmp, i + Strings.Len("</CSearchStateItem>"));
                i = Strings.InStr(tmp, "</CSearchStateItem>");
            }
        }

        /// <summary>
        /// Carica dalla richiesta POST
        /// </summary>
        public void FromPOST()
        {
            Clear();
            foreach (string s in WebSite.ASP_Request.Form)
                Add(s, WebSite.ASP_Request.Form[s]);
        }
    }
}