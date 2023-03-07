Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Office
Imports minidom.Internals.Office

Namespace Internals.Office

    Public Class CArticoliClass
        Inherits CModulesClass(Of Articolo)

        Public Sub New()
            MyBase.New("modOfficeArticoli", GetType(ArticoloCursor), 0)
        End Sub

        Protected Overrides Function CreateModuleInfo() As CModule
            Dim m As CModule = MyBase.CreateModuleInfo()
            m.Parent = minidom.Office.Module
            m.Visible = True
            m.Save()
            Return m
        End Function

        Public Function Find(ByVal text As String) As CCollection(Of Articolo)
            Dim ret As New CCollection(Of Articolo)

            text = Trim(text)
            If (text = "") Then Return ret

            Using cursor As New ArticoloCursor
                cursor.ValoreCodice.Value = Strings.JoinW(text, "%")
                cursor.ValoreCodice.Operator = OP.OP_LIKE
                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            End Using


            If (ret.Count = 0) Then
                Using cursor As New ArticoloCursor
                    cursor.Clear()
                    cursor.Nome.Value = Strings.JoinW("%", text, "%")
                    cursor.Nome.Operator = OP.OP_LIKE
                    While Not cursor.EOF
                        ret.Add(cursor.Item)
                        cursor.MoveNext()
                    End While
                End Using
            End If
            Return ret
        End Function


    End Class


End Namespace

Partial Public Class Office
     
    Private Shared m_Articoli As CArticoliClass = Nothing

    Public Shared ReadOnly Property Articoli As CArticoliClass
        Get
            If (m_Articoli Is Nothing) Then m_Articoli = New CArticoliClass
            Return m_Articoli
        End Get
    End Property




   
End Class


