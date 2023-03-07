Imports minidom.Anagrafica
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Office
Imports minidom.Internals


Namespace Internals


    Public NotInheritable Class CMarcatureClass
        Inherits CModulesClass(Of MarcaturaIngressoUscita)

        Friend Sub New()
            MyBase.New("modOfficeIngressiUscite", GetType(MarcatureIngressoUscitaCursor))
        End Sub


    End Class
End Namespace

Partial Class Office

    Private Shared m_Marcature As CMarcatureClass = Nothing

    Public Shared ReadOnly Property Marcature As CMarcatureClass
        Get
            If (m_Marcature Is Nothing) Then m_Marcature = New CMarcatureClass
            Return m_Marcature
        End Get
    End Property

End Class