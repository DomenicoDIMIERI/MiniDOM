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
        /// Rappresenta la definizione di percorso
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class PercorsoDefinito
            : minidom.Databases.DBObjectPO
        {
            private string m_NomeGruppo;
            private string m_Nome;
            private string m_Descrizione;
            private bool m_Attivo;
            private float? m_DistanzaStimataMin;
            private float? m_DistanzaStimataMax;
            private float? m_TempoStimatoMin;
            private float? m_TempoStimatoMax;
            private LuoghiDefinitiPerPercorso m_Luoghi;

            /// <summary>
            /// Costruttore
            /// </summary>
            public PercorsoDefinito()
            {
                m_NomeGruppo = "";
                m_Nome = "";
                m_Descrizione = "";
                m_Attivo = true;
                m_DistanzaStimataMin = default;
                m_DistanzaStimataMax = default;
                m_TempoStimatoMin = default;
                m_TempoStimatoMax = default;
                m_Luoghi = null;
            }

            /// <summary>
            /// Nome del percorso
            /// </summary>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    string oldValue = m_Nome;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del gruppo di percorsi
            /// </summary>
            public string NomeGruppo
            {
                get
                {
                    return m_NomeGruppo;
                }

                set
                {
                    string oldValue = m_NomeGruppo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeGruppo = value;
                    DoChanged("NomeGruppo", value, oldValue);
                }
            }

            /// <summary>
            /// Descrizione del percorso
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
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce true se il percorso é attivo
            /// </summary>
            public bool Attivo
            {
                get
                {
                    return m_Attivo;
                }

                set
                {
                    if (m_Attivo == value)
                        return;
                    m_Attivo = value;
                    DoChanged("Attivo", value, !value);
                }
            }

            /// <summary>
            /// Lunghezza minima del percorso
            /// </summary>
            public float? DistanzaStimataMin
            {
                get
                {
                    return m_DistanzaStimataMin;
                }

                set
                {
                    var oldValue = m_DistanzaStimataMin;
                    if (oldValue == value == true)
                        return;
                    m_DistanzaStimataMin = value;
                    DoChanged("DistanzaStimataMin", value, oldValue);
                }
            }

            /// <summary>
            /// Lunghezza massima del percorso
            /// </summary>
            public float? DistanzaStimataMax
            {
                get
                {
                    return m_DistanzaStimataMax;
                }

                set
                {
                    var oldValue = m_DistanzaStimataMax;
                    if (oldValue == value == true)
                        return;
                    m_DistanzaStimataMax = value;
                    DoChanged("DistanzaStimataMax", value, oldValue);
                }
            }

            /// <summary>
            /// Tempo minimo stimato per completare il percorso
            /// </summary>
            public int? TempoStimatoMin
            {
                get
                {
                    return (int?)m_TempoStimatoMin;
                }

                set
                {
                    int? oldValue = (int?)m_TempoStimatoMin;
                    if (oldValue == value == true)
                        return;
                    m_TempoStimatoMin = value;
                    DoChanged("TempoStimatoMin", value, oldValue);
                }
            }

            /// <summary>
            /// Tempo massimo di percorrenza del percorso
            /// </summary>
            public int? TempoStimatoMax
            {
                get
                {
                    return (int?)m_TempoStimatoMax;
                }

                set
                {
                    int? oldValue = (int?)m_TempoStimatoMax;
                    if (oldValue == value == true)
                        return;
                    m_TempoStimatoMax = value;
                    DoChanged("TempoStimatoMax", value, oldValue);
                }
            }

            /// <summary>
            /// Luoghi da visitare nel percorso
            /// </summary>
            public LuoghiDefinitiPerPercorso Luoghi
            {
                get
                {
                    if (m_Luoghi is null)
                        m_Luoghi = new LuoghiDefinitiPerPercorso(this);
                    return m_Luoghi;
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
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.PercorsiDefiniti;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeDefPercorsi";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", m_Nome);
                m_NomeGruppo = reader.Read("NomeGruppo", m_NomeGruppo);
                m_Descrizione = reader.Read("Descrizione", m_Descrizione);
                m_Attivo = reader.Read("Attivo", m_Attivo);
                m_DistanzaStimataMin = reader.Read("DistanzaStimataMin", m_DistanzaStimataMin);
                m_DistanzaStimataMax = reader.Read("DistanzaStimataMax", m_DistanzaStimataMax);
                m_TempoStimatoMin = reader.Read("TempoStimatoMin", m_TempoStimatoMin);
                m_TempoStimatoMax = reader.Read("TempoStimatoMax", m_TempoStimatoMax);
                string tmp = reader.Read("Luoghi", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    m_Luoghi = (LuoghiDefinitiPerPercorso)DMD.XML.Utils.Serializer.Deserialize(tmp);
                    m_Luoghi.SetPercorso(this);
                }
                
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
                writer.Write("NomeGruppo", m_NomeGruppo);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("Attivo", m_Attivo);
                writer.Write("DistanzaStimataMin", m_DistanzaStimataMin);
                writer.Write("DistanzaStimataMax", m_DistanzaStimataMax);
                writer.Write("TempoStimatoMin", m_TempoStimatoMin);
                writer.Write("TempoStimatoMax", m_TempoStimatoMax);
                writer.Write("Luoghi", DMD.XML.Utils.Serializer.Serialize(Luoghi));
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("NomeGruppo", typeof(string), 255);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("Attivo", typeof(bool), 1);
                c = table.Fields.Ensure("DistanzaStimataMin", typeof(double), 1);
                c = table.Fields.Ensure("DistanzaStimataMax", typeof(double), 1);
                c = table.Fields.Ensure("TempoStimatoMin", typeof(double), 1);
                c = table.Fields.Ensure("TempoStimatoMax", typeof(double), 1);
                c = table.Fields.Ensure("Luoghi", typeof(string), 0);
                 
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxDescrizione", new string[] { "NomeGruppo", "Descrizione", "Attivo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDistanze", new string[] { "DistanzaStimataMin", "DistanzaStimataMax", "TempoStimatoMin", "TempoStimatoMax" }, DBFieldConstraintFlags.None);
                //c = table.Fields.Ensure("Luoghi", typeof(string), 0);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("NomeGruppo", m_NomeGruppo);
                writer.WriteAttribute("Attivo", m_Attivo);
                writer.WriteAttribute("DistanzaStimataMin", m_DistanzaStimataMin);
                writer.WriteAttribute("DistanzaStimataMax", m_DistanzaStimataMax);
                writer.WriteAttribute("TempoStimatoMin", m_TempoStimatoMin);
                writer.WriteAttribute("TempoStimatoMax", m_TempoStimatoMax);
                base.XMLSerialize(writer);
                writer.WriteTag("Descrizione", m_Descrizione);
                writer.WriteTag("Luoghi", Luoghi);
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
                    case "Nome": m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "NomeGruppo": m_NomeGruppo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "Descrizione": m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "Attivo": m_Attivo = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true; break;
                    case "DistanzaStimataMin": m_DistanzaStimataMin = (float?)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue)); break;
                    case "DistanzaStimataMax": m_DistanzaStimataMax = (float?)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue)); break;
                    case "TempoStimatoMin": m_TempoStimatoMin = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue); break;
                    case "TempoStimatoMax": m_TempoStimatoMax = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue); break;
                    case "Luoghi":
                        {
                            m_Luoghi = (LuoghiDefinitiPerPercorso)fieldValue;
                            m_Luoghi.SetPercorso(this);
                            break;
                        }

                    default:
                        base.SetFieldInternal(fieldName, fieldValue);
                        break;
                }
            }
             
        }
    }
}