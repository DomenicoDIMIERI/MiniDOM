using System;
using System.Collections;
using minidom.repositories;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;

namespace minidom
{
    
    public partial class Anagrafica
    {

        /// <summary>
        /// Gruppi che il sistema identifica come un'unico oggetto
        /// </summary>
        [Serializable]
        public class MergeModuleGruppoPersona 
            : IDMDXMLSerializable
        {
            /// <summary>
            /// Persone appartenenti allo stesso gruppo
            /// </summary>
            public CCollection<CPersonaInfo> items = new CCollection<CPersonaInfo>();

            /// <summary>
            /// Costruttore
            /// </summary>
            public MergeModuleGruppoPersona()
            {
                ////DMDObject.IncreaseCounter(this);
            }

            /// <summary>
            /// Restituisce vero se la persona p2 appartiene allo stesso gruppo della prima persona nella collezione
            /// </summary>
            /// <param name="p2"></param>
            /// <returns></returns>
            public bool IsSameGroup(CPersonaInfo p2)
            {
                if (items.Count == 0)
                    return true;
                var p1 = items[0];
                if (p1.Persona.TipoPersona != p2.Persona.TipoPersona)
                    return false;
                if ((p1.Persona.Nominativo ?? "") == (p2.Persona.Nominativo ?? "") && p1.Persona.DataNascita.HasValue && p2.Persona.DataNascita.HasValue && DMD.DateUtils.Compare(p1.Persona.DataNascita, p2.Persona.DataNascita) == 0)
                    return true;
                foreach (CContatto c1 in p1.Persona.Recapiti)
                {
                    if (!string.IsNullOrEmpty(c1.Valore)) // AndAlso (c1.Validated .HasValue =False OrElse c1.Validated = True ) Then
                    {
                        foreach (CContatto c2 in p2.Persona.Recapiti)
                        {
                            if ((c2.Valore ?? "") == (c1.Valore ?? ""))
                                return true;
                        }
                    }
                }

                return false;
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
                    case "items":
                        {
                            items.Clear();
                            items.AddRange((IEnumerable)fieldValue);
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
                writer.WriteTag("items", items);
            }

            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
            {
                this.XMLSerialize(writer);
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
            {
                this.SetFieldInternal(fieldName, fieldValue);
            }

            /// <summary>
            /// Distruttore
            /// </summary>
            ~MergeModuleGruppoPersona()
            {
               // //DMDObject.DecreaseCounter(this);
            }
        }

        
    }
}