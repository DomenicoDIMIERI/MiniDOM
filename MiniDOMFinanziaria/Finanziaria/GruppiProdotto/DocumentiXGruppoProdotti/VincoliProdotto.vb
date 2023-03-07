Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Office
Imports minidom.Finanziaria
Imports minidom.Internals

Namespace Internals


    ''' <summary>
    ''' Rappresenta un documento caricabile per un prodotto
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CVincoliProdottiClass
        Inherits CModulesClass(Of CDocumentoXGruppoProdotti)

        Public Sub New()
            MyBase.New("modDocumentiXGruppoProdotti", GetType(CDocumentiXGruppoProdottiCursor), -1)
        End Sub



    End Class

End Namespace

Partial Public Class Finanziaria
   
    Private Shared m_VincoliProdotto As CVincoliProdottiClass = Nothing

    Public Shared ReadOnly Property VincoliProdotto As CVincoliProdottiClass
        Get
            If (m_VincoliProdotto Is Nothing) Then m_VincoliProdotto = New CVincoliProdottiClass
            Return m_VincoliProdotto
        End Get
    End Property
  

End Class