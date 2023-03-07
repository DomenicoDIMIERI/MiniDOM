using System.Collections.Generic;
using System.Diagnostics;
using DMD;
using DMD.Databases;
using minidom.CallManagers;
using minidom.CallManagers.Events;

namespace minidom.PBX
{

    /// <summary>
    /// Handler delle chiamate
    /// </summary>
    public sealed class Chiamate 
    {
        private static ChiamateRepository m_Chiamate;
        
        private Chiamate()
        {
        } 

        /// <summary>
        /// Repository di oggetti <see cref="Chiamata"/>
        /// </summary>
        public static ChiamateRepository Chiamate
        {
            get
            {
                if (m_Chiamate is null)
                    m_Chiamate = new ChiamateRepository();
                return m_Chiamate;
            }
        }
    }
}