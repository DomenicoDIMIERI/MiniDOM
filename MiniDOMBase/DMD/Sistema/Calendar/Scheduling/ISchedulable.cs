
namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Interfaccia implementata dagli oggetti schedulabili
        /// </summary>
        public interface ISchedulable
        {

            /// <summary>
            /// Restituisce l'insieme delle programmazioni
            /// </summary>
            MultipleScheduleCollection Programmazione { get; }

            /// <summary>
            /// Notifica all'oggetto che implementa la classe che la programmazione è stata cambiata esternamente e deve essere ricaricata
            /// </summary>
            void InvalidateProgrammazione();

            /// <summary>
            /// Notifica all'oggetto che la schedulazione s è stata modificata
            /// </summary>
            /// <param name="s"></param>
            void NotifySchedule(CalendarSchedule s);
        }
    }
}