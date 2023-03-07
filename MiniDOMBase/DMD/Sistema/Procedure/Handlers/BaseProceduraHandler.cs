
namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Gestore base di una procedura automatizzata
        /// </summary>
        public abstract class BaseProceduraHandler 
            : IProceduraHandler
        {

            /// <summary>
            /// Inizializza i parametri standard della procedura
            /// </summary>
            /// <param name="procedura"></param>
            public abstract void InitializeParameters(CProcedura procedura);

            /// <summary>
            /// Esegue la procedura
            /// </summary>
            /// <param name="procedura"></param>
            public abstract void Run(CProcedura procedura);

            
        }
    }
}