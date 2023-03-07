Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports minidom
Imports minidom.Sistema
Imports minidom.Databases

<Flags>
Public Enum ModuleSupportFlags As Integer
    SNone = 0
    SAnnotations = 1
    SCreate = 2
    SEdit = 4
    SDelete = 8
    SDuplicate = 16
    SImport = 32
    SExport = 64
    SPrint = 256
End Enum

<Serializable>
Public Class ExportableColumnInfo
    Implements XML.IDMDXMLSerializable, IComparable

    Public Key As String
    Public Value As String
    Public Posizione As Integer
    Public TipoValore As TypeCode
    Public Selected As Boolean

    Public Sub New()
        DMDObject.IncreaseCounter(Me)
    End Sub

    Public Sub New(ByVal nome As String, ByVal descrizione As String, ByVal tipoValore As TypeCode, Optional ByVal selected As Boolean = True)
        Me.New
        Me.Key = nome
        Me.Value = descrizione
        Me.TipoValore = tipoValore
        Me.Selected = selected
    End Sub

    Public Overrides Function ToString() As String
        Return Me.Key & "/" & Me.Value
    End Function

    Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
        Select Case fieldName
            Case "Key" : Me.Key = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "Value" : Me.Value = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "Posizione" : Me.Posizione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            Case "TipoValore" : Me.TipoValore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            Case "Selected" : Me.Selected = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
        End Select
    End Sub

    Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
        writer.WriteTag("Key", Me.Key)
        writer.WriteTag("Value", Me.Value)
        writer.WriteTag("Posizione", Me.Posizione)
        writer.WriteTag("TipoValore", Me.TipoValore)
        writer.WriteTag("Selected", Me.Selected)
    End Sub

    Public Function CompareTo(obj As ExportableColumnInfo) As Integer
        Dim ret As Integer = Me.Posizione - obj.Posizione
        If (ret = 0) Then ret = Strings.Compare(Me.Value, obj.Value, CompareMethod.Text)
        Return ret
    End Function

    Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMDObject.DecreaseCounter(Me)
    End Sub


End Class


Public Interface IModuleHandler
    ''' <summary>
    ''' Restituisce il modulo gestito
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property [Module] As CModule

    ''' <summary>
    ''' Imposta il modulo gestito
    ''' </summary>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Sub SetModule(ByVal value As CModule)

    ''' <summary>
    ''' Esegue l'azione specificata
    ''' </summary>
    ''' <param name="actionName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ExecuteAction(ByVal renderer As Object, ByVal actionName As String) As String

    ' ''' <summary>
    ' ''' Esegue l'azione specificata
    ' ''' </summary>
    ' ''' <param name="actionName"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Function ExecuteAction1(ByVal renderer As Object, ByVal actionName As String) As MethodResults


    ''' <summary>
    ''' Crea un cursore per l'accesso agli oggetti gestiti dal modulo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function CreateCursor() As DBObjectCursorBase

    ''' <summary>
    ''' Restituisce vero se l'utente corrente puo eseguire il comando list sul modulo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function CanList() As Boolean

    Function CanCreate(ByVal item As Object) As Boolean

    Function CanEdit(ByVal item As Object) As Boolean

    Function CanDelete(ByVal item As Object) As Boolean

    ''' <summary>
    ''' Restituisce vero se l'utente corrente può eseguire la configurazione del modulo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function CanConfigure() As Boolean

    ''' <summary>
    ''' Restituisce la collezione delle colonne esportabili
    ''' </summary>
    ''' <returns></returns>
    Function GetExportableColumnList() As CCollection(Of ExportableColumnInfo)
End Interface


 
