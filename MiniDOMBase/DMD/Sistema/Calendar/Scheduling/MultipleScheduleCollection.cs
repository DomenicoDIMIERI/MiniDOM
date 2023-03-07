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

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Programmazione multipla
        /// </summary>
        [Serializable]
        public class MultipleScheduleCollection 
            : CCollection<CalendarSchedule>
        {

            /// <summary>
            /// Oggetto a cui appartiene la collezione
            /// </summary>
            [NonSerialized] private ISchedulable m_Owner;

            /// <summary>
            /// Costruttore
            /// </summary>
            public MultipleScheduleCollection()
            {
                this.m_Owner = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            public MultipleScheduleCollection(object owner) 
                : this()
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");

                this.m_Owner = (ISchedulable)owner;
                this.Reload();
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CalendarSchedule value)
            {
                if (m_Owner is object)
                    value.SetOwner(m_Owner);

                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CalendarSchedule oldValue, CalendarSchedule newValue)
            {
                if (m_Owner is object)
                    newValue.SetOwner(m_Owner);
                
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Ricarica gli oggetti
            /// </summary>
            public void Reload()
            {
                this.Clear();
                if (DBUtils.GetID(m_Owner, 0) == 0)
                    return;

                using (var cursor = new CalendarScheduleCursor())
                { 
                    cursor.OwnerType.Value = DMD.RunTime.vbTypeName(m_Owner);
                    cursor.OwnerID.Value = DBUtils.GetID(m_Owner, 0);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }
                 
            }

            /// <summary>
            /// Aggiunge un nuovo elemento
            /// </summary>
            /// <param name="tipo"></param>
            /// <param name="dataInizio"></param>
            /// <param name="intervallo"></param>
            /// <param name="ripetizioni"></param>
            /// <returns></returns>
            public CalendarSchedule Add(ScheduleType tipo, DateTime dataInizio, float intervallo = 1f, int ripetizioni = 0)
            {
                var schedule = new CalendarSchedule(tipo, dataInizio, intervallo, ripetizioni);
                schedule.Stato = ObjectStatus.OBJECT_VALID;
                Add(schedule);
                return schedule;
            }

            /// <summary>
            /// Restituisce il primo evento non ancora verificatosi
            /// </summary>
            /// <returns></returns>
            public CalendarSchedule GetNextSchedule()
            {
                DateTime? ret = default;
                CalendarSchedule s = null;
                foreach (CalendarSchedule item in this)
                {
                    var u = item.CalcolaProssimaEsecuzione();
                    if (u.HasValue)
                    {
                        if (ret.HasValue)
                        {
                            if (u.Value < ret.Value)
                            {
                                ret = u;
                                s = item;
                            }
                        }
                        else
                        {
                            ret = u;
                            s = item;
                        }
                    }
                }

                return s;
            }

            ///// <summary>
            ///// Restituisce una stringa che rappresenta
            ///// </summary>
            ///// <returns></returns>
            //public override string ToString()
            //{
            //    string ret = "";
            //    foreach (CalendarSchedule Item in this)
            //    {
            //        if (!string.IsNullOrEmpty(ret))
            //            ret += DMD.Strings.vbCrLf;
            //        ret += Item.ToString();
            //    }

            //    return ret;
            //}

            /// <summary>
            /// Imposta il proprietario
            /// </summary>
            /// <param name="value"></param>
            public void SetOwner(object value)
            {
                this.m_Owner = (ISchedulable)value;
                if (value is object)
                {
                    foreach (CalendarSchedule Item in this)
                        Item.SetOwner(value);
                }

            }
        }
    }
}