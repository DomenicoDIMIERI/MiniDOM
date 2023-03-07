using System;
using DMD;
using DMD.XML;

namespace minidom.PBX
{

    /// <summary>
    /// Dispositivo esterno (es un telefono Cisco IP)
    /// </summary>
    [Serializable]
    public class DispositivoEsterno 
        : DMD.XML.DMDBaseXMLObject, IComparable, IComparable<DispositivoEsterno>
    {
        /// <summary>
        /// Nome del dispositivo
        /// </summary>
        public string Nome;

        /// <summary>
        /// Indirizzo ip del dispositivo
        /// </summary>
        public string Indirizzo;

        /// <summary>
        /// Tipologia del dispositivo (es. Cisco IP 4510)
        /// </summary>
        public string Tipo;

        /// <summary>
        /// Costruttore
        /// </summary>
        public DispositivoEsterno()
        {
            Nome = "";
            Indirizzo = "";
            Tipo = "";
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="indirizzo"></param>
        /// <param name="tipo"></param>
        public DispositivoEsterno(string nome, string indirizzo, string tipo) : this()
        {
            Nome = nome;
            Indirizzo = indirizzo;
            Tipo = tipo;
        }

        /// <summary>
        /// Restituisce true se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Equals(DispositivoEsterno obj)
        {
            return base.Equals(obj)
                && DMD.Strings.EQ(this.Nome, obj.Nome)
                && DMD.Strings.EQ(this.Indirizzo, obj.Indirizzo)
                && DMD.Strings.EQ(this.Tipo, obj.Tipo)
                ;
        }

        /// <summary>
        /// Restituisce true se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public sealed override bool Equals(DMDBaseXMLObject obj)
        {
            return (obj is DispositivoEsterno) && this.Equals((DispositivoEsterno)obj);
        }

        /// <summary>
        /// Restituisce una stringa che rappresenta l'oggetto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DMD.Strings.ConcatArray(this.Nome , " (" , this.Tipo , ")");
        }

        /// <summary>
        /// Restituisce il codice hash dell'oggetto
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCalculator.Calculate(this.Indirizzo);
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
                case "Nome":
                    {
                        Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Indirizzo":
                    {
                        Indirizzo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Tipo":
                    {
                        Tipo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }
                default:
                    base.SetFieldInternal(fieldName, fieldValue); break;
            }
        }

        /// <summary>
        /// Serializzazione xml
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Nome", Nome);
            writer.WriteAttribute("Indirizzo", Indirizzo);
            writer.WriteAttribute("Tipo", Tipo);
            base.XMLSerialize(writer);
        }
 
        /// <summary>
        /// Compara due oggetti
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(DispositivoEsterno other)
        {
            var ret = DMD.Strings.Compare(this.Nome, obj.Nome, true);
            return ret;
        }

        int IComparable.CompareTo(object obj) { return this.CompareTo((DispositivoEsterno)obj);  }
    }
}