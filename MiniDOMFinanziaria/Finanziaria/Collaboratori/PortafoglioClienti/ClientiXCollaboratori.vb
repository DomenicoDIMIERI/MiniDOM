Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Finanziaria
Imports minidom.Internals
Imports minidom.Anagrafica

Namespace Internals

    <Serializable>
    Public NotInheritable Class ClientiXCollaboratoriClass
        Inherits CModulesClass(Of ClienteXCollaboratore)


        Friend Sub New()
            MyBase.New("modCliXCollab", GetType(ClienteXCollaboratoreCursor), 0)
        End Sub

        Public Function GetItemByPersonaECollaboratore(ByVal p As CPersonaFisica, ByVal coll As CCollaboratore) As ClienteXCollaboratore
            If (p Is Nothing) Then Throw New ArgumentNullException("p")
            If (coll Is Nothing) Then Throw New ArgumentNullException("coll")
            Dim cursor As New ClienteXCollaboratoreCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDPersona.Value = GetID(p)
            cursor.IDCollaboratore.Value = GetID(coll)
            cursor.IgnoreRights = True
            Dim ret As ClienteXCollaboratore = cursor.Item
            cursor.Dispose()
            Return ret
        End Function


    End Class

End Namespace

