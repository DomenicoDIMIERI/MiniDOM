﻿Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports minidom
Imports minidom.Databases
Imports minidom.Anagrafica

Partial Public Class Sistema

    Public MustInherit Class CBaseEventHandler
        Implements IEventHandler

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Public MustOverride Sub NotifyEvent(ByVal e As EventDescription) Implements IEventHandler.NotifyEvent

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub


    End Class

End Class