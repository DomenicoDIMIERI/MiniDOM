Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports minidom.Net.Mime
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.Office

Namespace Internals

    Public Class CFoldersClass
        Inherits CModulesClass(Of MailFolder)

        Public Sub New()
            MyBase.New("modOfficeEMailsFolders", GetType(MailFolderCursor), 0)
        End Sub


    End Class

End Namespace
