Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Finanziaria
Imports minidom.Internals
Imports minidom.Anagrafica

Namespace Internals

    <Serializable>
    Public Class RichiestaApprovazioneGroupClass
        Inherits CModulesClass(Of RichiestaApprovazioneGroup)

        Public Sub New()
            MyBase.New("modCQSPDRichApprGrp", GetType(RichiestaApprovazioneGroupCursor), 0)
        End Sub

        Public Function GetRichiestaByPersona(ByVal p As CPersonaFisica) As RichiestaApprovazioneGroup
            Dim cursor As RichiestaApprovazioneGroupCursor = Nothing
            Dim ret As RichiestaApprovazioneGroup = Nothing
#If Not DEBUG Then
            Try
#End If
            If (p Is Nothing) Then Throw New ArgumentNullException("p")

            cursor = New RichiestaApprovazioneGroupCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IgnoreRights = True
            cursor.IDCliente.Value = GetID(p)
            cursor.DataEsito.Value = Nothing
            cursor.DataEsito.IncludeNulls = True
            ret = cursor.Item
            If (ret IsNot Nothing) Then ret.SetCliente(p)

#If Not DEBUG Then
            Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
            Throw
           Finally
#End If
            If cursor IsNot Nothing Then cursor.Dispose() : cursor = Nothing
#If Not DEBUG Then
            End Try
#End If
            Return ret
        End Function

        Friend Sub doOnRichiedi(e As ItemEventArgs(Of RichiestaApprovazioneGroup))
            Me.Module.DispatchEvent(New EventDescription("richiedi", "Richiesta Approvazione", e.Item))
        End Sub

        Friend Sub doOnApprova(e As ItemEventArgs(Of RichiestaApprovazioneGroup))
            Me.Module.DispatchEvent(New EventDescription("approva", "Approvazione Concessa", e.Item))
        End Sub

        Friend Sub doOnRifiuta(e As ItemEventArgs(Of RichiestaApprovazioneGroup))
            Me.Module.DispatchEvent(New EventDescription("rifiuta", "Approvazione Negata", e.Item))
        End Sub


    End Class

End Namespace

Partial Class Finanziaria

    Private Shared m_RichiesteApprovazioneGroups As RichiestaApprovazioneGroupClass = Nothing

    Public Shared ReadOnly Property RichiesteApprovazioneGroups As RichiestaApprovazioneGroupClass
        Get
            If (m_RichiesteApprovazioneGroups Is Nothing) Then m_RichiesteApprovazioneGroups = New RichiestaApprovazioneGroupClass()
            Return m_RichiesteApprovazioneGroups
        End Get
    End Property

End Class