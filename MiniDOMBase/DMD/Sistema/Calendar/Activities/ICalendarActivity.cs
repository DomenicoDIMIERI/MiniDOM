using System;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Stato di un'attività del calendario
        /// </summary>
        public enum StatoAttivita : int
        {
            /// <summary>
            /// In attesa
            /// </summary>
            INATTESA = 0,

            /// <summary>
            /// In corso
            /// </summary>
            INCORSO = 1,

            /// <summary>
            /// Conclusa
            /// </summary>
            CONCLUSA = 2
        }


        /// <summary>
        /// Flag validi per un'attività del calendario
        /// </summary>
        [Flags]
        public enum CalendarActivityFlags : int
        {
            /// <summary>
            /// Nessuno
            /// </summary>
            None = 0,

            /// <summary>
            /// Modificabile
            /// </summary>
            CanEdit = 1,

            /// <summary>
            /// Eliminabile
            /// </summary>
            CanDelete = 2,

            /// <summary>
            /// E' possibile eseguire delle azioni sull'attività
            /// </summary>
            IsAction = 4
        }



        /// <summary>
        /// Interfaccia implementata da un'oggetto che sfrutta il calendario per la gestione degli eventi
        /// </summary>
        /// <remarks></remarks>
        public interface ICalendarActivity 
            : IComparable
        {

            /// <summary>
            /// Restituisce o imposta i flag
            /// </summary>
            CalendarActivityFlags Flags { get; set; }

            /// <summary>
            /// Restituisce o imposta la categoria
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            string Categoria { get; set; }

            /// <summary>
            /// Stato dell'attività
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            StatoAttivita StatoAttivita { get; set; }

            /// <summary>
            /// Descrizione dell'attività
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            string Descrizione { get; set; }

            /// <summary>
            /// Luogo in cui si svolge l'attività
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            string Luogo { get; set; }

            /// <summary>
            /// Note aggiuntive
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            string Note { get; set; }

            /// <summary>
            /// Vero se l'attività è relativa all'intera giornata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            bool GiornataIntera { get; set; }

            /// <summary>
            /// Promemoria in minuti
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            int Promemoria { get; set; }

            /// <summary>
            /// Numero di ripetizioni dell'attività a partire dalla data di inizio.
            /// Se 0 l'attività si ripete fino a DataFine o all'infinito se DataFine non è valorizzata
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            int Ripetizione { get; set; }

            /// <summary>
            /// Data di inizio
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            DateTime DataInizio { get; set; }

            /// <summary>
            /// Data finale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            DateTime? DataFine { get; set; }

            /// <summary>
            /// ID dell'operatore a cui è assegnata l'attività
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            int IDOperatore { get; set; }

            /// <summary>
            /// Operatore a cui è assegnata l'attività
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            CUser Operatore { get; set; }

            /// <summary>
            /// Restituisce o imposta il nome dell'operatore
            /// </summary>
            string NomeOperatore { get; set; }

            /// <summary>
            /// Restituisce o imposta l'id dell'utente a cui é assegnata l'attività
            /// </summary>
            int IDAssegnatoA { get; set; }

            /// <summary>
            /// Restituisce o imposta l'utente a cui è assegnata l'attività
            /// </summary>
            CUser AssegnatoA { get; set; }

            /// <summary>
            /// Restituisce o imposta il nome dell'utente a cui è assegnata l'attività
            /// </summary>
            string NomeAssegnatoA { get; set; }

            /// <summary>
            /// Un campo aggiuntivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            object Tag { get; set; }

            /// <summary>
            /// Restituisce  imposta un'icona per l'attività
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            string IconURL { get; set; }

            /// <summary>
            /// Restituisce un numero intero che indica la priorità (crescente) dell'evento
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            int Priorita { get; set; }

            /// <summary>
            /// Restituisce o imposta l'id della persona collegata all'attività
            /// </summary>
            int IDPersona { get; set; }

            /// <summary>
            /// Restituisce o imposta la persona collegata all'attività
            /// </summary>
            Anagrafica.CPersona Persona { get; set; }

            /// <summary>
            /// Restituisce o imposta il nome della persona collegata all'attività
            /// </summary>
            string NomePersona { get; set; }

            /// <summary>
            /// Restituisce il nome del provider che gestisce l'attività
            /// </summary>
            string ProviderName { get; }

            /// <summary>
            /// Imposta il provider
            /// </summary>
            /// <param name="p"></param>
            void SetProvider(ICalendarProvider p);

            /// <summary>
            /// Restituisce il provider che gestisce l'attività
            /// </summary>
            ICalendarProvider Provider { get; }
        }
    }
}