Imports minidom
Imports minidom.Sistema
Imports minidom.Forms


Namespace Forms


    Public Class VerticalBarChartRenderer
        Inherits VerticalBarOverlappedChartRenderer

        Private maxNumXSerie As Integer = 0

        Public Sub New(ByVal chart As CChart)
            MyBase.New(chart)
        End Sub





        Protected Overrides Sub CalculateParameters()
            Dim init As Boolean = False
            Dim c As CChart = Me.Chart
            Dim N As Integer = Math.Max(1, c.Labels.Count)
            Dim minVal As Single = 0, maxVal As Single = 0
            Dim ms As ChartMeasure = c.GetMeasures
            minVal = ms.MinY
            maxVal = ms.MaxY
            For Each serie As CChartSerie In c.Series
                ms = serie.GetMeasures
                N = Math.Max(serie.Values.Count, N)
                'If (Not init) Then
                '    If (ms.MinY.HasValue) Then minVal = ms.MinY.Value
                '    If (ms.MaxY.HasValue) Then maxVal = ms.MaxY.Value
                '    init = True
                'Else
                '    If (ms.MinY.HasValue) Then minVal = Math.Min(ms.MinY.Value, minVal)
                '    If (ms.MaxY.HasValue) Then maxVal = Math.Max(ms.MaxY.Value, minVal)
                'End If
            Next
            Me.maxNumXSerie = N

            Dim W As Single = Me.renderRect.Width
            Me.m_BarWidth = Math.Ceiling((W / N) - Me.barDistance)

            Me.minX = 0
            Me.minY = minVal
            Me.maxX = Me.m_BarWidth * N
            Me.maxY = maxVal

            Me.xScale = 1 : Me.yScale = 1
            If (Me.minY * Me.maxY > 0) Then
                Me.yScale = (Me.renderRect.Height - 50) / Me.maxY
                Me.yOrig = 0
            ElseIf Me.maxY > 0 Then
                Me.yScale = (Me.renderRect.Height - 50) / (Me.maxY - Me.minY)
                Me.yOrig = -Me.minY
            End If

            If (Me.minX * Me.maxX > 0) Then
                Me.xScale = (Me.renderRect.Width - 50) / Me.maxX
                Me.xOrig = 0
            ElseIf Me.maxY > 0 Then
                Me.xScale = (Me.renderRect.Width - 50) / (Me.maxX - Me.minX)
                Me.xOrig = -Me.minX
            End If
        End Sub

        Public Overrides ReadOnly Property Type As ChartTypes
            Get
                Return ChartTypes.VerticalBarsMultibars
            End Get
        End Property

        Protected Overrides Sub DrawSeries()
            Me.CalculateParameters()

            Me.Context.DrawRectangle(Drawing.Pens.Red, Me.renderRect)
            'For tmp As Single = 0 To renderRect.Width Step (GroupWidth + GroupDistance)
            '    Me.Context.DrawLine(Drawing.Pens.Red, renderRect.Left + tmp, renderRect.Top, renderRect.Left + tmp, renderRect.Bottom)
            'Next

            Dim c As CChart = Me.Chart
            Dim label As CChartLabel
            Dim serie As CChartSerie
            Dim color As System.Drawing.Color
            Dim brush As System.Drawing.Brush
            Dim value As CChartValue

            Dim font As New System.Drawing.Font("Tahoma", 10, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
            For j As Integer = 0 To Me.maxNumXSerie
                label = Nothing : If (j < c.Labels.Count) Then label = c.Labels(j)
                serie = Nothing : If (j < c.Series.Count) Then serie = c.Series(j)
                If (serie IsNot Nothing) Then
                    Dim x As Single
                    Dim y As Single = 0
                    x = j * (Me.barDistance + Me.BarWidth)

                    For Each value In serie.Values
                        If (value.Value.HasValue) Then
                            color = serie.GetRenderColor
                            brush = New System.Drawing.SolidBrush(color)

                            Me.FillRectangle(brush, x, y, Me.BarWidth, value.Value)
                            Me.DrawRectangle(Drawing.Pens.Black, x, y, Me.BarWidth, value.Value)

                            Dim str As String = Formats.FormatValuta(value.Value)
                            Dim ts As System.Drawing.SizeF = Me.Context.MeasureString(str, font)
                            If (serie.ShowLabels) Then Me.DrawString(str, font, Drawing.Brushes.Black, x, y * yScale + value.Value + (ts.Height + 1) / yScale)

                            y += value.Value.Value * yScale

                            brush.Dispose()
                        End If
                    Next


                End If

            Next
            font.Dispose()
        End Sub

        Protected Overrides Sub DrawXAxe()
            Dim c As CChart = Me.Chart
            If (Not c.XAxe.ShowValues) Then Return

            Dim m As ChartMeasure = c.GetMeasures
            Dim dx As Single = Me.BarWidth + Me.barDistance
            Dim font As New System.Drawing.Font("Tahoma", 10, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
            For j As Integer = 0 To Me.maxNumXSerie
                Dim label As CChartLabel = Nothing : If (j < c.Labels.Count) Then label = c.Labels(j)
                Dim str As String = "" : If (label IsNot Nothing) Then str = label.Label
                Dim ts As System.Drawing.SizeF = Me.Context.MeasureString(str, font)

                Me.DrawString(str, font, Drawing.Brushes.Black, 1 / xScale + j * dx, yOrig - 1 / yScale)
            Next
            font.Dispose()
            'MyBase.DrawXAxe()
        End Sub

        Protected Overrides Sub DrawYAxe()
            MyBase.DrawYAxe()
        End Sub


    End Class

End Namespace