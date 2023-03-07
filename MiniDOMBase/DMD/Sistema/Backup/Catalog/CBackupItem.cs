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
using DMD.Zip;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Elemento nel catalogo del backup
        /// </summary>
        [Serializable]
        public class CBackupItem
            : IDMDXMLSerializable
        {

            /// <summary>
            /// Possessore
            /// </summary>
            [NonSerialized]  public CBackup m_Owner;

            /// <summary>
            /// Percorso completo del file originale
            /// </summary>
            public string PercorsoOrigine;

            /// <summary>
            /// Percorso completo del file destinazione
            /// </summary>
            public string FileDestinazione;

            /// <summary>
            /// Dimensione in bytes del file originale
            /// </summary>
            public long DimensioneOrigine;

            /// <summary>
            /// Dimensione in bytes del file compresso
            /// </summary>
            public long DimensioneCompressa;

            /// <summary>
            /// Data dell'ultima modifica del file originale
            /// </summary>
            public DateTime DataUltimaModifica;

            /// <summary>
            /// Data di compressione
            /// </summary>
            public DateTime DataCompressione;

            /// <summary>
            /// Messaggi generati dal compressore per l'elemento
            /// </summary>
            public string Messages;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CBackupItem()
            {
                //DMDObject.IncreaseCounter(this);
            }

            /// <summary>
            /// Riferimento al backup
            /// </summary>
            public CBackup Owner
            {
                get
                {
                    return m_Owner;
                }
            }

            /// <summary>
            /// Imposta il possessore
            /// </summary>
            /// <param name="owner"></param>
            protected internal virtual void SetOwner(CBackup owner)
            {
                m_Owner = owner;
            }

            /// <summary>
            /// Comprime l'elemento singolarmente
            /// </summary>
            /// <param name="percorsoOrigine"></param>
            /// <param name="percorsoBase"></param>
            public void Comprimi(string percorsoOrigine, string percorsoBase)
            {
                this.PercorsoOrigine = FileSystem.GetRelativePath(percorsoOrigine, percorsoBase);
                this.DimensioneOrigine = FileSystem.GetFileSize(percorsoOrigine);
                this.DataUltimaModifica = FileSystem.GetLastModifiedTime(percorsoOrigine);
                using (var @out = new System.IO.StringWriter())
                {
                    using (var zip = new ZipFile())
                    {
                        zip.StatusMessageTextWriter = @out;
                        zip.ZipErrorAction = ZipErrorAction.Skip;
                        // zip.AddDirectory(Sistema.ApplicationContext.WorkingFolder)
                        zip.AddFile(percorsoOrigine, "");
                        zip.CompressionLevel = (CompressionLevel)m_Owner.CompressionLevel;
                        zip.CompressionMethod = (CompressionMethod)m_Owner.CompressionMethod;
                        zip.UseZip64WhenSaving = Zip64Option.AsNecessary;
                        string dir = FileSystem.CombinePath(FileSystem.GetFolderName(m_Owner.FileName), FileSystem.GetBaseName(m_Owner.FileName));
                        lock (this.m_Owner)
                        {
                            this.FileDestinazione = DMD.Strings.GetRandomString(8) + ".ZIP";
                            while (FileSystem.FileExists(FileSystem.CombinePath(dir, FileDestinazione)))
                                FileDestinazione = DMD.Strings.GetRandomString(8) + ".ZIP";
                            using (var stream = new System.IO.FileStream(FileSystem.CombinePath(dir, FileDestinazione), System.IO.FileMode.Create))
                            {
                                zip.Save(stream);
                            }
                        }
                        DataCompressione = DMD.DateUtils.Now();
                        DimensioneCompressa = FileSystem.GetFileSize(FileSystem.CombinePath(dir, FileDestinazione));

                        this.Messages = @out.ToString();
                    }
                }
            }

            /// <summary>
            /// Decomprime l'elemento
            /// </summary>
            /// <param name="percorsoBase"></param>
            public void Decomprimi(string percorsoBase)
            {
                string dir = FileSystem.CombinePath(FileSystem.GetFolderName(m_Owner.FileName), FileSystem.GetBaseName(m_Owner.FileName));
                string f = FileSystem.GetFileName(FileDestinazione);
                using (var zip = new ZipFile(FileSystem.CombinePath(dir, f)))
                {
                    // AddHandler zip.ExtractExceptio, AddressOf zipExtractError
                    string p = FileSystem.GetAbsolutePath(PercorsoOrigine, percorsoBase);
                    p = FileSystem.GetFolderName(p);
                    FileSystem.CreateRecursiveFolder(p);
                    zip.ExtractAll(p, ExtractExistingFileAction.OverwriteSilently);
                }
            }

            // Public Sub Delete()
            // Dim dir As String = Sistema.FileSystem.CombinePath(Sistema.FileSystem.GetFolderName(Me.m_Owner.FileName), Sistema.FileSystem.GetBaseName(Me.m_Owner.FileName))
            // Dim f As String = Sistema.FileSystem.GetFileName(Me.FileDestinazione)
            // Dim p As String = Sistema.FileSystem.GetAbsolutePath(Me.PercorsoOrigine, percorsoBase)
            // p = Sistema.FileSystem.GetFolderName(p)
            // Sistema.FileSystem.CreateRecursiveFolder(p)
            // zip.ExtractAll(p, ExtractExistingFileAction.OverwriteSilently)
            // zip.Dispose()
            // End Sub

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            public void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "PercorsoOrigine":
                        {
                            PercorsoOrigine = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "FileDestinazione":
                        {
                            FileDestinazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DimensioneOrigine":
                        {
                            DimensioneOrigine = (long)DMD.XML.Utils.Serializer.DeserializeLong(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DimensioneCompressa":
                        {
                            DimensioneCompressa = (long)DMD.XML.Utils.Serializer.DeserializeLong(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "DataUltimaModifica":
                        {
                            DataUltimaModifica = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataCompressione":
                        {
                            DataCompressione = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Messages":
                        {
                            Messages = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            public void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("PercorsoOrigine", PercorsoOrigine);
                writer.WriteAttribute("FileDestinazione", FileDestinazione);
                writer.WriteAttribute("DimensioneOrigine", DimensioneOrigine);
                writer.WriteAttribute("DimensioneCompressa", DimensioneCompressa);
                writer.WriteAttribute("DataUltimaModifica", DataUltimaModifica);
                writer.WriteAttribute("DataCompressione", DataCompressione);
                writer.WriteTag("Messages", Messages);
            }

            //~CBackupItem()
            //{
            //    //DMDObject.DecreaseCounter(this);
            //}
        }
    }
}