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
        /// Rappresenta un trasferimento di chiamata, un inoltro di una email o un passaggio del cliente
        /// ad un altro consulente
        /// </summary>
        [Serializable]
        public class TrasferimentoContatto 
            : DMD.XML.DMDBaseXMLObject, IComparable,  IComparable<TrasferimentoContatto>
        {

            private int m_IDOperatore;
            [NonSerialized] private Sistema.CUser m_Operatore;
            private string m_NomeOperatore;
            private int m_IDTrasferitoA;
            [NonSerialized] private Sistema.CUser m_TrasferitoA;
            private string m_NomeTrasferitoA;
            private int m_IDRispostoDa;
            [NonSerialized] private Sistema.CUser m_RispostoDa;
            private string m_NomeRispostoDa;
            private DateTime m_DataTrasferimento;
            private DateTime? m_DataRisposta;
            private int m_IDContatto;
            [NonSerialized] private CContattoUtente m_Contatto;
            private int m_IDPuntoOperativo;
            [NonSerialized] private Anagrafica.CUfficio m_PuntoOperativo;
            private string m_NomePuntoOperativo;
            private int m_IDPuntoOperativoDestinazione;
            [NonSerialized] private Anagrafica.CUfficio m_PuntoOperativoDestinazione;
            private string m_NomePuntoOperativoDestinazione;
            private string m_Messaggio;

            /// <summary>
            /// Costruttore
            /// </summary>
            public TrasferimentoContatto()
            {
               //DMDObject.IncreaseCounter(this);
                m_IDOperatore = 0;
                m_Operatore = null;
                m_NomeOperatore = "";
                m_IDTrasferitoA = 0;
                m_TrasferitoA = null;
                m_NomeTrasferitoA = "";
                m_DataTrasferimento = DMD.DateUtils.Now();
                m_DataRisposta = default;
                m_IDContatto = 0;
                m_Contatto = null;
                m_IDPuntoOperativo = 0;
                m_PuntoOperativo = null;
                m_NomePuntoOperativo = "";
                m_IDPuntoOperativoDestinazione = 0;
                m_PuntoOperativoDestinazione = null;
                m_NomePuntoOperativoDestinazione = "";
                m_IDRispostoDa = 0;
                m_RispostoDa = null;
                m_NomeRispostoDa = "";
                m_Messaggio = "";
                
            }
             
          

            /// <summary>
            /// Restituisce o imposta un messaggio per l'operatore a cui viene trasferito il contatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Messaggio
            {
                get
                {
                    return m_Messaggio;
                }

                set
                {
                    string oldValue = m_Messaggio;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Messaggio = value;
                    DoChanged("Messaggio", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'operatore che ha effettuato il trasferimento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDOperatore
            {
                get
                {
                    return DBUtils.GetID(m_Operatore, m_IDOperatore);
                }

                set
                {
                    int oldValue = IDOperatore;
                    if (oldValue == value)
                        return;
                    m_IDOperatore = value;
                    m_Operatore = null;
                    DoChanged("IDOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'operatore che ha effettuato il trasferimento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser Operatore
            {
                get
                {
                    if (m_Operatore is null)
                        m_Operatore = Sistema.Users.GetItemById(m_IDOperatore);
                    return m_Operatore;
                }

                set
                {
                    var oldValue = Operatore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Operatore = value;
                    m_IDOperatore = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeOperatore = value.Nominativo;
                    DoChanged("Operatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'operatore che ha effettuato il trasferimento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeOperatore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatore = value;
                    DoChanged("NomeOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente a cui è stato trasferito il contatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDTrasferitoA
            {
                get
                {
                    return DBUtils.GetID(m_TrasferitoA, m_IDTrasferitoA);
                }

                set
                {
                    int oldValue = IDTrasferitoA;
                    if (oldValue == value)
                        return;
                    m_IDTrasferitoA = value;
                    m_TrasferitoA = null;
                    DoChanged("IDTrasferitoA", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente a cui è stato trasferito il contatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser TrasferitoA
            {
                get
                {
                    if (m_TrasferitoA is null)
                        m_TrasferitoA = Sistema.Users.GetItemById(m_IDTrasferitoA);
                    return m_TrasferitoA;
                }

                set
                {
                    var oldValue = TrasferitoA;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_TrasferitoA = value;
                    m_IDTrasferitoA = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeTrasferitoA = value.Nominativo;
                    DoChanged("TrasferitoA", value, oldValue);
                }
            }

            /// <summary>
            /// Nome dell'operatore a cui é stato trasferito il contatto
            /// </summary>
            public string NomeTrasferitoA
            {
                get
                {
                    return m_NomeTrasferitoA;
                }

                set
                {
                    string oldValue = m_NomeTrasferitoA;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeTrasferitoA = value;
                    DoChanged("NomeTrasferitoA", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente a cui è stato trasferito il contatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDRispostoDa
            {
                get
                {
                    return DBUtils.GetID(m_RispostoDa, m_IDRispostoDa);
                }

                set
                {
                    int oldValue = IDRispostoDa;
                    if (oldValue == value)
                        return;
                    m_IDRispostoDa = value;
                    m_RispostoDa = null;
                    DoChanged("IDRispostoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente a cui è stato trasferito il contatto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser RispostoDa
            {
                get
                {
                    if (m_RispostoDa is null)
                        m_RispostoDa = Sistema.Users.GetItemById(m_IDRispostoDa);
                    return m_RispostoDa;
                }

                set
                {
                    var oldValue = RispostoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_RispostoDa = value;
                    m_IDRispostoDa = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeRispostoDa = value.Nominativo;
                    DoChanged("RispostoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Nome dell'operatore che ha preso in carico il contatto (può essere diverso dall'operatore a cui era stato trasferito)
            /// </summary>
            public string NomeRispostoDa
            {
                get
                {
                    return m_NomeRispostoDa;
                }

                set
                {
                    string oldValue = m_NomeRispostoDa;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeRispostoDa = value;
                    DoChanged("NomeRispostoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data e l'ora in cui il contatto è stato trasferito
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime DataTrasferimento
            {
                get
                {
                    return m_DataTrasferimento;
                }

                set
                {
                    var oldValue = m_DataTrasferimento;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataTrasferimento = value;
                    DoChanged("DataTrasferimento", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataRisposta
            {
                get
                {
                    return m_DataRisposta;
                }

                set
                {
                    var oldValue = m_DataRisposta;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataRisposta = value;
                    DoChanged("DataRisposta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della telefonata o della visita trasferita
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDContatto
            {
                get
                {
                    return DBUtils.GetID(m_Contatto, m_IDContatto);
                }

                set
                {
                    int oldValue = IDContatto;
                    if (oldValue == value)
                        return;
                    m_IDContatto = value;
                    m_Contatto = null;
                    DoChanged("IDContatto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la telefonata o la visita trasferita
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CContattoUtente Contatto
            {
                get
                {
                    if (m_Contatto is null)
                        m_Contatto =  minidom.CustomerCalls.CRM.GetItemById(m_IDContatto);
                    return m_Contatto;
                }

                set
                {
                    var oldValue = m_Contatto;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Contatto = value;
                    m_IDContatto = DBUtils.GetID(value, 0);
                    DoChanged("Contatto", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del punto operativo di partenza
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDPuntoOperativo
            {
                get
                {
                    return DBUtils.GetID(m_PuntoOperativo, m_IDPuntoOperativo);
                }

                set
                {
                    int oldValue = IDPuntoOperativo;
                    if (oldValue == value)
                        return;
                    m_IDPuntoOperativo = value;
                    m_PuntoOperativo = null;
                    DoChanged("IDPuntoOperativo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il punto operativo di partenza
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CUfficio PuntoOperativo
            {
                get
                {
                    if (m_PuntoOperativo is null)
                        m_PuntoOperativo = Anagrafica.Uffici.GetItemById(m_IDPuntoOperativo);
                    return m_PuntoOperativo;
                }

                set
                {
                    var oldValue = PuntoOperativo;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PuntoOperativo = value;
                    m_IDPuntoOperativo = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomePuntoOperativo = value.Nome;
                    DoChanged("PuntoOperativo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del punto operativo di partenza
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomePuntoOperativo
            {
                get
                {
                    return m_NomePuntoOperativo;
                }

                set
                {
                    string oldValue = m_NomePuntoOperativo;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePuntoOperativo = value;
                    DoChanged("NomePuntoOperativo", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del punto operativo di destinazione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDPuntoOperativoDestinazione
            {
                get
                {
                    return DBUtils.GetID(m_PuntoOperativoDestinazione, m_IDPuntoOperativoDestinazione);
                }

                set
                {
                    int oldValue = IDPuntoOperativoDestinazione;
                    if (oldValue == value)
                        return;
                    m_IDPuntoOperativoDestinazione = value;
                    m_PuntoOperativoDestinazione = null;
                    DoChanged("IDPuntoOperativoDestinazione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce il punto operativo di destinazione da cui ha risposto il contatto
            /// </summary>
            public Anagrafica.CUfficio PuntoOperativoDestinazione
            {
                get
                {
                    if (m_PuntoOperativoDestinazione is null)
                        m_PuntoOperativoDestinazione = Anagrafica.Uffici.GetItemById(m_IDPuntoOperativoDestinazione);
                    return m_PuntoOperativoDestinazione;
                }

                set
                {
                    var oldValue = PuntoOperativoDestinazione;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_PuntoOperativoDestinazione = value;
                    m_IDPuntoOperativoDestinazione = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomePuntoOperativoDestinazione = value.Nome;
                    DoChanged("PuntoOperativoDestinazione", value, oldValue);
                }
            }

            /// <summary>
            /// Nome del punto operativo destinazione
            /// </summary>
            public string NomePuntoOperativoDestinazione
            {
                get
                {
                    return m_NomePuntoOperativoDestinazione;
                }

                set
                {
                    string oldValue = m_NomePuntoOperativoDestinazione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomePuntoOperativoDestinazione = value;
                    DoChanged("NomePuntoOperativoDestinazione", value, oldValue);
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
                    case "IDOperatore":
                        {
                            m_IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatore":
                        {
                            m_NomeOperatore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDTrasferitoA":
                        {
                            m_IDTrasferitoA = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeTrasferitoA":
                        {
                            m_NomeTrasferitoA = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDRispostoDa":
                        {
                            m_IDRispostoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeRispostoDa":
                        {
                            m_NomeRispostoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataTrasferimento":
                        {
                            m_DataTrasferimento = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataRisposta":
                        {
                            m_DataRisposta = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "IDContatto":
                        {
                            m_IDContatto = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDPuntoOperativo":
                        {
                            m_IDPuntoOperativo = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePuntoOperativo":
                        {
                            m_NomePuntoOperativo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDPuntoOperativoDestinazione":
                        {
                            m_IDPuntoOperativoDestinazione = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomePuntoOperativoDestinazione":
                        {
                            m_NomePuntoOperativoDestinazione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Messaggio":
                        {
                            m_Messaggio = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", m_NomeOperatore);
                writer.WriteAttribute("IDTrasferitoA", IDTrasferitoA);
                writer.WriteAttribute("NomeTrasferitoA", m_NomeTrasferitoA);
                writer.WriteAttribute("IDRispostoDa", IDRispostoDa);
                writer.WriteAttribute("NomeRispostoDa", m_NomeRispostoDa);
                writer.WriteAttribute("DataTrasferimento", m_DataTrasferimento);
                writer.WriteAttribute("DataRisposta", m_DataRisposta);
                writer.WriteAttribute("IDContatto", IDContatto);
                writer.WriteAttribute("IDPuntoOperativo", IDPuntoOperativo);
                writer.WriteAttribute("NomePuntoOperativo", m_NomePuntoOperativo);
                writer.WriteAttribute("IDPuntoOperativoDestinazione", IDPuntoOperativoDestinazione);
                writer.WriteAttribute("NomePuntoOperativoDestinazione", m_NomePuntoOperativoDestinazione);
                base.XMLSerialize(writer);
                writer.WriteTag("Messaggio", m_Messaggio);
            }

            /// <summary>
            /// Compara due oggetti sulla base della data di trasferimento
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(TrasferimentoContatto other)
            {
                return DMD.DateUtils.Compare(m_DataTrasferimento, other.m_DataTrasferimento);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((TrasferimentoContatto)obj);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray("Trasferimento da: ", this.m_NomeOperatore, " a ", this.m_NomeTrasferitoA, " (", this.m_DataTrasferimento , ")");
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return DMD.HashCalculator.Calculate(this.m_DataTrasferimento);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(DMDBaseXMLObject obj)
            {
                return (obj is TrasferimentoContatto) && this.Equals((TrasferimentoContatto)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(TrasferimentoContatto obj)
            {
                return base.Equals(obj)
                        && DMD.Integers.EQ(this.m_IDOperatore, obj.m_IDOperatore)
                        && DMD.Strings.EQ(this.m_NomeOperatore, obj.m_NomeOperatore)
                        && DMD.Integers.EQ(this.m_IDTrasferitoA, obj.m_IDTrasferitoA)
                        && DMD.Strings.EQ(this.m_NomeTrasferitoA, obj.m_NomeTrasferitoA)
                        && DMD.Integers.EQ(this.m_IDRispostoDa, obj.m_IDRispostoDa)
                        && DMD.Strings.EQ(this.m_NomeRispostoDa, obj.m_NomeRispostoDa)
                        && DMD.DateUtils.EQ(this.m_DataTrasferimento, obj.m_DataTrasferimento)
                        && DMD.DateUtils.EQ(this.m_DataRisposta, obj.m_DataRisposta)
                        && DMD.Integers.EQ(this.m_IDContatto, obj.m_IDContatto)
                        && DMD.Integers.EQ(this.m_IDPuntoOperativo, obj.m_IDPuntoOperativo)
                        && DMD.Strings.EQ(this.m_NomePuntoOperativo, obj.m_NomePuntoOperativo)
                        && DMD.Integers.EQ(this.m_IDPuntoOperativoDestinazione, obj.m_IDPuntoOperativoDestinazione)
                        && DMD.Strings.EQ(this.m_NomePuntoOperativoDestinazione, obj.m_NomePuntoOperativoDestinazione)
                        && DMD.Strings.EQ(this.m_Messaggio, obj.m_Messaggio)
                        ;
            }

        }
    }
}
