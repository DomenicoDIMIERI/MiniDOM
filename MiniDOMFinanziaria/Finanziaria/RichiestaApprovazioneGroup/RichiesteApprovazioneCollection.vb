Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Finanziaria
Imports minidom.Internals

Partial Class Finanziaria

    Public Class RichiesteApprovazioneCollection
        Inherits CCollection(Of CRichiestaApprovazione)

        Private m_Group As RichiestaApprovazioneGroup

        Public Sub New()
            Me.m_Group = Nothing
        End Sub

        Public Sub New(ByVal group As RichiestaApprovazioneGroup)
            Me.New
            Me.Load(group)
        End Sub

        Public ReadOnly Property Group As RichiestaApprovazioneGroup
            Get
                Return Me.m_Group
            End Get
        End Property

        Protected Friend Sub SetGroup(ByVal value As RichiestaApprovazioneGroup)
            Me.m_Group = value
            If (value Is Nothing) Then Return
            For Each r As CRichiestaApprovazione In Me
                r.SetGruppo(value)
            Next
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Group IsNot Nothing) Then DirectCast(value, CRichiestaApprovazione).SetGruppo(Me.m_Group)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Group IsNot Nothing) Then DirectCast(newValue, CRichiestaApprovazione).SetGruppo(Me.m_Group)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Sub Load(ByVal group As RichiestaApprovazioneGroup)
            If (group Is Nothing) Then Throw New ArgumentNullException("group")
            Me.Clear()
            Me.m_Group = group
            If (GetID(group) = 0) Then Return
            Dim cursor As New CRichiestaApprovazioneCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IgnoreRights = True
            cursor.IDGruppo.Value = GetID(group)
            While Not cursor.EOF
                Me.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
            cursor = Nothing
        End Sub

    End Class

End Class