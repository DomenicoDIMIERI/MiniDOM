Imports minidom.Anagrafica
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Office
Imports minidom.Internals


Namespace Internals


    Public NotInheritable Class CPeriodiLavorati
        Inherits CModulesClass(Of PeriodoLavorato)

        Friend Sub New()
            MyBase.New("modOfficePeriodiLavorati", GetType(PeriodoLavoratoCursor), 0)
        End Sub


    End Class
End Namespace

Partial Class Office

    Private Shared m_PeriodiLavorati As CPeriodiLavorati = Nothing

    Public Shared ReadOnly Property PeriodiLavorati As CPeriodiLavorati
        Get
            If (m_PeriodiLavorati Is Nothing) Then m_PeriodiLavorati = New CPeriodiLavorati
            Return m_PeriodiLavorati
        End Get
    End Property

End Class