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
        /// Rappresenta un punto della mappa del territorio attraversato dall'utente per la commissione
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class LuogoVisitato 
            : Databases.DBObject, IComparable, IComparable<LuogoVisitato>
        {
            private int m_IDOperatore;                // ID dell'operatore
            [NonSerialized] private CUser m_Operatore;                    // Operatore
            private int m_IDUscita;                   // ID dell'uscita
            [NonSerialized] private Uscita m_Uscita;                      // Uscita
            private CIndirizzo m_Indirizzo;               // Indirizzo presso cui l'operatore
            private DateTime? m_OraArrivo;        // Data e ora di arrivo presso l'indirizzo specificato
            private DateTime? m_OraPartenza;          // Data e ora di partenza dall'indirizzo specificato
            private string m_Descrizione;
            private int m_IDLuogo;
            private LuogoDaVisitare m_Luogo;
            private int m_IDPersona;
            [NonSerialized] private CPersona m_Persona;
            private string m_TipoMateriale;
            private int m_ConsegnatiAMano;
            private int m_ConsegnatiPostale;
            private int m_ConsegnatiAuto;
            private int m_ConsegnatiAltro;
            private int m_Progressivo;

            /// <summary>
            /// Costruttore
            /// </summary>
            public LuogoVisitato()
            {
                m_IDOperatore = 0;
                m_Operatore = null;
                m_IDUscita = 0;
                m_Uscita = null;
                m_Indirizzo = new CIndirizzo();
                m_OraArrivo = default;
                m_OraPartenza = default;
                m_Descrizione = DMD.Strings.vbNullString;
                m_IDLuogo = 0;
                m_Luogo = null;
                m_IDPersona = 0;
                m_Persona = null;
                m_TipoMateriale = "";
                m_ConsegnatiAMano = 0;
                m_ConsegnatiPostale = 0;
                m_ConsegnatiAuto = 0;
                m_ConsegnatiAltro = 0;
                m_Progressivo = 0;
            }

            /// <summary>
            /// Ordine progressivo del luogo visitato durante la visita
            /// </summary>
            public int Progressivo
            {
                get
                {
                    return m_Progressivo;
                }

                set
                {
                    int oldValue = m_Progressivo;
                    if (oldValue == value)
                        return;
                    m_Progressivo = value;
                    DoChanged("Progressivo", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il numero progressivo
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetProgressivo(int value)
            {
                m_Progressivo = value;
            }

            /// <summary>
            /// ID dell'operaotore
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
            public CUser Operatore
            {
                get
                {
                    if (m_Operatore is null)
                        m_Operatore = Sistema.Users.GetItemById(m_IDOperatore);
                    return m_Operatore;
                }

                set
                {
                    var oldValue = m_Operatore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Operatore = value;
                    m_IDOperatore = DBUtils.GetID(value, 0);
                    DoChanged("Operatore", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'uscita
            /// </summary>
            public int IDUscita
            {
                get
                {
                    return DBUtils.GetID(m_Uscita, m_IDUscita);
                }

                set
                {
                    int oldValue = IDUscita;
                    if (oldValue == value)
                        return;
                    m_IDUscita = value;
                    m_Uscita = null;
                    DoChanged("IDUscita", value, oldValue);
                }
            }

            /// <summary>
            /// Uscita
            /// </summary>
            public Uscita Uscita
            {
                get
                {
                    if (m_Uscita is null)
                        m_Uscita = Uscite.GetItemById(m_IDUscita);
                    return m_Uscita;
                }

                set
                {
                    var oldValue = m_Uscita;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Uscita = value;
                    m_IDUscita = DBUtils.GetID(value, 0);
                    DoChanged("Uscita", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta l'uscita
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetUscita(Uscita value)
            {
                m_Uscita = value;
                m_IDUscita = DBUtils.GetID(value, 0);
            }

            /// <summary>
            /// Indirizzo del luogo visitato
            /// </summary>
            public CIndirizzo Indirizzo
            {
                get
                {
                    return m_Indirizzo;
                }
            }

            /// <summary>
            /// Ora di arrivo
            /// </summary>
            public DateTime? OraArrivo
            {
                get
                {
                    return m_OraArrivo;
                }

                set
                {
                    var oldValue = m_OraArrivo;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_OraArrivo = value;
                    DoChanged("OraArrivo", value, oldValue);
                }
            }

            /// <summary>
            /// Ora di partenza
            /// </summary>
            public DateTime? OraPartenza
            {
                get
                {
                    return m_OraPartenza;
                }

                set
                {
                    var oldValue = m_OraPartenza;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_OraPartenza = value;
                    DoChanged("OraPartenza", value, oldValue);
                }
            }

            /// <summary>
            /// Descrizione della visita
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
            /// Percorso a cui appartiene la visita
            /// </summary>
            public PercorsoDefinito Perorso
            {
                get
                {
                    if (Luogo is null)
                        return null;
                    return Luogo.Percorso;
                }
            }

            /// <summary>
            /// ID del luogo visitato
            /// </summary>
            public int IDLuogo
            {
                get
                {
                    return DBUtils.GetID(m_Luogo, m_IDLuogo);
                }

                set
                {
                    int oldValue = IDLuogo;
                    if (oldValue == value)
                        return;
                    m_IDLuogo = value;
                    m_Luogo = null;
                    DoChanged("IDLuogo", value, oldValue);
                }
            }

            /// <summary>
            /// Luogo visitato
            /// </summary>
            public LuogoDaVisitare Luogo
            {
                get
                {
                    if (m_Luogo is null)
                        m_Luogo = PercorsiDefiniti.LuoghiDaVisitare.GetItemById(m_IDLuogo);
                    return m_Luogo;
                }

                set
                {
                    var oldValue = m_Luogo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Luogo = value;
                    m_IDLuogo = DBUtils.GetID(value, 0);
                    DoChanged("Luogo", value, oldValue);
                }
            }

            /// <summary>
            /// ID della persona per cui si é effettuata la visita
            /// </summary>
            public int IDPersona
            {
                get
                {
                    return DBUtils.GetID(m_Persona, m_IDPersona);
                }

                set
                {
                    int oldValue = IDPersona;
                    if (oldValue == value)
                        return;
                    m_IDPersona = value;
                    m_Persona = null;
                    DoChanged("IDPersona", value, oldValue);
                }
            }

            /// <summary>
            /// Persona per cui si é effettuata la visita
            /// </summary>
            public CPersona Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = Anagrafica.Persone.GetItemById(m_IDPersona);
                    return m_Persona;
                }

                set
                {
                    var oldValue = m_Persona;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Persona = value;
                    m_IDPersona = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_Indirizzo.Nome = value.Nominativo;
                    DoChanged("Persona", value, oldValue);
                }
            }

            /// <summary>
            /// Tipo materiale (TODO togliere)
            /// </summary>
            public string TipoMateriale
            {
                get
                {
                    return m_TipoMateriale;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoMateriale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoMateriale = value;
                    DoChanged("TipoMateriale", value, oldValue);
                }
            }

            /// <summary>
            /// Consegnati a mano (TODO Togliere)
            /// </summary>
            public int ConsegnatiAMano
            {
                get
                {
                    return m_ConsegnatiAMano;
                }

                set
                {
                    int oldValue = m_ConsegnatiAMano;
                    if (oldValue == value)
                        return;
                    m_ConsegnatiAMano = value;
                    DoChanged("ConsegnatiAMano", value, oldValue);
                }
            }

            /// <summary>
            /// Consegnati Poste (TODO Togliere)
            /// </summary>
            public int ConsegnatiPostale
            {
                get
                {
                    return m_ConsegnatiPostale;
                }

                set
                {
                    int oldValue = m_ConsegnatiPostale;
                    if (oldValue == value)
                        return;
                    m_ConsegnatiPostale = value;
                    DoChanged("ConsegnatiPostale", value, oldValue);
                }
            }

            /// <summary>
            /// Consegnati Auto (TODO togliere)
            /// </summary>
            public int ConsegnatiAuto
            {
                get
                {
                    return m_ConsegnatiAuto;
                }

                set
                {
                    int oldValue = m_ConsegnatiAuto;
                    if (oldValue == value)
                        return;
                    m_ConsegnatiAuto = value;
                    DoChanged("ConsegnatiAuto", value, oldValue);
                }
            }

            /// <summary>
            /// Consegnati Altro (TODO togliere)
            /// </summary>
            public int ConsegnatiAltro
            {
                get
                {
                    return m_ConsegnatiAltro;
                }

                set
                {
                    int oldValue = m_ConsegnatiAltro;
                    if (oldValue == value)
                        return;
                    m_ConsegnatiAltro = value;
                    DoChanged("ConsegnatiAltro", value, oldValue);
                }
            }

            /// <summary>
            /// Consegnati in totale (TODO TOGLIERE)
            /// </summary>
            public int ConsegnatiTotale
            {
                get
                {
                    return m_ConsegnatiAltro + m_ConsegnatiAMano + m_ConsegnatiAuto + m_ConsegnatiPostale;
                }
            }

            /// <summary>
            /// Restitusice una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Indirizzo.Nome;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Indirizzo);
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.LuoghiVisitati;
            }

            /// <summary>
            /// Restituisce true se l'oggetto é stato modificato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                return base.IsChanged() || m_Indirizzo.IsChanged();
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                m_Indirizzo.SetChanged(false);
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeLuoghiV";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_IDOperatore = reader.Read("IDOperatore", this.m_IDOperatore);
                m_IDUscita = reader.Read("IDUscita", this.m_IDUscita);
                m_Indirizzo.Nome = reader.Read("Indirizzo_Etichetta", m_Indirizzo.Nome);
                m_Indirizzo.ToponimoViaECivico = reader.Read("Indirizzo_Via", m_Indirizzo.ToponimoViaECivico);
                m_Indirizzo.Citta = reader.Read("Indirizzo_Citta", m_Indirizzo.Citta);
                m_Indirizzo.Provincia = reader.Read("Indirizzo_Provincia", m_Indirizzo.Provincia);
                m_Indirizzo.CAP = reader.Read("Indirizzo_CAP", m_Indirizzo.CAP);
                m_Indirizzo.Latitude = reader.Read("Lat", m_Indirizzo.Latitude);
                m_Indirizzo.Longitude = reader.Read("Lng", m_Indirizzo.Longitude);
                m_Indirizzo.Altitude = reader.Read("Alt", m_Indirizzo.Altitude);
                m_Indirizzo.SetChanged(false);

                m_OraArrivo = reader.Read("OraArrivo", this.m_OraArrivo);
                m_OraPartenza = reader.Read("OraPartenza", this.m_OraPartenza);
                m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                m_IDLuogo = reader.Read("IDLuogo", this.m_IDLuogo);
                m_IDPersona = reader.Read("IDPersona", this.m_IDPersona);
                m_TipoMateriale = reader.Read("TipoMateriale", this.m_TipoMateriale);
                m_ConsegnatiAMano = reader.Read("ConsegnatiAMano", this.m_ConsegnatiAMano);
                m_ConsegnatiPostale = reader.Read("ConsegnatiPostale", this.m_ConsegnatiPostale);
                m_ConsegnatiAuto = reader.Read("ConsegnatiAuto", this.m_ConsegnatiAuto);
                m_ConsegnatiAltro = reader.Read("ConsegnatiAltro", this.m_ConsegnatiAltro);
                m_Progressivo = reader.Read("Progressivo", this.m_Progressivo);

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
                writer.Write("IDUscita", IDUscita);
                writer.Write("Indirizzo_Etichetta", m_Indirizzo.Nome);
                writer.Write("Indirizzo_Via", m_Indirizzo.ToponimoViaECivico);
                writer.Write("Indirizzo_Citta", m_Indirizzo.Citta);
                writer.Write("Indirizzo_Provincia", m_Indirizzo.Provincia);
                writer.Write("Indirizzo_CAP", m_Indirizzo.CAP);
                writer.Write("Lat", m_Indirizzo.Latitude);
                writer.Write("Lng", m_Indirizzo.Longitude);
                writer.Write("Alt", m_Indirizzo.Altitude);
                writer.Write("OraArrivo", m_OraArrivo);
                writer.Write("OraPartenza", m_OraPartenza);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("IDLuogo", IDLuogo);
                writer.Write("IDPersona", IDPersona);
                writer.Write("TipoMateriale", m_TipoMateriale);
                writer.Write("ConsegnatiAMano", m_ConsegnatiAMano);
                writer.Write("ConsegnatiPostale", m_ConsegnatiPostale);
                writer.Write("ConsegnatiAuto", m_ConsegnatiAuto);
                writer.Write("ConsegnatiAltro", m_ConsegnatiAltro);
                writer.Write("Progressivo", m_Progressivo);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Preapra i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("IDOperatore", typeof(int), 1);
                c = table.Fields.Ensure("IDUscita", typeof(int), 1);
                c = table.Fields.Ensure("Indirizzo_Etichetta", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_Via", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_Citta", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_Provincia", typeof(string), 255);
                c = table.Fields.Ensure("Indirizzo_CAP", typeof(string), 255);
                c = table.Fields.Ensure("Lat", typeof(double), 1);
                c = table.Fields.Ensure("Lng", typeof(double), 1);
                c = table.Fields.Ensure("Alt", typeof(double), 1);
                c = table.Fields.Ensure("OraArrivo", typeof(DateTime), 1);
                c = table.Fields.Ensure("OraPartenza", typeof(DateTime), 1);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("IDLuogo", typeof(int), 1);
                c = table.Fields.Ensure("IDPersona", typeof(int), 1);
                c = table.Fields.Ensure("TipoMateriale", typeof(string), 255);
                c = table.Fields.Ensure("ConsegnatiAMano", typeof(int), 1);
                c = table.Fields.Ensure("ConsegnatiPostale", typeof(int), 1);
                c = table.Fields.Ensure("ConsegnatiAuto", typeof(int), 1);
                c = table.Fields.Ensure("ConsegnatiAltro", typeof(int), 1);
                c = table.Fields.Ensure("Progressivo", typeof(int), 1);
                 
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxOperatore", new string[] { "IDOperatore", "IDPersona" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxUscita", new string[] { "IDUscita", "Progressivo" , "IDLuogo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndirizzo", new string[] { "Indirizzo_Etichetta", "Indirizzo_Via", "Indirizzo_Citta",  "Indirizzo_Provincia", "Indirizzo_CAP" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxLatLng", new string[] { "Lat", "Lng", "Alt" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxOrari", new string[] { "OraArrivo", "OraPartenza" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDescrizione", new string[] { "Descrizione" }, DBFieldConstraintFlags.None);
                //TODO togliere
                c = table.Constraints.Ensure("idxMateriale", new string[] { "TipoMateriale", "ConsegnatiAMano", "ConsegnatiPostale", "ConsegnatiAuto" , "ConsegnatiAltro"}, DBFieldConstraintFlags.None);

            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("OraArrivo", m_OraArrivo);
                writer.WriteAttribute("OraPartenza", m_OraPartenza);
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("IDLuogo", IDLuogo);
                writer.WriteAttribute("IDPersona", IDPersona);

                //TODO togliere
                writer.WriteAttribute("TipoMateriale", m_TipoMateriale);
                writer.WriteAttribute("ConsegnatiAMano", m_ConsegnatiAMano);
                writer.WriteAttribute("ConsegnatiPostale", m_ConsegnatiPostale);
                writer.WriteAttribute("ConsegnatiAuto", m_ConsegnatiAuto);
                writer.WriteAttribute("ConsegnatiAltro", m_ConsegnatiAltro);
                writer.WriteAttribute("Progressivo", m_Progressivo);
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("IDUscita", IDUscita);
                base.XMLSerialize(writer);
                writer.WriteTag("Indirizzo", m_Indirizzo);
                writer.WriteTag("Luogo", Luogo);
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

                    case "IDUscita":
                        {
                            m_IDUscita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Indirizzo":
                        {
                            m_Indirizzo = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }

                    case "OraArrivo":
                        {
                            m_OraArrivo = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "OraPartenza":
                        {
                            m_OraPartenza = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDLuogo":
                        {
                            m_IDLuogo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDPersona":
                        {
                            m_IDPersona = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoMateriale":
                        {
                            m_TipoMateriale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ConsegnatiAMano":
                        {
                            m_ConsegnatiAMano = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ConsegnatiPostale":
                        {
                            m_ConsegnatiPostale = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ConsegnatiAuto":
                        {
                            m_ConsegnatiAuto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ConsegnatiAltro":
                        {
                            m_ConsegnatiAltro = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Progressivo":
                        {
                            m_Progressivo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Luogo":
                        {
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
                return CompareTo((LuogoVisitato)obj);
            }

            /// <summary>
            /// Compara due luoghi visitati in base al progressivo e all'ora
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual int CompareTo(LuogoVisitato obj)
            {
                int ret = m_Progressivo - obj.m_Progressivo;
                if (ret == 0) ret = DMD.DateUtils.Compare(this.m_OraArrivo, obj.m_OraArrivo);
                if (ret == 0) ret = DMD.DateUtils.Compare(this.m_OraPartenza, obj.m_OraPartenza);
                if (ret == 0) ret = DMD.Integers.Compare(this.ID, obj.ID);
                 
                return ret;
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is LuogoVisitato) && this.Equals((LuogoVisitato)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equlas(LuogoVisitato obj)
            {
                return base.Equals(obj)
                    && DMD.Integers.EQ(this.m_IDOperatore, obj.m_IDOperatore)
                    && DMD.Integers.EQ(this.m_IDUscita, obj.m_IDUscita)
                    && this.m_Indirizzo.Equals(obj.m_Indirizzo)
                    && DMD.DateUtils.EQ(this.m_OraArrivo, obj.m_OraArrivo)
                    && DMD.DateUtils.EQ(this.m_OraPartenza, obj.m_OraPartenza)
                    && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                    && DMD.Integers.EQ(this.m_IDLuogo, obj.m_IDLuogo)
                    && DMD.Integers.EQ(this.m_IDPersona, obj.m_IDPersona)
                    && DMD.Strings.EQ(this.m_TipoMateriale, obj.m_TipoMateriale)
                    && DMD.Integers.EQ(this.m_ConsegnatiAMano, obj.m_ConsegnatiAMano)
                    && DMD.Integers.EQ(this.m_ConsegnatiPostale, obj.m_ConsegnatiPostale)
                    && DMD.Integers.EQ(this.m_ConsegnatiAuto, obj.m_ConsegnatiAuto)
                    && DMD.Integers.EQ(this.m_ConsegnatiAltro, obj.m_ConsegnatiAltro)
                    && DMD.Integers.EQ(this.m_Progressivo, obj.m_Progressivo)
                    ;
            }
        }
    }
}