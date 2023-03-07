Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Office
Imports minidom.Internals.Office

Namespace Internals.Office

    Public Class CMagazziniClass
        Inherits CModulesClass(Of Magazzino)

        Public Sub New()
            MyBase.New("modOfficeMagazzini", GetType(Magazzino), 0)
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
     
    Private Shared m_Magazzini As CMagazziniClass = Nothing

    Public Shared ReadOnly Property Magazzini As CMagazziniClass
        Get
            If (m_Magazzini Is Nothing) Then m_Magazzini = New CMagazziniClass
            Return m_Magazzini
        End Get
    End Property




   
End Class


