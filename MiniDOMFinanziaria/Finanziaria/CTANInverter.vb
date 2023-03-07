Imports minidom.Databases
Imports minidom.Sistema

Partial Public Class Finanziaria


    ''' <summary>
    ''' Calcola il valore del capitale finanziato in funzione del TAN
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CTANInverter
        Private m_Rata As Double
        Private m_Durata As Integer
        Private m_TAN As Double
        Private m_CapitaleFinanziato As Double
        Private m_Calculated As Boolean

        Public Sub New(ByVal rata As Double, ByVal durata As Double, ByVal tan As Double)
            Me.m_Rata = rata
            Me.m_Durata = durata
            Me.m_TAN = tan
            Me.m_CapitaleFinanziato = 0
            Me.m_Calculated = False
        End Sub

        Public Sub New()
            Me.New(0, 120, 0)
        End Sub

        Public Sub Invalidate()
            Me.m_Calculated = False
        End Sub

        Public Property Rata As Double
            Get
                Return Me.m_Rata
            End Get
            Set(value As Double)
                If (Me.m_Rata = value) Then Return
                Me.m_Rata = value
                Me.Invalidate
            End Set
        End Property

        Public Property Durata As Integer
            Get
                Return Me.m_Durata
            End Get
            Set(value As Integer)
                If (Me.m_Durata = value) Then Return
                Me.m_Durata = value
                Me.Invalidate()
            End Set
        End Property

        Public Property TAN As Double
            Get
                Return Me.m_TAN
            End Get
            Set(value As Double)
                If (Me.m_TAN = value) Then Return
                Me.m_TAN = value
                Me.Invalidate()
            End Set
        End Property

        Public ReadOnly Property CapitaleFinanziato As Double
            Get
                If (Me.m_Calculated = False) Then Me.Calc()
                Return Me.m_CapitaleFinanziato
            End Get
        End Property

        Public Function Calc() As Double
            If (Me.m_Calculated) Then Return Me.m_CapitaleFinanziato
            Dim fEval As New CTANFunEvaluator
            Dim errCode As Integer = 0
            'Dim ret As Double
            fEval.Durata = Me.Durata
            fEval.TAN = Me.TAN
            fEval.Rata = Me.Rata
            Dim ml As Double = Me.Rata * Me.Durata

            Dim x As Double = 5
            Dim tan1 As Double = 0
            Dim cf As Double
            Do
                cf = Math.FindZero(fEval, ml * x / 10, ml)
                Dim cc As New CTANCalculator()
                cc.Rata = Me.Rata
                cc.Durata = Me.Durata
                cc.Importo = cf
                tan1 = cc.Calc()
                x += 1
            Loop While (x < 10 AndAlso Math.Abs(tan1 - Me.TAN) >= 0.001)

            Return IIf(x >= 10, 0, cf)
        End Function

    End Class


End Class