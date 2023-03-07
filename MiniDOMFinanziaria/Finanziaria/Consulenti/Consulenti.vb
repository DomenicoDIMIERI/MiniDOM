Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Internals
Imports minidom.Finanziaria

Namespace Internals

    <Serializable>
    Public NotInheritable Class CConsulentiClass
        Inherits CModulesClass(Of CConsulentePratica)

        Friend Sub New()
            MyBase.New("modCQSPDConsulenti", GetType(CConsulentiPraticaCursor), -1)
        End Sub

        Public Function GetItemByUser(ByVal user As CUser) As CConsulentePratica
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            Return GetItemByUser(GetID(user))
        End Function

        Public Function GetItemByUser(ByVal id As Integer) As CConsulentePratica
            If id = 0 Then Return Nothing

            For Each item As CConsulentePratica In Me.LoadAll
                If (item.IDUser = id) Then Return item
            Next


            Return Nothing
        End Function

        Public Function GetItemByName(nome As String) As CConsulentePratica
            nome = Strings.Trim(nome)
            If (nome = "") Then Return Nothing
            For Each c As CConsulentePratica In Me.LoadAll
                If (c.Stato = ObjectStatus.OBJECT_VALID AndAlso Strings.Compare(c.Nome, nome, CompareMethod.Text) = 0) Then Return c
            Next
            Return Nothing
        End Function
    End Class

End Namespace



Partial Public Class Finanziaria



    Private Shared m_Consulenti As CConsulentiClass = Nothing

    Public Shared ReadOnly Property Consulenti As CConsulentiClass
        Get
            If (m_Consulenti Is Nothing) Then m_Consulenti = New CConsulentiClass
            Return m_Consulenti
        End Get
    End Property

End Class