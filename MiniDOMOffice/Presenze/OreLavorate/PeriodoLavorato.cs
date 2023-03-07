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
        /// Periodo lavorato
        /// TODO Params e Flags in js
        /// </summary>
        [Serializable]
        public class PeriodoLavorato 
            : Databases.DBObjectPO, IComparable, IComparable<PeriodoLavorato>
        {
            private DateTime m_Periodo;                       // Periodo di riferimento (data completa se il periodo fa riferimento ad una giornata lavorativa
            private DateTime? m_DataInizio;                    // Ora di ingresso
            private DateTime? m_DataFine;                      // Ora di uscita
            private int m_IDOperatore;                // ID dell'operatore
            [NonSerialized] private Sistema.CUser m_Operatore;                    // Operatore
            private string m_NomeOperatore;               // Nome dell'operatore
            private int m_IDTurno;                    // ID del turno applicato
            [NonSerialized] private Turno m_Turno;                        // Turno applicato
            private string m_NomeTurno;                   // Nome del turno applicato
            private double m_DeltaIngresso;               // Differenza, in ore, tra l'orario di ingresso del turno e l'oraio di ingresso registrato
            private double m_DeltaUscita;                 // Differenza, in ore, tra l'orario di uscita del turno e l'oraio di uscita registrato
            private double m_OreLavorateTurno;           // Se gli ingressi e le uscite sono nel margine di tolleranza del turno le ore lavorate restituite sono quelle del tuorno
            private double m_OreLavorateEffettive;       // Differenza in ore tra l'ora di uscita e l'ora di ingresso
            private decimal? m_RetribuzioneCalcolata;
            private decimal? m_RetribuzioneErogabile;
            private decimal? m_RetribuzioneErogata;
            private DateTime? m_DataVerifica;
            private int m_IDVerificatoDa;
            [NonSerialized] private Sistema.CUser m_VerificatoDa;
            private string m_NomeVerificatoDa;
            private string m_NoteVerifica;

            /// <summary>
            /// Costruttore
            /// </summary>
            public PeriodoLavorato()
            {
                m_Periodo = DMD.DateUtils.Now();
                m_DataInizio = default;
                m_DataFine = default; // Ora di uscita
                m_IDOperatore = 0; // ID dell'operatore
                m_Operatore = null; // Operatore
                m_NomeOperatore = ""; // Nome dell'operatore
                m_IDTurno = 0; // ID del turno applicato
                m_Turno = null; // Turno applicato
                m_NomeTurno = ""; // Nome del turno applicato
                m_DeltaIngresso = 0.0d; // Differenza, in ore, tra l'orario di ingresso del turno e l'oraio di ingresso registrato
                m_DeltaUscita = 0.0d; // Differenza, in ore, tra l'orario di uscita del turno e l'oraio di uscita registrato
                m_OreLavorateTurno = 0.0d;
                m_OreLavorateEffettive = 0.0d;
                m_Flags = 0;
                m_RetribuzioneCalcolata = default;
                m_RetribuzioneErogabile = default;
                m_RetribuzioneErogata = default;
                m_DataVerifica = default;
                m_IDVerificatoDa = 0;
                m_VerificatoDa = null;
                m_NomeVerificatoDa = "";
                m_NoteVerifica = "";
            }

            /// <summary>
            /// Periodo di riferimento (data completa se il periodo fa riferimento ad una giornata lavorativa
            /// </summary>
            public DateTime Periodo                       
            {
                get
                {
                    return m_Periodo;
                }

                set
                {
                    var oldValue = m_Periodo;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_Periodo = value;
                    DoChanged("Periodo", value, oldValue);
                }
            }

            /// <summary>
            ///  Ora di ingresso
            /// </summary>
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
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            /// <summary>
            ///  Ora di uscita
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
            /// ID dell'operatore
            /// </summary>
            public int IDOperatore                
            {
                get
                {
                    return DBUtils.GetID(m_Operatore, m_IDOperatore);
                }

                set
                {
                    int oldValue = IDOperatore;
                    if (oldValue == value)
                        return;
                    m_IDOperatore = value;
                    m_Operatore = null;
                    DoChanged("IDOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// Operatore
            /// </summary>
            public Sistema.CUser Operatore                  
            {
                get
                {
                    if (m_Operatore is null)
                        m_Operatore = Sistema.Users.GetItemById(m_IDOperatore);
                    return m_Operatore;
                }

                set
                {
                    var oldValue = Operatore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDOperatore = DBUtils.GetID(value, 0);
                    m_Operatore = value;
                    m_NomeOperatore = "";
                    if (value is object)
                        m_NomeOperatore = value.Nominativo;
                    DoChanged("Operatore", value, oldValue);
                }
            }

            /// <summary>
            ///  Nome dell'operatore
            /// </summary>
            public string NomeOperatore                
            {
                get
                {
                    return m_NomeOperatore;
                }

                set
                {
                    string oldValue = m_NomeOperatore;
                    value = DMD.Strings.Trim(value);
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_NomeOperatore = value;
                    DoChanged("NomeOperatore", value, oldValue);
                }
            }

            /// <summary>
            ///  ID del turno applicato
            /// </summary>
            public int IDTurno                     
            {
                get
                {
                    return DBUtils.GetID(m_Turno, m_IDTurno);
                }

                set
                {
                    int oldValue = IDTurno;
                    if (oldValue == value)
                        return;
                    m_IDTurno = value;
                    m_Turno = null;
                    DoChanged("IDTurno", value, oldValue);
                }
            }

            /// <summary>
            /// Turno applicato
            /// </summary>
            public Turno Turno                        
            {
                get
                {
                    if (m_Turno is null)
                        m_Turno = Turni.GetItemById(m_IDTurno);
                    return m_Turno;
                }

                set
                {
                    var oldValue = Turno;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_IDTurno = DBUtils.GetID(value, 0);
                    m_Turno = value;
                    m_NomeTurno = "";
                    if (value is object)
                        m_NomeTurno = value.Nome;
                    DoChanged("Turno", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del turno applicato
            /// </summary>
            public string NomeTurno                   
            {
                get
                {
                    return m_NomeTurno;
                }

                set
                {
                    string oldValue = m_NomeTurno;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeTurno = value;
                    DoChanged("NomeTurno", value, oldValue);
                }
            }

            /// <summary>
            /// Tolleranza sull'ora di ingresso
            /// </summary>
            public double DeltaIngresso
            {
                get
                {
                    return m_DeltaIngresso;
                }

                set
                {
                    double oldValue = m_DeltaIngresso;
                    if (oldValue == value)
                        return;
                    m_DeltaIngresso = value;
                    DoChanged("DeltaIngresso", value, oldValue);
                }
            }

            /// <summary>
            /// Differenza, in ore, tra l'orario di uscita del turno e l'oraio di uscita registrato
            /// </summary>
            public double DeltaUscita                 
            {
                get
                {
                    return m_DeltaUscita;
                }

                set
                {
                    double oldValue = m_DeltaUscita;
                    if (oldValue == value)
                        return;
                    m_DeltaUscita = value;
                    DoChanged("DeltaUscita", value, oldValue);
                }
            }

            /// <summary>
            /// Se gli ingressi e le uscite sono nel margine di tolleranza del turno le ore lavorate restituite sono quelle del tuorno
            /// </summary>
            public double OreLavorateTurno           
            {
                get
                {
                    return m_OreLavorateTurno;
                }

                set
                {
                    double oldValue = m_OreLavorateTurno;
                    if (oldValue == value)
                        return;
                    m_OreLavorateTurno = value;
                    DoChanged("OreLavorateTurno", value, oldValue);
                }
            }

            /// <summary>
            /// Differenza in ore tra l'ora di uscita e l'ora di ingresso
            /// </summary>
            public double OreLavorateEffettive       
            {
                get
                {
                    return m_OreLavorateEffettive;
                }

                set
                {
                    double oldValue = m_OreLavorateEffettive;
                    if (oldValue == value)
                        return;
                    m_OreLavorateEffettive = value;
                    DoChanged("OreLavorateEffettive", value, oldValue);
                }
            }
              
            /// <summary>
            /// Retribuzione calcolata
            /// </summary>
            public decimal? RetribuzioneCalcolata
            {
                get
                {
                    return m_RetribuzioneCalcolata;
                }

                set
                {
                    var oldValue = m_RetribuzioneCalcolata;
                    if (oldValue == value == true)
                        return;
                    m_RetribuzioneCalcolata = value;
                    DoChanged("RetribuzioneCalcolata", value, oldValue);
                }
            }

            /// <summary>
            /// Retribuzione erogabile
            /// </summary>
            public decimal? RetribuzioneErogabile
            {
                get
                {
                    return m_RetribuzioneErogabile;
                }

                set
                {
                    var oldValue = m_RetribuzioneErogabile;
                    if (oldValue == value == true)
                        return;
                    m_RetribuzioneErogabile = value;
                    DoChanged("RetribuzioneErogabile", value, oldValue);
                }
            }

            /// <summary>
            /// Retribuzione erogata
            /// </summary>
            public decimal? RetribuzioneErogata
            {
                get
                {
                    return m_RetribuzioneErogata;
                }

                set
                {
                    var oldValue = m_RetribuzioneErogata;
                    if (oldValue == value == true)
                        return;
                    m_RetribuzioneErogata = value;
                    DoChanged("RetribuzioneErogata", value, oldValue);
                }
            }

            /// <summary>
            /// Data di verifica
            /// </summary>
            public DateTime? DataVerifica
            {
                get
                {
                    return m_DataVerifica;
                }

                set
                {
                    var oldValue = m_DataVerifica;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataVerifica = value;
                    DoChanged("DataVerifica", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'operatore che ha verificato 
            /// </summary>
            public int IDVerificatoDa
            {
                get
                {
                    return DBUtils.GetID(m_VerificatoDa, m_IDVerificatoDa);
                }

                set
                {
                    int oldValue = IDVerificatoDa;
                    if (oldValue == value)
                        return;
                    m_IDVerificatoDa = value;
                    m_VerificatoDa = null;
                    DoChanged("IDVerificatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Operatore che ha verificato l'orario
            /// </summary>
            public Sistema.CUser VerificatoDa
            {
                get
                {
                    if (m_VerificatoDa is null)
                        m_VerificatoDa = Sistema.Users.GetItemById(m_IDVerificatoDa);
                    return m_VerificatoDa;
                }

                set
                {
                    var oldValue = VerificatoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_VerificatoDa = value;
                    m_IDVerificatoDa = DBUtils.GetID(value, 0);
                    m_NomeVerificatoDa = "";
                    if (value is object)
                        m_NomeVerificatoDa = value.Nominativo;
                    DoChanged("VerificatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Nome dell'operatore che ha verificato
            /// </summary>
            public string NomeVerificatoDa
            {
                get
                {
                    return m_NomeVerificatoDa;
                }

                set
                {
                    string oldValue = m_NomeVerificatoDa;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeVerificatoDa = value;
                    DoChanged("NomeVerificatoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Note di verifica
            /// </summary>
            public string NoteVerifica
            {
                get
                {
                    return m_NoteVerifica;
                }

                set
                {
                    string oldValue = m_NoteVerifica;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NoteVerifica = value;
                    DoChanged("NoteVerifica", value, oldValue);
                }
            }
            
            /// <summary>
            /// Disciminator
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.PeriodiLavorati;
            }

            public override Sistema.CModule GetModule()
            {
                return PeriodiLavorati.Module;
            }

            public override string GetTableName()
            {
                return "tbl_OfficePeriodiLavorati";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Periodo = reader.Read("Periodo", this. m_Periodo);
                this.m_DataInizio = reader.Read("DataInizio", this.m_DataInizio);
                this.m_DataFine = reader.Read("DataFine", this.m_DataFine);
                this.m_IDOperatore = reader.Read("IDOperatore", this.m_IDOperatore);
                this.m_NomeOperatore = reader.Read("NomeOperatore", this.m_NomeOperatore);
                this.m_IDTurno = reader.Read("IDTurno", this.m_IDTurno);
                this.m_NomeTurno = reader.Read("NomeTurno", this.m_NomeTurno);
                this.m_DeltaIngresso = reader.Read("DeltaIngresso", this.m_DeltaIngresso);
                this.m_DeltaUscita = reader.Read("DeltaUscita", this.m_DeltaUscita);
                this.m_OreLavorateTurno = reader.Read("OreLavorateTurno", this.m_OreLavorateTurno);
                this.m_OreLavorateEffettive = reader.Read("OreLavorateEffettive", this.m_OreLavorateEffettive);
                this.m_RetribuzioneCalcolata = reader.Read("RetribuzioneCalcolata", this.m_RetribuzioneCalcolata);
                this.m_RetribuzioneErogabile = reader.Read("RetribuzioneErogabile", this.m_RetribuzioneErogabile);
                this.m_RetribuzioneErogata = reader.Read("RetribuzioneErogata", this.m_RetribuzioneErogata);
                this.m_DataVerifica = reader.Read("DataVerifica", this.m_DataVerifica);
                this.m_IDVerificatoDa = reader.Read("IDVerificatoDa", this.m_IDVerificatoDa);
                this.m_NomeVerificatoDa = reader.Read("NomeVerificatoDa", this.m_NomeVerificatoDa);
                this.m_NoteVerifica = reader.Read("NoteVerifica", this.m_NoteVerifica);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Periodo", m_Periodo);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("IDOperatore", IDOperatore);
                writer.Write("NomeOperatore", m_NomeOperatore);
                writer.Write("IDTurno", IDTurno);
                writer.Write("NomeTurno", m_NomeTurno);
                writer.Write("DeltaIngresso", m_DeltaIngresso);
                writer.Write("DeltaUscita", m_DeltaUscita);
                writer.Write("OreLavorateTurno", m_OreLavorateTurno);
                writer.Write("OreLavorateEffettive", m_OreLavorateEffettive);
                writer.Write("RetribuzioneCalcolata", m_RetribuzioneCalcolata);
                writer.Write("RetribuzioneErogabile", m_RetribuzioneErogabile);
                writer.Write("RetribuzioneErogata", m_RetribuzioneErogata);
                writer.Write("DataVerifica", m_DataVerifica);
                writer.Write("IDVerificatoDa", IDVerificatoDa);
                writer.Write("NomeVerificatoDa", m_NomeVerificatoDa);
                writer.Write("NoteVerifica", m_NoteVerifica);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Periodo", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataInizio", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataFine", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDOperatore", typeof(int), 1);
                c = table.Fields.Ensure("NomeOperatore", typeof(string), 255);
                c = table.Fields.Ensure("IDTurno", typeof(int), 1);
                c = table.Fields.Ensure("NomeTurno", typeof(string), 255);
                c = table.Fields.Ensure("DeltaIngresso", typeof(double), 1);
                c = table.Fields.Ensure("DeltaUscita", typeof(double), 1);
                c = table.Fields.Ensure("OreLavorateTurno", typeof(double), 1);
                c = table.Fields.Ensure("OreLavorateEffettive", typeof(double), 1);
                c = table.Fields.Ensure("RetribuzioneCalcolata", typeof(decimal), 1);
                c = table.Fields.Ensure("RetribuzioneErogabile", typeof(decimal), 1);
                c = table.Fields.Ensure("RetribuzioneErogata", typeof(decimal), 1);
                c = table.Fields.Ensure("DataVerifica", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDVerificatoDa", typeof(int), 1);
                c = table.Fields.Ensure("NomeVerificatoDa", typeof(string), 255);
                c = table.Fields.Ensure("NoteVerifica", typeof(string), 0);
                 
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxPeriodo", new string[] { "Periodo", "DataInizio", "DataFine" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxOperatore", new string[] { "IDOperatore", "NomeOperatore" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTurno", new string[] { "IDTurno", "NomeTurno" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDelta", new string[] { "DeltaIngresso", "DeltaUscita" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxOre", new string[] { "OreLavorateEffettive", "OreLavorateTurno" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRetribuzione", new string[] { "RetribuzioneCalcolata", "RetribuzioneErogabile", "RetribuzioneErogata" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxVerifica", new string[] { "DataVerifica", "IDVerificatoDa", "NomeVerificatoDa" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNoteVerifica", new string[] { "NoteVerifica" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Periodo", m_Periodo);
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", m_NomeOperatore);
                writer.WriteAttribute("IDTurno", IDTurno);
                writer.WriteAttribute("NomeTurno", m_NomeTurno);
                writer.WriteAttribute("DeltaIngresso", m_DeltaIngresso);
                writer.WriteAttribute("DeltaUscita", m_DeltaUscita);
                writer.WriteAttribute("OreLavorateTurno", m_OreLavorateTurno);
                writer.WriteAttribute("OreLavorateEffettive", m_OreLavorateEffettive);
                writer.WriteAttribute("RetribuzioneCalcolata", m_RetribuzioneCalcolata);
                writer.WriteAttribute("RetribuzioneErogabile", m_RetribuzioneErogabile);
                writer.WriteAttribute("RetribuzioneErogata", m_RetribuzioneErogata);
                writer.WriteAttribute("DataVerifica", m_DataVerifica);
                writer.WriteAttribute("IDVerificatoDa", IDVerificatoDa);
                writer.WriteAttribute("NomeVerificatoDa", m_NomeVerificatoDa);
                base.XMLSerialize(writer);                
                writer.WriteTag("NoteVerifica", m_NoteVerifica);
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
                    case "Periodo":
                        {
                            m_Periodo = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
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

                    case "IDOperatore":
                        {
                            m_IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatore":
                        {
                            m_NomeOperatore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDTurno":
                        {
                            m_IDTurno = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeTurno":
                        {
                            m_NomeTurno = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DeltaIngresso":
                        {
                            m_DeltaIngresso = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DeltaUscita":
                        {
                            m_DeltaUscita = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "OreLavorateTurno":
                        {
                            m_OreLavorateTurno = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "OreLavorateEffettive":
                        {
                            m_OreLavorateEffettive = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                   
                    case "RetribuzioneCalcolata":
                        {
                            m_RetribuzioneCalcolata = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "RetribuzioneErogabile":
                        {
                            m_RetribuzioneErogabile = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "RetribuzioneErogata":
                        {
                            m_RetribuzioneErogata = (decimal?)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DataVerifica":
                        {
                            m_DataVerifica = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDVerificatoDa":
                        {
                            m_IDVerificatoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeVerificatoDa":
                        {
                            m_NomeVerificatoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
 
                    case "NoteVerifica":
                        {
                            m_NoteVerifica = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(PeriodoLavorato obj)
            {
                int ret = DMD.DateUtils.Compare(m_Periodo, obj.m_Periodo);
                if (ret == 0)
                    ret = DMD.DateUtils.Compare(m_DataInizio, obj.m_DataInizio);
                if (ret == 0)
                    ret = DMD.DateUtils.Compare(m_DataFine, obj.m_DataFine);
                if (ret == 0)
                    ret = DMD.Strings.Compare(NomeOperatore, obj.NomeOperatore, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((PeriodoLavorato)obj);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.Periodo, " - " , this.NomeTurno);
            }

            /// <summary>
            /// Restitusice il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.Periodo);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is PeriodoLavorato) && this.Equals((PeriodoLavorato)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(PeriodoLavorato obj)
            {
                return base.Equals(obj)
                    && DMD.DateUtils.EQ(this.m_Periodo, obj.m_Periodo)
                    && DMD.DateUtils.EQ(this.m_DataInizio, obj.m_DataInizio)
                    && DMD.DateUtils.EQ(this.m_DataFine, obj.m_DataFine)
                    && DMD.Integers.EQ(this.m_IDOperatore, obj.m_IDOperatore)
                    && DMD.Strings.EQ(this.m_NomeOperatore, obj.m_NomeOperatore)
                    && DMD.Integers.EQ(this.m_IDTurno, obj.m_IDTurno)
                    && DMD.Strings.EQ(this.m_NomeTurno, obj.m_NomeTurno)
                    && DMD.Doubles.EQ(this.m_DeltaIngresso, obj.m_DeltaIngresso)
                    && DMD.Doubles.EQ(this.m_DeltaUscita, obj.m_DeltaUscita)
                    && DMD.Doubles.EQ(this.m_OreLavorateTurno, obj.m_OreLavorateTurno)
                    && DMD.Doubles.EQ(this.m_OreLavorateEffettive, obj.m_OreLavorateEffettive)
                    && DMD.Decimals.EQ(this.m_RetribuzioneCalcolata, obj.m_RetribuzioneCalcolata)
                    && DMD.Decimals.EQ(this.m_RetribuzioneErogabile, obj.m_RetribuzioneErogabile)
                    && DMD.Decimals.EQ(this.m_RetribuzioneErogata, obj.m_RetribuzioneErogata)
                    && DMD.DateUtils.EQ(this.m_DataVerifica, obj.m_DataVerifica)
                    && DMD.Integers.EQ(this.m_IDVerificatoDa, obj.m_IDVerificatoDa)
                    && DMD.Strings.EQ(this.m_NomeVerificatoDa, obj.m_NomeVerificatoDa)
                    && DMD.Strings.EQ(this.m_NoteVerifica, obj.m_NoteVerifica)
                    ;
            }
        }
    }
}