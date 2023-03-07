using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Statistiche del calendario
        /// </summary>
        [Serializable]
        public class CCalendarStats 
            : IDMDXMLSerializable
        {
            /// <summary>
            /// Conteggio delle attività previste
            /// </summary>
            public int Previste;

            /// <summary>
            /// Conteggio delle attività effettuate
            /// </summary>
            public int Effettuate;

            /// <summary>
            /// Conteggio delle nuove attività programmate
            /// </summary>
            public int Ricevute;
            // Public PersoneAttive As CCollection

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCalendarStats()
            {
                //DMDObject.IncreaseCounter(this);
                Previste = 0;
                Effettuate = 0;
                Ricevute = 0;
                // Me.PersoneAttive = New CCollection
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Previste", Previste);
                writer.WriteAttribute("Effettuate", Effettuate);
                writer.WriteAttribute("Ricevute", Ricevute);
                // writer.BeginTag("PersoneAttive")
                // If Me.PersoneAttive.Count > 0 Then
                // writer.Write(Me.PersoneAttive.ToArray, "CActivePerson")
                // End If
                // writer.EndTag()
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Previste":
                        {
                            Previste = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Effettuate":
                        {
                            Effettuate = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Ricevute":
                        {
                            Ricevute = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                        // Case "PersoneAttive"
                        // Me.PersoneAttive.Clear()
                        // If TypeName(fieldValue) <> "string" Then
                        // If Not IsArray(fieldValue) Then fieldValue = {fieldValue}
                        // Call Me.PersoneAttive.AddRange(fieldValue)
                        // End If
                }
            }

            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
            {
                this.XMLSerialize(writer);
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
            {
                this.SetFieldInternal(fieldName, fieldValue);
            }

            //~CCalendarStats()
            //{
            //    //DMDObject.DecreaseCounter(this);
            //}
        }
    }
}