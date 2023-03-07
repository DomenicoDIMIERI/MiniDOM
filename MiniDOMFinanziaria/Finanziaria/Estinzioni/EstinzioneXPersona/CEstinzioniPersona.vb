Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica



Partial Public Class Finanziaria


    ''' <summary>
    ''' Rappresenta una collezione di estinzioni associate ad una persona
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CEstinzioniPersona
        Inherits CEstinzioniCollection

        <NonSerialized> Private m_Persona As CPersona

        Public Sub New()
            Me.m_Persona = Nothing
        End Sub

        Public Sub New(ByVal p As CPersona)
            Me.New()
            Me.Initialize(p)
        End Sub

        Public ReadOnly Property Persona As CPersona
            Get
                Return Me.m_Persona
            End Get
        End Property

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Persona IsNot Nothing) Then DirectCast(value, CEstinzione).SetPersona(Me.m_Persona)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Persona IsNot Nothing) Then DirectCast(newValue, CEstinzione).SetPersona(Me.m_Persona)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Function Initialize(ByVal persona As CPersona) As Boolean
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            Me.Clear()
            Me.m_Persona = persona
            If (GetID(persona) <> 0) Then
                Using cursor As New CEstinzioniCursor()
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.IDPersona.Value = GetID(persona)
                    cursor.DataInizio.SortOrder = SortEnum.SORT_ASC
                    cursor.IgnoreRights = True
                    While Not cursor.EOF
                        Me.Add(cursor.Item)
                        cursor.MoveNext()
                    End While
                End Using
            End If
            Return True
        End Function

    End Class


End Class