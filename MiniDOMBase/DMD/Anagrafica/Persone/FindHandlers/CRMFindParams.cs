using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Filtro utilizzato nell'interfaccia di ricerca generica
        /// </summary>
        [Serializable]
        public class CRMFindParams 
            : IDMDXMLSerializable, ICloneable
        {

            /// <summary>
            /// Tipo della ricerca
            /// </summary>
            public string Tipo;

            /// <summary>
            /// Testo della ricerca
            /// </summary>
            public string Text;

            /// <summary>
            /// Numero massimo di elementi da includere nella ricerca (se NULL vengono inclusi tutti gli elementi)
            /// </summary>
            public int? nMax;

                /// <summary>
            /// Se true il sistema usa la ricerca "intelligente" altrimenti la stringa viene interpretata senza alcuna elaborazione
            /// </summary>
            public bool IntelliSearch;

            /// <summary>
            /// Flag di ricerca
            /// </summary>
            public PFlags? flags;

            /// <summary>
            /// Se vero il sistema non considera le limitazioni imposte dagli amministratori
            /// </summary>
            public bool ignoreRights;

            /// <summary>
            /// Se valorizzato restringe la ricerca alle sole persone fisiche o alle sole persone giuridiche
            /// </summary>
            public TipoPersona? tipoPersona;

            /// <summary>
            /// Se diverso da zero restringe la ricerca al solo punto operativo specificato
            /// </summary>
            public int IDPuntoOperativo;

            /// <summary>
            /// Se valorizzato restringe la ricerca ai soli elementi nello stato specificato
            /// </summary>
            public string DettaglioEsito;

            /// <summary>
            /// Parametri aggiuntivi
            /// </summary>
            public CKeyCollection Parameters;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRMFindParams()
            {
                ////DMDObject.IncreaseCounter(this);
                this.Tipo = "";
                this.Text = "";
                this.nMax = default;
                this.IntelliSearch = true;
                this.flags = default;
                this.ignoreRights = false;
                this.tipoPersona = default;
                this.IDPuntoOperativo = 0;
                this.DettaglioEsito = "";
                this.Parameters = new CKeyCollection();
            }

            /// <summary>
            /// Deserializzazione
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            public void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Tipo":
                        {
                            Tipo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "text":
                        {
                            Text = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "nMax":
                        {
                            nMax = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IntelliSearch":
                        {
                            IntelliSearch = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "flags":
                        {
                            flags = (PFlags?)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ignoreRights":
                        {
                            ignoreRights = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "tipoPersona":
                        {
                            tipoPersona = (TipoPersona?)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDPuntoOperativo":
                        {
                            IDPuntoOperativo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DettaglioEsito":
                        {
                            DettaglioEsito = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    case "Parameters":
                        {
                            this.Parameters = (CKeyCollection)fieldValue;
                            break;
                        }
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            public void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Tipo", Tipo);
                writer.WriteAttribute("text", Text);
                writer.WriteAttribute("nMax", nMax);
                writer.WriteAttribute("IntelliSearch", IntelliSearch);
                writer.WriteAttribute("flags", (int?)flags);
                writer.WriteAttribute("ignoreRights", ignoreRights);
                writer.WriteAttribute("tipoPersona", (int?)tipoPersona);
                writer.WriteAttribute("IDPuntoOperativo", IDPuntoOperativo);
                writer.WriteAttribute("DettaglioEsito", DettaglioEsito);
                writer.WriteTag("Parameters", this.Parameters);
            }

            /// <summary>
            /// Clona l'oggetto
            /// </summary>
            /// <returns></returns>
            public CRMFindParams Clone()
            {
                return (CRMFindParams)this.MemberwiseClone();
            }

            object ICloneable.Clone()
            {
                return this.Clone();
            }

            /// <summary>
            /// Distruttore
            /// </summary>
            ~CRMFindParams()
            {
                ////DMDObject.DecreaseCounter(this);
            }
        }
    }
}