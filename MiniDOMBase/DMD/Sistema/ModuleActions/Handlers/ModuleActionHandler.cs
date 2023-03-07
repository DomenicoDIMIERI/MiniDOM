
namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Interfaccia implementata dai gestori di un'azione
        /// </summary>
        /// <remarks></remarks>
        public interface ModuleActionHandler
        {

            /// <summary>
            /// Esegue l'azione specificata
            /// </summary>
            /// <param name="action"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            ModuleActionResult Execute(CModuleAction action, object context);

        }
    }
}