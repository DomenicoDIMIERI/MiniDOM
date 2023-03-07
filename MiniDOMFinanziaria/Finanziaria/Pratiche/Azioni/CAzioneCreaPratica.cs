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
    public partial class Finanziaria
    {


        /// <summary>
        /// Propone la creazione di una pratica
        /// </summary>
        /// <remarks></remarks>
        public class CAzioneCreaPratica 
            : CAzioneProposta
        {

            /// <summary>
            /// Inizializza il comando
            /// </summary>
            /// <param name="c"></param>
            protected internal override void Initialize(CContattoUtente c)
            {
                CTelefonata tel = (CTelefonata)c;
                base.Initialize(c);
                Descrizione = "Inizia pratica";
                SetCommand("/modPraticheCQSPD.aspx?_a=ContactToPratica&cid=" + DBUtils.GetID(tel, 0));
                SetIsScript(false);
            }
        }
    }
}