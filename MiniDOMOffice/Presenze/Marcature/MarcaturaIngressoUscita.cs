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
        /// Tipo operazione di marcatura
        /// </summary>
        public enum TipoMarcaturaIO 
            : int
        {
            /// <summary>
            /// Ingresso
            /// </summary>
            INGRESSO = 0,

            /// <summary>
            /// Uscita
            /// </summary>
            USCITA = 1
        }

        /// <summary>
        /// Metodo di riconoscimento usato
        /// </summary>
        [Flags]
        public enum MetodoRiconoscimento 
            : int
        {

            /// <summary>
            /// Sconosciuto
            /// </summary>
            Sconosciuto = 0,

            /// <summary>
            /// Autenticato tramite password
            /// </summary>
            Password = 1,

            /// <summary>
            /// Autenticato tramite impronta digitale
            /// </summary>
            ImprontaDigitale = 2,

            /// <summary>
            /// Autenticato tramite scansione della retina
            /// </summary>
            ImprontaRetina = 4,

            /// <summary>
            /// Autenticato tramite riconoscimento facciale
            /// </summary>
            RiconoscimentoFacciale = 16,

            /// <summary>
            /// Autenticato tramite impronta vocale
            /// </summary>
            RiconoscimentoVocale = 32
        }

        // 000001	2014-07-09 15:04:53	3	I


        /// <summary>
        /// Rappresenta un'operazione di ingresso/uscita di un utente
        /// in un ambiente protetto tramite un dispositivo di controllo
        /// accessi
        /// </summary>
        [Serializable]
        public class MarcaturaIngressoUscita 
            : minidom.Databases.DBObjectPO, IComparable, IComparable<MarcaturaIngressoUscita>
        {
            private int m_IDOperatore;
            [NonSerialized] private Sistema.CUser m_Operatore;
            private string m_NomeOperatore;
            private int m_IDDispositivo;
            [NonSerialized] private RilevatorePresenze m_Dispositivo;
            private DateTime m_Data;
            private TipoMarcaturaIO m_Operazione;
            private int m_IDReparto;
            private string m_NomeReparto;
            private MetodoRiconoscimento m_MetodiRiconoscimentoUsati;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public MarcaturaIngressoUscita()
            {
                m_IDOperatore = 0;
                m_Operatore = null;
                m_NomeOperatore = "";
                m_IDDispositivo = 0;
                m_Dispositivo = null;
                m_Data = DMD.DateUtils.Now();
                m_Operazione = TipoMarcaturaIO.INGRESSO;
                m_IDReparto = 0;
                m_NomeReparto = "";
                m_MetodiRiconoscimentoUsati = MetodoRiconoscimento.Sconosciuto;
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'operatore che ha effettualo la marcatura
            /// </summary>
            /// <returns></returns>
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
            /// Restituisce o imposta l'operatore che ha effettuato l'accesso
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
                    m_Operatore = value;
                    m_IDOperatore = DBUtils.GetID(value, 0);
                    m_NomeOperatore = (value is object )? value.Nominativo : "";
                    DoChanged("Operatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'operatore che ha effettuato l'accesso
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
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatore = value;
                    DoChanged("NomeOperatore", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta l'ID del dispositivo che ha registrato la marcatura
            /// </summary>
            /// <returns></returns>
            public int IDDispositivo
            {
                get
                {
                    return DBUtils.GetID(m_Dispositivo, m_IDDispositivo);
                }

                set
                {
                    int oldValue = IDDispositivo;
                    if (oldValue == value)
                        return;
                    m_IDDispositivo = value;
                    m_Dispositivo = null;
                    DoChanged("IDDispositivo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il dispositivo
            /// </summary>
            public RilevatorePresenze Dispositivo
            {
                get
                {
                    if (m_Dispositivo is null)
                        m_Dispositivo = minidom.Office.RilevatoriPresenze.GetItemById(m_IDDispositivo);
                    return m_Dispositivo;
                }

                set
                {
                    var oldValue = Dispositivo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Dispositivo = value;
                    m_IDDispositivo = DBUtils.GetID(value, 0);
                    DoChanged("Dispositivo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data della marcatura
            /// </summary>
            /// <returns></returns>
            public DateTime Data
            {
                get
                {
                    return m_Data;
                }

                set
                {
                    var oldValue = m_Data;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_Data = value;
                    DoChanged("Data", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo di operazione (ingresso o uscita)
            /// </summary>
            public TipoMarcaturaIO Operazione
            {
                get
                {
                    return m_Operazione;
                }

                set
                {
                    var oldValue = m_Operazione;
                    if (oldValue == value)
                        return;
                    m_Operazione = value;
                    DoChanged("Operazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del reparto
            /// </summary>
            /// <returns></returns>
            public int IDReparto
            {
                get
                {
                    return m_IDReparto;
                }

                set
                {
                    int oldValue = m_IDReparto;
                    if (oldValue == value)
                        return;
                    m_IDReparto = value;
                    DoChanged("IDReparto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del reparto
            /// </summary>
            public string NomeReparto
            {
                get
                {
                    return m_NomeReparto;
                }

                set
                {
                    string oldValue = m_NomeReparto;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeReparto = value;
                    DoChanged("NomeReparto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il metodo di riconoscimento usato
            /// </summary>
            public MetodoRiconoscimento MetodiRiconoscimentoUsati
            {
                get
                {
                    return m_MetodiRiconoscimentoUsati;
                }

                set
                {
                    var oldValue = m_MetodiRiconoscimentoUsati;
                    if (oldValue == value)
                        return;
                    m_MetodiRiconoscimentoUsati = value;
                    DoChanged("MetodiRiconoscimentoUsati", value, oldValue);
                }
            }
              
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Marcature;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override Sistema.CModule GetModule()
            {
                return Marcature.Module;
            }

            public override string GetTableName()
            {
                return "tbl_OfficeUserIO";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDOperatore = reader.Read("IDOperatore",  m_IDOperatore);
                m_NomeOperatore = reader.Read("NomeOperatore",  m_NomeOperatore);
                m_IDDispositivo = reader.Read("IDDispositivo",  m_IDDispositivo);
                m_Data = reader.Read("Data",  m_Data);
                m_Operazione = reader.Read("Operazione",  m_Operazione);
                m_IDReparto = reader.Read("IDReparto",  m_IDReparto);
                m_NomeReparto = reader.Read("NomeReparto",  m_NomeReparto);
                m_MetodiRiconoscimentoUsati = reader.Read("MetodiRiconoscimentoUsati",  m_MetodiRiconoscimentoUsati);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("IDOperatore", IDOperatore);
                writer.Write("NomeOperatore", m_NomeOperatore);
                writer.Write("IDDispositivo", IDDispositivo);
                writer.Write("Data", m_Data);
                writer.Write("Operazione", m_Operazione);
                writer.Write("IDReparto", IDReparto);
                writer.Write("NomeReparto", m_NomeReparto);
                writer.Write("MetodiRiconoscimentoUsati", m_MetodiRiconoscimentoUsati);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDOperatore", typeof(int), 1);
                c = table.Fields.Ensure("NomeOperatore", typeof(string), 255);
                c = table.Fields.Ensure("IDDispositivo", typeof(int), 1);
                c = table.Fields.Ensure("Data", typeof(DateTime), 1);
                c = table.Fields.Ensure("Operazione", typeof(int), 1);
                c = table.Fields.Ensure("IDReparto", typeof(int), 1);
                c = table.Fields.Ensure("NomeReparto", typeof(string), 255);
                c = table.Fields.Ensure("MetodiRiconoscimentoUsati", typeof(int), 1);
             
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxOperatore", new string[] { "IDOperatore", "IDDispositivo", "Data", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                
                c = table.Constraints.Ensure("idxParametri", new string[] { "NomeOperatore", "Operazione", "MetodiRiconoscimentoUsati" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxReparto", new string[] { "IDReparto", "NomeReparto" }, DBFieldConstraintFlags.None);
 

            }

            /// <summary>
            /// Seriaizzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", m_NomeOperatore);
                writer.WriteAttribute("IDDispositivo", IDDispositivo);
                writer.WriteAttribute("Data", m_Data);
                writer.WriteAttribute("Operazione", (int?)m_Operazione);
                writer.WriteAttribute("IDReparto", IDReparto);
                writer.WriteAttribute("NomeReparto", m_NomeReparto);
                writer.WriteAttribute("MetodiRiconoscimentoUsati", (int?)m_MetodiRiconoscimentoUsati);
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

                    case "IDDispositivo":
                        {
                            m_IDDispositivo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Data":
                        {
                            m_Data = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Operazione":
                        {
                            m_Operazione = (TipoMarcaturaIO)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDReparto":
                        {
                            m_IDReparto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeReparto":
                        {
                            m_NomeReparto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MetodiRiconoscimentoUsati":
                        {
                            m_MetodiRiconoscimentoUsati = (MetodoRiconoscimento)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
            public int CompareTo(MarcaturaIngressoUscita obj)
            {
                int ret = DMD.DateUtils.Compare(Data, obj.Data);
                if (ret == 0)
                {
                    int o1 = Sistema.IIF(Operazione == TipoMarcaturaIO.INGRESSO, 0, 1);
                    int o2 = Sistema.IIF(obj.Operazione == TipoMarcaturaIO.INGRESSO, 0, 1);
                    ret = o1.CompareTo(o2);
                }

                if (ret == 0)
                    ret = DMD.Strings.Compare(NomePuntoOperativo, obj.NomePuntoOperativo, true);
                if (ret == 0)
                    ret = IDOperatore.CompareTo(obj.IDOperatore);
                if (ret == 0)
                    ret = ID.CompareTo(obj.ID);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((MarcaturaIngressoUscita)obj);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.NomeOperatore, " ", this.m_Data , " ", this.Operazione);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Data);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is MarcaturaIngressoUscita) && this.Equals((MarcaturaIngressoUscita)obj); 
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(MarcaturaIngressoUscita obj)
            {
                return base.Equals(obj)
                     && DMD.Integers.EQ(this.m_IDOperatore, obj.m_IDOperatore)
                     && DMD.Strings.EQ(this.m_NomeOperatore, obj.m_NomeOperatore)
                     && DMD.Integers.EQ(this.m_IDDispositivo, obj.m_IDDispositivo)
                     && DMD.DateUtils.EQ(this.m_Data, obj.m_Data)
                     && DMD.Integers.EQ((int)this.m_Operazione, (int)obj.m_Operazione)
                     && DMD.Integers.EQ(this.m_IDReparto, obj.m_IDReparto)
                     && DMD.Strings.EQ(this.m_NomeReparto, obj.m_NomeReparto)
                     && DMD.Integers.EQ((int)this.m_MetodiRiconoscimentoUsati, (int)obj.m_MetodiRiconoscimentoUsati)
                     ;

            }
        }
    }
}