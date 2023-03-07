using System;
using System.Runtime.CompilerServices;
using DMD;
using DMD.XML;

namespace minidom
{
    public partial class Databases
    {
        public abstract class CDBObject : IDMDXMLSerializable, IComparable
        {
            private CDBConnection _m_Connection;

            private CDBConnection m_Connection
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                get
                {
                    return _m_Connection;
                }

                [MethodImpl(MethodImplOptions.Synchronized)]
                set
                {
                    if (_m_Connection != null)
                    {
                        _m_Connection.ConnectionClosed -= OnConnectionClosed;
                    }

                    _m_Connection = value;
                    if (_m_Connection != null)
                    {
                        _m_Connection.ConnectionClosed += OnConnectionClosed;
                    }
                }
            }

            private string m_Name;
            private bool m_Changed;
            private bool m_Created;

            public CDBObject()
            {
                DMDObject.IncreaseCounter(this);
                m_Changed = false;
                m_Created = false;
            }

            public CDBObject(string name) : this()
            {
                m_Name = DMD.Strings.Trim(name);
                m_Changed = false;
            }

            public CDBConnection Connection
            {
                get
                {
                    return m_Connection;
                }
            }

            protected internal void SetConnection(CDBConnection value)
            {
                m_Connection = value;
            }

            public string Name
            {
                get
                {
                    return m_Name;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Name;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Name = value;
                    SetChanged(true);
                }
            }

            protected internal void SetName(string value)
            {
                m_Name = value;
            }

            protected virtual void OnConnectionClosed(object sender, EventArgs e)
            {
            }

            public override string ToString()
            {
                return m_Name;
            }

            public virtual bool IsChanged()
            {
                return m_Changed;
            }

            protected internal virtual void SetChanged(bool value)
            {
                m_Changed = value;
            }

            public virtual bool IsCreated()
            {
                return m_Created;
            }

            protected internal virtual void SetCreated(bool value)
            {
                m_Created = value;
            }

            public void Create()
            {
                CreateInternal1();
                SetChanged(false);
                SetCreated(true);
                
            }

            protected abstract void CreateInternal1();

            public void Update()
            {
                if (IsCreated())
                {
                    UpdateInternal1();
                    SetChanged(false);
                }
                else
                {
                    Create();
                }
            }

            protected abstract void UpdateInternal1();

            public void Drop()
            {
                DropInternal1();
                SetChanged(true);
                SetCreated(false);
            }

            protected abstract void DropInternal1();

            public void Rename(string newName)
            {
                newName = DMD.Strings.Trim(newName);
                RenameItnernal(newName);
                m_Name = newName;
            }

            protected abstract void RenameItnernal(string newName);

            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Name":
                        {
                            m_Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Changed":
                        {
                            m_Changed = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Created":
                        {
                            m_Created = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }
                }
            }

            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Name", m_Name);
                writer.WriteAttribute("Changed", m_Changed);
                writer.WriteAttribute("Created", m_Created);
            }

            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer);  }
            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue);  }

            public int CompareTo(CDBObject obj)
            {
                return DMD.Strings.Compare(m_Name, obj.m_Name, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CDBObject)obj);
            }

            ~CDBObject()
            {
                DMDObject.DecreaseCounter(this);
            }
        }
    }
}