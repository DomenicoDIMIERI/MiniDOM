using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Passaggio di stato usato nel cursore delle pratiche
        /// </summary>
        [Serializable]
        public class CInfoStato 
            : CIntervalloData
        {
            public int IDOperatore;
            public string NomeOperatore;
            public string Parameters;
            public string Note;
            public bool? Forzato;
            public int IDFromStato;
            public int IDToStato;
            public int IDOfferta;
            public int IDRegolaApplicata;
            public string NomeRegolaApplicata;
            public string Descrizione;
            public StatoPraticaEnum? MacroStato;

            /// <summary>
        /// Costruttore
        /// </summary>
            public CInfoStato()
            {
                IDOperatore = 0;
                NomeOperatore = "";
                Parameters = "";
                Note = "";
                Forzato = default;
                IDFromStato = 0;
                IDToStato = 0;
                IDOfferta = 0;
                IDRegolaApplicata = 0;
                NomeRegolaApplicata = "";
                Descrizione = "";
                MacroStato = default;
            }

            /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="ms"></param>
            public CInfoStato(StatoPraticaEnum ms) : this()
            {
                MacroStato = ms;
            }

            public override bool IsSet()
            {
                var intervallo = DMD.DateUtils.PeriodoToDates(Tipo, Inizio, Fine);
                Inizio = intervallo.Inizio;
                Fine = intervallo.Fine;
                return Inizio.HasValue || Fine.HasValue || IDOperatore != 0 || !string.IsNullOrEmpty(NomeOperatore) || !string.IsNullOrEmpty(Parameters) || !string.IsNullOrEmpty(Note) || Forzato.HasValue || IDFromStato != 0 || IDOfferta != 0 || IDRegolaApplicata != 0 || !string.IsNullOrEmpty(NomeRegolaApplicata) || !string.IsNullOrEmpty(Descrizione) || IDToStato != 0; // OrElse (Me.MacroStato.HasValue)
            }

            public override void Clear()
            {
                base.Clear();
                IDOperatore = 0;
                NomeOperatore = "";
                Parameters = "";
                Note = "";
                Forzato = default;
                IDFromStato = 0;
                IDOfferta = 0;
                IDRegolaApplicata = 0;
                NomeRegolaApplicata = "";
                Descrizione = "";
                IDToStato = 0;
                // Me.MacroStato = Nothing
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", NomeOperatore);
                writer.WriteAttribute("Forzato", Forzato);
                writer.WriteAttribute("IDFromStato", IDFromStato);
                writer.WriteAttribute("IDToStato", IDToStato);
                writer.WriteAttribute("IDOfferta", IDOfferta);
                writer.WriteAttribute("IDRegolaApplicata", IDRegolaApplicata);
                writer.WriteAttribute("NomeRegolaApplicata", NomeRegolaApplicata);
                writer.WriteAttribute("Descrizione", Descrizione);
                writer.WriteAttribute("MacroStato", MacroStato);
                writer.WriteAttribute("Parameters", Parameters);
                base.XMLSerialize(writer);
                writer.WriteTag("Note", Note);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "IDOperatore":
                        {
                            IDOperatore = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeOperatore":
                        {
                            NomeOperatore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Forzato":
                        {
                            Forzato = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue);
                            break;
                        }

                    case "IDFromStato":
                        {
                            IDFromStato = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDToStato":
                        {
                            IDToStato = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDOfferta":
                        {
                            IDOfferta = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "IDRegolaApplicata":
                        {
                            IDRegolaApplicata = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "NomeRegolaApplicata":
                        {
                            NomeRegolaApplicata = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Descrizione":
                        {
                            Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Note":
                        {
                            Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Parameters":
                        {
                            Parameters = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "MacroStato":
                        {
                            MacroStato = (StatoPraticaEnum?)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public override void InitializeFrom(object value)
            {
                {
                    var withBlock = (CInfoStato)value;
                    IDOperatore = withBlock.IDOperatore;
                    NomeOperatore = withBlock.NomeOperatore;
                    Parameters = withBlock.Parameters;
                    Note = withBlock.Note;
                    Forzato = withBlock.Forzato;
                    IDFromStato = withBlock.IDFromStato;
                    IDOfferta = withBlock.IDOfferta;
                    IDRegolaApplicata = withBlock.IDRegolaApplicata;
                    NomeRegolaApplicata = withBlock.NomeRegolaApplicata;
                    Descrizione = withBlock.Descrizione;
                    IDToStato = withBlock.IDToStato;
                }

                base.InitializeFrom(value);
            }

            protected override void _CopyFrom(object value)
            {
                var withBlock = (CInfoStato)value;
                IDOperatore = withBlock.IDOperatore;
                NomeOperatore = withBlock.NomeOperatore;
                Parameters = withBlock.Parameters;
                Note = withBlock.Note;
                Forzato = withBlock.Forzato;
                IDFromStato = withBlock.IDFromStato;
                IDOfferta = withBlock.IDOfferta;
                IDRegolaApplicata = withBlock.IDRegolaApplicata;
                NomeRegolaApplicata = withBlock.NomeRegolaApplicata;
                Descrizione = withBlock.Descrizione;
                IDToStato = withBlock.IDToStato;
                base._CopyFrom(value);
            }

            public string GetSQL()
            {
                var intervallo = DMD.DateUtils.PeriodoToDates(Tipo, Inizio, Fine);
                Inizio = intervallo.Inizio;
                Fine = intervallo.Fine;
                string wherePart = "";
                if (IDOperatore != 0)
                    wherePart = DMD.Strings.Combine(wherePart, "[tbl_PraticheSTL].[IDOperatore]=" + DBUtils.DBNumber(IDOperatore), " AND ");
                if (!string.IsNullOrEmpty(NomeOperatore))
                    wherePart = DMD.Strings.Combine(wherePart, "[tbl_PraticheSTL].[NomeOperatore]=" + DBUtils.DBString(NomeOperatore), " AND ");
                if (!string.IsNullOrEmpty(Parameters))
                    wherePart = DMD.Strings.Combine(wherePart, "[tbl_PraticheSTL].[Parameters]=" + DBUtils.DBString(Parameters), " AND ");
                if (!string.IsNullOrEmpty(Note))
                    wherePart = DMD.Strings.Combine(wherePart, "[tbl_PraticheSTL].[Note] Like '" + DMD.Strings.Replace(Note, "'", "''") + "%')", " And ");
                if (Forzato.HasValue)
                    wherePart = DMD.Strings.Combine(wherePart, "[tbl_PraticheSTL].[Forzato]=" + DBUtils.DBBool(Forzato.Value), " And ");
                if (IDFromStato != 0)
                    wherePart = DMD.Strings.Combine(wherePart, "[tbl_PraticheSTL].[IDFromStato]=" + DBUtils.DBNumber(IDFromStato), " AND ");
                if (IDToStato != 0)
                    wherePart = DMD.Strings.Combine(wherePart, "[tbl_PraticheSTL].[IDToStato]=" + DBUtils.DBNumber(IDToStato), " AND ");
                if (IDOfferta != 0)
                    wherePart = DMD.Strings.Combine(wherePart, "[tbl_PraticheSTL].[IDOfferta]=" + DBUtils.DBNumber(IDOfferta), " AND ");
                if (IDRegolaApplicata != 0)
                    wherePart = DMD.Strings.Combine(wherePart, "[tbl_PraticheSTL].[IDRegolaApplicata]=" + DBUtils.DBNumber(IDRegolaApplicata), " AND ");
                if (!string.IsNullOrEmpty(NomeRegolaApplicata))
                    wherePart = DMD.Strings.Combine(wherePart, "[tbl_PraticheSTL].[NomeRegolaApplicata]=" + DBUtils.DBString(NomeRegolaApplicata), " AND ");
                if (!string.IsNullOrEmpty(Descrizione))
                    wherePart = DMD.Strings.Combine(wherePart, "[tbl_PraticheSTL].[Descrizione]=" + DBUtils.DBString(Descrizione), " AND ");
                if (MacroStato.HasValue)
                    wherePart = DMD.Strings.Combine(wherePart, "[tbl_PraticheSTL].[MacroStato]=" + DBUtils.DBNumber(MacroStato.Value), " AND ");
                if (Fine.HasValue)
                    Fine = DMD.DateUtils.DateAdd(DateTimeInterval.Second, 3600 * 24 - 1, DMD.DateUtils.GetDatePart(Fine.Value));
                if (Inizio.HasValue)
                    wherePart = DMD.Strings.Combine(wherePart, "([tbl_PraticheSTL].[Data] >= " + DBUtils.DBDate(Inizio.Value) + ")", " AND ");
                if (Fine.HasValue)
                    wherePart = DMD.Strings.Combine(wherePart, "([tbl_PraticheSTL].[Data] <= " + DBUtils.DBDate(Fine.Value) + ")", " AND ");
                return wherePart;
            }

            public string GetCampoSQL()
            {
                var intervallo = DMD.DateUtils.PeriodoToDates(Tipo, Inizio, Fine);
                Inizio = intervallo.Inizio;
                Fine = intervallo.Fine;
                if (IsSet() == false)
                    return "";
                string wherePart = "[Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
                if (IDOperatore != 0)
                    wherePart = DMD.Strings.Combine(wherePart, "[IDOperatore]=" + DBUtils.DBNumber(IDOperatore), " AND ");
                if (!string.IsNullOrEmpty(NomeOperatore))
                    wherePart = DMD.Strings.Combine(wherePart, "[NomeOperatore]=" + DBUtils.DBString(NomeOperatore), " AND ");
                if (!string.IsNullOrEmpty(Parameters))
                    wherePart = DMD.Strings.Combine(wherePart, "[Parameters]=" + DBUtils.DBString(Parameters), " AND ");
                if (!string.IsNullOrEmpty(Note))
                    wherePart = DMD.Strings.Combine(wherePart, "[Note] Like '%" + DMD.Strings.Replace(Note, "'", "''") + "%')", " And ");
                if (Forzato.HasValue)
                    wherePart = DMD.Strings.Combine(wherePart, "[Forzato]=" + DBUtils.DBBool(Forzato.Value), " And ");
                if (IDFromStato != 0)
                    wherePart = DMD.Strings.Combine(wherePart, "[IDFromStato]=" + DBUtils.DBNumber(IDFromStato), " AND ");
                if (IDToStato != 0)
                    wherePart = DMD.Strings.Combine(wherePart, "[IDToStato]=" + DBUtils.DBNumber(IDToStato), " AND ");
                if (IDOfferta != 0)
                    wherePart = DMD.Strings.Combine(wherePart, "[IDOfferta]=" + DBUtils.DBNumber(IDOfferta), " AND ");
                if (IDRegolaApplicata != 0)
                    wherePart = DMD.Strings.Combine(wherePart, "[IDRegolaApplicata]=" + DBUtils.DBNumber(IDRegolaApplicata), " AND ");
                if (!string.IsNullOrEmpty(NomeRegolaApplicata))
                    wherePart = DMD.Strings.Combine(wherePart, "[NomeRegolaApplicata]=" + DBUtils.DBString(NomeRegolaApplicata), " AND ");
                if (!string.IsNullOrEmpty(Descrizione))
                    wherePart = DMD.Strings.Combine(wherePart, "[Descrizione]=" + DBUtils.DBString(Descrizione), " AND ");
                if (MacroStato.HasValue)
                    wherePart = DMD.Strings.Combine(wherePart, "[MacroStato]=" + DBUtils.DBNumber(MacroStato.Value), " AND ");
                if (Fine.HasValue)
                    Fine = DMD.DateUtils.DateAdd(DateTimeInterval.Second, 3600 * 24 - 1, DMD.DateUtils.GetDatePart(Fine.Value));
                if (Inizio.HasValue)
                    wherePart = DMD.Strings.Combine(wherePart, "([Data] >= " + DBUtils.DBDate(Inizio.Value) + ")", " AND ");
                if (Fine.HasValue)
                    wherePart = DMD.Strings.Combine(wherePart, "([Data] <= " + DBUtils.DBDate(Fine.Value) + ")", " AND ");
                return "([ID] In (SELECT [IDPratica] FROM [tbl_PraticheSTL] WHERE " + wherePart + "))";
            }
        }
    }
}