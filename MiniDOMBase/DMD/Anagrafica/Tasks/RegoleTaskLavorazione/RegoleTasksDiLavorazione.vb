Imports minidom
Imports minidom.Sistema
Imports minidom.Databases
Imports minidom.Anagrafica
Imports minidom.Internals

Namespace Internals


    Public NotInheritable Class CRegoleTasksDiLavorazioneClass
        Inherits CModulesClass(Of RegolaTaskLavorazione)

        Friend Sub New()
            MyBase.New("modAnaRegoleTaskLavorazione", GetType(RegolaTaskLavorazioneCursor), -1)
        End Sub



    End Class

End Namespace


Partial Public Class Anagrafica



    Private Shared m_RegoleTasksLavorazione As CRegoleTasksDiLavorazioneClass = Nothing

    Public Shared ReadOnly Property RegoleTasksLavorazione As CRegoleTasksDiLavorazioneClass
        Get
            If (m_RegoleTasksLavorazione Is Nothing) Then m_RegoleTasksLavorazione = New CRegoleTasksDiLavorazioneClass
            Return m_RegoleTasksLavorazione
        End Get
    End Property

End Class