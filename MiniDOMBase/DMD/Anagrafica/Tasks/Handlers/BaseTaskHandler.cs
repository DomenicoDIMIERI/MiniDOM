
namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Classe base che implementa l'interfaccia <see cref="ITaskHandler"/>
        /// </summary>
        public abstract class BaseTaskHandler 
            : ITaskHandler
        {
            private TaskLavorazione m_Result = null;

            /// <summary>
            /// Costruttore
            /// </summary>
            public BaseTaskHandler()
            {
                ////DMDObject.IncreaseCounter(this);
                m_Result = null;
            }

            /// <summary>
            /// Risultati restituiti dell'handler
            /// </summary>
            public TaskLavorazione Result
            {
                get
                {
                    return m_Result;
                }
            }

            /// <summary>
            /// Gestisce la regola
            /// </summary>
            /// <param name="taskSorgente"></param>
            /// <param name="regola"></param>
            /// <param name="parametri"></param>
            /// <returns></returns>
            public TaskLavorazione Handle(TaskLavorazione taskSorgente, RegolaTaskLavorazione regola, string parametri)
            {
                m_Result = new TaskLavorazione();
                m_Result.TaskSorgente = taskSorgente;
                m_Result.RegolaEseguita = regola;
                m_Result.ParametriAzione = parametri;
                m_Result.PuntoOperativo = taskSorgente.PuntoOperativo;
                m_Result.DataInizioEsecuzione = DMD.DateUtils.Now();
                m_Result.Stato = ObjectStatus.OBJECT_VALID;
                m_Result.RisultatoAzione = "";
                m_Result.Save();
                // Me.m_Result.RisultatoAzione = Me.HandleInternal()
                HandleInternal();
                if (taskSorgente is object)
                {
                    taskSorgente.TaskDestinazione = m_Result;
                    taskSorgente.Save();
                }

                m_Result.Save();
                return m_Result;
            }

            /// <summary>
            /// Gestisce la regola internamente
            /// </summary>
            protected abstract void HandleInternal();

            /// <summary>
            /// Distruttore
            /// </summary>
            ~BaseTaskHandler()
            {
                ////DMDObject.DecreaseCounter(this);
            }
        }
    }
}