Imports minidom.Anagrafica
Imports minidom.Internals
Imports minidom.Sistema

Namespace Internals

    Public NotInheritable Class CTipiRapportoClass
        Inherits CModulesClass(Of CTipoRapporto)

        Public Sub New()
            MyBase.New("modTipiRapporto", GetType(CTipoRapportoCursor), -1)
        End Sub

        Public Function GetItemByIdTipoRapporto(ByVal sigla As String) As CTipoRapporto
            sigla = Left(Trim(sigla), 1)
            If (sigla = "") Then Return Nothing
            For Each item As CTipoRapporto In Me.LoadAll
                If Strings.Compare(item.IdTipoRapporto, sigla) = 0 Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetItemByName(ByVal name As String) As CTipoRapporto
            name = Trim(name)
            If (name = "") Then Return Nothing
            For Each item As CTipoRapporto In Me.LoadAll
                If Strings.Compare(item.Descrizione, name) = 0 Then Return item
            Next
            Return Nothing
        End Function



    End Class

End Namespace

Partial Public Class Anagrafica

    Private Shared m_TipiRapporto As CTipiRapportoClass = Nothing

    Public Shared ReadOnly Property TipiRapporto As CTipiRapportoClass
        Get
            If (m_TipiRapporto Is Nothing) Then m_TipiRapporto = New CTipiRapportoClass
            Return m_TipiRapporto
        End Get
    End Property

End Class
