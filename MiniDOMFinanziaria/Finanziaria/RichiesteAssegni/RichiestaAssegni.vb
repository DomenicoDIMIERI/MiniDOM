Imports minidom
Imports minidom.Sistema
Imports minidom.Databases

Partial Class Finanziaria

    Public NotInheritable Class CRichiesteAssegniClass
        Inherits CModulesClass(Of CRichiestaAssegni)

        Friend Sub New()
            MyBase.New("RichiesteAssegni", GetType(CRichiestaAssegniCursor))
        End Sub


    End Class

    Private Shared m_RichiestaAssegni As CRichiesteAssegniClass = Nothing

    Public Shared ReadOnly Property RichiesteAssegni As CRichiesteAssegniClass
        Get
            If (m_RichiestaAssegni Is Nothing) Then m_RichiestaAssegni = New CRichiesteAssegniClass
            Return m_RichiestaAssegni
        End Get
    End Property


End Class