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
    public partial class Office
    {

        /// <summary>
        /// Rappresenta una scansione fatta da un dispositivo in ufficio
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class Scansione 
            : minidom.Databases.DBObjectPO
        {
            private string m_NomeDispositivo;
            private string m_NomeDocumento;
            private string m_MetodoRicezione;
            private string m_ParametriScansione;
            private DateTime? m_DataInvio;
            private DateTime? m_DataRicezione;
            private DateTime? m_DataElaborazione;
            private int m_IDCliente;
            [NonSerialized] private CPersona m_Cliente;
            private string m_NomeCliente;
            private int m_IDAttachment;
            [NonSerialized] private CAttachment m_Attachment;
            private int m_IDInviataDa;
            [NonSerialized] private CUser m_InviataDa;
            private string m_NomeInviataDa;
            private int m_IDInviataA;
            [NonSerialized] private CUser m_InviataA;
            private string m_NomeInviataA;
            private int m_IDElaborataDa;
            [NonSerialized] private CUser m_ElaborataDa;
            private string m_NomeElaborataDa;

            /// <summary>
            /// Costruttore
            /// </summary>
            public Scansione()
            {
                m_NomeDispositivo = "";
                m_NomeDocumento = "";
                m_MetodoRicezione = "";
                m_ParametriScansione = "";
                m_DataInvio = default;
                m_DataRicezione = default;
                m_DataElaborazione = default;
                m_IDCliente = 0;
                m_NomeCliente = "";
                m_IDAttachment = 0;
                m_Attachment = null;
                m_Flags = 0;
                m_IDInviataDa = 0;
                m_InviataDa = null;
                m_NomeInviataDa = "";
                m_IDInviataA = 0;
                m_InviataA = null;
                m_NomeInviataA = "";
                m_IDElaborataDa = 0;
                m_ElaborataDa = null;
                m_NomeElaborataDa = "";
            }

            /// <summary>
            /// Restituisce o imposta il nome del dispositivo da cui è stata fatta la scansione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeDispositivo
            {
                get
                {
                    return m_NomeDispositivo;
                }

                set
                {
                    string oldValue = m_NomeDispositivo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeDispositivo = value;
                    DoChanged("NomeDispositivo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del documento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeDocumento
            {
                get
                {
                    return m_NomeDocumento;
                }

                set
                {
                    string oldValue = m_NomeDocumento;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeDocumento = value;
                    DoChanged("NomeDocumento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta i parametri di scansione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ParametriScansione
            {
                get
                {
                    return m_ParametriScansione;
                }

                set
                {
                    string oldValue = m_ParametriScansione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ParametriScansione = value;
                    DoChanged("ParametriScansione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il metodo tramite cui è stata ricevuta la scansione (e-mail, ftp, directory)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string MetodoRicezione
            {
                get
                {
                    return m_MetodoRicezione;
                }

                set
                {
                    string oldValue = m_MetodoRicezione;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_MetodoRicezione = value;
                    DoChanged("MetodoDiRicezione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data e lora in cui il dispositivo remoto ha inviato la scansione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataInvio
            {
                get
                {
                    return m_DataInvio;
                }

                set
                {
                    var oldValue = m_DataInvio;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataInvio = value;
                    DoChanged("DataInvio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data in cui il sistema ha ricevuto la scansione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataRicezione
            {
                get
                {
                    return m_DataRicezione;
                }

                set
                {
                    var oldValue = m_DataRicezione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRicezione = value;
                    DoChanged("DataRicezione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di elaborazione (cioè la data in cui l'utente ha associato la scansione ad un cliente)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataElaborazione
            {
                get
                {
                    return m_DataElaborazione;
                }

                set
                {
                    var oldValue = m_DataElaborazione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataElaborazione = value;
                    DoChanged("DataElaborazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del cliente a cui è associata la scansione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDCliente
            {
                get
                {
                    return DBUtils.GetID(m_Cliente, m_IDCliente);
                }

                set
                {
                    int oldValue = IDCliente;
                    if (oldValue == value)
                        return;
                    m_IDCliente = value;
                    m_Cliente = null;
                    DoChanged("IDCliente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il cliente a cui è associata la scansione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CPersona Cliente
            {
                get
                {
                    if (m_Cliente is null)
                        m_Cliente = Anagrafica.Persone.GetItemById(m_IDCliente);
                    return m_Cliente;
                }

                set
                {
                    var oldValue = m_Cliente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Cliente = value;
                    m_IDCliente = DBUtils.GetID(value, 0);
                    m_NomeCliente = "";
                    if (value is object)
                        m_NomeCliente = value.Nominativo;
                    DoChanged("Cliente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del cliente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeCliente
            {
                get
                {
                    return m_NomeCliente;
                }

                set
                {
                    string oldValue = m_NomeCliente;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCliente = value;
                    DoChanged("NomeCliente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'allegato che contiene la scansione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDAttachment
            {
                get
                {
                    return DBUtils.GetID(m_Attachment, m_IDAttachment);
                }

                set
                {
                    int oldValue = IDAttachment;
                    if (oldValue == value)
                        return;
                    m_IDAttachment = value;
                    DoChanged("IDAttachment", value, oldValue);
                }
            }

            /// <summary>
            /// Allegato
            /// </summary>
            public CAttachment Attachment
            {
                get
                {
                    if (m_Attachment is null)
                        m_Attachment = Sistema.Attachments.GetItemById(m_IDAttachment);
                    return m_Attachment;
                }

                set
                {
                    var oldValue = m_Attachment;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Attachment = value;
                    m_IDAttachment = DBUtils.GetID(value, 0);
                    DoChanged("Attachment", value, oldValue);
                }
            }

            ///// <summary>
            ///// Restituisce o imposta dei flags aggiuntivi
            ///// </summary>
            ///// <value></value>
            ///// <returns></returns>
            ///// <remarks></remarks>
            //public new int Flags
            //{
            //    get
            //    {
            //        return m_Flags;
            //    }

            //    set
            //    {
            //        int oldValue = m_Flags;
            //        if (oldValue == value)
            //            return;
            //        m_Flags = value;
            //        DoChanged("Flags", value, oldValue);
            //    }
            //}

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che ha inviato la scansione dal dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDInviataDa
            {
                get
                {
                    return DBUtils.GetID(m_InviataDa, m_IDInviataDa);
                }

                set
                {
                    int oldValue = IDInviataDa;
                    if (oldValue == value)
                        return;
                    m_IDInviataDa = value;
                    m_InviataDa = null;
                    DoChanged("IDInviataDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha inviato la scansione dal dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CUser InviataDa
            {
                get
                {
                    if (m_InviataDa is null)
                        m_InviataDa = Sistema.Users.GetItemById(m_IDInviataDa);
                    return m_InviataDa;
                }

                set
                {
                    var oldValue = InviataDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_InviataDa = value;
                    m_IDInviataDa = DBUtils.GetID(value, 0);
                    m_NomeInviataDa = "";
                    if (value is object)
                        m_NomeInviataDa = value.Nominativo;
                    DoChanged("InviataDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha inviato la scansione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeInviataDa
            {
                get
                {
                    return m_NomeInviataDa;
                }

                set
                {
                    string oldValue = m_NomeInviataDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeInviataDa = value;
                    DoChanged("NomeInviataDa", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che ha inviato la scansione dal dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDInviataA
            {
                get
                {
                    return DBUtils.GetID(m_InviataA, m_IDInviataA);
                }

                set
                {
                    int oldValue = IDInviataA;
                    if (oldValue == value)
                        return;
                    m_IDInviataA = value;
                    m_InviataA = null;
                    DoChanged("IDInviataA", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha inviato la scansione dal dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser InviataA
            {
                get
                {
                    if (m_InviataA is null)
                        m_InviataA = Sistema.Users.GetItemById(m_IDInviataA);
                    return m_InviataA;
                }

                set
                {
                    var oldValue = InviataA;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_InviataA = value;
                    m_IDInviataA = DBUtils.GetID(value, 0);
                    m_NomeInviataA = "";
                    if (value is object)
                        m_NomeInviataA = value.Nominativo;
                    DoChanged("InviataA", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha inviato la scansione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeInviataA
            {
                get
                {
                    return m_NomeInviataA;
                }

                set
                {
                    string oldValue = m_NomeInviataA;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeInviataA = value;
                    DoChanged("NomeInviataA", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che ha inviato la scansione dal dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDElaborataDa
            {
                get
                {
                    return DBUtils.GetID(m_ElaborataDa, m_IDElaborataDa);
                }

                set
                {
                    int oldValue = IDElaborataDa;
                    if (oldValue == value)
                        return;
                    m_IDElaborataDa = value;
                    m_ElaborataDa = null;
                    DoChanged("IDElaborataDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha inviato la scansione dal dispositivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser ElaborataDa
            {
                get
                {
                    if (m_ElaborataDa is null)
                        m_ElaborataDa = Sistema.Users.GetItemById(m_IDElaborataDa);
                    return m_ElaborataDa;
                }

                set
                {
                    var oldValue = ElaborataDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_ElaborataDa = value;
                    m_IDElaborataDa = DBUtils.GetID(value, 0);
                    m_NomeElaborataDa = "";
                    if (value is object)
                        m_NomeElaborataDa = value.Nominativo;
                    DoChanged("ElaborataDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha inviato la scansione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeElaborataDa
            {
                get
                {
                    return m_NomeElaborataDa;
                }

                set
                {
                    string oldValue = m_NomeElaborataDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeElaborataDa = value;
                    DoChanged("NomeElaborataDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_NomeDocumento;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_DataRicezione);
            }
             
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Scansioni;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeScansioni";
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_NomeDispositivo = reader.Read("NomeDispositivo", m_NomeDispositivo);
                m_NomeDocumento = reader.Read("NomeDocumento", m_NomeDocumento);
                m_MetodoRicezione = reader.Read("MetodoRicezione", m_MetodoRicezione);
                m_ParametriScansione = reader.Read("ParametriScansione", m_ParametriScansione);
                m_DataInvio = reader.Read("DataInvio", m_DataInvio);
                m_DataRicezione = reader.Read("DataRicezione", m_DataRicezione);
                m_DataElaborazione = reader.Read("DataElaborazione", m_DataElaborazione);
                m_IDCliente = reader.Read("IDCliente", m_IDCliente);
                m_NomeCliente = reader.Read("NomeCliente", m_NomeCliente);
                m_IDAttachment = reader.Read("IDAttachment", m_IDAttachment);
                m_IDInviataDa = reader.Read("IDInviataDa", m_IDInviataDa);
                m_NomeInviataDa = reader.Read("NomeInviataDa", m_NomeInviataDa);
                m_IDInviataA = reader.Read("IDInviataA", m_IDInviataA);
                m_NomeInviataA = reader.Read("NomeInviataA", m_NomeInviataA);
                m_IDElaborataDa = reader.Read("IDElaborataDa", m_IDElaborataDa);
                m_NomeElaborataDa = reader.Read("NomeElaborataDa", m_NomeElaborataDa);
                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("NomeDispositivo", m_NomeDispositivo);
                writer.Write("NomeDocumento", m_NomeDocumento);
                writer.Write("MetodoRicezione", m_MetodoRicezione);
                writer.Write("ParametriScansione", m_ParametriScansione);
                writer.Write("DataInvio", m_DataInvio);
                writer.Write("DataRicezione", m_DataRicezione);
                writer.Write("DataElaborazione", m_DataElaborazione);
                writer.Write("IDCliente", IDCliente);
                writer.Write("NomeCliente", m_NomeCliente);
                writer.Write("IDAttachment", IDAttachment);
                writer.Write("IDInviataDa", IDInviataDa);
                writer.Write("NomeInviataDa", m_NomeInviataDa);
                writer.Write("IDInviataA", IDInviataA);
                writer.Write("NomeInviataA", m_NomeInviataA);
                writer.Write("IDElaborataDa", IDElaborataDa);
                writer.Write("NomeElaborataDa", m_NomeElaborataDa);
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("NomeDispositivo", typeof(string), 255);
                c = table.Fields.Ensure("NomeDocumento", typeof(string), 255);
                c = table.Fields.Ensure("MetodoRicezione", typeof(string), 255);
                c = table.Fields.Ensure("ParametriScansione", typeof(string), 0);
                c = table.Fields.Ensure("DataInvio", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataRicezione", typeof(DateTime), 1);
                c = table.Fields.Ensure("DataElaborazione", typeof(DateTime), 1);
                c = table.Fields.Ensure("IDCliente", typeof(int), 1);
                c = table.Fields.Ensure("NomeCliente", typeof(string), 255);
                c = table.Fields.Ensure("IDAttachment", typeof(int), 1);
                c = table.Fields.Ensure("IDInviataDa", typeof(int), 1);
                c = table.Fields.Ensure("NomeInviataDa", typeof(string), 255);
                c = table.Fields.Ensure("IDInviataA", typeof(int), 1);
                c = table.Fields.Ensure("NomeInviataA", typeof(string), 255);
                c = table.Fields.Ensure("IDElaborataDa", typeof(int), 1);
                c = table.Fields.Ensure("NomeElaborataDa", typeof(string), 255);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "NomeDispositivo", "NomeDocumento", "MetodoRicezione", "ParametriScansione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDate", new string[] { "DataInvio", "DataRicezione", "DataElaborazione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCliente", new string[] { "IDCliente", "NomeCliente" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxAttachment", new string[] { "IDAttachment" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxInviatoDa", new string[] { "IDInviataDa", "NomeInviataDa" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxInviatoA", new string[] { "IDInviataA", "NomeInviataA" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxElaboratoDa", new string[] { "IDElaborataDa", "NomeElaborataDa" }, DBFieldConstraintFlags.None);
            }                

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("NomeDispositivo", m_NomeDispositivo);
                writer.WriteAttribute("NomeDocumento", m_NomeDocumento);
                writer.WriteAttribute("MetodoRicezione", m_MetodoRicezione);
                writer.WriteAttribute("ParametriScansione", m_ParametriScansione);
                writer.WriteAttribute("DataInvio", m_DataInvio);
                writer.WriteAttribute("DataRicezione", m_DataRicezione);
                writer.WriteAttribute("DataElaborazione", m_DataElaborazione);
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", m_NomeCliente);
                writer.WriteAttribute("IDAttachment", IDAttachment);
                writer.WriteAttribute("IDInviataDa", IDInviataDa);
                writer.WriteAttribute("NomeInviataDa", m_NomeInviataDa);
                writer.WriteAttribute("IDInviataA", IDInviataA);
                writer.WriteAttribute("NomeInviataA", m_NomeInviataA);
                writer.WriteAttribute("IDElaborataDa", IDElaborataDa);
                writer.WriteAttribute("NomeElaborataDa", m_NomeElaborataDa);
                base.XMLSerialize(writer);
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
                    case "NomeDispositivo":
                        {
                            m_NomeDispositivo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeDocumento":
                        {
                            m_NomeDocumento = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MetodoRicezione":
                        {
                            m_MetodoRicezione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ParametriScansione":
                        {
                            m_ParametriScansione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataInvio":
                        {
                            m_DataInvio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataRicezione":
                        {
                            m_DataRicezione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataElaborazione":
                        {
                            m_DataElaborazione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDCliente":
                        {
                            m_IDCliente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCliente":
                        {
                            m_NomeCliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDAttachment":
                        {
                            m_IDAttachment = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }
 

                    case "IDInviataDa":
                        {
                            m_IDInviataDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeInviataDa":
                        {
                            m_NomeInviataDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDInviataA":
                        {
                            m_IDInviataA = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeInviataA":
                        {
                            m_NomeInviataA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDElaborataDa":
                        {
                            m_IDElaborataDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeElaborataDa":
                        {
                            m_NomeElaborataDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is Scansione) && this.Equals((Scansione)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(Scansione obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_NomeDispositivo, obj.m_NomeDispositivo)
                    && DMD.Strings.EQ(this.m_NomeDocumento, obj.m_NomeDocumento)
                    && DMD.Strings.EQ(this.m_MetodoRicezione, obj.m_MetodoRicezione)
                    && DMD.Strings.EQ(this.m_ParametriScansione, obj.m_ParametriScansione)
                    && DMD.DateUtils.EQ(this.m_DataInvio, obj.m_DataInvio)
                    && DMD.DateUtils.EQ(this.m_DataRicezione, obj.m_DataRicezione)
                    && DMD.DateUtils.EQ(this.m_DataElaborazione, obj.m_DataElaborazione)
                    && DMD.Integers.EQ(this.m_IDCliente, obj.m_IDCliente)
                    && DMD.Strings.EQ(this.m_NomeCliente, obj.m_NomeCliente)
                    && DMD.Integers.EQ(this.m_IDAttachment, obj.m_IDAttachment)
                    && DMD.Integers.EQ(this.m_IDInviataDa, obj.m_IDInviataDa)
                    && DMD.Strings.EQ(this.m_NomeInviataDa, obj.m_NomeInviataDa)
                    && DMD.Integers.EQ(this.m_IDInviataA, obj.m_IDInviataA)
                    && DMD.Strings.EQ(this.m_NomeInviataA, obj.m_NomeInviataA)
                    && DMD.Integers.EQ(this.m_IDElaborataDa, obj.m_IDElaborataDa)
                    && DMD.Strings.EQ(this.m_NomeElaborataDa, obj.m_NomeElaborataDa)
                    ;
            }
        }
    }
}