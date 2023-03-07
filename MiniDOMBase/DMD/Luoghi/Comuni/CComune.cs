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
        /// Oggetto che rappresenta un comune
        /// </summary>
        [Serializable]
        public class CComune 
            : LuogoISTAT
        {
            private string m_SantoPatrono;
            private string m_GiornoFestivo;
            private string m_CAP;
            private string m_Prefisso;
            private string m_Provincia;
            private string m_Sigla;
            private string m_Regione;
            private CIntervalliCAP m_IntervalliCAP;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CComune()
            {
                m_SantoPatrono = "";
                m_GiornoFestivo = "";
                m_CAP = "";
                m_Prefisso = "";
                m_Provincia = "";
                m_Sigla = "";
                m_Regione = "";
                m_IntervalliCAP = null;
            }

            /// <summary>
            /// Elenco di intervalli di codici CAP assegnati al comune
            /// </summary>
            public CIntervalliCAP IntervalliCAP
            {
                get
                {
                    if (m_IntervalliCAP is null)
                        m_IntervalliCAP = new CIntervalliCAP(this);
                    return m_IntervalliCAP;
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Luoghi.Comuni; //.Module;
            }

            /// <summary>
            /// Restituisce o imposta la città e provincia es. Teggiano (SA)
            /// </summary>
            public string CittaEProvincia
            {
                get
                {
                    var ret = new System.Text.StringBuilder(128);
                    ret.Append(this.Nome);
                    if (!string.IsNullOrEmpty(this.m_Sigla))
                    {
                        ret.Append(" (");
                        ret.Append(this.m_Sigla);
                        ret.Append(")");
                    }
                    return ret.ToString();
                }

                set
                {
                    string oldValue = CittaEProvincia;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    Nome = Luoghi.GetComune(value);
                    m_Sigla = Luoghi.GetProvincia(value);
                    DoChanged("CittaEProvincia", value, oldValue);
                }
            }

              
            /// <summary>
            /// Restituisce o imposta il nome del santo patrono (es. San Cono)
            /// </summary>
            public string SantoPatrono
            {
                get
                {
                    return m_SantoPatrono;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_SantoPatrono;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SantoPatrono = value;
                    DoChanged("SantoPatrono", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che rappresenta la festività (es. 3 Giugno)
            /// </summary>
            public string GiornoFestivo
            {
                get
                {
                    return m_GiornoFestivo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_GiornoFestivo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_GiornoFestivo = value;
                    DoChanged("GiornoFestivo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il CAP del comune
            /// </summary>
            public string CAP
            {
                get
                {
                    return m_CAP;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_CAP;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CAP = value;
                    DoChanged("CAP", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il prefisso telefonico principale del comune
            /// </summary>
            public string Prefisso
            {
                get
                {
                    return m_Prefisso;
                }

                set
                {
                    value = Sistema.Formats.ParsePhoneNumber(value);
                    string oldValue = m_Prefisso;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Prefisso = value;
                    DoChanged("Prefisso", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la provincia di appartenenza del comune
            /// </summary>
            public string Provincia
            {
                get
                {
                    return m_Provincia;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Provincia;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Provincia = value;
                    DoChanged("Provincia", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la sigla della provincia di appartenenza del comune
            /// </summary>
            public string Sigla
            {
                get
                {
                    return m_Sigla;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Sigla;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Sigla = value;
                    DoChanged("Sigla", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della regione 
            /// </summary>
            public string Regione
            {
                get
                {
                    return m_Regione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Regione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Regione = value;
                    DoChanged("Regione", value, oldValue);
                }
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Luoghi_Comuni";
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Luoghi.Database;
            //}

            /// <summary>
            /// Restituisce true se l'oggetto è stato modificato
            /// </summary>
            /// <returns></returns>

            public override bool IsChanged()
            {
                bool ret = base.IsChanged();
                if (ret == false && m_IntervalliCAP is object)
                    ret = DBUtils.IsChanged(m_IntervalliCAP);
                return ret;
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                if (m_IntervalliCAP is object)
                    m_IntervalliCAP.Save(this.GetConnection(), force);
            }

            /// <summary>
            /// Elimina
            /// </summary>
            /// <param name="force"></param>
            public override void Delete(bool force = false)
            {
                this.IntervalliCAP.Delete(this.GetConnection(), force);
                base.Delete(force);
            }
             
            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_SantoPatrono = reader.Read("SantoPatrono", m_SantoPatrono);
                m_GiornoFestivo = reader.Read("GiornoFestivo", m_GiornoFestivo);
                m_CAP = reader.Read("CAP", m_CAP);
                m_Prefisso = reader.Read("Prefisso", m_Prefisso);
                m_Provincia = reader.Read("Provincia", m_Provincia);
                m_Sigla = reader.Read("Sigla", m_Sigla);
                m_Regione = reader.Read("Regione", m_Regione);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("SantoPatrono", m_SantoPatrono);
                writer.Write("GiornoFestivo", m_GiornoFestivo);
                writer.Write("CAP", m_CAP);
                writer.Write("Prefisso", m_Prefisso);
                writer.Write("Provincia", m_Provincia);
                writer.Write("Sigla", m_Sigla);
                writer.Write("Regione", m_Regione);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema del db
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("SantoPatrono", typeof(string), 255);
                c = table.Fields.Ensure("GiornoFestivo", typeof(string), 255);
                c = table.Fields.Ensure("CAP", typeof(string), 255);
                c = table.Fields.Ensure("Prefisso", typeof(string), 255);
                c = table.Fields.Ensure("Provincia", typeof(string), 255);
                c = table.Fields.Ensure("Sigla", typeof(string), 255);
                c = table.Fields.Ensure("Regione", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                //var c = table.Constraints.Ensure("idxAbitanti", new string[] {"NomeAbitanti", "NumeroAbitanti" }, DBFieldConstraintFlags.None);
                var c = table.Constraints.Ensure("idxSanto", new string[] { "SantoPatrono", "GiornoFestivo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCAP", new string[] { "CAP", "Sigla", "Provincia" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRegione", new string[] { "Regione", "Prefisso" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                var ret = new System.Text.StringBuilder();
                ret.Append(Nome);
                if (!string.IsNullOrEmpty(m_Provincia))
                {
                    ret.Append(" (");
                    ret.Append(m_Provincia);
                    ret.Append(")");
                }

                return ret.ToString();
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Provincia, base.GetHashCode());
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(LuogoISTAT obj)
            {
                return (obj is CComune) && this.Equals((CComune)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CComune obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_SantoPatrono, obj.m_SantoPatrono)
                    && DMD.Strings.EQ(this.m_GiornoFestivo, obj.m_GiornoFestivo)
                    && DMD.Strings.EQ(this.m_CAP, obj.m_CAP)
                    && DMD.Strings.EQ(this.m_Prefisso, obj.m_Prefisso)
                    && DMD.Strings.EQ(this.m_Provincia, obj.m_Provincia)
                    && DMD.Strings.EQ(this.m_Sigla, obj.m_Sigla)
                    && DMD.Strings.EQ(this.m_Regione, obj.m_Regione)
                    ;
                    //private CIntervalliCAP m_IntervalliCAP;
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("SantoPatrono", m_SantoPatrono);
                writer.WriteAttribute("GiornoFestivo", m_GiornoFestivo);
                writer.WriteAttribute("CAP", m_CAP);
                writer.WriteAttribute("Prefisso", m_Prefisso);
                writer.WriteAttribute("Provincia", m_Provincia);
                writer.WriteAttribute("Sigla", m_Sigla);
                writer.WriteAttribute("Regione", m_Regione);
                base.XMLSerialize(writer);
                writer.WriteTag("IntervalliCAP", IntervalliCAP);
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
                     
                    case "SantoPatrono":
                        {
                            m_SantoPatrono = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "GiornoFestivo":
                        {
                            m_GiornoFestivo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CAP":
                        {
                            m_CAP = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Prefisso":
                        {
                            m_Prefisso = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Provincia":
                        {
                            m_Provincia = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Sigla":
                        {
                            m_Sigla = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Regione":
                        {
                            m_Regione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IntervalliCAP":
                        {
                            m_IntervalliCAP = (CIntervalliCAP)fieldValue;
                            m_IntervalliCAP.SetComune(this);
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