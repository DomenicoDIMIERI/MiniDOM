using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;

namespace minidom
{
    public partial class Anagrafica
    {


        /// <summary>
        /// Rappresenta un luogo categorizzato dall'ISTAT
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public abstract class LuogoISTAT 
            : Luogo
        {
            private string m_CodiceCatasto;
            private string m_CodiceISTAT;

            /// <summary>
            /// Costruttore
            /// </summary>
            public LuogoISTAT()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="nome"></param>
            public LuogoISTAT(string nome) : base(nome)
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="nome"></param>
            /// <param name="codiceCatasto"></param>
            /// <param name="codiceISTAT"></param>
            public LuogoISTAT(string nome, string codiceCatasto, string codiceISTAT) : base(nome)
            {
                m_CodiceCatasto = DMD.Strings.Trim(codiceCatasto);
                m_CodiceISTAT = DMD.Strings.Trim(codiceISTAT);
            }

            ///// <summary>
            ///// Restituisce una stringa che rappresenta l'oggetto
            ///// </summary>
            ///// <returns></returns>
            //public override string ToString()
            //{
            //    return base.Nome;
            //}

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_CodiceISTAT, this.m_CodiceCatasto);
            }

            /// <summary>
            /// Restituisce  true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Luogo obj)
            {
                return (obj is LuogoISTAT) && this.Equals((LuogoISTAT)obj);
            }

            /// <summary>
            /// Restituisce  true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(LuogoISTAT obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_CodiceCatasto, obj.m_CodiceCatasto)
                    && DMD.Strings.EQ(this.m_CodiceISTAT, obj.m_CodiceISTAT)
                    ;
            }

            /// <summary>
            /// Restituisce o imposta il codice catastale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string CodiceCatasto
            {
                get
                {
                    return m_CodiceCatasto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_CodiceCatasto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceCatasto = value;
                    DoChanged("CodiceCatasto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il codice ISTAT
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string CodiceISTAT
            {
                get
                {
                    return m_CodiceISTAT;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_CodiceISTAT;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CodiceISTAT = value;
                    DoChanged("CodiceISTAT", value, oldValue);
                }
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_CodiceCatasto = reader.Read("Codice_Catasto", this.m_CodiceCatasto);
                this.m_CodiceISTAT = reader.Read("Codice_ISTAT", this.m_CodiceISTAT);
                return base.LoadFromRecordset(reader);
            }


            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Codice_Catasto", m_CodiceCatasto);
                writer.Write("Codice_ISTAT", m_CodiceISTAT);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Preapra lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Codice_Catasto", typeof(string), 255);
                c = table.Fields.Ensure("Codice_ISTAT", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxCodici", new string[] { "Codice_ISTAT", "Codice_Catasto" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("m_CodiceCatasto", m_CodiceCatasto);
                writer.WriteAttribute("Codice_ISTAT", m_CodiceISTAT);
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
                    case "m_CodiceCatasto":
                        {
                            this.m_CodiceCatasto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Codice_ISTAT":
                        {
                            this.m_CodiceISTAT = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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