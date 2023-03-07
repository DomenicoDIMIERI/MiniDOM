using System;
using System.Collections;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="TaskLavorazione"/>
        /// </summary>
        [Serializable]
        public class CTasksDiLavorazioneClass 
            : CModulesClass<Anagrafica.TaskLavorazione>
        {
           

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTasksDiLavorazioneClass() 
                : base("modAnaTaskLavorazione", typeof(Anagrafica.TaskLavorazioneCursor), 0)
            {
            }

            /// <summary>
            /// Inizializza il modulo
            /// </summary>
            public override void Initialize()
            {
                base.Initialize();
                minidom.Anagrafica.PersonaCreated += HandlePeronaModified;
                minidom.Anagrafica.PersonaDeleted += HandlePeronaModified;
                minidom.Anagrafica.PersonaModified += HandlePeronaModified;
                minidom.Anagrafica.PersonaMerged += HandlePeronaMerged;
                minidom.Anagrafica.PersonaUnMerged += HandlePeronaUnMerged;
            }

            /// <summary>
            /// Termina il modulo
            /// </summary>
            public override void Terminate()
            {
                minidom.Anagrafica.PersonaCreated -= HandlePeronaModified;
                minidom.Anagrafica.PersonaDeleted -= HandlePeronaModified;
                minidom.Anagrafica.PersonaModified -= HandlePeronaModified;
                minidom.Anagrafica.PersonaMerged -= HandlePeronaMerged;
                minidom.Anagrafica.PersonaUnMerged -= HandlePeronaUnMerged;
                base.Terminate();
            }

            /// <summary>
            /// Gestisce l'evento PersonaMerged
            /// </summary>
            /// <param name="e"></param>
            /// <param name="sender"></param>
            protected virtual void HandlePeronaMerged(object sender, MergePersonaEventArgs e)
            {
                lock (this.cacheLock)
                {
                    var mi = e.MI;
                    var persona1 = mi.Persona1;
                    var persona2 = mi.Persona2;
                    CMergePersonaRecord rec;


                    // Tabella tbl_TaskLavorazione 
                    using (var cursor = new TaskLavorazioneCursor())
                    {
                        cursor.IgnoreRights = true;
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IDCliente.Value = mi.IDPersona2;
                        while (cursor.Read())
                        {
                            rec = new CMergePersonaRecord();
                            rec.NomeTabella = "tbl_TaskLavorazione";
                            rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                            rec.FieldName = "IDCliente";
                            mi.TabelleModificate.Add(rec);

                            cursor.Item.Cliente = mi.Persona1;
                            cursor.Item.Save();
                        }

                    }

                }
            }

            /// <summary>
            /// Gestisce l'evento PersonaUnmerged
            /// </summary>
            /// <param name="e"></param>
            /// <param name="sender"></param>
            protected virtual void HandlePeronaUnMerged(object sender, MergePersonaEventArgs e)
            {
                lock (this.cacheLock)
                {
                    var mi = e.MI;

                    // Tabella sLavorazione 
                    var items = mi.GetAffectedRecorsIDs("tbl_TaskLavorazione", "IDCliente");
                    //if (!string.IsNullOrEmpty(items))
                    //    TasksDiLavorazione.Database.ExecuteCommand("UPDATE [tbl_TaskLavorazione] SET [IDCliente]=" + DBUtils.GetID(persona1) + ", [NomeCliente]=" + Databases.DBUtils.DBString(persona1.Nominativo) + "  WHERE [ID] In (" + items + ")");
                    using (var cursor = new TaskLavorazioneCursor())
                    {
                        cursor.ID.ValueIn(items);
                        cursor.IgnoreRights = true;
                        while (cursor.Read())
                        {
                            cursor.Item.Cliente = mi.Persona2;
                            cursor.Item.Save();
                        }
                    }

                }
            }



            /// <summary>
            /// Gestisce l'evento PersonaChanged
            /// </summary>
            /// <param name="e"></param>
            /// <param name="sender"></param>
            protected virtual void HandlePeronaModified(object sender, ItemEventArgs<CPersona> e)
            {
                lock (this.cacheLock)
                {
                    var p = e.Item;

                }
            }


            /// <summary>
            /// Restituisce la collezione di tutti i task attivi in corso per il cliente specificato
            /// </summary>
            /// <param name="cliente"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<TaskLavorazione> GetTasksInCorso(Anagrafica.CPersona cliente)
            {
                if (cliente is null)
                    throw new ArgumentNullException("cliente");

                var ret = new CCollection<Anagrafica.TaskLavorazione>();
                
                if (DBUtils.GetID(cliente, 0) == 0)
                    return ret;

                using (var cursor = new Anagrafica.TaskLavorazioneCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDTaskDestinazione.Value = 0;
                    cursor.IDTaskDestinazione.IncludeNulls = true;
                    cursor.IDCliente.Value = DBUtils.GetID(cliente, 0);
                    // Dim arrStatiFinali As New System.Collections.ArrayList
                    // Dim stati As CCollection(Of StatoTaskLavorazione) = Anagrafica.StatiTasksLavorazione.LoadAll
                    // For Each st As StatoTaskLavorazione In stati
                    // If st.Finale Then arrStatiFinali.Add(GetID(st))
                    // Next
                    // cursor.IDStatoAttuale.ValueIn(arrStatiFinali.ToArray)
                    // cursor.IDStatoAttuale.Operator = Databases.OP.OP_NOTIN
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }

                }

                return ret;
            }

            /// <summary>
            /// Restituisce l'ultimo stato di lavorazione relativo al cliente in ordine di data (il più recente)
            /// </summary>
            /// <param name="cliente"></param>
            /// <returns></returns>
            public TaskLavorazione GetTask(Anagrafica.CPersona cliente)
            {
                //TODO GetTask
                if (cliente is null)
                    throw new ArgumentNullException("cliente");
                //string dbSQL = "SELECT [tbl_TaskLavorazione].* FROM ";
                //dbSQL += "[tbl_TaskLavorazione] ";
                //dbSQL += "LEFT JOIN ";
                //dbSQL += "(SELECT tbl_TaskLavorazione.IDCliente, Max(tbl_TaskLavorazione.DataAssegnazione) AS MaxDiDataAssegnazione FROM tbl_TaskLavorazione GROUP BY tbl_TaskLavorazione.IDCliente) as T1 ";
                //dbSQL += "ON tbl_TaskLavorazione.DataAssegnazione=T1.MaxDiDataAssegnazione ";
                //dbSQL += "WHERE [tbl_TaskLavorazione].[Stato]=1 AND [tbl_TaskLavorazione].IDCliente=" + DBUtils.GetID(cliente);
                //Anagrafica.TaskLavorazione ret = null;
                //using (var dbRis = Database.ExecuteReader(dbSQL))
                //{
                //    if (dbRis.Read())
                //    {
                //        ret = new Anagrafica.TaskLavorazione();
                //        Database.Load(ret, dbRis);
                //        ret.SetCliente(cliente);
                //    }
                //}
                //return ret;
                using(var cursor = new TaskLavorazioneCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDCliente.Value = DBUtils.GetID(cliente, 0);
                    cursor.DataAssegnazione.SortOrder = SortEnum.SORT_DESC;
                    var ret = cursor.Item;
                    if (ret is object)
                        ret.SetCliente(cliente);
                    return ret;
                }
            }

            /// <summary>
            /// Inizializza il task
            /// </summary>
            /// <param name="cliente"></param>
            /// <param name="contesto"></param>
            /// <returns></returns>
            public TaskLavorazione Inizializza(CPersona cliente, string contesto)
            {
                if (cliente is Anagrafica.CAzienda)
                    return null;
                if (cliente is null)
                    throw new ArgumentNullException("cliente");
                Anagrafica.StatoTaskLavorazione stato = null;
                foreach (var st in Anagrafica.StatiTasksLavorazione.LoadAll())
                {
                    if (st.Stato == ObjectStatus.OBJECT_VALID && st.Attivo && st.Iniziale)
                    {
                        stato = st;
                        break;
                    }
                }

                if (stato is null)
                    return null;
                var ret = new TaskLavorazione();
                ret.AssegnatoA = Sistema.Users.CurrentUser;
                ret.AssegnatoDa = ret.AssegnatoA;
                ret.TipoContesto = contesto;
                ret.Stato = ObjectStatus.OBJECT_VALID;
                ret.DataAssegnazione = DMD.DateUtils.Now();
                ret.DataInizioEsecuzione = ret.DataAssegnazione;
                ret.Cliente = cliente;
                ret.PuntoOperativo = cliente.PuntoOperativo;
                ret.StatoAttuale = stato;
                ret.Save();
                return ret;
            }

            /// <summary>
            /// Restituisce la collezione di tutti i task attivi per il punto operativo e per l'operatore
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Anagrafica.TaskLavorazione> GetTasksInCorso(CUfficio po, Sistema.CUser op)
            {
                var ret = new CCollection<TaskLavorazione>();
                using (var cursor = new TaskLavorazioneCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDTaskDestinazione.Value = 0;
                    cursor.IDTaskDestinazione.IncludeNulls = true;
                    if (DBUtils.GetID(po, 0) != 0)
                        cursor.IDPuntoOperativo.Value = DBUtils.GetID(po, 0);
                    if (DBUtils.GetID(op, 0) != 0)
                        cursor.IDAssegnatoA.Value = DBUtils.GetID(op, 0);
                    var arrStatiFinali = new ArrayList();
                    var stati = Anagrafica.StatiTasksLavorazione.LoadAll();
                    foreach (var st in stati)
                    {
                        if (st.Finale)
                            arrStatiFinali.Add(DBUtils.GetID(st));
                    }

                    cursor.IDStatoAttuale.ValueIn(arrStatiFinali.ToArray());
                    cursor.IDStatoAttuale.Operator = OP.OP_NOTIN;
                    // cursor.IgnoreRights = True
                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }

                }

                return ret;
            }
        }
    }

    public partial class Anagrafica
    {
        private static CTasksDiLavorazioneClass m_TasksLavorazione = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="TaskLavorazione"/>
        /// </summary>
        public static CTasksDiLavorazioneClass TasksDiLavorazione
        {
            get
            {
                if (m_TasksLavorazione is null)
                    m_TasksLavorazione = new CTasksDiLavorazioneClass();
                return m_TasksLavorazione;
            }
        }
    }
}