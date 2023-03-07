using System;
using System.Runtime.Serialization.Formatters.Binary;
using minidom.internals;

namespace minidom
{
    public sealed partial class Sistema
        //: CModulesClass
    {
        

        ///// <summary>
        ///// Costruttore
        ///// </summary>
        //public Sistema()
        //    : base("modSistema")
        //{
        //}

        private static IApplicationContext m_ApplicationContext;


        /// <summary>
        /// Restituisce un riferimento all'applicazione predefinita
        /// </summary>
        public static IApplicationContext ApplicationContext
        {
            get
            {
                return m_ApplicationContext;
            }
        }

        /// <summary>
        /// Imposta l'applicazione predefinita
        /// </summary>
        /// <param name="value"></param>
        public static void SetApplicationContext(IApplicationContext value)
        {
            m_ApplicationContext = value;
        }



        /// <summary>
        /// Inizializza la libreria
        /// </summary>
        /// <remarks></remarks>
        public static void Initialize()
        {
            if (!Groups.KnownGroups.Administrators.Members.Contains(Users.KnownUsers.GlobalAdmin))
            {
                Groups.KnownGroups.Administrators.Members.Add(Users.KnownUsers.GlobalAdmin);
            }

            if (!Groups.KnownGroups.Guests.Members.Contains(Users.KnownUsers.GuestUser))
            {
                Groups.KnownGroups.Guests.Members.Add(Users.KnownUsers.GuestUser);
            }

            //Types.Initialize();
        }



        
    }
}