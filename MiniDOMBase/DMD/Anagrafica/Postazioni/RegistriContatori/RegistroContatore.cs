using System;
using DMD.XML;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Definisce un contatore valido per una postazione (es. il numero di copie di una stampante)
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class RegistroContatore 
            : IDMDXMLSerializable
        {

            /// <summary>
            /// Nome del registro
            /// </summary>
            public string Nome;

            /// <summary>
            /// Restituisce o imposta un valore che identifica il tipo del valore dei registri
            /// </summary>
            public TypeCode ValueType;

            /// <summary>
            /// Limite minimo dei valori validi per il registro
            /// </summary>
            public double? LimiteMin;

            /// <summary>
            /// Limite massimo dei valori validi assumibili dal registro
            /// </summary>
            public double? LimiteMax;

            /// <summary>
            /// Numero di posizioni decimali valide per i valori assumibili dal registro
            /// </summary>
            public int Decimali;

            /// <summary>
            /// Flags
            /// </summary>
            public int Flags;

            /// <summary>
            /// Parametri aggiuntivi
            /// </summary>
            public CKeyCollection Parameters;

            /// <summary>
            /// Costruttore
            /// </summary>
            public RegistroContatore()
            {
                this.Nome = "";
                this.LimiteMin = default;
                this.LimiteMax = default;
                this.Decimali = 0;
                this.Flags = 0;
                this.ValueType = TypeCode.Double;
                this.Parameters = new CKeyCollection();
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            public void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", Nome);
                writer.WriteAttribute("LimiteMin", LimiteMin);
                writer.WriteAttribute("LimiteMax", LimiteMax);
                writer.WriteAttribute("Decimali", Decimali);
                writer.WriteAttribute("Flags", Flags);
                writer.WriteAttribute("ValueType", this.ValueType);
                writer.WriteTag("Parameters", this.Parameters);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            public void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Nome":
                        {
                            Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "LimiteMin":
                        {
                            LimiteMin = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "LimiteMax":
                        {
                            LimiteMax = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Decimali":
                        {
                            Decimali = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            Flags = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    case "Parameters":
                        {
                            this.Parameters = (CKeyCollection)fieldValue;
                            break;
                        }
                    case "ValueType":
                        {
                            this.ValueType = (TypeCode)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
                return this.Nome;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.Nome, this.ValueType);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return (obj is RegistroContatore) && this.Equals((RegistroContatore)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(RegistroContatore obj)
            {
                return DMD.Strings.EQ(this.Nome, obj.Nome)
                           && DMD.Doubles.Equals(this.LimiteMin, obj.LimiteMin)
                           && DMD.Doubles.Equals(this.LimiteMax, obj.LimiteMax)
                           && DMD.Integers.Equals(this.Decimali, obj.Decimali)
                           && DMD.Integers.Equals(this.Flags, obj.Flags)
                           && DMD.Integers.Equals((int)this.ValueType, (int)obj.ValueType)
                        ;
                //this.Parameters = new CKeyCollection();
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
            {
                this.SetFieldInternal(fieldName, fieldValue);
            }

            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
            {
                this.XMLSerialize(writer);
            }
        }
    }
}