﻿Imports minidom.Databases
Imports minidom.Sistema

Partial Public Class Finanziaria

    Public Class CTAEGFunEvaluator
        Inherits minidom.FunEvaluator

        Public Durata As Integer
        Public Quota As Decimal
        Public NettoRicavo As Decimal

        Public Overrides Function EvalFunction(x As Double) As Double
            Dim s As Integer
            Dim ret As Double = 0
            s = Me.Durata
            While (s > 0)
                ret = ret + (CDbl(1) + x) ^ (-s / 12)
                s -= 1
            End While
            ret = ret * Me.Quota - Me.NettoRicavo
            Return ret
        End Function

    End Class


End Class