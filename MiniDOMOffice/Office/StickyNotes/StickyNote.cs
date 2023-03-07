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
        /// Flag per gli oggetti <see cref="StickyNote"/>
        /// </summary>
        [Flags]
        public enum StickyNoteFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Primo piano
            /// </summary>
            TopMost = 1
        }

        /// <summary>
        /// Nota visualizzata sul desktop
        /// </summary>
        [Serializable]
        public class StickyNote 
            : Databases.DBObjectPO
        {
            private string m_Text;

            /// <summary>
            /// Costruttore
            /// </summary>
            public StickyNote()
            {
                this.m_Text = "";
                this.m_Flags = (int)StickyNoteFlags.None;
            }

            /// <summary>
            /// Restituisce o imposta il testo della nota
            /// </summary>
            public string Text
            {
                get
                {
                    return m_Text;
                }

                set
                {
                    string oldValue = m_Text;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Text = value;
                    DoChanged("Text", value, oldValue);
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public new StickyNoteFlags Flags
            {
                get
                {
                    return (StickyNoteFlags) base.Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }
             
            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.StickyNotes;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeStickyNotes";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Text = reader.Read("Text", m_Text);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Text", m_Text);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);
                var c = table.Fields.Ensure("Text", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxText", new string[] { "Text" } ,   DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                base.XMLSerialize(writer);
                writer.WriteTag("Text", m_Text);
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
                    case "Text":
                        {
                            m_Text = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Text;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Text);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is StickyNote) && this.Equals((StickyNote)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(StickyNote obj)
            {
                return base.Equals(obj)
                    && Strings.EQ(this.m_Text, obj.m_Text);
            }
        }
    }
}