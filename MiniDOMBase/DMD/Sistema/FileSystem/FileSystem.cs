using System;
using System.Collections.Generic;
using minidom.internals;

namespace minidom
{
    namespace internals
    {
        public sealed class CFileSystemClass
        {
            private string m_LIMITROOT = DMD.Strings.vbNullString;

            internal CFileSystemClass()
            {
                //DMDObject.IncreaseCounter(this);
            }

            /// <summary>
            /// Restituisce lo spazio libero nel disco specificato (in bytes)
            /// </summary>
            /// <param name="driveLetter"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public long GetDiskFreeSpace(string driveLetter)
            {
                driveLetter = DMD.Strings.UCase(DMD.Strings.Left(DMD.Strings.Trim(driveLetter), 1) + @":\");
                foreach (System.IO.DriveInfo drive in System.IO.DriveInfo.GetDrives())
                {
                    if ((DMD.Strings.UCase(drive.Name) ?? "") == (driveLetter ?? ""))
                        return drive.TotalFreeSpace;
                }

                return -1;
            }

            /// <summary>
            /// Restituisce la dimensione totale del disco specificato (in bytes)
            /// </summary>
            /// <param name="driveLetter"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public long GetDiskTotalSpace(string driveLetter)
            {
                driveLetter = DMD.Strings.UCase(DMD.Strings.Left(DMD.Strings.Trim(driveLetter), 1) + @":\");
                foreach (System.IO.DriveInfo drive in System.IO.DriveInfo.GetDrives())
                {
                    if ((DMD.Strings.UCase(drive.Name) ?? "") == (driveLetter ?? ""))
                        return drive.TotalSize;
                }

                return -1;
            }

            /// <summary>
            /// Restituisce l'etichetta del disco specificato
            /// </summary>
            /// <param name="driveLetter"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public string GetDiskLabel(string driveLetter)
            {
                driveLetter = DMD.Strings.UCase(DMD.Strings.Left(DMD.Strings.Trim(driveLetter), 1) + @":\");
                foreach (System.IO.DriveInfo drive in System.IO.DriveInfo.GetDrives())
                {
                    if ((DMD.Strings.UCase(drive.Name) ?? "") == (driveLetter ?? ""))
                        return drive.VolumeLabel;
                }

                return "";
            }

            /// <summary>
        /// Restituisce o imposta il percorso minimo di lavoro. Se diverso da NULL la libreria potrà operare solo su cartelle e files contenuti all'interno di questo percorso
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string LimitRoot
            {
                get
                {
                    return m_LIMITROOT;
                }

                set
                {
                    m_LIMITROOT = DMD.Strings.Trim(value);
                }
            }

            /// <summary>
            /// Restituisce un nome di file temporaneo
            /// </summary>
            /// <param name="extension"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public string GetTempFileName(string extension = "tmp")
            {
                string strName;
                string path = Sistema.ApplicationContext.TmporaryFolder;
                do
                    strName = System.IO.Path.Combine(path, DMD.Strings.GetRandomString(8) + "." + extension);
                while (FileExists(strName));
                return strName;
            }

            /// <summary>
            /// Restituisce un nome di file temporaneo
            /// </summary>
            /// <param name="prefix"></param>
            /// <param name="extension"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public string GetTempFileName(string prefix, string extension)
            {
                string path = Sistema.ApplicationContext.TmporaryFolder;
                string strName = System.IO.Path.Combine(path, prefix + "." + extension);
                int i = 0;
                while (FileExists(strName))
                {
                    i += 1;
                    strName = System.IO.Path.Combine(path, prefix + " (" + i + ")." + extension);
                }

                return strName;
            }

            public string CreateTemporaryFileName(string folderName, string prefix, string extension = "tmp")
            {
                int i;
                if (DMD.Strings.Right(folderName, 1) != @"\")
                    folderName = folderName + @"\";
                do
                    i = (int) Maths.Floor(DMD.RunTime.GetRandomDouble(0, 1) * 100000f);
                while (Sistema.FileSystem.FileExists(folderName + prefix + i + "." + extension));
                return folderName + prefix + i + "." + extension;
            }

            /// <summary>
        /// Estrae il solo nome con l'eventuale estensione
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string GetFileName(string fileName)
            {
                string ret = fileName;
                int p = DMD.Strings.InStrRev(ret, @"\");
                if (p > 0)
                    ret = DMD.Strings.Mid(ret, p + 1);
                return ret;
            }

            /// <summary>
        /// Estrae il solo nome
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string GetBaseName(string fileName)
            {
                int p;
                string ret;
                ret = GetFileName(fileName);
                p = DMD.Strings.InStrRev(ret, ".");
                if (p > 0)
                    ret = DMD.Strings.Left(ret, p - 1);
                return ret;
            }

            /// <summary>
        /// Estrae l'estensione dal nome del file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string GetExtensionName(string fileName)
            {
                int p;
                string ret;
                ret = DMD.Strings.Trim(fileName);
                // Rimuoviamo il percorso
                p = DMD.Strings.InStrRev(ret, @"\");
                if (p > 0)
                    ret = DMD.Strings.Mid(ret, p + 1);
                // Estraiamo l'estensione
                p = DMD.Strings.InStrRev(ret, ".");
                if (p > 0)
                {
                    ret = DMD.Strings.Mid(ret, p + 1);
                }
                else
                {
                    ret = "";
                }

                return ret;
            }

            /// <summary>
        /// Crea o sostituisce il file indicato dal percorso inserendovi il contenuto specificato nella stringa content
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public long CreateTextFile(string path, string content)
            {
                System.IO.File.WriteAllText(path, content);
                return DMD.Strings.Len(content);
            }

            /// <summary>
        /// Crea il percorso (se il percorso esiste genera errore)
        /// </summary>
        /// <param name="path"></param>
        /// <remarks></remarks>
            public void CreateFolder(string path)
            {
                System.IO.Directory.CreateDirectory(path);
            }

            /// <summary>
        /// Restituisce vero se il folder esiste
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool FolderExists(string path)
            {
                return System.IO.Directory.Exists(path);
            }

            /// <summary>
        /// Restituisce vero se il file esiste
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool FileExists(string path)
            {
                return System.IO.File.Exists(path);
            }

            /// <summary>
            /// Crea il percorso (se il percorso esiste genera errore)
            /// </summary>
            /// <param name="path"></param>
            /// <remarks></remarks>
            public void CreateRecursiveFolder(string path)
            {
                DMD.IOUtils.CreateFolderRecursivelly(path);

                //if (System.IO.Directory.Exists(path))
                //    return;
                //string[] items;
                //var i = default(int);
                //string current;
                //path = DMD.Strings.Trim(path);
                //if (DMD.Strings.Right(path, 1) == @"\")
                //    path = DMD.Strings.Left(path, DMD.Strings.Len(path) - 1);
                //items = DMD.Strings.Split(path, @"\");
                //current = items[i];
                //if ((DMD.Strings.Left(LimitRoot, DMD.Strings.Len(current)) ?? "") != (current ?? "") & !FolderExists(current))
                //{
                //    try
                //    {
                //        CreateFolder(current);
                //    }
                //    catch (Exception ex)
                //    {
                //    }
                //}

                //var loopTo = DMD.Arrays.UBound(items);
                //for (i = 1; i <= loopTo; i++)
                //{
                //    current = current + @"\" + items[i];
                //    if ((DMD.Strings.Left(LimitRoot, DMD.Strings.Len(current)) ?? "") != (current ?? "") & !FolderExists(current))
                //    {
                //        try
                //        {
                //            CreateFolder(current);
                //        }
                //        catch (Exception ex)
                //        {
                //        }
                //    }
                //}

                //if (!FolderExists(path))
                //{
                //    throw new ArgumentException("Impossibile creare il percorso: " + path);
                //}
            }

            /// <summary>
        /// Estrae il percorso del folder che contiene il file o la cartella
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string GetFolderName(string path)
            {
                int i;
                string ret;
                i = DMD.Strings.InStrRev(path, @"\");
                if (i > 0)
                {
                    ret = DMD.Strings.Left(path, i - 1);
                }
                else
                {
                    ret = path;
                }

                return ret;
            }

            /// <summary>
        /// Ottiene un buffer stringa contenente l'intero file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string GetTextFileContents(string filePath)
            {
                return System.IO.File.ReadAllText(filePath);
            }

            public void SetTextFileContents(string fName, string text)
            {
                System.IO.File.WriteAllText(fName, text);
            }

            /// <summary>
        /// Crea una copia del file
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="overwrite"></param>
        /// <remarks></remarks>
            public void CopyFile(string source, string destination, bool overwrite = false)
            {
                System.IO.File.Copy(source, destination, overwrite);
            }


            /// <summary>
        /// Rimuove i caratteri non utilizzabili in un nome di files o di cartella
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string RemoveSpecialChars(string value)
            {
                const string invalidChars = @"\/:;?<>";
                int i;
                string ret;
                ret = value;
                var loopTo = DMD.Strings.Len(invalidChars);
                for (i = 1; i <= loopTo; i++)
                    ret = DMD.Strings.Replace(ret, DMD.Strings.Mid(invalidChars, i, 1), "_");
                return ret;
            }

            /// <summary>
            /// Restituisce un oggetto CCollection contenente tutti i nome di files e folder contenuti nel percorso specificato
            /// </summary>
            /// <param name="sFolder"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public List<string> GetAllFiles(string sFolder)
            {
                var c = new System.IO.DirectoryInfo(sFolder);
                var ret = new List<string>();
                foreach (System.IO.FileInfo f in c.GetFiles())
                    ret.Add(f.FullName);
                return ret;
            }

            /// <summary>
            /// Restituisce un oggetto CCollection contenente tutti i nome di files e folder contenuti nel percorso specificato
            /// </summary>
            /// <param name="sFolder"></param>
            /// <param name="searchPattern"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public List<string> GetAllFiles(string sFolder, string searchPattern)
            {
                var c = new System.IO.DirectoryInfo(sFolder);
                var ret = new List<string>();
                foreach (System.IO.FileInfo f in c.GetFiles(searchPattern))
                    ret.Add(f.FullName);
                return ret;
            }

            /// <summary>
        /// Restituisce la data di creazione del file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime GetCreationTime(string fileName)
            {
                return System.IO.File.GetCreationTime(fileName);
            }

            /// <summary>
        /// Restituisce la data dell'ultima modifica del file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime GetLastModifiedTime(string fileName)
            {
                return System.IO.File.GetLastWriteTime(fileName);
            }

            /// <summary>
        /// Restituisce la data dell'ultima modifica del file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime GetLastAccessTime(string fileName)
            {
                return System.IO.File.GetLastAccessTime(fileName);
            }

            /// <summary>
        /// Restituisce la dimensione in bytes del file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public long GetFileSize(string fileName)
            {
                var file = new System.IO.FileInfo(fileName);
                return file.Length;
            }

            /// <summary>
        /// Elimina uno o più files (wildchars)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="force"></param>
        /// <remarks></remarks>
            public void DeleteFile(string path, bool force = false)
            {
                if (DMD.Strings.InStr(path, "*") > 0 | DMD.Strings.InStr(path, "?") > 0)
                {
                    string[] files;
                    int i;
                    string searchPattern;
                    i = DMD.Strings.InStrRev(path, @"\");
                    if (i > 0)
                    {
                        searchPattern = DMD.Strings.Mid(path, i + 1);
                        path = DMD.Strings.Left(path, i - 1);
                    }
                    else
                    {
                        searchPattern = path;
                        path = LimitRoot;
                    }

                    files = System.IO.Directory.GetFiles(path, searchPattern);
                    foreach (string file in files)
                    {
                        if (force)
                        {
                            try
                            {
                                System.IO.File.Delete(file);
                            }
                            catch (Exception ex)
                            {
                                Sistema.ApplicationContext.Log("File non eliminato: " + file + " -> " + ex.Message);
                            }
                        }
                        else
                        {
                            System.IO.File.Delete(file);
                        }
                    }
                }
                else if (force)
                {
                    try
                    {
                        System.IO.File.Delete(path);
                    }
                    catch (Exception ex)
                    {
                        Sistema.ApplicationContext.Log("File non eliminato: " + path + " -> " + ex.Message);
                    }
                }
                else
                {
                    System.IO.File.Delete(path);
                }
            }

            /// <summary>
        /// Sposta il file
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <remarks></remarks>
            public void MoveFile(string source, string target)
            {
                System.IO.File.Move(source, target);
            }

            /// <summary>
        /// Combina il percorso ad un percorso base
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string CombinePath(string folderName, string fileName)
            {
                folderName = DMD.Strings.Trim(folderName);
                fileName = DMD.Strings.Trim(fileName);
                if (DMD.Strings.Right(folderName, 1) != @"\")
                    folderName += @"\";
                return folderName + fileName;
            }

            /// <summary>
        /// Restituisce il nome del percorso "normalizzato" cioè aggiungendo "\" alla fine (se non presente)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NormalizePath(string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return "";
                if (DMD.Strings.Right(value, 1) != @"\")
                    value = value + @"\";
                return value;
            }

            /// <summary>
        /// Effettua una copia dallo stream inStream allo stream outStream utilizzando le rispettive posizioni correnti.
        /// La copia viene effettuata passando per un buffer temporaneo di 2048 bytes
        /// </summary>
        /// <param name="inStream"></param>
        /// <param name="outStream"></param>
        /// <remarks></remarks>
            public void CopyStream(System.IO.Stream inStream, System.IO.Stream outStream)
            {
                const int BUFF_SIZE = 2048;
                byte[] buffer;
                buffer = new byte[2048];
                int n;
                n = inStream.Read(buffer, 0, BUFF_SIZE);
                while (n > 0)
                {
                    outStream.Write(buffer, 0, n);
                    n = inStream.Read(buffer, 0, BUFF_SIZE);
                }
            }



            /// <summary>
        /// Se il percorso è una sottodirectori del percorso radice restituisce solo la parte "relativa"
        /// altriemnti restituisce il percorso stesso
        /// </summary>
        /// <param name="path">[in] Percorso da analizzare</param>
        /// <param name="fromPath">[in] Percorso base rispetto a cui "relativizzare"</param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string GetRelativePath(string path, string fromPath)
            {
                path = GetAbsolutePath(path);
                fromPath = GetAbsolutePath(fromPath);
                if (DMD.Strings.Right(path, 1) == @"\")
                    path = DMD.Strings.Left(path, DMD.Strings.Len(path) - 1);
                if (DMD.Strings.Right(fromPath, 1) == @"\")
                    fromPath = DMD.Strings.Left(fromPath, DMD.Strings.Len(fromPath) - 1);
                var i1 = DMD.Strings.Split(path, @"\");
                var i2 = DMD.Strings.Split(fromPath, @"\");
                int l1 = DMD.Arrays.Len(i1);
                int l2 = DMD.Arrays.Len(i2);
                int cnt = 0;
                while (cnt < l1 && cnt < l2 && DMD.Strings.Compare(i1[cnt], i2[cnt], true) == 0)
                    cnt += 1;
                var ret = new System.Text.StringBuilder();
                while (l2 - 1 > cnt)
                {
                    ret.Append(@"..\");
                    l2 -= 1;
                }

                while (l1 > cnt + 1)
                {
                    ret.Append(i1[cnt] + @"\");
                    cnt += 1;
                }

                if (l1 > cnt)
                {
                    ret.Append(i1[cnt]);
                    cnt += 1;
                }

                return ret.ToString();
            }

            /// <summary>
        /// Restituisce il percorso assoluto calcolato sulla base del percorso di avvio dell'applicazione.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string GetAbsolutePath(string path)
            {
                return GetAbsolutePath(path, Sistema.ApplicationContext.StartupFloder);
            }


            /// <summary>
        /// Restituisce il percorso assoluto calcolato sulla base del percorso specificato.
        /// Se path è già un percorso assoluto la funziona restituisce path stesso
        /// </summary>
        /// <param name="path"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public string GetAbsolutePath(string path, string basePath)
            {
                path = DMD.Strings.Trim(path);
                basePath = DMD.Strings.Trim(basePath);
                if (IsRelativePath(path))
                {
                    if (DMD.Strings.Right(basePath, 1) == @"\")
                        basePath = DMD.Strings.Left(basePath, DMD.Strings.Len(basePath) - 1);
                    var p = DMD.Strings.Split(basePath, @"\");
                    int l = DMD.Arrays.Len(p);
                    while (DMD.Strings.Left(path, 3) == @"..\" && l > 0)
                    {
                        l = l - 1;
                        path = DMD.Strings.Mid(path, 4);
                    }

                    if (DMD.Strings.Left(path, 1) == @"\")
                        path = DMD.Strings.Mid(path, 2);
                    var ret = new System.Text.StringBuilder();
                    for (int i = 0, loopTo = l - 1; i <= loopTo; i++)
                        ret.Append(p[i] + @"\");
                    ret.Append(path);
                    return ret.ToString();
                }
                else
                {
                    return path;
                }
            }

            /// <summary>
        /// Restituisce vero se il percorso è un percorso relativo
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool IsRelativePath(string path)
            {
                path = DMD.Strings.Trim(path);
                return DMD.Strings.Mid(path, 2, 1) != ":" && DMD.Strings.Left(path, 2) != @"\\"; // (Left(path, 1) = "\") OrElse (Left(path, 1) = "~") OrElse (Left(path, 3) = "..\")
            }

            /// <summary>
        /// Restituisce il percorso utilizzato dal sistema per i files temporanei
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public string GetSystemTempFolder()
            {
                return Sistema.ApplicationContext.TmporaryFolder;
            }

            ~CFileSystemClass()
            {
                //DMDObject.DecreaseCounter(this);
            }
        }
    }

    public partial class Sistema
    {
        public enum FileOpenEnum : int
        {
            ForReading = 1,
            ForWriting = 2,
            ForAppending = 3
        }

        private static CFileSystemClass m_FileSystem;

        public static CFileSystemClass FileSystem
        {
            get
            {
                if (m_FileSystem is null)
                    m_FileSystem = new CFileSystemClass();
                return m_FileSystem;
            }
        }
    }
}