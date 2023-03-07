using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Office;


namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Flag definiti per un documento generabile
        /// </summary>
        [Flags]
        public enum DocFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Il documento è caratterizzato da un numero che lo identifica
            /// </summary>
            /// <remarks></remarks>
            HaNumero = 1,

            /// <summary>
            /// Il documento richiede l'inserimento del numero che lo identifica
            /// </summary>
            /// <remarks></remarks>
            RichiedeNumero = 2,

            /// <summary>
            /// Il documento ha un campo "Rilasciato Da"
            /// </summary>
            /// <remarks></remarks>
            HaRilasciatoDa = 4,

            /// <summary>
            /// Il documento richiede di compilare il campo "Rilasciato Da"
            /// </summary>
            /// <remarks></remarks>
            RichiedeRilasciatoDa = 8,

            /// <summary>
            /// Il documento ha il campo "Data Inizio"
            /// </summary>
            /// <remarks></remarks>
            HaDataInizio = 16,

            /// <summary>
            /// Il documento richiede di compilare il campo "Data Inizio"
            /// </summary>
            /// <remarks></remarks>
            RichiedeDataInizio = 32,


            /// <summary>
            /// Il documento ha il campo "Data Fine"
            /// </summary>
            /// <remarks></remarks>
            HaDataFine = 64,

            /// <summary>
            /// Il documento richiede di compulare il campo "Data Fine"
            /// </summary>
            /// <remarks></remarks>
            RichiedeDataFine = 128
        }


        /// <summary>
        /// Rappresenta un documento che è possibile caricare
        /// </summary>
        /// <remarks></remarks>
        public class DocumentoCaricato 
            : minidom.Databases.DBObject, IComparable, IComparable<DocumentoCaricato>
        {
            private string m_Nome;                // Nome del documento
            private string m_Descrizione;         // Descrizione del documento
            private string m_TemplatePath;        // URL del modello utilizzato per creare il documento destinazione
            private bool m_Uploadable;         // Se vero indica che il documento può essere caricato sul sistema
            private bool m_ValiditaLimitata;   // Se vero indica che si tratta di un documento la cui validità è limitata entro un certo intervallo
            private bool m_LegatoAlContesto;
            private string m_Categoria;
            private string m_SottoCategoria;

            /// <summary>
            /// Costruttore
            /// </summary>
            public DocumentoCaricato()
            {
                m_Nome = DMD.Strings.vbNullString;
                m_Descrizione = DMD.Strings.vbNullString;
                m_TemplatePath = DMD.Strings.vbNullString;
                m_Uploadable = false;
                m_ValiditaLimitata = false;
                m_LegatoAlContesto = false;
                m_Flags = (int) DocFlags.None;
                m_Categoria = "";
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.GDE;
            }

            /// <summary>
            /// Restituisce o imposta il nome del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la categoria del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Categoria
            {
                get
                {
                    return m_Categoria;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Categoria;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Categoria = value;
                    DoChanged("Categoria", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la categoria secondaria del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SottoCategoria
            {
                get
                {
                    return m_SottoCategoria;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_SottoCategoria;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SottoCategoria = value;
                    DoChanged("Sottocategoria", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta la descrizione del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la URL del modello utilizzato per creare il documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TemplatePath
            {
                get
                {
                    return m_TemplatePath;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_TemplatePath;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TemplatePath = value;
                    DoChanged("TemplatePath", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il documento può essere caricato a sistema
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Uploadable
            {
                get
                {
                    return m_Uploadable;
                }

                set
                {
                    if (m_Uploadable == value)
                        return;
                    m_Uploadable = value;
                    DoChanged("Uploadable", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il documento ha una scadenza
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool ValiditaLimitata
            {
                get
                {
                    return m_ValiditaLimitata;
                }

                set
                {
                    if (m_ValiditaLimitata == value)
                        return;
                    m_ValiditaLimitata = value;
                    DoChanged("ValiditaLimitata", value, !value);
                }
            }

            /// <summary>
            /// Il documento necessita di un contesto
            /// </summary>
            public bool LegatoAlContesto
            {
                get
                {
                    return m_LegatoAlContesto;
                }

                set
                {
                    if (m_LegatoAlContesto == value)
                        return;
                    m_LegatoAlContesto = value;
                    DoChanged("LegatoAlContesto", value, !value);
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public new DocFlags Flags
            {
                get
                {
                    return (DocFlags) base.Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (DMD.RunTime.EQ(oldValue , value))
                        return;
                    m_Flags = (int)value;
                    this.DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Testa il flag
            /// </summary>
            /// <param name="f"></param>
            /// <returns></returns>
            public bool GetFlag(DocFlags f)
            {
                return DMD.RunTime.TestFlag(this.Flags, f);
            }

            /// <summary>
            /// Imposta il flag
            /// </summary>
            /// <param name="f"></param>
            /// <param name="value"></param>
            public void SetFlag(DocFlags f, bool value)
            {
                this.Flags = DMD.RunTime.SetFlag(this.Flags, f, value);
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Documenti";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", this.m_Nome);
                m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                m_TemplatePath = reader.Read("Template", this.m_TemplatePath);
                m_Uploadable = reader.Read("Uploadable", this.m_Uploadable);
                m_ValiditaLimitata = reader.Read("ValiditaLimitata", this.m_ValiditaLimitata);
                m_LegatoAlContesto = reader.Read("LegatoAlContesto", this.m_LegatoAlContesto);
                m_Categoria = reader.Read("cat", this.m_Categoria);
                m_SottoCategoria = reader.Read("sotto_cat", this.m_SottoCategoria);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("Template", m_TemplatePath);
                writer.Write("Uploadable", m_Uploadable);
                writer.Write("ValiditaLimitata", m_ValiditaLimitata);
                writer.Write("LegatoAlContesto", m_LegatoAlContesto);
                writer.Write("cat", m_Categoria);
                writer.Write("sotto_cat", m_SottoCategoria);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("Template", typeof(string), 255);
                c = table.Fields.Ensure("Uploadable", typeof(bool), 1);
                c = table.Fields.Ensure("ValiditaLimitata", typeof(bool), 1);
                c = table.Fields.Ensure("LegatoAlContesto", typeof(bool), 1);
                c = table.Fields.Ensure("cat", typeof(string), 255);
                c = table.Fields.Ensure("sotto_cat", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxDescrizione", new string[] { "Descrizione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTemplate", new string[] { "Template" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxFlags", new string[] { "Uploadable", "ValiditaLimitata", "LegatoAlContesto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCategoria", new string[] { "cat", "sotto_cat" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Nome;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Nome);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("TemplatePath", m_TemplatePath);
                writer.WriteAttribute("Uploadable", m_Uploadable);
                writer.WriteAttribute("ValiditaLimitata", m_ValiditaLimitata);
                writer.WriteAttribute("LegatoAlContesto", m_LegatoAlContesto);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("Categoria", m_Categoria);
                writer.WriteAttribute("SottoCategoria", m_SottoCategoria);
                base.XMLSerialize(writer);
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
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "TemplatePath":
                        {
                            m_TemplatePath = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Uploadable":
                        {
                            m_Uploadable = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "ValiditaLimitata":
                        {
                            m_ValiditaLimitata = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "LegatoAlContesto":
                        {
                            m_LegatoAlContesto = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

              
                    case "Categoria":
                        {
                            m_Categoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SottoCategoria":
                        {
                            m_SottoCategoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((DocumentoCaricato)obj);
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual int CompareTo(DocumentoCaricato obj)
            {
                return DMD.Strings.Compare(m_Nome, obj.m_Nome, true);
            }
        }
    }
}