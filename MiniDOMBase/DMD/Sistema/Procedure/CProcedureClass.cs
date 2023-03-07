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
using System.Threading;
using System.Timers;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CProcedura"/>
        /// </summary>
        [Serializable]
        public partial class CProcedureClass 
            : CModulesClass<Sistema.CProcedura>
        {

           
           
            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            private const int WORKER_GRANULARITY_MILLI = 60 * 1000; // 1 minuto
            private bool stopping = false;
            private bool m_Running = false;
            private static object schedulerLock = new object();
            private static object workerLock = new object();
            private System.Timers.Timer m_SchedulerTimer;

             
            private static int m_RunningThreads = 0;
            private Worker[] workers;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CProcedureClass() 
                : base("modCalendarProcs", typeof(Sistema.CProcedureCursor), -1)
            {

                // Me.m_Scheduler = Nothing
                m_SchedulerTimer = new System.Timers.Timer(WORKER_GRANULARITY_MILLI);
                m_SchedulerTimer.Elapsed += m_SchedulerTimer_Elapsed;

                workers = (Worker[])Array.CreateInstance(typeof(Worker), 5);
                workers[0] = new Worker(PriorityEnum.PRIORITY_HIGHER);
                workers[1] = new Worker(PriorityEnum.PRIORITY_HIGH);
                workers[2] = new Worker(PriorityEnum.PRIORITY_NORMAL);
                workers[3] = new Worker(PriorityEnum.PRIOTITY_LOW);
                workers[4] = new Worker(PriorityEnum.PRIORITY_LOWER);
            }

            private Worker GetWorker(PriorityEnum priority)
            {
                switch (priority)
                {
                    case PriorityEnum.PRIORITY_HIGHER:
                        {
                            return workers[0];
                        }

                    case PriorityEnum.PRIORITY_HIGH:
                        {
                            return workers[1];
                        }

                    case PriorityEnum.PRIORITY_NORMAL:
                        {
                            return workers[2];
                        }

                    case PriorityEnum.PRIOTITY_LOW:
                        {
                            return workers[3];
                        }

                    case PriorityEnum.PRIORITY_LOWER:
                        {
                            return workers[4];
                        }
                }

                return null;
            }

            /// <summary>
            /// Restituisce il numero di threads usati dalle procedure
            /// </summary>
            /// <returns></returns>
            public int CountRunningThreads()
            {
                return m_RunningThreads;
            }

            /// <summary>
            /// Restituisce la procedura in base al nome
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Sistema.CProcedura GetItemByName(string name)
            {
                name = DMD.Strings.Trim(name);
                if (string.IsNullOrEmpty(name))
                    return null;
                var items = LoadAll();
                foreach (Sistema.CProcedura p in items)
                {
                    if (p.Stato == ObjectStatus.OBJECT_VALID && DMD.Strings.Compare(p.Nome, name, true) == 0)
                        return p;
                }

                return null;
            }

            /// <summary>
            /// Avvia il sistema di esecuzione delle procedure automatiche
            /// </summary>
            public void StartBackgroundWorker()
            {
                lock (schedulerLock)
                {
                    stopping = false;
                    foreach (Worker w in workers)
                        w.Start();
                    m_SchedulerTimer.Enabled = true;
                }
            }

            /// <summary>
            /// Forma il sistema di esecuzione delle procedure automatiche
            /// </summary>
            public void StopBackgroundWorker()
            {
                lock (schedulerLock)
                {
                    stopping = true;
                    foreach (Worker w in workers)
                    {
                        w.ForceQuit = true;
                        if (w.thread.IsAlive)
                            w.thread.Join(5000);
                    }

                    m_SchedulerTimer.Enabled = false;
                }
            }

            private void SchedulerThread()
            {
                lock (schedulerLock)
                {
#if (!DEBUG)
                    try {
#endif
                        if (m_Running)
                            return;
                        m_Running = true;

                        // While (Not Me.stopping)
                        var d = DMD.DateUtils.Now();
                        var tutte = LoadAll();
                        Sistema.CalendarSchedule s;
                        foreach (var c in tutte)
                        {
                            if (c.Stato != ObjectStatus.OBJECT_VALID || DMD.RunTime.TestFlag(c.Flags, Sistema.ProceduraFlags.Disabilitata))
                                continue;
                            s = c.Programmazione.GetNextSchedule();
                            if (s is object)
                            {
                                var dNext = s.CalcolaProssimaEsecuzione();
                                if (dNext.HasValue && dNext.Value <= DMD.DateUtils.Now())
                                {
                                    // SyncLock Me.queueLock
                                    var w = GetWorker(c.Priority);
                                    w.Enqueue(c);
                                    w.@lock.Set();
                                }
                            }

                            if (stopping)
                                break;
                        }
#if (!DEBUG)
                    } catch (Exception ex) {
                        Sistema.ApplicationContext.Log("Procedures: Scheduler Thread Crashed: " + ex.Message);
                    }
#endif
                    m_Running = false;
                }
            }

            private void m_SchedulerTimer_Elapsed(object sender, ElapsedEventArgs e)
            {
                SchedulerThread();
            }
        }
    }

    public partial class Sistema
    {
        private static CProcedureClass m_Procedure = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CProcedura"/>
        /// </summary>
        public static CProcedureClass Procedure
        {
            get
            {
                if (m_Procedure is null)
                    m_Procedure = new CProcedureClass();
                return m_Procedure;
            }
        }
    }
}