Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.Anagrafica
Imports minidom.CustomerCalls
Imports minidom.Internals

Namespace Internals

    Public NotInheritable Class CPauseCRMClass
        Inherits CModulesClass(Of CSessioneCRM)


        Friend Sub New()
            MyBase.New("modCRMPause", GetType(CPausaCRMCursor), 0)
        End Sub

        Protected Overrides Function CreateModuleInfo() As CModule
            Return MyBase.CreateModuleInfo()
        End Function

    End Class

End Namespace

Partial Public Class CustomerCalls

    Private Shared m_PauseCRM As CPauseCRMClass = Nothing

    Public Shared ReadOnly Property PauseCRM As CPauseCRMClass
        Get
            If (m_PauseCRM Is Nothing) Then m_PauseCRM = New CPauseCRMClass
            Return m_PauseCRM
        End Get
    End Property


End Class