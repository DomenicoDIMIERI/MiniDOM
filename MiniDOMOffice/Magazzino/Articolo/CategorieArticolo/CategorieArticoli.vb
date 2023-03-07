Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Office
Imports minidom.Internals.Office

Namespace Internals.Office

    <Serializable>
    Public Class CCategorieArticoliClass
        Inherits CModulesClass(Of CategoriaArticolo)

        Public Sub New()
            MyBase.New("modOfficeCategorieArticoli", GetType(CategoriaArticoloCursor), -1)
        End Sub

        Protected Overrides Function CreateModuleInfo() As CModule
            Dim m As CModule = MyBase.CreateModuleInfo()
            m.Parent = minidom.Office.Module
            m.Visible = True
            m.Save()
            Return m
        End Function

        ''' <summary>
        ''' Restituisce la categoria in base al nome
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        Public Function GetItemByName(ByVal value As String) As CategoriaArticolo
            value = Strings.Trim(value)
            If (value = "") Then Return Nothing
            Dim items As CCollection(Of CategoriaArticolo) = Me.LoadAll
            For Each c As CategoriaArticolo In items
                If (Strings.Compare(c.Nome, value, CompareMethod.Text) = 0) Then Return c
            Next
            Return Nothing
        End Function


    End Class


End Namespace

Partial Public Class Office
     
    Private Shared m_CategorieArticoli As CCategorieArticoliClass = Nothing

    ''' <summary>
    ''' Modulo che definisce le categorie degli articoli in magazzino
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property CategorieArticoli As CCategorieArticoliClass
        Get
            If (m_CategorieArticoli Is Nothing) Then m_CategorieArticoli = New CCategorieArticoliClass
            Return m_CategorieArticoli
        End Get
    End Property




   
End Class


