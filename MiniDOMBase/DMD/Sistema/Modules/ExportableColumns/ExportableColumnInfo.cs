using System;
using DMD;
using DMD.XML;

namespace minidom
{

   

    /// <summary>
    /// Rappresenta una colonna esportabile
    /// </summary>
    [Serializable]
    public class ExportableColumnInfo 
        : IDMDXMLSerializable, IComparable
    {
        /// <summary>
        /// Nome della colonna
        /// </summary>
        public string Key;

        /// <summary>
        /// 
        /// </summary>
        public string Value;

        /// <summary>
        /// Posizione della colonna
        /// </summary>
        public int Posizione;

        /// <summary>
        /// Tipo dei valori della colonna
        /// </summary>
        public TypeCode TipoValore;

        /// <summary>
        /// Se true indica che la colonna è inclusa nell'esportazione
        /// </summary>
        public bool Selected;

        /// <summary>
        /// Costruttore
        /// </summary>
        public ExportableColumnInfo()
        {
            //DMDObject.IncreaseCounter(this);
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="descrizione"></param>
        /// <param name="tipoValore"></param>
        /// <param name="selected"></param>
        public ExportableColumnInfo(string nome, string descrizione, TypeCode tipoValore, bool selected = true) : this()
        {
            Key = nome;
            Value = descrizione;
            TipoValore = tipoValore;
            Selected = selected;
        }

        /// <summary>
        /// Restituisce una stringa che rappresenta l'oggetto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DMD.Strings.ConcatArray( Key , "/" , Value);
        }

        /// <summary>
        /// Restituisce il codice hash dell'oggetto
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCalculator.Calculate(this.Key, this.Value);
        }

        /// <summary>
        /// Restituisce true se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public sealed override bool Equals(object obj)
        {
            return (obj is ExportableColumnInfo) && this.Equals((ExportableColumnInfo)obj);
        }

        /// <summary>
        /// Restituisce true se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Equals(ExportableColumnInfo obj)
        {
            return base.Equals(obj)
                && DMD.Strings.EQ(this.Key, obj.Key)
                && DMD.Strings.EQ(this.Value, obj.Value)
                && object.ReferenceEquals(this.TipoValore, obj.TipoValore)
                && DMD.Booleans.EQ(this.Selected, obj.Selected)
                ;
        }

        /// <summary>
        /// Deserializzazione xml
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Key":
                    {
                        Key = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Value":
                    {
                        Value = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Posizione":
                    {
                        Posizione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "TipoValore":
                    {
                        TipoValore = (TypeCode)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "Selected":
                    {
                        Selected = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                        break;
                    }
            }
        }

        /// <summary>
        /// Serializzazione xml
        /// </summary>
        /// <param name="writer"></param>
        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            writer.WriteTag("Key", Key);
            writer.WriteTag("Value", Value);
            writer.WriteTag("Posizione", Posizione);
            writer.WriteTag("TipoValore", (int?)TipoValore);
            writer.WriteTag("Selected", Selected);
        }

        /// <summary>
        /// Compara le colonne
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(ExportableColumnInfo obj)
        {
            int ret = DMD.Integers.Compare(this.Posizione , obj.Posizione);
            if (ret == 0) ret = DMD.Strings.Compare(Value, obj.Value, true);
            return ret;
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo((ExportableColumnInfo)obj);
        }

        
    }

    
}