using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom.internals
{


    /// <summary>
    /// Classe base del provider di attività del calendario
    /// </summary>
    /// <remarks></remarks>
    public class CCalendarActivitiesProvider 
        : Sistema.BaseCalendarActivitiesProvider
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public CCalendarActivitiesProvider()
        {
        }

        /// <summary>
        /// Restituisce il repository
        /// </summary>
        /// <returns></returns>
        protected override CModulesClass GetModule()
        {
            return minidom.Sistema.Calendar; //.Module;
        }

        /// <summary>
        /// Crea un nuovo oggetto gestito dal provider
        /// </summary>
        /// <returns></returns>
        public override object InstantiateNewItem()
        {
            return new Sistema.CCalendarActivity();
        }

        /// <summary>
        /// Restituisce la stringa interpretata dal provider
        /// </summary>
        /// <returns></returns>
        public override string GetCreateCommand()
        {
            return "/calendar/activities/?_a=create";
        }

        /// <summary>
        /// Restituisce una descrizione del provider
        /// </summary>
        /// <returns></returns>
        public override string GetShortDescription()
        {
            return "Attività";
        }

        /// <summary>
        /// Restituisce l'array dei tipi restituiti
        /// </summary>
        /// <returns></returns>
        public override Type[] GetSupportedTypes()
        {
            return new[] { typeof(Sistema.CCalendarActivity) };
        }

        /// <summary>
        /// Restituisce le persone che hanno eventi attivi
        /// </summary>
        /// <param name="nomeLista"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="ufficio"></param>
        /// <param name="operatore"></param>
        /// <returns></returns>
        public override CCollection<Sistema.CActivePerson> GetActivePersons(string nomeLista, DateTime? fromDate, DateTime? toDate, int ufficio = 0, int operatore = 0)
        {
            return new CCollection<Sistema.CActivePerson>();
        }

        /// <summary>
        /// Restituisce la lista delle cose da fare
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override CCollection<Sistema.ICalendarActivity> GetToDoList(Sistema.CUser user)
        {
            if (user is null)
                throw new ArgumentNullException("user");
            var ret = new CCollection<Sistema.ICalendarActivity>();
            if (DBUtils.GetID(user, 0) == 0)
                return ret;

            using (var cursor = new Sistema.CCalendarActivityCursor())
            {
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.IDAssegnatoA.Value = DBUtils.GetID(user, 0);
                cursor.StatoAttivita.Value = Sistema.StatoAttivita.CONCLUSA;
                cursor.StatoAttivita.Operator = OP.OP_NE;
                cursor.IgnoreRights = true;
                cursor.ProviderName.Value = UniqueName;
                cursor.ProviderName.IncludeNulls = true;
                while (cursor.Read())
                {
                    var a = cursor.Item;
                    a.SetProvider(this);
                    a.Flags = a.Flags | Sistema.CalendarActivityFlags.CanDelete | Sistema.CalendarActivityFlags.CanEdit;
                    ret.Add(a);
                }
            }
            return ret;
        }

        /// <summary>
        /// Restituisce le scadenze nel periodo
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public override CCollection<Sistema.ICalendarActivity> GetScadenze(DateTime? fromDate, DateTime? toDate)
        {
            var ret = new CCollection<Sistema.ICalendarActivity>();
            using(var cursor = new CCalendarActivityCursor())
            {
                if (fromDate != null) { cursor.DataInizio.Value = fromDate.Value; cursor.DataInizio.Operator = OP.OP_GE; }
                if (toDate != null) { cursor.DataFine.Value = toDate.Value; cursor.DataFine.Operator = OP.OP_LE; }
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                while (cursor.Read())
                {
                    ret.Add(cursor.Item);
                }
            }
            ret.Comparer = CCalendarItemsComparer.Instance;
            ret.Sort();
            return ret;
        }

        /// <summary>
        /// Restituisce le scadenze in attesa
        /// </summary>
        /// <returns></returns>
        public override CCollection<Sistema.ICalendarActivity> GetPendingActivities()
        {
            var ret = new CCollection<Sistema.ICalendarActivity>();
            using (var cursor = new CCalendarActivityCursor())
            {
                cursor.DataFine.Value = DMD.DateUtils.GetLastSecond(DMD.DateUtils.ToDay());
                cursor.DataFine.Operator = OP.OP_LE;
                cursor.StatoAttivita.Value = StatoAttivita.INATTESA;
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                while (cursor.Read())
                {
                    ret.Add(cursor.Item);
                }
            }
            ret.Comparer = CCalendarItemsComparer.Instance;
            ret.Sort();
            return ret;
        }

        /// <summary>
        /// Restituisce il nome univoco del provider
        /// </summary>
        public override string UniqueName
        {
            get
            {
                return "DEFCALPROV";
            }
        }

        /// <summary>
        /// Elimina l'attività
        /// </summary>
        /// <param name="item"></param>
        /// <param name="force"></param>
        public override void DeleteActivity(Sistema.ICalendarActivity item, bool force = false)
        {
            Sistema.CCalendarActivity c = (Sistema.CCalendarActivity)item;
            c.OldDelete(force);
        }

        /// <summary>
        /// Salva l'attività
        /// </summary>
        /// <param name="item"></param>
        /// <param name="force"></param>
        public override void SaveActivity(Sistema.ICalendarActivity item, bool force = false)
        {
            Sistema.CCalendarActivity c = (Sistema.CCalendarActivity)item;
            c.OldSave(force);
        }
    }
}