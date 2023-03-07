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

    /// <summary>
    /// Oggetto anomalo
    /// </summary>
    [Serializable]
    public class OggettoAnomalo 
        : IComparable, IDMDXMLSerializable, IComparable<OggettoAnomalo>
    {
        /// <summary>
        /// Oggetto
        /// </summary>
        [NonSerialized] public object Oggetto;

        /// <summary>
        /// Nome del gruppo di anomalie
        /// </summary>
        public string Gruppo;

        /// <summary>
        /// Elenco delle anomalie
        /// </summary>
        public CCollection<Anomalia> Anomalie = new CCollection<Anomalia>();

        /// <summary>
        /// Costruttore
        /// </summary>
        public OggettoAnomalo()
        {
            //DMDObject.IncreaseCounter(this);
        }

        /// <summary>
        /// Aggiunge un'anomalia
        /// </summary>
        /// <param name="descrizione"></param>
        /// <param name="importanza"></param>
        /// <returns></returns>
        public Anomalia AggiungiAnomalia(string descrizione, int importanza)
        {
            Anomalia a;
            foreach (var currentA in Anomalie)
            {
                a = currentA;
                if ((a.Descrizione ?? "") == (descrizione ?? ""))
                {
                    a.Importanza = Maths.Min(a.Importanza, importanza);
                    return a;
                }
            }

            a = new Anomalia();
            a.Oggetto = Oggetto;
            a.Descrizione = descrizione;
            a.Importanza = importanza;
            Anomalie.Add(a);
            return a;
        }

        /// <summary>
        /// Compara i due oggetti
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int CompareTo(OggettoAnomalo obj)
        {
            return DMD.Strings.Compare(Gruppo, obj.Gruppo, true);
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo((OggettoAnomalo)obj);
        }

        /// <summary>
        /// Deserializzazione xml
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        protected virtual void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Gruppo":
                    {
                        Gruppo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Oggetto":
                    {
                        Oggetto = DMD.XML.Utils.Serializer.ToObject(fieldValue);
                        break;
                    }

                case "Anomalie":
                    {
                        Anomalie.Clear();
                        Anomalie.AddRange((IEnumerable)fieldValue);
                        break;
                    }
            }
        }

        /// <summary>
        /// Serializzazione xml
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Gruppo", Gruppo);
            writer.WriteTag("Oggetto", Oggetto);
            writer.WriteTag("Anomalie", Anomalie);
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            this.XMLSerialize(writer);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            this.SetFieldInternal(fieldName, fieldValue);
        }

        
    }

     
}