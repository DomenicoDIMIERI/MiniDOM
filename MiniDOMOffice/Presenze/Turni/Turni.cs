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
using static minidom.Office;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di <see cref="Turno"/>
        /// </summary>
        [Serializable]
        public sealed class CTurniClass
            : CModulesClass<Turno>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTurniClass() 
                : base("modOfficeTurniIO", typeof(minidom.Office.TurniCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce il turno in base al nome
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Turno GetItemByName(string name)
            {
                name = DMD.Strings.Trim(name);
                if (string.IsNullOrEmpty(name))
                    return null;
                var items = LoadAll();
                items.Sort();
                foreach (minidom.Office.Turno item in items)
                {
                    if (DMD.Strings.Compare(item.Nome, name, true) == 0)
                        return item;
                }

                return null;
            }

            // ''' <summary>
            // ''' Restituisce il miglior turno disponibile per l'ora specificata
            // ''' </summary>
            // ''' <param name="ora"></param>
            // ''' <returns></returns>
            // Public Function MatchTurnoIngresso(ByVal ora As Date) As Turno
            // Dim items As CCollection(Of Turno) = Me.LoadAll
            // items.Sort()
            // For Each item As Turno In items
            // If (Strings.Compare(item.Nome, name, true) = 0) Then Return item
            // Next
            // Return Nothing
            // End Function

        }
    }

    public partial class Office
    {
        private static CTurniClass m_Turni = null;

        /// <summary>
        /// Repository di <see cref="Turno"/>
        /// </summary>
        public static CTurniClass Turni
        {
            get
            {
                if (m_Turni is null)
                    m_Turni = new CTurniClass();
                return m_Turni;
            }
        }
    }
}