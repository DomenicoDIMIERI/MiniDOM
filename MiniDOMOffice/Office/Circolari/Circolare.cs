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
        /// Rappresenta un documento o una comunicazione pubblicata sul sito
        /// TODO Modificare in DBObjectPO in js
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class Circolare 
            : minidom.Databases.DBObjectPO, IIndexable, IComparable, IComparable<Circolare>
        {
            private DateTime? m_DataInizio; // Data
            private DateTime? m_DataFine; // Data Fine
            private string m_Categoria; // Categoria
            private string m_Sottocategoria; // SottoCategoria TODO aggiungere in js
            private string m_Titolo; // Titolo della circolare TODO rinominare in js
            private string m_Testo; //Contenuto della circolare TODO rinominare in js
            private PriorityEnum m_Priorita; //Priorità
            private string m_URL; //Percorso di lavoro
            private bool m_PrimaPagina; // Vero se visibile in prima pagina
            private int m_Progressivo; //Numero progressivo dell'oggetto
            private string m_NumeroCircolare; //Numero della circolare //TODO aggiungere in js
            private CAttachmentsCollection m_Attachments; //cambiare tipo in js
            [NonSerialized] private CircolareXUserCollection m_UsersAllowNegate; //todo riportare in js
            [NonSerialized] private CircolareXGroupCollection m_GroupsAllowNegate; //todo riportare in js

            /// <summary>
            /// Costruttore
            /// </summary>
            public Circolare()
            {
                this.m_DataInizio = DMD.DateUtils.Now();
                this.m_DataFine = default;
                this.m_Categoria = "";
                this.m_Sottocategoria = "";
                this.m_Titolo = "";
                this.m_Testo = "";
                this.m_Priorita = 0;
                this.m_URL = "";
                this.m_PrimaPagina = false;
                this.m_Progressivo = 0;
                this.m_Attachments = null;
                this.m_NumeroCircolare = "";
            }

            /// <summary>
            /// Restituisce la collezione delle autorizzazioni utente per la circolare
            /// TODO riportare in js
            /// </summary>
            public CircolareXUserCollection UsersAllowNegate
            {
                get
                {
                    if (this.m_UsersAllowNegate is null)
                        this.m_UsersAllowNegate = new CircolareXUserCollection(this);
                    return this.m_UsersAllowNegate;
                }
            }

            /// <summary>
            /// Restituisce la collezione delle autorizzazioni per gruppo sulla circolare
            /// TODO riportare in js
            /// </summary>
            public CircolareXGroupCollection GroupsAllowNegate
            {
                get
                {
                    if (this.m_GroupsAllowNegate is null)
                        this.m_GroupsAllowNegate = new CircolareXGroupCollection(this);
                    return this.m_GroupsAllowNegate;
                }
            }


            /// <summary>
            /// Compara due oggetti
            /// TODO riportare in js le modifiche (manca l'operatore)
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(Circolare other)
            {
                int ret = DateUtils.Compare(this.m_DataInizio, other.m_DataFine);
                if (ret == 0) ret = Strings.Compare(this.m_Titolo, other.m_Titolo, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return this.CompareTo((Circolare)obj);
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Circolari;
            }

            /// <summary>
            /// Restituisce la collezione degli allegati alla comunicazione
            /// TODO riportare in js la modifica
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CAttachmentsCollection Attachments
            {
                get
                {
                    if (m_Attachments is null)
                        m_Attachments = new CAttachmentsCollection(this);
                    return m_Attachments;
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero della circolare
            /// </summary>
            public string NumeroCircolare
            {
                get
                {
                    return this.m_NumeroCircolare;
                }
                set
                {
                    value = Strings.Trim(value);
                    var oldValue = this.m_NumeroCircolare;
                    if (Strings.EQ(value, oldValue)) return;
                    this.m_NumeroCircolare = value;
                    this.DoChanged("NumeroCircolare", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero progressivo del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
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
            /// Restituisce o imposta la data di inizio validità del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataInizio
            {
                get
                {
                    return m_DataInizio;
                }

                set
                {
                    var oldValue = m_DataInizio;
                    if (DateUtils.EQ(oldValue , value))
                        return;
                    m_DataInizio = value;
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di fine validità del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataFine
            {
                get
                {
                    return m_DataFine;
                }

                set
                {
                    var oldValue = m_DataFine;
                    if (DateUtils.EQ(oldValue, value))
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la categoria del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Categoria
            {
                get
                {
                    return m_Categoria;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Categoria;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Categoria = value;
                    DoChanged("Categoria", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la sottocategoria
            /// </summary>
            public string Sottocategoria
            {
                get
                {
                    return this.m_Sottocategoria;
                }
                set
                {
                    value = Strings.Trim(value);
                    var oldValue = this.m_Sottocategoria;
                    if (Strings.EQ(value, oldValue))
                        return;
                    this.m_Sottocategoria = value;
                    this.DoChanged("Sottocategoria", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la descrizione del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Titolo
            {
                get
                {
                    return m_Titolo;
                }

                set
                {
                    string oldValue = m_Titolo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Titolo = value;
                    DoChanged("Titolo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il livello di priorità del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public PriorityEnum Priorita
            {
                get
                {
                    return m_Priorita;
                }

                set
                {
                    var oldValue = m_Priorita;
                    if (oldValue == value)
                        return;
                    m_Priorita = value;
                    DoChanged("Priorita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la URL del collegamento esterno
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string URL
            {
                get
                {
                    return m_URL;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_URL;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_URL = value;
                    DoChanged("URL", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il documento deve essere visualizzato nell'elenco delle comunicazioni
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool PrimaPagina
            {
                get
                {
                    return m_PrimaPagina;
                }

                set
                {
                    if (m_PrimaPagina == value)
                        return;
                    m_PrimaPagina = value;
                    DoChanged("PrimaPagina", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta le note
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Testo
            {
                get
                {
                    return m_Testo;
                }

                set
                {
                    string oldValue = m_Testo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Testo = value;
                    DoChanged("Testo", value, oldValue);
                }
            }

             
            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Comunicazioni";
            }
 
            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_DataInizio = reader.Read("DataInizio", m_DataInizio);
                this.m_DataFine = reader.Read("DataFine", m_DataFine);
                this.m_Categoria = reader.Read("Categoria", m_Categoria);
                this.m_Sottocategoria = reader.Read("Sottocategoria", m_Sottocategoria);
                this.m_Titolo = reader.Read("Titolo", m_Titolo);
                this.m_Priorita = reader.Read("Priorita", m_Priorita);
                this.m_URL = reader.Read("URL", m_URL);
                this.m_PrimaPagina = reader.Read("PrimaPagina", m_PrimaPagina);
                this.m_Testo = reader.Read("Testo", m_Testo);
                this.m_Progressivo = reader.Read("Progressivo", m_Progressivo);
                this.m_NumeroCircolare = reader.Read("NumeroCircolare", this.m_NumeroCircolare);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                writer.Write("Categoria", m_Categoria);
                writer.Write("Sottocategoria", m_Sottocategoria);
                writer.Write("Titolo", m_Titolo);
                writer.Write("Priorita", m_Priorita);
                writer.Write("URL", m_URL);
                writer.Write("PrimaPagina", m_PrimaPagina);
                writer.Write("Testo", m_Testo);
                writer.Write("Progressivo", m_Progressivo);
                writer.Write("NumeroCircolare", this.m_NumeroCircolare);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("DataInizio", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataFine", typeof(DateTime), 1);
                c = table.Fields.Ensure("Categoria", typeof(string), 255);
                c = table.Fields.Ensure("Sottocategoria", typeof(string), 255);
                c = table.Fields.Ensure("Titolo", typeof(string), 255);
                c = table.Fields.Ensure("Priorita", typeof(int), 1);
                c = table.Fields.Ensure("URL", typeof(string), 255);
                c = table.Fields.Ensure("PrimaPagina", typeof(bool), 1);
                c = table.Fields.Ensure("Testo", typeof(string), 0);
                c = table.Fields.Ensure("Progressivo", typeof(int), 1);
                c = table.Fields.Ensure("NumeroCircolare", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxDate", new string[] { "DataInizio", "DataFine" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCategoria", new string[] { "Categoria", "Sottocategoria" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTitolo", new string[] { "Titolo", "URL" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxPriorita", new string[] { "Priorita", "PrimaPagina" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxTesto", new string[] { "Testo" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxProgressivo", new string[] { "NumeroCircolare", "Progressivo" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.NumeroCircolare, " ",  this.m_Titolo);
            }

            private class UserAN
            {
                public CUser User = null;
                public bool UserAllow = false;
                public bool UserNegate = false;
                public CGroup Group = null;
                public bool GroupAllow = false;
                public bool GroupNegate = false;
            }

            /// <summary>
            /// Restituisce un oggetto CCollection contenente gli utenti interessati alla modifica
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<CUser> GetAffectedUsers()
            {
                var dic = new Dictionary<int, UserAN>();
                int uID;
                bool a, n;
                foreach(var grpAN in this.GroupsAllowNegate)
                {
                    var grp = grpAN.Group;
                    if (grp is null || grp.Stato != ObjectStatus.OBJECT_VALID)
                        continue;
                    a = grpAN.Allow; n = grpAN.Negate;
                    foreach(var u in grp.Members)
                    {
                        uID = DBUtils.GetID(u, 0);
                        if (u.Stato == ObjectStatus.OBJECT_VALID)
                        {
                            UserAN an = null;
                            if (!dic.TryGetValue(uID, out an))
                            {
                                an = new UserAN();
                                an.User = u;
                                an.Group = grp;
                                an.GroupAllow = an.GroupAllow | grpAN.Allow;
                                an.GroupNegate = an.GroupNegate | grpAN.Negate;
                                dic.Add(uID, an);
                            }

                            a = false; n = false;
                            this.UsersAllowNegate.GetAllowNegate(u, ref a, ref n);
                            an.UserAllow = an.UserAllow | a;
                            an.UserNegate = an.UserNegate | n;
                        }
                    }

                }

                foreach(var usrAN in this.UsersAllowNegate)
                {
                    var u = usrAN.User;
                    uID = DBUtils.GetID(u, 0);
                    if (u.Stato == ObjectStatus.OBJECT_VALID)
                    {
                        UserAN an = null;
                        if (!dic.TryGetValue(uID, out an))
                        {
                            an = new UserAN();
                            an.User = u;
                            dic.Add(uID, an);
                        }

                        a = false; n = false;
                        this.UsersAllowNegate.GetAllowNegate(u, ref a, ref n);
                        an.UserAllow = an.UserAllow | a;
                        an.UserNegate = an.UserNegate | n;
                    }
                }

                var ret = new CCollection<CUser>();
                foreach(var item in dic.Values)
                {
                    if ((item.UserAllow | item.GroupAllow) && !(item.UserNegate | item.GroupNegate))
                        ret.Add(item.User);
                }
                return ret;
            }
              
            /// <summary>
            /// Restituisce true se l'utente è abilitato a visualizzare la circolare
            /// Questo accade se l'utente o uno dei gruppi a cui appartiene ha un'autorizzazione esplicita
            /// e se non esiste una negazione esplicita per l'utente o per uno dei gruppi a cui appartiene
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public bool IsUserAllowed(CUser user)
            {
                bool a = false, n = false;
                this.UsersAllowNegate.GetAllowNegate(user, ref a, ref n);
                if (n) return false;
                foreach(var grp in user.Groups)
                {
                    this.GroupsAllowNegate.GetAllowNegate(grp, ref a, ref n);
                    if (n) return false;
                }
                return a && !n;
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("DataInizio", m_DataInizio);
                writer.WriteAttribute("DataFine", m_DataFine);
                writer.WriteAttribute("Categoria", m_Categoria);
                writer.WriteAttribute("Sottocategoria", m_Sottocategoria);
                writer.WriteAttribute("Titolo", m_Titolo);
                writer.WriteAttribute("Priorita", (int?)m_Priorita);
                writer.WriteAttribute("URL", m_URL);
                writer.WriteAttribute("PrimaPagina", m_PrimaPagina);
                writer.WriteAttribute("Progressivo", m_Progressivo);
                base.XMLSerialize(writer);
                writer.WriteTag("Note", m_Testo);
                writer.WriteTag("Attachments", Attachments);
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

                    case "Categoria": m_Categoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "Sottocategoria": m_Sottocategoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "Descrizione": m_Titolo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "Testo": m_Testo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "Priorita":
                        {
                            m_Priorita = (PriorityEnum)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "URL":
                        {
                            m_URL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "PrimaPagina":
                        {
                            m_PrimaPagina = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Progressivo":
                        {
                            m_Progressivo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Attachments":
                        {
                            m_Attachments = (CAttachmentsCollection)fieldValue;
                            m_Attachments.SetOwner(this);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            string[] IIndexable.GetIndexedWords()
            {
                var ret = new List<string>();
                string str = DMD.Strings.JoinW(Categoria , " " , Titolo , " " , DMD.WebUtils.RemoveHTMLTags(Testo));
                str = Strings.Replace(str, DMD.Strings.vbCrLf, " ");
                str = Strings.Replace(str, DMD.Strings.vbCr, " ");
                str = Strings.Replace(str, DMD.Strings.vbLf, " ");
                str = Strings.Replace(str, "  ", " ");
                var a = Strings.Split(str, " ");
                if (DMD.Arrays.Len(a) > 0)
                    ret.AddRange(a);
                return ret.ToArray();
            }

            string[] IIndexable.GetKeyWords()
            {
                var ret = new List<string>();
                ret.Add(Categoria);
                string str = Strings.Replace(Titolo, DMD.Strings.vbCrLf, " ");
                str = Strings.Replace(str, DMD.Strings.vbCr, " ");
                str = Strings.Replace(str, DMD.Strings.vbLf, " ");
                str = Strings.Replace(str, "  ", " ");
                var a = Strings.Split(str, " ");
                if (DMD.Arrays.Len(a) > 0)
                    ret.AddRange(a);
                return ret.ToArray();
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is Circolare) && this.Equals((Circolare)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(Circolare obj)
            {
                return base.Equals(obj)
                    && DMD.DateUtils.EQ(this.m_DataInizio, obj.m_DataInizio)
                    && DMD.DateUtils.EQ(this.m_DataFine, obj.m_DataFine)
                    && DMD.Strings.EQ(this.m_Categoria, obj.m_Categoria)
                    && DMD.Strings.EQ(this.m_Sottocategoria, obj.m_Sottocategoria)
                    && DMD.Strings.EQ(this.m_Titolo, obj.m_Titolo)
                    && DMD.Strings.EQ(this.m_Testo, obj.m_Testo)
                    && DMD.Integers.EQ((int)this.m_Priorita, (int)obj.m_Priorita)
                    && DMD.Strings.EQ(this.m_URL, obj.m_URL)
                    && DMD.Booleans.EQ(this.m_PrimaPagina, obj.m_PrimaPagina)
                    && DMD.Integers.EQ(this.m_Progressivo, obj.m_Progressivo)
                    && DMD.Strings.EQ(this.m_NumeroCircolare, obj.m_NumeroCircolare)
                    ;

            }
        }
    }
}