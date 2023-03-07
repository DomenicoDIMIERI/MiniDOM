using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Informazioni su una persona in attesa nel CRM
        /// </summary>
        [Serializable]
        public class ContattoInAttesaInfo 
            : Anagrafica.CPersonaInfo
        {

            /// <summary>
            /// Contatto in attesa (telefonata, visita, fax, ecc)
            /// </summary>
            public CContattoUtente Contatto;

            /// <summary>
            /// Costruttore
            /// </summary>
            public ContattoInAttesaInfo()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="contatto"></param>
            public ContattoInAttesaInfo(CContattoUtente contatto) : base()
            {
                if (contatto is null)
                    throw new ArgumentNullException("contatto");
                if (contatto.Persona is object)
                    Parse(contatto.Persona);
                Contatto = contatto;
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                base.XMLSerialize(writer);
                writer.WriteTag("Contatto", Contatto);
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
                    case "Contatto":
                        {
                            Contatto = (CContattoUtente)fieldValue;
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }
        }
    }
}