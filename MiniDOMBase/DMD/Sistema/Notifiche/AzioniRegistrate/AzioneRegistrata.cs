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

namespace minidom
{
    public partial class Sistema
    {




        /// <summary>
        /// Azione definita per il tipo di oggetto associato alla notifica (campo SourceName)
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class AzioneRegistrata 
            : Databases.DBObject, IComparable, IComparable<AzioneRegistrata>
        {
            private int m_Priorita;
            private string m_Description;
            private string m_Categoria;
            private string m_SourceName;
            private string m_ActionType;
            private string m_IconURL;
            private bool m_Attivo;

            /// <summary>
            /// Costruttore
            /// </summary>
            public AzioneRegistrata()
            {
                m_Priorita = 0;
                m_Description = "";
                m_SourceName = "";
                m_ActionType = "";
                m_Categoria = "";
                m_IconURL = "";
                m_Attivo = true;
            }

            /// <summary>
            /// Istanzia un nuovo oggetto AzioneEseguibile del tipo registrato
            /// </summary>
            /// <returns></returns>
            public AzioneEseguibile NewAzione()
            {
                return (AzioneEseguibile) minidom.Sistema.ApplicationContext.CreateInstance(this.ActionType);
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il gestore è abilitato o meno
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
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
            /// Restituisce o imposta un valore intero che viene utilizzato per ordinare in senso crescente le azioni
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int Priorita
            {
                get
                {
                    return m_Priorita;
                }

                set
                {
                    int oldValue = m_Priorita;
                    if (oldValue == value)
                        return;
                    m_Priorita = value;
                    DoChanged("Priorita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'immagine associata all'azione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string IconURL
            {
                get
                {
                    return m_IconURL;
                }

                set
                {
                    string oldValue = m_IconURL;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IconURL = value;
                    DoChanged("IconURL", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una descrizione per l'azione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Description
            {
                get
                {
                    return m_Description;
                }

                set
                {
                    string oldValue = m_Description;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Description = value;
                    DoChanged("Description", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la categoria della notifica a cui si applica l'azione.
            /// Se questo campo è vuoto l'azione si applica a tutte le categorie
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
                    string oldValue = m_Categoria;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Categoria = value;
                    DoChanged("Categoria", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo su cui è definita l'azione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SourceName
            {
                get
                {
                    return m_SourceName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_SourceName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SourceName = value;
                    DoChanged("SourceName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del tipo che definisce l'azione sugli oggetti SourceName
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ActionType
            {
                get
                {
                    return m_ActionType;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_ActionType;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ActionType = value;
                    DoChanged("ActionType", value, oldValue);
                }
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Priorita = reader.Read("Priorita", this. m_Priorita);
                this.m_IconURL = reader.Read("IconURL", this.m_IconURL);
                this.m_Description = reader.Read("Description", this.m_Description);
                this.m_SourceName = reader.Read("SourceType", this.m_SourceName);
                this.m_ActionType = reader.Read("ActionType", this.m_ActionType);
                this.m_Categoria = reader.Read("Categoria", this.m_Categoria);
                this.m_Attivo = reader.Read("Attivo", this.m_Attivo);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Priorita", m_Priorita);
                writer.Write("Description", m_Description);
                writer.Write("SourceType", m_SourceName);
                writer.Write("ActionType", m_ActionType);
                writer.Write("Categoria", m_Categoria);
                writer.Write("IconURL", m_IconURL);
                writer.Write("Attivo", m_Attivo);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);
                var c = table.Fields.Ensure("Priorita", typeof(int), 1);
                c = table.Fields.Ensure("Description", typeof(string), 255);
                c = table.Fields.Ensure("SourceType", typeof(string), 255);
                c = table.Fields.Ensure("ActionType", typeof(string), 255);
                c = table.Fields.Ensure("Categoria", typeof(string), 255);
                c = table.Fields.Ensure("IconURL", typeof(string), 255);
                c = table.Fields.Ensure("Attivo", typeof(bool), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.GetItemByName("idxStato");
                if (c is object)
                    c.Drop();
                c = table.Constraints.Ensure("idxSource", new string[] { "SourceType", "ActionType", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxDescription", new string[] { "Description" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxCategoria", new string[] { "Categoria", "Priorita", "Attivo" }, DBFieldConstraintFlags.PrimaryKey);
                //c = table.Fields.Ensure("IconURL", typeof(string), 255);
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Notifiche.Database;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Notifiche.AzioniRegistrateRepository;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_SYSNotifyAct";
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Priorita", m_Priorita);
                writer.WriteAttribute("Description", m_Description);
                writer.WriteAttribute("SourceName", m_SourceName);
                writer.WriteAttribute("ActionType", m_ActionType);
                writer.WriteAttribute("Categoria", m_Categoria);
                writer.WriteAttribute("IconURL", m_IconURL);
                writer.WriteAttribute("Attivo", m_Attivo);
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
                    case "Priorita":
                        {
                            m_Priorita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Description":
                        {
                            m_Description = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceName":
                        {
                            m_SourceName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ActionType":
                        {
                            m_ActionType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Categoria":
                        {
                            m_Categoria = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Attivo":
                        {
                            m_Attivo = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            ///// <summary>
            ///// OnAfterSave
            ///// </summary>
            ///// <param name="e"></param>
            //protected override void OnAfterSave(DMDEventArgs e)
            //{
            //    base.OnAfterSave(e);
            //    lock (minidom.Sistema.Notifiche.cacheLock)
            //    {
            //        var item = minidom.Sistema.Notifiche.RegisteredActions.GetItemById(DBUtils.GetID(this));
            //        if (item is object)
            //            minidom.Sistema.Notifiche.RegisteredActions.Remove(item);
            //        if (Stato == ObjectStatus.OBJECT_VALID)
            //        {
            //            minidom.Sistema.Notifiche.RegisteredActions.Add(this);
            //            minidom.Sistema.Notifiche.RegisteredActions.Sort();
            //        }
            //    }
            //}

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(AzioneRegistrata other)
            {
                int ret = m_Priorita - other.m_Priorita;
                if (ret == 0)
                    ret = DMD.Strings.Compare(m_Description, other.m_Description, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((AzioneRegistrata)obj);
            }
        }
    }
}