Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica
Imports System.Security.Cryptography.X509Certificates
Imports minidom.Net.Mail


Partial Public Class Sistema

    Public Class MailMessageReceivedEventArgs
        Inherits MailMessageEventArgs

        Private m_DeleteOnServer As Boolean

        Public Sub New()

        End Sub

        Public Sub New(ByVal message As MailMessageEx, Optional ByVal cancelled As Boolean = False, Optional ByVal exception As System.Exception = Nothing)
            MyBase.New(message, cancelled, exception)

        End Sub

        Public Sub New(ByVal account As CEmailAccount, ByVal message As MailMessageEx, Optional ByVal cancelled As Boolean = False, Optional ByVal exception As System.Exception = Nothing)
            MyBase.New(account, message, cancelled, exception)
        End Sub

        ''' <summary>
        ''' Restitusice o imposta un valore booleano che indica se forzare l'eliminazione del messaggio dopo il download
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DeleteOnServer As Boolean
            Get
                Return Me.m_DeleteOnServer
            End Get
            Set(value As Boolean)
                Me.m_DeleteOnServer = value
            End Set
        End Property
    End Class


End Class