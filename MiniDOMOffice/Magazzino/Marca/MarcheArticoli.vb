Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Office
Imports minidom.Internals.Office

Namespace Internals.Office

    Public Class CMarcheArticoliClass
        Inherits CModulesClass(Of MarcaArticolo)

        Public Sub New()
            MyBase.New("modOfficeMarcheArticoli", GetType(MarcaArticolo), 0)
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
     
    Private Shared m_MarcheArticoli As CMarcheArticoliClass = Nothing

    Public Shared ReadOnly Property MarcheArticoli As CMarcheArticoliClass
        Get
            If (m_MarcheArticoli Is Nothing) Then m_MarcheArticoli = New CMarcheArticoliClass
            Return m_MarcheArticoli
        End Get
    End Property




   
End Class


