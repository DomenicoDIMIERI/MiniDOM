Imports minidom

Partial Class Sistema


    Public MustInherit Class BaseProceduraHandler
        Implements IProceduraHandler

        Public MustOverride Sub InitializeParameters(procedura As CProcedura) Implements IProceduraHandler.InitializeParameters

        Public MustOverride Sub Run(procedura As CProcedura) Implements IProceduraHandler.Run

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class

