Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica

Imports minidom.Sistema
Imports System.Net.Mail

Partial Public Class ADV

    Public NotInheritable Class CRisultatiCampagnaClass
        Inherits CModulesClass(Of CRisultatoCampagna)

        Friend Sub New()
            MyBase.New("modADVResults", GetType(CRisultatoCampagnaCursor), 0)
        End Sub

    End Class

    Private Shared m_RisultatiCampagna As CRisultatiCampagnaClass = Nothing

    Public Shared ReadOnly Property RisultatiCampagna As CRisultatiCampagnaClass
        Get
            If (m_RisultatiCampagna Is Nothing) Then m_RisultatiCampagna = New CRisultatiCampagnaClass
            Return m_RisultatiCampagna
        End Get
    End Property

End Class