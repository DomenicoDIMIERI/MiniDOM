Imports minidom.Databases
Imports minidom.Sistema

Partial Public Class Finanziaria


    ''' <summary>
    ''' Calcola il TAEG
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CTAEGCalculator
        Private m_Durata As Integer
        Private m_Rata As Double
        Private m_NettoRicavo As Double
        Private m_TAEG As Double
        Private m_Calculated As Boolean

        Public Sub New()
            Me.New(120, 0, 0)
        End Sub

        Public Sub New(ByVal durata As Integer)
            Me.New(durata, 0, 0)
        End Sub

        Public Sub New(ByVal durata As Integer, ByVal rata As Double)
            Me.New(durata, rata, 0)
        End Sub

        Public Sub New(ByVal durata As Integer, ByVal rata As Double, ByVal nettoRicavo As Double)
            Me.m_Durata = durata
            Me.m_Rata = rata
            Me.m_NettoRicavo = nettoRicavo
            Me.m_TAEG = 0
            Me.m_Calculated = False
        End Sub

        Public Sub Invalidate()
            Me.m_Calculated = False
        End Sub


        Public Property Durata As Integer
            Get
                Return Me.m_Durata
            End Get
            Set(value As Integer)
                If (Me.m_Durata = value) Then Return
                Me.m_Durata = value
                Me.Invalidate
            End Set
        End Property

        Public Property Rata As Double
            Get
                Return Me.m_Rata
            End Get
            Set(value As Double)
                If (Me.m_Rata = value) Then Return
                Me.m_Rata = value
                Me.Invalidate()
            End Set
        End Property

        Public Property NettoRicavo As Double
            Get
                Return Me.m_NettoRicavo
            End Get
            Set(value As Double)
                If (Me.m_NettoRicavo = value) Then Return
                Me.m_NettoRicavo = value
                Me.Invalidate()
            End Set
        End Property

        Public ReadOnly Property TAEG As Double
            Get
                Me.Calc()
                Return Me.m_TAEG
            End Get
        End Property



        Public Function Calc() As Double
            If (Me.m_Calculated) Then Return Me.m_TAEG
            Dim fEval As New CTAEGFunEvaluator
            Dim errCode As Integer = 0
            fEval.Durata = Me.Durata
            fEval.NettoRicavo = Me.NettoRicavo
            fEval.Quota = Me.Rata
            Me.m_TAEG = Math.FindZero(fEval, Math.TOLMIN, 1, 0.0000000001) * 100
            Me.m_Calculated = True
            Return Me.m_TAEG
        End Function

    End Class


End Class