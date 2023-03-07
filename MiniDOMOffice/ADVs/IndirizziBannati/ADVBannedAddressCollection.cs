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
    public partial class ADV
    {

        /// <summary>
        /// Collezione degli indirizzi bannati
        /// </summary>
        [Serializable]
        public sealed class ADVBannedAddressCollection 
            : CKeyCollection<string>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public ADVBannedAddressCollection()
            {
            }

            /// <summary>
            /// Aggiunge un indirizzo bannato
            /// </summary>
            /// <param name="bannedAddress"></param>
            /// <returns></returns>
            public new string Add(string bannedAddress) // System.Text.RegularExpressions.Regex
            {
                // Dim item As New System.Text.RegularExpressions.Regex(bannedAddress)
                string item = bannedAddress;
                Add(bannedAddress, item);
                return item;
            }

            /// <summary>
            /// Restitusice vero se l'indirizzo o il numero è "bannato" cioè se il test con una delle espressioni contenute in questo elenco è vero
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool IsBanned(string value)
            {
                foreach (string reg in this)
                {
                    if (DMD.Strings.Like(value, reg))
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
                writer.WriteTag("keys", Keys);
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
                    case "keys":
                        {
                            Clear();
                            string[] keys = (string[])DMD.Arrays.Convert<string>(fieldValue);
                            if (keys is object)
                            {
                                foreach (string s in keys)
                                {
                                    if (!string.IsNullOrEmpty(Strings.Trim(s)))
                                        Add(s);
                                }
                            }

                            break;
                        }
                }
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <param name="fieldName"></param>
            internal void Read(DBReader reader, string fieldName)
            {
                Clear();
                string str = "";
                str = reader.Read(fieldName, str);
                str = Strings.Trim(str);
                if (!string.IsNullOrEmpty(str))
                {
                    foreach (string s in Strings.Split(str, ";"))
                    {
                        if (!string.IsNullOrEmpty(Strings.Trim(s)))
                            Add(s);
                    }
                }
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <param name="fieldName"></param>
            internal void Write(DBWriter writer, string fieldName)
            {
                var buffer = new System.Text.StringBuilder();
                foreach (string str in Keys)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (buffer.Length > 0)
                            buffer.Append(";");
                        buffer.Append(str);
                    }
                }

                writer.Write(fieldName, buffer.ToString());
            }
        }
    }
}