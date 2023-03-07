Imports minidom
Imports minidom.Sistema
Imports minidom.Databases

Partial Public Class Anagrafica

    ''' <summary>
    ''' Filtro utilizzato nell'interfaccia di ricerca generica
    ''' </summary>
    <Serializable>
    Public Class CRMFindParams
        Implements XML.IDMDXMLSerializable, ICloneable

        ''' <summary>
        ''' Tipo della ricerca
        ''' </summary>
        Public Tipo As String

        ''' <summary>
        ''' Testo della ricerca
        ''' </summary>
        Public Text As String

        ''' <summary>
        ''' Numero massimo di elementi da includere nella ricerca (se NULL vengono inclusi tutti gli elementi)
        ''' </summary>
        Public nMax As Integer?

        ''' <summary>
        ''' Se true il sistema usa la ricerca "intelligente" altrimenti la stringa viene interpretata senza alcuna elaborazione
        ''' </summary>
        Public IntelliSearch As Boolean

        ''' <summary>
        ''' Flag di ricerca
        ''' </summary>
        Public flags As PFlags?

        ''' <summary>
        ''' Se vero il sistema non considera le limitazioni imposte dagli amministratori
        ''' </summary>
        Public ignoreRights As Boolean

        ''' <summary>
        ''' Se valorizzato restringe la ricerca alle sole persone fisiche o alle sole persone giuridiche
        ''' </summary>
        Public tipoPersona As TipoPersona?

        ''' <summary>
        ''' Se diverso da zero restringe la ricerca al solo punto operativo specificato
        ''' </summary>
        Public IDPuntoOperativo As Integer

        ''' <summary>
        ''' Se valorizzato restringe la ricerca ai soli elementi nello stato specificato
        ''' </summary>
        Public DettaglioEsito As String

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
            Me.Tipo = ""
            Me.Text = ""
            Me.nMax = Nothing
            Me.IntelliSearch = True
            Me.flags = Nothing
            Me.ignoreRights = False
            Me.tipoPersona = Nothing
            Me.IDPuntoOperativo = 0
            Me.DettaglioEsito = ""
        End Sub

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Tipo" : Me.Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "text" : Me.Text = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "nMax" : Me.nMax = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IntelliSearch" : Me.IntelliSearch = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "flags" : Me.flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ignoreRights" : Me.ignoreRights = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "tipoPersona" : Me.tipoPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDPuntoOperativo" : Me.IDPuntoOperativo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DettaglioEsito" : Me.DettaglioEsito = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Tipo", Me.Tipo)
            writer.WriteAttribute("text", Me.Text)
            writer.WriteAttribute("nMax", Me.nMax)
            writer.WriteAttribute("IntelliSearch", Me.IntelliSearch)
            writer.WriteAttribute("flags", Me.flags)
            writer.WriteAttribute("ignoreRights", Me.ignoreRights)
            writer.WriteAttribute("tipoPersona", Me.tipoPersona)
            writer.WriteAttribute("IDPuntoOperativo", Me.IDPuntoOperativo)
            writer.WriteAttribute("DettaglioEsito", Me.DettaglioEsito)
        End Sub

        Public Function Clone() As Object Implements ICloneable.Clone
            Return Me.MemberwiseClone
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub
    End Class



End Class