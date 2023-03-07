using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Messenger
    {

        /// <summary>
        /// Utente appartenente ad una chat
        /// </summary>
        [Serializable]
        public class CChatUser 
            : DMD.XML.DMDBaseXMLObject, IComparable, IComparable<CChatUser>
        {
            /// <summary>
            /// ID dell'utente di sistema
            /// </summary>
            public int uID;

            /// <summary>
            /// Icona associata
            /// </summary>
            public string IconURL;

            /// <summary>
            /// Nome dell'utente di sistema
            /// </summary>
            public string UserName;

            /// <summary>
            /// Nome visibile dell'utente della chat
            /// </summary>
            public string DisplayName;

            /// <summary>
            /// Restituisce o imposta un valore boolano che indica se l'utente é online
            /// </summary>
            public bool IsOnline;

            /// <summary>
            /// Ultima data di accesso
            /// </summary>
            public DateTime? UltimoAccesso;

            /// <summary>
            /// Numero di messaggi non letti
            /// </summary>
            public int MessaggiNonLetti;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CChatUser()
            {
                uID = 0;
                IconURL = "";
                UserName = "";
                DisplayName = "";
                IsOnline = false;
                UltimoAccesso = default;
                MessaggiNonLetti = 0;
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("UserName", UserName);
                writer.WriteAttribute("DisplayName", DisplayName);
                writer.WriteAttribute("IsOnline", IsOnline);
                writer.WriteAttribute("uID", uID);
                writer.WriteAttribute("IconURL", IconURL);
                writer.WriteAttribute("UltimoAccesso", UltimoAccesso);
                writer.WriteAttribute("MessaggiNonLetti", MessaggiNonLetti);
                base.XMLSerialize(writer);
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
                    case "uID":
                        {
                            uID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "UserName":
                        {
                            UserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DisplayName":
                        {
                            DisplayName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IsOnline":
                        {
                            IsOnline = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "IconURL":
                        {
                            IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "UltimoAccesso":
                        {
                            UltimoAccesso = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "MessaggiNonLetti":
                        {
                            MessaggiNonLetti = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    default:
                        base.SetFieldInternal(fieldName, fieldValue);
                        break;
                }
            }


            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.UserName;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.UserName);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(DMDBaseXMLObject obj)
            {
                return (obj is CChatRoomUser) && this.Equals((CChatRoomUser)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Euqlas(CChatRoomUser obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.uID, obj.uID)
                    && DMD.Strings.EQ(this.IconURL, obj.IconURL)
                    && DMD.Strings.EQ(this.UserName, obj.UserName)
                    && DMD.Strings.EQ(this.DisplayName, obj.DisplayName)
                    && DMD.Booleans.EQ(this.IsOnline, obj.IsOnline)
                    && DMD.DateUtils.EQ(this.UltimoAccesso, obj.UltimoAccesso)
                    && DMD.Integers.EQ(this.MessaggiNonLetti, obj.MessaggiNonLetti)
                    ;
            }

            /// <summary>
            /// Compara due oggetti in base al nome utente
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(CChatUser other)
            {
                return Strings.Compare(this.UserName, other.UserName, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return this.CompareTo((CChatRoomUser)obj);
            }
        }
    }
}