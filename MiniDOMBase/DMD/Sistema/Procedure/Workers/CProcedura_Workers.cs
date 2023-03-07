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

namespace minidom
{
    namespace repositories
    {

        public partial class CProcedureClass
        {

            private class Worker
                  : IDisposable
            {

                public ManualResetEvent @lock = new ManualResetEvent(false);
                public Thread thread = null;
                public PriorityEnum priority;
                public CCollection<Sistema.CProcedura> queue;
                public DateTime startDate;
                public DateTime? endDate;
                public Sistema.CProcedura c;
                public bool ForceQuit = false;

                public Worker(PriorityEnum priority)
                {
                    this.priority = priority;
                    queue = new CCollection<Sistema.CProcedura>();
                    thread = new Thread(Work);
                    switch (priority)
                    {
                        case PriorityEnum.PRIORITY_HIGHER:
                            {
                                thread.Priority = ThreadPriority.Highest;
                                break;
                            }

                        case PriorityEnum.PRIORITY_HIGH:
                            {
                                thread.Priority = ThreadPriority.AboveNormal;
                                break;
                            }

                        case PriorityEnum.PRIORITY_NORMAL:
                            {
                                thread.Priority = ThreadPriority.Normal;
                                break;
                            }

                        case PriorityEnum.PRIOTITY_LOW:
                            {
                                thread.Priority = ThreadPriority.BelowNormal;
                                break;
                            }

                        case PriorityEnum.PRIORITY_LOWER:
                            {
                                thread.Priority = ThreadPriority.Lowest;
                                break;
                            }
                    }
                }

                public void Enqueue(Sistema.CProcedura c)
                {
                    lock (this)
                        queue.Add(c);
                }

                public Sistema.CProcedura Dequeue()
                {
                    Sistema.CProcedura c = null;
                    lock (this)
                    {
                        if (queue.Count > 0)
                        {
                            c = queue[0];
                            queue.RemoveAt(0);
                        }
                    }

                    return c;
                }

                private void Work()
                {
                    do
                    {
#if (!DEBUG)
                        try {
#endif
                        @lock.WaitOne(5000);
#if (!DEBUG)
                        } catch (Exception) {
                             
                        }
#endif

                        do
                        {
                            c = Dequeue();
                            if (c is object)
                            {
                                startDate = DMD.DateUtils.Now();
                                endDate = default;
                                lock (workerLock)
                                    m_RunningThreads += 1;
#if (!DEBUG)
                                try {
#endif
                                var d1 = DMD.DateUtils.Now();
                                Exception erroreP = null;
                                Sistema.ApplicationContext.Log("Procedura [" + DMD.RunTime.vbTypeName(c) + ":" + DBUtils.GetID(c) + "]: " + c.Nome + " -> Inizio Esecuzione");
#if (!DEBUG)
                                    try {
#endif
                                c.Run();
#if (!DEBUG)
                                    } catch (Exception ex) {
                                        erroreP = ex;
                                    }
#endif
                                var d2 = DMD.DateUtils.Now();
                                var s = c.Programmazione.GetNextSchedule();
                                s.UltimaEsecuzione = startDate;
                                s.ConteggioEsecuzioni += 1;
                                s.Save();
                                c.Save();
                                if (erroreP is null)
                                {
                                    Sistema.ApplicationContext.Log("Procedura [" + DMD.RunTime.vbTypeName(c) + ":" + DBUtils.GetID(c) + "]: " + c.Nome + " -> Fine Esecuzione (" + Sistema.Formats.FormatDurata((long?)(d2 - d1).TotalSeconds) + ")");
                                }
                                else
                                {
                                    Sistema.ApplicationContext.Log("Procedura [" + DMD.RunTime.vbTypeName(c) + ":" + DBUtils.GetID(c) + "]: " + c.Nome + " -> Errore (" + Sistema.Formats.FormatDurata((long?)(d2 - d1).TotalSeconds) + ")");
                                    Sistema.ApplicationContext.Log(erroreP.Message);
                                }
#if (!DEBUG)
                                } catch (Exception ex) {
                                    Sistema.ApplicationContext.Log("Procedura [" + DMD.RunTime.vbTypeName(c) + ":" + DBUtils.GetID(c) + "]: Eccezione: " + ex.Message + DMD.Strings.vbNewLine + ex.StackTrace);
                                }
#endif

                                lock (workerLock)
                                    m_RunningThreads -= 1;
                                endDate = DMD.DateUtils.Now();
                            }
                        }
                        while (c is object && !ForceQuit);
                        c = null;
                    }
                    while (!ForceQuit);
                }

                /// <summary>
                /// Restituisce true se la procedura è in esecuzione
                /// </summary>
                /// <returns></returns>
                public bool IsRunning()
                {
                    return c is object;
                }

                /// <summary>
                /// Avvia la procedura
                /// </summary>
                public void Start()
                {
                    thread.Start();
                }

                /// <summary>
                /// Rilascia le risorse
                /// </summary>
                public void Dispose()
                {
                    this.queue.Clear();
                    this.thread.Abort();
                    this.@lock.Dispose();
                }
            }

        }
    }
}