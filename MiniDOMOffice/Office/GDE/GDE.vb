Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Office
Imports minidom.Internals

Namespace Internals

    Public NotInheritable Class CGDECLass
        Inherits CModulesClass(Of CDocumento)

        Friend Sub New()
            MyBase.New("modGDE", GetType(CDocumentiCursor), -1)
        End Sub

        Public Function GetItemByName(ByVal name As String) As CDocumento
            name = Strings.Trim(name)
            If (name = "") Then Return Nothing
            For Each d As CDocumento In Me.LoadAll
                If Strings.Compare(d.Nome, name) = 0 Then Return d
            Next
            Return Nothing
        End Function


    End Class

End Namespace

Partial Class Office


    Private Shared m_GDE As Internals.CGDECLass = Nothing

    Public Shared ReadOnly Property GDE As CGDECLass
        Get
            If (m_GDE Is Nothing) Then m_GDE = New CGDECLass
            Return m_GDE
        End Get
    End Property

End Class