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
        /// Collezione di oggetti <see cref="CRegisteredCalendarProvider"/>
        /// </summary>
        [Serializable]
        public class CRegisteredCalendarProvidersCollection 
            : CCollection<CRegisteredCalendarProvider>
        {


            /// <summary>
            /// Registra il nome della classe come nuove provider per il calendario
            /// </summary>
            /// <param name="pName"></param>
            /// <param name="type"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CRegisteredCalendarProvider RegisterProvider(string pName, Type type)
            {
                if (type is null)
                    throw new ArgumentException("type non può essere null");
                var ret = new CRegisteredCalendarProvider(pName, type);
                ret.Save();
                Add(ret);
                return ret;
            }

            /// <summary>
            /// Registra il nome della classe come nuove provider per il calendario
            /// </summary>
            /// <param name="pName"></param>
            /// <param name="typeName"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CRegisteredCalendarProvider RegisterProvider(string pName, string typeName)
            {
                var t = System.Reflection.Assembly.GetCallingAssembly().GetType(typeName);
                return RegisterProvider(pName, t);
            }

            /// <summary>
            /// Rimuove il nome della classe come nuove provider per il calendario
            /// </summary>
            /// <param name="pName"></param>
            /// <remarks></remarks>
            public void UnregisterProvider(string pName)
            {
                var p = this[pName];
                p.Delete();
                Remove(p);
            }

            internal bool Load()
            {
                Clear();
                string dbSQL = "SELECT * FROM [tbl_RegisteredCalendarProviders]";
                var reader = new Databases.DBReader(Databases.APPConn.Tables["tbl_RegisteredCalendarProviders"], dbSQL);
                while (reader.Read())
                {
                    var item = new CRegisteredCalendarProvider();
                    if (Databases.APPConn.Load(item, reader))
                        Add(item);
                }

                reader.Dispose();
                return true;
            }

            public int IndexOf(string providerName)
            {
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    if ((this[i].Nome ?? "") == (providerName ?? ""))
                        return i;
                }

                return -1;
            }

            public bool Contains(string providerName)
            {
                return IndexOf(providerName) >= 0;
            }

            public new CRegisteredCalendarProvider this[string providerName]
            {
                get
                {
                    return base[IndexOf(providerName)];
                }
            }
        }
    }
}