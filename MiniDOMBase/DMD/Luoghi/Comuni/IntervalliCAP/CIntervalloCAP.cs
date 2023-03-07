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
        /// Rappresenta un intervallo di codici CAP assegnati ad un comune
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CIntervalloCAP 
            : minidom.Databases.DBObjectBase
        {
            private int m_Da;
            private int m_A;
            private int m_IDComune;
            [NonSerialized] private CComune m_Comune;
            

            /// <summary>
            /// Costruttore
            /// </summary>
            public CIntervalloCAP()
            {
                this.m_Da = 0;
                this.m_A = 0;
                this.m_IDComune = 0;
                this.m_Comune = null;
            }

            /// <summary>
            /// Restituisce o imposta l'estremo sinistro dell'intervallo
            /// </summary>
            public int Da
            {
                get
                {
                    return m_Da;
                }

                set
                {
                    int oldValue = m_Da;
                    if (oldValue == value)
                        return;
                    m_Da = value;
                    DoChanged("Da", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'estremo destro dell'intervallo
            /// </summary>
            public int A
            {
                get
                {
                    return m_A;
                }

                set
                {
                    int oldValue = m_A;
                    if (oldValue == value)
                        return;
                    m_A = value;
                    DoChanged("A", value, oldValue);
                }
            }

         

            /// <summary>
            /// Restituisce o imposta l'id del comune a cui appartiene l'intervallo
            /// </summary>
            public int IDComune
            {
                get
                {
                    return DBUtils.GetID(m_Comune, m_IDComune);
                }

                set
                {
                    int oldValue = IDComune;
                    if (oldValue == value)
                        return;
                    m_IDComune = value;
                    m_Comune = null;
                    DoChanged("IDComune", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il comune a cui appartiene l'intervallo
            /// </summary>
            public CComune Comune
            {
                get
                {
                    if (m_Comune is null)
                        m_Comune = Luoghi.Comuni.GetItemById(m_IDComune);
                    return m_Comune;
                }

                set
                {
                    var oldValue = m_Comune;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Comune = value;
                    m_IDComune = DBUtils.GetID(value, 0);
                    DoChanged("Comune", value, oldValue);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Luoghi.Comuni.IntervalliCAP; // Comuni.Module;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_LuoghiCAP";
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Luoghi.Database;
            //}

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_IDComune = reader.Read("IDComune", this.m_IDComune);
                this.m_Da = reader.Read("Da", this.m_Da);
                this.m_A = reader.Read("A", this.m_A);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDComune", this.IDComune);
                writer.Write("Da", this.m_Da);
                writer.Write("A", this.m_A);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDComune", typeof(int), 1);
                c = table.Fields.Ensure("Da", typeof(int), 1);
                c = table.Fields.Ensure("A", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxComune", new string[] { "IDComune" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDaA", new string[] { "Da", "A", "Flags" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("Parameters", typeof(string), 0);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                if (DMD.Integers.EQ(this.m_Da, this.m_A))
                    return DMD.Strings.CStr(this.m_Da);
                else
                    return DMD.Strings.JoinW("[" , this.m_Da , " - " , this.m_A , "]");
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDComune", this.IDComune);
                writer.WriteAttribute("Da", this.m_Da);
                writer.WriteAttribute("A", this.m_A);
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
                    case "IDComune":
                        {
                            m_IDComune = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Da":
                        {
                            m_Da = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "A":
                        {
                            m_A = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
            /// Imposta il comune
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetComune(CComune value)
            {
                m_Comune = value;
                m_IDComune = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Da, this.m_A, this.m_IDComune);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is CIntervalloCAP) && this.Equals((CIntervalloCAP)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CIntervalloCAP obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_Da, obj.m_Da)
                    && DMD.Integers.EQ(this.m_A, obj.m_A)
                    && DMD.Integers.EQ(this.m_IDComune, obj.m_IDComune)
                    && DMD.Integers.EQ(this.m_Flags, obj.m_Flags)
                    //&& CollectionUtils.EQ(this.Parameters, obj.Parameters)
                    ;
            }


        }
    }
}