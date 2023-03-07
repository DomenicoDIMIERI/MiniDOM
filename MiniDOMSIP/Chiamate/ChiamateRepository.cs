using System.Collections.Generic;
using System.Diagnostics;
using DMD;
using DMD.Databases;
using minidom.CallManagers;
using minidom.CallManagers.Events;
using minidom.PBX;

namespace minidom.repositories
{

    /// <summary>
    /// Repository di oggetti <see cref="Chiamata"/>
    /// </summary>
    public sealed class ChiamateRepository
        : CModulesClass<Chiamata>
    {
        
        private List<Chiamata> m_Attive = new List<Chiamata>();
        
        public Chiamate()
            : base("modChiamateRepo", typeof(ChiamateCursor), 0)
        {
        }
 

        /// <summary>
        /// Istanza un oggetto che racchiude le informazioni su una chiamata effettuata
        /// </summary>
        /// <param name="server"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static Chiamata NewOutCall(AsteriskServer server, DialEvent e)
        {
            var c = new Chiamata();
            c.ServerIP = server.ServerName + ":" + server.ServerPort;
            c.ServerName = server.ServerName;
            c.SourceNumber = e.CallerIDNumber;
            c.TargetNumber = e.ConnectedLineNum;
            c.Direzione = 1;
            c.Channel = e.Channel;
            c.StatoChiamata = StatoChiamata.Dialling;
            c.StartTime = e.Data;
            c.Parameters.SetItemByKey("Destination", e.Destination);
            c.Parameters.SetItemByKey("DialString", e.DialString);
            c.Parameters.SetItemByKey("Uniqueid", e.UniqueID);
            c.Save();
            lock (@lock)
                m_Attive.Add(c);
            return c;
        }

        /// <summary>
        /// Istanzia un oggetto che racchiude le informazioni su una chiamata ricevuta
        /// </summary>
        /// <param name="server"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static Chiamata NewInCall(AsteriskServer server, DialEvent e)
        {
            var c = new Chiamata();
            c.ServerIP = server.ServerName + ":" + server.ServerPort;
            c.ServerName = server.ServerName;
            c.TargetNumber = server.Channel;
            c.SourceNumber = e.CallerIDNumber;
            c.Channel = e.Channel;
            c.Direzione = 0;
            c.StatoChiamata = StatoChiamata.Ringing;
            c.StartTime = e.Data;
            c.Parameters.SetItemByKey("Destination", e.Destination);
            c.Parameters.SetItemByKey("DialString", e.DialString);
            c.Parameters.SetItemByKey("Uniqueid", e.UniqueID);
            c.Save();
            lock (@lock)
                m_Attive.Add(c);
            return c;
        }

        /// <summary>
        /// Informa il sistema che la chiamata é terminata
        /// </summary>
        /// <param name="e"></param>
        public static void NotifyCallEnded(HangupEvent e)
        {
            Chiamata c = null;
            lock (@lock)
            {
                foreach (var c1 in m_Attive)
                {
                    if ((DMD.Strings.CStr(c1.Parameters.GetItemByKey("Destination")) ?? "") == (e.Channel ?? ""))
                    {
                        c = c1;
                        break;
                    }
                }

                if (c is object)
                {
                    m_Attive.Remove(c);
                }
            }

            if (c is object)
            {
                if (c.StatoChiamata == StatoChiamata.Speaking)
                {
                    c.StatoChiamata = StatoChiamata.Answered;
                }
                else
                {
                    c.StatoChiamata = StatoChiamata.NotAnswered;
                }

                c.EndTime = e.Data;
                c.Save();
            }
        }

        /// <summary>
        /// Informa il sistema che lo stato di una chiamata ha subito delle modifiche
        /// </summary>
        /// <param name="e"></param>
        public static void NotifyChannelState(AsteriskEvent e)
        {
            string channel = e.GetAttribute("Channel");
            if (string.IsNullOrEmpty(channel))
                return;
            lock (@lock)
            {
                foreach (var c1 in m_Attive)
                {
                    string destination = DMD.Strings.CStr(c1.Parameters.GetItemByKey("Destination"));
                    string state = e.GetAttribute("CHANNELSTATE");
                    string stateEx = e.GetAttribute("CHANNELSTATEDESC");
                    if ((destination ?? "") == (channel ?? ""))
                    {
                        switch (state ?? "")
                        {
                            case "6": // Risposto
                                {
                                    c1.AnswerTime = e.Data;
                                    c1.StatoChiamata = StatoChiamata.Speaking;
                                    c1.Save();
                                    break;
                                }

                            case "5": // Ringing
                                {
                                    c1.StatoChiamata = StatoChiamata.Ringing;
                                    c1.Save();
                                    break;
                                }

                            default:
                                {
                                    Debug.Print(state + " -> " + stateEx);
                                    break;
                                }
                        }

                        break;
                    }
                }
            }
        }
    }
}