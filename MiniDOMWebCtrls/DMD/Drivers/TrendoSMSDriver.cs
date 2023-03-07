using System;
using DMD;

namespace minidom.Drivers
{
    public class TrendoSMSDriverOptions : Sistema.SMSDriverOptions
    {

        /// <summary>
        /// Restituisce o imposta la url della pagina che riceve il risultato di invio dell'SMS
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SMSFeedBackURL
        {
            get
            {
                return GetValueString("SMSFeedBackURL", "");
            }

            set
            {
                SetValueString("SMSFeedBackURL", DMD.Strings.Trim(value));
            }
        }

        /// <summary>
        /// Restituisce o imposta il nome utente per l'accesso al servizio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string UserName
        {
            get
            {
                return GetValueString("UserName", "");
            }

            set
            {
                SetValueString("UserName", DMD.Strings.Trim(value));
            }
        }

        /// <summary>
        /// Restituisce o imposta la password per l'accesso al servizio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Password
        {
            get
            {
                return GetValueString("Password", "");
            }

            set
            {
                SetValueString("Password", value);
            }
        }
    }

    public class TrendoSMSDriver : Sistema.BasicSMSDriver
    {
        public enum ServiceTypeEnum : int
        {
            Silver = 0,
            Gold = 1,
            GoldPlus = 2
        }

        private string m_DefaultAreaCode = "+39";
        private ServiceTypeEnum? m_ServiceType = default;
        private string m_UserName = DMD.Strings.vbNullString;
        private string m_Password = DMD.Strings.vbNullString;
        private DateTime? m_ScheduledTime = default;

        public TrendoSMSDriver()
        {
        }


        /// <summary>
        /// Restituisce o imposta il codice internazionale utilizzato per completare eventuali numeri per cui non è impostato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string DefaultAreaCode
        {
            get
            {
                return m_DefaultAreaCode;
            }

            set
            {
                value = m_DefaultAreaCode;
            }
        }

        /// <summary>
        /// Restituisce o imposta il tipo di servizio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ServiceTypeEnum? ServiceType
        {
            get
            {
                return m_ServiceType;
            }

            set
            {
                m_ServiceType = value;
            }
        }

        /// <summary>
        /// Restituisce o imposta la username usata per l'accesso al servizio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string UserName
        {
            get
            {
                return m_UserName;
            }

            set
            {
                m_UserName = Strings.Trim(value);
            }
        }

        /// <summary>
        /// Restituisce o imposta la password usata per l'accesso al servizio
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Password
        {
            get
            {
                return m_Password;
            }

            set
            {
                m_Password = value;
            }
        }


        /// <summary>
        /// Restituisce o imposta la data e l'ora di invio posticipato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DateTime? ScheduledTiem
        {
            get
            {
                return m_ScheduledTime;
            }

            set
            {
                m_ScheduledTime = value;
            }
        }

        public override string Description
        {
            get
            {
                return "Trendoo SMS";
            }
        }

        public override string GetUniqueID()
        {
            return "Trendoo SMS 1.00";
        }

        protected override string Send(string destNumber, string testo, Sistema.SMSDriverOptions options)
        {
            ServiceTypeEnum st;
            if (m_ScheduledTime.HasValue)
            {
                st = (ServiceTypeEnum)m_ServiceType;
            }
            else
            {
                st = Sistema.IIF(options.RichiediConfermaDiLettura, ServiceTypeEnum.Gold, ServiceTypeEnum.Silver);
                if (!string.IsNullOrEmpty(options.Mittente))
                    st = ServiceTypeEnum.GoldPlus;
            }

            var modem = GetModem(options.ModemName);
            if (modem is null)
                throw new InvalidOperationException("Nessun modem installato");
            if (modem.SendEnabled == false)
                throw new InvalidOperationException("Il modem non consente l'invio di SMS");
            string ret = SMSSEND(modem.UserName, modem.Password, new string[] { modem.DialPrefix + destNumber }, testo, st, options.Mittente, ScheduledTiem, "");
            var results = Strings.Split(ret, "|");
            if (results[0] != "OK")
                throw new Exception("Errore di invio SMS:" + ret);
            return results[1];   // Restituisce l'order_id
        }

        private Sistema.MessageStatusEnum ParseMessageStatus(string value)
        {
            switch (Strings.UCase(Strings.Trim(value)) ?? "")
            {
                case "SCHEDULED":
                    {
                        return Sistema.MessageStatusEnum.Scheduled;
                    }

                case "SENT":
                    {
                        return Sistema.MessageStatusEnum.Sent;
                    }

                case "DLVRD":
                    {
                        return Sistema.MessageStatusEnum.Delivered;
                    }

                case "ERROR":
                    {
                        return Sistema.MessageStatusEnum.Error;
                    }

                case "TIMEOUT":
                    {
                        return Sistema.MessageStatusEnum.Timeout;
                    }

                case "TOOM4NUM":
                    {
                        return Sistema.MessageStatusEnum.Error; // Troppi SMS per lo stesso destinatario nelle ultime 24 ore
                    }

                case "TOOM4USER":
                    {
                        return Sistema.MessageStatusEnum.Error; // Troppi SMS inviati dall'utente nelle ultime 24 ore
                    }

                case "UNKNPFX":
                    {
                        return Sistema.MessageStatusEnum.BadNumber;  // Prefisso SMS non valido o sconosciuto
                    }

                case "UNKNRCPT":
                    {
                        return Sistema.MessageStatusEnum.BadNumber;  // Numero di telefono del destinatario non valido o sconosciuto
                    }

                case "WAIT4DLVR":
                    {
                        return Sistema.MessageStatusEnum.Waiting;    // Numero di telefono del destinatario non valido o sconosciuto
                    }

                case "WAITING":
                    {
                        return Sistema.MessageStatusEnum.Waiting; // Numero di telefono del destinatario non valido o sconosciuto
                    }

                default:
                    {
                        // Case "UNKNOWN"
                        return Sistema.MessageStatusEnum.Unknown;
                    }
            }
        }

        public override bool SupportaConfermaDiRecapito()
        {
            return true;
        }

        protected override int CountRequiredSMS(string text)
        {
            int maxl = GetMaxSMSLen();
            int msgl = GetSMSTextLen(text);
            if (msgl > maxl)
            {
                return (int)Maths.Ceiling(msgl / 153d);
            }
            else
            {
                return 1;
            }
        }

        protected override int GetSMSTextLen(string text)
        {
            int cnt = 0;
            for (int i = 0, loopTo = Strings.Len(text); i <= loopTo; i++)
            {
                string ch = Strings.Mid(text, i, 1);
                switch (ch ?? "")
                {
                    case "^":
                    case "{":
                    case "}":
                    case @"\":
                    case "[":
                    case "~":
                    case "]":
                    case "|":
                    case "€":
                        {
                            cnt += 2;
                            break;
                        }

                    default:
                        {
                            cnt += 1;
                            break;
                        }
                }
            }

            return cnt;
        }

        public override bool SupportaMessaggiConcatenati()
        {
            return true;
        }

        protected override Sistema.DriverMessageStatus GetStatus(string order_id)
        {
            order_id = Strings.Trim(order_id);
            if (string.IsNullOrEmpty(order_id))
                throw new ArgumentNullException("order_id");
            string ret = SMSSTATUS(UserName, Password, order_id);
            if (Strings.Left(ret, 2) != "OK")
            {
                throw new Exception("Formato della risposta non valido: " + ret);
            }

            ret = Strings.Mid(ret, 4);
            var rows = Strings.Split(ret, ";");
            var result = new Sistema.DriverMessageStatus();
            // For i As Integer = 0 To UBound(rows)
            int i = 0;
            var nibbles = Strings.Split(rows[i], "|");
            result.MessageID = order_id;
            if (DMD.Arrays.Len(nibbles) > 0)
                result.TargetNumber = nibbles[0];
            if (DMD.Arrays.Len(nibbles) > 1)
            {
                result.MessageStatus = ParseMessageStatus(nibbles[1]);
                result.MessageStatusEx = nibbles[1];
            }
            else
            {
                result.MessageStatus = Sistema.MessageStatusEnum.Unknown;
                result.MessageStatusEx = "";
            }

            if (DMD.Arrays.Len(nibbles) > 2 && result.MessageStatus == Sistema.MessageStatusEnum.Delivered)
            {
                result.DeliveryTime = ParseDriverDate(nibbles[2]);
            }
            // Next

            switch (result.MessageStatus)
            {
                case Sistema.MessageStatusEnum.BadNumber:
                    {
                        result.MessageStatusEx = "Messaggio non inviato: Numero Errato";
                        break;
                    }

                case Sistema.MessageStatusEnum.Delivered:
                    {
                        result.MessageStatusEx = "Messaggio Ricevuto";
                        break;
                    }

                case Sistema.MessageStatusEnum.Error:
                    {
                        result.MessageStatusEx = "Errore generico";
                        break;
                    }

                case Sistema.MessageStatusEnum.Scheduled:
                    {
                        result.MessageStatusEx = "Messaggio predisposto per l'invio ritardato";
                        break;
                    }

                case Sistema.MessageStatusEnum.Sent:
                    {
                        result.MessageStatusEx = "Messaggio Inviato";
                        break;
                    }

                case Sistema.MessageStatusEnum.Timeout:
                    {
                        result.MessageStatusEx = "Errore di Timeout";
                        break;
                    }

                case Sistema.MessageStatusEnum.Unknown:
                    {
                        result.MessageStatusEx = "Stato di invio Sconosciuto";
                        break;
                    }

                case Sistema.MessageStatusEnum.Waiting:
                    {
                        result.MessageStatusEx = "Messaggio in attesa di essere inviato";
                        break;
                    }
            }

            return result;
        }

        public void RemoveDelayed(string order_id)
        {
            order_id = Strings.Trim(order_id);
            if (string.IsNullOrEmpty(order_id))
                throw new ArgumentNullException("order_id");
            string ret = REMOVE_DELAYED(UserName, Password, order_id);
            if (Strings.Left(ret, 2) != "OK")
            {
                throw new Exception("Formato della risposta non valido: " + ret);
            }
        }

        /// <summary>
        /// Restituisce la lista degli SMS inviati nell'intervallo data specificato
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public CCollection GetSMSHistory(DateTime from, DateTime to)
        {
            var col = new CCollection();
            string ret = SMSHISTORY(UserName, Password, from, to);
            if (Strings.Left(ret, 2) != "OK")
            {
                throw new Exception("Formato della risposta non valido: " + ret);
            }

            return col;
        }

        /// <summary>
        /// Restituisce la disponibilità di credito
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public decimal GetCredit()
        {
            string ret = GetCredit(UserName, Password);
            if (Strings.Left(ret, 2) != "OK")
            {
                throw new Exception("Formato della risposta non valido: " + ret);
            }
            else
            {
                return (decimal)Sistema.Formats.ParseValuta(Strings.Mid(ret, 3));
            }
        }

        protected override void VerifySender(string sender)
        {
            CheckSender(sender);
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private void CheckSender(string value)
        {
            string numbers = "0123456789+";
            bool isAlfa = false;
            for (int i = 1, loopTo = Strings.Len(numbers); i <= loopTo; i++)
            {
                if (Strings.InStr(value, Strings.Mid(numbers, i, 1)) > 0)
                {
                    isAlfa = true;
                    break;
                }
            }

            if (isAlfa)
            {
                if (Strings.Len(value) > 11)
                    throw new ArgumentException("Il nome del mittente non può superare 11 caratteri");
            }
            else if (Strings.Len(value) > 16)
                throw new ArgumentException("Il numero del mittente non può superare 16 cifre");
        }

        private string FormatDriverDate(DateTime? value)
        {
            if (!value.HasValue)
                return DMD.Strings.vbNullString;
            return Sistema.Formats.Format(value, "yyyyMMddHHmmss");
        }

        private DateTime? ParseDriverDate(string value)
        {
            value = Strings.Trim(value);
            if (string.IsNullOrEmpty(value))
                return default;
            int yyyy = Sistema.Formats.ToInteger(Strings.Mid(value, 1, 4));
            int MM = Sistema.Formats.ToInteger(Strings.Mid(value, 5, 2));
            int dd = Sistema.Formats.ToInteger(Strings.Mid(value, 7, 2));
            int HH = Sistema.Formats.ToInteger(Strings.Mid(value, 9, 2));
            int m = Sistema.Formats.ToInteger(Strings.Mid(value, 11, 2));
            int ss = Sistema.Formats.ToInteger(Strings.Mid(value, 13, 2));
            return DMD.DateUtils.MakeDate(yyyy, MM, dd, HH, m, ss);
        }

        public string FormatTargetNumber(string value)
        {
            string ret = Sistema.Formats.ParsePhoneNumber(value);
            if (Strings.Left(ret, 1) != "+" || Strings.Left(ret, 2) != "00")
                ret = m_DefaultAreaCode + ret;
            return ret;
        }

        private string URLEncode(string value)
        {
            return DMD.WebUtils.URLEncode(value);
        }

        private string PrepareRecipients(string[] numbers)
        {
            var ret = new System.Text.StringBuilder();
            if (numbers is object && numbers.Length > 0)
            {
                for (int i = 0, loopTo = DMD.Arrays.UBound(numbers); i <= loopTo; i++)
                {
                    string n = numbers[i];
                    string p = Sistema.Formats.ParsePhoneNumber(n);
                    if (!string.IsNullOrEmpty(p))
                    {
                        if (ret.Length > 0)
                            ret.Append(",");
                        ret.Append(p);
                    }
                }
            }

            return ret.ToString();
        }

        private string FormatMessageType(ServiceTypeEnum value)
        {
            switch (value)
            {
                case ServiceTypeEnum.Silver:
                    {
                        return "SI";
                    }

                case ServiceTypeEnum.Gold:
                    {
                        return "GS";
                    }

                case ServiceTypeEnum.GoldPlus:
                    {
                        return "GP";
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException("message type");
                        break;
                    }
            }
        }

        private string HttpRequest(string address)
        {
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped ElseDirectiveTrivia */
            var req = new System.Net.WebClient();
            string ret = req.DownloadString(address);
            req.Dispose();
            return ret;
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
        }

        public string SMSSEND(string username, string password, string[] destNumbers, string testo, ServiceTypeEnum messageType, string sender, DateTime? scheduledTime, string orderID)
        {
            username = Strings.Trim(username);
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");
            password = Strings.Trim(password);
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            string recips = PrepareRecipients(destNumbers);
            if (string.IsNullOrEmpty(recips))
                throw new ArgumentNullException("destNumbers");
            testo = Strings.Trim(testo);
            if (string.IsNullOrEmpty(testo))
                throw new ArgumentNullException("testo");
            sender = Strings.Trim(sender);
            if (!string.IsNullOrEmpty(sender))
                CheckSender(sender);
            orderID = Strings.Trim(orderID);
            var buffer = new System.Text.StringBuilder();
            buffer.Append("https://api.trendoo.it/Trend/SENDSMS?");
            buffer.Append("login=" + URLEncode(username));
            buffer.Append("&password=" + URLEncode(password));
            buffer.Append("&message_type=" + FormatMessageType(messageType));
            buffer.Append("&recipient=" + recips);
            buffer.Append("&message=" + URLEncode(DMD.WebUtils.RemoveHTMLTags(testo)));
            if (!string.IsNullOrEmpty(sender))
                buffer.Append("&sender=" + URLEncode(sender));
            if (scheduledTime.HasValue)
                buffer.Append("&scheduled_delivery_time=" + FormatDriverDate(scheduledTime.Value));
            if (!string.IsNullOrEmpty(orderID))
                buffer.Append("&order_id=" + URLEncode(orderID));
            return HttpRequest(buffer.ToString());
        }

        public string SMSSTATUS(string userName, string password, string order_id)
        {
            userName = Strings.Trim(userName);
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("username");
            password = Strings.Trim(password);
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            order_id = Strings.Trim(order_id);
            if (string.IsNullOrEmpty(order_id))
                throw new ArgumentNullException("order_id");
            var buffer = new System.Text.StringBuilder();
            buffer.Append("https://api.trendoo.it/Trend/SMSSTATUS?");
            buffer.Append("login=" + URLEncode(userName));
            buffer.Append("&password=" + URLEncode(password));
            buffer.Append("&order_id=" + URLEncode(order_id));
            return HttpRequest(buffer.ToString());
        }

        public string REMOVE_DELAYED(string userName, string password, string order_id)
        {
            userName = Strings.Trim(userName);
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("username");
            password = Strings.Trim(password);
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            order_id = Strings.Trim(order_id);
            if (string.IsNullOrEmpty(order_id))
                throw new ArgumentNullException("order_id");
            var buffer = new System.Text.StringBuilder();
            buffer.Append("https://api.trendoo.it/Trend/REMOVE_DELAYED?");
            buffer.Append("login=" + URLEncode(userName));
            buffer.Append("&password=" + URLEncode(password));
            buffer.Append("&order_id=" + URLEncode(order_id));
            return HttpRequest(buffer.ToString());
        }

        /// <summary>
        /// Restituisce la lista degli SMS inviati nell'intervallo data specificato
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SMSHISTORY(string userName, string password, DateTime from, DateTime to)
        {
            userName = Strings.Trim(userName);
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("username");
            password = Strings.Trim(password);
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            var buffer = new System.Text.StringBuilder();
            buffer.Append("https://api.trendoo.it/Trend/SMSHISTORY?");
            buffer.Append("login=" + URLEncode(userName));
            buffer.Append("&password=" + URLEncode(password));
            buffer.Append("&from=" + FormatDriverDate(from));
            buffer.Append("&to=" + FormatDriverDate(to));
            return HttpRequest(buffer.ToString());
        }

        /// <summary>
        /// Restituisce la disponibilità di credito
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetCredit(string userName, string password)
        {
            userName = Strings.Trim(userName);
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("username");
            password = Strings.Trim(password);
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            var buffer = new System.Text.StringBuilder();
            var col = new CCollection();
            buffer.Append("https://api.trendoo.it/Trend/CREDITS?");
            buffer.Append("login=" + URLEncode(userName));
            buffer.Append("&password=" + URLEncode(password));
            return HttpRequest(buffer.ToString());
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        protected override DriverOptions InstantiateNewOptions()
        {
            return new TrendoSMSDriverOptions();
        }

        protected override void InternalConnect()
        {
            // Il driver non richiede la connessione ma username e password
            {
                var withBlock = (TrendoSMSDriverOptions)GetDefaultOptions();
                UserName = withBlock.UserName;
                Password = withBlock.Password;
            }
        }

        protected override void InternalDisconnect()
        {
            // Il driver non richiede la connessione
        }
    }
}