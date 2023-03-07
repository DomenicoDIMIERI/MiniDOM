Imports minidom.Databases
Imports minidom.Sistema

Partial Public Class Finanziaria

    Public Class CTANFunEvaluator
        Inherits FunEvaluator

        Private m_Durata As Integer
        Private m_Rata As Double
        Private m_TAN As Double

        Public Sub New()
            Me.m_Durata = 120
            Me.m_Rata = 0.0
            Me.m_TAN = 0.0
        End Sub

        Public Property Durata As Integer
            Get
                Return Me.m_Durata
            End Get
            Set(value As Integer)
                Me.m_Durata = value
            End Set
        End Property

        Public Property Rata As Double
            Get
                Return Me.m_Rata
            End Get
            Set(value As Double)
                Me.m_Rata = value
            End Set
        End Property

        Public Property TAN As Double
            Get
                Return Me.m_TAN
            End Get
            Set(value As Double)
                Me.m_TAN = value
            End Set
        End Property

        Public Overrides Function EvalFunction(x As Double) As Double
            Dim c As New CTANCalculator
            c.Rata = Me.m_Rata
            c.Durata = Me.m_Durata
            c.Importo = x
            Return (c.Calc() - Me.m_TAN)
        End Function


    End Class


End Class