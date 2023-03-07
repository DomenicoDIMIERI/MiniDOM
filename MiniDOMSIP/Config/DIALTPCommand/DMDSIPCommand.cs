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

namespace minidom.PBX
{


    /// <summary>
    /// Rappresenta un coamndo inviato ad un sistema remoto
    /// </summary>
    public sealed class DMDSIPCommand
        : DMD.XML.DMDBaseXMLObject 
    {
        public string IDPostazione;        // Stringa che identifica la postazione a cui fa riferimento la configurazione
        public string IDMacchina;          // Stringa che identifica la macchina a cui fa riferimento la configurazione
        public string IDUtente;            // Strings che identifica l'utente collegato alla macchina
        public string Name;                        // Nome del comando da eseguire
        public CKeyCollection Parameters = new CKeyCollection();          // Parametri del comando
        public DateTime? ProgramedTime;           // Data e ora in cui eseguire il comando
        public DateTime? RunTime;                   // Data in cui il comando è stato eseguito
        public CKeyCollection Results = new CKeyCollection();             // Risultati del comando 

        /// <summary>
        /// Costruttore
        /// </summary>
        public DMDSIPCommand()
        {
            IDPostazione = "";
            IDMacchina = "";
            IDUtente = "";
            Name = "";
            Parameters = new CKeyCollection();
            ProgramedTime = default;
            RunTime = default;
            Results = new CKeyCollection();
        }

       

        //public override CModulesClass GetModule()
        //{
        //    return null;
        //}

        //public override string GetTableName()
        //{
        //    return "tbl_DialTPCmd";
        //}

        //protected override bool LoadFromRecordset(DBReader reader)
        //{
        //    this.IDPostazione = reader.Read("IDPostazione", this.IDPostazione);
        //    this.IDMacchina = reader.Read("IDMacchina", this.IDMacchina);
        //    this.IDUtente = reader.Read("IDUtente", this.IDUtente);
        //    this.Name = reader.Read("Name", this.Name);
        //    string tmp = reader.Read("Parameters", "");
        //    if (!string.IsNullOrEmpty(tmp))
        //        this.Parameters = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);

        //    ProgramedTime = reader.Read("ProgramedTime", this.ProgramedTime);
        //    RunTime = reader.Read("RunTime", this.RunTime);

        //    tmp = reader.Read("Results", "");
        //    if (!string.IsNullOrEmpty(tmp))
        //        Results = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);

        //    return base.LoadFromRecordset(reader);
        //}

        //protected override bool SaveToRecordset(DBWriter writer)
        //{
        //    writer.Write("IDPostazione", IDPostazione);
        //    writer.Write("IDMacchina", IDMacchina);
        //    writer.Write("IDUtente", IDUtente);
        //    writer.Write("Name", Name);
        //    writer.Write("Parameters", DMD.XML.Utils.Serializer.Serialize(Parameters));
        //    writer.Write("ProgramedTime", ProgramedTime);
        //    writer.Write("RunTime", RunTime);
        //    writer.Write("Results", DMD.XML.Utils.Serializer.Serialize(Results));
        //    return base.SaveToRecordset(writer);
        //}

        /// <summary>
        /// Serializzazione xml
        /// </summary>
        /// <param name="writer"></param>
        protected override void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("IDPostazione", IDPostazione);
            writer.WriteAttribute("IDMacchina", IDMacchina);
            writer.WriteAttribute("IDUtente", IDUtente);
            writer.WriteAttribute("Name", Name);
            writer.WriteAttribute("ProgramedTime", ProgramedTime);
            writer.WriteAttribute("RunTime", RunTime);
            base.XMLSerialize(writer);
            writer.WriteTag("Parameters", DMD.XML.Utils.Serializer.Serialize(Parameters));
            writer.WriteTag("Results", DMD.XML.Utils.Serializer.Serialize(Results));
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
                case "IDPostazione":
                    {
                        IDPostazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "IDMacchina":
                    {
                        IDMacchina = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "IDUtente":
                    {
                        IDUtente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "Name":
                    {
                        Name = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "ProgramedTime":
                    {
                        ProgramedTime = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                        break;
                    }

                case "RunTime":
                    {
                        RunTime = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                        break;
                    }

                case "Parameters":
                    {
                        Parameters = (CKeyCollection)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                        break;
                    }

                case "Results":
                    {
                        Results = (CKeyCollection)DMD.XML.Utils.Serializer.ToObject(fieldValue);
                        break;
                    }

                default:
                    {
                        base.SetFieldInternal(fieldName, fieldValue);
                        break;
                    }
            }
        }

        /// <summary>
        /// Esegue il comando
        /// </summary>
        public void Execute()
        {
            Log.LogMessage("DialTPCommand.Execute: " + Name);
            switch (DMD.Strings.LCase(DMD.Strings.Trim(Name)) ?? "")
            {
                case "shutdown":
                    {
                        Process.Start("shutdown", "-s -t 00");
                        Results.SetItemByKey("[default]", "ok");
                        break;
                    }

                case "reboot":
                    {
                        Process.Start("shutdown", "-r -t 00");
                        Results.SetItemByKey("[default]", "ok");
                        break;
                    }

                case "restart":
                    {
                        Results.SetItemByKey("[default]", "ok");
                        System.Windows.Forms.Application.Restart();
                        break;
                    }

                case "forceupdate":
                    {
                        DMDSIPApp.InstallUpdateSyncWithInfo(true);
                        Results.SetItemByKey("[default]", "ok");
                        break;
                    }

                case "ffmpeg_streammic":
                    {
                        string micName = DMD.Strings.CStr(Parameters.GetItemByKey("micname"));
                        string udpAddress = DMD.Strings.CStr(Parameters.GetItemByKey("updaddress"));
                        // ffmpeg -f dshow -i audio="USB Mic (2- Samson GoMic)" -c:a libmp3lame -f mpegts udp://192.168.0.255:12345
                        Process.Start("ffmpeg", "-f dshow -i audio=\"" + DMD.Strings.Replace(micName, DMD.Strings.CStr('"'), "'") + "\" -c:a libmp3lame -f mpegts udp://" + udpAddress);
                        Results.SetItemByKey("[default]", "ok");
                        break;
                    }
            }
        }
    }
}