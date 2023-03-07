Public Class DecimalFormatSymbols

    Private _locale As Locale

    Sub New(locale As Locale)
        DMDObject.IncreaseCounter(Me)
        _locale = locale
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMDObject.DecreaseCounter(Me)
    End Sub
End Class