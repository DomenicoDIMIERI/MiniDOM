using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using DMD;
using DMD.XML;

namespace minidom
{
    public partial class WebSite
    {
        public class CCustomFileUploader : CFileUploader
        {
            private enum StatusFlag
            {
                UPLOADER_CERCAFIRMA = 0, // Siamo ancora in attesa della firma
                UPLOADER_CERCAINFO = 1,        // Siamo alla ricerca delle info sul file
                UPLOADER_CERCATIPO = 2,        // Siamo alla ricerca del tipo di dati inviati
                UPLOADER_SCARICA = 3      // Stiamo scaricando i dati
            }

            private Stream m_InSream;
            private Stream m_OutputStream;
            private StatusFlag stato;
            private string strBuffer;
            private string firma;
            private string contentDisposition;
            private string contentType;
            private string m_FieldName;

            public CCustomFileUploader()
            {
                m_InSream = null;
                m_OutputStream = null;
                stato = StatusFlag.UPLOADER_CERCAFIRMA;
                firma = DMD.Strings.vbNullString;
                contentDisposition = DMD.Strings.vbNullString;
                contentType = DMD.Strings.vbNullString;
                m_FieldName = DMD.Strings.vbNullString;
            }

            public override void Dispose()
            {
                base.Dispose();
                if (m_InSream is object)
                    m_InSream = null;
                if (m_OutputStream is object)
                {
                    m_OutputStream.Dispose();
                    m_OutputStream = null;
                }

                strBuffer = DMD.Strings.vbNullString;
                firma = DMD.Strings.vbNullString;
                contentDisposition = DMD.Strings.vbNullString;
                contentType = DMD.Strings.vbNullString;
                m_FieldName = DMD.Strings.vbNullString;
            }

            public string FieldName
            {
                get
                {
                    return m_FieldName;
                }

                set
                {
                    m_FieldName = value;
                }
            }

            /// <summary>
        /// Avvia il caricamento
        /// </summary>
        /// <remarks></remarks>
            public void SetFields(string destURL, string destFile, bool async)
            {
                DestinationUrl = destURL;
                TargetFileName = destFile;
                Async = async;
            }

            protected override void PrepareUpload()
            {
                m_InSream = ASP_Request.InputStream;
                stato = StatusFlag.UPLOADER_CERCAFIRMA;
                strBuffer = "";
                firma = "";
                m_OutputStream = new FileStream(TargetFileName, FileMode.CreateNew);
            }

            protected override int DoReader(byte[] buffer)
            {
                // Carichiamo il buffer dal computer remoto
                int n = (int)(TotalBytes - UploadedBytes);
                byte[] tmp;
                if (n > BufferSize)
                    n = BufferSize;
                tmp = ASP_Request.BinaryRead(BufferSize);
                Array.Copy(tmp, buffer, n);
                return n;
            }

            protected override void DoConsumer(byte[] buffer, int nBytes)
            {
                base.DoConsumer(buffer, nBytes);
                strBuffer += System.Text.Encoding.Default.GetString(buffer, 0, nBytes);
                int p, j;
                string[] items;
                switch (stato)
                {
                    case StatusFlag.UPLOADER_CERCAFIRMA:
                        {
                            // Cerchiamo l'inizio del file da uploadare
                            p = Strings.InStr(strBuffer, DMD.Strings.vbCrLf);
                            if (p > 0)
                            {
                                firma = Strings.Left(strBuffer, p - 1);
                                strBuffer = Strings.Mid(strBuffer, p + 2);
                                stato = StatusFlag.UPLOADER_CERCAINFO;
                            }

                            break;
                        }

                    case StatusFlag.UPLOADER_CERCAINFO:
                        {
                            // Cerchiamo l'inizio del file da uploadare
                            p = Strings.InStr(strBuffer, DMD.Strings.vbCrLf);
                            if (p > 0)
                            {
                                items = Strings.Split(Strings.Left(strBuffer, p - 1), ";");
                                for (int i = DMD.Arrays.LBound(items), loopTo = DMD.Arrays.UBound(items); i <= loopTo; i++)
                                {
                                    j = Strings.InStr(items[i], "=");
                                    if (j < 1)
                                        j = Strings.InStr(items[i], ":");
                                    if (j >= 1)
                                    {
                                        switch (Strings.LCase(Strings.Trim(Strings.Left(items[i], j - 1))) ?? "")
                                        {
                                            case var @case when CultureInfo.CurrentCulture.CompareInfo.Compare(@case, "content-disposition", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                                                {
                                                    contentDisposition = Strings.Mid(items[i], j + 1);
                                                    break;
                                                }

                                            case var case1 when CultureInfo.CurrentCulture.CompareInfo.Compare(case1, "name", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                                                {
                                                    m_FieldName = Strings.Mid(items[i], j + 1);
                                                    break;
                                                }

                                            case var case2 when CultureInfo.CurrentCulture.CompareInfo.Compare(case2, "filename", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                                                {
                                                    FileName = Strings.Mid(items[i], j + 1);
                                                    if (Strings.Left(FileName, 1) == DMD.Strings.CStr('"'))
                                                    {
                                                        FileName = Strings.Mid(FileName, 2);
                                                        j = Strings.InStr(FileName, DMD.Strings.CStr('"'));
                                                        contentType = Strings.Mid(FileName, j + 1);
                                                        FileName = Strings.Left(FileName, j - 1);
                                                        j = Strings.InStr(contentType, ":");
                                                        contentType = Strings.Trim(Strings.Mid(contentType, j + 1));
                                                    }

                                                    break;
                                                }

                                            default:
                                                {
                                                    break;
                                                }
                                        }
                                    }
                                }

                                stato = StatusFlag.UPLOADER_CERCATIPO;
                                strBuffer = Strings.Mid(strBuffer, p + 2);
                            }

                            break;
                        }

                    case StatusFlag.UPLOADER_CERCATIPO:
                        {
                            // Cerchiamo l'inizio del file da uploadare
                            p = Strings.InStr(strBuffer, DMD.Strings.vbCrLf);
                            if (p > 0)
                            {
                                items = Strings.Split(Strings.Left(strBuffer, p - 1), ";");
                                for (int i = DMD.Arrays.LBound(items), loopTo1 = DMD.Arrays.UBound(items); i <= loopTo1; i++)
                                {
                                    j = Strings.InStr(items[i], "=");
                                    if (j < 1)
                                        j = Strings.InStr(items[i], ":");
                                    if (j >= 1)
                                    {
                                        switch (Strings.LCase(Strings.Trim(Strings.Left(items[i], j - 1))) ?? "")
                                        {
                                            case var case3 when CultureInfo.CurrentCulture.CompareInfo.Compare(case3, "content-type", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                                                {
                                                    contentType = Strings.Mid(items[i], j + 1);
                                                    break;
                                                }

                                            default:
                                                {
                                                    break;
                                                }
                                        }
                                    }
                                }

                                stato = StatusFlag.UPLOADER_SCARICA;
                                strBuffer = Strings.Mid(strBuffer, p + 4);
                                if (Strings.Len(strBuffer) > 0)
                                {
                                    buffer = System.Text.Encoding.Default.GetBytes(strBuffer);
                                    m_OutputStream.Write(buffer, 0, nBytes);
                                }
                            }

                            break;
                        }

                    case StatusFlag.UPLOADER_SCARICA:
                        {
                            p = Strings.InStr(strBuffer, firma);
                            if (p > 0)
                            {
                                // fStream.WriteText(Left(bigBuffer, p - 1))
                                // fStream.SaveToFile(lastUploadedFilePath, 2)
                                // fStream.Close()
                                // fStream = Nothing

                                // bigBuffer = Mid(bigBuffer, p + Len(firma) + 2)
                                // stato = UPLOADER_CERCAINFO
                                // FileName = ""
                                // FieldName = ""
                                // contentType = ""
                                // m_CurrentUpload.Percentage = 100
                                Debug.Print(firma);
                            }
                            else
                            {
                                m_OutputStream.Write(buffer, 0, nBytes);
                            }

                            break;
                        }

                    default:
                        {
                            throw new InvalidOperationException("Stato non valido");
                            break;
                        }
                }
            }

            protected override void FinalizeUpload()
            {
                if (m_OutputStream is object)
                    m_OutputStream.Dispose();
                // If (Me.m_InputStream IsNot Nothing) Then Me.m_InputStream.Dispose()
                m_OutputStream = null;
                m_InSream = null;
            }

            protected override long GetTotalBytesToReceive()
            {
                return ASP_Request.TotalBytes;
            }
        }
    }
}