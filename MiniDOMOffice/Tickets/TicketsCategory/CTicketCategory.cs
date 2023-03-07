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
        /// Rappresenta una categoria e sottocategoria per una richiesta di assistenza
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CTicketCategory 
            : Databases.DBObject, IComparable, IComparable<CTicketCategory>
        {
            private string m_Categoria;
            private string m_Sottocategoria;
            private CCollection<CUser> m_NotifyUsers;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTicketCategory()
            {
                m_Categoria = DMD.Strings.vbNullString;
                m_Sottocategoria = DMD.Strings.vbNullString;
                m_NotifyUsers = null;
            }

            /// <summary>
            /// Compara due oggetti per categoria e sottocategoria in ordine crescente
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(CTicketCategory other)
            {
                var ret = DMD.Strings.Compare(this.m_Categoria, other.m_Categoria, true);
                if (ret == 0) ret = DMD.Strings.Compare(this.m_Sottocategoria, other.m_Sottocategoria, true);
                return ret;
            }

            int IComparable.CompareTo(object obj) { return this.CompareTo((CTicketCategory)obj); }

            /// <summary>
            /// Restituisce l'elenco degli utenti a cui vengono notificate le richieste di assistenza per questo tipo di categoria
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<CUser> NotifyUsers
            {
                get
                {
                    lock (this)
                    {
                        if (m_NotifyUsers is null)
                            m_NotifyUsers = new CCollection<CUser>();
                        return m_NotifyUsers;
                    }
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della categoria
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
            /// Restituisce o imposta il nome della sottocategoria
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Sottocategoria
            {
                get
                {
                    return m_Sottocategoria;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Sottocategoria;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Sottocategoria = value;
                    DoChanged("Sottocategoria", value, oldValue);
                }
            }

            private string SerializeUsers()
            {
                var ret = new System.Text.StringBuilder();
                foreach (CUser u in NotifyUsers)
                {
                    if (ret.Length > 0)
                        ret.Append(",");
                    ret.Append(DBUtils.GetID(u, 0));
                }

                return ret.ToString();
            }

            private CCollection<Sistema.CUser> DeserializeUsers(string str)
            {
                var ret = new CCollection<Sistema.CUser>();
                str = DMD.Strings.Trim(str);
                var ids = Strings.Split(str, ",");
                if (DMD.Arrays.Len(ids) > 0)
                {
                    foreach (string id in ids)
                    {
                        var user = Sistema.Users.GetItemById(Sistema.Formats.ToInteger(id));
                        if (user is object)
                            ret.Add(user);
                    }
                }

                return ret;
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("{ " , this.m_Categoria , ", " , this.m_Sottocategoria  , " }");
            }

            /// <summary>
            /// Restutyusce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Categoria);
            }

            /// <summary>
            /// Restitusice true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CTicketCategory) && this.Equals((CTicketCategory)obj);
            }

            /// <summary>
            /// Restitusice true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CTicketCategory obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Categoria, obj.m_Categoria)
                    && DMD.Strings.EQ(this.m_Sottocategoria, obj.m_Sottocategoria)
                    ;
                    //m_NotifyUsers;
            }


            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Categoria = reader.Read("Categoria", m_Categoria);
                m_Sottocategoria = reader.Read("Sottocategoria", m_Sottocategoria);
                var tmp = reader.Read("NotifyUsers", "");
                m_NotifyUsers = DeserializeUsers(tmp);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Categoria", m_Categoria);
                writer.Write("Sottocategoria", m_Sottocategoria);
                writer.Write("NotifyUsers", SerializeUsers());
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Categoria", typeof(string), 255);
                c = table.Fields.Ensure("Sottocategoria", typeof(string), 255);
                c = table.Fields.Ensure("NotifyUsers", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxCategoria", new string[] { "Categoria", "Sottocategoria", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                //c = table.Fields.Ensure("NotifyUsers", typeof(string), 0);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Categoria", m_Categoria);
                writer.WriteAttribute("Sottocategoria", m_Sottocategoria);
                base.XMLSerialize(writer);
                writer.WriteTag("NotifyUsers", SerializeUsers());
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
                    case "Categoria":
                        {
                            m_Categoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Sottocategoria":
                        {
                            m_Sottocategoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NotifyUsers":
                        {
                            m_NotifyUsers = DeserializeUsers(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
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
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.TicketCategories;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_SupportTicketsCat";
            }

            /// <summary>
            /// Gestisce l'evento AfterSave
            /// </summary>
            /// <param name="e"></param>
            protected override void OnAfterSave(DMDEventArgs e)
            {
                base.OnAfterSave(e);
                minidom.Office.TicketCategories.InvalidateUserAllowedCategories();
            }
        }
    }
}