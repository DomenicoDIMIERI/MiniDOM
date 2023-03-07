Imports minidom.Anagrafica
Imports minidom.Databases
Imports minidom.Office
Imports minidom.Sistema
Imports minidom.Internals

Namespace Internals



End Namespace

Partial Class Office


    Public NotInheritable Class CPercorsiDefinitiClass
        Inherits CModulesClass(Of PercorsoDefinito)

        Friend Sub New()
            MyBase.New("modOfficePercorsiDefiniti", GetType(PercorsiDefinitiCursor), -1)
        End Sub


    End Class

    Private Shared m_PercorsiDefiniti As CPercorsiDefinitiClass = Nothing

    Public Shared ReadOnly Property PercorsiDefiniti As CPercorsiDefinitiClass
        Get
            If (m_PercorsiDefiniti Is Nothing) Then m_PercorsiDefiniti = New CPercorsiDefinitiClass
            Return m_PercorsiDefiniti
        End Get
    End Property

End Class