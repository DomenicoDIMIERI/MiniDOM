using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Cursore su file e cartelle
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class FindFileCursor 
            : IDisposable, IDMDXMLSerializable, IEnumerable<string>
        {
            private string m_Pattern;
            private FileAttributes m_AttributesMask;
            private bool m_IncludeSubDirs;
            private List<string> m_Results;
            private int m_Index;

            /// <summary>
            /// Costruttore
            /// </summary>
            public FindFileCursor()
            {
                //DMDObject.IncreaseCounter(this);
                m_Pattern = "";
                m_AttributesMask = FileAttributes.Normal;
                m_Results = null;
                m_Index = 0;
                m_IncludeSubDirs = false;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="pattern"></param>
            public FindFileCursor(string pattern) : this(pattern, FileAttributes.Normal, false)
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="pattern"></param>
            /// <param name="attributeMask"></param>
            public FindFileCursor(string pattern, FileAttributes attributeMask) : this(pattern, attributeMask, false)
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="pattern"></param>
            /// <param name="attributeMask"></param>
            /// <param name="includeSubDirs"></param>
            public FindFileCursor(string pattern, FileAttributes attributeMask, bool includeSubDirs) : this()
            {
                m_Pattern = DMD.Strings.Trim(pattern);
                m_AttributesMask = attributeMask;
                m_IncludeSubDirs = includeSubDirs;
                m_Results = null;
                m_Index = -1;
            }

            /// <summary>
            /// Restituisce il filtro di ricerca
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Pattern
            {
                get
                {
                    return m_Pattern;
                }
            }

            /// <summary>
            /// Restituisce la maschera di ricerca
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public FileAttributes AttributesMask
            {
                get
                {
                    return m_AttributesMask;
                }
            }

            /// <summary>
            /// Restituisce un valore booleano che indica se la ricerca include le sottocartelle
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool IncludeSubDirs
            {
                get
                {
                    return m_IncludeSubDirs;
                }
            }

            /// <summary>
            /// Restituisce il numero di files e/o cartelle corrispondenti al filtro
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public int Count()
            {
                CheckInit();
                return m_Results.Count;
            }

            public bool EOF
            {
                get
                {
                    CheckInit();
                    return m_Index >= Count();
                }
            }

            public bool MoveFirst()
            {
                CheckInit();
                m_Index = 0;
                return m_Index < Count();
            }

            public bool MoveNext()
            {
                CheckInit();
                m_Index = Maths.Min(m_Index + 1, Count());
                return m_Index < Count();
            }

            public bool MovePrev()
            {
                CheckInit();
                m_Index = Maths.Max(m_Index - 1, 0);
                return m_Index < Count();
            }

            public bool MoveLast()
            {
                CheckInit();
                m_Index = Maths.Max(Count() - 1, 0);
                return m_Index < Count();
            }

            public string Item
            {
                get
                {
                    CheckInit();
                    if (m_Index < Count())
                    {
                        return m_Results[m_Index];
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            private void CheckInit()
            {
                if (m_Index == -1)
                    Init();
            }

            private void Init()
            {
                string path;
                string wildChars;
                if (DMD.Strings.InStr(m_Pattern, "*") > 0 || DMD.Strings.InStr(m_Pattern, "?") > 0)
                {
                    path = FileSystem.NormalizePath(FileSystem.GetFolderName(m_Pattern));
                    wildChars = DMD.Strings.Mid(m_Pattern, DMD.Strings.Len(path) + 1);
                }
                else
                {
                    path = FileSystem.NormalizePath(m_Pattern);
                    wildChars = "*.*";
                }

                this.m_Results = new List<string>();
                this.Elabora(this.m_Results, path, wildChars, m_IncludeSubDirs);
                this.m_Index = 0;
            }

            private void Elabora(List<string> items, string path, string wildChars, bool includeSubDirs)
            {
                var d = new System.IO.DirectoryInfo(path);
                foreach(var f in d.GetFiles(wildChars))
                {
                    if (
                        ((FileAttributes)f.Attributes & this.m_AttributesMask ) == this.m_AttributesMask
                       )
                    {
                        items.Add(f.FullName);
                    }                    
                }
                if (includeSubDirs)
                {
                    foreach(var c in d.GetDirectories())
                    {
                        this.Elabora(items, c.FullName, wildChars, includeSubDirs);
                    }
                }
            }

            public void Reset()
            {
                m_Index = -1;
                m_Results = null;
            }

            public void Dispose()
            {
                Reset();
                m_Pattern = DMD.Strings.vbNullString;
                m_Results = null;
            }

            public void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Pattern":
                        {
                            m_Pattern = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "AttributesMask":
                        {
                            m_AttributesMask = (FileAttributes)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IncludeSubDirs":
                        {
                            m_IncludeSubDirs = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Results":
                        {
                            if (fieldValue is null)
                            {
                                m_Results = null;
                            }
                            else if (DMD.RunTime.IsEnumerable<string>(fieldValue))
                            {
                                m_Results = new List<string>(DMD.Arrays.Convert<string>(fieldValue));
                            }
                            else
                            {
                                m_Results = new List<string>(new string[] { DMD.Strings.CStr(fieldValue) });
                            }

                            break;
                        }

                    case "Index":
                        {
                            m_Index = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
                }
            }

            public void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Pattern", m_Pattern);
                writer.WriteAttribute("AttributesMask", (int?)m_AttributesMask);
                writer.WriteAttribute("IncludeSubDirs", m_IncludeSubDirs);
                writer.WriteAttribute("Index", m_Index);
                writer.WriteTag("Results", GetResultsAsArray());
            }

            public string[] GetResultsAsArray()
            {
                CheckInit();
                string[] ret = null;
                if (Count() > 0)
                {
                    ret = new string[(Count())];
                    for (int i = 0, loopTo = Count() - 1; i <= loopTo; i++)
                        ret[i] = m_Results[i];
                }

                return ret;
            }

            public IEnumerator<string> GetEnumerator()
            {
                return this.m_Results.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            ~FindFileCursor()
            {
                //DMDObject.DecreaseCounter(this);
            }
        }
    }
}