Imports minidom
Imports minidom.Databases
Imports minidom.Sistema

Imports minidom.Anagrafica
Imports minidom.CustomerCalls




Partial Public Class CustomerCalls

    Public NotInheritable Class CAppuntiClass
        Inherits CModulesClass(Of CAppunto)


        Friend Sub New()
            MyBase.New("modAppunti", GetType(CAppuntiCursor))
        End Sub
              


    End Class

    Private Shared m_Appunti As CAppuntiClass = Nothing

    Public Shared ReadOnly Property Appunti As CAppuntiClass
        Get
            If (m_Appunti Is Nothing) Then m_Appunti = New CAppuntiClass
            Return m_Appunti
        End Get
    End Property

End Class