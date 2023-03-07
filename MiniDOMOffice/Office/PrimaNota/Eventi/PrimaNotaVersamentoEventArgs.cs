Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica

Partial Class Office

 
        Public Class PrimaNotaVersamentoEventArgs
            Inherits System.EventArgs

            Private m_Registrazione

        Public Sub New(ByVal registrazione As RigaPrimaNota)
            DMDObject.IncreaseCounter(Me)
            Me.m_Registrazione = registrazione
        End Sub

        Public ReadOnly Property Registrazione As RigaPrimaNota
                Get
                    Return Me.m_Registrazione
                End Get
            End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class