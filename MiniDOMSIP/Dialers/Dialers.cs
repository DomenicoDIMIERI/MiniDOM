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


    public sealed class Dialers
    {
        // Private Shared m_InstalledDialers As Collection


        /// <summary>
        /// Restituisce la collezione dei diallers installati
        /// </summary>
        /// <returns></returns>
        public static CCollection<DialerBaseClass> GetInstalledDialers()
        {
            var m_InstalledDialers = new CCollection<DialerBaseClass>();
            var types = DMD.RunTime.GetAllDefinedTypesOf(typeof(DialerBaseClass));
            foreach (Type t in types)
            {
                d = (DialerBaseClass)Activator.CreateInstance(t);
                if (d.IsInstalled())
                    m_InstalledDialers.Add(d);
            }

            foreach (var disp in DMDSIPApp.CurrentConfig.Dispositivi)
            {
                switch (disp.Tipo ?? "")
                {
                    case "Cisco 7940":
                    case "Cisco 7960":
                        {
                            d = new CiscoDialer(disp.Indirizzo, disp.Nome);
                            m_InstalledDialers.Add(d);
                            break;
                        }
                }
            }

            foreach (AsteriskServer server in DMDSIPApp.CurrentConfig.AsteriskServers)
            {
                try
                {
                    if (!server.IsConnected())
                        server.Connect();
                    d = new AsteriskDialer(server);
                    m_InstalledDialers.Add(d);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.StackTrace);
                }
            }

            return m_InstalledDialers;
        }
    }
}