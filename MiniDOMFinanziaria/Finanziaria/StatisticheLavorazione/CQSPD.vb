Imports minidom
Imports minidom.Databases
Imports minidom.Finanziaria
Imports minidom.Sistema
Imports minidom.Anagrafica

Partial Public Class Finanziaria

    <Serializable>
    Public Class CPraticaInfoLavorazione
        Implements minidom.XML.IDMDXMLSerializable

        Public m_IDPratica As Integer
        <NonSerialized> Public m_Pratica As CPraticaCQSPD
        Public IDPersona As Integer
        <NonSerialized> Public Persona As CPersonaFisica
        Public NomePersona As String
        Public Rata As Decimal
        Public Durata As Integer
        Public NettoRicavo As Decimal
        Public NettoAllaMano As Decimal
        Public TAN As Single
        Public TAEG As Single

        Public Sub New()
            Me.m_IDPratica = 0
            Me.m_Pratica = Nothing
            Me.IDPersona = 0
            Me.Persona = Nothing
            Me.NomePersona = ""
            Me.Rata = 0
            Me.Durata = 0
            Me.NettoAllaMano = 0
            Me.NettoRicavo = 0
            Me.TAEG = 0
            Me.TAN = 0
        End Sub

        Public Sub New(ByVal p As CPraticaCQSPD)
            If (p Is Nothing) Then Throw New ArgumentNullException("p")
            Me.m_IDPratica = GetID(p)
            Me.m_Pratica = p
            Me.Persona = p.Cliente
            Me.IDPersona = GetID(Me.Persona)
            Me.NomePersona = Me.Persona.Nominativo
            Me.Rata = p.OffertaCorrente.Rata
            Me.Durata = p.OffertaCorrente.Durata
            Me.NettoRicavo = p.OffertaCorrente.NettoRicavo
            'Me.m_NettoAllaMano = p.OffertaCorrente.NettoRicavo - p.altr
            Me.TAN = p.OffertaCorrente.TAN
            Me.TAEG = p.OffertaCorrente.TAEG
        End Sub

        Public Property IDPratica As Integer
            Get
                Return GetID(Me.m_Pratica, Me.m_IDPratica)
            End Get
            Set(value As Integer)
                If (Me.IDPratica = value) Then Exit Property
                Me.m_IDPratica = value
                Me.m_Pratica = Nothing
            End Set
        End Property

        Public Property Pratica As CPraticaCQSPD
            Get
                If (Me.m_Pratica Is Nothing) Then Me.m_Pratica = Finanziaria.Pratiche.GetItemById(Me.m_IDPratica)
                Return Me.m_Pratica
            End Get
            Set(value As CPraticaCQSPD)
                Me.m_Pratica = value
                Me.m_IDPratica = GetID(value)
            End Set
        End Property



        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IDPratica" : Me.m_IDPratica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDPersona" : Me.IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona" : Me.NomePersona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Rata" : Me.Rata = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Durata" : Me.Durata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NettoRicavo" : Me.NettoRicavo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "NettoAllaMano" : Me.NettoAllaMano = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TAN" : Me.TAN = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TAEG" : Me.TAEG = XML.Utils.Serializer.DeserializeDouble(fieldValue)
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDPratica", Me.IDPratica)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("NomePersona", Me.NomePersona)
            writer.WriteAttribute("Rata", Me.Rata)
            writer.WriteAttribute("Durata", Me.Durata)
            writer.WriteAttribute("NettoRicavo", Me.NettoRicavo)
            writer.WriteAttribute("NettoAllaMano", Me.NettoAllaMano)
            writer.WriteAttribute("TAN", Me.TAN)
            writer.WriteAttribute("TAEG", Me.TAEG)
        End Sub
    End Class


    <Serializable>
    Public Class CQSPDSTXANNSTATITEM
        Implements IComparable, XML.IDMDXMLSerializable

        Public Anno As Integer
        Public Conteggio() As Integer
        Public ML() As Decimal

        Public Sub New()
            Me.Anno = 0
            ReDim Me.Conteggio(12)
            ReDim Me.ML(12)
        End Sub

        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim tmp As CQSPDSTXANNSTATITEM = obj
            Dim ret As Integer = Me.Anno - tmp.Anno
            Return ret
        End Function

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Anno" : Me.Anno = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Conteggio" : Me.Conteggio = XML.Utils.Serializer.ToArray(Of Integer)(fieldValue)
                Case "ML" : Me.ML = XML.Utils.Serializer.ToArray(Of Integer)(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Anno", Me.Anno)
            writer.WriteTag("Conteggio", Me.Conteggio)
            writer.WriteTag("ML", Me.ML)
        End Sub
    End Class


#Region "Statistiche per prodotto"

    Public Enum InfoStatoEnum As Integer
        CONTATTO = 0
        LIQUIDATO = 1
        ANNULLATO = 2
        ALTRO = 3
    End Enum

    Public Class InfoStato
        Implements XML.IDMDXMLSerializable

        Public Descrizione As String
        Public stato As InfoStatoEnum
        Public Conteggio As Integer
        Public Montante As Decimal

        Public Sub New(ByVal descrizione As String, ByVal stato As InfoStatoEnum, ByVal conteggio As Integer, ByVal montante As Decimal)
            DMDObject.IncreaseCounter(Me)
            Me.Descrizione = descrizione
            Me.stato = stato
            Me.Conteggio = conteggio
            Me.Montante = montante
        End Sub

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Descrizione" : Me.Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Stato" : Me.stato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Conteggio" : Me.Conteggio = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Montante" : Me.Montante = XML.Utils.Serializer.DeserializeDouble(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Descrizione", Me.Descrizione)
            writer.WriteAttribute("Stato", Me.stato)
            writer.WriteAttribute("Conteggio", Me.Conteggio)
            writer.WriteAttribute("Montante", Me.Montante)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

    Public Class CompareByAnnullato
        Implements IComparer, IComparer(Of CRigaStatisticaPerStato)

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(x As CRigaStatisticaPerStato, y As CRigaStatisticaPerStato) As Integer Implements IComparer(Of CRigaStatisticaPerStato).Compare
            Dim v1 As Decimal = x.Item(InfoStatoEnum.ANNULLATO).Montante
            Dim v2 As Decimal = y.Item(InfoStatoEnum.ANNULLATO).Montante
            If (v1 < v2) Then Return 1
            If (v1 > v2) Then Return -1
            Return 0
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function
    End Class

    Public Class CompareByContatto
        Implements IComparer, IComparer(Of CRigaStatisticaPerStato)

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(x As CRigaStatisticaPerStato, y As CRigaStatisticaPerStato) As Integer Implements IComparer(Of CRigaStatisticaPerStato).Compare
            Dim v1 As Decimal = x.Item(InfoStatoEnum.CONTATTO).Montante
            Dim v2 As Decimal = y.Item(InfoStatoEnum.CONTATTO).Montante
            If (v1 < v2) Then Return 1
            If (v1 > v2) Then Return -1
            Return 0
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function
    End Class

    Public Class CompareByLiquidato
        Implements IComparer, IComparer(Of CRigaStatisticaPerStato)

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(x As CRigaStatisticaPerStato, y As CRigaStatisticaPerStato) As Integer Implements IComparer(Of CRigaStatisticaPerStato).Compare
            Dim v1 As Decimal = x.Item(InfoStatoEnum.LIQUIDATO).Montante
            Dim v2 As Decimal = y.Item(InfoStatoEnum.LIQUIDATO).Montante
            If (v1 < v2) Then Return 1
            If (v1 > v2) Then Return -1
            Return 0
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function
    End Class

    Public Class CompareByAltriStati
        Implements IComparer, IComparer(Of CRigaStatisticaPerStato)

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(x As CRigaStatisticaPerStato, y As CRigaStatisticaPerStato) As Integer Implements IComparer(Of CRigaStatisticaPerStato).Compare
            Dim v1 As Decimal = x.Item(InfoStatoEnum.ALTRO).Montante
            Dim v2 As Decimal = y.Item(InfoStatoEnum.ALTRO).Montante
            If (v1 < v2) Then Return 1
            If (v1 > v2) Then Return -1
            Return 0
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function
    End Class

    Public Class CompareByKey
        Implements IComparer, IComparer(Of CRigaStatisticaPerStato)

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(x As CRigaStatisticaPerStato, y As CRigaStatisticaPerStato) As Integer Implements IComparer(Of CRigaStatisticaPerStato).Compare
            Return Strings.Compare(x.Descrizione, y.Descrizione, CompareMethod.Text)
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function
    End Class

    <Serializable>
    Public Class CRigaStatisticaPerStato
        Implements IComparable, XML.IDMDXMLSerializable

        Public Descrizione As String = ""
        Public m_Items() As InfoStato
        Public Tag As String
        Public Tag1 As Integer

        Public Sub New()
            DMDObject.IncreaseCounter(Me)
            ReDim Me.m_Items(3)
            Me.m_Items(0) = New InfoStato("Contatto", InfoStatoEnum.CONTATTO, 0, 0)
            Me.m_Items(1) = New InfoStato("Liquidato", InfoStatoEnum.LIQUIDATO, 0, 0)
            Me.m_Items(2) = New InfoStato("Annullato", InfoStatoEnum.ANNULLATO, 0, 0)
            Me.m_Items(3) = New InfoStato("Altri Stati", InfoStatoEnum.ALTRO, 0, 0)
        End Sub

        Public ReadOnly Property Item(ByVal stato As InfoStatoEnum) As InfoStato
            Get
                Select Case stato
                    Case InfoStatoEnum.ALTRO : Return Me.m_Items(3)
                    Case InfoStatoEnum.ANNULLATO : Return Me.m_Items(2)
                    Case InfoStatoEnum.LIQUIDATO : Return Me.m_Items(1)
                    Case Else : Return Me.m_Items(0)
                End Select
            End Get
        End Property

        Public Function CompareTo(ByVal b As CRigaStatisticaPerStato) As Integer
            Return Strings.Compare(Me.Descrizione, b.Descrizione, CompareMethod.Text)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Descrizione" : Me.Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Tag" : Me.Tag = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Tag1" : Me.Tag1 = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Items" : Me.m_Items = XML.Utils.Serializer.ToArray(Of InfoStato)(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Descrizione", Me.Descrizione)
            writer.WriteAttribute("Tag", Me.Tag)
            writer.WriteAttribute("Tag1", Me.Tag1)
            writer.WriteTag("Items", Me.m_Items)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

    <Serializable>
    Public Class CRigaStatisticaPerStatoCollection
        Inherits CKeyCollection(Of CRigaStatisticaPerStato)

        Public Sub New()
        End Sub

        Public Shadows Function Add(ByVal descrizione As String) As CRigaStatisticaPerStato
            Dim item As New CRigaStatisticaPerStato
            item.Descrizione = descrizione
            MyBase.Add("" & descrizione, item)
            'MyBase.Sort()
            Return item
        End Function

    End Class

#End Region



End Class

