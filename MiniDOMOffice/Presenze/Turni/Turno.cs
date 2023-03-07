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
        /// Flag validi per un turno lavorativo
        /// </summary>
        [Flags]
        public enum TurnoFlagGiorni : int
        {
            /// <summary>
            /// Tutti
            /// </summary>
            Tutti = 0,

            /// <summary>
            /// Turno domenicale
            /// </summary>
            Domenica = 1,

            /// <summary>
            /// Lunedì
            /// </summary>
            Lunedi = 2,

            /// <summary>
            /// Martedì
            /// </summary>
            Martedi = 4,

            /// <summary>
            /// Mercoledì
            /// </summary>
            Mercoledi = 8,

            /// <summary>
            /// Giovedì
            /// </summary>
            Giovedi = 16,

            /// <summary>
            /// Venerdì
            /// </summary>
            Venerdi = 32,

            /// <summary>
            /// Sabato
            /// </summary>
            Sabato = 64
        }

        /// <summary>
        /// Turni lavorativi
        /// </summary>
        [Serializable]
        public class Turno 
            : minidom.Databases.DBObject, IComparable, IComparable<Turno>
        {
            private string m_Nome;                        // Nome del turno
            private DateTime m_OraIngresso;                  // Ora di ingresso
            private DateTime m_OraUscita;                 // Ora di uscita
            private int m_TolleranzaIngressoAnticipato;        // Tolleranza (in minuti) rispetto all'ora di ingresso (ingresso anticipato)
            private int m_TolleranzaIngressoRitardato;        // Tolleranza (in minuti) rispetto all'ora di ingresso (ingresso posticipato
            private int m_TolleranzaUscitaAnticipata;          // Tolleranza (in minuti) rispetto all'ora di uscita (uscita anticipata)
            private int m_TolleranzaUscitaRitardata;          // Tolleranza (in minuti) rispetto all'ora di uscita (uscita posticipata)
            private DateTime? m_ValidoDal;                    // Data da cui ha inizio la validità del turno
            private DateTime? m_ValidoAl;                     // Data in cui finisce la validità del turno
            private bool m_Attivo;                     // Se vero indica che il turno è abilitato
            private TurnoFlagGiorni m_GiorniDellaSettimana;   // Giorni della settimana a cui si applica il turno
            private CCollection<Sistema.CUser> m_Utenti;       // Utenti a cui è assegnato il turno
            private int m_Periodicita;                // Periodicità del turno rispetto alla data di inizio validità

            /// <summary>
            /// Costruttore
            /// </summary>
            public Turno()
            {
                m_Nome = "";
                m_OraIngresso = default;
                m_OraUscita = default;
                m_TolleranzaIngressoAnticipato = 0;
                m_TolleranzaIngressoRitardato = 0;
                m_TolleranzaUscitaAnticipata = 0;
                m_TolleranzaUscitaRitardata = 0;
                m_ValidoDal = default;
                m_ValidoAl = default;
                m_Attivo = true;
                m_GiorniDellaSettimana = TurnoFlagGiorni.Tutti;
                m_Utenti = null;
                m_Periodicita = 0;
            }

            /// <summary>
            /// Restituisce o imposta il nome del turno
            /// </summary>
            /// <returns></returns>
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
                    if ((value ?? "") == (oldValue ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ora di ingresso pervista
            /// </summary>
            /// <returns></returns>
            public DateTime OraIngresso
            {
                get
                {
                    return m_OraIngresso;
                }

                set
                {
                    var oldValue = m_OraIngresso;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_OraIngresso = value;
                    DoChanged("OraIngresso", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ora di uscita prevista
            /// </summary>
            /// <returns></returns>
            public DateTime OraUscita
            {
                get
                {
                    return m_OraUscita;
                }

                set
                {
                    var oldValue = m_OraUscita;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_OraUscita = value;
                    DoChanged("OraUscita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di minuti consentiti per l'ingresso anticipato
            /// </summary>
            /// <returns></returns>
            public int TolleranzaIngressoAnticipato
            {
                get
                {
                    return m_TolleranzaIngressoAnticipato;
                }

                set
                {
                    int oldValue = m_TolleranzaIngressoAnticipato;
                    if (oldValue == value)
                        return;
                    m_TolleranzaIngressoAnticipato = value;
                    DoChanged("TolleranzaIngressoAnticipato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di minuti di tolleranza per l'ingresso posticipato
            /// </summary>
            /// <returns></returns>
            public int TolleranzaIngressoRitardato
            {
                get
                {
                    return m_TolleranzaIngressoRitardato;
                }

                set
                {
                    int oldValue = m_TolleranzaIngressoRitardato;
                    if (oldValue == value)
                        return;
                    m_TolleranzaIngressoRitardato = value;
                    DoChanged("TolleranzaIngressoRitardato", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero di minuti di tolleranza per l'uscita anticipata
            /// </summary>
            /// <returns></returns>
            public int TolleranzaUscitaAnticipata
            {
                get
                {
                    return m_TolleranzaUscitaAnticipata;
                }

                set
                {
                    int oldValue = m_TolleranzaUscitaAnticipata;
                    if (oldValue == value)
                        return;
                    m_TolleranzaUscitaAnticipata = value;
                    DoChanged("TolleranzaUscitaAnticipata", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la tolleranza in minuti per l'uscita ritardata
            /// </summary>
            /// <returns></returns>
            public int TolleranzaUscitaRitardata
            {
                get
                {
                    return m_TolleranzaUscitaRitardata;
                }

                set
                {
                    int oldValue = m_TolleranzaUscitaRitardata;
                    if (oldValue == value)
                        return;
                    m_TolleranzaUscitaRitardata = value;
                    DoChanged("TolleranzaUscitaRitardata", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di inizio validità del turno
            /// </summary>
            /// <returns></returns>
            public DateTime? ValidoDal
            {
                get
                {
                    return m_ValidoDal;
                }

                set
                {
                    var oldValue = m_ValidoDal;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_ValidoDal = value;
                    DoChanged("ValidoDal", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di fine validità del turno
            /// </summary>
            /// <returns></returns>
            public DateTime? ValidoAl
            {
                get
                {
                    return m_ValidoAl;
                }

                set
                {
                    var oldValue = m_ValidoAl;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_ValidoAl = value;
                    DoChanged("ValidoAl", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta
            /// </summary>
            /// <returns></returns>
            public int Periodicita
            {
                get
                {
                    return m_Periodicita;
                }

                set
                {
                    int oldValue = m_Periodicita;
                    if (oldValue == value)
                        return;
                    m_Periodicita = value;
                    DoChanged("Periodicita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il turno è abilitato
            /// </summary>
            /// <returns></returns>
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
            /// Restituisce o imposta un valore booleano che indica se il turno è attivo per i giorni della settimana specificati
            /// </summary>
            /// <param name="giorni"></param>
            /// <returns></returns>
            public bool get_AttivoGiornoSettimana(TurnoFlagGiorni giorni)
            {
                return DMD.RunTime.TestFlag(m_GiorniDellaSettimana, giorni);
            }

            /// <summary>
            /// Imposta la validità del turno per il giorno specifico
            /// </summary>
            /// <param name="giorni"></param>
            /// <param name="value"></param>
            public void set_AttivoGiornoSettimana(TurnoFlagGiorni giorni, bool value)
            {
                bool oldValue = get_AttivoGiornoSettimana(giorni);
                if (oldValue == value)
                    return;
                m_GiorniDellaSettimana = DMD.RunTime.SetFlag(m_GiorniDellaSettimana, giorni, value);
                DoChanged("AttivioGiornoSettimana", value, oldValue);
            }

         

            /// <summary>
            /// Restituisce la collezione degli utenti a cui è assegnato il turno
            /// </summary>
            /// <returns></returns>
            public CCollection<Sistema.CUser> Utenti
            {
                get
                {
                    if (m_Utenti is null)
                        m_Utenti = new CCollection<Sistema.CUser>();
                    return m_Utenti;
                }
            }

            /// <summary>
            /// Restituisce true se il turno é attivo per la data specificata
            /// </summary>
            /// <param name="at"></param>
            /// <returns></returns>
            public bool IsValid(DateTime at)
            {
                return m_Attivo && DMD.DateUtils.CheckBetween(at, m_ValidoDal, m_ValidoAl);
            }

            /// <summary>
            /// Restituisce true se il turno é attivo per la data e l'ora odierna
            /// </summary>
            /// <returns></returns>
            public bool IsValid()
            {
                return IsValid(DMD.DateUtils.Now());
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Turni;
            }

            public override Sistema.CModule GetModule()
            {
                return Turni.Module;
            }

            public override string GetTableName()
            {
                return "tbl_OfficeTurniIO";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.m_OraIngresso = reader.Read("OraIngresso", this.m_OraIngresso);
                this.m_OraUscita = reader.Read("OraUscita", this.m_OraUscita);
                this.m_TolleranzaIngressoAnticipato = reader.Read("TolleranzaIngressoAnticipato", this.m_TolleranzaIngressoAnticipato);
                this.m_TolleranzaIngressoRitardato = reader.Read("TolleranzaIngressoRitardato", this.m_TolleranzaIngressoRitardato);
                this.m_TolleranzaUscitaAnticipata = reader.Read("TolleranzaUscitaAnticipata", this.m_TolleranzaUscitaAnticipata);
                this.m_TolleranzaUscitaRitardata = reader.Read("TolleranzaUscitaRitardata", this.m_TolleranzaUscitaRitardata);
                this.m_ValidoDal = reader.Read("ValidoDal", this.m_ValidoDal);
                this.m_ValidoAl = reader.Read("ValidoAl", this.m_ValidoAl);
                this.m_Attivo = reader.Read("Attivo", this.m_Attivo);
                this.m_GiorniDellaSettimana = reader.Read("GiorniDellaSettimana", this.m_GiorniDellaSettimana);
                this.m_Periodicita = reader.Read("Periodicita", this.m_Periodicita);
                string tmp = reader.Read("Utenti", "");
                if (!string.IsNullOrEmpty(tmp))
                    this.m_Utenti = GetUsers(tmp);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", this.m_Nome);
                writer.Write("OraIngresso", this.m_OraIngresso);
                writer.Write("OraUscita", this.m_OraUscita);
                writer.Write("TolleranzaIngressoAnticipato", this.m_TolleranzaIngressoAnticipato);
                writer.Write("TolleranzaIngressoRitardato", this.m_TolleranzaIngressoRitardato);
                writer.Write("TolleranzaUscitaAnticipata", this.m_TolleranzaUscitaAnticipata);
                writer.Write("TolleranzaUscitaRitardata", this.m_TolleranzaUscitaRitardata);
                writer.Write("ValidoDal", this.m_ValidoDal);
                writer.Write("ValidoAl", this.m_ValidoAl);
                writer.Write("Attivo", this.m_Attivo);
                writer.Write("GiorniDellaSettimana", this.m_GiorniDellaSettimana);
                writer.Write("Periodicita", this.m_Periodicita);
                writer.Write("Utenti", GetUserIDArr(this.Utenti));
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi del db
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("OraIngresso", typeof(DateTime), 1);
                c = table.Fields.Ensure("OraUscita", typeof(DateTime), 1);
                c = table.Fields.Ensure("TolleranzaIngressoAnticipato", typeof(int), 1);
                c = table.Fields.Ensure("TolleranzaIngressoRitardato", typeof(int), 1);
                c = table.Fields.Ensure("TolleranzaUscitaAnticipata", typeof(int), 1);
                c = table.Fields.Ensure("TolleranzaUscitaRitardata", typeof(int), 1);
                c = table.Fields.Ensure("ValidoDal", typeof(DateTime), 1);
                c = table.Fields.Ensure("ValidoAl", typeof(DateTime), 1);
                c = table.Fields.Ensure("Attivo", typeof(bool), 1);
                c = table.Fields.Ensure("GiorniDellaSettimana", typeof(int), 1);
                c = table.Fields.Ensure("Periodicita", typeof(int), 1);
                c = table.Fields.Ensure("Utenti", typeof(string), 0);
                
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", this.m_Nome);
                writer.WriteAttribute("OraIngresso", this.m_OraIngresso);
                writer.WriteAttribute("OraUscita", this.m_OraUscita);
                writer.WriteAttribute("TolleranzaIngressoAnticipato", this.m_TolleranzaIngressoAnticipato);
                writer.WriteAttribute("TolleranzaIngressoRitardato", this.m_TolleranzaIngressoRitardato);
                writer.WriteAttribute("TolleranzaUscitaAnticipata", this.m_TolleranzaUscitaAnticipata);
                writer.WriteAttribute("TolleranzaUscitaRitardata", this.m_TolleranzaUscitaRitardata);
                writer.WriteAttribute("ValidoDal", this.m_ValidoDal);
                writer.WriteAttribute("ValidoAl", this.m_ValidoAl);
                writer.WriteAttribute("Attivo", this.m_Attivo);
                writer.WriteAttribute("GiorniDellaSettimana", (int?)this.m_GiorniDellaSettimana);
                writer.WriteAttribute("Periodicita", this.m_Periodicita);
                base.XMLSerialize(writer);
                writer.WriteTag("Utenti", GetUserIDArr(this.Utenti));
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

                    case "OraIngresso":
                        {
                            m_OraIngresso = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "OraUscita":
                        {
                            m_OraUscita = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "TolleranzaIngressoAnticipato":
                        {
                            m_TolleranzaIngressoAnticipato = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TolleranzaIngressoRitardato":
                        {
                            m_TolleranzaIngressoRitardato = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TolleranzaUscitaAnticipata":
                        {
                            m_TolleranzaUscitaAnticipata = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TolleranzaUscitaRitardata":
                        {
                            m_TolleranzaUscitaRitardata = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ValidoDal":
                        {
                            m_ValidoDal = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ValidoAl":
                        {
                            m_ValidoAl = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Attivo":
                        {
                            m_Attivo = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "GiorniDellaSettimana":
                        {
                            m_GiorniDellaSettimana = (TurnoFlagGiorni)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Periodicita":
                        {
                            m_Periodicita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

             
                    case "Utenti":
                        {
                            m_Utenti = GetUsers(DMD.Strings.CStr(fieldValue));
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
            /// Compara due turno in ordine crescente per data
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(Turno obj)
            {
                int ret = DMD.DateUtils.Compare(m_OraIngresso, obj.m_OraIngresso);
                if (ret == 0)
                    ret = DMD.DateUtils.Compare(m_OraUscita, obj.m_OraUscita);
                if (ret == 0)
                    ret = DMD.Strings.Compare(m_Nome, obj.m_Nome, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((Turno)obj);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Nome;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.OraIngresso);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is Turno) && this.Equals((Turno)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(Turno obj)
            {
                return base.Equals(obj)
                     && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                     && DMD.DateUtils.EQ(this.m_OraIngresso, obj.m_OraIngresso)
                    && DMD.DateUtils.EQ(this.m_OraUscita, obj.m_OraUscita)
                    && DMD.Integers.EQ(this.m_TolleranzaIngressoAnticipato, obj.m_TolleranzaIngressoAnticipato)
                    && DMD.Integers.EQ(this.m_TolleranzaIngressoRitardato, obj.m_TolleranzaIngressoRitardato)
                    && DMD.Integers.EQ(this.m_TolleranzaUscitaAnticipata, obj.m_TolleranzaUscitaAnticipata)
                    && DMD.Integers.EQ(this.m_TolleranzaUscitaRitardata, obj.m_TolleranzaUscitaRitardata)
                    && DMD.DateUtils.EQ(this.m_ValidoDal, obj.m_ValidoDal)
                    && DMD.DateUtils.EQ(this.m_ValidoAl, obj.m_ValidoAl)
                    && DMD.Booleans.EQ(this.m_Attivo, obj.m_Attivo)
                    && DMD.Integers.EQ((int)this.m_GiorniDellaSettimana, (int)obj.m_GiorniDellaSettimana)
                    && DMD.Integers.EQ(this.m_Periodicita, obj.m_Periodicita)
                    ;

            }

            private string GetUserIDArr(CCollection<Sistema.CUser> col)
            {
                var ret = new System.Text.StringBuilder();
                foreach (Sistema.CUser u in col)
                {
                    if (ret.Length > 0)
                        ret.Append(",");
                    ret.Append(DBUtils.GetID(u).ToString());
                }

                return ret.ToString();
            }

            private CCollection<Sistema.CUser> GetUsers(string text)
            {
                var ret = new CCollection<Sistema.CUser>();
                var arr = Strings.Split(text, ",");
                if (DMD.Arrays.Len(arr) > 0)
                {
                    foreach (string str in arr)
                    {
                        int uid = Sistema.Formats.ToInteger(str);
                        var u = Sistema.Users.GetItemById(uid);
                        if (u is object)
                            ret.Add(u);
                    }
                }

                return ret;
            }
        }
    }
}