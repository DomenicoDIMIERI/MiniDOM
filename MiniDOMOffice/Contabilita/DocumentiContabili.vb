Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Internals
Imports minidom.Office

Namespace Internals

    <Serializable>
    Public Class DocumentiContabiliClass
        Inherits CModulesClass(Of DocumentoContabile)

        Public Sub New()
            MyBase.New("modOfficeDocumentiContabili", GetType(DocumentoContabileCursor), 0)
        End Sub

    End Class


End Namespace

Partial Class Office

    Private Shared m_DocumentiContabili As DocumentiContabiliClass = Nothing

    Public Shared ReadOnly Property DocumentiContabili As DocumentiContabiliClass
        Get
            If m_DocumentiContabili Is Nothing Then m_DocumentiContabili = New DocumentiContabiliClass
            Return m_DocumentiContabili
        End Get
    End Property


End Class