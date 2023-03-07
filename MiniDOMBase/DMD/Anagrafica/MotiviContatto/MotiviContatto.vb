Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.Internals
Imports minidom.Anagrafica

Namespace Internals

    Public NotInheritable Class MotiviContattoClass
        Inherits CModulesClass(Of MotivoContatto)


        Friend Sub New()
            MyBase.New("modCRMMotiviContatto", GetType(MotiviContattoCursor), -1)
        End Sub

        Public Function GetItemByName(ByVal nome As String) As MotivoContatto
            nome = Strings.Trim(nome)
            If (nome = "") Then Return Nothing
            For Each item As MotivoContatto In Me.LoadAll
                If Strings.Compare(item.Nome, nome, CompareMethod.Text) = 0 Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetItemsByTipoContatto(ByVal tipo As String, ByVal isIn As Boolean) As CCollection(Of MotivoContatto)
            Dim ret As New CCollection(Of MotivoContatto)
            tipo = Strings.Trim(tipo)
            If (tipo = "") Then Return ret
            For Each item As MotivoContatto In Me.LoadAll
                If item.Stato = ObjectStatus.OBJECT_VALID AndAlso
                        Sistema.TestFlag(item.Flags, MotivoContattoFlags.Attivo) AndAlso
                        ((Sistema.TestFlag(item.Flags, MotivoContattoFlags.InEntrata) AndAlso isIn) OrElse (Sistema.TestFlag(item.Flags, MotivoContattoFlags.InUscita) AndAlso Not isIn)) AndAlso
                        (item.TipoContatto = "" OrElse
                    Strings.Compare(item.TipoContatto, tipo, CompareMethod.Text) = 0) Then
                    ret.Add(item)
                End If
            Next
            Return ret
        End Function

    End Class


End Namespace

Partial Public Class Anagrafica



    Private Shared m_MotiviContatto As MotiviContattoClass = Nothing

    Public Shared ReadOnly Property MotiviContatto As MotiviContattoClass
        Get
            If (m_MotiviContatto Is Nothing) Then m_MotiviContatto = New MotiviContattoClass
            Return m_MotiviContatto
        End Get
    End Property

End Class