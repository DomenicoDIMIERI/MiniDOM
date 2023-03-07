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
        /// Filtro usato per le rierche di persone e aziende
        /// </summary>
        [Serializable]
        public class CRMFindFilter 
            : DMD.XML.DMDBaseXMLObject
        {
            /// <summary>
            /// ID della persona da cercare
            /// </summary>
            public int ID;

            /// <summary>
            /// Nominativo da ricercare
            /// </summary>
            public string Nominativo;

            /// <summary>
            /// Tipo dell'oggetto
            /// </summary>
            public string TipoOggetto;

            /// <summary>
            /// ID del punto operativo
            /// </summary>
            public int IDPuntoOperativo;

            /// <summary>
            /// ID dell'operatore
            /// </summary>
            public int IDOperatore;

            /// <summary>
            /// ID della persona
            /// </summary>
            public int IDPersona;

            /// <summary>
            /// Flags
            /// </summary>
            public int Flags;

            /// <summary>
            /// Numero
            /// </summary>
            public string Numero;

            /// <summary>
            /// Contenuto
            /// </summary>
            public string Contenuto;

            /// <summary>
            /// Etichetta
            /// </summary>
            public string Etichetta;

            /// <summary>
            /// Dal
            /// </summary>
            public DateTime? Dal;

            /// <summary>
            /// Al
            /// </summary>
            public DateTime? Al;

            /// <summary>
            /// Scopo
            /// </summary>
            public string Scopo;

            /// <summary>
            /// Esito
            /// </summary>
            public int? Esito;

            /// <summary>
            /// Dettagli esito
            /// </summary>
            public string DettaglioEsito;

            /// <summary>
            /// Stato Conversazione
            /// </summary>
            public int? StatoConversazione;

            /// <summary>
            /// ID contesto
            /// </summary>
            public int? IDContesto;

            /// <summary>
            /// Tipo contesto
            /// </summary>
            public string TipoContesto;

            /// <summary>
            /// Numero massimo di risultati da restituire
            /// </summary>
            public int? nMax;

            /// <summary>
            /// Se true vengono restituiti tutti i risultati
            /// </summary>
            public bool IgnoreRights;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CRMFindFilter()
            {
                //DMDObject.IncreaseCounter(this);
                ID = 0;
                Nominativo = "";
                TipoOggetto = "";
                IDPuntoOperativo = 0;
                IDOperatore = 0;
                IDPersona = 0;
                Flags = 0;
                Numero = "";
                Contenuto = "";
                Etichetta = "";
                Dal = default;
                Al = default;
                Scopo = "";
                Esito = default;
                DettaglioEsito = "";
                StatoConversazione = default;
                IDContesto = default;
                TipoContesto = "";
                nMax = default;
                IgnoreRights = false;
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
                    case "Nominativo":
                        {
                            Nominativo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoOggetto":
                        {
                            TipoOggetto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDPuntoOperativo":
                        {
                            IDPuntoOperativo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDOperatore":
                        {
                            IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDPersona":
                        {
                            IDPersona = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            Flags = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Esito":
                        {
                            Esito = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoConversazione":
                        {
                            StatoConversazione = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Numero":
                        {
                            Numero = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Contenuto":
                        {
                            Contenuto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DettaglioEsito":
                        {
                            DettaglioEsito = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Etichetta":
                        {
                            Etichetta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Dal":
                        {
                            Dal = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Al":
                        {
                            Al = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "TipoContesto":
                        {
                            TipoContesto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDContesto":
                        {
                            IDContesto = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "nMax":
                        {
                            nMax = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IgnoreRights":
                        {
                            IgnoreRights = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "ID":
                        {
                            ID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Scopo":
                        {
                            Scopo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
                writer.WriteAttribute("Nominativo", Nominativo);
                writer.WriteAttribute("TipoOggetto", TipoOggetto);
                writer.WriteAttribute("IDPuntoOperativo", IDPuntoOperativo);
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("IDPersona", IDPersona);
                writer.WriteAttribute("Flags", Flags);
                writer.WriteAttribute("Numero", Numero);
                writer.WriteAttribute("Contenuto", Contenuto);
                writer.WriteAttribute("Etichetta", Etichetta);
                writer.WriteAttribute("Dal", Dal);
                writer.WriteAttribute("Al", Al);
                writer.WriteAttribute("Esito", Esito);
                writer.WriteAttribute("DettaglioEsito", DettaglioEsito);
                writer.WriteAttribute("StatoConversazione", StatoConversazione);
                writer.WriteAttribute("IDContesto", IDContesto);
                writer.WriteAttribute("TipoContesto", TipoContesto);
                writer.WriteAttribute("nMax", nMax);
                writer.WriteAttribute("IgnoreRights", nMax);
                writer.WriteAttribute("ID", ID);
                writer.WriteAttribute("Scopo", Scopo);
                base.XMLSerialize(writer);
            }
 
        }
    }
}