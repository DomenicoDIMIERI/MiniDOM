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
        /// Sistema di indicizzazione degli oggetti gestiti da un modulo
        /// </summary>
        public class CIndexingService
        {
            /// <summary>
            /// Oggetto usato per sincronizzare la cache
            /// </summary>
            public readonly object lockObject = new object();

            private static readonly char[] chars = _initChars();

            private static char[] _initChars()
            {
                var ret = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
                DMD.Arrays.Sort(ret);
                return ret;
            }

           

            
            

          

            
            

            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            private DBConnection m_Db = null;
            private int m_MAXCACHESIZE = 100;
            private float m_UnloadFactor = 0.25f;
            private string m_WordIndexFolder = "";
            private CKeyCollection<CachedWord> m_CachedWords = null;    // Collezione delle parole indicizzate
            private Queue queue = new Queue();
            private System.Threading.Thread indexingThread;
            private System.Threading.ManualResetEvent sem = new System.Threading.ManualResetEvent(false);
            private string m_TableStatsName = "tbl_WordStats";
            private string m_IndexTableName = "tbl_Index";

            /// <summary>
            /// Costruttore
            /// </summary>
            public CIndexingService()
            {
                indexingThread = new System.Threading.Thread(indexing_proc);
                ////DMDObject.IncreaseCounter(this);
                indexingThread.Start();
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="conn"></param>
            /// <param name="tableStatsName"></param>
            /// <param name="indexTableName"></param>
            public CIndexingService(DBConnection conn, string tableStatsName, string indexTableName) 
                : this()
            {
                if (conn is null)
                    throw new ArgumentNullException("conn");
                if (string.IsNullOrEmpty(tableStatsName))
                    throw new ArgumentNullException("tableStatsName");
                this.m_Db = conn;
                this.m_TableStatsName = tableStatsName;
                this.m_IndexTableName = indexTableName;
            }

            /// <summary>
            /// Restituisce il nome della tabella utilizzata per memorizzare gli indici
            /// </summary>
            public string TableStatsName
            {
                get
                {
                    return this.m_TableStatsName;
                }
            }

            /// <summary>
            /// Restituisce il nome della tabella utilizzata per memorizzare gli indici
            /// </summary>
            public string IndexTableName
            {
                get
                {
                    return this.m_IndexTableName;
                }
            }

            private void indexing_proc()
            {
                while (true)
                {
                    sem.WaitOne();
                    lock (queue)
                    {
                        if (queue.Count > 0)
                        {
                            object o;
                            o = queue.Dequeue();
#if (!DEBUG)
                            try {
#endif
                                Index(o, GetIndexableWords(o));
#if (!DEBUG)
                            } catch (Exception ex) {
                                Sistema.Events.NotifyUnhandledException(ex);
                            }
#endif
                        }
                    }
                }
            }

            /// <summary>
            /// Restituisce o imposta la collezione delle parole indicizzate
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CKeyCollection<CachedWord> CachedWords
            {
                get
                {
                    lock (lockObject)
                    {
                        if (m_CachedWords is null)
                        {
                            m_CachedWords = new CKeyCollection<CachedWord>();
                        }

                        return m_CachedWords;
                    }
                }
            }

            /// <summary>
            /// Restituisce la parola cached corrispondente
            /// </summary>
            /// <param name="word"></param>
            /// <returns></returns>
            public CachedWord GetCachedWord(string word)
            {
                lock (lockObject)
                {
                    word = DMD.Strings.Left(DMD.Strings.Trim(word), 255);
                    var ret = CachedWords.GetItemByKey(word);
                    if (ret is null)
                    {
                        ret = GetNonChangedWord(word);
                        AddToCache(ret);
                    }

                    ret.Consume();
                    return ret;
                }
            }

            /// <summary>
            /// Carica la parola dal db
            /// </summary>
            /// <param name="word"></param>
            /// <returns></returns>
            protected virtual CachedWord GetNonChangedWord(string word)
            {
                var ret = new CachedWord(this, word);
                ret.Load();
                return ret;
            }

            /// <summary>
            /// Restituisce o impsota la connessione al database che contiene le tabelle per l'indicizzazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DBConnection Database
            {
                get
                {
                    return m_Db;
                }

                set
                {
                    m_Db = value;
                }
            }

            /// <summary>
            /// Restituisce o imposta il percorso in cui vengono salvati gli indici relativi a ciascuna parola memorizzata
            /// Se il percorso è vuoto gli indici vengono memorizzati nel database
            /// </summary>
            /// <returns></returns>
            public string WordIndexFolder
            {
                get
                {
                    return m_WordIndexFolder;
                }

                set
                {
                    m_WordIndexFolder = DMD.Strings.Trim(value);
                }
            }

            /// <summary>
            /// Restituisce o imposta la dimensione massima della cache
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int MaxCacheSize
            {
                get
                {
                    return m_MAXCACHESIZE;
                }

                set
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("MaxCacheSize");
                    }

                    if (m_MAXCACHESIZE == value)
                        return;
                    lock (lockObject)
                    {
                        m_MAXCACHESIZE = value;
                        if (m_CachedWords is object)
                        {
                            if (m_MAXCACHESIZE > 0)
                            {
                                while (m_CachedWords.Count > m_MAXCACHESIZE)
                                    m_CachedWords.RemoveAt(m_MAXCACHESIZE);
                            }
                            else if (m_MAXCACHESIZE == 0)
                            {
                                m_CachedWords.Clear();
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Restituisce o imposta la percentuale della cache che viene svuotata quando si supera la dimensione massima
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public float UnloadFactor
            {
                get
                {
                    return m_UnloadFactor;
                }

                set
                {
                    if (m_UnloadFactor == value)
                        return;
                    lock (lockObject)
                    {
                        if (Maths.Floor(value * MaxCacheSize) < 1d)
                        {
                            throw new ArgumentNullException("UnloadFactor");
                        }

                        m_UnloadFactor = value;
                    }
                }
            }

            /// <summary>
            /// Restituisce vero l'oggetto è stato indicizzato
            /// </summary>
            /// <param name="o"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool IsIndexed(object o)
            {
                int objectID = DBUtils.GetID(o, 0);
                if (objectID == 0)
                    return false;

                using(var cursor = new CachedIndexCursor (this))
                {
                    cursor.IgnoreRights = true;
                    cursor.ObjectID.Value = objectID;
                    return cursor.Item is object;
                }
            }

            /// <summary>
            /// Restituisce true se la parola è contenuta nell'array ordinato
            /// </summary>
            /// <param name="items"></param>
            /// <param name="w"></param>
            /// <returns></returns>
            private bool IsIndexedWord(string[] items, string w)
            {
                return DMD.Arrays.BinarySearch(items, 0, DMD.Arrays.Len(items), w) >= 0;
            }

            /// <summary>
        /// Rimuove gli indici relativi all'oggetto
        /// </summary>
        /// <param name="o"></param>
        /// <remarks></remarks>
            public void Unindex(object o)
            {
                lock (lockObject)
                {
                    int objectID = DBUtils.GetID(o, 0);
                    var words = GetIndexedWords(o);
                    int i;
                    if (DMD.Arrays.Len(words) > 0)
                    {
                        GetWordStats(words);
                        var loopTo = DMD.Arrays.UBound(words);
                        for (i = 0; i <= loopTo; i++)
                        {
                            words[i].Frequenza -= 1;
                            var cw = GetCachedWord(words[i].Word);
                            cw.UnIndicizza(objectID);
                            cw.Save(true);

                        }

#if (!DEBUG)
                    try {
#endif
                        using (var cursor = new CachedIndexCursor(this))
                        {
                            cursor.IgnoreRights = true;
                            cursor.ObjectID.Value = objectID;
                            while (cursor.Read())
                            {
                                cursor.Item.Delete();
                            }
                        }
                    
#if (!DEBUG)
                    } catch (Exception ex) { }
#endif
                    }
                }
            }

            /// <summary>
            /// Restituisce le parole indicizzate per l'oggetto
            /// </summary>
            /// <param name="o"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public WordInfo[] GetIndexedWords(object o)
            {
                var lst = new List<WordInfo>();
                int objectID = DBUtils.GetID(o, 0);
                if (objectID != 0)
                {
                    using (var cursor = new CachedIndexCursor(this))
                    {
                        cursor.IgnoreRights = true;
                        cursor.ObjectID.Value = objectID;
                        while (cursor.Read())
                        {
                            var w = cursor.Item;
                            var wi = new WordInfo(w.Word);
                            wi.Frequenza = w.Frequenza;                            
                            lst.Add(wi);
                        }
                    }
                }
                return lst.ToArray();

            }

            /// <summary>
            /// Unisce le parole che vengono indicizzate con lo stesso simbolo
            /// </summary>
            /// <param name="words"></param>
            /// <returns></returns>
            public WordInfo[] MergeWords(string[] words)
            {
                var ret = new CKeyCollection<WordInfo>();
                MergeWords(ret, words);
                if (ret.Count > 1)
                {
                    ret[ret.Count - 1].IsLike = DMD.Strings.Len(ret[ret.Count - 1].Word) >= 3;
                }

                return ret.ToArray();
            }

            private void MergeWords(CKeyCollection<WordInfo> ret, string[] words)
            {
                WordInfo info;
                for (int i = 0, loopTo = DMD.Arrays.Len(words) - 1; i <= loopTo; i++)
                {
                    words[i] = DMD.Strings.UCase(DMD.Strings.Trim(words[i]));
                    words[i] = DMD.Strings.Replace(words[i], DMD.Strings.vbCrLf, " ");
                    words[i] = DMD.Strings.Replace(words[i], DMD.Strings.vbCr, " ");
                    words[i] = DMD.Strings.Replace(words[i], DMD.Strings.vbLf, " ");
                    words[i] = DMD.Strings.Replace(words[i], "  ", " ");
                    var w = DMD.Strings.Split(words[i], " ");
                    for (int j = 0, loopTo1 = DMD.Arrays.Len(w) - 1; j <= loopTo1; j++)
                    {
                        w[j] = DMD.Strings.OnlyCharsAndNumbers(w[j]);
                        if (!string.IsNullOrEmpty(w[j]))
                        {
                            info = ret.GetItemByKey(w[j]);
                            if (info is null)
                            {
                                info = new WordInfo();
                                info.Word = w[j];
                                info.Position = i;
                                ret.Add(w[j], info);
                            }

                            info.Frequenza += 1;
                        }
                    }
                }
            }

            /// <summary>
            /// Ottiene dall'oggetto le parole indicizzabili e le analizza per
            /// restituire l'array dei WordInfo che verrà effettivamente indicizzato
            /// </summary>
            /// <param name="o"></param>
            /// <returns></returns>
            public WordInfo[] GetIndexableWords(object o)
            {
                // Me.Index(o, Me.SplitWords(text))
                var ret = new CKeyCollection<WordInfo>();
                var tmpWords = ((Sistema.IIndexable)o).GetIndexedWords();
                var words = new List<string>();
                foreach (string s in tmpWords)
                {
                    words.Add(s);
                    var tmp = new System.Text.StringBuilder();
                    foreach (var ch in s)
                    {
                        if (DMD.Arrays.BinarySearch(chars, ch) >= 0)
                        {
                            tmp.Append(ch);
                        }
                        else if (tmp.Length > 0)
                        {
                            words.Add(tmp.ToString());
                            tmp.Clear();
                        }
                    }

                    if (tmp.Length > 0)
                        words.Add(tmp.ToString());
                }

                MergeWords(ret, words.ToArray());
                var ret1 = new CKeyCollection<WordInfo>();
                MergeWords(ret1, ((Sistema.IIndexable)o).GetKeyWords());
                foreach (WordInfo w in ret)
                {
                    if (ret1.ContainsKey(w.Word))
                    {
                        ret1.RemoveByKey(w.Word);
                    }
                }

                foreach (WordInfo w in ret1)
                {
                    var info = ret.GetItemByKey(w.Word);
                    if (info is null)
                    {
                        ret.Add(w.Word, w);
                    }
                    else
                    {
                        info.Frequenza += 1;
                    }
                }

                return ret.ToArray();
            }

            /// <summary>
            /// Indicizza l'oggetto
            /// </summary>
            /// <param name="o"></param>
            /// <remarks></remarks>
            public void Index(object o)
            {
                lock (queue)
                    queue.Enqueue(o);
                sem.Set();
            }

            private void Index(object o, WordInfo[] words, bool force = false)
            {
                lock (lockObject)
                {
                    string objectType = DMD.RunTime.vbTypeName(o);
                    int objectID = DBUtils.GetID(o, 0);

                    // Eliminiamo le vecchie parole
                    // Me.Unindex(o)
                    var oldWords = GetIndexedWords(o);

                    int i = 0;
                    if (!force)
                    {
                        while (i < DMD.Arrays.Len(words))
                        {
                            int i1 = -1;
                            for (int j = 0, loopTo = DMD.Arrays.Len(oldWords) - 1; j <= loopTo; j++)
                            {
                                if ((oldWords[j].Word ?? "") == (words[i].Word ?? ""))
                                {
                                    i1 = j;
                                    break;
                                }
                            }

                            if (i1 >= 0)
                            {
                                // joinWords.Add(words(i))
                                oldWords = DMD.Arrays.RemoveAt(oldWords, i1);
                                words = DMD.Arrays.RemoveAt(words, i);
                            }
                            else
                            {
                                i += 1;
                            }
                        }
                    }

                    // Eliminiamo le parole non più indicizzate
                    var loopTo1 = DMD.Arrays.Len(oldWords) - 1;
                    for (i = 0; i <= loopTo1; i++)
                    {
                        string word = DMD.Strings.UCase(DMD.Strings.Trim(oldWords[i].Word));
                        if (!string.IsNullOrEmpty(word))
                        {
                            oldWords[i].Frequenza -= 1;
                            var cw = GetCachedWord(word);
                            lock (cw)
                            {
                                cw.UnIndicizza(objectID);
                                cw.Save(true);
                            }

#if (!DEBUG)
                            try {
#endif
                            using(var cursor = new CachedIndexCursor(this))
                            {
                                cursor.IgnoreRights = true;
                                cursor.ObjectID.Value = objectID;
                                cursor.Word.Value = word;
                                while (cursor.Read())
                                {
                                    cursor.Item.Delete();
                                }
                            }                             
#if (!DEBUG)
                            } catch (Exception ex) { }
#endif
                        }
                    }

                    // Inseriamo le nuove parole nel database
                    var loopTo2 = DMD.Arrays.Len(words) - 1;
                    for (i = 0; i <= loopTo2; i++)
                    {
                        string word = DMD.Strings.UCase(DMD.Strings.Trim(words[i].Word));
                        if (!string.IsNullOrEmpty(word))
                        {
                            words[i].Frequenza += 1;
                            var cw = GetCachedWord(word);
                            lock (cw)
                            {
                                cw.Indicizza(objectID);
                                cw.Save(true);
                            }

#if (!DEBUG)
                            try {
#endif
                            var wi = new CachedIndex(this, word, objectType, objectID, -i);
                            wi.Save(true);
#if (!DEBUG)
                            } catch (Exception ex) {
                                Debug.Print(ex.Message);
                            }
#endif
                        }
                    }
                }
            }

         
        
            /// <summary>
            /// Analizza la frase ed estrae le parole indicizzabili
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public WordInfo[] SplitWords(string text)
            {
                text = DMD.Strings.UCase(text);
                text = DMD.Strings.Replace(text, "  ", " ");
                // text = Replace(text, "'", "")
                // text = Replace(text, " ", ";")
                // text = Replace(text, vbCr, ";")
                // text = Replace(text, vbLf, ";")

                var tmp = new System.Text.StringBuilder();
                foreach(var ch in  text)
                { 
                    if (DMD.Arrays.BinarySearch(chars, ch)>=0)
                    {
                        tmp.Append(ch);
                    }
                    else if (ch == ' ' || ch == ';')
                    {
                        tmp.Append( ';');
                    }
                    else
                    {
                        //tmp += "";
                    }
                }

                text = tmp.ToString();
                var ret = new CKeyCollection<WordInfo>();
                var tokenizer = new StringTokenizer(text, ";");
                while (tokenizer.hasMoreTokens())
                {
                    string token = tokenizer.nextToken();
                    WordInfo item;
                    if (tokenizer.hasMoreTokens() == false)
                    {
                        item = ret.GetItemByKey(token);
                        if (item is null)
                        {
                            item = new WordInfo();
                            ret.Add(token, item);
                            item.Word = token;
                            item.Frequenza += 1;
                            item.IsLike = true;
                        }
                        else
                        {
                            item = new WordInfo();
                            ret.Add(token, item);
                            item.Word = token;
                            item.Frequenza += 1;
                            item.IsLike = true;
                        }
                    }
                    else
                    {
                        item = ret.GetItemByKey(token);
                        if (item is null)
                        {
                            item = new WordInfo();
                            ret.Add(token, item);
                        }

                        item.Word = token;
                        item.Frequenza += 1;
                    }
                }

                return ret.ToArray();
            }

            // Private Function ParseWords(ByVal items() As String) As String()
            // Dim tmp As New CKeyCollection(Of String)
            // For i As Integer = 0 To UBound(items)
            // items(i) = DMD.Strings.OnlyChars(UCase(items(i)))
            // If tmp.ContainsKey(items(i)) = False Then tmp.Add(items(i), items(i))
            // Next
            // Dim ret() As String = Nothing
            // If (tmp.Count > 0) Then
            // ReDim ret(tmp.Count - 1)
            // tmp.CopyTo(ret, 0)
            // End If
            // Return ret
            // End Function

            private void GetWordStats(WordInfo[] words)
            {
                words = DMD.Arrays.Clone(words);
                DMD.Arrays.Sort(words);

                var wordsArr = new List<string>();
                foreach(var w in words )
                {
                    w.Frequenza = 0;
                    w.IsNew = true;
                    wordsArr.Add(w.Word);
                }
                

                using (var cursor = new CachedWordCursor(this))
                {
                    cursor.IgnoreRights = true;
                    cursor.Word.ValueIn(wordsArr.ToArray());
                    while (cursor.Read())
                    {
                        var ws = cursor.Item;
                        var i = DMD.Arrays.BinarySearch(words, new WordInfo(ws.Word));
                        var wi = words[i];
                        {
                            wi.Frequenza = ws.Frequenza; 
                            wi.IsNew = false;
                        }
                    }
                };
            }



            /// <summary>
            /// Cerca le parole
            /// </summary>
            /// <param name="words"></param>
            /// <returns></returns>
            public int[] Find(WordInfo[] words)
            {
                int[] ret = null;
                for (int i = 0, loopTo = DMD.Arrays.UBound(words); i <= loopTo; i++)
                {
                    ret = Find(words[i], ret);
                    if (ret is null || DMD.Arrays.UBound(ret) == -1)
                        return ret;
                }

                return ret;
            }

            /// <summary>
            /// Cerca le parole restringendo i risultati agli elementi dell'array specificato
            /// </summary>
            /// <param name="words"></param>
            /// <param name="findIn"></param>
            /// <returns></returns>
            public int[] Find(WordInfo[] words, int[] findIn)
            {
                var ret = findIn;
                for (int i = 0, loopTo = DMD.Arrays.UBound(words); i <= loopTo; i++)
                {
                    ret = Find(words[i], ret);
                    if (ret is null || DMD.Arrays.UBound(ret) == -1)
                        return ret;
                }

                return ret;
            }


            //private string Join(int[] arr)
            //{
            //    var ret = new System.Text.StringBuilder();
            //    for (int i = 0, loopTo = arr.Length - 1; i <= loopTo; i++)
            //    {
            //        if (i > 0)
            //            ret.Append(",");
            //        ret.Append(Databases.DBUtils.DBNumber(arr[i]));
            //    }

            //    return ret.ToString();
            //}

            /// <summary>
            /// Cerca gli elementi corrispondenti alla parola limitando i risultati all'array specificato
            /// </summary>
            /// <param name="word"></param>
            /// <param name="arr"></param>
            /// <returns></returns>
            public int[] Find(WordInfo word, int[] arr)
            {
                int[] ret = null;
                if (word.IsLike)
                {
                    var lst = new List<int>();

                    using (var cursor = new CachedIndexCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.Word.Value = word.Word;
                        cursor.Word.Operator = OP.OP_LIKE;
                        if (arr is object)
                            cursor.ObjectID.ValueIn(arr);
                        while (cursor.Read())
                        {
                            lst.Add(cursor.Item.ObjectID);
                        }
                    }
                    ret = lst.ToArray();
                }
                else
                {
                    var item = GetCachedWord(word.Word);
                    if (arr is null)
                    {
                        ret = item.GetIndice;
                    }
                    else
                    {
                        ret = DMD.Arrays.Join(item.GetIndice, arr);
                    }
                }

                return ret;
            }

            /// <summary>
            /// Cerca gli elementi corrispondenti al testo
            /// </summary>
            /// <param name="text"></param>
            /// <param name="nMax"></param>
            /// <returns></returns>
            public CCollection<CResult> Find(string text, int? nMax)
            {
                return Find(text, null, nMax);
            }

            /// <summary>
            /// Cerca gli elementi corrispondenti al testo restringendo i risultati all'array specificato
            /// </summary>
            /// <param name="text"></param>
            /// <param name="findIn"></param>
            /// <param name="nMax"></param>
            /// <returns></returns>
            public CCollection<CResult> Find(string text, int[] findIn, int? nMax)
            {
                var ret = new CCollection<CResult>();
                text = DMD.Strings.Trim(text);
                if (string.IsNullOrEmpty(text))
                    return ret;
                WordInfo[] words;

                // Estraggo le parole da cercare
                words = SplitWords(text);

                // Ottengo le informazioni sulle parole da cercare per migliorare la ricerca
                if (words is object && DMD.Arrays.UBound(words) > 0)
                {
                    GetWordStats(words);

                    // Ordino le parole da cercare in modo da migliorare la ricerca
                    Array.Sort(words, 0, DMD.Arrays.Len(words)); // - 1
                }

                // Lancio la ricerca
                if (DMD.Arrays.Len(words) > 0)
                    words[DMD.Arrays.UBound(words)].IsLike = false;

                var items = Find(words, findIn);

                // Se non ci sono risultati provo a rilassare la ricerca
                if (DMD.Arrays.Len(items) == 0 && DMD.Arrays.Len(words) > 0)
                {
                    if (DMD.Arrays.Len(words) > 1)
                    {
                        words[DMD.Arrays.UBound(words)].IsLike = true;
                    }
                    else
                    {
                        words[0].IsLike = DMD.Strings.Len(words[0].Word) > 3;
                    }

                    // Array.Sort(words, 0, DMD.Arrays.Len(words)) '- 1
                    items = Find(words, findIn);
                }

                if (items is object)
                {
                    int i = 0;
                    while (i <= DMD.Arrays.UBound(items) && (nMax.HasValue == false || i < nMax.Value))
                    {
                        ret.Add(new CResult(items[i]));
                        i += 1;
                    }
                }

                return ret;
            }

            private void AddToCache(CachedWord item)
            {
                lock (lockObject)
                {
                    if (m_MAXCACHESIZE != 0)
                    {
                        CachedWords.Add(item.Word, item);
                        if (m_MAXCACHESIZE > 0)
                        {
                            if (CachedWords.Count > MaxCacheSize)
                            {
                                CachedWords.Sort();
                                while (CachedWords.Count > MaxCacheSize * m_UnloadFactor)
                                    CachedWords.RemoveAt(CachedWords.Count - 1);
                            }
                        }
                    }
                }
            }

            //~CIndexingService()
            //{
            //    //DMDObject.DecreaseCounter(this);
            //}

            /// <summary>
            /// Azzera completamente l'indice
            /// </summary>
            public void Reset()
            {
                lock (lockObject)
                {
                    m_CachedWords = null;
                    this.Database.DeleteAll(this.Database.AllEntities[this.IndexTableName]);
                    this.Database.DeleteAll(this.Database.AllEntities[this.TableStatsName]);
                    if (!string.IsNullOrEmpty(this.WordIndexFolder))
                    {
                        var folder = new System.IO.DirectoryInfo(WordIndexFolder);
                        if (folder.Exists)
                        {
                            var files = folder.GetFiles("*.idx");
                            foreach (System.IO.FileInfo file in files)
                                file.Delete();
                        }
                    }
                }
            }
        }
    }

    public partial class Sistema
    {
        

        public static readonly object @lock = new object();
        private static CIndexingService m_IndexingService = null;

        public static CIndexingService IndexingService
        {
            get
            {
                lock (@lock)
                {
                    if (m_IndexingService is null)
                        m_IndexingService = new CIndexingService();
                    return m_IndexingService;
                }
            }
        }
    }
}