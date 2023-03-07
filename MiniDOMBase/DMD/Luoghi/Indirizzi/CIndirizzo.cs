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
        /// Rappresenta un indirizzo postale
        /// </summary>
        [Serializable]
        public class CIndirizzo 
            : Databases.DBObject, ISupportInitializeFrom
        {
            private int m_PersonaID;     // ID della persona associata
            [NonSerialized] private CPersona m_Persona; // Oggetto CPersona associato
            private string m_Nome; // Nome dell'indirizzo
            private string m_Citta; // Nome della città
            private string m_Provincia; // Nome della provincia
            private string m_CAP; // Codice di avviamento postale
            private string m_Toponimo;
            private string m_Via; // Via o piazza
            private string m_Civico; // Numero civico
            private string m_Note; // Note
            private double? m_Lat;
            private double? m_Lng;
            private double? m_Alt;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CIndirizzo()
            {
                m_PersonaID = 0;
                m_Persona = null;
                m_Nome = "";
                m_Citta = "";
                m_Provincia = "";
                m_CAP = "";
                m_Toponimo = "";
                m_Via = "";
                m_Civico = "";
                m_Note = "";
                m_Lat = null;
                m_Lng = null;
                m_Alt = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="text"></param>
            public CIndirizzo(CPersona owner, string text) : this()
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                SetPersona(owner);
                Parse(text);
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="text"></param>
            public CIndirizzo(string text) : this()
            {
                Parse(text);
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            public CIndirizzo(CPersona owner) : this()
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                SetPersona(owner);
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="etichetta"></param>
            /// <param name="text"></param>
            public CIndirizzo(string etichetta, string text) : this()
            {
                Parse(text);
                m_Nome = DMD.Strings.Trim(etichetta);
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="etichetta"></param>
            /// <param name="text"></param>
            public CIndirizzo(CPersona owner, string etichetta, string text) : this()
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                SetPersona(owner);
                Parse(text);
                m_Nome = DMD.Strings.Trim(etichetta);
            }

          
            /// <summary>
            /// Unisce i due indirizzi
            /// </summary>
            /// <param name="indirizzo"></param>
            public void MergeWith(CIndirizzo indirizzo)
            {
                if (m_PersonaID == 0)
                    m_PersonaID = indirizzo.PersonaID;
                if (m_Persona is null)
                    m_Persona = indirizzo.Persona;

                if (string.IsNullOrEmpty(m_Nome))
                    m_Nome = indirizzo.Nome;
                if (string.IsNullOrEmpty(m_Citta))
                    m_Citta = indirizzo.Citta;
                if (string.IsNullOrEmpty(m_Provincia))
                    m_Provincia = indirizzo.Provincia;
                if (string.IsNullOrEmpty(m_CAP))
                    m_CAP = indirizzo.CAP;
                if (string.IsNullOrEmpty(m_Toponimo))
                    m_Toponimo = indirizzo.Toponimo;
                if (string.IsNullOrEmpty(m_Via))
                    m_Via = indirizzo.Via;
                if (string.IsNullOrEmpty(m_Civico))
                    m_Civico = indirizzo.Civico;
                if (string.IsNullOrEmpty(m_Note))
                    m_Note = indirizzo.Note;
                if (!m_Lat.HasValue)
                    m_Lat = indirizzo.m_Lat;
                if (!m_Lng.HasValue)
                    m_Lng = indirizzo.m_Lng;
                if (!m_Alt.HasValue)
                    m_Alt = indirizzo.m_Alt;
                this.m_Flags = this.m_Flags | indirizzo.m_Flags;
                foreach(var k in indirizzo.Parameters.Keys )
                {
                    if (!this.Parameters.ContainsKey(k))
                        this.Parameters.Add(k, indirizzo.Parameters[k]);
                }
            }

             

            /// <summary>
            /// Restituisce o imposta la latitudine
            /// </summary>
            public double? Latitude
            {
                get
                {
                    return m_Lat;
                }

                set
                {
                    var oldValue = m_Lat;
                    if (oldValue == value)
                        return;
                    m_Lat = value;
                    DoChanged("Latitude", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la longitudine
            /// </summary>
            public double? Longitude
            {
                get
                {
                    return m_Lng;
                }

                set
                {
                    var oldValue = m_Lng;
                    if (oldValue == value)
                        return;
                    m_Lng = value;
                    DoChanged("Longitude", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il valore dell'altitudine
            /// </summary>
            public double? Altitude
            {
                get
                {
                    return m_Alt;
                }

                set
                {
                    var oldValue = m_Alt;
                    if (oldValue == value)
                        return;
                    m_Alt = value;
                    DoChanged("Altitude", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona associata all'indirizzo
            /// </summary>
            public int PersonaID
            {
                get
                {
                    return DBUtils.GetID(m_Persona, m_PersonaID);
                }

                set
                {
                    int oldValue = PersonaID;
                    if (oldValue == value)
                        return;
                    m_PersonaID = value;
                    m_Persona = null;
                    DoChanged("PersonaID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'oggetto Persona a cui è associato l'indirizzo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CPersona Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = Persone.GetItemById(m_PersonaID);
                    return m_Persona;
                }

                set
                {
                    var oldValue = Persona;
                    if (oldValue == value)
                        return;
                    m_Persona = value;
                    m_PersonaID = DBUtils.GetID(value, this.m_PersonaID);
                    DoChanged("Persona", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta la persona associata all'indirizzo
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetPersona(CPersona value)
            {
                m_Persona = value;
                m_PersonaID = DBUtils.GetID(value, this.m_PersonaID);
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'indirizzo (es. Residenza, Domicilio, ec..)
            /// </summary>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della città
            /// </summary>
            public string Citta
            {
                get
                {
                    return m_Citta;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Citta;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Citta = value;
                    DoChanged("Citta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della provincia
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
            /// Restituisce o imposta il CAP dell'indirizzo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
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
            /// Restituisce o imposta il tipo del luogo (via, piazza, contrada, ecc..)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Toponimo
            {
                get
                {
                    return m_Toponimo;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Toponimo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Toponimo = value;
                    DoChanged("Toponimo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce il nome del luogo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Via
            {
                get
                {
                    return m_Via;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Via;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Via = value;
                    DoChanged("Via", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce il numero civico
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Civico
            {
                get
                {
                    return m_Civico;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Civico;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Civico = value;
                    DoChanged("Civico", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo del luogo seguito dal nome (es. Via Varco della Calce)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ToponimoEVia
            {
                get
                {
                    return DMD.Strings.Trim(m_Toponimo + " " + m_Via);
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ToponimoEVia;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    var items = Luoghi.GetToponimiSupportati();
                    bool t = false;
                    value = DMD.Strings.Replace(value, "  ", " ");
                    for (int i = 0, loopTo = DMD.Arrays.UBound(items); i <= loopTo; i++)
                    {
                        if ((DMD.Strings.LCase(DMD.Strings.Left(value, DMD.Strings.Len(items[i]))) ?? "") == (DMD.Strings.LCase(items[i]) ?? ""))
                        {
                            m_Toponimo = Luoghi.EspandiToponimo(DMD.Strings.Left(value, DMD.Strings.Len(items[i])));
                            m_Via = DMD.Strings.Trim(DMD.Strings.Mid(value, DMD.Strings.Len(items[i]) + 1));
                            t = true;
                            break;
                        }
                    }

                    if (t == false)
                    {
                        m_Toponimo = "";
                        m_Via = value;
                    }

                    DoChanged("ToponimoEVia", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la parte toponimo, via e civico (es. Via Varco della Calce, 41)
            /// </summary>
            public string ToponimoViaECivico
            {
                get
                {
                    return DMD.Strings.Combine(ToponimoEVia, m_Civico, ", ");
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = ToponimoViaECivico;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    int p = DMD.Strings.InStr(value, ",");
                    if (p > 0)
                    {
                        ToponimoEVia = DMD.Strings.Left(value, p - 1);
                        m_Civico = DMD.Strings.Trim(DMD.Strings.Mid(value, p + 1));
                    }
                    else
                    {
                        ToponimoEVia = value;
                    }

                    DoChanged("ToponimoViaECivico", value, oldValue);
                }
            }



            /// <summary>
            /// Restituisce o imposta delle note
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Note
            {
                get
                {
                    return m_Note;
                }

                set
                {
                    string oldValue = m_Note;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Note = value;
                    DoChanged("Note", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della città seguito dal nome della provincia tra parentesi tonde
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeComune
            {
                get
                {
                    var ret = new System.Text.StringBuilder();
                    ret.Append(this.m_Citta);
                    if (!string.IsNullOrEmpty(m_Provincia))
                    {
                        ret.Append(" (");
                        ret.Append(this.m_Provincia);
                        ret.Append(")");
                    }
                    return ret.ToString();
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = NomeComune;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    int p;
                    string name, prov;
                    p = DMD.Strings.InStrRev(value, "(");
                    if (p > 1)
                    {
                        name = DMD.Strings.Trim(DMD.Strings.Left(value, p - 1));
                        value = DMD.Strings.Mid(value, p + 1);
                        p = DMD.Strings.InStrRev(value, ")");
                        if (p >= 1)
                        {
                            prov = DMD.Strings.Trim(DMD.Strings.Left(value, p - 1));
                        }
                        else
                        {
                            prov = DMD.Strings.Trim(value);
                        }
                    }
                    else
                    {
                        name = DMD.Strings.Trim(value);
                        prov = "";
                    }

                    m_Citta = name;
                    m_Provincia = prov;
                    DoChanged("NomeComune", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce il nome della tabella
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Indirizzi";
            }

            /// <summary>
            /// Restituisce il repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Luoghi.Indirizzi; //.Module;
            }


            ///// <summary>
            ///// Restituisce un riferimento al database
            ///// </summary>
            ///// <returns></returns>
            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Anagrafica.Persone.Database;
            //}

            /// <summary>
            /// Carica dal DB
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_PersonaID = reader.Read("Persona", m_PersonaID);
                this.m_Nome = reader.Read("Nome", m_Nome);
                this.m_Citta = reader.Read("Citta", m_Citta);
                this.m_Provincia = reader.Read("Provincia", m_Provincia);
                this.m_CAP = reader.Read("CAP", m_CAP);
                this.m_Toponimo = reader.Read("Toponimo", m_Toponimo);
                this.m_Via = reader.Read("Via", m_Via);
                this.m_Civico = reader.Read("Civico", m_Civico);
                this.m_Note = reader.Read("Note", m_Note);
                this.m_Lat = reader.Read("Lat", m_Lat);
                this.m_Lng = reader.Read("Lng", m_Lng);
                this.m_Alt = reader.Read("Alt", m_Alt);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Persona", this.PersonaID);
                writer.Write("Nome", this.m_Nome);
                writer.Write("Citta", this.m_Citta);
                writer.Write("Provincia", this.m_Provincia);
                writer.Write("CAP", this.m_CAP);
                writer.Write("Toponimo", this.m_Toponimo);
                writer.Write("Via", this.m_Via);
                writer.Write("Civico", this.m_Civico);
                writer.Write("Note", this.m_Note);
                writer.Write("Lat", this.m_Lat);
                writer.Write("Lng", this.m_Lng);
                writer.Write("Alt", this.m_Alt);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo shcema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);
                var c = table.Fields.Ensure("Persona", typeof(int), 1);
                c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("Citta", typeof(string), 255);
                c = table.Fields.Ensure("Provincia", typeof(string), 255);
                c = table.Fields.Ensure("Regione", typeof(string), 255);
                c = table.Fields.Ensure("Nazione", typeof(string), 255);
                c = table.Fields.Ensure("CAP", typeof(string), 32);
                c = table.Fields.Ensure("Toponimo", typeof(string), 255);
                c = table.Fields.Ensure("Via", typeof(string), 255);
                c = table.Fields.Ensure("Civico", typeof(string), 255);
                c = table.Fields.Ensure("Note", typeof(string), 1);
                c = table.Fields.Ensure("Lat", typeof(double), 1);
                c = table.Fields.Ensure("Lng", typeof(double), 1);
                c = table.Fields.Ensure("Alt", typeof(double), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);
                var c = table.Constraints.Ensure("idxPersona", new string[] {"Persona", "Nome" }, DBFieldConstraintFlags.Unique);
                c = table.Constraints.Ensure("idxCitta", new string[]{ "Nazione", "Provincia", "Citta" }, DBFieldConstraintFlags.None );
                c = table.Constraints.Ensure("idxCAP", new string[] { "CAP", "Toponimo", "Via", "Civico" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxLatLng", new string[] { "Lat", "Lng", "Alt" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                var ret = new System.Text.StringBuilder();
                ret.Append(this.m_Toponimo);
                if (ret.Length > 0 && !string.IsNullOrEmpty(this.m_Via)) ret.Append(" ");
                ret.Append(this.m_Via);
                if (ret.Length > 0 && !string.IsNullOrEmpty(m_Civico)) ret.Append(this.m_Civico);
                ret.Append(this.m_Civico);
                if (ret.Length > 0 && !string.IsNullOrEmpty(m_CAP)) ret.Append(DMD.Strings.vbNewLine);
                ret.Append(this.m_CAP);
                if (ret.Length > 0 && !string.IsNullOrEmpty(this.m_Citta)) ret.Append(" - ");
                ret.Append(this.m_Citta);
                if (ret.Length > 0 && !string.IsNullOrEmpty(this.m_Provincia)) ret.Append(" ");
                if (!string.IsNullOrEmpty(this.m_Provincia )) {
                    ret.Append("(");
                    ret.Append(this.m_Provincia);
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
                //     private int m_PersonaID;     // ID della persona associata
                //[NonSerialized] private CPersona m_Persona; // Oggetto CPersona associato
                return HashCalculator.Calculate(
                                this.m_Nome,
                                this.m_Citta,
                                this.m_Provincia,
                                this.m_CAP,
                                this.m_Toponimo,
                                this.m_Via,
                                //this.m_Civico,
                                //this.m_Note,
                                this.m_Lat,
                                this.m_Lng,
                                this.m_Alt
                                );

            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggeto
            /// </summary>
            /// <returns></returns>
            public string ToStringFormat()
            {
                string ret;
                ret = DMD.Strings.Trim(Strings.ConcatArray(this.Toponimo ,  " " , this.Via));
                if (!string.IsNullOrEmpty(ret) && !string.IsNullOrEmpty(this.Civico))
                    ret = Strings.ConcatArray(ret, ", ", this.Civico);
                return ret;
            }

            /// <summary>
            /// Deserializzazione XML
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "PersonaID":
                        {
                            m_PersonaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Citta":
                        {
                            m_Citta = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Provincia":
                        {
                            m_Provincia = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CAP":
                        {
                            m_CAP = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Toponimo":
                        {
                            m_Toponimo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Via":
                        {
                            m_Via = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Civico":
                        {
                            m_Civico = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Lat":
                        {
                            m_Lat = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Lng":
                        {
                            m_Lng = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Alt":
                        {
                            m_Alt = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
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
            /// Serializzazione XML
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("PersonaID", PersonaID);
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Citta", m_Citta);
                writer.WriteAttribute("Provincia", m_Provincia);
                writer.WriteAttribute("CAP", m_CAP);
                writer.WriteAttribute("Toponimo", m_Toponimo);
                writer.WriteAttribute("Via", m_Via);
                writer.WriteAttribute("Civico", m_Civico);
                writer.WriteAttribute("Lat", m_Lat);
                writer.WriteAttribute("Lng", m_Lng);
                writer.WriteAttribute("Alt", m_Alt);
                base.XMLSerialize(writer);
                writer.WriteTag("Note", m_Note);
            }

            /// <summary>
            /// Inizializza l'oggetto copiando i valori omologhi dell'indirizzo
            /// </summary>
            /// <param name="indirizzo"></param>
            public void CopyFrom(CIndirizzo indirizzo)
            {
                base._CopyFrom(indirizzo);
                this.m_PersonaID = indirizzo.PersonaID;
                this.m_Persona = indirizzo.m_Persona;
                this.m_Nome = indirizzo.Nome;
                this.m_Citta = indirizzo.Citta;
                this.m_Provincia = indirizzo.Provincia;
                this.m_CAP = indirizzo.CAP;
                this.m_Toponimo = indirizzo.Toponimo;
                this.m_Via = indirizzo.Via;
                this.m_Civico = indirizzo.Civico;
                this.m_Note = indirizzo.Note;
                this.m_Alt = indirizzo.m_Alt;
                this.m_Lat = indirizzo.m_Lat;
                this.m_Lng = indirizzo.m_Lng;
                this.m_Flags = indirizzo.m_Flags;
                this.m_Parameters = CollectionUtils.Clone(indirizzo.Parameters);
            }

            /// <summary>
            /// Inizializza l'oggetto dalla stringa
            /// </summary>
            /// <param name="value"></param>
            public void Parse(string value)
            {
                // Dim lines() As String
                // Dim nibbles() As String
                // text = Replace(text, "  ", "")
                // lines = Split(text, vbNewLine)
                // For i As Integer = 0 To UBound(lines)
                // Dim line As String = Trim(lines(i))
                // If (Left(line, 1) >= "0" And Left(line, 1) <= "9") Then
                // nibbles = Split(line, "-")
                // Me.m_CAP = nibbles(0)
                // If (UBound(nibbles) > 0) Then
                // Me.m_Citta = Luoghi.GetComune(nibbles(1))
                // Me.m_Provincia = Luoghi.GetProvincia(nibbles(1))
                // End If
                // ElseIf (InStr(line, "(") > 0 And InStr(line, "(") > 0) Then
                // Me.m_Citta = Luoghi.GetComune(line)
                // Me.m_Provincia = Luoghi.GetProvincia(line)
                // Else
                // Dim p As Integer = InStrRev(line, ",")
                // If (p > 0) Then
                // Me.m_Via = Trim(Left(line, p - 1))
                // Me.m_Civico = Trim(Mid(line, p + 1))
                // Else
                // Me.m_Via = line
                // End If
                // End If
                // Next

                value = DMD.Strings.Trim(value);
                value = DMD.Strings.Replace(value, DMD.Strings.vbCr, " ");
                value = DMD.Strings.Replace(value, DMD.Strings.vbLf, " ");
                value = DMD.Strings.Replace(value, "  ", " ");
                if ((value ?? "") == (ToString() ?? ""))
                    return;
                int p = value.LastIndexOf("(");
                string name;
                string prov;
                if (p > 0)
                {
                    name = DMD.Strings.Trim(value.Substring(0, p));
                    value = value.Substring(p + 1);
                    p = value.LastIndexOf(")");
                    if (p >= 0)
                    {
                        prov = value.Substring(0, p);
                    }
                    else
                    {
                        prov = value;
                    }
                }
                else
                {
                    name = value;
                    prov = "";
                }

                m_Provincia = prov;
                var nibbles = DMD.Strings.Split(name, " ");
                int i;
                m_Citta = "";
                for (i = nibbles.Length - 1; i >= 0; i -= 1)
                {
                    if (isCAP(nibbles[i]))
                    {
                        m_CAP = nibbles[i];
                        i -= 1;
                        break; // break;
                    }
                    else if (nibbles[i] == ",")
                    {
                        if (i + 1 < nibbles.Length)
                            m_Civico = nibbles[i + 1];
                        break;
                    }
                    else if (nibbles[i] == "-")
                    {
                        if (i - 1 >= 0 && isCAP(nibbles[i - 1]))
                        {
                            m_CAP = nibbles[i - 1];
                            i -= 1;
                            break; // break;
                        }

                        break; // break;
                    }
                    else
                    {
                        m_Citta = DMD.Strings.Combine(nibbles[i], m_Citta, " ");
                    }
                }

                string via = "";
                i -= 1;
                while (i >= 0)
                {
                    via = DMD.Strings.Combine(nibbles[i], via, " ");
                    i -= 1;
                }

                ToponimoViaECivico = via;
                SetChanged(true);
            }


            private static bool isCAP(string value)
            {
                return value.Length >= 5 && DMD.Strings.IsNumeric(value);
            }

            /// <summary>
            /// Restituisce vero se i due indirizzi sono ugali a meno del numero civico
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool equalsIgnoreCivico(CIndirizzo other)
            {
                var tmp1 = (CIndirizzo)MemberwiseClone();
                var tmp2 = (CIndirizzo)other.MemberwiseClone();
                tmp1.Civico = "";
                tmp2.Civico = "";
                return tmp1.Equals(tmp2);
            }

            /// <summary>
            /// Restituisce true se i due indirizzo sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CIndirizzo) && this.Equals((CIndirizzo)obj);
            }

            /// <summary>
            /// Restituisce true se i due indirizzo sono uguali
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public virtual bool Equals(CIndirizzo other)
            {
                return base.Equals(other)
                    && DMD.Integers.EQ(this.m_PersonaID, other.m_PersonaID)
                    && DMD.Strings.EQ(this.m_Nome, other.m_Nome)
                    && DMD.Strings.EQ(this.m_Citta, other.m_Citta)
                    && DMD.Strings.EQ(this.m_Provincia, other.m_Provincia)
                    && DMD.Strings.EQ(this.m_CAP, other.m_CAP)
                    && DMD.Strings.EQ(this.m_Toponimo, other.m_Toponimo)
                    && DMD.Strings.EQ(this.m_Via, other.m_Via)
                    && DMD.Strings.EQ(this.m_Civico, other.m_Civico)
                    && DMD.Strings.EQ(this.m_Note, other.m_Note)
                    && DMD.Doubles.EQ(this.m_Lat, other.m_Lat)
                    && DMD.Doubles.EQ(this.m_Lng, other.m_Lng)
                    && DMD.Doubles.EQ(this.m_Alt, other.m_Alt)
                    && DMD.Integers.EQ(this.m_Flags, other.m_Flags)
                    ;
                    //private CKeyCollection m_Parameters;

            }

            /// <summary>
            /// Copia i valori dei campi dall'indirizzo specificato
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public CIndirizzo InitializeFrom(CIndirizzo value)
            {
                //this.m_PersonaID = value.PersonaID;
                //this.m_Persona = value.m_Persona;
                this.m_Nome = value.m_Nome;
                this.m_Citta = value.m_Citta;
                this.m_Provincia = value.m_Provincia;
                this.m_CAP = value.m_CAP;
                this.m_Toponimo = value.m_Toponimo;
                this.m_Via = value.m_Via;
                this.m_Civico = value.m_Civico;
                this.m_Note = value.m_Note;
                this.m_Lat = value.m_Lat;
                this.m_Lng = value.m_Lng;
                this.m_Alt = value.m_Alt;
                this.m_Flags = value.m_Flags;
                this.m_Parameters = CollectionUtils.Clone(value.Parameters);
                return this;
            }

            void ISupportInitializeFrom.InitializeFrom(object value) { this.InitializeFrom((CIndirizzo)value); }
        }
    }
}