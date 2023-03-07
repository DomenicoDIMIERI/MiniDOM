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
using static minidom.CustomerCalls;

namespace minidom
{
    public partial class CustomerCalls
    {

        /// <summary>
        /// Rappresenta una serie di informazioni minimali mostrate nell'interfaccia utente ed in grado di 
        /// rappresentare un oggetto del CRM e la sua evoluzione nel crm
        /// </summary>
        [Serializable]
        public class StoricoAction 
            : DMD.XML.DMDBaseXMLObject, IComparable, IComparable<StoricoAction>
        {
            /// <summary>
            /// Data dell'evento
            /// </summary>
            public DateTime? Data;

            /// <summary>
            /// ID dell'operatore
            /// </summary>
            public int IDOperatore;

            /// <summary>
            /// Nome dell'operatore
            /// </summary>
            public string NomeOperatore;

            /// <summary>
            /// ID del cliente
            /// </summary>
            public int IDCliente;

            /// <summary>
            /// Nome del cliente
            /// </summary>
            public string NomeCliente;

            /// <summary>
            /// Descrizione dell'evento
            /// </summary>
            public string Note;

            /// <summary>
            /// Scopo dell'evento
            /// </summary>
            public string Scopo;

            /// <summary>
            /// Numero o indirizzo dell'evento
            /// </summary>
            public string NumeroOIndirizzo;

            /// <summary>
            /// Esito dell'evento
            /// </summary>
            public EsitoChiamata Esito;

            /// <summary>
            /// Informazioni aggiuntive sull'esito dell'evento
            /// </summary>
            public string DettaglioEsito;

            /// <summary>
            /// Durata totale dell'evento
            /// </summary>
            public double Durata;

            /// <summary>
            /// Tempo di Attesa, ad esempio il tempo di composizione + attesa risposta di una chiamata (compreso nella durata)
            /// </summary>
            public double Attesa;

            /// <summary>
            /// Tag
            /// </summary>
            private object m_Tag;

            /// <summary>
            /// Indica lo stato della conversazione nell'istante descritto dall'evento
            /// </summary>
            public StatoConversazione StatoConversazione;

            /// <summary>
            /// ID dell'oggetto che ha generato l'evento
            /// </summary>
            public int ActionID;

            /// <summary>
            /// Tipo dell'oggetto che ha generato l'evento
            /// </summary>
            public string ActionType;

            /// <summary>
            /// Codice dell'evento che descrive quale sottostato dell'oggetto di origine é descritta dall'evento
            /// </summary>
            public int ActionSubID;

            /// <summary>
            /// Se true indica che l'evento è stato iniziato dal cliente.
            /// Se false indica che l'evento é stato iniziato da un operatore o dal sistema automatico
            /// </summary>
            public bool Ricevuta;


            private int m_AttachmentID;
            private Sistema.CAttachment m_Attachment;

            /// <summary>
            /// Costruttore
            /// </summary>
            public StoricoAction()
            {
                 
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Strings.ConcatArray(this.Data, " - ", this.ActionType, " - ", this.NomeCliente);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.Data);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(DMDBaseXMLObject obj)
            {
                return (obj is StoricoAction) && this.Equals((StoricoAction)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(StoricoAction obj)
            {
                return base.Equals(obj)
                    && DMD.DateUtils.EQ(this.Data, obj.Data)
                    && DMD.Integers.EQ(this.IDOperatore, obj.IDOperatore)
                    && DMD.Strings.EQ(this.NomeOperatore, obj.NomeOperatore)
                    && DMD.Integers.EQ(this.IDCliente, obj.IDCliente)
                    && DMD.Strings.EQ(this.NomeCliente, obj.NomeCliente)
                    && DMD.Strings.EQ(this.Note, obj.Note)
                    && DMD.Strings.EQ(this.Scopo, obj.Scopo)
                    && DMD.Strings.EQ(this.NumeroOIndirizzo, obj.NumeroOIndirizzo)
                    && DMD.Integers.EQ((int)this.Esito, (int)obj.Esito)
                    && DMD.Strings.EQ(this.DettaglioEsito, obj.DettaglioEsito)
                    && DMD.Doubles.EQ(this.Durata, obj.Durata)
                    && DMD.Doubles.EQ(this.Attesa, obj.Attesa)
                    //object m_Tag;
                    && DMD.Integers.EQ((int)this.StatoConversazione, (int)obj.StatoConversazione)
                    && DMD.Integers.EQ(this.ActionID, obj.ActionID)
                    && DMD.Strings.EQ(this.ActionType, obj.ActionType)
                    && DMD.Integers.EQ(this.ActionSubID, obj.ActionSubID)
                    && DMD.Booleans.EQ(this.Ricevuta, obj.Ricevuta)
                    && DMD.Integers.EQ(this.m_AttachmentID, obj.m_AttachmentID)
                    ;
            }



            /// <summary>
            /// Tag
            /// </summary>
            public object Tag
            {
                get
                {
                    return m_Tag;
                }

                set
                {
                    ActionID = DBUtils.GetID(value, 0);
                    ActionType = (value is null)? "" : DMD.RunTime.vbTypeName(value);
                    m_Tag = value;
                }
            }

            /// <summary>
            /// AttachmentID
            /// </summary>
            public int AttachmentID
            {
                get
                {
                    return DBUtils.GetID(m_Attachment, m_AttachmentID);
                }

                set
                {
                    m_AttachmentID = value;
                    m_Attachment = null;
                }
            }

            /// <summary>
            /// Attachment
            /// </summary>
            public Sistema.CAttachment Attachment
            {
                get
                {
                    if (m_Attachment is null)
                        m_Attachment = Sistema.Attachments.GetItemById(m_AttachmentID);
                    return m_Attachment;
                }

                set
                {
                    m_Attachment = value;
                    m_AttachmentID = DBUtils.GetID(value, 0);
                }
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
                    case "Data":
                        {
                            Data = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDOperatore":
                        {
                            IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatore":
                        {
                            NomeOperatore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDCliente":
                        {
                            IDCliente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCliente":
                        {
                            NomeCliente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Note":
                        {
                            Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Scopo":
                        {
                            Scopo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NumeroOIndirizzo":
                        {
                            NumeroOIndirizzo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Esito":
                        {
                            Esito = (EsitoChiamata)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DettaglioEsito":
                        {
                            DettaglioEsito = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Durata":
                        {
                            Durata = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Attesa":
                        {
                            Attesa = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "StatoConversazione":
                        {
                            StatoConversazione = (StatoConversazione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ActionID":
                        {
                            ActionID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ActionType":
                        {
                            ActionType = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ActionSubID":
                        {
                            ActionSubID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Ricevuta":
                        {
                            Ricevuta = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "Tag":
                        {
                            Tag = fieldValue;
                            break;
                        }

                    case "AttachmentID":
                        {
                            m_AttachmentID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Attachment":
                        {
                            m_Attachment = (Sistema.CAttachment)fieldValue;
                            break;
                        }
                    default:
                        base.SetFieldInternal(fieldName, fieldValue);
                        break;
                }
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Data", Data);
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", NomeOperatore);
                writer.WriteAttribute("IDCliente", IDCliente);
                writer.WriteAttribute("NomeCliente", NomeCliente);
                writer.WriteAttribute("Scopo", Scopo);
                writer.WriteAttribute("NumeroOIndirizzo", NumeroOIndirizzo);
                writer.WriteAttribute("Esito", (int?)Esito);
                writer.WriteAttribute("DettaglioEsito", DettaglioEsito);
                writer.WriteAttribute("Durata", Durata);
                writer.WriteAttribute("Attesa", Attesa);
                writer.WriteAttribute("StatoConversazione", (int?)StatoConversazione);
                writer.WriteAttribute("ActionID", ActionID);
                writer.WriteAttribute("ActionType", ActionType);
                writer.WriteAttribute("ActionSubID", ActionSubID);
                writer.WriteAttribute("Ricevuta", Ricevuta);
                writer.WriteAttribute("AttachmentID", AttachmentID);
                base.XMLSerialize(writer);
                writer.WriteTag("Note", Note);
                // writer.WriteTag("Tag", Me.Tag)
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(StoricoAction other)
            {
                int ret = -DMD.DateUtils.Compare(Data, other.Data);
                if (ret == 0)
                    ret = DMD.Strings.Compare(NomeCliente, other.NomeCliente, true);
                return ret;
            }

            int IComparable.CompareTo(object obj)
            {
                return this.CompareTo((StoricoAction)obj);
            }

          
        }
    }
}