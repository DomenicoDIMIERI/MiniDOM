using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;
using minidom;
using minidom.repositories;
using static minidom.Sistema;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Rappresenta una carta di credito
        /// </summary>
        [Serializable]
        public class CartaDiCredito 
            : Databases.DBObject, IMetodoDiPagamento
        {
            private string m_Name;
            private int m_IDContoCorrente;
            [NonSerialized] private ContoCorrente m_ContoCorrente;
            private string m_NomeConto;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            private string m_NumeroCarta;
            private string m_NomeIntestatario;
            private string m_CircuitoCarta;
            private string m_CodiceVerifica;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CartaDiCredito()
            {
                this.m_Name = "";
                this.m_IDContoCorrente = 0;
                this.m_ContoCorrente = null;
                this.m_NomeConto = "";
                this.m_DataInizio = default;
                this.m_DataFine = default;
                this.m_NumeroCarta = "";
                this.m_NomeIntestatario = "";
                this.m_CircuitoCarta = "";
                this.m_CodiceVerifica = "";
                this.m_Flags = 0;
            }

            

            /// <summary>
            /// Restituisce o imposta il nome della carta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Name
            {
                get
                {
                    return m_Name;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Name;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Name = value;
                    DoChanged("Name", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del conto corrente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDContoCorrente
            {
                get
                {
                    return DBUtils.GetID(m_ContoCorrente, m_IDContoCorrente);
                }

                set
                {
                    int oldValue = IDContoCorrente;
                    if (oldValue == value)
                        return;
                    m_IDContoCorrente = value;
                    m_ContoCorrente = null;
                    DoChanged("IDContoCorrente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il conto corrente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public ContoCorrente ContoCorrente
            {
                get
                {
                    if (m_ContoCorrente is null)
                        m_ContoCorrente = ContiCorrente.GetItemById(m_IDContoCorrente);
                    return m_ContoCorrente;
                }

                set
                {
                    var oldValue = m_ContoCorrente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_ContoCorrente = value;
                    m_IDContoCorrente = DBUtils.GetID(value, m_IDContoCorrente);
                    m_NomeConto = "";
                    if (value is object)
                        m_NomeConto = value.Nome;
                    DoChanged("ContoCorrente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del conto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeConto
            {
                get
                {
                    return m_NomeConto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeConto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeConto = value;
                    DoChanged("NomeConto", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il conto corrente
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetContoCorrente(ContoCorrente value)
            {
                m_ContoCorrente = value;
                m_IDContoCorrente = DBUtils.GetID(value, m_IDContoCorrente);
            }

            /// <summary>
            /// Restitusice o imposta il nome a cui è intestata la carta
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeIntestatario
            {
                get
                {
                    return m_NomeIntestatario;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeIntestatario;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeIntestatario = value;
                    DoChanged("NomeIntestatario", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di inizio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataInizio
            {
                get
                {
                    return m_DataInizio;
                }

                set
                {
                    var oldValue = m_DataInizio;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataInizio = value;
                    DoChanged("DataInzio", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice o imposta la data di scadenza
            /// </summary>
            public DateTime? DataFine
            {
                get
                {
                    return m_DataFine;
                }

                set
                {
                    var oldValue = m_DataFine;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero della carta
            /// </summary>
            public string NumeroCarta
            {
                get
                {
                    return m_NumeroCarta;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NumeroCarta;
                    m_NumeroCarta = value;
                    DoChanged("NumeroCarta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice di verifica della carta
            /// </summary>
            public string CodiceVerifica
            {
                get
                {
                    return m_CodiceVerifica;
                }

                set
                {
                    string oldValue = m_CodiceVerifica;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceVerifica = value;
                    DoChanged("CodiceVerifica", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il circuito (VISA, Maestro, Ecc..)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string CircuitoCarta
            {
                get
                {
                    return m_CircuitoCarta;
                }

                set
                {
                    string oldValue = m_CircuitoCarta;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CircuitoCarta = value;
                    DoChanged("CircuitoCarta", value, oldValue);
                }
            }

              

            /// <summary>
            /// Restituisce il repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.CarteDiCredito; //.Module;
            }

            /// <summary>
            /// Restituisce il discriminante del repository
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_CarteDiCredito";
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Strings.ConcatArray(this.m_CircuitoCarta , " : ", this.m_NumeroCarta);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_CircuitoCarta, this.m_NumeroCarta);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CartaDiCredito) && this.Equals((CartaDiCredito)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CartaDiCredito obj)
            {
                return base.Equals(obj)
                     && DMD.Strings.EQ(this.m_Name , obj.m_Name)
                     && DMD.Integers.EQ(this.IDContoCorrente, obj.IDContoCorrente) //m_ContoCorrente
                     && DMD.Strings.EQ(this.m_NomeConto, obj.m_NomeConto)
                     && DMD.Strings.EQ(this.m_NumeroCarta, obj.m_NumeroCarta)
                     && DMD.Strings.EQ(this.m_NomeIntestatario, obj.m_NomeIntestatario)
                     && DMD.Strings.EQ(this.m_CircuitoCarta, obj.m_CircuitoCarta)
                     && DMD.Strings.EQ(this.m_CodiceVerifica, obj.m_CodiceVerifica)
                     && DMD.DateUtils.EQ(this.m_DataInizio, obj.m_DataInizio)
                     && DMD.DateUtils.EQ(this.m_DataFine, obj.m_DataFine)
                      
                    ;
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Name = reader.Read("Name", m_Name);
                m_IDContoCorrente = reader.Read("IDContoCorrente", m_IDContoCorrente);
                m_NomeConto = reader.Read("NomeConto", m_NomeConto);
                m_DataInizio = reader.Read("DataInizio", m_DataInizio);
                m_DataFine = reader.Read("DataFine", m_DataFine);
                m_CircuitoCarta = reader.Read("CircuitoCarta", m_CircuitoCarta);
                m_CodiceVerifica = reader.Read("CodiceVerifica", m_CodiceVerifica);
                m_NomeIntestatario = reader.Read("NomeIntestatario", m_NomeIntestatario);
                m_NumeroCarta = reader.Read("NumeroCarta", m_NumeroCarta);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Name", m_Name);
                writer.Write("IDContoCorrente", IDContoCorrente);
                writer.Write("NomeConto", m_NomeConto);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("CircuitoCarta", m_CircuitoCarta);
                writer.Write("CodiceVerifica", m_CodiceVerifica);
                writer.Write("NomeIntestatario", m_NomeIntestatario);
                writer.Write("NumeroCarta", m_NumeroCarta);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Name", typeof(string), 255);
                c = table.Fields.Ensure("IDContoCorrente", typeof(int), 0);
                c = table.Fields.Ensure("NomeConto", typeof(string), 255);
                c = table.Fields.Ensure("DataInizio", typeof(DateTime), 0);
                c = table.Fields.Ensure("DataFine", typeof(DateTime), 0);
                c = table.Fields.Ensure("Flags", typeof(int), 0);
                c = table.Fields.Ensure("CircuitoCarta", typeof(string), 255);
                c = table.Fields.Ensure("CodiceVerifica", typeof(string), 255);
                c = table.Fields.Ensure("NomeIntestatario", typeof(string), 255);
                c = table.Fields.Ensure("NumeroCarta", typeof(string), 255);
            }

            /// <summary>
            /// Prepara gli indici
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "CircuitoCarta", "Name" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataInizio", "DataFine", "Flags" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPossessore", new string[] { "IDContoCorrente", "NomeIntestatario", "NomeConto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNumeri", new string[] { "NumeroCarta" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Name", m_Name);
                writer.WriteAttribute("IDContoCorrente", IDContoCorrente);
                writer.WriteAttribute("NomeConto", m_NomeConto);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("CircuitoCarta", m_CircuitoCarta);
                writer.WriteAttribute("CodiceVerifica", m_CodiceVerifica);
                writer.WriteAttribute("NomeIntestatario", m_NomeIntestatario);
                writer.WriteAttribute("NumeroCarta", m_NumeroCarta);
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
                    case "Name":
                        {
                            m_Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDContoCorrente":
                        {
                            m_IDContoCorrente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeConto":
                        {
                            m_NomeConto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataInizio":
                        {
                            m_DataInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFine":
                        {
                            m_DataFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }
 
                    case "CircuitoCarta":
                        {
                            m_CircuitoCarta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CodiceVerifica":
                        {
                            m_CodiceVerifica = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeIntestatario":
                        {
                            m_NomeIntestatario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NumeroCarta":
                        {
                            m_NumeroCarta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            string IMetodoDiPagamento.NomeMetodo
            {
                get
                {
                    return m_Name;
                }
            }
        }
    }
}