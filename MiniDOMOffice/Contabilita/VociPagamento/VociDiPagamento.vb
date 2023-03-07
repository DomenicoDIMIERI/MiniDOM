﻿Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Internals
Imports minidom.Office

Namespace Internals

    Public Class VociDiPagamentoClass
        Inherits CModulesClass(Of VoceDiPagamento)

        Public Sub New()
            MyBase.New("modOfficeVociDiPagamento", GetType(VoceDiPagamentoCursor), 0)
        End Sub

    End Class


End Namespace

Partial Class Office

    Private Shared m_VociDiPagamento As VociDiPagamentoClass = Nothing

    Public Shared ReadOnly Property VociDiPagamento As VociDiPagamentoClass
        Get
            If m_VociDiPagamento Is Nothing Then m_VociDiPagamento = New VociDiPagamentoClass
            Return m_VociDiPagamento
        End Get
    End Property


End Class