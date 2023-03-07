Imports minidom
Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica

Namespace CQSPDInternals

    Public Class CRichiesteApprovazioneClass
        Inherits CModulesClass(Of Finanziaria.CRichiestaApprovazione)

        Public Sub New()
            MyBase.New("modCQSPDRichiesteApprovazione", GetType(Finanziaria.CRichiestaApprovazioneCursor), 0)
        End Sub

        ''' <summary>
        ''' Restituisce tutte le richieste di approvazione generate per l'oggetto specificate oridinate per Data della richiesta crescente
        ''' </summary>
        ''' <param name="oggetto"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRichiesteByOggetto(ByVal oggetto As Object) As CCollection(Of Finanziaria.CRichiestaApprovazione)
            If (oggetto Is Nothing) Then Throw New ArgumentNullException("oggetto")
            Dim ret As New CCollection(Of Finanziaria.CRichiestaApprovazione)
            Dim cursor As New Finanziaria.CRichiestaApprovazioneCursor
            'cursor.IgnoreRights = True
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.TipoOggettoApprovabile.Value = TypeName(oggetto)
            cursor.IDOggettoApprovabile.Value = GetID(oggetto)
            cursor.DataRichiestaApprovazione.SortOrder = SortEnum.SORT_ASC
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()

            Return ret
        End Function

        Protected Friend Shadows Sub doItemCreated(ByVal e As ItemEventArgs)
            MyBase.doItemCreated(e)
        End Sub

        Protected Friend Shadows Sub doItemDeleted(ByVal e As ItemEventArgs)
            MyBase.doItemDeleted(e)
        End Sub

        Protected Friend Shadows Sub doItemModified(ByVal e As ItemEventArgs)
            MyBase.doItemModified(e)
        End Sub

    End Class

End Namespace


Partial Public Class Finanziaria


    Private Shared m_RichiesteApprovazione As CQSPDInternals.CRichiesteApprovazioneClass

    Public Shared ReadOnly Property RichiesteApprovazione As CQSPDInternals.CRichiesteApprovazioneClass
        Get
            If (m_RichiesteApprovazione Is Nothing) Then m_RichiesteApprovazione = New CQSPDInternals.CRichiesteApprovazioneClass
            Return m_RichiesteApprovazione
        End Get
    End Property



    


End Class
