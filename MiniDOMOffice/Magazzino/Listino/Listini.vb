Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Office
Imports minidom.Internals.Office

Namespace Internals.Office

    Public Class CListiniClass
        Inherits CModulesClass(Of Listino)

        Public Sub New()
            MyBase.New("modOfficeListini", GetType(Listino), 0)
        End Sub

        Protected Overrides Function CreateModuleInfo() As CModule
            Dim m As CModule = MyBase.CreateModuleInfo()
            m.Parent = minidom.Office.Module
            m.Visible = True
            m.Save()
            Return m
        End Function

      

    End Class


End Namespace

Partial Public Class Office
     
    Private Shared m_Listini As CListiniClass = Nothing

    Public Shared ReadOnly Property Listini As CListiniClass
        Get
            If (m_Listini Is Nothing) Then m_Listini = New CListiniClass
            Return m_Listini
        End Get
    End Property




   
End Class


