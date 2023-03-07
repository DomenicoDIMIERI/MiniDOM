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

namespace minidom.PBX
{

    /// <summary>
    /// Repository di <see cref="DMDSIPConfig"/>
    /// </summary>
    [Serializable]
    public sealed class DMDSIPConfigRepository 
        : CModulesClass<DMDSIPConfig>
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public DMDSIPConfigRepository() 
            : base("modDialTPConfig", typeof(DMDSIPConfigCursor), -1)
        {
        }

        public DMDSIPConfig GetConfiguration(string machine, string user)
        {
            machine = DMD.Strings.Trim(machine);
            user = DMD.Strings.Trim(user);
            
            if (string.IsNullOrEmpty(machine) || string.IsNullOrEmpty(user))
                throw new ArgumentNullException();

            foreach (DMDSIPConfig c in LoadAll())
            {
                if (
                    c.Attiva 
                    && DMD.Strings.Compare(c.IDPostazione, machine, true) == 0 
                    && DMD.Strings.Compare(c.UserName, user, true) == 0)
                {
                    return c;
                }
            }

            using (var cursor = new DMDSIPConfigCursor())
            {  
                // cursor.IDMacchina.Value = mn
                cursor.IDPostazione.Value = machine;
                cursor.IDUtente.Value = user;
                cursor.Attiva.SortOrder = Databases.SortEnum.SORT_ASC;
                // cursor.MinVersion.Value = appver
                // cursor.MinVersion.Operator = OP.OP_GE
                // cursor.MinVersion.IncludeNulls = True

                // cursor.MaxVersion.Value = appver
                // cursor.MaxVersion.Operator = OP.OP_LE
                // cursor.MaxVersion.IncludeNulls = True
                cursor.IgnoreRights = true;
                return cursor.Item;
            }
             
        }
    }
}