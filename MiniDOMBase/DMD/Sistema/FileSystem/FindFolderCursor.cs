using System;
using System.Collections.Generic;
using DMD;
using DMD.XML;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Cursore sulle sottocartelle
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public sealed class FindFolderCursor 
            : IDisposable, IDMDXMLSerializable
        {
            private string m_Pattern;
            private FileAttributes m_AttributesMask;
            private bool m_IncludeSubDirs;
            private List<string> m_Results;
            private int m_Index;

            /// <summary>
            /// Costruttore
            /// </summary>
            public FindFolderCursor()
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
            public FindFolderCursor(string pattern) : this(pattern, FileAttributes.Normal, false)
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="pattern"></param>
            /// <param name="attributeMask"></param>
            public FindFolderCursor(string pattern, FileAttributes attributeMask) : this(pattern, attributeMask, false)
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="pattern"></param>
            /// <param name="attributeMask"></param>
            /// <param name="includeSubDirs"></param>
            public FindFolderCursor(string pattern, FileAttributes attributeMask, bool includeSubDirs) : this()
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
                if (DMD.Strings.Contains(m_Pattern, "*")  || DMD.Strings.Contains(m_Pattern, "?")  )
                {
                    path = FileSystem.NormalizePath(FileSystem.GetFolderName(m_Pattern));
                    wildChars = DMD.Strings.Mid(m_Pattern, DMD.Strings.Len(path) + 1);
                }
                else
                {
                    path = FileSystem.NormalizePath(m_Pattern);
                    wildChars = "*.*";
                }

                var d = new System.IO.DirectoryInfo(path);
                this.m_Results = new List<string>();
                this.Elabora(this.m_Results, path, wildChars, this.m_IncludeSubDirs);

                m_Index = 0;
            }

            private void Elabora(List<string> items, string path, string wildChars, bool incudeSubDirs)
            {
                var d = new System.IO.DirectoryInfo(path);
                var dirs = d.GetDirectories(wildChars);
                foreach (var c in dirs) {
                    if (
                       ((FileAttributes)c.Attributes & this.m_AttributesMask) == this.m_AttributesMask
                      )
                    {
                        items.Add(c.FullName);
                    }
                }

                if (IncludeSubDirs)
                {
                    foreach (var c in d.GetDirectories())
                    {
                        this.Elabora(items, c.FullName, wildChars, incudeSubDirs);
                    }
                }
            }

            public void Reset()
            {
                m_Index = -1;
                m_Results = null;
            }


            // This code added by Visual Basic to correctly implement the disposable pattern.
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
                writer.WriteTag("Pattern", m_Pattern);
                writer.WriteTag("AttributesMask", (int?)m_AttributesMask);
                writer.WriteTag("IncludeSubDirs", m_IncludeSubDirs);
                writer.WriteTag("Results", GetResultsAsArray());
                writer.WriteTag("Index", m_Index);
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

            ~FindFolderCursor()
            {
                //DMDObject.DecreaseCounter(this);
            }
        }
    }
}

