Imports minidom
Imports minidom.Sistema
Imports minidom.Databases



Partial Public Class Anagrafica


    Public NotInheritable Class CAzioniTasksDiLavorazioneClass
        Inherits CModulesClass(Of AzioneTaskLavorazione)

        Friend Sub New()
            MyBase.New("modAnaAzioniTaskLavorazione", GetType(AzioneTaskLavorazioneCursor), 0)
        End Sub


    End Class

    Private Shared m_AzioniTasksLavorazione As CAzioniTasksDiLavorazioneClass = Nothing

    Public Shared ReadOnly Property AzioniTasksLavorazione As CAzioniTasksDiLavorazioneClass
        Get
            If (m_AzioniTasksLavorazione Is Nothing) Then m_AzioniTasksLavorazione = New CAzioniTasksDiLavorazioneClass
            Return m_AzioniTasksLavorazione
        End Get
    End Property

End Class