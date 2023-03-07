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
using static minidom.Finanziaria;


namespace minidom.repositories
{

    /// <summary>
    /// Repository di oggetti di tipo <see cref="NotaCollaboratore"/>
    /// </summary>
    [Serializable]
    public sealed class CNotaCollaboratoreRepository
        : CModulesClass<NotaCollaboratore>
    {
        
        /// <summary>
        /// Costruttore
        /// </summary>
        public CNotaCollaboratoreRepository()
            : base("modFINCollabNote", typeof(NotaCollaboratoreCursor), 0)
        {
             
        }
          
    }

    public partial class CCollaboratoriClass
    {

        private CNotaCollaboratoreRepository m_Note = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="NotaCollaboratore"/>
        /// </summary>
        public CNotaCollaboratoreRepository Note
        {
            get
            {
                if (this.m_Note is null) this.m_Note = new CNotaCollaboratoreRepository();
                return this.m_Note;
            }
        }
    }
}