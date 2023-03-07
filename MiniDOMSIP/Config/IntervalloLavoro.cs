using System;
using DMD.XML;
using DMD;

namespace minidom.PBX
{

    /// <summary>
    /// Orario di lavoro definito per una postazione di lavoro
    /// </summary>
    [Serializable]
    public class IntervalloLavoro 
        : DMD.XML.DMDBaseXMLObject, IComparable, IComparable<IntervalloLavoro>
    {
        public bool Attivo;                // Vero se l'intervallo è abilitato
        public string Nome;                   // Nome dell'intervallo di lavoro
        public DateTime? OraInizio;               // Ora di inizio dell'intervallo di lavoro
        public DateTime? OraFine;                 // Ora di fine dell'intervallo di lavoro
        public bool Consenti;              // Vero se l'intervallo di lavoro è consentito
        public bool Nega;                  // Vero se l'intervallo di lavoro è negato
        public int Tolleranza;            // Tolleranza in minuti rispetto agli orari di ingresso/uscita
        public string[] AzioniInizio;         // Elenco delle azioni da eseguire quando l'intervallo diventa attivo
        public string[] AzioniFine;           // Elenco delle azioni da eseguire quando l'intervallo scade

        /// <summary>
        /// Costruttore
        /// </summary>
        public IntervalloLavoro()
        {
            Attivo = true;
            Nome = "";
            OraInizio = default;
            OraFine = default;
            Consenti = true;
            Nega = false;
            Tolleranza = 15;
            AzioniInizio = DMD.Arrays.Empty<string>();
            AzioniFine = new[] { "Soft Shutdown" };
        }

        /// <summary>
        /// Restituisce una stringa che rappresenta l'oggetto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Nome;
        }

        /// <summary>
        /// Compara due oggetti
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IntervalloLavoro other)
        {
            int ret = Strings.Compare(this.Nome, other.Nome, true);
            if (ret == 0) ret = DateUtils.Compare(this.OraInizio, other.OraInizio);
            if (ret == 0) ret = DateUtils.Compare(this.OraFine, other.OraFine);
            return ret;
        }

        int IComparable.CompareTo(object obj) {  return this.CompareTo((IntervalloLavoro)obj);  }

        /// <summary>
        /// Restituisce il codice hash dell'oggetto
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCalculator.Calculate(this.Nome);
        }

        /// <summary>
        /// Restituisce true se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public sealed override bool Equals(DMDBaseXMLObject obj)
        {
            return (obj is IntervalloLavoro) && this.Equals((IntervalloLavoro)obj);
        }

        /// <summary>
        /// Restituisce true se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Equals(IntervalloLavoro obj)
        {
            return base.Equals(obj)
                 && DMD.Booleans.EQ(this.Attivo, obj.Attivo)
                 && DMD.Strings.EQ(this.Nome, obj.Nome)
                 && DMD.DateUtils.EQ(this.OraInizio, obj.OraInizio)
                 && DMD.DateUtils.EQ(this.OraFine, obj.OraFine)
                 && DMD.Booleans.EQ(this.Consenti, obj.Consenti)
                 && DMD.Booleans.EQ(this.Nega, obj.Nega)
                 && DMD.Integers.EQ(this.Tolleranza, obj.Tolleranza)
                 && DMD.Arrays.EQ(this.AzioniInizio, obj.AzioniInizio)
                 && DMD.Arrays.EQ(this.AzioniFine, obj.AzioniFine)
                ;
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
                case "Attivo":
                    {
                        Attivo = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue).Value;
                        break;
                    }

                case "Nome":
                    {
                        Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "OraInizio":
                    {
                        OraInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                        break;
                    }

                case "OraFine":
                    {
                        OraFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                        break;
                    }

                case "Consenti":
                    {
                        Consenti = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue).Value;
                        break;
                    }

                case "Nega":
                    {
                        Nega = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue).Value;
                        break;
                    }

                case "Tolleranza":
                    {
                        Tolleranza = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue).Value;
                        break;
                    }

                case "AzioniInizio":
                    {
                        if (fieldValue is Array)
                        {
                            AzioniInizio = (string[])fieldValue;
                        }
                        else
                        {
                            string str = DMD.Strings.Trim(DMD.Strings.CStr(fieldValue));
                            if (!string.IsNullOrEmpty(str))
                                AzioniInizio = new[] { str };
                        }

                        break;
                    }

                case "AzioniFine":
                    {
                        if (fieldValue is Array)
                        {
                            AzioniFine = (string[])fieldValue;
                        }
                        else
                        {
                            string str = DMD.Strings.Trim(DMD.Strings.CStr(fieldValue));
                            if (!string.IsNullOrEmpty(str))
                                AzioniFine = new[] { str };
                        }

                        break;
                    }
                default:
                    base.SetFieldInternal(fieldName, fieldValue);
                    break;
            }
        }

        /// <summary>
        /// Serializzazione xml
        /// </summary>
        /// <param name="writer"></param>
        protected override void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("Attivo", Attivo);
            writer.WriteAttribute("Nome", Nome);
            writer.WriteAttribute("OraInizio", OraInizio);
            writer.WriteAttribute("OraFine", OraFine);
            writer.WriteAttribute("Consenti", Consenti);
            writer.WriteAttribute("Nega", Nega);
            writer.WriteAttribute("Tolleranza", Tolleranza);
            base.XMLSerialize(writer);
            writer.WriteTag("AzioniInizio", AzioniInizio);
            writer.WriteTag("AzioniFine", AzioniFine);
        }

    }
}