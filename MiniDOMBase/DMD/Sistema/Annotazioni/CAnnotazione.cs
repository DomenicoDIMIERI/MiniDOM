using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Rappresenta una nota collegata ad un oggetto del db
        /// </summary>
        [Serializable]
        public class CAnnotazione 
            : Databases.DBObject
        {
            private int m_OwnerID; // ID della persona associata
            private string m_OwnerType;
            [NonSerialized] private object m_Owner;
            private string m_Valore;
            private int m_IDContesto;
            private string m_TipoContesto;
            private string m_SourceName;
            private string m_SourceParam;
            // Private m_IDProduttore As Integer
            // Private m_Produttore As CAzienda

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAnnotazione()
            {
                m_OwnerID = 0;
                m_OwnerType = "";
                m_Owner = null;
                m_Valore = "";
                m_IDContesto = 0;
                m_TipoContesto = DMD.Strings.vbNullString;
                m_SourceName = DMD.Strings.vbNullString;
                m_SourceParam = DMD.Strings.vbNullString;
                // Me.m_IDProduttore = 0
                // Me.m_Produttore = Nothing
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            public CAnnotazione(object owner) : this()
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                m_Owner = owner;
                m_OwnerType = DMD.RunTime.vbTypeName(owner);
                m_OwnerID = DBUtils.GetID(owner, 0);
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="context"></param>
            public CAnnotazione(object owner, object context) : this(owner)
            {
                if (context is null)
                    throw new ArgumentNullException("context");
                m_IDContesto = DBUtils.GetID(context, 0);
                m_TipoContesto = DMD.RunTime.vbTypeName(context);
            }

          

            // Public Property IDProduttore As Integer
            // Get
            // Return GetID(Me.m_Produttore, Me.m_IDProduttore)
            // End Get
            // Set(value As Integer)
            // Dim oldValue As Integer = Me.IDProduttore
            // If (oldValue = value) Then Exit Property
            // Me.m_IDProduttore = value
            // Me.m_Produttore = Nothing
            // Me.DoChanged("IDProduttore", value, oldValue)
            // End Set
            // End Property

            // Public Property Produttore As CAzienda
            // Get
            // If (Me.m_Produttore Is Nothing) Then Me.m_Produttore = Anagrafica.Aziende.GetItemById(Me.m_IDProduttore)
            // Return Me.m_Produttore
            // End Get
            // Set(value As CAzienda)
            // Dim oldValue As CAzienda = Me.m_Produttore
            // If (oldValue Is value) Then Exit Property
            // Me.m_Produttore = value
            // Me.m_IDProduttore = GetID(value)
            // Me.DoChanged("Produttore", value, oldValue)
            // End Set
            // End Property

            /// <summary>
            /// Restituisce o imposta il nome della sorgente
            /// </summary>
            public string SourceName
            {
                get
                {
                    return m_SourceName;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_SourceName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SourceName = value;
                    DoChanged("SourceName", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il parametro che identifica la sorgente dell'annotazione
            /// </summary>
            public string SourceParam
            {
                get
                {
                    return m_SourceParam;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_SourceParam;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SourceParam = value;
                    DoChanged("SourceParam", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id dell'oggetto a cui appartiene l'annotazione
            /// </summary>
            public int OwnerID
            {
                get
                {
                    return DBUtils.GetID(m_Owner, m_OwnerID);
                }
                set
                {
                    var oldValue = this.OwnerID;
                    if (oldValue == value) return;
                    this.m_OwnerID = value;
                    this.m_Owner = null;
                    this.DoChanged("OwnerID", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo del possessore
            /// </summary>
            public string OwnerType
            {
                get
                {
                    return this.m_OwnerType;
                }
                set
                {
                    var oldValue = this.m_OwnerType;
                    value = Strings.Trim(value);
                    if (DMD.Strings.EQ(value, oldValue)) return;
                    this.m_OwnerType = value;
                    this.m_Owner = null;
                    this.DoChanged("OwnerType", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il possessore
            /// </summary>
            public object Owner
            {
                get
                {
                    return m_Owner;
                }
                set
                {
                    var oldValue = this.m_Owner;
                    if (oldValue == value) return;
                    this.m_Owner = value;
                    this.m_OwnerID = DBUtils.GetID(value, 0);
                    this.m_OwnerType = DMD.RunTime.vbTypeName(value);
                    this.DoChanged("Owner", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il possessore
            /// </summary>
            /// <param name="value"></param>
            public void SetOwner(object value)
            {
                m_Owner = value;
                m_OwnerID = DBUtils.GetID(value, 0);
                m_OwnerType = DMD.RunTime.vbTypeName(value);
                DoChanged("Owner");
            }

            /// <summary>
            /// Restituisce o imposta il contenuto della nota
            /// </summary>
            public string Valore
            {
                get
                {
                    return m_Valore;
                }

                set
                {
                    string oldValue = m_Valore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Valore = value;
                    DoChanged("Valore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'id del contesto
            /// </summary>
            public int IDContesto
            {
                get
                {
                    return m_IDContesto;
                }

                set
                {
                    int oldValue = m_IDContesto;
                    if (oldValue == value)
                        return;
                    m_IDContesto = value;
                    DoChanged("IDContesto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il tipo del contesto
            /// </summary>
            public string TipoContesto
            {
                get
                {
                    return m_TipoContesto;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_TipoContesto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_TipoContesto = value;
                    DoChanged("TipoContesto", value, oldValue);
                }
            }

            /// <summary>
            /// Imposta il contesto
            /// </summary>
            /// <param name="value"></param>
            internal void SetContesto(object value)
            {
                m_IDContesto = DBUtils.GetID(value, 0);
                m_TipoContesto = DMD.RunTime.vbTypeName(value);
            }

            /// <summary>
            /// Discriminante nel repository
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_Annotazioni";
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Sistema.Annotazioni; //.Module;
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Annotazioni.Database;
            //}

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>

            protected override bool LoadFromRecordset(DBReader reader)
            {
                if (m_Owner is null)
                {
                    m_OwnerID = reader.Read("OwnerID", m_OwnerID);
                    m_OwnerType = reader.Read("OwnerType", m_OwnerType);
                }

                m_Valore = reader.Read("Valore",  m_Valore);
                m_IDContesto = reader.Read("IDContesto",  m_IDContesto);
                m_TipoContesto = reader.Read("TipoContesto",  m_TipoContesto);
                m_SourceName = reader.Read("SourceName",  m_SourceName);
                m_SourceParam = reader.Read("SourceParam",  m_SourceParam);
                // Me.m_IDProduttore = reader.Read("IDProduttore", Me.m_IDProduttore)
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("OwnerID", OwnerID);
                writer.Write("OwnerType", m_OwnerType);
                writer.Write("Valore", m_Valore);
                writer.Write("IDContesto", m_IDContesto);
                writer.Write("TipoContesto", m_TipoContesto);
                writer.Write("SourceName", m_SourceName);
                writer.Write("SourceParam", m_SourceParam);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i campi
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("OwnerID", typeof(int), 1);
                c = table.Fields.Ensure("OwnerType", typeof(string), 255);
                c = table.Fields.Ensure("Valore", typeof(string), 0);
                c = table.Fields.Ensure("IDContesto", typeof(int), 1);
                c = table.Fields.Ensure("TipoContesto", typeof(string), 255);
                c = table.Fields.Ensure("SourceName", typeof(string), 255);
                c = table.Fields.Ensure("SourceParam", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxOwner", new string[] { "OwnerType", "OwnerID" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxContext", new string[] { "TipoContesto", "IDContesto" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSource", new string[] { "SourceName", "SourceParam" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxValore", new string[] { "Valore" }, DBFieldConstraintFlags.None);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_Valore;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_OwnerType, this.m_OwnerID, this.m_Valore);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is CAnnotazione) && this.Equals((CAnnotazione)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CAnnotazione obj)
            {
                return base.Equals(obj)
                     && DMD.Integers.EQ(this.OwnerID, obj.OwnerID)
                     && DMD.Strings.EQ(this.m_OwnerType, obj.m_OwnerType)
                    && DMD.Strings.EQ(this.m_Valore, obj.m_Valore)
                    && DMD.Integers.EQ(this.IDContesto, obj.IDContesto)
                    && DMD.Strings.EQ(this.m_TipoContesto, obj.m_TipoContesto)
                    && DMD.Strings.EQ(this.m_SourceName, obj.m_SourceName)
                    && DMD.Strings.EQ(this.m_SourceParam, obj.m_SourceParam)
                    ;
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("OwnerID", OwnerID);
                writer.WriteAttribute("OwnerType", m_OwnerType);
                writer.WriteAttribute("IDContesto", m_IDContesto);
                writer.WriteAttribute("TipoContesto", m_TipoContesto);
                writer.WriteAttribute("SourceName", m_SourceName);
                writer.WriteAttribute("SourceParam", m_SourceParam);
                // writer.WriteAttribute("IDProduttore", Me.IDProduttore)
                base.XMLSerialize(writer);
                writer.WriteTag("Valore", m_Valore);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "OwnerID":
                        {
                            m_OwnerID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "OwnerType":
                        {
                            m_OwnerType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Valore":
                        {
                            m_Valore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDContesto":
                        {
                            m_IDContesto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "TipoContesto":
                        {
                            m_TipoContesto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceName":
                        {
                            m_SourceName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SourceParam":
                        {
                            m_SourceParam = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }
                    
                    default:
                        {
                            // Case "IDProduttore" : Me.m_IDProduttore = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue)
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            
        }


        // Public MustInherit Class DBObjectExt
        // Inherits DBObject

        // Private m_Annotazioni As CAnnotazioni
        // Private m_Attachments As CAttachmentsCollection

        // Public Sub New()
        // Me.m_Annotazioni = Nothing
        // Me.m_Attachments = Nothing
        // End Sub

        // Public Overridable ReadOnly Property Annotazioni As CAnnotazioni
        // Get
        // If Me.m_Annotazioni Is Nothing Then
        // Me.m_Annotazioni = New CAnnotazioni(Me)
        // End If
        // Return Me.m_Annotazioni
        // End Get
        // End Property

        // Public Overridable ReadOnly Property Attachments As CAttachmentsCollection
        // Get
        // If Me.m_Attachments Is Nothing Then
        // Me.m_Attachments = New CAttachmentsCollection(Me)
        // End If
        // Return Me.m_Attachments
        // End Get
        // End Property

        // Public Overrides Function IsChanged() As Boolean
        // Dim ret As Boolean = MyBase.IsChanged()
        // If (ret = False AndAlso Me.m_Annotazioni IsNot Nothing) Then ret = DBUtils.IsChanged(Me.m_Annotazioni)
        // If (ret = False AndAlso Me.m_Attachments IsNot Nothing) Then ret = DBUtils.IsChanged(Me.m_Attachments)
        // Return ret
        // End Function

        // Protected Overrides Function SaveToDatabase(dbConn As CDBConnection) As Boolean
        // Dim ret As Boolean
        // ret = MyBase.SaveToDatabase(dbConn)
        // If (ret) Then
        // If (Me.m_Annotazioni IsNot Nothing) Then dbConn.Save(Me.m_Annotazioni)
        // If (Me.m_Attachments IsNot Nothing) Then dbConn.Save(Me.m_Attachments)
        // End If
        // Return ret
        // End Function



        // Protected Overrides Sub XMLSerialize(ByVal writer As minidom.XMLWriter)
        // MyBase.XMLSerialize(writer)

        // End Sub

        // Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
        // Dim i As Integer
        // Select Case fieldName
        // Case "m_Annotazioni"
        // Me.m_Annotazioni = New CAnnotazioni
        // Me.m_Annotazioni.SetOwner(Me)
        // If TypeName(fieldValue) = "String" Then Exit Sub
        // If Not IsArray(fieldValue) Then fieldValue = {fieldValue}
        // For i = 0 To UBound(fieldValue)
        // Me.m_Annotazioni.AddItem(DirectCast(fieldValue, CAnnotazione())(i))
        // Next
        // Case "m_Attachments"
        // m_Attachments = New CAttachmentsCollection
        // Call m_Attachments.SetOwner(Me)
        // If TypeName(fieldValue) = "String" Then Exit Sub
        // If Not IsArray(fieldValue) Then fieldValue = {fieldValue}
        // For i = 0 To UBound(fieldValue)
        // Me.m_Attachments.Add(DirectCast(fieldValue, CAttachment())(i))
        // Next
        // Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
        // End Select
        // End Sub

        // End Class

    }
}