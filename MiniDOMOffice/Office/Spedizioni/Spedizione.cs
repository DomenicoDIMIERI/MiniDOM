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
        /// Stato di una spedizione <see cref="Spedizione"/>
        /// </summary>
        public enum StatoSpedizione : int
        {
            /// <summary>
            /// Oggetto in preparazione, non ancora spedito
            /// </summary>
            /// <remarks></remarks>
            InPreparazione = 0,

            /// <summary>
            /// L'oggetto è stato dato nelle mani del corriere
            /// </summary>
            /// <remarks></remarks>
            Spedita = 10,

            /// <summary>
            /// L'oggetto è stato stoccato nel magazzino del corriere
            /// </summary>
            /// <remarks></remarks>
            FermaInMagazzino = 20,

            /// <summary>
            /// L'ggetto è stato spedito ad un altro magazzino del corriere
            /// </summary>
            /// <remarks></remarks>
            TrasferimentoMagazzino = 30,

            /// <summary>
            /// L'oggetto è in consegna (dal magazzino del corriere al destinatario)
            /// </summary>
            /// <remarks></remarks>
            InConsegna = 40,

            /// <summary>
            /// C'è stato un problema nella consegna dell'oggetto (vedi flags) e
            /// </summary>
            /// <remarks></remarks>
            Fallita = 50
        }

        /// <summary>
        /// Stato di consegna di una <see cref="Spedizione"/>
        /// </summary>
        public enum StatoConsegna : int
        {
            /// <summary>
            /// Non consegnato
            /// </summary>
            /// <remarks></remarks>
            NonConsegnata = 0,

            /// <summary>
            /// Consegnato
            /// </summary>
            /// <remarks></remarks>
            Consegnata = 1,

            /// <summary>
            /// Indirizzo del destinatario non corretto
            /// </summary>
            /// <remarks></remarks>
            IndirizzoDestinatarioErrato = 2,

            /// <summary>
            /// Il destinatario non era all'indirizzo indicato
            /// </summary>
            /// <remarks></remarks>
            DestinatarioNonTrovato = 3,

            /// <summary>
            /// Il destinatario ha rifiutato la consegna
            /// </summary>
            /// <remarks></remarks>
            DestinatarioRifiuto = 4
        }


        /// <summary>
        /// Flags validi per una <see cref="Spedizione"/>
        /// </summary>
        [Flags]
        public enum SpedizioneFlags
            : int
        {

            /// <summary>
            /// Nessuno
            /// </summary>
            None = 0,

            /// <summary>
            /// Spedizione effettuata
            /// </summary>
            Effettuata = 1
        }


        /// <summary>
        /// Rappresenta una spedizione effettuata tramite corriere
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class Spedizione
            : Databases.DBObjectPO, IComparable, IComparable<Spedizione>
        {
            private string m_AspettoBeni;                 // Indica il tipo di oggetto spedito (busta, pacco, pallet)
            private int m_IDMittente;                 // Persona che invia l'oggetto
            [NonSerialized] private CPersona m_Mittente;
            private string m_NomeMittente;
            private Anagrafica.CIndirizzo m_IndirizzoMittente;       // Indirizzo da cui è partita la spedizione
            private int m_IDDestinatario;             // Persona a cui è destinato l'oggetto
            [NonSerialized] private CPersona m_Destinatario;
            private string m_NomeDestinatario;
            private CIndirizzo m_IndirizzoDestinatario;   // Indirizzo a cui inviare l'oggetto
            private int? m_NumeroColli;                // Numero di colli spediti
            private double? m_Peso;                       // Peso degli oggetti spediti
            private double? m_Altezza;                    // Altezza
            private double? m_Larghezza;                  // Larghezza
            private double? m_Profondita;                // Profondita
            [NonSerialized] private CUser m_SpeditoDa;                    // Operatore che ha effettuato la spedizione
            private int m_IDSpeditoDa;
            private string m_NomeSpeditoDa;
            [NonSerialized] private CUser m_RicevutoDa;                    // Operatore che ha effettuato la spedizione
            private int m_IDRicevutoDa;
            private string m_NomeRicevutoDa;
            private DateTime? m_DataInizioSpedizione;
            private string m_NotePerIlCorriere;
            private string m_NotePerIlDestinatario;
            private StatoSpedizione m_StatoSpedizione;
            private StatoConsegna m_StatoConsegna;
            private DateTime? m_DataConsegna;
            private string m_NomeCorriere;
            private int m_IDCorriere;
            private string m_NumeroSpedizione;
            private CCollection<PassaggioSpedizione> m_Passaggi;
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public Spedizione()
            {
                m_AspettoBeni = "";
                m_IDMittente = 0;
                m_Mittente = null;
                m_NomeMittente = "";
                m_IndirizzoMittente = new Anagrafica.CIndirizzo();
                m_IDDestinatario = 0;
                m_Destinatario = null;
                m_NomeDestinatario = "";
                m_IndirizzoDestinatario = new Anagrafica.CIndirizzo();
                m_NumeroColli = default;
                m_Peso = default;
                m_Altezza = default;
                m_Larghezza = default;
                m_Profondita = default;
                m_SpeditoDa = null;
                m_IDSpeditoDa = 0;
                m_NomeSpeditoDa = "";
                m_DataInizioSpedizione = default;
                m_NotePerIlCorriere = "";
                m_NotePerIlDestinatario = "";
                m_StatoSpedizione = StatoSpedizione.InPreparazione;
                m_StatoConsegna = StatoConsegna.NonConsegnata;
                m_DataConsegna = default;
                m_Flags = (int) SpedizioneFlags.Effettuata;
                m_NomeCorriere = "";
                m_IDCorriere = 0;
                m_NumeroSpedizione = "";
                m_Passaggi = null;
            }

            /// <summary>
            /// Restituisce o imposta una stringa che descrive l'aspetto degli oggetti spediti (Buste, Pacchi, Pallet, )
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string AspettoBeni
            {
                get
                {
                    return m_AspettoBeni;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_AspettoBeni;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_AspettoBeni = value;
                    DoChanged("AspettoBeni", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona che ha inviato l'oggetto (mittente)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDMittente
            {
                get
                {
                    return DBUtils.GetID(m_Mittente, m_IDMittente);
                }

                set
                {
                    string oldValue = IDMittente.ToString();
                    if (DMD.Doubles.CDbl(oldValue) == value)
                        return;
                    m_IDMittente = value;
                    m_Mittente = null;
                    DoChanged("IDMittente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la persona che ha inviato l'oggetto (mittente)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CPersona Mittente
            {
                get
                {
                    if (m_Mittente is null)
                        m_Mittente = Anagrafica.Persone.GetItemById(m_IDMittente);
                    return m_Mittente;
                }

                set
                {
                    var oldValue = m_Mittente;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Mittente = value;
                    m_IDMittente = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeMittente = value.Nominativo;
                    DoChanged("Mittente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del mittente
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeMittente
            {
                get
                {
                    return m_NomeMittente;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeMittente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeMittente = value;
                    DoChanged("NomeMittente", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'oggetto che descrive l'indirizzo del mittente (da cui parte la spedizione)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CIndirizzo IndirizzoMittente
            {
                get
                {
                    return m_IndirizzoMittente;
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID della persona a cui è destinata la spedizione (Destinatario)
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDDestinatario
            {
                get
                {
                    return DBUtils.GetID(m_Destinatario, m_IDDestinatario);
                }

                set
                {
                    int oldValue = IDDestinatario;
                    if (oldValue == value)
                        return;
                    m_Destinatario = null;
                    m_IDDestinatario = value;
                    DoChanged("IDDestinatario", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la persona a cui è destinata la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CPersona Destinatario
            {
                get
                {
                    if (m_Destinatario is null)
                        m_Destinatario = Anagrafica.Persone.GetItemById(m_IDDestinatario);
                    return m_Destinatario;
                }

                set
                {
                    var oldValue = m_Destinatario;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Destinatario = value;
                    m_IDDestinatario = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeDestinatario = value.Nominativo;
                    DoChanged("Destinatario", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della persoan a cui è destinata la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeDestinatario
            {
                get
                {
                    return m_NomeDestinatario;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeDestinatario;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeDestinatario = value;
                    DoChanged("NomeDestinatario", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce l'oggetto che descrive l'indirizzo presso cui consegnare la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Anagrafica.CIndirizzo IndirizzoDestinatario
            {
                get
                {
                    return m_IndirizzoDestinatario;
                }
            }

            /// <summary>
            /// Restituisce o imosta il numero di colli della spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int? NumeroColli
            {
                get
                {
                    return m_NumeroColli;
                }

                set
                {
                    var oldValue = m_NumeroColli;
                    if (oldValue == value == true)
                        return;
                    m_NumeroColli = value;
                    DoChanged("NumeroColli", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il peso totale degli oggetti spediti
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double? Peso
            {
                get
                {
                    return m_Peso;
                }

                set
                {
                    double oldValue = (double)m_Peso;
                    if (oldValue == value == true)
                        return;
                    m_Peso = value;
                    DoChanged("Peso", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'altezza degli oggetti spediti
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double? Altezza
            {
                get
                {
                    return m_Altezza;
                }

                set
                {
                    var oldValue = m_Altezza;
                    if (oldValue == value == true)
                        return;
                    m_Altezza = value;
                    DoChanged("Altezza", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la larghezza degli oggetti spediti
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double? Larghezza
            {
                get
                {
                    return m_Larghezza;
                }

                set
                {
                    double oldValue = (double)m_Larghezza;
                    if (oldValue == value == true)
                        return;
                    m_Larghezza = value;
                    DoChanged("Larghezza", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la profondità degli oggetti spediti
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public double? Profondita
            {
                get
                {
                    return m_Profondita;
                }

                set
                {
                    var oldValue = m_Profondita;
                    if (oldValue == value == true)
                        return;
                    m_Profondita = value;
                    DoChanged("Profondita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha effettuato la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser SpeditoDa
            {
                get
                {
                    if (m_SpeditoDa is null)
                        m_SpeditoDa = Sistema.Users.GetItemById(m_IDSpeditoDa);
                    return m_SpeditoDa;
                }

                set
                {
                    var oldValue = SpeditoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_SpeditoDa = value;
                    m_IDSpeditoDa = DBUtils.GetID(value, 0);
                    if (value is object)
                        m_NomeSpeditoDa = value.Nominativo;
                    DoChanged("SpeditoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che ha effettuato la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDSpeditoDa
            {
                get
                {
                    return DBUtils.GetID(m_SpeditoDa, m_IDSpeditoDa);
                }

                set
                {
                    int oldValue = IDSpeditoDa;
                    if (oldValue == value)
                        return;
                    m_IDSpeditoDa = value;
                    m_SpeditoDa = null;
                    DoChanged("IDSpeditoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha effettuato la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeSpeditoDa
            {
                get
                {
                    return m_NomeSpeditoDa;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeSpeditoDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeSpeditoDa = value;
                    DoChanged("NomeSpeditoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'utente che ha ricevuto la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public Sistema.CUser RicevutoDa
            {
                get
                {
                    if (m_RicevutoDa is null)
                        m_RicevutoDa = Sistema.Users.GetItemById(m_IDRicevutoDa);
                    return m_RicevutoDa;
                }

                set
                {
                    var oldValue = RicevutoDa;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_RicevutoDa = value;
                    m_IDRicevutoDa = DBUtils.GetID(value, 0);
                    if (value is object)
                    {
                        m_NomeRicevutoDa = value.Nominativo;
                    }

                    DoChanged("RicevutoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID dell'utente che ha ricevuto la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDRicevutoDa
            {
                get
                {
                    return DBUtils.GetID(m_RicevutoDa, m_IDRicevutoDa);
                }

                set
                {
                    int oldValue = IDRicevutoDa;
                    if (oldValue == value)
                        return;
                    m_IDRicevutoDa = value;
                    m_RicevutoDa = null;
                    DoChanged("IDRicevutoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente che ha ricevuto la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeRicevutoDa
            {
                get
                {
                    return m_NomeRicevutoDa;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeRicevutoDa;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeRicevutoDa = value;
                    DoChanged("NomeRicevutoDa", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di inizio della spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataInizioSpedizione
            {
                get
                {
                    return m_DataInizioSpedizione;
                }

                set
                {
                    var oldValue = m_DataInizioSpedizione;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataInizioSpedizione = value;
                    DoChanged("DataInizioSpedizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa contenente degli avvertimenti diretti al corriere
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NotePerIlCorriere
            {
                get
                {
                    return m_NotePerIlCorriere;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NotePerIlCorriere;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NotePerIlCorriere = value;
                    DoChanged("NotePerIlCorriere", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta delle annotazioni dirette al destinatario
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NotePerIlDestinatario
            {
                get
                {
                    return m_NotePerIlDestinatario;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NotePerIlDestinatario;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NotePerIlDestinatario = value;
                    DoChanged("NotePerIlDestinatario", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato della spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoSpedizione StatoSpedizione
            {
                get
                {
                    return m_StatoSpedizione;
                }

                set
                {
                    var oldValue = m_StatoSpedizione;
                    if (oldValue == value)
                        return;
                    m_StatoSpedizione = value;
                    DoChanged("StatoSpedizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta lo stato della consegna
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public StatoConsegna StatoConsegna
            {
                get
                {
                    return m_StatoConsegna;
                }

                set
                {
                    var oldValue = m_StatoConsegna;
                    if (oldValue == value)
                        return;
                    m_StatoConsegna = value;
                    DoChanged("StatoConsegna", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la data di consegna
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public DateTime? DataConsegna
            {
                get
                {
                    return m_DataConsegna;
                }

                set
                {
                    var oldValue = m_DataConsegna;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_DataConsegna = value;
                    DoChanged("DataConsegna", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta dei flags
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public new SpedizioneFlags Flags
            {
                get
                {
                    return (SpedizioneFlags)this.m_Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta l'ID del corriere utilizzato per la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int IDCorriere
            {
                get
                {
                    return m_IDCorriere;
                }

                set
                {
                    int oldValue = IDCorriere;
                    if (oldValue == value)
                        return;
                    m_IDCorriere = value;
                    DoChanged("IDCorriere", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome del corriere utilizzato per la spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NomeCorriere
            {
                get
                {
                    return m_NomeCorriere;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeCorriere;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCorriere = value;
                    DoChanged("NomeCorriere", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il numero della spedizione
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string NumeroSpedizione
            {
                get
                {
                    return m_NumeroSpedizione;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NumeroSpedizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NumeroSpedizione = value;
                    DoChanged("NumeroSpedizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la sequenza dei passaggi di stato
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<PassaggioSpedizione> Passaggi
            {
                get
                {
                    if (m_Passaggi is null)
                        m_Passaggi = new CCollection<PassaggioSpedizione>();
                    return m_Passaggi;
                }
            }
              
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Office.Spedizioni;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeSpedizioni";
            }

            /// <summary>
            /// Restituisce true se l'oggetto é stato modifiato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                return base.IsChanged() 
                    || m_IndirizzoDestinatario.IsChanged() 
                    || m_IndirizzoMittente.IsChanged();
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                m_IndirizzoDestinatario.SetChanged(false);
                m_IndirizzoMittente.SetChanged(false);
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_AspettoBeni = reader.Read("AspettoBeni", m_AspettoBeni);
                m_IDMittente = reader.Read("IDMittente", m_IDMittente);
                m_NomeMittente = reader.Read("NomeMittente", m_NomeMittente);
                {
                    var withBlock = m_IndirizzoMittente;
                    withBlock.Nome = reader.Read("IndirizzoMittente_Nome", withBlock.Nome);
                    withBlock.ToponimoViaECivico = reader.Read("IndirizzoMittente_Via", withBlock.ToponimoViaECivico);
                    withBlock.CAP = reader.Read("IndirizzoMittente_CAP", withBlock.CAP);
                    withBlock.Citta = reader.Read("IndirizzoMittente_Citta", withBlock.Citta);
                    withBlock.Provincia = reader.Read("IndirizzoMittente_Provincia", withBlock.Provincia);
                    withBlock.SetChanged(false);
                }

                m_IDDestinatario = reader.Read("IDDestinatario",  m_IDDestinatario);
                m_NomeDestinatario = reader.Read("NomeDestinatario",  m_NomeDestinatario);
                {
                    var withBlock1 = m_IndirizzoDestinatario;
                    withBlock1.Nome = reader.Read("IndirizzoDest_Nome", withBlock1.Nome);
                    withBlock1.ToponimoViaECivico = reader.Read("IndirizzoDest_Via", withBlock1.ToponimoViaECivico);
                    withBlock1.CAP = reader.Read("IndirizzoDest_CAP", withBlock1.CAP);
                    withBlock1.Citta = reader.Read("IndirizzoDest_Citta", withBlock1.Citta);
                    withBlock1.Provincia = reader.Read("IndirizzoDest_Provincia", withBlock1.Provincia);
                    withBlock1.SetChanged(false);
                }

                m_NumeroColli = reader.Read("NumeroColli",  m_NumeroColli);
                m_Peso = reader.Read("Peso",  m_Peso);
                m_Altezza = reader.Read("Altezza",  m_Altezza);
                m_Larghezza = reader.Read("Larghezza",  m_Larghezza);
                m_Profondita = reader.Read("Profondita",  m_Profondita);
                m_IDSpeditoDa = reader.Read("IDSpeditoDa",  m_IDSpeditoDa);
                m_NomeSpeditoDa = reader.Read("NomeSpeditoDa",  m_NomeSpeditoDa);
                m_IDRicevutoDa = reader.Read("IDRicevutoDa",  m_IDRicevutoDa);
                m_NomeRicevutoDa = reader.Read("NomeRicevutoDa",  m_NomeRicevutoDa);
                m_DataInizioSpedizione = reader.Read("DataInizioSpedizione",  m_DataInizioSpedizione);
                m_NotePerIlCorriere = reader.Read("NotePerIlCorriere",  m_NotePerIlCorriere);
                m_NotePerIlDestinatario = reader.Read("NotePerIlDestinatario",  m_NotePerIlDestinatario);
                m_StatoSpedizione = reader.Read("StatoSpedizione",  m_StatoSpedizione);
                m_StatoConsegna = reader.Read("StatoConsegna",  m_StatoConsegna);
                m_DataConsegna = reader.Read("DataConsegna",  m_DataConsegna);
                m_Flags = reader.Read("Flags",  m_Flags);
                m_NomeCorriere = reader.Read("NomeCorriere",  m_NomeCorriere);
                m_IDCorriere = reader.Read("IDCorriere",  m_IDCorriere);
                m_NumeroSpedizione = reader.Read("NumeroSpedizione",  m_NumeroSpedizione);
                try
                {
                    string argvalue10 = "";
                    m_Passaggi = (CCollection<PassaggioSpedizione>)DMD.XML.Utils.Serializer.Deserialize(reader.Read("Passaggi", argvalue10));
                }
                catch (Exception ex)
                {
                    m_Passaggi = new CCollection<PassaggioSpedizione>();
                }

                try
                {
                    string argvalue11 = "";
                    m_Attributi = (CKeyCollection)DMD.XML.Utils.Serializer.Deserialize(reader.Read("Attributi",  argvalue11));
                }
                catch (Exception ex)
                {
                    m_Attributi = new CKeyCollection();
                }

                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("AspettoBeni", m_AspettoBeni);
                writer.Write("IDMittente", IDMittente);
                writer.Write("NomeMittente", m_NomeMittente);
                {
                    var withBlock = m_IndirizzoMittente;
                    writer.Write("IndirizzoMittente_Nome", withBlock.Nome);
                    writer.Write("IndirizzoMittente_Via", withBlock.ToponimoViaECivico);
                    writer.Write("IndirizzoMittente_CAP", withBlock.CAP);
                    writer.Write("IndirizzoMittente_Citta", withBlock.Citta);
                    writer.Write("IndirizzoMittente_Provincia", withBlock.Provincia);
                }

                writer.Write("IDDestinatario", IDDestinatario);
                writer.Write("NomeDestinatario", m_NomeDestinatario);
                {
                    var withBlock1 = m_IndirizzoDestinatario;
                    writer.Write("IndirizzoDest_Nome", withBlock1.Nome);
                    writer.Write("IndirizzoDest_Via", withBlock1.ToponimoViaECivico);
                    writer.Write("IndirizzoDest_CAP", withBlock1.CAP);
                    writer.Write("IndirizzoDest_Citta", withBlock1.Citta);
                    writer.Write("IndirizzoDest_Provincia", withBlock1.Provincia);
                }

                writer.Write("NumeroColli", m_NumeroColli);
                writer.Write("Peso", m_Peso);
                writer.Write("Altezza", m_Altezza);
                writer.Write("Larghezza", m_Larghezza);
                writer.Write("Profondita", m_Profondita);
                writer.Write("IDSpeditoDa", IDSpeditoDa);
                writer.Write("NomeSpeditoDa", m_NomeSpeditoDa);
                writer.Write("IDRicevutoDa", IDRicevutoDa);
                writer.Write("NomeRicevutoDa", m_NomeRicevutoDa);
                writer.Write("DataInizioSpedizione", m_DataInizioSpedizione);
                writer.Write("NotePerIlCorriere", m_NotePerIlCorriere);
                writer.Write("NotePerIlDestinatario", m_NotePerIlDestinatario);
                writer.Write("StatoSpedizione", m_StatoSpedizione);
                writer.Write("StatoConsegna", m_StatoConsegna);
                writer.Write("DataConsegna", m_DataConsegna);
                writer.Write("Flags", m_Flags);
                writer.Write("NomeCorriere", m_NomeCorriere);
                writer.Write("IDCorriere", IDCorriere);
                writer.Write("NumeroSpedizione", m_NumeroSpedizione);
                writer.Write("Passaggi", DMD.XML.Utils.Serializer.Serialize(Passaggi));
                writer.Write("Attributi", DMD.XML.Utils.Serializer.Serialize(Attributi));
                return base.SaveToRecordset(writer);
            }


            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("AspettoBeni", typeof(string), 255);
                c = table.Fields.Ensure("IDMittente", typeof(int), 1);
                c = table.Fields.Ensure("NomeMittente", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoMittente_Nome", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoMittente_Via", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoMittente_CAP", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoMittente_Citta", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoMittente_Provincia", typeof(string), 255);
                c = table.Fields.Ensure("IDDestinatario", typeof(int), 1);
                c = table.Fields.Ensure("NomeDestinatario", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoDest_Nome", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoDest_Via", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoDest_CAP", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoDest_Citta", typeof(string), 255);
                c = table.Fields.Ensure("IndirizzoDest_Provincia", typeof(string), 255);
                c = table.Fields.Ensure("NumeroColli", typeof(int), 1);
                c = table.Fields.Ensure("Peso", typeof(double), 1);
                c = table.Fields.Ensure("Altezza", typeof(double), 1);
                c = table.Fields.Ensure("Larghezza", typeof(double), 1);
                c = table.Fields.Ensure("Profondita", typeof(double), 1);
                c = table.Fields.Ensure("IDSpeditoDa", typeof(int), 1);
                c = table.Fields.Ensure("NomeSpeditoDa", typeof(string), 255);
                c = table.Fields.Ensure("IDRicevutoDa", typeof(int), 1);
                c = table.Fields.Ensure("NomeRicevutoDa", typeof(string), 255);
                c = table.Fields.Ensure("DataInizioSpedizione", typeof(DateUtils), 1);
                c = table.Fields.Ensure("NotePerIlCorriere", typeof(string), 0);
                c = table.Fields.Ensure("NotePerIlDestinatario", typeof(string), 0);
                c = table.Fields.Ensure("StatoSpedizione", typeof(int), 1);
                c = table.Fields.Ensure("StatoConsegna", typeof(int), 1);
                c = table.Fields.Ensure("DataConsegna", typeof(DateUtils), 1);
                c = table.Fields.Ensure("NomeCorriere", typeof(string), 255);
                c = table.Fields.Ensure("IDCorriere", typeof(int), 1);
                c = table.Fields.Ensure("NumeroSpedizione", typeof(string), 255);
                c = table.Fields.Ensure("Passaggi", typeof(string), 0);
                c = table.Fields.Ensure("Attributi", typeof(string), 0);
                 
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxAspetto", new string[] { "AspettoBeni" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxMittente", new string[] { "IDMittente", "NomeMittente" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndMittente", new string[] { "IndirizzoMittente_Nome", "IndirizzoMittente_Via", "IndirizzoMittente_CAP", "IndirizzoMittente_Citta", "IndirizzoMittente_Provincia"  }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDestinatario", new string[] { "IDDestinatario", "NomeDestinatario" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxIndDestinatario", new string[] { "IndirizzoDest_Nome", "IndirizzoDest_Via", "IndirizzoDest_CAP", "IndirizzoDest_Citta", "IndirizzoDest_Provincia" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxParams", new string[] { "NumeroColli", "Peso", "Altezza", "Larghezza", "Profondita" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxSpeditoA", new string[] { "IDSpeditoDa", "NomeSpeditoDa" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxRicevutoDa", new string[] { "IDRicevutoDa", "NomeRicevutoDa" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDataSped", new string[] { "DataInizioSpedizione", "StatoSpedizione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNotePerCo", new string[] { "NotePerIlCorriere" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNotePerDe", new string[] { "NotePerIlDestinatario" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxDataCons", new string[] { "DataConsegna", "StatoConsegna" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCorriere", new string[] { "IDCorriere", "NomeCorriere" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxNumeroSped", new string[] { "NumeroSpedizione" }, DBFieldConstraintFlags.None);

                //c = table.Fields.Ensure("Passaggi", typeof(string), 0);
                //c = table.Fields.Ensure("Attributi", typeof(string), 0);

            }


            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("AspettoBeni", m_AspettoBeni);
                writer.WriteAttribute("IDMittente", IDMittente);
                writer.WriteAttribute("NomeMittente", m_NomeMittente);
                writer.WriteAttribute("IDDestinatario", IDDestinatario);
                writer.WriteAttribute("NomeDestinatario", m_NomeDestinatario);
                writer.WriteAttribute("NumeroColli", m_NumeroColli);
                writer.WriteAttribute("Peso", m_Peso);
                writer.WriteAttribute("Altezza", m_Altezza);
                writer.WriteAttribute("Larghezza", m_Larghezza);
                writer.WriteAttribute("Profondita", m_Profondita);
                writer.WriteAttribute("IDSpeditoDa", IDSpeditoDa);
                writer.WriteAttribute("NomeSpeditoDa", m_NomeSpeditoDa);
                writer.WriteAttribute("IDRicevutoDa", IDRicevutoDa);
                writer.WriteAttribute("NomeRicevutoDa", m_NomeRicevutoDa);
                writer.WriteAttribute("DataInizioSpedizione", m_DataInizioSpedizione);
                writer.WriteAttribute("StatoSpedizione", (int?)m_StatoSpedizione);
                writer.WriteAttribute("StatoConsegna", (int?)m_StatoConsegna);
                writer.WriteAttribute("DataConsegna", m_DataConsegna);
                writer.WriteAttribute("Flags", (int?)m_Flags);
                writer.WriteAttribute("NomeCorriere", m_NomeCorriere);
                writer.WriteAttribute("IDCorriere", m_IDCorriere);
                writer.WriteAttribute("NumeroSpedizione", m_NumeroSpedizione);
                base.XMLSerialize(writer);
                writer.WriteTag("IndirizzoMittente", m_IndirizzoMittente);
                writer.WriteTag("IndirizzoDestinatario", m_IndirizzoDestinatario);
                writer.WriteTag("Passaggi", Passaggi);
                writer.WriteTag("Attributi", Attributi);
                writer.WriteTag("NotePerIlCorriere", m_NotePerIlCorriere);
                writer.WriteTag("NotePerIlDestinatario", m_NotePerIlDestinatario);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "AspettoBeni":
                        {
                            m_AspettoBeni = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDMittente":
                        {
                            m_IDMittente = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeMittente":
                        {
                            m_NomeMittente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDDestinatario":
                        {
                            m_IDDestinatario = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeDestinatario":
                        {
                            m_NomeDestinatario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NumeroColli":
                        {
                            m_NumeroColli = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Peso":
                        {
                            m_Peso = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Altezza":
                        {
                            m_Altezza = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Larghezza":
                        {
                            m_Larghezza = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "Profondita":
                        {
                            m_Profondita = DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "IDSpeditoDa":
                        {
                            m_IDSpeditoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeSpeditoDa":
                        {
                            m_NomeSpeditoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDRicevutoDa":
                        {
                            m_IDRicevutoDa = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeRicevutoDa":
                        {
                            m_NomeRicevutoDa = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataInizioSpedizione":
                        {
                            m_DataInizioSpedizione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "StatoSpedizione":
                        {
                            m_StatoSpedizione = (StatoSpedizione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoConsegna":
                        {
                            m_StatoConsegna = (StatoConsegna)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DataConsegna":
                        {
                            m_DataConsegna = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Flags":
                        {
                            m_Flags = (SpedizioneFlags)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeCorriere":
                        {
                            m_NomeCorriere = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IDCorriere":
                        {
                            m_IDCorriere = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NumeroSpedizione":
                        {
                            m_NumeroSpedizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IndirizzoMittente":
                        {
                            m_IndirizzoMittente = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }

                    case "IndirizzoDestinatario":
                        {
                            m_IndirizzoDestinatario = (Anagrafica.CIndirizzo)fieldValue;
                            break;
                        }

                    case "Passaggi":
                        {
                            Passaggi.Clear();
                            Passaggi.AddRange((IEnumerable)fieldValue);
                            break;
                        }

                    case "Attributi":
                        {
                            m_Attributi = (CKeyCollection)fieldValue;
                            break;
                        }

                    case "NotePerIlCorriere":
                        {
                            m_NotePerIlCorriere = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NotePerIlDestinatario":
                        {
                            m_NotePerIlDestinatario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Compara due oggetti
            /// </summary>
            /// <param name="b"></param>
            /// <returns></returns>
            public int CompareTo(Spedizione b)
            {
                return DMD.DateUtils.Compare(m_DataInizioSpedizione, b.m_DataInizioSpedizione);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((Spedizione)obj);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(
                        "Spedizione " , m_NomeCorriere ,
                        " del " , Sistema.Formats.FormatUserDateTime(m_DataInizioSpedizione)
                        );
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_IDDestinatario);
            }

            /// <summary>
            /// Restitusice true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is Spedizione) && this.Equals((Spedizione)obj);
            }

            /// <summary>
            /// Restitusice true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(Spedizione obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_AspettoBeni, obj.m_AspettoBeni)
                    && DMD.Integers.EQ(this.m_IDMittente, obj.m_IDMittente)
                    && DMD.Strings.EQ(this.m_NomeMittente, obj.m_NomeMittente)
                    && this.m_IndirizzoMittente.Equals(obj.m_IndirizzoMittente)
                    && DMD.Integers.EQ(this.m_IDDestinatario, obj.m_IDDestinatario)
                    && DMD.Strings.EQ(this.m_NomeDestinatario, obj.m_NomeDestinatario)
                    && this.m_IndirizzoDestinatario.Equals(obj.m_IndirizzoDestinatario)
                    && DMD.Integers.EQ(this.m_NumeroColli, obj.m_NumeroColli)
                    && DMD.Doubles.EQ(this.m_Peso, obj.m_Peso)
                    && DMD.Doubles.EQ(this.m_Altezza, obj.m_Altezza)
                    && DMD.Doubles.EQ(this.m_Larghezza, obj.m_Larghezza)
                    && DMD.Doubles.EQ(this.m_Profondita, obj.m_Profondita)
                    && DMD.Integers.EQ(this.m_IDSpeditoDa, obj.m_IDSpeditoDa)
                    && DMD.Strings.EQ(this.m_NomeSpeditoDa, obj.m_NomeSpeditoDa)
                    && DMD.Integers.EQ(this.m_IDRicevutoDa, obj.m_IDRicevutoDa)
                    && DMD.Strings.EQ(this.m_NomeRicevutoDa, obj.m_NomeRicevutoDa)
                    && DMD.DateUtils.EQ(this.m_DataInizioSpedizione, obj.m_DataInizioSpedizione)
                    && DMD.Strings.EQ(this.m_NotePerIlCorriere, obj.m_NotePerIlCorriere)
                    && DMD.Strings.EQ(this.m_NotePerIlDestinatario, obj.m_NotePerIlDestinatario)
                    && DMD.RunTime.EQ(this.m_StatoSpedizione, obj.m_StatoSpedizione)
                    && DMD.RunTime.EQ(this.m_StatoConsegna, obj.m_StatoConsegna)
                    && DMD.DateUtils.EQ(this.m_DataConsegna, obj.m_DataConsegna)
                    && DMD.Strings.EQ(this.m_NomeCorriere, obj.m_NomeCorriere)
                    && DMD.Integers.EQ(this.m_IDCorriere, obj.m_IDCorriere)
                    && DMD.Strings.EQ(this.m_NumeroSpedizione, obj.m_NumeroSpedizione)
                    ;
            //private CCollection<PassaggioSpedizione> m_Passaggi;

            }
        }
    }
}