Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Finanziaria
Imports minidom.Internals

Namespace Internals

    <Serializable>
    Public NotInheritable Class CInfoPraticaClass
        Inherits CModulesClass(Of CInfoPratica)

        Public Sub New()
            MyBase.New("modInfoPraticaCQSPD", GetType(CInfoPraticaCursor), 0)
        End Sub
        'Friend Sub New()
        '    DMDObject.IncreaseCounter(Me)
        'End Sub

        'Protected Overrides Sub Finalize()
        '    MyBase.Finalize()
        '    DMDObject.DecreaseCounter(Me)
        'End Sub

        Public Overrides Function GetItemById(ByVal id As Integer) As CInfoPratica
            If (id = 0) Then Return Nothing
            Dim cursor As New CInfoPraticaCursor
            cursor.IgnoreRights = True
            cursor.PageSize = 1
            cursor.ID.Value = id
            Dim ret As CInfoPratica = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
        End Function

        Public Function GetItemByPratica(ByVal pratica As CPraticaCQSPD) As CInfoPratica
            If (pratica Is Nothing) Then Throw New ArgumentNullException("pratica")
            If (GetID(pratica) = 0) Then Return Nothing
            Dim cursor As New CInfoPraticaCursor
            cursor.IDPratica.Value = GetID(pratica)
            cursor.IgnoreRights = True
            cursor.PageSize = 1
            Dim ret As CInfoPratica = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
        End Function

    End Class

End Namespace


Partial Public Class Finanziaria



    Private Shared m_InfoPratica As CInfoPraticaClass = Nothing

    Public Shared ReadOnly Property InfoPratica As CInfoPraticaClass
        Get
            If (m_InfoPratica Is Nothing) Then m_InfoPratica = New CInfoPraticaClass
            Return m_InfoPratica
        End Get
    End Property


End Class
