using System;
using System.Globalization;
using DMD;
using DMD.XML;

namespace minidom
{
    public partial class WebSite
    {


        [Serializable]
        public class IPADDRESSinfo 
            : Databases.DBObject, IComparable
        {
            private byte[] m_IP;
            private byte[] m_NetMask;
            private string m_Descrizione;
            private bool m_Allow;
            private bool m_Negate;
            private bool m_Interno;
            private string m_AssociaUfficio;

            public IPADDRESSinfo()
            {
                m_IP = new byte[4];
                m_NetMask = new byte[4];
                m_Descrizione = "";
                m_Allow = true;
                m_Negate = false;
                m_Interno = true;
                m_AssociaUfficio = "";
            }

            public IPADDRESSinfo(string value)
            {
                int p = Strings.InStr(value, "/");
                string ip, netMask;
                if (p > 1)
                {
                    ip = Strings.Left(value, p - 1);
                    netMask = Strings.Mid(value, p + 1);
                }
                else
                {
                    ip = value;
                    netMask = "";
                }

                m_IP = GetBytes(ip);
                if (Strings.InStr(netMask, ".") > 0)
                {
                    m_NetMask = GetBytes(netMask);
                }
                else
                {
                    m_NetMask = GetMaskBytes32(DMD.Integers.ValueOf(netMask));
                }

                m_Allow = true;
            }

            public IPADDRESSinfo(string ip, string mask)
            {
                m_IP = GetBytes(ip);
                m_NetMask = GetBytes(mask);
                m_Allow = true;
            }

            public byte[] IP
            {
                get
                {
                    return m_IP;
                }

                set
                {
                    m_IP = value;
                }
            }

            public byte[] NetMask
            {
                get
                {
                    return m_NetMask;
                }

                set
                {
                    m_NetMask = value;
                }
            }

            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    m_Descrizione = value;
                }
            }

            public bool Allow
            {
                get
                {
                    return m_Allow;
                }

                set
                {
                    m_Allow = value;
                }
            }

            public bool Negate
            {
                get
                {
                    return m_Negate;
                }

                set
                {
                    m_Negate = value;
                }
            }

            public bool Match(string ip)
            {
                var bytes = GetBytes(ip);
                for (int i = 0, loopTo = Maths.Min(DMD.Arrays.UBound(m_IP), DMD.Arrays.UBound(m_NetMask)); i <= loopTo; i++)
                {
                    if ((bytes[i] & m_NetMask[i]) != (m_IP[i] & m_NetMask[i]))
                        return false;
                }

                return true;
            }

            public byte[] GetBytes(string ip)
            {
                string[] items;
                byte[] bytes = null;
                if (Strings.InStr(ip, "::") > 0)
                {
                    bytes = new byte[8];
                }
                else
                {
                    items = Strings.Split(ip, ".");
                    if (DMD.Arrays.UBound(items) == 3)
                    {
                        bytes = new byte[DMD.Arrays.UBound(items) + 1];
                        for (int i = 0, loopTo = DMD.Arrays.UBound(items); i <= loopTo; i++)
                            bytes[i] = DMD.Bytes.CByte(items[i]);
                    }
                    else
                    {
                        throw new ArgumentException("L'indirizzo [" + ip + "] non sembra un indirizzo IPv4 o IPv6");
                    }
                }

                return bytes;
            }

            public byte[] GetMaskBytes32(int bits)
            {
                byte[] bytes;
                int i;
                i = 0;
                bytes = new byte[4];
                while (bits > 8)
                {
                    bytes[i] = GetHighBits(8);
                    i = i + 1;
                    bits -= 8;
                }

                bytes[i] = GetHighBits(bits);
                return bytes;
            }

            private byte GetHighBits(int bits)
            {
                int ret = 0;
                while (bits > 0)
                {
                    bits -= 1;
                    ret = (int)((long)ret | (long)Maths.Pow(2d, bits));
                }

                return (byte)ret;
            }

            public override CModulesClass GetModule()
            {
                return Instance.Module;
            }

            public override string GetTableName()
            {
                return "tbl_AllowedIPs";
            }

            private byte[] FromString(string str)
            {
                byte[] ret;
                string[] tmp;
                tmp = Strings.Split(Strings.Trim(str), ".");
                ret = new byte[DMD.Arrays.UBound(tmp) + 1];
                for (int i = 0, loopTo = DMD.Arrays.UBound(tmp); i <= loopTo; i++)
                    ret[i] = DMD.Bytes.Parse(tmp[i]);
                return ret;
            }

            private string ToString(byte[] items)
            {
                string[] tmp;
                tmp = new string[DMD.Arrays.UBound(items) + 1];
                for (int i = 0, loopTo = DMD.Arrays.UBound(tmp); i <= loopTo; i++)
                    tmp[i] = items[i].ToString();
                return Strings.Join(tmp, ".");
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se considerare l'indirizzo come interno all'azienda
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Interno
            {
                get
                {
                    return m_Interno;
                }

                set
                {
                    if (m_Interno == value)
                        return;
                    m_Interno = value;
                    DoChanged("Interno", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome dell'ufficio da associare al login in caso l'utnte effettui l'accesso da un IP consentito da questo oggetto
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string AssociaUfficio
            {
                get
                {
                    return m_AssociaUfficio;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_AssociaUfficio;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(oldValue ?? "", value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        return;
                    m_AssociaUfficio = value;
                    DoChanged("AssociaUfficio", value, oldValue);
                }
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Databases.APPConn;
            }

            protected override bool LoadFromRecordset(Databases.DBReader reader)
            {
                IP = FromString(reader.GetValue("IP", ""));
                NetMask = FromString(reader.GetValue("NetMask", ""));
                m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                m_Allow = reader.Read("Allow", this.m_Allow);
                m_Negate = reader.Read("Negate", this.m_Negate);
                m_Interno = reader.Read("Interno", this.m_Interno);
                m_AssociaUfficio = reader.Read("AssociaUfficio", this.m_AssociaUfficio);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(Databases.DBWriter writer)
            {
                writer.Write("IP", ToString(IP));
                writer.Write("NetMask", ToString(NetMask));
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("Allow", m_Allow);
                writer.Write("Negate", m_Negate);
                writer.Write("Interno", m_Interno);
                writer.Write("AssociaUfficio", m_AssociaUfficio);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IP", ToString(IP));
                writer.WriteAttribute("NetMask", ToString(NetMask));
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("Allow", m_Allow);
                writer.WriteAttribute("Negate", m_Negate);
                writer.WriteAttribute("Interno", m_Interno);
                writer.WriteAttribute("AssociaUfficio", m_AssociaUfficio);
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case var @case when CultureInfo.CurrentCulture.CompareInfo.Compare(@case, "IP", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_IP = FromString(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case var case1 when CultureInfo.CurrentCulture.CompareInfo.Compare(case1, "NetMask", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_NetMask = FromString(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case var case2 when CultureInfo.CurrentCulture.CompareInfo.Compare(case2, "Descrizione", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case3 when CultureInfo.CurrentCulture.CompareInfo.Compare(case3, "Allow", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_Allow = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case var case4 when CultureInfo.CurrentCulture.CompareInfo.Compare(case4, "Negate", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_Negate = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case var case5 when CultureInfo.CurrentCulture.CompareInfo.Compare(case5, "Interno", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_Interno = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case var case6 when CultureInfo.CurrentCulture.CompareInfo.Compare(case6, "AssociaUfficio", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            m_AssociaUfficio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public override string ToString()
            {
                return ToString(m_IP) + "/" + ToString(m_NetMask);
            }

            public int CompareTo(IPADDRESSinfo obj)
            {
                if (Negate & !obj.Negate)
                {
                    return -1;
                }
                else if (!Negate & obj.Negate)
                {
                    return 1;
                }
                else
                {
                    return DMD.Strings.Compare(ToString(), obj.ToString(), true);
                }
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((IPADDRESSinfo)obj);
            }
        }
    }
}