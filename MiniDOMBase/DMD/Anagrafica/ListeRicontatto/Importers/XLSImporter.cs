using System;
using System.Collections;
using DMD.Databases.Collections;
using DMD.XML;
using DMD.XLS;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Importer da Excel
        /// </summary>
        [Serializable]
        public class XLSImporter 
            : IDMDXMLSerializable
        {

            /// <summary>
            /// Percorso completo del file da importare
            /// </summary>
            public string URL;                                   

            /// <summary>
            /// Nome del file
            /// </summary>
            public string FileName;

            /// <summary>
            /// Tabelle estratte dal file
            /// </summary>
            public CCollection<XLSImporterTable> Tables;

            /// <summary>
            /// Nome della tabella da importare
            /// </summary>
            public string SelectedTableName;

            /// <summary>
            /// Nome della lista di ricontatti in cui inserire gli elmenti importati
            /// </summary>
            public string NomeListaDestinazione;

            /// <summary>
            /// Nome della campagna adv da simulare
            /// </summary>
            public string NomeCampagnaDestinazione;

            /// <summary>
            /// Tipo Fonte di default
            /// </summary>
            public string TipoFonte;

            /// <summary>
            /// Nome Fonte di default
            /// </summary>
            public string NomeFonte;

            /// <summary>
            /// Punto Operativo di default
            /// </summary>
            public string PuntoOperativo;

            /// <summary>
            /// Operatore di default
            /// </summary>
            public string Operatore;

            /// <summary>
            /// Data di ricontatto di default
            /// </summary>
            public DateTime? DataRicontatto;

            /// <summary>
            /// Numero della riga da cui iniziare ad importare (0 based)
            /// </summary>
            public int InitRow;

            /// <summary>
            /// Numero di righe da importare (se &lt; 1 importa tutto)
            /// </summary>
            public int EndCnt;    
            
            /// <summary>
            /// Parametri
            /// </summary>
            public CKeyCollection Parameters;

            /// <summary>
            /// Risultati
            /// </summary>
            public CCollection<XLSImporterResult> Results;

            /// <summary>
            /// Note
            /// </summary>
            public string Note;

            /// <summary>
            /// Costruttore
            /// </summary>
            public XLSImporter()
            {
                URL = "";
                FileName = "";
                Tables = new CCollection<XLSImporterTable>();
                SelectedTableName = "";
                NomeListaDestinazione = "";
                NomeCampagnaDestinazione = "";
                TipoFonte = "";
                NomeFonte = "";
                PuntoOperativo = "";
                Operatore = "";
                DataRicontatto = default;
                InitRow = 0;
                EndCnt = 0;
                Parameters = new CKeyCollection();
                Results = new CCollection<XLSImporterResult>();
                Note = "";
            }

            /// <summary>
            /// Restituisce la tabella selezionata
            /// </summary>
            /// <returns></returns>
            public XLSImporterTable GetSelectedTable()
            {
                foreach (XLSImporterTable tbl in Tables)
                {
                    if ((tbl.Name ?? "") == (SelectedTableName ?? ""))
                        return tbl;
                }

                return null;
            }

            /// <summary>
            /// Apre il file da importare e ne identifica le tabelle
            /// </summary>
            public virtual void Parse()
            {
                using (var wb = new XLWorkbook(this.FileName))
                {
                    foreach (var ws in wb.Worksheets)
                    {
                        var t = this.ParseTable(ws);
                        if (t is object)
                            Tables.Add(t);
                    }
                }
            }

            /// <summary>
            /// Carica la tabella
            /// </summary>
            /// <param name="tbl"></param>
            /// <returns></returns>
            protected virtual XLSImporterTable ParseTable(IXLWorksheet tbl)
            {
                var c = new XLSImporterTable();
                c.Name = tbl.Name;
                //var fc = tbl.FirstColumnUsed(XLCellsUsedOptions.AllContents);
                //var lc = tbl.FirstColumnUsed(XLCellsUsedOptions.AllContents);
                for (int i = 1; i < tbl.ColumnCount(); i++)
                {
                    var fld = ParseColumn(tbl.Column(i));
                    if (fld is object)
                        c.Columns.Add(fld);
                }

                return c;
            }

            /// <summary>
            /// Carica la colonna
            /// </summary>
            /// <param name="col"></param>
            /// <returns></returns>
            protected virtual XLSImporterColumn ParseColumn(IXLColumn col)
            {
                var c = new XLSImporterColumn();
                var fc = col.FirstCellUsed(XLCellsUsedOptions.AllContents);
                var lc = col.LastCellUsed(XLCellsUsedOptions.AllContents);

                c.SourceName = fc.GetValue<string>();
                c.SourceDataType = TypeCode.Empty;

                int i = 1;
                while ((i < 10) && ((fc.Address.RowNumber + i) <= lc.Address.RowNumber))
                {
                    i += 1;
                    var cell = col.Cell(fc.Address.RowNumber + i);
                    var value = cell.Value;
                    if (value is object )
                    {
                        if (c.SourceDataType == TypeCode.Empty)
                        {
                            c.SourceDataType = Type.GetTypeCode(value.GetType());
                        }
                        else if (c.SourceDataType != Type.GetTypeCode(value.GetType()))
                        {
                            c.SourceDataType = TypeCode.String;
                        }
                        else
                        {
                            //c.SourceDataType = Type.GetTypeCode(value.GetType();
                        }
                    }
                }
                 
                return c;
            }

            /// <summary>
            /// Imposta
            /// </summary>
            public virtual void Import()
            {
            }

            /// <summary>
            /// Serializza
            /// </summary>
            /// <param name="writer"></param>
            protected virtual void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("URL", URL);
                writer.WriteAttribute("FileName", FileName);
                writer.WriteAttribute("SelectedTableName", SelectedTableName);
                writer.WriteAttribute("NomeListaDestinazione", NomeListaDestinazione);
                writer.WriteAttribute("NomeCampagnaDestinazione", NomeCampagnaDestinazione);
                writer.WriteAttribute("TipoFonte", TipoFonte);
                writer.WriteAttribute("NomeFonte", NomeFonte);
                writer.WriteAttribute("PuntoOperativo", PuntoOperativo);
                writer.WriteAttribute("Operatore", Operatore);
                writer.WriteAttribute("DataRicontatto", DataRicontatto);
                writer.WriteAttribute("InitRow", InitRow);
                writer.WriteAttribute("EndCnt", EndCnt);
                writer.WriteTag("Tables", Tables);
                writer.WriteTag("Parameters", Parameters);
                writer.WriteTag("Results", Results);
                writer.WriteTag("Note", Note);
            }

            /// <summary>
            /// Deserializza
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected virtual void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "URL":
                        {
                            URL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "FileName":
                        {
                            FileName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SelectedTableName":
                        {
                            SelectedTableName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeListaDestinazione":
                        {
                            NomeListaDestinazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeCampagnaDestinazione":
                        {
                            NomeCampagnaDestinazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TipoFonte":
                        {
                            TipoFonte = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeFonte":
                        {
                            NomeFonte = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PuntoOperativo":
                        {
                            PuntoOperativo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Operatore":
                        {
                            Operatore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataRicontatto":
                        {
                            DataRicontatto = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "InitRow":
                        {
                            InitRow = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "EndCnt":
                        {
                            EndCnt = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Tables":
                        {
                            Tables.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Parameters":
                        {
                            Parameters = (CKeyCollection)fieldValue;
                            break;
                        }

                    case "Results":
                        {
                            Results.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Note":
                        {
                            Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                }
            }

            void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
            {
                this.XMLSerialize(writer);
            }

            void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
            {
                this.SetFieldInternal(fieldName, fieldValue);
            }
        }


    }
}