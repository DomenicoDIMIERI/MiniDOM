Imports minidom.Anagrafica
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Office
Imports minidom.Internals

Namespace Internals

    <Serializable>
    Public NotInheritable Class CCandidatureClass
        Inherits CModulesClass(Of Candidatura)

        Friend Sub New()
            MyBase.New("modOfficeCandidature", GetType(CandidaturaCursor), 0)
        End Sub

    End Class

End Namespace

Partial Class Office



    Private Shared m_Candidature As CCandidatureClass = Nothing

    ''' <summary>
    ''' Modulo Canditature
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property Candidature As CCandidatureClass
        Get
            If (m_Candidature Is Nothing) Then m_Candidature = New CCandidatureClass
            Return m_Candidature
        End Get
    End Property


End Class