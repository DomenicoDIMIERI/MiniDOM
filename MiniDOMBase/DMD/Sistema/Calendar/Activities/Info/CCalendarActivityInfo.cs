using System;
using DMD;
using DMD.XML;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Racchiude alcune info sui contatti
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CCalendarActivityInfo 
            : IDMDXMLSerializable
        {
            /// <summary>
            /// ID ricontatto
            /// </summary>
            public int IDRicontatto;

            /// <summary>
            /// TipoRicontatto
            /// </summary>
            public string TipoRicontatto;

            /// <summary>
            /// IDContattoPrecedente
            /// </summary>
            public int IDContattoPrecedente;

            /// <summary>
            /// DataProssimoRicontatto
            /// </summary>
            public DateTime? DataProssimoRicontatto;

            /// <summary>
            /// DataProssimoAppuntamento
            /// </summary>
            public DateTime? DataProssimoAppuntamento;

            /// <summary>
            /// DataUltimoRicontatto
            /// </summary>
            public DateTime? DataUltimoRicontatto;

            /// <summary>
            /// DataUltimoAppuntamento
            /// </summary>
            public DateTime? DataUltimoAppuntamento;

            /// <summary>
            /// NumeroPraticheInCorso
            /// </summary>
            public int NumeroPraticheInCorso;

            /// <summary>
            /// NumeroPraticheArchiviate
            /// </summary>
            public int NumeroPraticheArchiviate;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCalendarActivityInfo()
            {
                //DMDObject.IncreaseCounter(this);
                IDRicontatto = 0;
                TipoRicontatto = "";
                IDContattoPrecedente = 0;
                DataProssimoRicontatto = default;
                DataProssimoAppuntamento = default;
                DataUltimoRicontatto = default;
                DataUltimoAppuntamento = default;
                NumeroPraticheInCorso = 0;
                NumeroPraticheArchiviate = 0;
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
            /// Inizializza
            /// </summary>
            /// <param name="personID"></param>
            public void Initialize(int personID)
            {
                // Dim cursor As New CContattoUtenteCursor
                // cursor.IDPersona.Value = personID
                // cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                // 'cursor.Ricontattare.Value = True
                // 'cursor.DataRicontatto.SortOrder = SortEnum.SORT_ASC
                // Me.IDContattoPrecedente = GetID(cursor.Item)
                // ' If Not (cursor.Item Is Nothing) Then Me.DataProssimoRicontatto = cursor.Item.DataRicontatto
                // cursor.Reset()

                // cursor = New CContattoUtenteCursor
                // cursor.IDPersona.Value = personID
                // cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                // cursor.Data.SortOrder = SortEnum.SORT_DESC

                // If Not (cursor.Item Is Nothing) Then Me.DataUltimoRicontatto = cursor.Item.Data
                // cursor.Reset()

                // Me.IDRicontatto = GetID(Ricontatti.GetProssimoRicontatto(personID))
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDRicontatto", IDRicontatto);
                writer.WriteAttribute("TipoRicontatto", TipoRicontatto);
                writer.WriteAttribute("IDContattoPrecedente", IDContattoPrecedente);
                writer.WriteAttribute("DataProssimoRicontatto", DataProssimoRicontatto);
                writer.WriteAttribute("DataProssimoAppuntamento", DataProssimoAppuntamento);
                writer.WriteAttribute("DataUltimoRicontatto", DataUltimoRicontatto);
                writer.WriteAttribute("DataUltimoAppuntamento", DataUltimoAppuntamento);
                writer.WriteAttribute("NumeroPraticheInCorso", NumeroPraticheInCorso);
                writer.WriteAttribute("NumeroPraticheArchiviate", NumeroPraticheArchiviate);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDRicontatto":
                        {
                            IDRicontatto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoRicontatto":
                        {
                            TipoRicontatto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDContattoPrecedente":
                        {
                            IDContattoPrecedente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataProssimoRicontatto":
                        {
                            DataProssimoRicontatto = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataProssimoAppuntamento":
                        {
                            DataProssimoAppuntamento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataUltimoRicontatto":
                        {
                            DataUltimoRicontatto = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataUltimoAppuntamento":
                        {
                            DataUltimoAppuntamento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "NumeroPraticheInCorso":
                        {
                            NumeroPraticheInCorso = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NumeroPraticheArchiviate":
                        {
                            NumeroPraticheArchiviate = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                }
            }

            //~CCalendarActivityInfo()
            //{
            //    //DMDObject.DecreaseCounter(this);
            //}
        }
    }
}