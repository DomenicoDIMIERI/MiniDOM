using System;
using DMD;
using DMD.XML;
using DMD.Databases;
using minidom;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Elemento di una lista di ricontatti
        /// </summary>
        [Serializable]
        public class ListaRicontattoItem 
            : CRicontatto
        {

            private string m_NomeLista;
            private int m_IDLista;
            private CListaRicontatti m_ListaRicontatti;

            /// <summary>
            /// Costruttore
            /// </summary>
            public ListaRicontattoItem()
            {
                this.m_NomeLista = "";
                this.m_IDLista = 0;
                this.m_ListaRicontatti = null;
            }

            /// <summary>
            /// Restituisce o imposta il nome della coda di ricontatti a cui appartiene questo oggetto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeLista
            {
                get
                {
                    return m_NomeLista;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeLista;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeLista = value;
                    DoChanged("NomeLista", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della lista di ricontatti
            /// </summary>
            public int IDListaRicontatti
            {
                get
                {
                    return DBUtils.GetID(this.m_ListaRicontatti, this.m_IDLista);
                }
                set
                {
                    int oldValue = this.IDListaRicontatti;
                    if (oldValue == value) return;
                    this.m_IDLista = value;
                    this.m_ListaRicontatti = null;
                    this.DoChanged("IDListaRicontatti", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la lista di ricontatti
            /// </summary>
            public CListaRicontatti ListaRicontatti
            {
                get
                {
                    if (this.m_ListaRicontatti is null) this.m_ListaRicontatti = minidom.Anagrafica.ListeRicontatto.GetItemById(this.m_IDLista);
                    return this.m_ListaRicontatti;
                }
                set
                {
                    var oldValue = this.m_ListaRicontatti;
                    if (oldValue == value) return;
                    this.m_ListaRicontatti = value;
                    this.m_IDLista = DBUtils.GetID(value, this.m_IDLista);
                    this.m_NomeLista = (value is null) ? string.Empty : value.Name;
                    this.DoChanged("ListaRicontatti", value, oldValue);
                }
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.ListeRicontatto.Items; //.Module;
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(CRicontatto obj)
            {
                return (obj is ListaRicontattoItem) && this.Equals((ListaRicontattoItem)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(ListaRicontattoItem obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_NomeLista, obj.m_NomeLista)
                    && DMD.Integers.EQ(this.IDListaRicontatti, obj.IDListaRicontatti);
            }


            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_ListeRicontattoItems";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_NomeLista = reader.Read("NomeLista", this.m_NomeLista);
                this.m_IDLista = reader.Read("IDLista ", this.m_IDLista);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("NomeLista", this.m_NomeLista);
                writer.Write("IDLista", this.IDListaRicontatti);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("NomeLista", typeof(string), 255);
                c = table.Fields.Ensure("IDLista", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxLista", new string[] { "IDLista", "NomeLista"}, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.m_NomeLista,  " - ", base.ToString());
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_NomeLista, base.GetHashCode());
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("NomeLista", m_NomeLista);
                writer.WriteAttribute("IDLista", this.IDListaRicontatti);
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
                    case "NomeLista":
                        {
                            m_NomeLista = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    case "IDLista":
                        {
                            this.m_IDLista = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return ListeRicontatto.Database;
            //}

            //protected override void NotifyCreated()
            //{
            //    ListeRicontatto.Items.doItemCreated(new ItemEventArgs(this));
            //}

            //protected override void NotifyDeleted()
            //{
            //    ListeRicontatto.Items.doItemDeleted(new ItemEventArgs(this));
            //}

            //protected override void NotifyModified()
            //{
            //    ListeRicontatto.Items.doItemModified(new ItemEventArgs(this));
            //}

            //protected override void MirrorMe(Databases.CDBConnection dbConn)
            //{
            //    // MyBase.MirrorMe(dbConn)
            //}
        }
    }
}