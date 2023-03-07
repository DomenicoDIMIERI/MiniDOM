using System;
using System.Collections;
using static minidom.Sistema;
using DMD;
using DMD.XML;

namespace minidom
{
    [Serializable]
    public class CKeyCollection 
        : CCollection
    {
        private string[] m_Keys;
        private int[] m_Indicies;
        private static CStringComparer m_KeyComparer = new CStringComparer(false);

        public CKeyCollection()
        {
            m_Keys = DMD.Arrays.Empty<string>();
            m_Indicies = DMD.Arrays.Empty<int>();
        }

        public override bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public CKeyCollection(CKeyCollection col) : this()
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    var keys = col.Keys;
                    for (int i = 0, loopTo = DMD.Arrays.UBound(keys); i <= loopTo; i++)
                        Add(keys[i], col[keys[i]]);
                }
            }
            else
            {
                var keys = col.Keys;
                for (int i = 0, loopTo1 = DMD.Arrays.UBound(keys); i <= loopTo1; i++)
                    Add(keys[i], col[keys[i]]);
            }
        }

        public object GetItemByKey(string key)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    int i = IndexOfKey(key);
                    if (i < 0)
                        return null;
                    return this[i];
                }
            }
            else
            {
                int i = IndexOfKey(key);
                if (i < 0)
                    return null;
                return this[i];
            }
        }

        public void SetItemByKey(string key, object value)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    int i = IndexOfKey(key);
                    if (i < 0)
                    {
                        Add(key, value);
                    }
                    else
                    {
                        this[i] = value;
                    }
                }
            }
            else
            {
                int i = IndexOfKey(key);
                if (i < 0)
                {
                    Add(key, value);
                }
                else
                {
                    this[i] = value;
                }
            }
        }

        protected override void OnRemove(int index, object value)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    int kI = -1;
                    for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                    {
                        if (m_Indicies[i] == index)
                        {
                            kI = i;
                            break;
                        }
                    }

                    m_Keys = DMD.Arrays.RemoveAt(m_Keys, kI, 1);
                    for (int i = 0, loopTo1 = DMD.Arrays.Len(m_Indicies) - 1; i <= loopTo1; i++)
                    {
                        if (m_Indicies[i] >= index)
                            m_Indicies[i] -= 1;
                    }

                    m_Indicies = DMD.Arrays.RemoveAt(m_Indicies, kI, 1);
                    base.OnRemove(index, value);
                }
            }
            else
            {
                int kI = -1;
                for (int i = 0, loopTo2 = Count - 1; i <= loopTo2; i++)
                {
                    if (m_Indicies[i] == index)
                    {
                        kI = i;
                        break;
                    }
                }

                m_Keys = DMD.Arrays.RemoveAt(m_Keys, kI, 1);
                for (int i = 0, loopTo3 = DMD.Arrays.Len(m_Indicies) - 1; i <= loopTo3; i++)
                {
                    if (m_Indicies[i] >= index)
                        m_Indicies[i] -= 1;
                }

                m_Indicies = DMD.Arrays.RemoveAt(m_Indicies, kI, 1);
                base.OnRemove(index, value);
            }
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            base.OnRemoveComplete(index, value);
        }

        protected override void OnClear()
        {
            base.OnClear();
        }

        protected override void OnClearComplete()
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    m_Keys = DMD.Arrays.Empty<string>();
                    m_Indicies = DMD.Arrays.Empty<int>();
                    base.OnClearComplete();
                }
            }
            else
            {
                m_Keys = DMD.Arrays.Empty<string>();
                m_Indicies = DMD.Arrays.Empty<int>();
                base.OnClearComplete();
            }
        }

        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsertComplete(index, value);
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
        }

        public new void Add(string key, object value)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    int p = DMD.Arrays.GetInsertPosition(m_Keys, key, 0, Count, m_KeyComparer);
                    m_Keys = DMD.Arrays.Insert(m_Keys, key, p);
                    m_Indicies = DMD.Arrays.Insert(m_Indicies, Count, p);
                    Add(value);
                }
            }
            else
            {
                int p = DMD.Arrays.GetInsertPosition(m_Keys, key, 0, Count, m_KeyComparer);
                m_Keys = DMD.Arrays.Insert(m_Keys, key, p);
                m_Indicies = DMD.Arrays.Insert(m_Indicies, Count, p);
                Add(value);
            }
        }

        // Private Sub SortKeys()
        // Me.m_KeysChanged = False
        // DMD.Arrays.Sort(Me.m_Keys, Me.m_Indicies, 0, Me.Count, m_KeyComparer)
        // End Sub

        public new void Insert(int index, string key, object value)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    int p = DMD.Arrays.GetInsertPosition(m_Keys, key, 0, Count, m_KeyComparer);
                    m_Keys = DMD.Arrays.Insert(m_Keys, key, p);
                    for (int i = 0, loopTo = DMD.Arrays.Len(m_Indicies) - 1; i <= loopTo; i++)
                    {
                        if (m_Indicies[i] >= index)
                            m_Indicies[i] += 1;
                    }

                    m_Indicies = DMD.Arrays.Insert(m_Indicies, index, p);
                    /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                    Insert(index, value);
                }
            }
            else
            {
                int p = DMD.Arrays.GetInsertPosition(m_Keys, key, 0, Count, m_KeyComparer);
                m_Keys = DMD.Arrays.Insert(m_Keys, key, p);
                for (int i = 0, loopTo1 = DMD.Arrays.Len(m_Indicies) - 1; i <= loopTo1; i++)
                {
                    if (m_Indicies[i] >= index)
                        m_Indicies[i] += 1;
                }

                m_Indicies = DMD.Arrays.Insert(m_Indicies, index, p);
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                Insert(index, value);
            }
        }

        public new object this[string key]
        {
            get
            {
                if (IsSynchronized)
                {
                    lock (SyncRoot)
                    {
                        int i = IndexOfKey(key);
                        if (i < 0 || i >= Count)
                            throw new ArgumentOutOfRangeException("Chiave non valida: [" + key + "]");
                        return base[i];
                    }
                }
                else
                {
                    int i = IndexOfKey(key);
                    if (i < 0 || i >= Count)
                        throw new ArgumentOutOfRangeException("Chiave non valida: [" + key + "]");
                    return base[i];
                }
            }

            set
            {
                if (IsSynchronized)
                {
                    lock (SyncRoot)
                    {
                        int i = IndexOfKey(key);
                        if (i < 0 || i >= Count)
                            throw new ArgumentOutOfRangeException("Chiave non valida: [" + key + "]");
                        base[i] = value;
                    }
                }
                else
                {
                    int i = IndexOfKey(key);
                    if (i < 0 || i >= Count)
                        throw new ArgumentOutOfRangeException("Chiave non valida: [" + key + "]");
                    base[i] = value;
                }
            }
        }

        public new int IndexOf(object item)
        {
            return base.IndexOf(item);
        }

        public new bool Contains(object item)
        {
            return IndexOf(item) >= 0;
        }

        public new void Remove(object item)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                    RemoveAt(IndexOf(item));
            }
            else
            {
                RemoveAt(IndexOf(item));
            }
        }

        public override void RemoveAt(int index)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                    base.RemoveAt(index);
            }
            else
            {
                base.RemoveAt(index);
            }
        }

        public int IndexOfKey(string key)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    int i = -1;
                    if (m_Keys is object)
                        i = Array.BinarySearch(m_Keys, key, m_KeyComparer); // DMD.Arrays.BinarySearch(Me.m_Keys, 0, Me.Count, key, m_KeyComparer)
                    if (i >= 0)
                        return m_Indicies[i];
                    return -1;
                }
            }
            else
            {
                int i = -1;
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                if (m_Keys is object)
                    i = Array.BinarySearch(m_Keys, key, m_KeyComparer); // DMD.Arrays.BinarySearch(Me.m_Keys, 0, Me.Count, key, m_KeyComparer)
                if (i >= 0)
                    return m_Indicies[i];
                return -1;
            }
        }

        public bool ContainsKey(string key)
        {
            return IndexOfKey(key) >= 0;
        }

        public void RemoveByKey(string Key)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    int i = IndexOfKey(Key);
                    if (i < 0)
                        throw new ArgumentOutOfRangeException("Chiave non valida: [" + Key + "]");
                    RemoveAt(i);
                }
            }
            else
            {
                int i = IndexOfKey(Key);
                if (i < 0)
                    throw new ArgumentOutOfRangeException("Chiave non valida: [" + Key + "]");
                RemoveAt(i);
            }
        }

        public string[] Keys
        {
            get
            {
                if (IsSynchronized)
                {
                    lock (SyncRoot)
                    {
                        if (m_Keys is null)
                        {
                            return new string[] { };
                        }
                        else
                        {
                            string[] ret = (string[])Array.CreateInstance(typeof(string), m_Keys.Length);
                            for (int i = 0, loopTo = m_Keys.Length - 1; i <= loopTo; i++)
                                ret[i] = m_Keys[i];
                            return ret;
                        }
                    }
                }
                else if (m_Keys is null)
                {
                    return new string[] { };
                }
                else
                {
                    string[] ret = (string[])Array.CreateInstance(typeof(string), m_Keys.Length);
                    for (int i = 0, loopTo1 = m_Keys.Length - 1; i <= loopTo1; i++)
                        ret[i] = m_Keys[i];
                    return ret;
                }
            }
        }

        /// <summary>
        /// Oggetto usato per l'ordinamento
        /// </summary>
        [Serializable]
        internal class KeyValue 
            : IDMDXMLSerializable
        {

            /// <summary>
            /// Chiave
            /// </summary>
            public string Key;

            /// <summary>
            /// Valore
            /// </summary>
            public object Value;
            
            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Key + "/" + Value.ToString();
            }

            /// <summary>
            /// Restituisce il codice hash
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.Key, this.Value);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                if (!(obj is KeyValue))
                    return false;
                var o = (KeyValue)obj;
                return RunTime.EQ(this.Key, o.Key)
                       && RunTime.EQ(this.Value, o.Value);
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Key":
                        {
                            Key = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TypeCode":
                        {
                            Value = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Value":
                        {
                            Value = Sistema.Types.CastTo(fieldValue, (TypeCode) DMD.Integers.CInt(Value));
                            break;
                        }
                }
            }

            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Key", Key);
                writer.WriteAttribute("TypeCode", (int?)Sistema.Types.GetTypeCode(Value));
                writer.WriteAttribute("Value", DMD.Strings.CStr(Sistema.Types.CastTo(Value, TypeCode.String)));
            }
        }

        internal class KeyValueComparer : IComparer
        {
            public CKeyCollection c;

            public KeyValueComparer(CKeyCollection c)
            {
                this.c = c;
            }

            public int Compare(object x, object y)
            {
                KeyValue a = (KeyValue)x;
                KeyValue b = (KeyValue)y;
                return c.Compare(a.Value, b.Value);
            }
        }

        public override void Sort()
        {
            var e = new EventArgs();
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    OnBeforeSort(e);
                    if (Count > 0)
                    {
                        KeyValue[] items;
                        bool oldV = m_Sorted;
                        m_Sorted = false;
                        int i;
                        items = new KeyValue[Count];
                        var loopTo = Count - 1;
                        for (i = 0; i <= loopTo; i++)
                        {
                            {
                                var withBlock = items[i];
                                withBlock.Key = m_Keys[i];
                                withBlock.Value = base[m_Indicies[i]];
                            }
                        }

                        DMD.Arrays.Sort(items, 0, Count, new KeyValueComparer(this));
                        Clear();
                        var loopTo1 = DMD.Arrays.UBound(items);
                        for (i = 0; i <= loopTo1; i++)
                            Add(items[i].Key, items[i].Value);
                        m_Sorted = oldV;
                    }

                    OnAfterSort(e);
                }
            }
            else
            {
                OnBeforeSort(e);
                if (Count > 0)
                {
                    KeyValue[] items;
                    bool oldV = m_Sorted;
                    m_Sorted = false;
                    int i;
                    items = new KeyValue[Count];
                    var loopTo2 = Count - 1;
                    for (i = 0; i <= loopTo2; i++)
                    {
                        {
                            items[i] = new KeyValue();
                            items[i].Key = m_Keys[i];
                            items[i].Value = base[m_Indicies[i]];
                        }
                    }

                    DMD.Arrays.Sort(items, 0, Count, new KeyValueComparer(this));
                    Clear();
                    var loopTo3 = DMD.Arrays.UBound(items);
                    for (i = 0; i <= loopTo3; i++)
                        Add(items[i].Key, items[i].Value);
                    m_Sorted = oldV;
                }

                OnAfterSort(e);
            }
        }

        protected override void XMLSerialize(XMLWriter writer)
        {
            if (IsSynchronized)
            {
                lock (SyncRoot)
                {
                    base.XMLSerialize(writer);
                    writer.BeginTag("keys");
                    writer.Write(this.m_Keys);
                    writer.EndTag("keys");
                    writer.BeginTag("Indicies");
                    writer.Write(this.m_Indicies);
                    writer.EndTag("Indicies");
                }
            }
            else
            {
                base.XMLSerialize(writer);
                writer.BeginTag("keys");
                writer.Write(this.m_Keys);
                writer.EndTag("keys");
                writer.BeginTag("Indicies");
                writer.Write(this.m_Indicies);
                writer.EndTag("Indicies");
            }
        }

        protected override void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "keys":
                    {
                        m_Keys = (string[])DMD.Arrays.Convert<string>(fieldValue);
                        if (Count != DMD.Arrays.Len(m_Keys))
                        {
                            // Throw New Exception("XML Deserialization Execption")
                            object[] arr;
                            arr = new object[m_Keys.Length];
                            base.SetFieldInternal("items", arr);
                        }

                        break;
                    }

                case "Indicies":
                    {
                        m_Indicies = (int[])DMD.Arrays.Convert<int>(fieldValue);
                        if (Count != DMD.Arrays.Len(m_Indicies))
                        {
                            // Throw New Exception("XML Deserialization Execption")
                            object[] arr;
                            arr = new object[m_Indicies.Length];
                            base.SetFieldInternal("items", arr);
                        }

                        break;
                    }

                case "items":
                    {
                        base.SetFieldInternal(fieldName, fieldValue);
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