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
    public partial class Office
    {

        /// <summary>
        /// Handler usato per cercare una persona in base al numero di targa dei veicoli che possiede
        /// </summary>
        public class FindPersonaByTargaVeicolo 
            : Anagrafica.FindPersonaHandler
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public FindPersonaByTargaVeicolo()
            {
            }

            private bool IsNuovaTarga(string targa)
            {
                if (Strings.Len(targa) != 7)
                    return false;
                if (char.IsLetter(DMD.Chars.CChar(Strings.Mid(targa, 1, 1))) && char.IsLetter(DMD.Chars.CChar(Strings.Mid(targa, 2, 1))) && char.IsLetter(DMD.Chars.CChar(Strings.Mid(targa, 6, 1))) && char.IsLetter(DMD.Chars.CChar(Strings.Mid(targa, 7, 1))))
                {
                    for (int i = 3; i <= 5; i++)
                    {
                        if (!char.IsNumber(DMD.Chars.CChar(Strings.Mid(targa, i, 1))))
                            return false;
                    }

                    return true;
                }

                return false;
            }

            /// <summary>
            /// Restituisce true se l'handler può gestire la stringa come targa
            /// </summary>
            /// <param name="targa"></param>
            /// <param name="filter"></param>
            /// <returns></returns>
            public override bool CanHandle(string targa, Anagrafica.CRMFindParams filter)
            {
                targa = Veicoli.ParseTarga(targa);
                return IsNuovaTarga(targa);
            }

            /// <summary>
            /// Cerca le persone corrispondenti alla targa
            /// </summary>
            /// <param name="targa"></param>
            /// <param name="filter"></param>
            /// <param name="ret"></param>
            public override void Find(string targa, Anagrafica.CRMFindParams filter, CCollection<Anagrafica.CPersonaInfo> ret)
            {
                var list = new ArrayList();
                int[] arr = null;
                targa = Veicoli.ParseTarga(targa);
                if (Strings.Len(Strings.Trim(targa)) < 3)
                    return;
                using (var cursor1 = new VeicoliCursor())
                {
                    cursor1.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor1.Targa.Value = DMD.Strings.JoinW(targa, "%");
                    cursor1.Targa.Operator = OP.OP_LIKE;
                    while (cursor1.Read())
                    {
                        var veicolo = cursor1.Item;
                        if (veicolo.IDProprietario != 0)
                            list.Add(veicolo.IDProprietario);
                    }
                }

                if (list.Count == 0)
                    return;
                using (var cursor = new CPersonaFisicaCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = filter.ignoreRights;
                    arr = (int[])list.ToArray(typeof(int));
                    cursor.ID.ValueIn(arr);
                    if (filter.IDPuntoOperativo != 0)
                        cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo;
                    if (filter.tipoPersona.HasValue)
                        cursor.TipoPersona.Value = filter.tipoPersona.Value;
                    if (filter.flags.HasValue)
                    {
                        cursor.PFlags.Value = filter.flags.Value;
                        cursor.PFlags.Operator = OP.OP_ALLBITAND;
                    }

                    int cnt = 0;
                    while (cursor.Read() && (filter.nMax.HasValue == false || cnt < filter.nMax.Value))
                    {
                        ret.Add(new Anagrafica.CPersonaInfo(cursor.Item));
                        cnt += 1;
                    }
                }
            }

            /// <summary>
            /// Restituisce il tipo del comando
            /// </summary>
            /// <returns></returns>
            public override string GetHandledCommand()
            {
                return "Targa Veicolo";
            }
        }
    }
}