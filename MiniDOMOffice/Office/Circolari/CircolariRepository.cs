using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Office;


namespace minidom
{

    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti <see cref="Circolare"/>
        /// </summary>
        [Serializable]
        public partial class CircolariRepository
            : CModulesClass<Circolare>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CircolariRepository() 
                : base("modComunicazioni", typeof(CircolariCursor))
            {
            }
               

            /// <summary>
            /// Crea un avviso per la ricezione della comunicazione
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<NotificaUtenteXCircolare> Notify(Circolare comunicazione, string via = DMD.Strings.vbNullString)
            {
                var users = comunicazione.GetAffectedUsers();
                var ret = new CCollection<NotificaUtenteXCircolare>();
                int i = 0;
                via = Strings.Trim(via);
                foreach (var user in users)
                {
                    var item = new NotificaUtenteXCircolare();
                    item.Comunicazione = comunicazione;
                    item.TargetUser = user;
                    item.Stato = ObjectStatus.OBJECT_VALID;
                    item.Via = via;
                    item.Param = user.eMail;
                    item.Notify();
                    item.StatoComunicazione = StatoNotificaUtenteXCircolare.CONSEGNATO;
                    item.DataConsegna = DMD.DateUtils.Now();
                    item.DettaglioStato = "Consegnato";
                    i += 1;
                    
                    item.Save();
                    ret.Add(item);
                }

                return ret;
            }

            /// <summary>
            /// Restituisce il primo numero progressivo disponibile
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public int GetNextAvailableProgressivo()
            {
                var db = this.Database;                
                var dbSQL = db.SELECT(DBField.Field("Progressivo").Max().As("NextProgressivo"))
                               .FROM("tbl_Comunicazioni")
                               .WHERE(DBCursorField.Field("Stato").EQ(1));
                int maxID = db.ExecuteScalar<int>(dbSQL, 0);
                return maxID + 1;
            }


            // ''' <summary>
            // ''' Restituisce l'oggetto Configurazione
            // ''' </summary>
            // ''' <value></value>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Shared ReadOnly Property Configuration As CComunicazioniConfiguration
            // Get
            // If m_Configuration Is Nothing Then
            // m_Configuration = New CComunicazioniConfiguration
            // m_Configuration.Load()
            // End If
            // Return m_Configuration
            // End Get
            // End Property



            // Public Class CComunicazioniConfiguration
            // Inherits DBObjectBase

            // Private m_SMTPServerName As String 'Nome del server STMP
            // Private m_SMTPServerPort As Integer 'Porta di ascolto del server SMTP
            // Private m_SenderEMail As String 'Indirizzo e-mail del mittente
            // Private m_SenderPassword As String 'Password del mittente per accedere al server SMTP
            // Private m_TemplateURL As String 'Percorso del modello da utilizzare per l'invio e-mail
            // Private m_SendList As String 'Elenco degli utenti a cui inviare la comunicazione

            // Public Sub New()
            // m_SMTPServerName = ""
            // m_SMTPServerPort = 0
            // m_SenderEMail = ""
            // m_SenderPassword = ""
            // m_TemplateURL = ""
            // m_SendList = ""
            // End Sub

            // Public Overrides Function GetModule() As CModule
            // Return Nothing
            // End Function

            // Public Property SMTPServerName As String
            // Get
            // Return Me.m_SMTPServerName
            // End Get
            // Set(value As String)
            // value = Trim(value)
            // Dim oldValue As String = Me.m_SMTPServerName
            // If (oldValue = value) Then Exit Property
            // Me.m_SMTPServerName = value
            // Me.DoChanged("SMTPServerName", value, oldValue)
            // End Set
            // End Property

            // Public Property SMTPServerPort As Integer
            // Get
            // Return Me.m_SMTPServerPort
            // End Get
            // Set(value As Integer)
            // Dim oldValue As Integer = Me.m_SMTPServerPort
            // If (oldValue = value) Then Exit Property
            // Me.m_SMTPServerPort = value
            // Me.DoChanged("SMTPServerName", value, oldValue)
            // End Set
            // End Property

            // Public Property SenderEMail As String
            // Get
            // Return Me.m_SenderEMail
            // End Get
            // Set(value As String)
            // value = Trim(value)
            // Dim oldValue As String = Me.m_SenderEMail
            // If (oldValue = value) Then Exit Property
            // Me.m_SenderEMail = value
            // Me.DoChanged("SenderEMail", value, oldValue)
            // End Set
            // End Property

            // Public Property SenderPassword As String
            // Get
            // Return Me.m_SenderPassword
            // End Get
            // Set(value As String)
            // Dim oldValue As String = Me.m_SenderPassword
            // If (oldValue = value) Then Exit Property
            // Me.m_SenderPassword = value
            // Me.DoChanged("SenderPassword", value, oldValue)
            // End Set
            // End Property

            // Public Property TemplateURL As String
            // Get
            // Return Me.m_TemplateURL
            // End Get
            // Set(value As String)
            // value = Trim(value)
            // Dim oldValue As String = Me.m_TemplateURL
            // If (oldValue = value) Then Exit Property
            // Me.m_TemplateURL = value
            // Me.DoChanged("TemplateURL", value, oldValue)
            // End Set
            // End Property

            // Public Property SendList As String
            // Get
            // Return Me.m_SendList
            // End Get
            // Set(value As String)
            // value = Trim(value)
            // Dim oldValue As String = Me.m_SendList
            // If (oldValue = value) Then Exit Property
            // Me.m_SendList = value
            // Me.DoChanged("SendList", value, oldValue)
            // End Set
            // End Property

            // Public Overrides Function GetTableName() As String
            // Return "tbl_ComunicazioniConfig"
            // End Function

            // Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            // reader.Read("SMTPServerName", Me.m_SMTPServerName)
            // reader.Read("SMTPServerPort", Me.m_SMTPServerPort)
            // reader.Read("SenderEMail", Me.m_SenderEMail)
            // reader.Read("SenderPassword", Me.m_SenderPassword)
            // reader.Read("TemplateURL", Me.m_TemplateURL)
            // reader.Read("SendList", Me.m_SendList)
            // Return MyBase.LoadFromRecordset(reader)
            // End Function

            // Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            // writer.Write("SMTPServerName", Me.m_SMTPServerName)
            // writer.Write("SMTPServerPort", Me.m_SMTPServerPort)
            // writer.Write("SenderEMail", Me.m_SenderEMail)
            // writer.Write("SenderPassword", Me.m_SenderPassword)
            // writer.Write("TemplateURL", Me.m_TemplateURL)
            // writer.Write("SendList", Me.m_SendList)
            // Return MyBase.SaveToRecordset(writer)
            // End Function

            // Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            // Return Databases.APPConn
            // End Function

            // Public Sub Load()
            // Dim dbRis1 As System.Data.IDataReader
            // dbRis1 = APPConn.ExecuteReader("SELECT * FROM [tbl_ComunicazioniConfig] ORDER BY [ID] ASC")
            // Dim reader As New DBReader(APPConn.Tables("tbl_ComunicazioniConfig"), dbRis1)
            // If reader.Read Then APPConn.Load(Me, reader)
            // dbRis1.Close()
            // End Sub



            // End Class

        }

    }

    public partial class Office
    {
   
        private static CircolariRepository m_Circolari = null;

        /// <summary>
        /// Repository di oggetti <see cref="Circolare"/>
        /// TODO riportare le modifiche del nome in js (da Comunicazioni in Circolari)
        /// </summary>
        public static CircolariRepository Circolari
        {
            get
            {
                if (m_Circolari is null)
                    m_Circolari = new CircolariRepository();
                return m_Circolari;
            }
        }
    }
}