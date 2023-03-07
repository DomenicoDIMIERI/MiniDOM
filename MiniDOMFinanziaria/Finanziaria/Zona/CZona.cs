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
using static minidom.Finanziaria;


namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Rappresenta una zona assegnata ad un agente
        /// </summary>
        [Serializable]
        public class CZona 
            : minidom.Databases.DBObjectPO
        {
            private string m_Nome;
            private CCollection<CComune> m_Comuni;
            private CCollection<CUser> m_Responsabili;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CZona()
            {
                m_Nome = DMD.Strings.vbNullString;
                m_Comuni = new CCollection<Anagrafica.CComune>();
                m_Responsabili = new CCollection<Sistema.CUser>();
            }

            /// <summary>
            /// Elenco dei comuni compresi nella zona
            /// </summary>
            public CCollection<Anagrafica.CComune> Comuni
            {
                get
                {
                    return m_Comuni;
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della zona
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
            /// Restituisce la collezione degli utente definiti come responsabili di zona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Sistema.CUser> Responsabili
            {
                get
                {
                    return m_Responsabili;
                }
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Finanziaria.Zone;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_CQSPDZone";
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("Comuni", GetIDStr(m_Comuni));
                writer.Write("Responsabili", GetIDStr(m_Responsabili));
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.LoadComuniIDs(reader.Read("Comuni", ""));
                this.LoadResponsabiliIDs(reader.Read("Responsabili", ""));
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("Comuni", typeof(string), 0);
                c = table.Fields.Ensure("Responsabili", typeof(string), 0);                 
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome" },  DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Comuni", GetIDStr(m_Comuni));
                writer.WriteAttribute("Responsabili", GetIDStr(m_Responsabili));
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

                    case "Comuni":
                        {
                            LoadComuniIDs(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Responsabili":
                        {
                            LoadResponsabiliIDs(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            private string GetIDStr(IEnumerable col)
            {
                var ret = new System.Text.StringBuilder(256);
                foreach (object c in col)
                {
                    if (ret.Length > 0)
                        ret.Append(",");
                    ret.Append(DBUtils.GetID((Databases.IDBObjectBase)c));
                }

                return ret.ToString();
            }

            private void LoadComuniIDs(string value)
            {
                m_Comuni.Clear();
                value = Strings.Trim(value);
                if (!string.IsNullOrEmpty(value))
                {
                    var items = Strings.Split(value, ",");
                    if (items is object)
                    {
                        foreach (string s in items)
                            m_Comuni.Add(Anagrafica.Luoghi.Comuni.GetItemById(DMD.Integers.ValueOf(s)));
                    }
                }
            }

            private void LoadResponsabiliIDs(string value)
            {
                m_Responsabili.Clear();
                value = Strings.Trim(value);
                if (!string.IsNullOrEmpty(value))
                {
                    var items = Strings.Split(value, ",");
                    if (items is object)
                    {
                        foreach (string s in items)
                            m_Responsabili.Add(Sistema.Users.GetItemById(DMD.Integers.ValueOf(s)));
                    }
                }
            }

            /// <summary>
            /// Restituisce la somma del numero di abitanti appartenenti ai comuni inclusi nella zona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int NumeroAbitanti
            {
                get
                {
                    int ret = 0;
                    for (int i = 0, loopTo = m_Comuni.Count - 1; i <= loopTo; i++)
                    {
                        var c = m_Comuni[i];
                        ret += c.NumeroAbitanti ?? 0;
                    }

                    return ret;
                }
            }

            /// <summary>
            /// Restituisce il numero dei comuni compresi nella zona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int NumeroComuni
            {
                get
                {
                    return m_Comuni.Count;
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_Nome;
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
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is CZona) && this.Equals((CZona)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CZona obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome, true);
            }
        }
    }
}