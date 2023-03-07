using System;
using DMD;

namespace minidom
{
    public partial class WebSite
    {

        /// <summary>
    /// Questa classe serve per stabilire un sistema di comunicazione ad eventi
    /// con
    /// </summary>
    /// <remarks></remarks>
        public class WebSiteDebugInfo : Sistema.SysDebuInfo
        {
            public WebSiteDebugInfo()
            {
                // DMDObject.IncreaseCounter(Me)
            }

            public override void Initialize()
            {
                base.Initialize();
                Notes += "";
                Notes += DMD.Strings.NChars(80, "-") + DMD.Strings.vbNewLine;
                Notes += "SESSIONI ATTIVE" + DMD.Strings.vbNewLine;
                Notes += DMD.Strings.NChars(80, "-") + DMD.Strings.vbNewLine;
                var col = ((WebApplicationContext)Sistema.ApplicationContext).GetAllSessions();
                foreach (CSessionInfo info in col)
                {
                    Notes += DMD.Strings.vbNewLine;
                    Notes += DMD.Strings.NChars(80, "-") + DMD.Strings.vbNewLine;
                    Notes += "SessionID: " + info.CurrentSession.SessionID + DMD.Strings.vbNewLine;
                    Notes += "StartTime: " + Sistema.Formats.FormatUserDateTime(info.CurrentSession.StartTime);
                    Notes += DMD.Strings.vbNewLine;
                    if (info.CurrentUser is object)
                    {
                        Notes += "User: " + info.CurrentUser.UserName + DMD.Strings.vbNewLine;
                    }

                    if (info.CurrentLogin is object)
                    {
                        Notes += "LogIn: " + info.CurrentLogin.RemoteIP + ": " + info.CurrentLogin.RemotePort + " (" + info.CurrentLogin.NomeUfficio + ")" + DMD.Strings.vbNewLine;
                        Notes += "       " + Sistema.Formats.FormatUserDateTime(info.CurrentLogin.LogInTime) + " - " + Sistema.Formats.FormatUserDateTime(info.CurrentLogin.LogOutTime) + " (" + Enum.GetName(typeof(Sistema.LogOutMethods), info.CurrentLogin.LogoutMethod) + ")" + DMD.Strings.vbNewLine;
                        Notes += "       " + info.CurrentLogin.UserAgent + DMD.Strings.vbNewLine;
                    }

                    Notes += DMD.Strings.vbNewLine;
                    lock (info.CurrentSession)
                    {
                        Notes += "Cursori aperti: " + info.RemoteOpenedCursors.Count + DMD.Strings.vbNewLine;
                        foreach (Databases.DBObjectCursorBase c in info.RemoteOpenedCursors)
                            Notes += DMD.RunTime.vbTypeName(c) + "[" + c.Token + ", " + Enum.GetName(typeof(Databases.DBCursorStatus), c.CursorStatus) + "]: " + c.GetFullSQL() + DMD.Strings.vbNewLine;
                    }

                    Notes += DMD.Strings.vbNewLine;
                    lock (info)
                    {
                        Notes += "";
                        Notes += DMD.Strings.NChars(80, "-") + DMD.Strings.vbNewLine;
                        Notes += "QUERIS PENDENTI " + DMD.Strings.vbNewLine;
                        Notes += DMD.Strings.NChars(80, "-") + DMD.Strings.vbNewLine;

                        // For Each item As StatsItem In info.QueriesInfo
                        // Me.Notes &= Formats.FormatUserDateTime(item.LastRun) & " " & item.Name & vbNewLine
                        // Next

                        // Me.Notes &= ""
                        // Me.Notes &= minidom.DMD.Strings.NChars(80, "-") & vbNewLine
                        // Me.Notes &= "RICHIESTE PENDENTI " & vbNewLine
                        // Me.Notes &= minidom.DMD.Strings.NChars(80, "-") & vbNewLine

                        // For Each item As StatsItem In info.PagesInfo
                        // Me.Notes &= Formats.FormatUserDateTime(item.LastRun) & " " & item.Name & vbNewLine
                        // Next
                    }
                }
            }
        }
    }
}