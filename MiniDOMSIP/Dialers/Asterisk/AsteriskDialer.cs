using System;
using DMD.XML;
using minidom.CallManagers.Actions;

namespace minidom.PBX
{
    public class AsteriskDialer : DialerBaseClass
    {

        // Private m_Thread As System.Threading.Thread
        private AsteriskServer m_Server;

        public AsteriskDialer()
        {
            // Me.m_Thread = Nothing
        }

        public AsteriskDialer(AsteriskServer server) : this()
        {
            m_Server = server;
        }

        public string PrepareNumber(string number)
        {
            return DMD.Strings.Trim(number);
        }

        public override void Dial(string number)
        {
            if (DMD.Strings.Len(number) <= 1)
                return;
            HangUp();

            // Me.m_Thread = New System.Threading.Thread(New System.Threading.ParameterizedThreadStart(AddressOf thread))
            // Me.m_Thread.Start(number)
            // End Sub

            // Private Sub thread(ByVal o As Object)
            // Dim Number As String = CStr(o)

            var e = new DialEventArgs(number);
            OnBegidDial(e);
            var a = new Originate();

            // If Not Me.m_Server.IsConnected Then Me.m_Server.Connect()
            // Me.m_Server.Disconnect()
            // System.Threading.Thread.Sleep(100)
            // Me.m_Server.Connect()
            // System.Threading.Thread.Sleep(100)
            if (!IsInstalled())
                return;

            // Me.m_Server.Connect()
            a.CallerID = "A: " + number; // Me.m_Server.CallerID
            a.Context = "from-internal";
            a.Channel = m_Server.Channel;
            a.Exten = number;
            a.Priority = 1;
            CallManagers.Responses.OriginateResponse r;
            try
            {
                r = (CallManagers.Responses.OriginateResponse)m_Server.GetManager().Execute(a, 1000);
            }
            catch (Exception ex)
            {
                Sistema.ApplicationContext.Log("AsteriskDialerException - RETRY 1 - " + ex.Message + DMD.Strings.vbNewLine + ex.StackTrace);
                try
                {
                    if (m_Server.IsConnected())
                        m_Server.Disconnect();
                    System.Threading.Thread.Sleep(500);
                    m_Server.Connect();
                    System.Threading.Thread.Sleep(500);
                    r = (CallManagers.Responses.OriginateResponse)m_Server.GetManager().Execute(a, 1000);
                }
                // If r.IsSuccess Then
                // Return
                // Else
                // Return
                // End If
                catch (Exception ex1)
                {
                    Sistema.ApplicationContext.Log("AsteriskDialerException - FAIL - " + ex1.Message + DMD.Strings.vbNewLine + ex1.StackTrace);
                }
            }

            OnEndDial(e);
        }

        public override bool IsInstalled()
        {
            return m_Server is object && m_Server.GetManager() is object && m_Server.GetManager().IsConnected();
        }

        public override string Name
        {
            get
            {
                return "Asterisk: " + m_Server.Channel;
            }
        }

        public override void HangUp()
        {
            // If (Me.m_Thread IsNot Nothing) Then
            // Me.m_Thread.Abort()
            // Me.m_Thread = Nothing
            // End If
        }

        public override bool Equals(object obj)
        {
            if (!(obj is C3CXDialer))
                return false;
            return base.Equals(obj) && ((AsteriskDialer)obj).m_Server.Equals(m_Server);
        }
    }
}