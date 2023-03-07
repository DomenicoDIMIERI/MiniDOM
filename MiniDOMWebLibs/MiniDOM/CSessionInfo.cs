using System;
using System.Globalization;
using DMD;
using DMD.XML;

namespace minidom
{
    public partial class WebSite
    {

        [Serializable]
        public class CSessionInfo 
            : IDMDXMLSerializable, IDisposable
        {
            [NonSerialized] public readonly object @lock = new object();

            public int InternalID;
            public string SessionID;
            public string RemoteIP;
            public string RemotePort;
            public DateTime StartTime;
            public DateTime? RemoteTime;
            public DateTime? ServerTime;
            public DateTime LastUpdated;
            public string CurrentUserName;
            [NonSerialized] public Sistema.CUser CurrentUser;
            [NonSerialized] public Anagrafica.CUfficio CurrentUfficio;
            [NonSerialized] public CSiteSession CurrentSession;
            [NonSerialized] public Sistema.CLoginHistory CurrentLogin;
            public bool ForceAbadon;
            [NonSerialized] public Sistema.CUser OriginalUser;
            public CKeyCollection Parameters;
            public string Descrizione;
            [NonSerialized] public CKeyCollection<Databases.DBObjectCursorBase> RemoteOpenedCursors = new CKeyCollection<Databases.DBObjectCursorBase>();

            // Public QueriesInfo As New CKeyCollection(Of StatsItem)
            // Public PagesInfo As New CKeyCollection(Of StatsItem)
            // Public WebServicesInfo As New CKeyCollection(Of StatsItem)
            // 'Public userSessions As New CCollection(Of CSiteSession)

            public CSessionInfo()
            {
                InternalID = 0;
                SessionID = "";
                RemoteIP = "";
                RemotePort = "";
                StartTime = DMD.DateUtils.Now();
                RemoteTime = default;
                ServerTime = default;
                CurrentUserName = "";
                CurrentUser = null;
                CurrentUfficio = null;
                CurrentSession = null;
                CurrentLogin = null;
                OriginalUser = null;
                Parameters = new CKeyCollection();
                Descrizione = "";
                ForceAbadon = false;
                LastUpdated = DMD.DateUtils.Now();
                RemoteOpenedCursors = new CKeyCollection<Databases.DBObjectCursorBase>();
            }

            public CSessionInfo(string sessionID) : this()
            {
                DMDObject.IncreaseCounter(this);
                SessionID = sessionID;
            }

            public void Reset()
            {
                lock (@lock)
                {
                    foreach (Databases.DBObjectCursorBase c in RemoteOpenedCursors)
                    {
                        try
                        {
                            c.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Sistema.ApplicationContext.Log("Session: " + SessionID + " resetting cursor " + DMD.RunTime.vbTypeName(c) + " error" + DMD.Strings.vbCrLf + ex.Message + DMD.Strings.vbCrLf + ex.StackTrace);
                        }
                    }

                    RemoteOpenedCursors.Clear();
                }
            }

            public void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case var @case when CultureInfo.CurrentCulture.CompareInfo.Compare(@case, "InternalID", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            InternalID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "SessionID":
                        {
                            SessionID = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RemoteIP":
                        {
                            RemoteIP = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "RemotePort":
                        {
                            RemotePort = DMD.Strings.CStr(DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue));
                            break;
                        }

                    case var case4 when CultureInfo.CurrentCulture.CompareInfo.Compare(case4, "RemoteTime", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            RemoteTime = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case var case5 when CultureInfo.CurrentCulture.CompareInfo.Compare(case5, "ServerTime", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            ServerTime = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case var case6 when CultureInfo.CurrentCulture.CompareInfo.Compare(case6, "CurrentUserID", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            CurrentUser = Sistema.Users.GetItemById((int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue));
                            break;
                        }

                    case var case7 when CultureInfo.CurrentCulture.CompareInfo.Compare(case7, "CurrentUserName", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            CurrentUserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case8 when CultureInfo.CurrentCulture.CompareInfo.Compare(case8, "OriginalUserID", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            OriginalUser = Sistema.Users.GetItemById((int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue));
                            break;
                        }

                    case var case9 when CultureInfo.CurrentCulture.CompareInfo.Compare(case9, "Descrizione", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case var case10 when CultureInfo.CurrentCulture.CompareInfo.Compare(case10, "StartTime", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            StartTime = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case var case11 when CultureInfo.CurrentCulture.CompareInfo.Compare(case11, "ForceAbadon", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            ForceAbadon = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case var case12 when CultureInfo.CurrentCulture.CompareInfo.Compare(case12, "PagesInfo", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            break;
                        }
                    // Me.PagesInfo.Clear()
                    // Dim tmp As CKeyCollection = fieldValue
                    // For Each k As String In tmp.Keys
                    // Me.PagesInfo.Add(k, tmp(k))
                    // Next
                    case var case13 when CultureInfo.CurrentCulture.CompareInfo.Compare(case13, "WebServicesInfo", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            break;
                        }
                    // Me.WebServicesInfo.Clear()
                    // Dim tmp As CKeyCollection = fieldValue
                    // For Each k As String In tmp.Keys
                    // Me.WebServicesInfo.Add(k, tmp(k))
                    // Next
                    case var case14 when CultureInfo.CurrentCulture.CompareInfo.Compare(case14, "QueriesInfo", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                        {
                            break;
                        }
                        // Me.QueriesInfo.Clear()
                        // Dim tmp As CKeyCollection = fieldValue
                        // For Each k As String In tmp.Keys
                        // Me.QueriesInfo.Add(k, tmp(k))
                        // Next
                }
            }

            public void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("InternalID", InternalID);
                writer.WriteAttribute("SessionID", SessionID);
                writer.WriteAttribute("RemoteIP", RemoteIP);
                writer.WriteAttribute("RemotePort", RemotePort);
                writer.WriteAttribute("RemoteTime", RemoteTime);
                writer.WriteAttribute("ServerTime", ServerTime);
                writer.WriteAttribute("CurrentUserID", Databases.GetID(CurrentUser));
                writer.WriteAttribute("CurrentUserName", CurrentUserName);
                writer.WriteAttribute("OriginalUserID", Databases.GetID(OriginalUser));
                writer.WriteAttribute("Descrizione", Descrizione);
                writer.WriteAttribute("StartTime", StartTime);
                writer.WriteAttribute("ForceAbadon", ForceAbadon);
                // writer.WriteTag("Parameters", Me.Parameters)
                // writer.WriteTag("QueriesInfo", Me.QueriesInfo)
                // writer.WriteTag("PagesInfo", Me.PagesInfo)
                // writer.WriteTag("WebServicesInfo", Me.WebServicesInfo)
            }


            // Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
            public virtual void Dispose()
            {
                // Me.RemoteIP = vbNullString
                // Me.RemotePort = vbNullString
                // Me.RemoteTime = Nothing
                // Me.ServerTime = Nothing
                // Me.CurrentUserName = vbNullString
                // Me.CurrentUser = Nothing
                // Me.CurrentUfficio = Nothing
                // Me.CurrentSession = Nothing
                // Me.CurrentLogin = Nothing
                // Me.OriginalUser = Nothing
                lock (@lock)
                {
                    while (RemoteOpenedCursors.Count > 0)
                    {
                        var c = RemoteOpenedCursors[0];
                        RemoteOpenedCursors.RemoveAt(0);
                        c.Dispose();
                    }
                }

                // Me.RemoteOpenedCursors = Nothing
            }

            ~CSessionInfo()
            {
                DMDObject.DecreaseCounter(this);
            }


            // Public Function BeginPage(ByVal pageName As String) As StatsItem
            // SyncLock Me.PagesInfo
            // Dim info As StatsItem = Me.PagesInfo.GetItemByKey(pageName)
            // If (info Is Nothing) Then
            // info = New StatsItem
            // info.Name = pageName
            // Me.PagesInfo.Add(info.Name, info)
            // End If
            // info.Begin()
            // Return New StatsItem(pageName, info.Count, info.LastRun, GC.GetTotalMemory(False))
            // End SyncLock
            // End Function

            // Public Function EndPage(ByVal info As StatsItem) As StatsItem
            // SyncLock Me.PagesInfo
            // Dim ret As StatsItem = Me.PagesInfo.GetItemByKey(info.Name)
            // ret.End(info)
            // Return ret
            // End SyncLock
            // End Function

            // Public Function BeginWebService(ByVal pageName As String) As StatsItem
            // SyncLock Me.WebServicesInfo
            // Dim info As StatsItem = Me.WebServicesInfo.GetItemByKey(pageName)
            // If (info Is Nothing) Then
            // info = New StatsItem
            // info.Name = pageName
            // Me.WebServicesInfo.Add(info.Name, info)
            // End If
            // info.Begin()
            // Return New StatsItem(pageName, info.Count, info.LastRun, GC.GetTotalMemory(False))
            // End SyncLock
            // End Function

            // Public Function EndWebService(ByVal info As StatsItem) As StatsItem
            // SyncLock Me.WebServicesInfo
            // Dim ret As StatsItem = Me.WebServicesInfo.GetItemByKey(info.Name)
            // ret.End(info)
            // Return ret
            // End SyncLock
            // End Function


        }
    }
}