Imports minidom.Databases
Imports minidom.Sistema
Imports minidom.Finanziaria
Imports minidom.Internals

Namespace Internals

    ''' <summary>
    ''' Gestione degli istituti cessionari
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public NotInheritable Class CCessionariClass
        Inherits CModulesClass(Of CCQSPDCessionarioClass)


        <NonSerialized> Private _default As CCQSPDCessionarioClass = Nothing

        Friend Sub New()
            MyBase.New("modCQSPDCessionari", GetType(CCessionariCursor), -1)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta un riferimento al cessionario predefinito
        ''' </summary>
        ''' <returns></returns>
        Public Property [Default] As CCQSPDCessionarioClass
            Get
                If (Me._default Is Nothing) Then
                    Dim idDefaultCQS As Integer = Sistema.ApplicationContext.Settings.GetValueInt("CQSPD.Cessionari.DefaultCessionarioID", 0)
                    Me._default = Me.GetItemById(idDefaultCQS)
                End If
                If (Me._default Is Nothing OrElse Me._default.Stato <> ObjectStatus.OBJECT_VALID OrElse Not Me._default.IsValid()) Then
                    Dim items = Me.LoadAll()
                    For Each c As CCQSPDCessionarioClass In items
                        If (c.UsabileInPratiche AndAlso c.IsValid()) Then
                            Me._default = c
                            Exit For
                        End If
                    Next
                End If
                Return Me._default
            End Get
            Set(value As CCQSPDCessionarioClass)
                If (Me._default Is value) Then Return
                Me._default = value
            End Set
        End Property


        ''' <summary>
        ''' Restituisce il cessionario in base al suo nome. 
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByName(ByVal value As String) As CCQSPDCessionarioClass
            value = Trim(value)
            If (value = vbNullString) Then Return Nothing
            For Each ret As CCQSPDCessionarioClass In Me.LoadAll
                If (Strings.Compare(value, ret.Nome, CompareMethod.Text) = 0) Then Return ret
            Next
            Return Nothing
        End Function



        ''' <summary>
        ''' Restituisce un array contenente l'elenco di tutti i cessionari attivi ed inattivi
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetArrayCessionari() As CCQSPDCessionarioClass()
            Return GetAllCessionari.ToArray
        End Function

        ''' <summary>
        ''' Restituisce un array base 0 contenente tutti gli oggetti CCessionario validi
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetArrayCessionariValidi() As CCQSPDCessionarioClass()
            Dim ret As New CCollection(Of CCQSPDCessionarioClass)
            For Each c As CCQSPDCessionarioClass In Me.LoadAll
                If (c.IsValid) Then ret.Add(c)
            Next
            Return ret.ToArray
        End Function

        Public Function GetAllCessionari() As CCollection(Of CCQSPDCessionarioClass)
            Return New CCollection(Of CCQSPDCessionarioClass)(Me.LoadAll)
        End Function


        Public Overrides Sub UpdateCached(item As CCQSPDCessionarioClass)
            If (Me._default IsNot Nothing AndAlso DBUtils.GetID(Me._default) = DBUtils.GetID(item)) Then
                Me._default = item
            End If
            MyBase.UpdateCached(item)
        End Sub

    End Class

End Namespace

Partial Public Class Finanziaria



    Private Shared m_Cessionari As CCessionariClass = Nothing

    Public Shared ReadOnly Property Cessionari As CCessionariClass
        Get
            If (m_Cessionari Is Nothing) Then m_Cessionari = New CCessionariClass
            Return m_Cessionari
        End Get
    End Property

End Class