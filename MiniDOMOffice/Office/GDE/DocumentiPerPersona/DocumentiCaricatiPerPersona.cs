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
        /// Repository di <see cref="CDocCarPerPersona"/>
        /// </summary>
        [Serializable]
        public sealed class DocumentiCaricatiPerPersonaRepository
            : CModulesClass<CDocCarPerPersona>
        {
             
            /// <summary>
            /// Costruttore
            /// </summary>
            public DocumentiCaricatiPerPersonaRepository()
                : base("modGDEDocumentiXPersona", typeof(CDocCarPerPersonaCursor), 0)
            {

            }

            /// <summary>
            /// Restituisce la collezione dei documenti caricati per la persona
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            public  CCollection<CDocCarPerPersona> GetDocumentiCaricatiPerPersona(
                                                                        CPersona persona
                                                                        )
            {
                var ret = new CCollection<CDocCarPerPersona>();

                using (var cursor = new CDocCarPerPersonaCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDPersona.Value = DBUtils.GetID(persona, 0);
                    while (cursor.Read())
                    {
                        var item = cursor.Item;
                        item.SetPersona(persona);
                        ret.Add(item);
                    }
                }

                return ret;
            }

            /// <summary>
            /// Carica i documenti allegti per la persona
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            public  CCollection<CDocCarPerPersona> MatchDocumentiAttachmentsPersona(
                                                                                CPersona persona
                                                                                )
            {
                var noncaricati = GetDocumentiNonCaricatiPerPersona(persona);
                var attachments = new CAttachmentsCollection(persona);
                var ret = GetDocumentiCaricatiPerPersona(persona);
                foreach (var doc in noncaricati)
                {
                    foreach (var att in attachments)
                    {
                        if ((att.Tipo ?? "") == (doc.Documento.Nome ?? ""))
                        {
                            doc.Attachment = att;
                            break;
                        }
                    }

                    ret.Add(doc);
                }

                return ret;
            }

            /// <summary>
            /// Restituisce l'elenco dei documenti non caricati e previsti
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            public  CCollection<CDocCarPerPersona> GetDocumentiNonCaricatiPerPersona(
                                                            CPersona persona
                                                            )
            {
                CCollection<CDocCarPerPersona> caricati;
                var ret = new CCollection<CDocCarPerPersona>();
                using (var cursor = new DocumentiCaricatiCursor())
                {
                     
                    caricati = GetDocumentiCaricatiPerPersona(persona);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Nome.SortOrder = SortEnum.SORT_ASC;

                    while (cursor.Read())
                    {
                        bool t = false;
                        foreach (var item in caricati)
                        {
                            t = item.IDDocumento == DBUtils.GetID(cursor.Item, 0);
                            if (t)
                                break;
                        }

                        if (!t)
                        {
                            var doc = new CDocCarPerPersona();
                            doc.SetDocumento(cursor.Item);
                            doc.SetPersona(persona);
                        }
                         
                    }

                }

                return ret;
            }



#if (false)

public static CCollection<CDocCarPerPersona> GetDocumentiCaricatiPerPersona(Anagrafica.CPersona persona)
            {
                var cursor = new CDocCarPerPersonaCursor();
                var ret = new CCollection<CDocCarPerPersona>();
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.IDPersona.Value = DBUtils.GetID(persona);
                while (!cursor.EOF())
                {
                    ret.Add(cursor.Item);
                    cursor.MoveNext();
                }

                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }

                return ret;
            }

            public static CCollection<CDocCarPerPersona> MatchDocumentiAttachmentsPersona(Anagrafica.CPersona persona)
            {
                var noncaricati = GetDocumentiNonCaricatiPerPersona(persona);
                var attachments = new Sistema.CAttachmentsCollection(persona);
                var ret = GetDocumentiCaricatiPerPersona(persona);
                foreach (CDocCarPerPersona doc in noncaricati)
                {
                    foreach (Sistema.CAttachment att in attachments)
                    {
                        if ((att.Tipo ?? "") == (doc.Documento.Nome ?? ""))
                        {
                            doc.Attachment = att;
                            break;
                        }
                    }

                    ret.Add(doc);
                }

                return ret;
            }

            public static CCollection<CDocCarPerPersona> GetDocumentiNonCaricatiPerPersona(Anagrafica.CPersona persona)
            {
                CCollection<CDocCarPerPersona> caricati;
                var ret = new CCollection<CDocCarPerPersona>();
                var cursor = new DocumentiCaricatiCursor();
                bool t;
                caricati = GetDocumentiCaricatiPerPersona(persona);
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.Nome.SortOrder = SortEnum.SORT_ASC;
                while (!cursor.EOF())
                {
                    t = false;
                    foreach (CDocCarPerPersona item in caricati)
                    {
                        t = item.IDDocumento == DBUtils.GetID(cursor.Item);
                        if (t)
                            break;
                    }

                    if (!t)
                    {
                        var doc = new CDocCarPerPersona();
                        doc.Documento = cursor.Item;
                        doc.Persona = persona;
                    }

                    cursor.MoveNext();
                }

                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }

                return ret;
            }

           
#endif

        }

    }

    public partial class Office
    {

        private static DocumentiCaricatiPerPersonaRepository _DocumentiCaricatiPerPersona = null;

        /// <summary>
        /// Repository di <see cref="CDocCarPerPersona"/>
        /// </summary>
        public static DocumentiCaricatiPerPersonaRepository DocumentiCaricatiPerPersona
        {
            get
            {
                if (_DocumentiCaricatiPerPersona is null)
                    _DocumentiCaricatiPerPersona = new DocumentiCaricatiPerPersonaRepository();
                return _DocumentiCaricatiPerPersona;
            }
        }
    }
}