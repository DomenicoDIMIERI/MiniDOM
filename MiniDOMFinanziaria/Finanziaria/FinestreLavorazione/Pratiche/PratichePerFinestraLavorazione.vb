#Const CaricaDocumentiOnLoad = True

Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.CustomerCalls
Imports minidom.Office

Partial Public Class Finanziaria

    <Serializable>
    Public Class PratichePerFinestraLavorazione
        Inherits CCollection(Of CPraticaCQSPD)

        Private m_W As FinestraLavorazione

        Public Sub New()
            Me.m_W = Nothing
        End Sub

        Public Sub New(ByVal w As FinestraLavorazione)
            Me.New
            Me.Load(w)
        End Sub

        Public ReadOnly Property Finestra As FinestraLavorazione
            Get
                Return Me.m_W
            End Get
        End Property

        Protected Friend Sub SetFinestra(ByVal value As FinestraLavorazione)
            Me.m_W = value
            For Each p As CPraticaCQSPD In Me
                p.SetFinestraLavorazione(value)
            Next
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_W IsNot Nothing) Then DirectCast(value, CPraticaCQSPD).SetFinestraLavorazione(Me.m_W)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_W IsNot Nothing) Then DirectCast(newValue, CPraticaCQSPD).SetFinestraLavorazione(Me.m_W)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub


        Protected Sub Load(ByVal w As FinestraLavorazione)
            If (w Is Nothing) Then Throw New ArgumentNullException("w")
            Me.SetFinestra(w)
            If (GetID(w) = 0) Then Return
            Dim cursor As New CPraticheCQSPDCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IgnoreRights = True
            cursor.IDFinestraLavorazione.Value = GetID(w)
            While Not cursor.EOF
                Me.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
        End Sub



    End Class

End Class
