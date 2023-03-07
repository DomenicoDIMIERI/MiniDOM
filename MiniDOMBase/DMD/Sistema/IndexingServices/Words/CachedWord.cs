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
        /// Rappresenta una parola indicizzata
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CachedWord
            : Databases.DBObjectBase, IComparable, IComparable<CachedWord>
        {
            [NonSerialized] private CIndexingService m_Owner;
            private string m_Word;
            private DateTime? m_FirstUsed;
            private DateTime? m_LastUsed;
            private int? m_Frequenza;
            private int m_ConteggioUtilizzi;
            private int[] m_Indexes;

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            public CachedWord(CIndexingService owner)
            {
                //IncreaseCounter(this);
                if (owner is null)
                    throw new ArgumentNullException("owner");
                this.m_Owner = owner;
                this.m_Word = "";
                this.m_FirstUsed = default;
                this.m_LastUsed = default;
                this.m_Frequenza = default;
                this.m_ConteggioUtilizzi = 0;
                this.m_Indexes = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="word"></param>
            public CachedWord(CIndexingService owner, string word) 
                : this(owner)
            {
                m_Word = DMD.Strings.Left(DMD.Strings.Trim(DMD.Strings.UCase(word)), 255);
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
            public string Word
            {
                get
                {
                    return m_Word;
                }
            }

            /// <summary>
            /// Restituisce l'ultima data di utilizzo
            /// </summary>
            public DateTime? LastUsed
            {
                get
                {
                    return this.m_LastUsed;
                }
            }

            /// <summary>
            /// Restituisce la prima data di utilizzo
            /// </summary>
            public DateTime? FirstUsed
            {
                get
                {
                    return this.m_FirstUsed;
                }
            }

            /// <summary>
            /// Restituisce la frequenza
            /// </summary>
            public int? Frequenza
            {
                get
                {
                    return this.m_Frequenza;
                }
            }

            /// <summary>
            /// Restituisce il conteggio degli utilizzi
            /// </summary>
            public int? ConteggioUtilizzi
            {
                get
                {
                    return this.m_ConteggioUtilizzi;
                }
            }



            /// <summary>
            /// Restituisce gli id di tutti gli elementi indicizzati per la parola
            /// </summary>
            public int[] GetIndice
            {
                get
                {
                    lock (this)
                    {
                        if (m_Frequenza.HasValue == false)
                        {
                            Rebuild();
                            Save();
                        }

                        if (m_Indexes is null)
                            m_Indexes = new int[] { };
                        return m_Indexes;
                    }
                }
            }

            /// <summary>
            /// Restituisce l'indice base 0 dell'elemento nell'array degli indici sul db
            /// </summary>
            /// <param name="objectID"></param>
            /// <returns></returns>
            public int GetIndexedPosition(int objectID)
            {
                lock (this)
                    return DMD.Arrays.BinarySearch(GetIndice, objectID);
            }

            /// <summary>
            /// Restituisce true se l'id dell'oggetto è presente nell'indice sul db
            /// </summary>
            /// <param name="objectID"></param>
            /// <returns></returns>
            public bool IsIndexed(int objectID)
            {
                return GetIndexedPosition(objectID) >= 0;
            }

            /// <summary>
            /// Indicizza l'oggetto
            /// </summary>
            /// <param name="objectID"></param>
            /// <returns></returns>
            public int Indicizza(int objectID)
            {
                lock (this)
                {
                    int p = GetIndexedPosition(objectID);
                    if (p >= 0)
                        return p;
                    this.m_Indexes = this.GetIndice;
                    p = DMD.Arrays.GetInsertPosition(this.m_Indexes, objectID, 0, m_Indexes.Length);
                    this.m_Indexes = DMD.Arrays.Insert(this.m_Indexes, 0, m_Indexes.Length, objectID, p);
                    /* TODO ERROR: Skipped IfDirectiveTrivia */
                    if (p > 0 && p < DMD.Arrays.UBound(m_Indexes))
                    {
                        System.Diagnostics.Debug.Assert(m_Indexes[p - 1] < m_Indexes[p]);
                        System.Diagnostics.Debug.Assert(m_Indexes[p] < m_Indexes[p + 1]);
                    }
                    else if (p > 0)
                    {
                        System.Diagnostics.Debug.Assert(m_Indexes[p - 1] < m_Indexes[p]);
                    }
                    else if (p < DMD.Arrays.UBound(m_Indexes))
                    {
                        System.Diagnostics.Debug.Assert(m_Indexes[p] < m_Indexes[p + 1]);
                    }

                    /* TODO ERROR: Skipped EndIfDirectiveTrivia */
                    m_Frequenza = DMD.Arrays.Len(m_Indexes);
                    SetChanged(true);
                    return p;
                }
            }

            /// <summary>
            /// Rimuove l'indice
            /// </summary>
            /// <param name="objectID"></param>
            /// <returns></returns>
            public int UnIndicizza(int objectID)
            {
                lock (this)
                {
                    int p = GetIndexedPosition(objectID);
                    if (p < 0)
                        return -1;
                    this.m_Indexes = DMD.Arrays.RemoveAt(this.m_Indexes, p);
                    this.m_Frequenza = DMD.Arrays.Len(this.m_Indexes);
                    SetChanged(true);
                    return p;
                }
            }

            /// <summary>
            /// Ricrea l'indice
            /// </summary>
            public void Rebuild()
            {
                lock (this)
                {
                    if (Owner is null)
                        throw new ArgumentException("Owner");
                    var conn = this.Owner.Database;
                    if (conn is null)
                        throw new ArgumentNullException("conn");

                    var lst = new List<int>(1024);
                    using (var cursor = new CachedIndexCursor(this.Owner))
                    {
                        cursor.IgnoreRights = true;
                        cursor.Word.Value = this.m_Word;
                        while (cursor.Read())
                        {
                            lst.Add(cursor.Item.ObjectID);
                        }
                    }
                    lst.Sort();
                    this.m_Indexes = lst.ToArray();
                }

            }

            /// <summary>
            /// Carica l'indice dal db
            /// </summary>
            public void Load()
            {
                if (Owner is null)
                    throw new ArgumentNullException("Owner");

                var conn = this.Owner.Database;
                if (conn is null)
                    throw new ArgumentNullException("conn");
                
                using(var cursor = new CachedWordCursor(this.Owner))
                {
                    cursor.Word.Value = this.m_Word;
                    cursor.IgnoreRights = true;
                    this.m_FirstUsed = cursor.Item.m_FirstUsed;
                    this.m_LastUsed = cursor.Item.m_LastUsed;
                    this.m_Frequenza = cursor.Item.m_Frequenza;
                    this.m_ConteggioUtilizzi = cursor.Item.m_ConteggioUtilizzi;
                    this.m_Indexes = cursor.Item.m_Indexes;
                }
                        
            }

            private string GetWordIndexFileName()
            {
                if (DBUtils.GetID(this, 0) == 0)
                    return "";
                return System.IO.Path.Combine(
                                Owner.WordIndexFolder, 
                                DMD.Strings.ConcatArray("W" , DBUtils.GetID(this) , ".idx")
                                );
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Word = reader.Read("Word", this. m_Word);
                this.m_Frequenza = reader.Read("Frequenza", this.m_Frequenza);
                this.m_FirstUsed = reader.Read("FirstUsed", this.m_FirstUsed);
                this.m_LastUsed = reader.Read("LastUsed", this.m_LastUsed);
                this.m_ConteggioUtilizzi = reader.Read("ConteggioUtilizzi", this.m_ConteggioUtilizzi);
                base.LoadFromRecordset(reader);

                if (string.IsNullOrEmpty(this.Owner.WordIndexFolder))
                {
                    this.m_Indexes = reader.Read("Indice", this.m_Indexes);
                }
                else
                {
                    string fileName = GetWordIndexFileName();

                    if (System.IO.File.Exists(fileName))
                    {
                        this.m_Indexes = this.LoadIndexFromFile(fileName); // reader.Read("Indice", Me.m_Indexes)
                    }
                    else
                    {
                        this.m_Indexes = DMD.Arrays.Empty<int>();
                    }
                }
                return true;
            }

            private int[] LoadIndexFromFile(string fileName)
            {
                byte[] bytes = null;
                var ret = DMD.Arrays.Empty<int>();

                using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
                {
                    if (stream.Length > 0L)
                    {
                        var reader = new System.IO.BinaryReader(stream);
                        bytes = (byte[])Array.CreateInstance(typeof(byte), stream.Length);
                        reader.Read(bytes, 0, bytes.Length);
                        ret = (int[])Array.CreateInstance(typeof(int), (int)(bytes.Length / 4d));
                        Buffer.BlockCopy(bytes, 0, ret, 0, bytes.Length);
                    }
                }
                return ret;
            }

            private void SaveIndexToFile(string fileName)
            {
                byte[] bytes = null;
                using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                {  
                    var writer = new System.IO.BinaryWriter(stream);
                    // writer.Write()
                    bytes = (byte[])Array.CreateInstance(typeof(byte), m_Indexes.Length * 4);
                    Buffer.BlockCopy(m_Indexes, 0, bytes, 0, bytes.Length);
                    writer.Write(bytes, 0, bytes.Length);
                }
            }

            /// <summary>
            /// Salva nel recordset
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Word", m_Word);
                if (!string.IsNullOrEmpty(Owner.WordIndexFolder))
                {
                    writer.Write("Indice", DBNull.Value);
                }
                else
                {
                    writer.Write("Indice", GetIndice);
                }

                writer.Write("Frequenza", m_Frequenza);
                writer.Write("FirstUsed", this.m_FirstUsed);
                writer.Write("LastUsed", this.m_LastUsed);
                writer.Write("ConteggioUtilizzi", this.m_ConteggioUtilizzi);
                base.SaveToRecordset(writer);
                if (!string.IsNullOrEmpty(Owner.WordIndexFolder))
                {
                    string fileName = GetWordIndexFileName();
                    // minidom.Sistema.FileSystem.CreateFolder(Me.Owner.WordIndexFolder)
                    SaveIndexToFile(fileName);
                }
                return true;
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Word", typeof(string), 255);
                c = table.Fields.Ensure("Indice", typeof(int), 1);
                c = table.Fields.Ensure("Frequenza", typeof(int), 1);
                c = table.Fields.Ensure("FirstUsed", typeof(DateTime), 1);
                c = table.Fields.Ensure("LastUsed", typeof(DateTime), 1);
                c = table.Fields.Ensure("ConteggioUtilizzi", typeof(int), 1);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxWord", new string[] { "Word" },  DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxStats", new string[] { "Frequenza", "FirstUsed", "LastUsed", "ConteggioUtilizzi" }, DBFieldConstraintFlags.PrimaryKey);
            }



            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(CachedWord other)
            {
                int ret = -DMD.DateUtils.Compare(m_LastUsed, other.m_LastUsed);
                if (ret == 0)
                    ret = other.m_ConteggioUtilizzi - m_ConteggioUtilizzi;
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return this.CompareTo((CachedWord)obj);
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
                return this.Owner.TableStatsName;
            }

            /// <summary>
            /// Aggiorna i conteggi
            /// </summary>
            public void Consume()
            {
                var d = DMD.DateUtils.Now();
                this.m_FirstUsed = DMD.DateUtils.Min(d, this.m_FirstUsed);
                this.m_LastUsed = DMD.DateUtils.Max(d, this.m_LastUsed);
                this.m_ConteggioUtilizzi += 1;
            }

            //~CachedWord()
            //{
            //    DecreaseCounter(this);
            //}

            /// <summary>
            /// Restituisce una stringa  he rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_Word;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
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
                return base.Equals(obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CachedWord obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Word, obj.m_Word)
                    && DMD.DateUtils.EQ(this.m_FirstUsed, obj.m_FirstUsed)
                    && DMD.DateUtils.EQ(this.m_LastUsed, obj.m_LastUsed)
                    && DMD.Integers.EQ(this.m_Frequenza, obj.m_Frequenza)
                    && DMD.Integers.EQ(this.m_ConteggioUtilizzi, obj.m_ConteggioUtilizzi)
                    //&& DMD.Arrays.EQ(this.m_Indexes, obj.m_Indexes)
                    ;
            }   
        }


    }
}