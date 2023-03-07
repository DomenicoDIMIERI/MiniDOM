
namespace minidom
{
    public partial class Sistema
    {
        /// <summary>
        /// Interfaccia implementata da un sistema che esegue una procedura automatica
        /// </summary>
        public interface IProceduraHandler
        {

            /// <summary>
            /// Esegue la procedura
            /// </summary>
            /// <param name="procedura"></param>
            void Run(CProcedura procedura);

            /// <summary>
            /// Inizializza i parametri standard della procedura
            /// </summary>
            /// <param name="procedura"></param>
            void InitializeParameters(CProcedura procedura);
        }
    }
}