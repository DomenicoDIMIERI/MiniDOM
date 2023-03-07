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
    public partial class Office
    {

        /// <summary>
        /// Regola base
        /// </summary>
        [Serializable]
        public abstract class MailRuleBase 
            : DMD.XML.DMDBaseXMLObject, IComparable, IComparable<MailRuleBase>
        {


            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se la regola è attiva
            /// </summary>
            /// <remarks></remarks>
            public bool Attiva;

            /// <summary>
            /// Restituisce o imposta l'ordine di applicazione della regola
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int Priorita { get; set; }

            /// <summary>
            /// Nome della regola
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Costruttore
            /// </summary>
            public MailRuleBase()
            {
                Attiva = false;
                Priorita = 0;
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(MailRuleBase obj)
            {
                return Priorita.CompareTo(obj.Priorita);
            }

            int IComparable.CompareTo(object obj)
            {
                return this.CompareTo((MailRuleBase) obj);
            }

            /// <summary>
            /// Restituisce vero se la regola si applica al messaaggio
            /// </summary>
            /// <param name="m"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public abstract bool Check(MailMessage m);

            /// <summary>
            /// Applica la regola al messaggio
            /// </summary>
            /// <param name="m"></param>
            /// <remarks></remarks>
            public abstract void Execute(MailMessage m);

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Priorita": Priorita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue); break;
                    case "Attiva": Attiva = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true; break;
                    default: base.SetFieldInternal(fieldName, fieldValue); break;
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Attiva", Attiva);
                writer.WriteAttribute("Priorita", Priorita);
                base.XMLSerialize(writer);
            }

            /// <summary>
            /// Restituisce una sringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.Name;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.Name);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(DMDBaseXMLObject obj)
            {
                return (obj is MailRuleBase) && this.Equals((MailRuleBase)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(MailRuleBase obj)
            {
                return base.Equals(obj)
                    && DMD.Booleans.EQ(this.Attiva, obj.Attiva)
                    && DMD.Integers.EQ(this.Priorita, obj.Priorita)
                    && DMD.Strings.EQ(this.Name, obj.Name)
                    ;
            }


        }
    }
}