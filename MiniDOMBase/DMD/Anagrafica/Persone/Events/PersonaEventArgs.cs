//TODO rimuovere PersonaEventArgs da Javascript

//using System;

//namespace minidom
//{
//    public partial class Anagrafica
//    {

//        /// <summary>
//        /// Evento relativo ad una persona fisica o giuridica
//        /// </summary>
//        [Serializable]
//        public class PersonaEventArgs 
//            : EventArgs
//        {
            
//            [NonSerialized] private CPersona m_Persona;

//            /// <summary>
//            /// Csotruttore
//            /// </summary>
//            public PersonaEventArgs()
//            {
//                ////DMDObject.IncreaseCounter(this);
//            }

//            /// <summary>
//            /// Csotruttore
//            /// </summary>
//            /// <param name="persona"></param>
//            public PersonaEventArgs(CPersona persona) : this()
//            {
//                m_Persona = persona;
//            }

//            /// <summary>
//            /// Restituisce la persona che ha generato l'evento
//            /// </summary>
//            public CPersona Persona
//            {
//                get
//                {
//                    return m_Persona;
//                }
//            }

//            /// <summary>
//            /// Distruttore
//            /// </summary>
//            ~PersonaEventArgs()
//            {
//                ////DMDObject.DecreaseCounter(this);
//            }
//        }
//    }
//}