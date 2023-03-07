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
using minidom.internals;

namespace minidom
{
    namespace internals
    {


        /// <summary>
        /// Rappresenta un oggetto indicizzato tbl_Index
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CachedIndex
            : Databases.DBObjectBase, IComparable, IComparable<CachedIndex>
        {
            [NonSerialized] private CIndexingService m_Owner;
            private string m_ObjectType;
            private int m_ObjectID;
            private string m_Word;
            private int? m_Frequenza;
                        
            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            public CachedIndex(CIndexingService owner)
            {
                //IncreaseCounter(this);
                if (owner is null)
                    throw new ArgumentNullException("owner");
                this.m_Owner = owner;
                this.m_ObjectType = "";
                this.m_ObjectID = 0;
                this.m_Word = "";
                this.m_Frequenza = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="frequenza"></param>
            /// <param name="objectID"></param>
            /// <param name="objectType"></param>
            /// <param name="word"></param>
            public CachedIndex(
                                CIndexingService owner,
                                string word, 
                                string objectType, 
                                int objectID, 
                                int frequenza
                                )
            {
                //IncreaseCounter(this);
                if (owner is null)
                    throw new ArgumentNullException("owner");
                this.m_Owner = owner;
                this.m_ObjectType = objectType;
                this.m_ObjectID = objectID ;
                this.m_Word = word;
                this.m_Frequenza = frequenza;
            }


            /// <summary>
            /// Restituisce un riferimento al servizio che gestisce l'oggetto
            /// </summary>
            public CIndexingService Owner
            {
                get
                {
                    return m_Owner;
                }
            }

            /// <summary>
            /// Parola indicizzata
            /// </summary>
            public string Word
            {
                get
                {
                    return this.m_Word;
                }
            }

            /// <summary>
            /// Frequenza
            /// </summary>
            public int? Frequenza
            {
                get
                {
                    return this.m_Frequenza;
                }
            }

            /// <summary>
            /// Imposta il riferimento al servizio che gestisce l'oggetto
            /// </summary>
            /// <param name="owner"></param>
            protected internal void SetOwner(CIndexingService owner)
            {
                m_Owner = owner;
            }

            /// <summary>
            /// Restituisce la parola indicizzata
            /// </summary>
            public int ObjectID 
            {
                get
                {
                    return this.m_ObjectID;
                }
            }

                   

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(CachedIndex other)
            {
                return this.m_ObjectID.CompareTo(other.m_ObjectID);
            }

            int IComparable.CompareTo(object obj)
            {
                return this.CompareTo((CachedIndex)obj);
            }

            /// <summary>
            /// Restituisce la connessione al db del servizio
            /// </summary>
            /// <returns></returns>
            protected override DBConnection GetConnection()
            {
                return this.Owner.Database;
            }

            /// <summary>
            /// Nessun repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return null;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return this.Owner.IndexTableName;
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_ObjectID = reader.Read("ObjectID", this.m_ObjectID);
                this.m_Word = reader.Read("Word", this.m_Word);
                this.m_Frequenza = reader.Read("Rank", this.m_Frequenza);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("ObjectID", this.ObjectID);
                writer.Write("Word", this.m_Word);
                writer.Write("Rank", this.m_Frequenza);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("ObjectID", typeof(int), 1);
                c = table.Fields.Ensure("Word", typeof(string), 255);
                c = table.Fields.Ensure("Rank", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxObject", new string[] { "ObjectID", "Word" }, DBFieldConstraintFlags.PrimaryKey);
                //c = table.Fields.Ensure("Rank", typeof(int), 1);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("ObjectID", this.ObjectID);
                writer.WriteAttribute("Word", this.m_Word);
                writer.WriteAttribute("Rank", this.m_Frequenza);
                base.XMLSerialize(writer);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName)
                {
                    case "ObjectID": this.m_ObjectID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue); break;
                    case "Word": this.m_Word = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "Rank": this.m_Frequenza = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue); break;
                    default: base.SetFieldInternal(fieldName, fieldValue); break;
                }
                
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_Word;
            }

            /// <summary>
            /// Calcola il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Word); 
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObjectBase obj)
            {
                return (obj is CachedIndex) && this.Equals((CachedIndex)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CachedIndex obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_ObjectType, obj.m_ObjectType)
                    && DMD.Integers.EQ(this.m_ObjectID, obj.m_ObjectID)
                    && DMD.Strings.EQ(this.m_Word, obj.m_Word)
                    && DMD.Integers.EQ(this.m_Frequenza, obj.m_Frequenza)
                    ;
            }
        }


    }
}