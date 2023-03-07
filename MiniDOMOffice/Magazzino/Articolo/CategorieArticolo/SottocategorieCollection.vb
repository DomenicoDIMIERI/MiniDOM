Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Office
Imports minidom.Internals.Office

Partial Public Class Office

    <Serializable>
    Public Class SottocategorieCollection
        Inherits CCollection(Of CategoriaArticolo)

        <NonSerialized> Private m_Parent As CategoriaArticolo = Nothing

        Public Sub New()
            Me.m_Parent = Nothing
        End Sub

        Public Sub New(ByVal parent As CategoriaArticolo)
            Me.New()
            Me.Load(parent)
        End Sub


        Protected Friend Overridable Sub SetParent(ByVal parent As CategoriaArticolo)
            Me.m_Parent = parent
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Parent IsNot Nothing) Then DirectCast(newValue, CategoriaArticolo).SetParent(Me.m_Parent)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Parent IsNot Nothing) Then DirectCast(value, CategoriaArticolo).SetParent(Me.m_Parent)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Friend Overridable Sub Load(ByVal parent As CategoriaArticolo)
            If (parent Is Nothing) Then Throw New ArgumentNullException("parent")
            Me.Clear()
            If GetID(parent) = 0 Then Exit Sub
            Using cursor As New CategoriaArticoloCursor()
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDParent.Value = GetID(parent)
                cursor.Nome.SortOrder = SortEnum.SORT_ASC
                While Not cursor.EOF
                    Me.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            End Using
        End Sub

    End Class




End Class


