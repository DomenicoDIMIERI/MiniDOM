Imports minidom
Imports minidom.Forms
Imports minidom.Databases
Imports minidom.WebSite

Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica
Imports minidom.Web
Imports minidom.XML

Namespace Forms

    Public Class TabellaProdottoRow
        Implements minidom.XML.IDMDXMLSerializable

        Public rows As ProdottoRow()
        Public TabelleFinanziarie As CKeyCollection(Of CTabellaFinanziaria)
        Public TabelleAssicurative As CKeyCollection(Of CTabellaAssicurativa)

        Public Sub New()
            Me.rows = Nothing
            Me.TabelleFinanziarie = New CKeyCollection(Of CTabellaFinanziaria)
            Me.TabelleAssicurative = New CKeyCollection(Of CTabellaAssicurativa)
        End Sub

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "row" : Me.rows = Arrays.Convert(Of ProdottoRow)(fieldValue)
                Case "TabelleFinanziarie"
                    Me.TabelleFinanziarie.Clear()
                    If (TypeOf (fieldValue) Is CKeyCollection) Then
                        With DirectCast(fieldValue, CKeyCollection)
                            For Each k As String In .Keys
                                Me.TabelleFinanziarie.Add(k, .Item(k))
                            Next
                        End With
                    End If
                Case "TabelleAssicurative"
                    Me.TabelleAssicurative.Clear()
                    If (TypeOf (fieldValue) Is CKeyCollection) Then
                        With DirectCast(fieldValue, CKeyCollection)
                            For Each k As String In .Keys
                                Me.TabelleAssicurative.Add(k, .Item(k))
                            Next
                        End With
                    End If
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            Me.TabelleFinanziarie.Clear()
            Me.TabelleAssicurative.Clear()
            If (Me.rows IsNot Nothing) Then
                For Each row As ProdottoRow In Me.rows
                    If row.TabellaFinanziaria IsNot Nothing Then
                        If (Not Me.TabelleFinanziarie.ContainsKey("T" & row.TabellaFinanziaria.IDTabella)) Then
                            Me.TabelleFinanziarie.Add("T" & row.TabellaFinanziaria.IDTabella, row.TabellaFinanziaria.Tabella)
                        End If
                    End If
                    If row.TabellaAssicurativa IsNot Nothing Then
                        If (Not Me.TabelleAssicurative.ContainsKey("T" & row.TabellaAssicurativa.IDRischioVita)) Then
                            Me.TabelleAssicurative.Add("T" & row.TabellaAssicurativa.IDRischioVita, row.TabellaAssicurativa.RischioVita)
                        End If
                        If (Not Me.TabelleAssicurative.ContainsKey("T" & row.TabellaAssicurativa.IDRischioImpiego)) Then
                            Me.TabelleAssicurative.Add("T" & row.TabellaAssicurativa.IDRischioImpiego, row.TabellaAssicurativa.RischioImpiego)
                        End If
                        If (Not Me.TabelleAssicurative.ContainsKey("T" & row.TabellaAssicurativa.IDRischioCredito)) Then
                            Me.TabelleAssicurative.Add("T" & row.TabellaAssicurativa.IDRischioCredito, row.TabellaAssicurativa.RischioCredito)
                        End If
                    End If
                Next
            End If
            writer.WriteTag("TabelleFinanziarie", Me.TabelleFinanziarie)
            writer.WriteTag("TabelleAssicurative", Me.TabelleAssicurative)
            writer.WriteTag("rows", Me.rows)
        End Sub
    End Class

    Public Class ProdottoRow
        Implements minidom.XML.IDMDXMLSerializable

        Public Index As Integer
        Public IDProdotto As Integer
        Public IDGruppo As Integer
        Public TabellaFinanziaria As CProdottoXTabellaFin
        Public TabellaAssicurativa As CProdottoXTabellaAss

        Public Sub New()
            Me.Index = 0
            Me.IDProdotto = 0
            Me.IDGruppo = 0
            Me.TabellaFinanziaria = Nothing
            Me.TabellaAssicurativa = Nothing
        End Sub

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Index" : Me.Index = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDProdotto" : Me.IDProdotto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDGruppo" : Me.IDGruppo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TabellaFinanziaria" : Me.TabellaFinanziaria = XML.Utils.Serializer.ToObject(fieldValue)
                Case "TabellaAssicurativa" : Me.TabellaAssicurativa = XML.Utils.Serializer.ToObject(fieldValue)
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Index", Me.Index)
            writer.WriteAttribute("IDGruppo", Me.IDGruppo)
            writer.WriteTag("IDProdotto", Me.IDProdotto)
            writer.WriteTag("TabellaFinanziaria", Me.TabellaFinanziaria)
            writer.WriteTag("TabellaAssicurativa", Me.TabellaAssicurativa)
        End Sub
    End Class

    Public Class ProdottoRowFilter
        Implements minidom.XML.IDMDXMLSerializable

        Public IDCessionario As Integer
        Public IDGruppo As Integer
        Public IDProfilo As Integer
        Public IDProdotto As Integer

        Public Sub New()
            Me.IDCessionario = 0
            Me.IDGruppo = 0
            Me.IDProfilo = 0
            Me.IDProdotto = 0
        End Sub

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IDCessionario" : Me.IDCessionario = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDGruppo" : Me.IDGruppo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDProfilo" : Me.IDProfilo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDProdotto" : Me.IDProdotto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDCessionario", Me.IDCessionario)
            writer.WriteAttribute("IDGruppo", Me.IDGruppo)
            writer.WriteAttribute("IDProfilo", Me.IDProfilo)
            writer.WriteAttribute("IDProdotto", Me.IDProdotto)
        End Sub
    End Class

    Public Class CProdottiModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CProdottiCursor
        End Function


    End Class


End Namespace