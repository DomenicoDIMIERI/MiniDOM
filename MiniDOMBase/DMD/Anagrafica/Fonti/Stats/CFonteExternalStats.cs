using System;
using DMD.Databases;
using DMD.XML;
using static minidom.Sistema;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Statistiche relative ad una fonte
        /// </summary>
        [Serializable]
        public class CFonteExternalStats
            : IComparable, IDMDXMLSerializable, IComparable<CFonteExternalStats>
        {

            /// <summary>
            /// Fonte
            /// </summary>
            public IFonte Fonte;               

            /// <summary>
            /// Stringa che identifica la fonte sui siti esterni
            /// </summary>
            public string IDAnnuncio;

            /// <summary>
            /// Numero di visualizzazioni totali
            /// </summary>
            public int Conteggio;

            /// <summary>
            /// Data della prima visualizzazione
            /// </summary>
            public DateTime? DI;

            /// <summary>
            /// Data dell'ultima visualizzazione
            /// </summary>
            public DateTime? DF;

            /// <summary>
            /// Visualizzazioni per giorno (medie)
            /// </summary>
            public int DV_AVE;

            /// <summary>
            /// Visualizzazioni per giorno (minimo)
            /// </summary>
            public int DV_MIN;

            /// <summary>
            /// Visualizzazioni per giorno (massimo)
            /// </summary>
            public int DV_MAX;

            /// <summary>
            /// Visualizzazioni per giorno (deviazione standard)
            /// </summary>
            public int DV_STD;             

            /// <summary>
            /// Costruttore
            /// </summary>
            public CFonteExternalStats()
            {
                ////DMDObject.IncreaseCounter(this);
                Fonte = null;
                Conteggio = 0;
                DI = default;
                DF = default;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="fonte"></param>
            public CFonteExternalStats(IFonte fonte) : this()
            {
                if (fonte is null)
                    throw new ArgumentNullException("fonte");
                Fonte = fonte;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="idAnnuncio"></param>
            public CFonteExternalStats(string idAnnuncio) : this()
            {
                idAnnuncio = DMD.Strings.Trim(idAnnuncio);
                if (string.IsNullOrEmpty(idAnnuncio))
                    throw new ArgumentNullException("idAnnuncio");
                IDAnnuncio = idAnnuncio;
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(CFonteExternalStats obj)
            {
                int ret = obj.Conteggio - this.Conteggio;
                if (ret == 0) ret = DMD.Strings.Compare(IDAnnuncio, obj.IDAnnuncio, true);
                return ret;
            }
            int IComparable.CompareTo(object obj)
            {
                return this.CompareTo((CFonteExternalStats)obj);
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
                    case "Fonte":
                        {
                            break;
                        }

                    case "IDAnnuncio":
                        {
                            IDAnnuncio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Conteggio":
                        {
                            Conteggio = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DI":
                        {
                            DI = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DF":
                        {
                            DF = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DV_AVE":
                        {
                            DV_AVE = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DV_MIN":
                        {
                            DV_MIN = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DV_MAX":
                        {
                            DV_MAX = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DV_STD":
                        {
                            DV_STD = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
                writer.WriteAttribute("Fonte", DBUtils.GetID(this.Fonte, 0));
                writer.WriteAttribute("IDAnnuncio", IDAnnuncio);
                writer.WriteAttribute("Conteggio", Conteggio);
                writer.WriteAttribute("DI", DI);
                writer.WriteAttribute("DF", DF);
                writer.WriteAttribute("DV_AVE", DV_AVE);
                writer.WriteAttribute("DV_MIN", DV_MIN);
                writer.WriteAttribute("DV_MAX", DV_MAX);
                writer.WriteAttribute("DV_STD", DV_STD);
            }


            /// <summary>
            /// Distruttore
            /// </summary>
            ~CFonteExternalStats()
            {
                ////DMDObject.DecreaseCounter(this);
            }
        }
    }
}