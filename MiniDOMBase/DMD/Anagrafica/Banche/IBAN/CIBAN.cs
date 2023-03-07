using System;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using static minidom.Sistema;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Classe che rappresenta un codice IBAN
        /// </summary>
        /// <remarks></remarks>
        public sealed class CIBAN
            : DMD.XML.DMDBaseXMLObject
            //: Databases.DBObjectBase
        {

            /// <summary>
            /// Caratteri validi in un codice IBAN
            /// </summary>
            public const string ValidDigits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZZ";

            private string m_CodiceNazione;
            private string m_CodiceDiControllo;
            private string m_BBAN;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CIBAN()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="codice"></param>
            public CIBAN(string codice) : this()
            {
                IBAN = codice;
                SetChanged(false);
            }

            /// <summary>
            /// Restituisce o imposta il codice di nazione
            /// </summary>
            public string CodiceNazione
            {
                get
                {
                    return m_CodiceNazione;
                }

                set
                {
                    value = DMD.Strings.UCase(DMD.Strings.Left(DMD.Strings.Trim(value), 2));
                    string oldValue = m_CodiceNazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceNazione = value;
                    this.doPropChanged("CodiceNazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice di controllo
            /// </summary>
            public string CodiceDiControllo
            {
                get
                {
                    return m_CodiceDiControllo;
                }

                set
                {
                    value = DMD.Strings.UCase(DMD.Strings.Left(DMD.Strings.Trim(value), 2));
                    string oldValue = m_CodiceDiControllo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceDiControllo = value;
                    this.doPropChanged("CodiceDiControllo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce il codice BBAN
            /// </summary>
            public string BBAN
            {
                get
                {
                    return m_BBAN;
                }

                set
                {
                    value = DMD.Strings.UCase(DMD.Strings.Trim(value));
                    string oldValue = m_BBAN;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_BBAN = value;
                    this.doPropChanged("BBAN", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice IBAN rappresentato dall'oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string IBAN
            {
                get
                {
                    return m_CodiceNazione + m_CodiceDiControllo + m_BBAN;
                }

                set
                {
                    value = DMD.Strings.UCase(DMD.Strings.Replace(value, " ", ""));
                    string oldValue = IBAN;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceNazione = DMD.Strings.Left(value, 2);
                    m_CodiceDiControllo = DMD.Strings.Mid(value, 3, 2);
                    m_BBAN = DMD.Strings.Mid(value, 5);
                    this.doPropChanged("IBAN", value, oldValue);
                }
            }

            
            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.IBAN;
            }


            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_CodiceNazione, this.m_BBAN, this.m_CodiceDiControllo);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(object obj)
            {
                return (obj is CIBAN) && this.Equals((CIBAN)obj);
            }

            /// <summary>
            /// Restituisce true se gli oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public bool Equals(CIBAN obj)
            {
                return base.Equals(obj)
                        && DMD.Strings.EQ(this.m_CodiceNazione, obj.m_CodiceDiControllo)
                        && DMD.Strings.EQ(this.m_CodiceDiControllo, obj.m_CodiceDiControllo)
                        && DMD.Strings.EQ(this.m_BBAN, obj.m_BBAN)
                        ;
            }

            /// <summary>
            /// Restituisce vero se il codice rappresentato è valido
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool IsValid()
            {
                string text = IBAN;
                int i, j;
                string ch;
                string numStr;
                int r;
                if (DMD.Strings.Len(text) < 5)
                    return false;
                text = DMD.Strings.Mid(text, 5) + DMD.Strings.Left(text, 4);
                numStr = "";
                var loopTo = DMD.Strings.Len(text);
                for (i = 1; i <= loopTo; i++)
                {
                    ch = DMD.Strings.Mid(text, i, 1);
                    j = DMD.Strings.InStr(ValidDigits, ch) - 1;
                    if (j < 0)
                        return false;
                    numStr += j.ToString();
                }

                var nibbles = new List<string>();
                i = DMD.Strings.Len(numStr) - 8;
                while (!string.IsNullOrEmpty(numStr))
                {
                    if (DMD.Strings.Len(numStr) > 8)
                    {
                        nibbles.Add(DMD.Strings.Mid(numStr, DMD.Strings.Len(numStr) - 7, 8));
                        numStr = DMD.Strings.Left(numStr, DMD.Strings.Len(numStr) - 8);
                    }
                    else
                    {
                        nibbles.Add(numStr);
                        numStr = "";
                    }
                }

                r = 0;
                for (i = nibbles.Count - 1; i >= 0; i -= 1)
                    r = (int)(DMD.Longs.CLng(r.ToString() + DMD.Strings.CStr(nibbles[i])) % 97L);
                return r % 97 == 1;
            }

            ////protected internal override Databases.CDBConnection GetConnection()
            ////{
            ////    return Databases.APPConn;
            ////}

            //public override CModulesClass GetModule()
            //{
            //    return minidom.Anagrafica.IBAN;
            //}

            ///// <summary>
            ///// Restituisce il dirscirminante
            ///// </summary>
            ///// <returns></returns>
            //public override string GetTableName()
            //{
            //    return "tbl_IBANCodes";
            //}

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("CodiceNazione", this.m_CodiceNazione);
                writer.WriteAttribute("CodiceDiControllo", this.m_CodiceDiControllo);
                writer.WriteAttribute("BBAN", this.m_BBAN);
                base.XMLSerialize(writer);
            }

            
            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName)
                {
                    case "CodiceNazione": this.m_CodiceNazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "CodiceDiControllo": this.m_CodiceDiControllo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "BBAN": this.m_BBAN = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    default: base.SetFieldInternal(fieldName, fieldValue); break;
                }
            }
        }
    }
}