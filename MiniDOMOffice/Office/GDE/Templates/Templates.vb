Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Internals
Imports minidom.Office
Imports System.Drawing

Namespace Internals

    <Serializable>
    Public Class CGDETemplatesClass
        Inherits CModulesClass(Of DocumentTemplate)

        Public Sub New()
            MyBase.New("modGDETemplates", GetType(DocumentTemplateCursor), -1)
        End Sub

        Public Function GetItemByName(ByVal name As String) As DocumentTemplate
            name = Strings.Trim(name)
            If (name = "") Then Return Nothing
            Dim items As CCollection(Of DocumentTemplate) = Me.LoadAll
            For Each item As DocumentTemplate In items
                If (item.Stato = ObjectStatus.OBJECT_VALID AndAlso Strings.Compare(item.Name, name, CompareMethod.Text) = 0) Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetItemsByContext(ByVal context As String) As CCollection(Of DocumentTemplate)
            Dim ret As New CCollection(Of DocumentTemplate)
            context = Strings.Trim(context)
            Dim items As CCollection(Of DocumentTemplate) = Me.LoadAll
            For Each item As DocumentTemplate In items
                If (item.Stato = ObjectStatus.OBJECT_VALID AndAlso Strings.Compare(item.ContextType, context, CompareMethod.Text) = 0) Then ret.Add(item)
            Next
            Return ret
        End Function


    End Class

End Namespace


Partial Class Office


    Private Shared m_Templates As CGDETemplatesClass = Nothing

    Public Shared ReadOnly Property Templates As CGDETemplatesClass
        Get
            If (m_Templates Is Nothing) Then m_Templates = New CGDETemplatesClass
            Return m_Templates
        End Get
    End Property

End Class