using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Flag per i backup automatici
        /// </summary>
        [Flags]
        public enum BackupFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0,

            /// <summary>
            /// Abilita i backup parziali
            /// </summary>
            PartialBackupEnabled = 1,

            /// <summary>
            /// Abilita i backup integrali
            /// </summary>
            FullBackupEnabled = 2
        }


        /// <summary>
        /// Configurazione del debug
        /// </summary>
        [Serializable]
        public sealed class CBackupsConfiguration 
            : DMD.XML.DMDBaseXMLObject
        {
            private BackupFlags m_Flags;
            private string m_BackupFolder;
            private string[] m_ExludedDirs;
            private string[] m_IncludeDirs;
            private int m_FullBackupInterval;
            private int m_PartialBackupInterval;
            private DateTime? m_LastFullBackupDate;
            private DateTime? m_LastPartialBackupDate;
            private DateTime m_RunBackupAt; // Ora in cui eseguire il backup
            private CompressionMethods m_CompressionMethod;
            private CompressionLevels m_CompressionLevel;
            private int m_MaxSegmentSize;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CBackupsConfiguration()
            {
                m_Flags = BackupFlags.None;
                m_BackupFolder = @"\Backup";
                m_ExludedDirs = null;
                m_IncludeDirs = null;
                m_FullBackupInterval = 30;
                m_PartialBackupInterval = 7;
                m_LastFullBackupDate = default;
                m_LastPartialBackupDate = default;
                m_RunBackupAt = new DateTime(2000, 1, 1, 23, 0, 0);
                m_MaxSegmentSize = 0;
            }

            /// <summary>
            /// Restituisce o imposta la dimensione massima di un elemento del file di backup (se > 0 il backup sarà diviso in n spezzoni di dimensione massima MaxSegmentSize)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int MaxSegmentSize
            {
                get
                {
                    return m_MaxSegmentSize;
                }

                set
                {
                    int oldValue = m_MaxSegmentSize;
                    if (oldValue == value)
                        return;
                    m_MaxSegmentSize = value;
                    DoChanged("MaxSegmentSize", value, oldValue);
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
            /// Restituisce o imposta l'ora in cui eseguire il backup
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime RunBackupAt
            {
                get
                {
                    return m_RunBackupAt;
                }

                set
                {
                    var oldValue = m_RunBackupAt;
                    if (DMD.DateUtils.Compare(oldValue, value) == 0)
                        return;
                    m_RunBackupAt = value;
                    DoChanged("RunBackupAt", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta i flags
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public BackupFlags Flags
            {
                get
                {
                    return m_Flags;
                }

                set
                {
                    var oldValue = m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il sistema deve effettuare il backup completo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool FullBackupEnabled
            {
                get
                {
                    return DMD.RunTime.TestFlag(m_Flags, BackupFlags.FullBackupEnabled);
                }

                set
                {
                    if (FullBackupEnabled == value)
                        return;
                    m_Flags = DMD.RunTime.SetFlag(m_Flags, BackupFlags.FullBackupEnabled, value);
                    DoChanged("FullBackupEnabled", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il sistema deve effettuare i backup parziali
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool PartialBackupEnabled
            {
                get
                {
                    return DMD.RunTime.TestFlag(m_Flags, BackupFlags.PartialBackupEnabled);
                }

                set
                {
                    if (PartialBackupEnabled == value)
                        return;
                    m_Flags = DMD.RunTime.SetFlag(m_Flags, BackupFlags.PartialBackupEnabled, value);
                    DoChanged("PartialBackupEnabled", value, !value);
                }
            }

            /// <summary>
            /// Restituisce l'intervallo di tmpo tra due backup completi
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int FullBackupInterval
            {
                get
                {
                    return m_FullBackupInterval;
                }

                set
                {
                    int oldValue = m_FullBackupInterval;
                    if (oldValue == value)
                        return;
                    if (value <= 0)
                        throw new ArgumentOutOfRangeException("FullBackupInterval");
                    m_FullBackupInterval = value;
                    DoChanged("FullBackupInterval", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'intervallo di tmpo tra due backup parziali
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int PartialBackupInterval
            {
                get
                {
                    return m_PartialBackupInterval;
                }

                set
                {
                    int oldValue = m_PartialBackupInterval;
                    if (oldValue == value)
                        return;
                    if (value <= 0)
                        throw new ArgumentOutOfRangeException("PartialBackupInterval");
                    m_PartialBackupInterval = value;
                    DoChanged("PartialBackupInterval", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data dell'ultimo backup completo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? LastFullBackupDate
            {
                get
                {
                    return m_LastFullBackupDate;
                }

                set
                {
                    var oldValue = m_LastFullBackupDate;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_LastFullBackupDate = value;
                    DoChanged("LastFullBackupDate", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data dell'ultimo backup parziale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? LastPartialBackupDate
            {
                get
                {
                    return m_LastPartialBackupDate;
                }

                set
                {
                    var oldValue = m_LastPartialBackupDate;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_LastPartialBackupDate = value;
                    DoChanged("LastPartialBackupDate", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce il percorso predefinito in cui vengono memorizzati i files di backup
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public string GetDefaultBackupFolder()
            {
                return @"\Backups";
            }

            /// <summary>
            /// Restituisce o imposta il percorso in cui vengono memorizzati i files di backup
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string BackupFolder
            {
                get
                {
                    return m_BackupFolder;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_BackupFolder;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    if (string.IsNullOrEmpty(value))
                        value = GetDefaultBackupFolder();
                    m_BackupFolder = value;
                    DoChanged("BackupFolder", value, oldValue);
                }
            }



            /// <summary>
            /// Restituisce un array contenente tutte le cartelle escluse dai backup
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public string[] GetExcludedDirs()
            {
                // Dim ret As New System.Collections.ArrayList
                // 'Dim tmp As String = Me.m_ExludedDirs ' Backups.Module.Settings.GetValueString("ExcludedDirs", "")
                // Dim items() As String = Me.m_ExludedDirs ' DMD.Strings.Split(tmp, ",")
                // Dim bkFolder As String = Sistema.FileSystem.NormalizePath(Me.BackupFolder)
                // ret.Add(bkFolder)
                // ret.Add(Sistema.FileSystem.NormalizePath(Sistema.ApplicationContext.TmporaryFolder))
                // If (items IsNot Nothing) Then
                // For i As Integer = 0 To DMD.Arrays.Len(items) - 1
                // items(i) = Sistema.FileSystem.NormalizePath(DMD.Strings.Trim(items(i)))
                // If (items(i) <> "") AndAlso (LCase(items(i)) <> LCase(bkFolder)) Then
                // ret.Add(items(i))
                // End If
                // Next
                // End If
                // Return ret.ToArray(GetType(String))
                if (m_ExludedDirs is null)
                    return null;
                return (string[])m_ExludedDirs.Clone();
            }

            /// <summary>
            /// Aggiunge una cartella all'elenco delle cartelle escluse
            /// </summary>
            /// <param name="path"></param>
            /// <remarks></remarks>
            public void AddExcludedDir(string path)
            {
                path = DMD.Strings.Trim(path);
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException("Directory di esclusione non valida: NULL");
                var items = GetExcludedDirs();
                path = FileSystem.NormalizePath(path);
                foreach (string s in items)
                {
                    if ((DMD.Strings.LCase(s) ?? "") == (DMD.Strings.LCase(path) ?? ""))
                        return;
                }

                items = DMD.Arrays.Append(items, path);
                m_ExludedDirs = items; // , ",") '  Backups.Module.Settings.SetValueString("ExcludedDirs", Join(items, ","))
            }

            /// <summary>
            /// Rimuove una cartella dall'elenco delle cartelle escluse dal backup
            /// </summary>
            /// <param name="path"></param>
            /// <remarks></remarks>
            public void RemoveExcludedDir(string path)
            {
                path = DMD.Strings.Trim(path);
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException("Directory di esclusione non valida: NULL");
                var items = GetExcludedDirs();
                path = FileSystem.NormalizePath(path);
                for (int i = 0, loopTo = DMD.Arrays.Len(items) - 1; i <= loopTo; i++)
                {
                    if ((DMD.Strings.LCase(items[i]) ?? "") == (DMD.Strings.LCase(path) ?? ""))
                    {
                        items = DMD.Arrays.RemoveAt(items, i);
                        break;
                    }
                }

                m_ExludedDirs = items; // Backups.Module.Settings.SetValueString("ExcludedDirs", Join(items, ","))
            }

            /// <summary>
            /// Restituisce un array contenente tutte le cartelle aggiuntive
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public string[] GetIncludedDirs()
            {
                // Dim ret As New System.Collections.ArrayList
                // Dim tmp As String = Backups.Module.Settings.GetValueString("IncludedDirs", "")
                // Dim items() As String = Me.m_IncludeDirs ' DMD.Strings.Split(tmp, ",")
                // Dim bkFolder As String = Sistema.FileSystem.NormalizePath(BackupFolder)
                // If (items IsNot Nothing) Then
                // For i As Integer = 0 To DMD.Arrays.Len(items) - 1
                // items(i) = Sistema.FileSystem.NormalizePath(DMD.Strings.Trim(items(i)))
                // If (items(i) <> "") AndAlso (LCase(items(i)) <> LCase(bkFolder)) Then
                // ret.Add(items(i))
                // End If
                // Next
                // End If
                if (m_IncludeDirs is null)
                    return new string[] { };
                return (string[])m_IncludeDirs.Clone();
            }

            /// <summary>
            /// Aggiunge una cartella all'elenco delle cartelle aggiuntive
            /// </summary>
            /// <param name="path"></param>
            /// <remarks></remarks>
            public void AddIncludedDir(string path)
            {
                path = DMD.Strings.Trim(path);
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException("Directory di inclusione non valida: NULL");
                var items = GetIncludedDirs();
                path = FileSystem.NormalizePath(path);
                foreach (string s in items)
                {
                    if ((DMD.Strings.LCase(s) ?? "") == (DMD.Strings.LCase(path) ?? ""))
                        return;
                }

                items = DMD.Arrays.Append(items, path);
                m_IncludeDirs = items; // Backups.Module.Settings.SetValueString("IncludedDirs", Join(items, ","))
            }

            /// <summary>
            /// Rimuove una cartella dall'elenco delle cartelle aggiuntive
            /// </summary>
            /// <param name="path"></param>
            /// <remarks></remarks>
            public void RemoveIncludedDir(string path)
            {
                path = DMD.Strings.Trim(path);
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException("Directory di inclusione non valida: NULL");
                var items = GetIncludedDirs();
                path = FileSystem.NormalizePath(path);
                for (int i = 0, loopTo = DMD.Arrays.Len(items) - 1; i <= loopTo; i++)
                {
                    if ((DMD.Strings.LCase(items[i]) ?? "") == (DMD.Strings.LCase(path) ?? ""))
                    {
                        items = DMD.Arrays.RemoveAt(items, i);
                        break;
                    }
                }

                m_IncludeDirs = items; // Backups.Module.Settings.SetValueString("IncludedDirs", Join(items, ","))
            }

            
            private string[] MakeArray(string value)
            {
                return DMD.Strings.Split(value, ",");
            }

            private string ToArray(string[] value)
            {
                if (value is null)
                    return "";
                return DMD.Strings.Join(value, ",");
            }

           
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Flags", (int?)m_Flags);
                writer.WriteAttribute("BackupFolder", m_BackupFolder);
                writer.WriteAttribute("FullBackupInterval", m_FullBackupInterval);
                writer.WriteAttribute("PartialBackupInterval", m_PartialBackupInterval);
                writer.WriteAttribute("LastFullBackupDate", m_LastFullBackupDate);
                writer.WriteAttribute("LastPartialBackupDate", m_LastPartialBackupDate);
                writer.WriteAttribute("RunBackupAt", m_RunBackupAt);
                writer.WriteAttribute("CompressionMethod", (int?)m_CompressionMethod);
                writer.WriteAttribute("CompressionLevel", (int?)m_CompressionLevel);
                writer.WriteAttribute("MaxSegmentSize", m_MaxSegmentSize);
                base.XMLSerialize(writer);
                writer.WriteTag("ExcludedDirs", ToArray(m_ExludedDirs));
                writer.WriteTag("IncludedDirs", ToArray(m_IncludeDirs));
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Flags":
                        {
                            m_Flags = (BackupFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "BackupFolder":
                        {
                            m_BackupFolder = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "FullBackupInterval":
                        {
                            m_FullBackupInterval = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "PartialBackupInterval":
                        {
                            m_PartialBackupInterval = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "LastFullBackupDate":
                        {
                            m_LastFullBackupDate = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "LastPartialBackupDate":
                        {
                            m_LastPartialBackupDate = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "RunBackupAt":
                        {
                            m_RunBackupAt = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "ExcludedDirs":
                        {
                            m_ExludedDirs = MakeArray(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "IncludedDirs":
                        {
                            m_IncludeDirs = MakeArray(DMD.XML.Utils.Serializer.DeserializeString(fieldValue));
                            break;
                        }

                    case "CompressionMethod":
                        {
                            m_CompressionMethod = (CompressionMethods)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "CompressionLevel":
                        {
                            m_CompressionLevel = (CompressionLevels)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "MaxSegmentSize":
                        {
                            m_MaxSegmentSize = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

             
        }
    }

    namespace repositories
    {
        public partial class CBackupsClass
        {

            /// <summary>
            /// Evento generato quando viene modificata la configurazione del sistema dei backup
            /// </summary>
            /// <remarks></remarks>
            public event ConfigurationChangedEventHandler ConfigurationChanged;

            /// <summary>
            /// Evento generato quando viene modificata la configurazione del sistema dei backup
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public delegate void ConfigurationChangedEventHandler(object sender, EventArgs e);

            private CBackupsConfiguration m_Configuration = null;

            /// <summary>
            /// Configurazione del backup
            /// </summary>
            public CBackupsConfiguration Configuration
            {
                get
                {
                    lock (this)
                    {
                        if (m_Configuration is null)
                        {
                            var str = minidom.Sistema.ApplicationContext.Settings.GetValueString("Sistema.Backups.Configuration", "");
                            m_Configuration = DMD.XML.Utils.Deserialize<CBackupsConfiguration>(str);
                        }

                        return m_Configuration;
                    }
                }
            }

            internal void SetConfiguration(CBackupsConfiguration c)
            {
                m_Configuration = c;
                var str = DMD.XML.Utils.Serialize(c);
                minidom.Sistema.ApplicationContext.Settings.SetValueString("Sistema.Backups.Configuration", str);
                var e = new EventArgs();
                ConfigurationChanged?.Invoke(this, e);
            }
        
        }
    
    
    }

}
