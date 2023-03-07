using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using DMD.Databases.Collections;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Record 
        /// </summary>
        [Serializable]
        public class CMergePersonaRecord 
            : DMD.XML.DMDBaseXMLObject // Databases.DBObjectBase
        {
            /// <summary>
            /// Nome della tabella
            /// </summary>
            public string NomeTabella;

            /// <summary>
            /// ID del record
            /// </summary>
            public int RecordID;

            /// <summary>
            /// Nome del campo affetto
            /// </summary>
            public string FieldName;

            /// <summary>
            /// Parametri
            /// </summary>
            public CKeyCollection @params;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CMergePersonaRecord()
            {
                NomeTabella = "";
                RecordID = 0;
                FieldName = "";
                @params = new CKeyCollection();
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.NomeTabella , "[" , this.RecordID , "]");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.NomeTabella, this.RecordID);
            }

            /// <summary>
            /// Restituisce true se gli oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed  override bool Equals(object obj)
            {
                return (obj is CMergePersonaRecord) && this.Equals((CMergePersonaRecord)obj);
            }

            /// <summary>
            /// Restituisce true se gli oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CMergePersonaRecord obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.NomeTabella, obj.NomeTabella)
                    && DMD.Integers.EQ(this.RecordID, obj.RecordID)
                    && DMD.Strings.EQ(this.FieldName, obj.FieldName)
              ; //public CKeyCollection @params;
            }

            //public override CModulesClass GetModule()
            //{
            //    return null;
            //}

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            //public override string GetTableName()
            //{
            //    return "tbl_MergePersonaRecords";
            //}

            //protected override bool LoadFromRecordset(Databases.DBReader reader)
            //{
            //    NomeTabella = reader.Read("NomeTabella",  NomeTabella);
            //    RecordID = reader.Read("RecordID",  RecordID);
            //    FieldName = reader.Read("FieldName",  FieldName);
            //    string tmp = reader.Read("params", "");
            //    if (!string.IsNullOrEmpty(tmp))
            //    {
            //        @params = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);
            //    }
            //    else
            //    {
            //        @params = new CKeyCollection();
            //    }

            //    return base.LoadFromRecordset(reader);
            //}

            //protected override bool SaveToRecordset(Databases.DBWriter writer)
            //{
            //    writer.Write("NomeTabella", NomeTabella);
            //    writer.Write("RecordID", RecordID);
            //    writer.Write("FieldName", FieldName);
            //    writer.Write("params", DMD.XML.Utils.Serializer.Serialize(@params));
            //    return base.SaveToRecordset(writer);
            //}

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("NomeTabella", NomeTabella);
                writer.WriteAttribute("RecordID", RecordID);
                writer.WriteAttribute("FieldName", FieldName);
                base.XMLSerialize(writer);
                writer.WriteTag("params", @params);
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
                    case "NomeTabella":
                        {
                            NomeTabella = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "FieldName":
                        {
                            FieldName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RecordID":
                        {
                            RecordID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "params":
                        {
                            @params = (CKeyCollection)fieldValue;
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