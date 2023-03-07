
namespace minidom
{
    public partial class Anagrafica
    {
        /// <summary>
        /// Handler di una regola
        /// </summary>
        public interface ITaskHandler
        {

            /// <summary>
            /// Esegue la regola specificata sul task sorgente e restituisce il task generato
            /// </summary>
            /// <param name="taskSorgente"></param>
            /// <param name="regola"></param>
            /// <param name="parametri"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            TaskLavorazione Handle(TaskLavorazione taskSorgente, RegolaTaskLavorazione regola, string parametri);
        }
    }
}