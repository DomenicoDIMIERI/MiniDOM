Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.Internals
Imports minidom.Anagrafica

Namespace Internals


    <Serializable>
    Public Class CMergePersone
        Inherits CModulesClass(Of CMergePersona)

        Public Sub New()
            MyBase.New("modMergePersone", GetType(CMergePersonaCursor), 0)
        End Sub

        Public Function GetLastMerge(ByVal persona As CPersona) As CMergePersona
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            Dim cursor As New CMergePersonaCursor
            cursor.IDPersona1.Value = GetID(persona)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.DataOperazione.SortOrder = SortEnum.SORT_DESC
            Dim ret As CMergePersona = cursor.Item
            cursor.Dispose()
            Return ret
        End Function


    End Class

End Namespace


Partial Public Class Anagrafica

    <Serializable>
    Public Class MergeModuleGruppoPersona
        Implements minidom.XML.IDMDXMLSerializable

        Public items As New CCollection(Of CPersonaInfo)

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Public Function IsSameGroup(ByVal p2 As CPersonaInfo) As Boolean
            If (Me.items.Count = 0) Then Return True
            Dim p1 As CPersonaInfo = Me.items(0)
            If (p1.Persona.TipoPersona <> p2.Persona.TipoPersona) Then Return False
            If (
                (p1.Persona.Nominativo = p2.Persona.Nominativo) AndAlso
                (p1.Persona.DataNascita.HasValue AndAlso p2.Persona.DataNascita.HasValue AndAlso DateUtils.Compare(p1.Persona.DataNascita, p2.Persona.DataNascita) = 0)
                ) Then Return True
            For Each c1 As CContatto In p1.Persona.Recapiti
                If (c1.Valore <> "") Then 'AndAlso (c1.Validated .HasValue =False OrElse c1.Validated = True ) Then
                    For Each c2 As CContatto In p2.Persona.Recapiti
                        If (c2.Valore = c1.Valore) Then Return True
                    Next
                End If
            Next
            Return False
        End Function

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "items" : Me.items.Clear() : Me.items.AddRange(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteTag("items", Me.items)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub
    End Class



    Private Shared m_MergePersone As CMergePersone = Nothing

    Public Shared ReadOnly Property MergePersone As CMergePersone
        Get
            If m_MergePersone Is Nothing Then m_MergePersone = New CMergePersone
            Return m_MergePersone
        End Get
    End Property
End Class