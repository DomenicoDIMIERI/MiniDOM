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
using minidom.internals;

namespace minidom
{
    public partial class Sistema
    {
        /// <summary>
        /// Interfaccia che devono implementare gli oggetti che utilizzano il servizio
        /// <see cref="CIndexingService"/>
        /// </summary>
        public interface IIndexable
        {
            /// <summary>
            /// Restituisce l'array delle parole da indicizzare
            /// </summary>
            /// <returns></returns>
            string[] GetIndexedWords();

            /// <summary>
            /// Restituisce l'array delle parole chiave tra quelle da indicizzare
            /// </summary>
            /// <returns></returns>
            string[] GetKeyWords();
        }
    }
}