using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using minidom;
using static minidom.Sistema;
using DMD.Databases.Collections;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {


        /// <summary>
        /// Rappresenta un consumabile o una manutenzione effettuata su una postazione
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CManutenzione 
            : minidom.Databases.DBObjectPO, IComparable<CManutenzione>, IComparable
        {
            private int m_IDPostazione;
            [NonSerialized] private CPostazione m_Postazione;
            private string m_NomePostazione;
            private DateTime? m_DataInizioIntervento;
            private DateTime? m_DataFineIntervento;
            private decimal? m_ValoreImponibile;
            private decimal? m_ValoreIvato;
            private decimal? m_CostoSpedizione;
            private decimal? m_AltreSpese;
            private string m_Descrizione;
            private string m_Categoria1;
            private string m_Categoria2;
            private string m_Categoria3;
            private string m_Categoria4;
            private string m_Note;
            private VociManutenzioneCollection m_ElencoVoci;
            private int m_IDAziendaFornitrice;
            [NonSerialized] private CAzienda m_AziendaFornitrice;
            private string m_NomeAziendaFornitrice;
            private int m_IDRegistrataDa;
            [NonSerialized] private Sistema.CUser m_RegistrataDa;
            private string m_NomeRegistrataDa;
            private int m_IDDocumento;
            [NonSerialized] private object m_Documento;
            private string m_NumeroDocumento;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CManutenzione()
            {
                m_IDPostazione = 0;
                m_Postazione = null;
                m_NomePostazione = "";
                m_DataInizioIntervento = default;
                m_DataFineIntervento = default;
                m_ValoreImponibile = default;
                m_CostoSpedizione = default;
                m_AltreSpese = default;
                m_ValoreIvato = default;
                m_Descrizione = "";
                m_Categoria1 = "";
                m_Categoria2 = "";
                m_Categoria3 = "";
                m_Categoria4 = "";
                m_Note = "";
                m_ElencoVoci = null;
                m_IDAziendaFornitrice = 0;
                m_AziendaFornitrice = null;
                m_NomeAziendaFornitrice = "";
                m_IDRegistrataDa = 0;
                m_RegistrataDa = null;
                m_NomeRegistrataDa = "";
                m_IDDocumento = 0;
                m_Documento = null;
                m_NumeroDocumento = "";
                
            }
             
            /// <summary>
            /// Restituisce o imposta l'id della postazione
            /// </summary>
            public int IDPostazione
            {
                get
                {
                    return DBUtils.GetID(m_Postazione, m_IDPostazione);
                }

                set
                {
                    int oldValue = IDPostazione;
                    if (oldValue == value)
                        return;
                    m_IDPostazione = value;
                    m_Postazione = null;
                    DoChanged("IDPostazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la postazione
            /// </summary>
            public CPostazione Postazione
            {
                get
                {
                    if (m_Postazione is null)
                        m_Postazione = Postazioni.GetItemById(m_IDPostazione);
                    return m_Postazione;
                }

                set
                {
                    var oldValue = Postazione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Postazione = value;
                    m_IDPostazione = DBUtils.GetID(value, 0);
                    m_NomePostazione = "";
                    if (value is object)
                        m_NomePostazione = value.Nome;
                    DoChanged("Postazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della postazione
            /// </summary>
            public string NomePostazione
            {
                get
                {
                    return m_NomePostazione;
                }

                set
                {
                    string oldValue = m_NomePostazione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePostazione = value;
                    DoChanged("NomePostazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data e l'ora di inizio intervento
            /// </summary>
            public DateTime? DataInizioIntervento
            {
                get
                {
                    return m_DataInizioIntervento;
                }

                set
                {
                    var oldValue = m_DataInizioIntervento;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataInizioIntervento = value;
                    DoChanged("DataInizioIntervento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data e l'ora di fine intervento
            /// </summary>
            public DateTime? DataFineIntervento
            {
                get
                {
                    return m_DataFineIntervento;
                }

                set
                {
                    var oldValue = m_DataFineIntervento;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataFineIntervento = value;
                    DoChanged("DataFineIntervento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'imponibile pagato per l'intervento
            /// </summary>
            public decimal? ValoreImponibile
            {
                get
                {
                    return m_ValoreImponibile;
                }

                set
                {
                    var oldValue = m_ValoreImponibile;
                    if (value == oldValue == true)
                        return;
                    m_ValoreImponibile = value;
                    DoChanged("ValoreImponibile", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce il costo ivato dell'intervento
            /// </summary>
            public decimal? ValoreIvato
            {
                get
                {
                    return m_ValoreIvato;
                }

                set
                {
                    var oldValue = m_ValoreIvato;
                    if (oldValue == value == true)
                        return;
                    m_ValoreIvato = value;
                    DoChanged("ValoreIvato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta eventuali costi di spedizione
            /// </summary>
            public decimal? CostoSpedizione
            {
                get
                {
                    return m_CostoSpedizione;
                }

                set
                {
                    var oldValue = m_CostoSpedizione;
                    if (oldValue == value == true)
                        return;
                    m_CostoSpedizione = value;
                    DoChanged("CostoSpedizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta eventuali altre spese
            /// </summary>
            public decimal? AltreSpese
            {
                get
                {
                    return m_AltreSpese;
                }

                set
                {
                    var oldValue = m_AltreSpese;
                    if (oldValue == value == true)
                        return;
                    m_AltreSpese = value;
                    DoChanged("AltreSpese", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la descrizione dell'intervento
            /// </summary>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la categoria dell'intervento
            /// </summary>
            public string Categoria1
            {
                get
                {
                    return m_Categoria1;
                }

                set
                {
                    string oldValue = m_Categoria1;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Categoria1 = value;
                    DoChanged("Categoria1", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la categoria dell'intervento
            /// </summary>
            public string Categoria2
            {
                get
                {
                    return m_Categoria2;
                }

                set
                {
                    string oldValue = m_Categoria2;
                    value = DMD.Strings.Trim(value);
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_Categoria2 = value;
                    DoChanged("Categoria", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la categoria dell'intervento
            /// </summary>
            public string Categoria3
            {
                get
                {
                    return m_Categoria3;
                }

                set
                {
                    string oldValue = m_Categoria3;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Categoria3 = value;
                    DoChanged("Categoria3", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la categoria dell'intervento
            /// </summary>
            public string Categoria4
            {
                get
                {
                    return m_Categoria4;
                }

                set
                {
                    string oldValue = m_Categoria4;
                    value = DMD.Strings.Trim(value);
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_Categoria4 = value;
                    DoChanged("Categoria4", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una nota sull'intervento
            /// </summary>
            public string Note
            {
                get
                {
                    return m_Note;
                }

                set
                {
                    string oldValue = m_Note;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Note = value;
                    DoChanged("Note", value, oldValue);
                }
            }

            /// <summary>
            /// Elenco dettagliato delle voci dell'intervento
            /// </summary>
            public VociManutenzioneCollection ElencoVoci
            {
                get
                {
                    if (m_ElencoVoci is null)
                        m_ElencoVoci = new VociManutenzioneCollection(this);
                    return m_ElencoVoci;
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'azienda che ha fornito la manutenzione
            /// </summary>
            public int IDAziendaFornitrice
            {
                get
                {
                    return DBUtils.GetID(m_AziendaFornitrice, m_IDAziendaFornitrice);
                }

                set
                {
                    int oldValue = IDAziendaFornitrice;
                    if (oldValue == value)
                        return;
                    m_IDAziendaFornitrice = value;
                    m_AziendaFornitrice = null;
                    DoChanged("IDAziendaFornitrice", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'azienda che ha fornito la manutenzione
            /// </summary>
            public CAzienda AziendaFornitrice
            {
                get
                {
                    if (m_AziendaFornitrice is null)
                        m_AziendaFornitrice = Aziende.GetItemById(m_IDAziendaFornitrice);
                    return m_AziendaFornitrice;
                }

                set
                {
                    var oldValue = m_AziendaFornitrice;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_AziendaFornitrice = value;
                    m_IDAziendaFornitrice = DBUtils.GetID(value, 0);
                    m_NomeAziendaFornitrice = "";
                    if (value is object)
                        m_NomeAziendaFornitrice = value.Nominativo;
                    DoChanged("AziendaFornitrice", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'azienda che ha effettuato la manutenzione
            /// </summary>
            public string NomeAziendaFornitrice
            {
                get
                {
                    return m_NomeAziendaFornitrice;
                }

                set
                {
                    string oldValue = m_NomeAziendaFornitrice;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeAziendaFornitrice = value;
                    DoChanged("NomeAziendaFornitrice", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id dell'utente che ha registrato l'operazione
            /// </summary>
            public int IDRegistrataDa
            {
                get
                {
                    return DBUtils.GetID(m_RegistrataDa, m_IDRegistrataDa);
                }

                set
                {
                    int oldValue = IDRegistrataDa;
                    if (oldValue == value)
                        return;
                    m_IDRegistrataDa = value;
                    m_RegistrataDa = null;
                    DoChanged("IDRegistrataDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha registrato la manutenzione
            /// </summary>
            public Sistema.CUser RegistrataDa
            {
                get
                {
                    if (m_RegistrataDa is null)
                        m_RegistrataDa = Sistema.Users.GetItemById(m_IDRegistrataDa);
                    return m_RegistrataDa;
                }

                set
                {
                    var oldValue = RegistrataDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_RegistrataDa = value;
                    m_IDRegistrataDa = DBUtils.GetID(value, 0);
                    m_NomeRegistrataDa = "";
                    if (value is object)
                        m_NomeRegistrataDa = value.Nominativo;
                    DoChanged("RegistrataDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha registrato l'oeprazione
            /// </summary>
            public string NomeRegistrataDa
            {
                get
                {
                    return m_NomeRegistrataDa;
                }

                set
                {
                    string oldValue = m_NomeRegistrataDa;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeRegistrataDa = value;
                    DoChanged("NomeRegistratoDa", value, oldValue);
                }
            }

             
            /// <summary>
            /// Restituisce o imposta l'ID del documento rilasciato dal manutentore
            /// </summary>
            public int IDDocumento
            {
                get
                {
                    return DBUtils.GetID(this.m_Documento, this.m_IDDocumento);
                }

                set
                {
                    int oldValue = this.IDDocumento;
                    if (oldValue == value)
                        return;
                    m_IDDocumento = value;
                    m_Documento = null;
                    DoChanged("IDDocumento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il documento
            /// </summary>
            public object Documento
            {
                get
                {
                    return m_Documento;
                }

                set
                {
                    var oldValue = m_Documento;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Documento = value;
                    m_IDDocumento = DBUtils.GetID(value, 0);
                    DoChanged("Documento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero del documento
            /// </summary>
            public string NumeroDocumento
            {
                get
                {
                    return m_NumeroDocumento;
                }

                set
                {
                    string oldValue = m_NumeroDocumento;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NumeroDocumento = value;
                    DoChanged("NumeroDocumento", value, oldValue);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Manutenzioni; // Manutenzioni.Module;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}

            /// <summary>
            /// Discriminante
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Manutenzioni";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDPostazione = reader.Read("IDPostazione",  m_IDPostazione);
                m_NomePostazione = reader.Read("NomePostazione",  m_NomePostazione);
                m_DataInizioIntervento = reader.Read("DataInizioIntervento",  m_DataInizioIntervento);
                m_DataFineIntervento = reader.Read("DataFineIntervento",  m_DataFineIntervento);
                m_ValoreImponibile = reader.Read("ValoreImponibile",  m_ValoreImponibile);
                m_ValoreIvato = reader.Read("ValoreIvato",  m_ValoreIvato);
                m_CostoSpedizione = reader.Read("CostoSpedizione",  m_CostoSpedizione);
                m_AltreSpese = reader.Read("AltreSpese",  m_AltreSpese);
                m_Descrizione = reader.Read("Descrizione",  m_Descrizione);
                m_Categoria1 = reader.Read("Categoria1",  m_Categoria1);
                m_Categoria2 = reader.Read("Categoria2",  m_Categoria2);
                m_Categoria3 = reader.Read("Categoria3",  m_Categoria3);
                m_Categoria4 = reader.Read("Categoria4",  m_Categoria4);
                m_Note = reader.Read("Note",  m_Note);
                // Me.m_ElencoVoci = reader.Read("ElencoVoci", Me.m_ElencoVoci)
                m_IDAziendaFornitrice = reader.Read("IDAziendaFornitrice",  m_IDAziendaFornitrice);
                m_NomeAziendaFornitrice = reader.Read("NomeAziendaFornitrice",  m_NomeAziendaFornitrice);
                m_IDRegistrataDa = reader.Read("IDRegistrataDa",  m_IDRegistrataDa);
                m_NomeRegistrataDa = reader.Read("NomeRegistrataDa",  m_NomeRegistrataDa);
                m_IDDocumento = reader.Read("IDDocumento",  m_IDDocumento);
                m_NumeroDocumento = reader.Read("NumeroDocumento",  m_NumeroDocumento);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDPostazione", IDPostazione);
                writer.Write("NomePostazione", m_NomePostazione);
                writer.Write("DataInizioIntervento", m_DataInizioIntervento);
                writer.Write("DataFineIntervento", m_DataFineIntervento);
                writer.Write("ValoreImponibile", m_ValoreImponibile);
                writer.Write("ValoreIvato", m_ValoreIvato);
                writer.Write("CostoSpedizione", m_CostoSpedizione);
                writer.Write("AltreSpese", m_AltreSpese);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("Categoria1", m_Categoria1);
                writer.Write("Categoria2", m_Categoria2);
                writer.Write("Categoria3", m_Categoria3);
                writer.Write("Categoria4", m_Categoria4);
                writer.Write("Note", m_Note);
                // Me.m_ElencoVoci = reader.Read("ElencoVoci", Me.m_ElencoVoci)
                writer.Write("IDAziendaFornitrice", IDAziendaFornitrice);
                writer.Write("NomeAziendaFornitrice", m_NomeAziendaFornitrice);
                writer.Write("IDRegistrataDa", IDRegistrataDa);
                writer.Write("NomeRegistrataDa", m_NomeRegistrataDa);
                writer.Write("IDDocumento", IDDocumento);
                writer.Write("NumeroDocumento", m_NumeroDocumento);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDPostazione", typeof(int), 1);
                c = table.Fields.Ensure("NomePostazione", typeof(string), 0);
                c = table.Fields.Ensure("DataInizioIntervento", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataFineIntervento", typeof(DateTime), 1);
                c = table.Fields.Ensure("ValoreImponibile", typeof(Decimal), 1);
                c = table.Fields.Ensure("ValoreIvato", typeof(Decimal), 1);
                c = table.Fields.Ensure("CostoSpedizione", typeof(Decimal), 1);
                c = table.Fields.Ensure("AltreSpese", typeof(Decimal), 1);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("Categoria1", typeof(string), 255);
                c = table.Fields.Ensure("Categoria2", typeof(string), 255);
                c = table.Fields.Ensure("Categoria3", typeof(string), 255);
                c = table.Fields.Ensure("Categoria4", typeof(string), 255);
                c = table.Fields.Ensure("Note", typeof(string), 0);
                c = table.Fields.Ensure("IDAziendaFornitrice", typeof(int), 1);
                c = table.Fields.Ensure("NomeAziendaFornitrice", typeof(string), 255);
                c = table.Fields.Ensure("IDRegistrataDa", typeof(int), 1);
                c = table.Fields.Ensure("NomeRegistrataDa", typeof(string), 255);
                c = table.Fields.Ensure("IDDocumento", typeof(int), 1);
                c = table.Fields.Ensure("NumeroDocumento", typeof(string), 255);
                 
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxPostazione", new string[] { "IDPostazione", "NomePostazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataInizioIntervento", "DataFineIntervento" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCosti", new string[] { "ValoreImponibile", "ValoreIvato", "CostoSpedizione", "AltreSpese"}, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCategorie", new string[] { "Categoria1", "Categoria2", "Categoria3", "Categoria4" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAziendaMan", new string[] { "IDAziendaFornitrice", "NomeAziendaFornitrice" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRegistrato", new string[] { "IDRegistrataDa", "NomeRegistrataDa" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDocumento", new string[] { "IDDocumento", "NumeroDocumento" }, DBFieldConstraintFlags.None);

                //c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                //c = table.Fields.Ensure("Note", typeof(string), 0);
            }


            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDPostazione", IDPostazione);
                writer.WriteAttribute("NomePostazione", m_NomePostazione);
                writer.WriteAttribute("DataInizioIntervento", m_DataInizioIntervento);
                writer.WriteAttribute("DataFineIntervento", m_DataFineIntervento);
                writer.WriteAttribute("ValoreImponibile", m_ValoreImponibile);
                writer.WriteAttribute("ValoreIvato", m_ValoreIvato);
                writer.WriteAttribute("CostoSpedizione", m_CostoSpedizione);
                writer.WriteAttribute("AltreSpese", m_AltreSpese);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("Categoria1", m_Categoria1);
                writer.WriteAttribute("Categoria2", m_Categoria2);
                writer.WriteAttribute("Categoria3", m_Categoria3);
                writer.WriteAttribute("Categoria4", m_Categoria4);
                writer.WriteAttribute("IDAziendaFornitrice", IDAziendaFornitrice);
                writer.WriteAttribute("NomeAziendaFornitrice", m_NomeAziendaFornitrice);
                writer.WriteAttribute("IDRegistrataDa", IDRegistrataDa);
                writer.WriteAttribute("NomeRegistrataDa", m_NomeRegistrataDa);
                writer.WriteAttribute("IDDocumento", IDDocumento);
                writer.WriteAttribute("NumeroDocumento", m_NumeroDocumento);
                base.XMLSerialize(writer);
                writer.WriteTag("ElencoVoci", this.ElencoVoci);
                writer.WriteTag("Note", this.m_Note);
            }

            /// <summary>
            /// Deserializza xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDPostazione":
                        {
                            m_IDPostazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePostazione":
                        {
                            m_NomePostazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataInizioIntervento":
                        {
                            m_DataInizioIntervento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFineIntervento":
                        {
                            m_DataFineIntervento = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ValoreImponibile":
                        {
                            m_ValoreImponibile = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "ValoreIvato":
                        {
                            m_ValoreIvato = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "CostoSpedizione":
                        {
                            m_CostoSpedizione = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "AltreSpese":
                        {
                            m_AltreSpese = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Categoria1":
                        {
                            m_Categoria1 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Categoria2":
                        {
                            m_Categoria2 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Categoria3":
                        {
                            m_Categoria3 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Categoria4":
                        {
                            m_Categoria4 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAziendaFornitrice":
                        {
                            m_IDAziendaFornitrice = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeAziendaFornitrice":
                        {
                            m_NomeAziendaFornitrice = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDRegistrataDa":
                        {
                            m_IDRegistrataDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeRegistrataDa":
                        {
                            m_NomeRegistrataDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    
                    case "IDDocumento":
                        {
                            m_IDDocumento = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NumeroDocumento":
                        {
                            m_NumeroDocumento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ElencoVoci":
                        {
                            m_ElencoVoci = new VociManutenzioneCollection();
                            m_ElencoVoci.SetOwner(this);
                            m_ElencoVoci.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    
                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Compara due oggeti
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(CManutenzione other)
            {
                int ret = DMD.DateUtils.Compare(m_DataInizioIntervento, other.m_DataInizioIntervento);
                if (ret == 0) ret = DMD.DateUtils.Compare(m_DataFineIntervento, other.m_DataFineIntervento);
                if (ret == 0) ret = DMD.Strings.Compare(m_Descrizione, other.m_Descrizione, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CManutenzione)obj);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return 
                    DMD.Strings.ConcatArray(
                            "Manutenzione su " , m_NomePostazione ,
                            " del " , Sistema.Formats.FormatUserDateTime(m_DataInizioIntervento)
                            );
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_NomePostazione, this.m_DataInizioIntervento);
            }

            /// <summary>
            /// Restituisce true se gli oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CManutenzione) && this.Equals((CManutenzione)obj);
            }

            /// <summary>
            /// Restituisce true se gli oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CManutenzione obj)
            {
                return base.Equals(obj)
                     && DMD.Integers.EQ(this.IDPostazione, obj.IDPostazione)
                    && DMD.Strings.EQ(this.m_NomePostazione, obj.m_NomePostazione)
                    && DMD.DateUtils.EQ(this.m_DataInizioIntervento, obj.m_DataInizioIntervento)
                    && DMD.DateUtils.EQ(this.m_DataFineIntervento, obj.m_DataFineIntervento)
                    && DMD.Decimals.EQ(this.m_ValoreImponibile, obj.m_ValoreImponibile)
                    && DMD.Decimals.EQ(this.m_ValoreIvato, obj.m_ValoreIvato)
                    && DMD.Decimals.EQ(this.m_CostoSpedizione, obj.m_CostoSpedizione)
                    && DMD.Decimals.EQ(this.m_AltreSpese, obj.m_AltreSpese)
                    && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                    && DMD.Strings.EQ(this.m_Categoria1, obj.m_Categoria1)
                    && DMD.Strings.EQ(this.m_Categoria2, obj.m_Categoria2)
                    && DMD.Strings.EQ(this.m_Categoria3, obj.m_Categoria3)
                    && DMD.Strings.EQ(this.m_Categoria4, obj.m_Categoria4)
                    && DMD.Strings.EQ(this.m_Note, obj.m_Note)
                    && DMD.Integers.EQ(this.IDAziendaFornitrice, obj.IDAziendaFornitrice)
                    && DMD.Strings.EQ(this.m_NomeAziendaFornitrice, obj.m_NomeAziendaFornitrice)
                    && DMD.Integers.EQ(this.IDRegistrataDa, obj.IDRegistrataDa)
                    && DMD.Strings.EQ(this.m_NomeRegistrataDa, obj.m_NomeRegistrataDa)
                    && DMD.Integers.EQ(this.IDDocumento, obj.IDDocumento)
                    && DMD.Strings.EQ(this.m_NumeroDocumento, obj.m_NumeroDocumento)
                    ;
                    //private VociManutenzioneCollection m_ElencoVoci;
              
                    //private CKeyCollection m_Parameters;
                }
        }
    }
}