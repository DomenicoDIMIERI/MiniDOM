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


namespace minidom
{
    public partial class Sistema
    {
        public class CNumberInfo 
            : IDMDXMLSerializable
        {
            public string Zona;
            public string Nazione;
            public string PrefissoInternazionale;
            public string PrefissoLocale;
            public string Distretto;
            public string Numero;

            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer); }
            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue); }

            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Zona", Zona);
                writer.WriteAttribute("Nazione", Nazione);
                writer.WriteAttribute("PrefissoInternazionale", PrefissoInternazionale);
                writer.WriteAttribute("PrefissoLocale", PrefissoLocale);
                writer.WriteAttribute("Distretto", Distretto);
                writer.WriteAttribute("Numero", Numero);
            }

            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Zona":
                        {
                            Zona = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Nazione":
                        {
                            Nazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PrefissoInternazionale":
                        {
                            PrefissoInternazionale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PrefissoLocale":
                        {
                            PrefissoLocale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Distretto":
                        {
                            Distretto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Numero":
                        {
                            Numero = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                string ret;
                ret = DMD.Strings.Combine(PrefissoInternazionale, PrefissoLocale, " ");
                ret = DMD.Strings.Combine(ret, Numero, " ");
                return ret;
            }
        }

        public class CPhonePrefix
        {
            public string destinazione;
            public string per;
            public string da;
            public int zona;

            public CPhonePrefix(string destinazione, string per, string da = DMD.Strings.vbNullString, int zona = 0)
            {
                this.destinazione = DMD.Strings.Trim(destinazione);
                this.per = DMD.Strings.Trim(per);
                this.da = da;
                this.zona = zona;
            }
        }

        public sealed class CFormatsClass
        {
            private object lockPrefissi = new object();
            private CCollection<CPhonePrefix> m_Prefissi;
            public readonly string _defaultPhoneNumberFormat = "ITA";
            public readonly string _validPhoneNumberChars = "+0123456789";

            internal CFormatsClass()
            {
                //DMDObject.IncreaseCounter(this);
                m_Prefissi = null;
            }

            public string FormatBytes(long value, int numDecimals = 0)
            {
                const long KB = 1024L;
                const long MB = 1048576L;
                const long GB = 1073741824L;
                string ret;
                if (value < KB)
                {
                    ret = Formats.FormatNumber(value, numDecimals) + " bytes";
                }
                else if (value < MB)
                {
                    ret = Formats.FormatNumber(value / (double)KB, numDecimals) + " KB";
                }
                else if (value < GB)
                {
                    ret = Formats.FormatNumber(value / (double)MB, numDecimals) + " MB";
                }
                else
                {
                    ret = Formats.FormatNumber(value / (double)GB, numDecimals) + " GB";
                }

                return ret;
            }

            public string Format(object value, string formatString)
            {
                const string separators = " /|\"'()[]{}+*°<>!_-.:,;";
                int i;
                string ch, ret, token;
                //int status;
                i = 1;
                //status = 0;
                token = "";
                ret = "";
                while (i <= DMD.Strings.Len(formatString))
                {
                    ch = DMD.Strings.Mid(formatString, i, 1);
                    if (DMD.Strings.InStr(separators, ch) > 0)
                    {
                        switch (token ?? "")
                        {
                            case "d":
                                {
                                    token = DMD.DateUtils.Day(DMD.DateUtils.CDate(value)).ToString();
                                    break;
                                }

                            case "dd":
                                {
                                    token = DMD.Strings.Right("00" + DMD.DateUtils.Day(DMD.DateUtils.CDate(value)), 2);
                                    break;
                                }

                            case "ddd":
                                {
                                    token = DMD.DateUtils.WeekdayName(DMD.DateUtils.Weekday(DMD.DateUtils.CDate(value)), true);
                                    break;
                                }

                            case "dddd":
                                {
                                    token = DMD.DateUtils.WeekdayName(DMD.DateUtils.Weekday(DMD.DateUtils.CDate(value)), false);
                                    break;
                                }

                            case "M":
                                {
                                    token = DMD.DateUtils.Month(DMD.DateUtils.CDate(value)).ToString();
                                    break;
                                }

                            case "MM":
                                {
                                    token = DMD.Strings.Right("00" + DMD.DateUtils.Month(DMD.DateUtils.CDate(value)), 2);
                                    break;
                                }

                            case "MMM":
                                {
                                    token = DMD.DateUtils.MonthName(DMD.DateUtils.Month(DMD.DateUtils.CDate(value)), true);
                                    break;
                                }

                            case "MMMM":
                                {
                                    token = DMD.DateUtils.MonthName(DMD.DateUtils.Month(DMD.DateUtils.CDate(value)), false);
                                    break;
                                }

                            case "yy":
                                {
                                    token = DMD.Strings.Right("00" + DMD.DateUtils.Year(DMD.DateUtils.CDate(value)), 2);
                                    break;
                                }

                            case "yyy":
                                {
                                    token = DMD.DateUtils.Year(DMD.DateUtils.CDate(value)).ToString();
                                    break;
                                }

                            case "yyyy":
                                {
                                    token = DMD.DateUtils.Year(DMD.DateUtils.CDate(value)).ToString();
                                    break;
                                }

                            case "h":
                                {
                                    token = DMD.DateUtils.Hour(DMD.DateUtils.CDate(value)).ToString();
                                    break;
                                }

                            case "hh":
                                {
                                    token = DMD.Strings.Right("00" + DMD.DateUtils.Hour(DMD.DateUtils.CDate(value)), 2);
                                    break;
                                }

                            case "m":
                                {
                                    token = DMD.DateUtils.Minute(DMD.DateUtils.CDate(value)).ToString();
                                    break;
                                }

                            case "mm":
                                {
                                    token = DMD.Strings.Right("00" + DMD.DateUtils.Minute(DMD.DateUtils.CDate(value)), 2);
                                    break;
                                }

                            case "s":
                                {
                                    token = DMD.DateUtils.Second(DMD.DateUtils.CDate(value)).ToString();
                                    break;
                                }

                            case "ss":
                                {
                                    token = DMD.Strings.Right("00" + DMD.DateUtils.Second(DMD.DateUtils.CDate(value)), 2);
                                    break;
                                }

                            default:
                                {
                                    break;
                                }
                        }

                        ret += token;
                        ret += ch;
                        token = "";
                    }
                    else
                    {
                        token = token + ch;
                    }

                    i = i + 1;
                }

                ch = "";
                switch (token ?? "")
                {
                    case "d":
                        {
                            token = DMD.DateUtils.Day(DMD.DateUtils.CDate(value)).ToString();
                            break;
                        }

                    case "dd":
                        {
                            token = DMD.Strings.Right("00" + DMD.DateUtils.Day(DMD.DateUtils.CDate(value)), 2);
                            break;
                        }

                    case "ddd":
                        {
                            token = DMD.DateUtils.WeekdayName(DMD.DateUtils.Weekday(DMD.DateUtils.CDate(value)), true);
                            break;
                        }

                    case "dddd":
                        {
                            token = DMD.DateUtils.WeekdayName(DMD.DateUtils.Weekday(DMD.DateUtils.CDate(value)), false);
                            break;
                        }

                    case "M":
                        {
                            token = DMD.DateUtils.Month(DMD.DateUtils.CDate(value)).ToString();
                            break;
                        }

                    case "MM":
                        {
                            token = DMD.Strings.Right("00" + DMD.DateUtils.Month(DMD.DateUtils.CDate(value)), 2);
                            break;
                        }

                    case "MMM":
                        {
                            token = DMD.DateUtils.MonthName(DMD.DateUtils.Month(DMD.DateUtils.CDate(value)), true);
                            break;
                        }

                    case "MMMM":
                        {
                            token = DMD.DateUtils.MonthName(DMD.DateUtils.Month(DMD.DateUtils.CDate(value)), false);
                            break;
                        }

                    case "yy":
                        {
                            token = DMD.Strings.Right("00" + DMD.DateUtils.Year(DMD.DateUtils.CDate(value)), 2);
                            break;
                        }

                    case "yyy":
                        {
                            token = DMD.DateUtils.Year(DMD.DateUtils.CDate(value)).ToString();
                            break;
                        }

                    case "yyyy":
                        {
                            token = DMD.DateUtils.Year(DMD.DateUtils.CDate(value)).ToString();
                            break;
                        }

                    case "h":
                        {
                            token = DMD.DateUtils.Hour(DMD.DateUtils.CDate(value)).ToString();
                            break;
                        }

                    case "hh":
                        {
                            token = DMD.Strings.Right("00" + DMD.DateUtils.Hour(DMD.DateUtils.CDate(value)), 2);
                            break;
                        }

                    case "m":
                        {
                            token = DMD.DateUtils.Minute(DMD.DateUtils.CDate(value)).ToString();
                            break;
                        }

                    case "mm":
                        {
                            token = DMD.Strings.Right("00" + DMD.DateUtils.Minute(DMD.DateUtils.CDate(value)), 2);
                            break;
                        }

                    case "s":
                        {
                            token = DMD.DateUtils.Second(DMD.DateUtils.CDate(value)).ToString();
                            break;
                        }

                    case "ss":
                        {
                            token = DMD.Strings.Right("00" + DMD.DateUtils.Second(DMD.DateUtils.CDate(value)), 2);
                            break;
                        }

                    case "#":
                        {
                            token = "" + DMD.Doubles.CDbl(value);
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }

                ret += token;
                ret += ch;
                token = "";
                return ret;
            }

            public string TrimInternationalPrefix(string value)
            {
                var i = GetInfoNumero(value);
                return DMD.Strings.Combine(i.PrefissoLocale, i.Numero, " ");
            }

            public CNumberInfo GetInfoNumero(string value)
            {
                lock (lockPrefissi)
                {
                    value = DMD.Strings.Trim(value);
                    if (value.StartsWith("00"))
                        value = "+" + value.Substring(2);
                    var items = GetPrefissiInt();
                    var ret = new CNumberInfo();
                    foreach (CPhonePrefix p in items)
                    {
                        if (value.StartsWith(p.per))
                        {
                            if (p.zona != 0)
                            {
                                ret.PrefissoInternazionale = p.per;
                                ret.Zona = p.zona.ToString();
                                ret.Nazione = p.destinazione;
                                value = value.Substring(p.per.Length);
                            }
                            else
                            {
                                ret.PrefissoLocale = p.per;
                                ret.Distretto = p.destinazione;
                                ret.Numero = value.Substring(p.per.Length);
                                value = "";
                            }

                            if (string.IsNullOrEmpty(value))
                                break;
                        }
                    }

                    if (!string.IsNullOrEmpty(value))
                        ret.Numero = value;
                    return ret;
                }
            }

            public string ParsePartitaIVA(string value)
            {
                string validLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string validNumbers = "0123456789";
                string ret = "";
                value = DMD.Strings.UCase(DMD.Strings.Replace(value, " ", ""));
                int i = 1;
                while (i <= DMD.Maths.Min(2, DMD.Strings.Len(value)) && DMD.Strings.InStr(validLetters, DMD.Strings.Mid(value, i, 1)) > 0)
                {
                    ret += DMD.Strings.Mid(value, i, 1);
                    i += 1;
                }

                while (i <= Maths.Min(13, DMD.Strings.Len(value)) && DMD.Strings.InStr(validNumbers, DMD.Strings.Mid(value, i, 1)) > 0)
                {
                    ret += DMD.Strings.Mid(value, i, 1);
                    i += 1;
                }

                return ret;
            }

            public string FormatPartitaIVA(string value)
            {
                string ret;
                ret = DMD.Strings.UCase(DMD.Strings.Replace("" + value, " ", ""));
                return ret;
            }

            public string FormatUserTime(object value)
            {
                return DMD.DateUtils.Format(value, "hh:mm:ss");
            }

            public string FormatUserDateTimeOggi(object value)
            {
                var d = DMD.DateUtils.TryParse(value);
                if (d == null) return string.Empty;
                var date = d.Value;

                if (DMD.DateUtils.GetDatePart(date) == DMD.DateUtils.ToDay() )
                {
                    return "Oggi alle " + DMD.DateUtils.Format(date, "hh:mm:ss");
                }
                else if (DMD.DateUtils.GetDatePart(date) == DMD.DateUtils.YesterDay() == true)
                {
                    return "Ieri alle " + DMD.DateUtils.Format(value, "hh:mm:ss");
                }
                else
                {
                    return DMD.DateUtils.Format(value, "dd/MM/yyyy hh:mm:ss");
                }
            }

            public bool IsNullOrNothing(object value)
            {
                if (value is DBNull)
                    return true;
                if (value is ValueType)
                    return false;
                return value is null;
            }

            public DateTime? ParseDate(object value, bool ignoreErrors = false)
            {
                if (IsNullOrNothing(value))
                    return default;
                if (ignoreErrors)
                {
                    try
                    {
                        return ParseDate(value, false);
                    }
                    catch (Exception)
                    {
                        return default;
                    }
                }
                else if (value is string)
                {
                    if (string.IsNullOrEmpty(DMD.Strings.Trim(DMD.Strings.CStr(value))))
                        return default;
                    value = DMD.Strings.Replace(DMD.Strings.CStr(value), ".", ":");
                    return DMD.DateUtils.CDate(value);
                }
                else
                {
                    return DMD.DateUtils.CDate(value);
                }
            }

            // Public Function ParseDate(ByVal value As Object, ByVal format As String) As Date?
            // If IsNullOrNothing(value) Then Return Nothing

            // End Function

        

            

            public DateTime ToDate(object value, object defaultValue = null)
            {
                var ret = ParseDate(value);
                if (ret.HasValue)
                    return ret.Value;
                return DMD.DateUtils.CDate(defaultValue);
            }

            public string FormatValuta(object value, int numDecimals = 2, string symbol = DMD.Strings.vbNullString)
            {
                if (IsNullOrNothing(value))
                    return DMD.Strings.vbNullString;
                return DMD.Strings.Trim(FormatNumber(value, numDecimals) + " " + symbol);
            }

            public string FormatValuta0(object value)
            {
                if (IsNullOrNothing(value))
                    return null;
                return FormatNumber(value, 2);
            }

            public double? ParseDouble(object value, bool ignoreErrors = false)
            {
                if (IsNullOrNothing(value))
                    return default;
                if (ignoreErrors)
                {
                    try
                    {
                        return ParseDouble(value, false);
                    }
                    catch (Exception)
                    {
                        return default;
                    }
                }
                else if (value is string)
                {
                    string tValue = DMD.Strings.Trim(DMD.Strings.CStr(value));
                    if (string.IsNullOrEmpty(tValue))
                        return default;
                    tValue = DMD.Strings.Replace(tValue, ".", "");
                    return DMD.Doubles.CDbl(tValue);
                }
                else
                {
                    return DMD.Doubles.CDbl(value);
                }
            }

            public float? ParseSingle(object value, bool ignoreErrors = false)
            {
                if (IsNullOrNothing(value))
                    return default;
                if (ignoreErrors)
                {
                    try
                    {
                        return ParseSingle(value, false);
                    }
                    catch (Exception)
                    {
                        return default;
                    }
                }
                else if (value is string)
                {
                    string tValue = DMD.Strings.Trim(DMD.Strings.CStr(value));
                    if (string.IsNullOrEmpty(tValue))
                        return default;
                    tValue = DMD.Strings.Replace(tValue, ".", "");
                    return DMD.Floats.CSng(tValue);
                }
                else
                {
                    return DMD.Floats.CSng(value);
                }
            }

            public double? ParsePercentage(object value)
            {
                return ParseDouble(value);
            }

            public decimal? ParseValuta(object value, bool ignoreErrors = false)
            {
                if (IsNullOrNothing(value))
                    return default;
                if (ignoreErrors)
                {
                    try
                    {
                        return ParseValuta(value, false);
                    }
                    catch (Exception)
                    {
                        return default;
                    }
                }
                else if (value is string)
                {
                    string tValue = DMD.Strings.Trim(DMD.Strings.CStr(value));
                    if (string.IsNullOrEmpty(tValue))
                        return default;
                    tValue = DMD.Strings.Replace(tValue, ".", "");
                    return DMD.Decimals.CDec(tValue);
                }
                else
                {
                    return DMD.Decimals.CDec(value);
                }
            }

            public decimal ToValuta(object value, decimal defValue = 0m)
            {
                var ret = ParseValuta(value);
                if (ret.HasValue)
                    return ret.Value;
                return defValue;
            }

            public string FormatFloat(object value, int numDecimals = 2)
            {
                if (IsNullOrNothing(value))
                    return null;
                return FormatNumber(value, numDecimals);
            }

            public int? ParseInteger(object value, bool ignoreErrors = false)
            {
                if (IsNullOrNothing(value))
                    return default;
                if (ignoreErrors)
                {
                    try
                    {
                        return ParseInteger(value, false);
                    }
                    catch (Exception )
                    {
                        return default;
                    }
                }
                else if (value is string)
                {
                    string tValue = DMD.Strings.Trim(DMD.Strings.CStr(value));
                    if (string.IsNullOrEmpty(tValue))
                        return default;
                    tValue = DMD.Strings.Replace(tValue, ".", "");
                    long ret;
                    /* TODO ERROR: Skipped IfDirectiveTrivia */
                    try
                    {
                        ret = DMD.Longs.CLng(tValue);
                        if (ret > 2147483648L)
                            ret = ret - 4294967296L;
                    }
                    catch (Exception )
                    {
                        throw;
                    }
                    /* TODO ERROR: Skipped ElseDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                    return (int)ret;
                }
                else
                {
                    return DMD.Integers.ValueOf(value);
                }
            }

            public long? ParseLong(object value, bool ignoreErrors = false)
            {
                if (IsNullOrNothing(value))
                    return default;
                if (ignoreErrors)
                {
                    try
                    {
                        return ParseLong(value, false);
                    }
                    catch (Exception)
                    {
                        return default;
                    }
                }
                else if (value is string)
                {
                    string tValue = DMD.Strings.Trim(DMD.Strings.CStr(value));
                    if (string.IsNullOrEmpty(tValue))
                        return default;
                    tValue = DMD.Strings.Replace(tValue, ".", "");
                    return DMD.Longs.CLng(tValue);
                }
                else
                {
                    return DMD.Longs.CLng(value);
                }
            }

            public bool? ParseBool(object value, bool ignoreErrors = false)
            {
                if (IsNullOrNothing(value))
                    return default;
                if (value is bool || value is bool?)
                    return (bool?)value;
                if (ignoreErrors)
                {
                    try
                    {
                        return ParseBool(value, false);
                    }
                    catch (Exception)
                    {
                        return default;
                    }
                }
                else if (value is string)
                {
                    string tmp = DMD.Strings.Trim(DMD.Strings.CStr(value));
                    if (string.IsNullOrEmpty(tmp))
                        return default;
                    return DMD.Booleans.ValueOf(value);
                }
                else
                {
                    return DMD.Booleans.ValueOf(value);
                }
            }

            public byte? ParseByte(object value)
            {
                if (IsNullOrNothing(value))
                    return default;
                if (value is byte || value is byte?)
                    return (byte?)value;
                if (value is string)
                {
                    string tmp = DMD.Strings.Trim(DMD.Strings.CStr(value));
                    if (string.IsNullOrEmpty(tmp))
                        return default;
                    return DMD.Bytes.CByte(value);
                }
                else
                {
                    return DMD.Bytes.CByte(value);
                }
            }

            public sbyte? ParseSByte(object value)
            {
                if (IsNullOrNothing(value))
                    return default;
                if (value is sbyte || value is sbyte?)
                    return (sbyte?)value;
                if (value is string)
                {
                    string tmp = DMD.Strings.Trim(DMD.Strings.CStr(value));
                    if (string.IsNullOrEmpty(tmp))
                        return default;
                    return DMD.SBytes.ValueOf(value);
                }
                else
                {
                    return DMD.SBytes.ValueOf(value);
                }
            }

            public char? ParseChar(object value)
            {
                if (IsNullOrNothing(value))
                    return default;
                if (value is char || value is char?)
                    return (char?)value;
                if (value is string)
                {
                    string tmp = DMD.Strings.Trim(DMD.Strings.CStr(value));
                    if (string.IsNullOrEmpty(tmp))
                        return default;
                    return DMD.Chars.CChar(value);
                }
                else
                {
                    return DMD.Chars.CChar(value);
                }
            }

            public short? ParseShort(object value)
            {
                if (IsNullOrNothing(value))
                    return default;
                if (value is short || value is short?)
                    return (short?)value;
                if (value is string)
                {
                    string tmp = DMD.Strings.Trim(DMD.Strings.CStr(value));
                    if (string.IsNullOrEmpty(tmp))
                        return default;
                    return DMD.Shorts.ValueOf(value);
                }
                else
                {
                    return DMD.Shorts.ValueOf(value);
                }
            }

            public ushort? ParseUShort(object value)
            {
                if (IsNullOrNothing(value))
                    return default;
                if (value is ushort || value is ushort?)
                    return (ushort?)value;
                if (value is string)
                {
                    string tmp = DMD.Strings.Trim(DMD.Strings.CStr(value));
                    if (string.IsNullOrEmpty(tmp))
                        return default;
                    return DMD.UShorts.ValueOf(value);
                }
                else
                {
                    return DMD.UShorts.ValueOf(value);
                }
            }

            public ulong? ParseULong(object value)
            {
                if (IsNullOrNothing(value))
                    return default;
                if (value is ulong || value is ulong?)
                    return (ulong?)value;
                if (value is string)
                {
                    string tmp = DMD.Strings.Trim(DMD.Strings.CStr(value));
                    if (string.IsNullOrEmpty(tmp))
                        return default;
                    return DMD.ULongs.CULng(value);
                }
                else
                {
                    return DMD.ULongs.CULng(value);
                }
            }

            public uint? ParseUInteger(object value)
            {
                if (IsNullOrNothing(value))
                    return default;
                if (value is uint || value is uint?)
                    return (uint?)value;
                if (value is string)
                {
                    string tmp = DMD.Strings.Trim(DMD.Strings.CStr(value));
                    if (string.IsNullOrEmpty(tmp))
                        return default;
                    return DMD.UIntegers.CUInt(value);
                }
                else
                {
                    return DMD.UIntegers.CUInt(value);
                }
            }

            public decimal? ParseDecimal(object value)
            {
                if (IsNullOrNothing(value))
                    return default;
                if (value is decimal || value is decimal?)
                    return (decimal?)value;
                if (value is string)
                {
                    string tmp = DMD.Strings.Trim(DMD.Strings.CStr(value));
                    if (string.IsNullOrEmpty(tmp))
                        return default;
                    return DMD.Decimals.CDec(value);
                }
                else
                {
                    return DMD.Decimals.CDec(value);
                }
            }

            public string ToString(object value)
            {
                if (IsNullOrNothing(value))
                    return null;
                if (value is string)
                    return DMD.Strings.CStr(value);
                return value.ToString();
            }

            public double ToDouble(object value, double defValue = 0d)
            {
                var ret = ParseDouble(value);
                if (ret.HasValue)
                    return ret.Value;
                return defValue;
            }

            public string FormatUserDate(object value)
            {
                if (IsNullOrNothing(value))
                    return null;
                return DMD.DateUtils.Format(value, "dd/MM/yyyy");
            }

            public string FormatUserDate(object value, string format)
            {
                if (IsNullOrNothing(value))
                    return null;
                return DMD.DateUtils.Format(value, format);
            }

            public string FormatUserDateTime(object value)
            {
                if (IsNullOrNothing(value))
                    return DMD.Strings.vbNullString;
                return DMD.DateUtils.Format(value, "dd/MM/yyyy HH:mm:ss");
            }

            public string FormatCodiceFiscale(string value)
            {
                string ret = DMD.Strings.Replace(value, " ", "");
                if (
                       (DMD.Strings.Compare(DMD.Strings.Left(ret, 1), "0", false) >= 0)
                    && (DMD.Strings.Compare(DMD.Strings.Left(ret, 1), "9", false) <= 0)
                    )
                    return FormatPartitaIVA(ret);
                return DMD.Strings.Trim(DMD.Strings.Mid(ret, 1, 3) + " " + DMD.Strings.Mid(ret, 4, 3) + " " + DMD.Strings.Mid(ret, 7, 5) + " " + DMD.Strings.Mid(ret, 12, 4) + " " + DMD.Strings.Mid(ret, 16, 1));
            }

            public string FormatNumber(object num, int decimalNum = 0, bool bolLeadingZero = true, bool bolParens = false, bool bolCommas = true)
            {
                num = ParseDouble(num);
                //return DMD.Doubles.FormatNumber(num, decimalNum,  bolLeadingZero, bolParens, bolCommas);
                return DMD.Doubles.Format(num);
            }

            public string FormatInteger(object value)
            {
                if (IsNullOrNothing(value))
                    return DMD.Strings.vbNullString;
                return FormatNumber(value, 0);
            }

            public string FormatPercentage(object value, int n = 2)
            {
                if (IsNullOrNothing(value))
                    return DMD.Strings.vbNullString;
                return FormatNumber(value, n, true, false, true);
            }

            /// <summary>
        /// Restituisce vero se tutti i caratteri della stringa rappresentano una cifra numerica  da 0 a 9
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
            private bool IsAllNumbers(string text, int from = 0, int len = -1)
            {
                const string validNumbers = "0123456789";
                if (len < 0)
                    len = DMD.Strings.Len(text);
                for (int i = from, loopTo = from + len - 1; i <= loopTo; i++)
                {
                    char ch = text[i];
                    if (validNumbers.IndexOf(ch) < 0)
                        return false;
                }

                return true;
            }

            /// <summary>
        /// Restituisce vero se tutti i caratteri della stringa rappresentano una lettera
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
            private bool IsAllLetters(string text, int from = 0, int len = -1)
            {
                const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                if (len < 0)
                    len = DMD.Strings.Len(text);
                for (int i = from, loopTo = from + len - 1; i <= loopTo; i++)
                {
                    char ch = text[i];
                    if (validChars.IndexOf(ch) < 0)
                        return false;
                }

                return true;
            }

            public string ParseCodiceFiscale(string value)
            {
                value = DMD.Strings.UCase(DMD.Strings.Replace(value, " ", ""));
                if (DMD.Strings.Len(value) == 11 && IsAllNumbers(value))
                {
                    return value;
                }
                else if (DMD.Strings.Len(value) == 13 && IsAllLetters(value, 0, 2) && IsAllNumbers(value, 2, 11))
                {
                    return value;
                }
                else
                {
                    const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    const string validNumbers = "0123456789";
                    string ret = "";
                    value = DMD.Strings.UCase(DMD.Strings.Replace(value, " ", ""));
                    if (DMD.Strings.InStr(validChars, DMD.Strings.Mid(value, 1, 1)) > 0 && DMD.Strings.InStr(validNumbers, DMD.Strings.Mid(value, 7, 1)) > 0)
                    {
                        string ch;
                        for (int i = 1, loopTo = Maths.Min(6, DMD.Strings.Len(value)); i <= loopTo; i++)
                        {
                            ch = DMD.Strings.Mid(value, i, 1);
                            if (DMD.Strings.InStr(validChars, ch) > 0)
                                ret += ch;
                        }

                        for (int i = 7, loopTo1 = Maths.Min(8, DMD.Strings.Len(value)); i <= loopTo1; i++)
                        {
                            ch = DMD.Strings.Mid(value, i, 1);
                            if (DMD.Strings.InStr(validNumbers, ch) > 0)
                                ret += ch;
                        }

                        for (int i = 9, loopTo2 = Maths.Min(9, DMD.Strings.Len(value)); i <= loopTo2; i++)
                        {
                            ch = DMD.Strings.Mid(value, i, 1);
                            if (DMD.Strings.InStr(validChars, ch) > 0)
                                ret += ch;
                        }

                        for (int i = 10, loopTo3 = Maths.Min(11, DMD.Strings.Len(value)); i <= loopTo3; i++)
                        {
                            ch = DMD.Strings.Mid(value, i, 1);
                            if (DMD.Strings.InStr(validNumbers, ch) > 0)
                                ret += ch;
                        }

                        for (int i = 12, loopTo4 = Maths.Min(16, DMD.Strings.Len(value)); i <= loopTo4; i++)
                        {
                            ch = DMD.Strings.Mid(value, i, 1);
                            if (DMD.Strings.InStr(validNumbers, ch) > 0 || DMD.Strings.InStr(validChars, ch) > 0)
                                ret += ch;
                        }
                    }
                    else
                    {
                        ret = "";
                    }

                    return ret;
                }
            }

            public bool ToBool(object value, bool defValue = false)
            {
                var ret = ParseBool(value);
                if (ret.HasValue)
                    return ret.Value;
                return defValue;
            }

            public bool ToBoolean(object value, bool defValue = false)
            {
                return ToBool(value, defValue);
            }

            public int ToInteger(object value, int defValue = 0)
            {
                var ret = ParseInteger(value);
                if (ret.HasValue)
                    return ret.Value;
                return defValue;
            }

            public long ToLong(object value, long defValue = 0L)
            {
                var ret = ParseLong(value);
                if (ret.HasValue)
                    return ret.Value;
                return defValue;
            }

            // Public Function ParseInteger(ByVal value As Object) As Integer?
            // var ret = null;
            // if (value != null) {
            // if (typeof (value) == "number") {
            // ret = parseInt(value);
            // } else {
            // ret = parseInt(DMD.Strings.Replace(value.toString(), ",", ""));
            // }
            // if (ret > 2147483648) ret = ret - 4294967296;
            // }
            // return ret;
            // }
            // Formats.ParseBool = function (value) {
            // try {
            // return (value == true);
            // } catch (ex) {
            // return null;
            // }    
            // }
            // Formats.ParseBoolean = function (value) { return Formats.ParseBool(value); }


            private void _AddPrefixInt(int zona, string nazione, string per, string da = DMD.Strings.vbNullString)
            {
                m_Prefissi.Add(new CPhonePrefix(nazione, per, da, zona));
            }

            public void _AddPrefix(string destinazione, string per, string da = DMD.Strings.vbNullString)
            {
                m_Prefissi.Add(new CPhonePrefix(destinazione, per, da));
            }

            private CCollection<CPhonePrefix> GetPrefissiInt()
            {
                if (m_Prefissi is null)
                    _InitPhonePrefixes();
                return m_Prefissi;
            }

            public CCollection<CPhonePrefix> GetPrefissi()
            {
                lock (this)
                    return new CCollection<CPhonePrefix>(GetPrefissiInt());
            }

            private void _InitPhonePrefixes()
            {
                // Zona 1: America settentrionale
                m_Prefissi = new CCollection<CPhonePrefix>();
                _AddPrefixInt(1, "Stati Uniti d'America", "+1");
                Formats._AddPrefixInt(1, "Canada", "+1");
                // Zona 2: Principalmente Africa
                Formats._AddPrefixInt(2, "Egitto", "+20");
                Formats._AddPrefixInt(2, "riservato al Marocco", "+210");
                Formats._AddPrefixInt(2, "Sudan del Sud", "+211");
                Formats._AddPrefixInt(2, "Marocco", "+212");
                Formats._AddPrefixInt(2, "Algeria", "+213");
                Formats._AddPrefixInt(2, "riservato all'Algeria", "+214");
                Formats._AddPrefixInt(2, "riservato all'Algeria", "+215");
                Formats._AddPrefixInt(2, "Tunisia", "+216");
                Formats._AddPrefixInt(2, "riservato allaTunisia", "+217");
                Formats._AddPrefixInt(2, "Libia", "+218");
                Formats._AddPrefixInt(2, "riservato alla Libia", "+219");
                Formats._AddPrefixInt(2, "Gambia", "+220");
                Formats._AddPrefixInt(2, "Senegal", "+221");
                Formats._AddPrefixInt(2, "Mauritania", "+222");
                Formats._AddPrefixInt(2, "Mali", "+223");
                Formats._AddPrefixInt(2, "Guinea", "+224");
                Formats._AddPrefixInt(2, "Costa d'Avorio", "+225");
                Formats._AddPrefixInt(2, "Burkina Faso", "+226");
                Formats._AddPrefixInt(2, "Niger", "+227");
                Formats._AddPrefixInt(2, "Togo", "+228");
                Formats._AddPrefixInt(2, "Benin", "+229");
                Formats._AddPrefixInt(2, "Mauritius", "+230");
                Formats._AddPrefixInt(2, "Liberia", "+231");
                Formats._AddPrefixInt(2, "Sierra Leone", "+232");
                Formats._AddPrefixInt(2, "Ghana", "+233");
                Formats._AddPrefixInt(2, "Nigeria", "+234");
                Formats._AddPrefixInt(2, "Ciad", "+235");
                Formats._AddPrefixInt(2, "Repubblica Centrafricana", "+236");
                Formats._AddPrefixInt(2, "Camerun", "+237");
                Formats._AddPrefixInt(2, "Capo Verde", "+238");
                Formats._AddPrefixInt(2, "Sao Tome e Principe", "+239");
                Formats._AddPrefixInt(2, "Guinea Equatoriale", "+240");
                Formats._AddPrefixInt(2, "Gabon", "+241");
                Formats._AddPrefixInt(2, "Repubblica del Congo (Brazzaville)", "+242");
                Formats._AddPrefixInt(2, "Repubblica Democratica del Congo (Kinshasa, precedentemente detta Zaire)", "+243");
                Formats._AddPrefixInt(2, "Angola", "+244");
                Formats._AddPrefixInt(2, "Angola", "+244");
                Formats._AddPrefixInt(2, "Guinea-Bissau", "+245");
                Formats._AddPrefixInt(2, "Diego Garcia", "+246");
                Formats._AddPrefixInt(2, "Isola Ascensione", "+247");
                Formats._AddPrefixInt(2, "Seychelles", "+248");
                Formats._AddPrefixInt(2, "Sudan", "+249");
                Formats._AddPrefixInt(2, "Ruanda", "+250");
                Formats._AddPrefixInt(2, "Etiopia", "+251");
                Formats._AddPrefixInt(2, "Somalia", "+252");
                Formats._AddPrefixInt(2, "Gibuti", "+253");
                Formats._AddPrefixInt(2, "Kenia", "+254");
                Formats._AddPrefixInt(2, "Tanzania", "+255");
                Formats._AddPrefixInt(2, "Uganda", "+256");
                Formats._AddPrefixInt(2, "Burundi", "+257");
                Formats._AddPrefixInt(2, "Mozambico", "+258");
                Formats._AddPrefixInt(2, "Zanzibar – mai implementato (vedi +255 Tanzania)", "+259");
                Formats._AddPrefixInt(2, "Zambia", "+260");
                Formats._AddPrefixInt(2, "Madagascar", "+261");
                Formats._AddPrefixInt(2, "Riunione", "+262");
                Formats._AddPrefixInt(2, "Zimbabwe", "+263");
                Formats._AddPrefixInt(2, "Namibia", "+264");
                Formats._AddPrefixInt(2, "Malawi", "+265");
                Formats._AddPrefixInt(2, "Lesotho", "+266");
                Formats._AddPrefixInt(2, "Botswana", "+267");
                Formats._AddPrefixInt(2, "Swaziland", "+268");
                Formats._AddPrefixInt(2, "Comore e Mayotte", "+269");
                Formats._AddPrefixInt(2, "Sudafrica", "+27");
                Formats._AddPrefixInt(2, "Sant'Elena", "+290");
                Formats._AddPrefixInt(2, "Eritrea", "+291");
                Formats._AddPrefixInt(2, "dismesso (era assegnato a San Marino, che ora usa +378)", "+295");
                Formats._AddPrefixInt(2, "Aruba", "+297");
                Formats._AddPrefixInt(2, "Isole Fær Øer", "+298");
                Formats._AddPrefixInt(2, "Groenlandia", "+299");
                // Zona 3: Europa
                Formats._AddPrefixInt(3, "Grecia", "+30");
                Formats._AddPrefixInt(3, "Paesi Bassi", "+31");
                Formats._AddPrefixInt(3, "Belgio", "+32");
                Formats._AddPrefixInt(3, "Francia", "+33");
                Formats._AddPrefixInt(3, "Spagna", "+34");
                Formats._AddPrefixInt(3, "Gibilterra", "+350");
                Formats._AddPrefixInt(3, "Portogallo", "+351");
                Formats._AddPrefixInt(3, "Lussemburgo", "+352");
                Formats._AddPrefixInt(3, "Irlanda", "+353");
                Formats._AddPrefixInt(3, "Islanda", "+354");
                Formats._AddPrefixInt(3, "Albania", "+355");
                Formats._AddPrefixInt(3, "Malta", "+356");
                Formats._AddPrefixInt(3, "Cipro", "+357");
                Formats._AddPrefixInt(3, "Finlandia", "+358");
                Formats._AddPrefixInt(3, "Bulgaria", "+359");
                Formats._AddPrefixInt(3, "Ungheria", "+36");
                Formats._AddPrefixInt(3, "era usato dalla Repubblica Democratica Tedesca. In tali regioni si usa adesso il codice +49 della Germania riunificata", "+37");
                Formats._AddPrefixInt(3, "Lituania", "+370");
                Formats._AddPrefixInt(3, "Lettonia", "+371");
                Formats._AddPrefixInt(3, "Estonia", "+372");
                Formats._AddPrefixInt(3, "Moldavia", "+373");
                Formats._AddPrefixInt(3, "Armenia", "+374");
                Formats._AddPrefixInt(3, "Bielorussia", "+375");
                Formats._AddPrefixInt(3, "Andorra", "+376");
                Formats._AddPrefixInt(3, "Principato di Monaco", "+377");
                Formats._AddPrefixInt(3, "San Marino", "+378");
                Formats._AddPrefixInt(3, "assegnato a Città del Vaticano, ma non attivato (usa +39 06 = Roma)", "+379");
                Formats._AddPrefixInt(3, "era usato dalla Jugoslavia", "+38");
                Formats._AddPrefixInt(3, "Ucraina", "+380");
                Formats._AddPrefixInt(3, "Serbia", "+381");
                Formats._AddPrefixInt(3, "Montenegro", "+382");
                Formats._AddPrefixInt(3, "Croazia", "+385");
                Formats._AddPrefixInt(3, "Slovenia", "+386");
                Formats._AddPrefixInt(3, "Bosnia ed Erzegovina", "+387");
                Formats._AddPrefixInt(3, "Spazio di numerazione telefonica europeo – Servizi Europei", "+388");
                Formats._AddPrefixInt(3, "Macedonia (F.Y.R.O.M.)", "+389");
                Formats._AddPrefixInt(3, "Italia", "+39");

                // Zona 4: Europa
                Formats._AddPrefixInt(4, "Romania", "+40");
                Formats._AddPrefixInt(4, "Svizzera", "+41");
                Formats._AddPrefixInt(4, "era usato dalla Cecoslovacchia", "+42");
                Formats._AddPrefixInt(4, "Repubblica Ceca", "+420");
                Formats._AddPrefixInt(4, "Slovacchia", "+421");
                Formats._AddPrefixInt(4, "Liechtenstein", "+423");
                Formats._AddPrefixInt(4, "Austria", "+43");
                Formats._AddPrefixInt(4, "Regno Unito", "+44");
                Formats._AddPrefixInt(4, "Danimarca", "+45");
                Formats._AddPrefixInt(4, "Svezia", "+46");
                Formats._AddPrefixInt(4, "Norvegia", "+47");
                Formats._AddPrefixInt(4, "Polonia", "+48");
                Formats._AddPrefixInt(4, "Germania", "+49");
                // Zona 5: Messico, America centrale e meridionale, Indie occidentali
                Formats._AddPrefixInt(5, "Isole Falkland", "+500");
                Formats._AddPrefixInt(5, "Belize", "+501");
                Formats._AddPrefixInt(5, "Guatemala", "+502");
                Formats._AddPrefixInt(5, "El Salvador", "+503");
                Formats._AddPrefixInt(5, "Honduras", "+504");
                Formats._AddPrefixInt(5, "Nicaragua", "+505");
                Formats._AddPrefixInt(5, "Costa Rica", "+506");
                Formats._AddPrefixInt(5, "Panamá", "+507");
                Formats._AddPrefixInt(5, "Saint Pierre e Miquelon", "+508");
                Formats._AddPrefixInt(5, "Haiti", "+509");
                Formats._AddPrefixInt(5, "Perù", "+51");
                Formats._AddPrefixInt(5, "Messico", "+52");
                Formats._AddPrefixInt(5, "Cuba", "+53");
                Formats._AddPrefixInt(5, "Argentina", "+54");
                Formats._AddPrefixInt(5, "Brasile", "+55");
                Formats._AddPrefixInt(5, "Cile", "+56");
                Formats._AddPrefixInt(5, "Colombia", "+57");
                Formats._AddPrefixInt(5, "Venezuela", "+58");
                Formats._AddPrefixInt(5, "Guadalupa", "+590");
                Formats._AddPrefixInt(5, "Bolivia", "+591");
                Formats._AddPrefixInt(5, "Guyana", "+592");
                Formats._AddPrefixInt(5, "Ecuador", "+593");
                Formats._AddPrefixInt(5, "Guyana Francese", "+594");
                Formats._AddPrefixInt(5, "Paraguay", "+595");
                Formats._AddPrefixInt(5, "Martinica", "+596");
                Formats._AddPrefixInt(5, "Suriname", "+597");
                Formats._AddPrefixInt(5, "Uruguay", "+598");
                Formats._AddPrefixInt(5, "era usato dalle Antille Olandesi ora divise in", "+599");
                Formats._AddPrefixInt(5, "Sint Eustatius", "+5993");
                Formats._AddPrefixInt(5, "Saba", "+5994");
                Formats._AddPrefixInt(5, "Sint Maarten", "+5995");
                Formats._AddPrefixInt(5, "Bonaire", "+5997");
                Formats._AddPrefixInt(5, "Curaçao", "+5999");
                // Zona 6: Oceano Pacifico meridionale e Oceania
                Formats._AddPrefixInt(6, "Malesia", "+60");
                Formats._AddPrefixInt(6, "Australia", "+61");
                Formats._AddPrefixInt(6, "Indonesia", "+62");
                Formats._AddPrefixInt(6, "Filippine", "+63");
                Formats._AddPrefixInt(6, "Nuova Zelanda", "+64");
                Formats._AddPrefixInt(6, "Singapore", "+65");
                Formats._AddPrefixInt(6, "Thailandia", "+66");
                Formats._AddPrefixInt(6, "Timor Est – era assegnato alle Isole Marianne Settentrionali, che ora usano il prefisso +1", "+670");
                Formats._AddPrefixInt(6, "era assegnato a Guam, che ora usa il prefisso +1", "+671");
                Formats._AddPrefixInt(6, "Territori Australiani Esterni: Antartide, Isola del Natale, Isole Cocos e Isola Norfolk", "+672");
                Formats._AddPrefixInt(6, "Brunei", "+673");
                Formats._AddPrefixInt(6, "Nauru", "+674");
                Formats._AddPrefixInt(6, "Papua Nuova Guinea", "+675");
                Formats._AddPrefixInt(6, "Tonga", "+676");
                Formats._AddPrefixInt(6, "Isole Salomone", "+677");
                Formats._AddPrefixInt(6, "Vanuatu", "+678");
                Formats._AddPrefixInt(6, "Figi", "+679");
                Formats._AddPrefixInt(6, "Palau", "+680");
                Formats._AddPrefixInt(6, "Wallis e Futuna", "+681");
                Formats._AddPrefixInt(6, "Isole Cook", "+682");
                Formats._AddPrefixInt(6, "Niue", "+683");
                Formats._AddPrefixInt(6, "Samoa Americane", "+684");
                Formats._AddPrefixInt(6, "Samoa", "+685");
                Formats._AddPrefixInt(6, "Kiribati, Isola Gilbert", "+686");
                Formats._AddPrefixInt(6, "Nuova Caledonia", "+687");
                Formats._AddPrefixInt(6, "Tuvalu, Isole Ellice", "+688");
                Formats._AddPrefixInt(6, "Polinesia Francese", "+689");
                Formats._AddPrefixInt(6, "Tokelau", "+690");
                Formats._AddPrefixInt(6, "Stati Federati di Micronesia", "+691");
                Formats._AddPrefixInt(6, "Isole Marshall", "+692");
                // Zona 7: Russia e Asia centrale (ex Unione Sovietica)
                Formats._AddPrefixInt(7, "Russia, Kazakistan", "+7");
                // Zona 8: Asia orientale e Servizi Speciali
                Formats._AddPrefixInt(8, "International Freephone", "+800");
                Formats._AddPrefixInt(8, "riservato per Shared Cost Services", "+808");
                Formats._AddPrefixInt(8, "Giappone", "+81");
                Formats._AddPrefixInt(8, "Corea del Sud", "+82");
                Formats._AddPrefixInt(8, "Vietnam", "+84");
                Formats._AddPrefixInt(8, "Corea del Nord", "+850");
                Formats._AddPrefixInt(8, "Hong Kong", "+852");
                Formats._AddPrefixInt(8, "Macao", "+853");
                Formats._AddPrefixInt(8, "Cambogia", "+855");
                Formats._AddPrefixInt(8, "Laos", "+856");
                Formats._AddPrefixInt(8, "Cina", "+86");
                Formats._AddPrefixInt(8, "Servizio Inmarsat 'SNAC'", "+870");
                Formats._AddPrefixInt(8, "riservato per il Servizio Mobile Marittimo", "+875");
                Formats._AddPrefixInt(8, "riservato per il Servizio Mobile Marittimo", "+876");
                Formats._AddPrefixInt(8, "riservato per il Servizio Mobile Marittimo", "+877");
                Formats._AddPrefixInt(8, "Universal Personal Telecommunications services", "+878");
                Formats._AddPrefixInt(8, "riservato per il Servizio Mobile Marittimo", "+879");
                Formats._AddPrefixInt(8, "Bangladesh", "+880");
                Formats._AddPrefixInt(8, "Mobile Satellite System", "+881");
                Formats._AddPrefixInt(8, "International Networks", "+882");
                Formats._AddPrefixInt(8, "Taiwan", "+886");
                // Zona 9: Asia occidentale e meridionale, Medio Oriente
                Formats._AddPrefixInt(9, "Turchia", "+90");
                Formats._AddPrefixInt(9, "India", "+91");
                Formats._AddPrefixInt(9, "Pakistan", "+92");
                Formats._AddPrefixInt(9, "Afghanistan", "+93");
                Formats._AddPrefixInt(9, "Sri Lanka", "+94");
                Formats._AddPrefixInt(9, "Myanmar", "+95");
                Formats._AddPrefixInt(9, "Maldive", "+960");
                Formats._AddPrefixInt(9, "Libano", "+961");
                Formats._AddPrefixInt(9, "Giordania", "+962");
                Formats._AddPrefixInt(9, "Siria", "+963");
                Formats._AddPrefixInt(9, "Iraq", "+964");
                Formats._AddPrefixInt(9, "Kuwait", "+965");
                Formats._AddPrefixInt(9, "Arabia Saudita", "+966");
                Formats._AddPrefixInt(9, "Yemen", "+967");
                Formats._AddPrefixInt(9, "Oman", "+968");
                Formats._AddPrefixInt(9, "era usato dalla Repubblica Democratica dello Yemen, ora unificata con lo Yemen (+967)", "+969");
                Formats._AddPrefixInt(9, "Palestina", "+970");
                Formats._AddPrefixInt(9, "Emirati Arabi Uniti", "+971");
                Formats._AddPrefixInt(9, "Israele", "+972");
                Formats._AddPrefixInt(9, "Bahrain", "+973");
                Formats._AddPrefixInt(9, "Qatar", "+974");
                Formats._AddPrefixInt(9, "Bhutan", "+975");
                Formats._AddPrefixInt(9, "Mongolia", "+976");
                Formats._AddPrefixInt(9, "Nepal", "+977");
                Formats._AddPrefixInt(9, "International Premium Rate Service", "+979");
                Formats._AddPrefixInt(9, "Iran", "+98");
                Formats._AddPrefixInt(9, "International Telecommunications Public Correspondence Service trial (ITPCS)", "+991");
                Formats._AddPrefixInt(9, "Tagikistan", "+992");
                Formats._AddPrefixInt(9, "Turkmenistan", "+993");
                Formats._AddPrefixInt(9, "Azerbaigian", "+994");
                Formats._AddPrefixInt(9, "Georgia", "+995");
                Formats._AddPrefixInt(9, "Kirghizistan", "+996");
                Formats._AddPrefixInt(9, "Uzbekistan", "+998");
                Formats._AddPrefix("Campioni d'Italia", "004191", "");
                Formats._AddPrefix("", "010", "");
                Formats._AddPrefix("", "011", "");
                Formats._AddPrefix("", "0121", "");
                Formats._AddPrefix("", "0122", "");
                Formats._AddPrefix("", "0123", "");
                Formats._AddPrefix("", "0124", "");
                Formats._AddPrefix("", "0125", "");
                Formats._AddPrefix("", "0131", "");
                Formats._AddPrefix("", "0141", "");
                Formats._AddPrefix("", "0142", "");
                Formats._AddPrefix("", "0143", "");
                Formats._AddPrefix("", "0144", "");
                Formats._AddPrefix("", "015", "");
                Formats._AddPrefix("", "0161", "");
                Formats._AddPrefix("", "0163", "");
                Formats._AddPrefix("", "0165", "");
                Formats._AddPrefix("", "0166", "");
                Formats._AddPrefix("", "0171", "");
                Formats._AddPrefix("", "0172", "");
                Formats._AddPrefix("", "0173", "");
                Formats._AddPrefix("", "0174", "");
                Formats._AddPrefix("", "0175", "");
                Formats._AddPrefix("", "0182", "");
                Formats._AddPrefix("", "0183", "");
                Formats._AddPrefix("", "0184", "");
                Formats._AddPrefix("", "0185", "");
                Formats._AddPrefix("", "0187", "");
                Formats._AddPrefix("", "019", "");
                Formats._AddPrefix("", "02", "");
                Formats._AddPrefix("", "030", "");
                Formats._AddPrefix("", "031", "");
                Formats._AddPrefix("", "0321", "");
                Formats._AddPrefix("", "0322", "");
                Formats._AddPrefix("", "0323", "");
                Formats._AddPrefix("", "0324", "");
                Formats._AddPrefix("", "0331", "");
                Formats._AddPrefix("", "0332", "");
                Formats._AddPrefix("", "0341", "");
                Formats._AddPrefix("", "0342", "");
                Formats._AddPrefix("", "0343", "");
                Formats._AddPrefix("", "0344", "");
                Formats._AddPrefix("", "0345", "");
                Formats._AddPrefix("", "0346", "");
                Formats._AddPrefix("", "035", "");
                Formats._AddPrefix("", "0362", "");
                Formats._AddPrefix("", "0363", "");
                Formats._AddPrefix("", "0364", "");
                Formats._AddPrefix("", "0365", "");
                Formats._AddPrefix("", "0371", "");
                Formats._AddPrefix("", "0372", "");
                Formats._AddPrefix("", "0373", "");
                Formats._AddPrefix("", "0374", "");
                Formats._AddPrefix("", "0375", "");
                Formats._AddPrefix("", "0376", "");
                Formats._AddPrefix("", "0377", "");
                Formats._AddPrefix("", "0381", "");
                Formats._AddPrefix("", "0382", "");
                Formats._AddPrefix("", "0383", "");
                Formats._AddPrefix("", "0384", "");
                Formats._AddPrefix("", "0385", "");
                Formats._AddPrefix("", "0386", "");
                Formats._AddPrefix("", "039", "");
                Formats._AddPrefix("", "040", "");
                Formats._AddPrefix("", "041", "");
                Formats._AddPrefix("", "0421", "");
                Formats._AddPrefix("", "0422", "");
                Formats._AddPrefix("", "0423", "");
                Formats._AddPrefix("", "0424", "");
                Formats._AddPrefix("", "0425", "");
                Formats._AddPrefix("", "0426", "");
                Formats._AddPrefix("", "0427", "");
                Formats._AddPrefix("", "0428", "");
                Formats._AddPrefix("", "0429", "");
                Formats._AddPrefix("", "0431", "");
                Formats._AddPrefix("", "0432", "");
                Formats._AddPrefix("", "0433", "");
                Formats._AddPrefix("", "0434", "");
                Formats._AddPrefix("", "0435", "");
                Formats._AddPrefix("", "0436", "");
                Formats._AddPrefix("", "0437", "");
                Formats._AddPrefix("", "0438", "");
                Formats._AddPrefix("", "0439", "");
                Formats._AddPrefix("", "0442", "");
                Formats._AddPrefix("", "0444", "");
                Formats._AddPrefix("", "0445", "");
                Formats._AddPrefix("", "045", "");
                Formats._AddPrefix("", "0461", "");
                Formats._AddPrefix("", "0462", "");
                Formats._AddPrefix("", "0463", "");
                Formats._AddPrefix("", "0464", "");
                Formats._AddPrefix("", "0465", "");
                Formats._AddPrefix("", "0471", "");
                Formats._AddPrefix("", "0472", "");
                Formats._AddPrefix("", "0473", "");
                Formats._AddPrefix("", "0474", "");
                Formats._AddPrefix("", "0481", "");
                Formats._AddPrefix("", "049", "");
                Formats._AddPrefix("", "050", "");
                Formats._AddPrefix("", "051", "");
                Formats._AddPrefix("", "0521", "");
                Formats._AddPrefix("", "0522", "");
                Formats._AddPrefix("", "0523", "");
                Formats._AddPrefix("", "0524", "");
                Formats._AddPrefix("", "0525", "");
                Formats._AddPrefix("", "0532", "");
                Formats._AddPrefix("", "0533", "");
                Formats._AddPrefix("", "0534", "");
                Formats._AddPrefix("", "0535", "");
                Formats._AddPrefix("", "0536", "");
                Formats._AddPrefix("", "0541", "");
                Formats._AddPrefix("", "0542", "");
                Formats._AddPrefix("", "0543", "");
                Formats._AddPrefix("", "0544", "");
                Formats._AddPrefix("", "0545", "");
                Formats._AddPrefix("", "0546", "");
                Formats._AddPrefix("", "0547", "");
                Formats._AddPrefix("", "055", "");
                Formats._AddPrefix("", "0564", "");
                Formats._AddPrefix("", "0565", "");
                Formats._AddPrefix("", "0566", "");
                Formats._AddPrefix("", "0571", "");
                Formats._AddPrefix("", "0572", "");
                Formats._AddPrefix("", "0573", "");
                Formats._AddPrefix("", "0574", "");
                Formats._AddPrefix("", "0575", "");
                Formats._AddPrefix("", "0577", "");
                Formats._AddPrefix("", "0578", "");
                Formats._AddPrefix("", "0583", "");
                Formats._AddPrefix("", "0584", "");
                Formats._AddPrefix("", "0585", "");
                Formats._AddPrefix("", "0586", "");
                Formats._AddPrefix("", "0587", "");
                Formats._AddPrefix("", "0588", "");
                Formats._AddPrefix("", "059", "");
                Formats._AddPrefix("", "06", "");
                Formats._AddPrefix("", "070", "");
                Formats._AddPrefix("", "071", "");
                Formats._AddPrefix("", "0721", "");
                Formats._AddPrefix("", "0722", "");
                Formats._AddPrefix("", "0731", "");
                Formats._AddPrefix("", "0732", "");
                Formats._AddPrefix("", "0733", "");
                Formats._AddPrefix("", "0734", "");
                Formats._AddPrefix("", "0735", "");
                Formats._AddPrefix("", "0736", "");
                Formats._AddPrefix("", "0737", "");
                Formats._AddPrefix("", "0742", "");
                Formats._AddPrefix("", "0743", "");
                Formats._AddPrefix("", "0744", "");
                Formats._AddPrefix("", "0746", "");
                Formats._AddPrefix("", "075", "");
                Formats._AddPrefix("", "0761", "");
                Formats._AddPrefix("", "0763", "");
                Formats._AddPrefix("", "0765", "");
                Formats._AddPrefix("", "0766", "");
                Formats._AddPrefix("", "0771", "");
                Formats._AddPrefix("", "0773", "");
                Formats._AddPrefix("", "0774", "");
                Formats._AddPrefix("", "0775", "");
                Formats._AddPrefix("", "0776", "");
                Formats._AddPrefix("", "0781", "");
                Formats._AddPrefix("", "0782", "");
                Formats._AddPrefix("", "0783", "");
                Formats._AddPrefix("", "0784", "");
                Formats._AddPrefix("", "0785", "");
                Formats._AddPrefix("", "0789", "");
                Formats._AddPrefix("", "079", "");
                Formats._AddPrefix("", "080", "");
                Formats._AddPrefix("", "081", "");
                Formats._AddPrefix("", "0823", "");
                Formats._AddPrefix("", "0824", "");
                Formats._AddPrefix("", "0825", "");
                Formats._AddPrefix("", "0827", "");
                Formats._AddPrefix("", "0828", "");
                Formats._AddPrefix("", "0831", "");
                Formats._AddPrefix("", "0832", "");
                Formats._AddPrefix("", "0833", "");
                Formats._AddPrefix("", "0835", "");
                Formats._AddPrefix("", "0836", "");
                Formats._AddPrefix("", "085", "");
                Formats._AddPrefix("", "0861", "");
                Formats._AddPrefix("", "0862", "");
                Formats._AddPrefix("", "0863", "");
                Formats._AddPrefix("", "0864", "");
                Formats._AddPrefix("", "0865", "");
                Formats._AddPrefix("", "0871", "");
                Formats._AddPrefix("", "0872", "");
                Formats._AddPrefix("", "0873", "");
                Formats._AddPrefix("", "0874", "");
                Formats._AddPrefix("", "0875", "");
                Formats._AddPrefix("", "0881", "");
                Formats._AddPrefix("", "0882", "");
                Formats._AddPrefix("", "0883", "");
                Formats._AddPrefix("", "0884", "");
                Formats._AddPrefix("", "0885", "");
                Formats._AddPrefix("", "089", "");
                Formats._AddPrefix("", "090", "");
                Formats._AddPrefix("", "091", "");
                Formats._AddPrefix("", "0921", "");
                Formats._AddPrefix("", "0922", "");
                Formats._AddPrefix("", "0923", "");
                Formats._AddPrefix("", "0924", "");
                Formats._AddPrefix("", "0925", "");
                Formats._AddPrefix("", "0931", "");
                Formats._AddPrefix("", "0932", "");
                Formats._AddPrefix("", "0933", "");
                Formats._AddPrefix("", "0934", "");
                Formats._AddPrefix("", "0935", "");
                Formats._AddPrefix("", "0941", "");
                Formats._AddPrefix("", "0942", "");
                Formats._AddPrefix("", "095", "");
                Formats._AddPrefix("", "0961", "");
                Formats._AddPrefix("", "0962", "");
                Formats._AddPrefix("", "0963", "");
                Formats._AddPrefix("", "0964", "");
                Formats._AddPrefix("", "0965", "");
                Formats._AddPrefix("", "0966", "");
                Formats._AddPrefix("", "0967", "");
                Formats._AddPrefix("", "0968", "");
                Formats._AddPrefix("", "0971", "");
                Formats._AddPrefix("", "0972", "");
                Formats._AddPrefix("", "0973", "");
                Formats._AddPrefix("", "0974", "");
                Formats._AddPrefix("", "0975", "");
                Formats._AddPrefix("", "0976", "");
                Formats._AddPrefix("", "0981", "");
                Formats._AddPrefix("", "0982", "");
                Formats._AddPrefix("", "0983", "");
                Formats._AddPrefix("", "0984", "");
                Formats._AddPrefix("", "0985", "");
                Formats._AddPrefix("", "099", "");
                Formats._AddPrefix("TIM", "330", "");
                Formats._AddPrefix("TIM", "331", "");
                Formats._AddPrefix("TIM", "333", "");
                Formats._AddPrefix("TIM", "334", "");
                Formats._AddPrefix("TIM", "335", "");
                Formats._AddPrefix("TIM", "336");
                Formats._AddPrefix("TIM", "337");
                Formats._AddPrefix("TIM", "338");
                Formats._AddPrefix("TIM", "339");
                Formats._AddPrefix("TIM", "360");
                Formats._AddPrefix("TIM", "361*");
                Formats._AddPrefix("TIM", "362*");
                Formats._AddPrefix("TIM", "363*");
                Formats._AddPrefix("TIM", "366");
                Formats._AddPrefix("TIM", "368");
                Formats._AddPrefix("Vodafone", "340");
                Formats._AddPrefix("Vodafone", "341*");
                Formats._AddPrefix("Vodafone", "342");
                Formats._AddPrefix("Vodafone", "343*");
                Formats._AddPrefix("Vodafone", "345");
                Formats._AddPrefix("Vodafone", "346");
                Formats._AddPrefix("Vodafone", "347");
                Formats._AddPrefix("Vodafone", "348");
                Formats._AddPrefix("Vodafone", "349");
                Formats._AddPrefix("Vodafone", "383*");
                Formats._AddPrefix("Wind", "320");
                Formats._AddPrefix("Wind", "322*");
                Formats._AddPrefix("Wind", "323*");
                Formats._AddPrefix("Wind", "324");
                Formats._AddPrefix("Wind", "327");
                Formats._AddPrefix("Wind", "328");
                Formats._AddPrefix("Wind", "329");
                Formats._AddPrefix("Wind", "380");
                Formats._AddPrefix("Wind", "383");
                Formats._AddPrefix("Wind", "388");
                Formats._AddPrefix("Wind", "389");
                Formats._AddPrefix("Tre", "390*");
                Formats._AddPrefix("Tre", "391");
                Formats._AddPrefix("Tre", "392");
                Formats._AddPrefix("Tre", "393");
                Formats._AddPrefix("Tre", "397*");
                Formats._AddPrefix("RFI", "313");
                // Formats._AddPrefix("A-Mobile (Wind)", "389")
                Formats._AddPrefix("BIP MOBILE (H3G)", "373");
                Formats._AddPrefix("BT Mobile (Vodafone)", "377");
                Formats._AddPrefix("CoopVoce (TIM)", "331");
                Formats._AddPrefix("CoopVoce (TIM)", "370");
                Formats._AddPrefix("CoopVoce (TIM)", "371");
                Formats._AddPrefix("CoopVoce (TIM)", "372");
                Formats._AddPrefix("CoopVoce (TIM)", "373");
                Formats._AddPrefix("Daily Telecom Mobile (Vodafone)", "377");
                Formats._AddPrefix("Daily Telecom Mobile (Vodafone)", "378");
                Formats._AddPrefix("ERG Mobile (Vodafone)", "375");
                Formats._AddPrefix("ERG Mobile (Vodafone)", "376");
                Formats._AddPrefix("ERG Mobile (Vodafone)", "377");
                // //Fastweb (Tre): 373
                // //Italia Mobile: n.d.
                // //MTV Mobile (TIM): 331, 366
                // //Noverca (TIM): 350-5, 370-7
                // //PlusCom (H3G): 373
                // //PosteMobile (Vodafone): 377-1, 377-2, 377-4, 377-9
                // //Tiscali Mobile (TIM): 370-1
                // //UNOMobile (Vodafone): 377-3
                // //Telogic (H3G): 373
                m_Prefissi.Comparer = new _prefixComparer();
                m_Prefissi.Sort();
            }

            private class _prefixComparer : IComparer
            {
                public int Compare(CPhonePrefix a, CPhonePrefix b)
                {
                    if (a.zona != 0 & b.zona == 0)
                        return -1;
                    if (a.zona == 0 & b.zona != 0)
                        return 1;
                    return b.per.Length - a.per.Length;
                }

                public int Compare(object x, object y)
                {
                    return Compare((CPhonePrefix)x, (CPhonePrefix)y);
                }
            }

            public string ParsePhoneNumber(string text)
            {
                int i;
                char ch;
                var ret = new System.Text.StringBuilder();
                text = DMD.Strings.Trim(text);
                var loopTo = DMD.Strings.Len(text);
                for (i = 1; i <= loopTo; i++)
                {
                    ch = text[i - 1];
                    if (Formats._validPhoneNumberChars.IndexOf(ch) >= 0)
                        ret.Append(ch);
                }

                text = ret.ToString();
                if (DMD.Strings.Left(text, 2) == "00")
                    text = "+" + DMD.Strings.Mid(text, 3);
                return text;
            }

            public string FormatPhoneNumber(string text)
            {
                // Dim i As Integer
                // Dim ret As String
                // Dim p As Integer
                // text = Formats.ParsePhoneNumber(text)
                // If (text = vbNullString) Then Return vbNullString

                // If (Formats._phPrefx Is Nothing) Then Formats._InitPhonePrefixes()
                // p = 1
                // ret = ""
                // For i = 0 To Formats._phPrefx.Count - 1
                // With Formats._phPrefx(i)
                // If (Mid(text, p, .per.Length) = .per) Then
                // ret &= .per & " "
                // p += .per.Length
                // End If
                // End With
                // Next
                // ret &= Mid(text, p)
                // Return ret
                text = ParsePhoneNumber(text);
                var i = GetInfoNumero(text);
                return i.ToString();
            }

            /// <summary>
        /// Formatta un intervallo temporale misurato in secondi in una stringa che mostra gg hh mm ss
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string FormatDurata(long? seconds)
            {
                if (seconds.HasValue == false)
                    return "";
                int g, h, m, s;
                s = (int)(seconds % 60);
                seconds = (long?)Maths.Floor(seconds.Value / 60d);
                m = (int)(seconds % 60);
                seconds = (long?)Maths.Floor(seconds.Value / 60d);
                h = (int)(seconds % 60);
                g = (int)Maths.Floor(seconds.Value / 60d);
                string ret = "";
                if (g > 0)
                    ret = DMD.Strings.Combine(ret, g + " gg", " ");
                if (h > 0)
                    ret = DMD.Strings.Combine(ret, h + " h", " ");
                if (m > 0)
                    ret = DMD.Strings.Combine(ret, m + " m", " ");
                if (s > 0)
                    ret = DMD.Strings.Combine(ret, s + " s", " ");
                return ret;
            }

            public bool IsEMailAddress(string value)
            {
                return !string.IsNullOrEmpty(ParseEMailAddress(value));
            }

            public string ParseEMailAddress(string value)
            {
                var m = new System.Net.Mail.MailAddress(value);
                return m.Address;
            }

            public string FormatEMailAddress(string value)
            {
                return ParseEMailAddress(value);
            }

            public string ParseWebAddress(string value)
            {
                string ret = DMD.Strings.LCase(DMD.Strings.Replace(value, " ", ""));
                if (string.IsNullOrEmpty(ret))
                    return "";
                if (DMD.Strings.Left(ret, 7) == "http://")
                {
                    return ret;
                }
                else if (DMD.Strings.Left(ret, 8) == "https://")
                {
                    return ret;
                }
                else if (DMD.Strings.Left(ret, 6) == "ftp://")
                {
                    return ret;
                }
                else if (DMD.Strings.Left(ret, 7) == "ftps://")
                {
                    return ret;
                }
                else if (DMD.Strings.Left(ret, 4) == "ftp.")
                {
                    return "ftp://" + ret;
                }
                else
                {
                    return "http://" + ret;
                }
            }

            public string FormatWebAddress(string value)
            {
                string ret = DMD.Strings.LCase(DMD.Strings.Replace(value, " ", ""));
                if (string.IsNullOrEmpty(ret))
                    return "";
                if (DMD.Strings.Left(ret, 7) == "http://")
                {
                    return ret;
                }
                else if (DMD.Strings.Left(ret, 8) == "https://")
                {
                    return ret;
                }
                else if (DMD.Strings.Left(ret, 6) == "ftp://")
                {
                    return ret;
                }
                else if (DMD.Strings.Left(ret, 7) == "ftps://")
                {
                    return ret;
                }
                else if (DMD.Strings.Left(ret, 4) == "ftp.")
                {
                    return "ftp://" + ret;
                }
                else
                {
                    return "http://" + ret;
                }
            }

            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            public sealed class CUSAClass
            {
                private System.Globalization.CultureInfo m_CultureInfo;

                internal CUSAClass(string culture)
                {
                    m_CultureInfo = new System.Globalization.CultureInfo(culture);
                }

                public float ParseSingle(string value)
                {
                    return float.Parse(value, m_CultureInfo);
                }

                public string FormatSingle(float value)
                {
                    return value.ToString(m_CultureInfo);
                }

                public double? ParseDouble(string value)
                {
                    value = DMD.Strings.Trim(value);
                    if (string.IsNullOrEmpty(value))
                        return default;
                    return double.Parse(value, m_CultureInfo);
                }

                public string FormatDouble(object value)
                {
                    try
                    {
                        return DMD.Doubles.CDbl(value).ToString(m_CultureInfo);
                    }
                    catch (Exception)
                    {
                        return "";
                    }
                }

                public string FormatNumber(object value)
                {
                    return FormatDouble(value);
                }

                public string FormatNumber(object value, int decimals)
                {
                    try
                    {
                        System.Globalization.CultureInfo tmp = (System.Globalization.CultureInfo)m_CultureInfo.Clone();
                        tmp.NumberFormat.NumberDecimalDigits = decimals;
                        return DMD.Doubles.CDbl(value).ToString(tmp);
                    }
                    catch (Exception)
                    {
                        return "";
                    }
                }

                public double ToDouble(string value)
                {
                    try
                    {
                        return double.Parse(value, m_CultureInfo);
                    }
                    catch (Exception)
                    {
                        return 0d;
                    }
                }

                public DateTime? ParseDate(string str)
                {
                    str = DMD.Strings.Trim(str);
                    if (string.IsNullOrEmpty(str))
                        return default;
                    return DateTime.Parse(str, m_CultureInfo);
                }
            }

            private CUSAClass m_USA = null;

            public CUSAClass USA
            {
                get
                {
                    if (m_USA is null)
                        m_USA = new CUSAClass("en-US");
                    return m_USA;
                }
            }


            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            /// <summary>
        /// Restituisce una stringa nel formato YYYYMMDDHHnnssmmm"
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public string GetTimeStamp()
            {
                lock (this)
                {
                    var d = DMD.DateUtils.Now();
                    return DMD.Strings.Right("0000" + d.Year, 4) + DMD.Strings.Right("00" + d.Month, 2) + DMD.Strings.Right("00" + d.Day, 2) + DMD.Strings.Right("00" + d.Hour, 2) + DMD.Strings.Right("00" + d.Minute, 2) + DMD.Strings.Right("00" + d.Second, 2) + DMD.Strings.Right("000" + d.Millisecond, 3);
                }
            }

            ~CFormatsClass()
            {
                //DMDObject.DecreaseCounter(this);
            }
        }

        private static CFormatsClass m_Formats = null;

        public static CFormatsClass Formats
        {
            get
            {
                if (m_Formats is null)
                    m_Formats = new CFormatsClass();
                return m_Formats;
            }
        }
    }
}