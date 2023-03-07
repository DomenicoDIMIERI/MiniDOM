using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Modalità di compressione del backup
        /// </summary>
        public enum CompressionLevels : int
        {
            /// <summary>
            /// Nessuna compressione
            /// </summary>
            None = 0,

            /// <summary>
            /// Compressione minima (veloce)
            /// </summary>
            BestSpeed = 1,
            // Level2 = 2
            // Level3 = 3
            // Level4 = 4
            // Level5 = 5

            /// <summary>
            /// Compressione normale (bilanciata)
            /// </summary>
            Default = 6,
            // Level7 = 7
            // Level8 = 8

            /// <summary>
            /// Compressione massima (lenta)
            /// </summary>
            BestCompression = 9
        }

        /// <summary>
        /// Tipo di compressione del backup
        /// </summary>
        public enum CompressionMethods : int
        {
            /// <summary>
            /// Predefinito
            /// </summary>
            Default = 0,

            /// <summary>
            /// Compressione normale
            /// </summary>
            Filtered = 1,

            /// <summary>
            /// Huffman
            /// </summary>
            HuffmanOnly = 2
        }


        /// <summary>
        /// Backup di uno o più file
        /// </summary>
        [Serializable]
        public class CBackup 
            : Databases.DBObject
        {
            private string m_Name;
            private string m_FileName;
            private DateTime m_FileDate;
            private long m_FileSize;
            private string m_LogMessages;
            private float m_ExecTime;
            private CompressionLevels m_CompressionLevel;
            private CompressionMethods m_CompressionMethod;
            private CBackupItems m_Items;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CBackup()
            {
                m_Name = "";
                m_FileName = "";
                m_FileDate = default;
                m_FileSize = 0L;
                m_LogMessages = "";
                m_ExecTime = 0f;
                m_CompressionLevel = CompressionLevels.Default;
                m_CompressionMethod = CompressionMethods.Default;
                m_Items = null;                 
            }

            
            /// <summary>
            /// Catalogo
            /// </summary>
            public CBackupItems Items
            {
                get
                {
                    lock (this)
                    {
                        if (m_Items is null)
                            m_Items = new CBackupItems(this);
                        return m_Items;
                    }
                }
            }

            /// <summary>
            /// Restituisce o imposta il livello di compressione utilizzato per comprimere l'archivio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CompressionLevels CompressionLevel
            {
                get
                {
                    return m_CompressionLevel;
                }

                set
                {
                    var oldValue = m_CompressionLevel;
                    if (oldValue == value)
                        return;
                    m_CompressionLevel = value;
                    DoChanged("CompressionLevel", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'algoritmo utilizzato per comprimere l'archivio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CompressionMethods CompressionMethod
            {
                get
                {
                    return m_CompressionMethod;
                }

                set
                {
                    var oldValue = m_CompressionMethod;
                    if (oldValue == value)
                        return;
                    m_CompressionMethod = value;
                    DoChanged("CompressionMethod", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del backup
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Name
            {
                get
                {
                    return m_Name;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Name;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Name = value;
                    DoChanged("Name", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del file in cui è memorizzato il backup
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string FileName
            {
                get
                {
                    return m_FileName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_FileName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_FileName = FileSystem.GetAbsolutePath(value, ApplicationContext.StartupFloder);
                    DoChanged("FileName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data e l'ora di creazione del file di backup
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime FileDate
            {
                get
                {
                    return m_FileDate;
                }

                set
                {
                    var oldValue = m_FileDate;
                    if (oldValue == value)
                        return;
                    m_FileDate = value;
                    DoChanged("FileDate", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la dimensione in bytes del file di backup
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public long FileSize
            {
                get
                {
                    return m_FileSize;
                }

                set
                {
                    long oldValue = m_FileSize;
                    if (oldValue == value)
                        return;
                    m_FileSize = value;
                    DoChanged("FileSize", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'output
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string LogMessages
            {
                get
                {
                    return m_LogMessages;
                }
            }

            /// <summary>
            /// Restituisce il tempo di esecuzione del backup
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public float ExecTime
            {
                get
                {
                    return m_ExecTime;
                }
            }

            /// <summary>
            /// Carica da file
            /// </summary>
            /// <param name="fileName"></param>
            /// <returns></returns>
            public bool Load(string fileName)
            {
                m_Name = FileSystem.GetBaseName(fileName);
                m_FileName = fileName;
                m_FileDate = FileSystem.GetCreationTime(fileName);
                m_FileSize = FileSystem.GetFileSize(fileName);
                m_LogMessages = "";
                return true;
            }

            /// <summary>
            /// Restituisce il nome predefinito
            /// </summary>
            /// <returns></returns>
            internal string GetDefaultName()
            {
                string ret = "BK";
                ret += DMD.Strings.PadLeft(DMD.DateUtils.Year(m_FileDate).ToString(), 4, "0");
                ret += DMD.Strings.PadLeft(DMD.DateUtils.Month(m_FileDate).ToString(), 2, "0");
                ret += DMD.Strings.PadLeft(DMD.DateUtils.Day(m_FileDate).ToString(), 2, "0");
                ret += DMD.Strings.PadLeft(DMD.DateUtils.Hour(m_FileDate).ToString(), 2, "0");
                ret += DMD.Strings.PadLeft(DMD.DateUtils.Minute(m_FileDate).ToString(), 2, "0");
                ret += DMD.Strings.PadLeft(DMD.DateUtils.Second(m_FileDate).ToString(), 2, "0");
                ret += ".BKP";
                return ret;
            }

            /// <summary>
            /// Effettua il backup
            /// </summary>
            /// <remarks></remarks>
            internal void Create(DateTime? fd)
            {
                var include = Backups.Configuration.GetIncludedDirs();
                var exclude = Backups.Configuration.GetExcludedDirs();
                // Dim out As New System.IO.StringWriter
                DateTime t1, t2;
                t1 = DMD.DateUtils.Now();
                // note: this does not recurse directories! 

                // Try
                // zip.AddFile(APPConn.Path)
                // Catch ex As Exception
                // zip.StatusMessageTextWriter.WriteLine("Errore " & ex.Message & " sul file " & APPConn.Path)
                // End Try
                // Try
                // zip.AddFile(LOGConn.Path)
                // Catch ex As Exception
                // zip.StatusMessageTextWriter.WriteLine("Errore " & ex.Message & " sul file " & LOGConn.Path)
                // End Try

                for (int i = 0, loopTo = DMD.Arrays.Len(include) - 1; i <= loopTo; i++)
                {
                    include[i] = DMD.Strings.Trim(include[i]);
                    if (!string.IsNullOrEmpty(include[i]))
                    {
                        include[i] = DMD.Strings.LCase(FileSystem.GetAbsolutePath(include[i], ApplicationContext.StartupFloder));
                        ApplicationContext.Log("Includo la cartella " + include[i]);
                    }
                }

                for (int i = 0, loopTo1 = DMD.Arrays.Len(exclude) - 1; i <= loopTo1; i++)
                {
                    exclude[i] = DMD.Strings.Trim(exclude[i]);
                    if (!string.IsNullOrEmpty(exclude[i]))
                    {
                        exclude[i] = DMD.Strings.LCase(FileSystem.GetAbsolutePath(exclude[i], ApplicationContext.StartupFloder));
                        ApplicationContext.Log("Escludo la cartella " + exclude[i]);
                    }
                }

                // Dim zip As New ZipFile
                // zip.StatusMessageTextWriter = out
                // zip.ZipErrorAction = ZipErrorAction.Skip

                // zip.AddDirectory(Sistema.ApplicationContext.WorkingFolder)
                string bp = ApplicationContext.MapPath("/");
                string dir = FileSystem.CombinePath(FileSystem.GetFolderName(FileName), FileSystem.GetBaseName(FileName));
                FileSystem.CreateRecursiveFolder(dir);
                for (int i = 0, loopTo2 = DMD.Arrays.Len(include) - 1; i <= loopTo2; i++)
                    ProcessFolder(exclude, include[i], bp, fd);

                // 'zip.AlternateEncoding = System.Text.Encoding.Unicode
                // If (Sistema.Backups.Configuration.MaxSegmentSize > 0) Then zip.MaxOutputSegmentSize = Sistema.Backups.Configuration.MaxSegmentSize
                // zip.CompressionLevel = Me.CompressionLevel
                // zip.CompressionMethod = Me.CompressionMethod
                // zip.UseZip64WhenSaving = Zip64Option.AsNecessary
                // ' zip.TempFileFolder = minidom.Sistema.FileSystem.GetSystemTempFolder
                // Sistema.ApplicationContext.Log("Inizio la compressione " & Me.FileName)
                // zip.Save(Me.FileName)

                t2 = DMD.DateUtils.Now();
                m_ExecTime = (float) (t2 - t1).TotalMilliseconds / 1000;
                Items.Save();
                m_FileDate = FileSystem.GetCreationTime(FileName);
                m_FileSize = 0L; // Sistema.FileSystem.GetFileSize(Me.FileName)
                foreach (CBackupItem item in Items)
                    m_FileSize += item.DimensioneCompressa;

                // Me.m_LogMessages = out.ToString
                // out.Dispose()


                // Sistema.ApplicationContext.Log("Compressione terminata " & Me.m_LogMessages)
            }

            private void ProcessFolder(string[] exclude, string folder, string percorsoBase, DateTime? fd)
            {
                folder = FileSystem.NormalizePath(folder);
                // Sistema.ApplicationContext.Log("Elaboro la cartella " & folder)

                if (!IsFileIncluded(exclude, folder))
                {
                    ApplicationContext.Log("Percorso escluso: " + folder);
                    return;
                }


                // Aggiungiamo tutti i files
                FindFileCursor fCursor = null;
                int numitems;
                try
                {
                    fCursor = new FindFileCursor(folder + "*.*", FileAttributes.Normal, false);
                    numitems = fCursor.Count();
                    while (!fCursor.EOF)
                    {
                        string fName = fCursor.Item;
                        if ((fName ?? "") == (FileName ?? ""))
                        {
                            ApplicationContext.Log("Escludo il file di destinazione del backup: " + fName);
                        }

                        if (fd.HasValue == false || System.IO.File.GetLastWriteTime(fName) >= fd.Value)
                        {
                            // zip.AddFile(fCursor.Item, Me.GetInternalPath(Sistema.FileSystem.GetFolderName(fCursor.Item)))
                            var item = new CBackupItem();
                            Items.Add(item);
                            item.Comprimi(fCursor.Item, percorsoBase);
                            ApplicationContext.Log("zip.AddFile(" + fCursor.Item + ", " + GetInternalPath(FileSystem.GetFolderName(fCursor.Item)));
                        }

                        fCursor.MoveNext();
                    }
                }
                catch (AccessViolationException)
                {
                    ApplicationContext.Log("Errore di accesso al percorso: " + folder);
                    return;
                }
                catch (Exception ex)
                {
                    // zip.StatusMessageTextWriter.WriteLine(ex.Message)
                    ApplicationContext.Log(Formats.FormatUserDateTime(DMD.DateUtils.Now()) + " " + ex.Message + DMD.Strings.vbNewLine);
                    m_LogMessages += Formats.FormatUserDateTime(DMD.DateUtils.Now()) + " " + ex.Message + DMD.Strings.vbNewLine;
                    return;
                }
                finally
                {
                    if (fCursor is object)
                        fCursor.Dispose();
                }


                 
                // Aggiungiamo le sottocartelle 
                FindFolderCursor dCursor = null;
                try
                {
                    dCursor = new FindFolderCursor(folder + "*.*", FileAttributes.Normal, false);
                    numitems = dCursor.Count();
                    while (!dCursor.EOF)
                    {
                        ProcessFolder(exclude, dCursor.Item, percorsoBase, fd);
                        dCursor.MoveNext();
                    }
                }
                catch (AccessViolationException)
                {
                    ApplicationContext.Log("Errore di accesso al percorso: " + folder);
                    return;
                }
                catch (Exception ex)
                {
                    ApplicationContext.Log(ex.Message);
                    return;
                }
                finally
                {
                    if (dCursor is object)
                        dCursor.Dispose();
                }

                 
            }

            /// <summary>
            /// Ripristina tutti i files
            /// </summary>
            /// <remarks></remarks>
            internal void Restore()
            {
                // Dim zip As New ZipFile(Me.FileName)
                // Dim bp As String = ApplicationContext.MapPath("/")
                // 'AddHandler zip.ExtractExceptio, AddressOf zipExtractError
                // zip.ExtractAll(bp, ExtractExistingFileAction.OverwriteSilently)
                string bp = ApplicationContext.MapPath("/");
                foreach (CBackupItem item in Items)
                    item.Decomprimi(bp);
            }



            // Private Sub zipExtractError(ByVal sender As Object, ByVal e As Ionic.Zip.ExtractExceptionEventArgs)
            // Debug.Print("Problema nell'estrarre: " & e.TargetPath & ": " & e.Exception.Message)
            // e.Terminate = False
            // End Sub

            /// <summary>
        /// Elimina i file di backup e cancella la registrazione nel DB
        /// </summary>
        /// <remarks></remarks>
            internal void Destroy()
            {
                string bp = ApplicationContext.MapPath("/");
                string dir = FileSystem.CombinePath(FileSystem.GetFolderName(FileName), FileSystem.GetBaseName(FileName));

                // Dim zip As New ZipFile(Me.FileName)
                // Dim bp As String = ApplicationContext.MapPath("/")
                // 'AddHandler zip.ExtractExceptio, AddressOf zipExtractError
                // zip.ExtractAll(bp, ExtractExistingFileAction.OverwriteSilently)
                // For Each item As CBackupItem In Me.Items
                // item.Delete()
                // Next

                if (System.IO.Directory.Exists(dir))
                    System.IO.Directory.Delete(dir, true);
                if (System.IO.File.Exists(FileName))
                    System.IO.File.Delete(FileName);
                Delete();
            }

            private bool IsFileIncluded(string[] excluded, string fileName)
            {
                if (excluded is null || excluded.Length == 0)
                    return true;
                string fPath = DMD.Strings.LCase(FileSystem.NormalizePath(FileSystem.GetFolderName(fileName)));
                for (int i = 0, loopTo = DMD.Arrays.UBound(excluded); i <= loopTo; i++)
                {
                    string f = excluded[i];
                    if (DMD.Strings.Len(f) > 0 && (DMD.Strings.LCase(f) ?? "") == (DMD.Strings.Left(fPath, DMD.Strings.Len(f)) ?? ""))
                        return false;
                }

                return true;
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}


            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Backups; //.Module;
            }

            /// <summary>
            /// Nome della tabella
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Backups";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Name = reader.Read("Name", this. m_Name);
                m_FileName = reader.Read("FileName", this.m_FileName);
                m_FileDate = reader.Read("FileDate", this.m_FileDate);
                m_FileSize = reader.Read("FileSize", this.m_FileSize);
                m_LogMessages = reader.Read("LogMessages", this.m_LogMessages);
                m_ExecTime = reader.Read("ExecTime", this.m_ExecTime);
                m_CompressionLevel = reader.Read("CompressionLevel", this.m_CompressionLevel);
                m_CompressionMethod = reader.Read("CompressionMethod", this.m_CompressionMethod);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Name", m_Name);
                writer.Write("FileName", m_FileName);
                writer.Write("FileDate", m_FileDate);
                writer.Write("FileSize", m_FileSize);
                writer.Write("LogMessages", m_LogMessages);
                writer.Write("ExecTime", m_ExecTime);
                writer.Write("CompressionLevel", m_CompressionLevel);
                writer.Write("CompressionMethod", m_CompressionMethod);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Name", typeof(string), 255);
                c = table.Fields.Ensure("FileName", typeof(string), 255);
                c = table.Fields.Ensure("FileDate", typeof(DateTime), 1);
                c = table.Fields.Ensure("FileSize", typeof(long), 1);
                c = table.Fields.Ensure("LogMessages", typeof(string), 0);
                c = table.Fields.Ensure("ExecTime", typeof(double), 1);
                c = table.Fields.Ensure("CompressionLevel", typeof(int), 1);
                c = table.Fields.Ensure("CompressionMethod", typeof(int), 1);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Name", "CompressionLevel", "CompressionMethod"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxFileName", new string[] { "FileName", "ExecTime" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxFileParams", new string[] { "FileDate", "FileSize" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxLog", new string[] { "LogMessages" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Name", m_Name);
                writer.WriteAttribute("FileName", m_FileName);
                writer.WriteAttribute("FileDate", m_FileDate);
                writer.WriteAttribute("FileSize", m_FileSize);
                writer.WriteAttribute("ExecTime", m_ExecTime);
                writer.WriteAttribute("CompressionLevel", (int?)m_CompressionLevel);
                writer.WriteAttribute("CompressionMethod", (int?)m_CompressionMethod);
                base.XMLSerialize(writer);
                writer.WriteTag("LogMessages", m_LogMessages);
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
                    case "Name":
                        {
                            m_Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "FileName":
                        {
                            m_FileName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "FileDate":
                        {
                            m_FileDate = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "FileSize":
                        {
                            m_FileSize = (long)DMD.XML.Utils.Serializer.DeserializeLong(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "LogMessages":
                        {
                            m_LogMessages = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ExecTime":
                        {
                            m_ExecTime = (float)DMD.XML.Utils.Serializer.DeserializeFloat(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "CompressionLevel":
                        {
                            m_CompressionLevel = (CompressionLevels)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "CompressionMethod":
                        {
                            m_CompressionMethod = (CompressionMethods)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
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
                return 
                    DMD.Strings.ConcatArray(m_Name , ", backup del " , m_FileDate , ", dimensione file: "  , m_FileSize);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Name);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CBackup) && this.Equals((CBackup)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CBackup obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Name, obj.m_Name)
                    && DMD.Strings.EQ(this.m_FileName, obj.m_FileName)
                    && DMD.DateUtils.EQ(this.m_FileDate, obj.m_FileDate)
                    && DMD.Longs.EQ(this.m_FileSize, obj.m_FileSize)
                    && DMD.Strings.EQ(this.m_LogMessages, obj.m_LogMessages)
                    && DMD.Doubles.EQ(this.m_ExecTime, obj.m_ExecTime)
                    && DMD.Integers.EQ((int)this.m_CompressionLevel, (int)obj.m_CompressionLevel)
                    && DMD.Integers.EQ((int)this.m_CompressionMethod, (int)obj.m_CompressionMethod)
            //private CBackupItems m_Items;
                ;

            }

            private string GetInternalPath(string path)
            {
                // Dim bp As String = ApplicationContext.MapPath("/")
                // If (DMD.Strings.Left(path, Len(bp)) = bp) Then Return Mid(path, Len(bp))
                // Return path
                return FileSystem.GetRelativePath(path, ApplicationContext.StartupFloder);
            }
        }
    }
}
