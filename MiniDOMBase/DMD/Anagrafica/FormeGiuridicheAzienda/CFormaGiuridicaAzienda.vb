Imports minidom
Imports minidom.Sistema
Imports minidom.Databases



Partial Public Class Anagrafica

    <Serializable> _
    Public Class CFormaGiuridicaAzienda
        Inherits DBObject

        Private m_Nome As String

        Public Sub New()
            Me.m_Nome = ""
        End Sub

        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nome
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.FormeGiuridicheAzienda.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_FormeGiuridicheAzienda"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            reader.Read("Nome", Me.m_Nome)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Sub Save(Optional force As Boolean = False)
            MyBase.Save(force)
            Anagrafica.FormeGiuridicheAzienda.UpdateCached(Me)
        End Sub

        Public Overrides Sub Delete(Optional force As Boolean = False)
            MyBase.Delete(force)
            Anagrafica.FormeGiuridicheAzienda.UpdateCached(Me)
        End Sub
    End Class

 


End Class