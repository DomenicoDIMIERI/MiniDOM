using System;
using DMD;

namespace minidom.Forms
{
    public partial class Utils
    {
        public sealed class CCRMUtilsClas
        {
            public string CreateElencoUffici(Anagrafica.CUfficio selValue)
            {
                CCollection<Anagrafica.CUfficio> uffici;
                var writer = new System.Text.StringBuilder();
                if (Anagrafica.Uffici.Module.UserCanDoAction("list"))
                {
                    uffici = Anagrafica.Uffici.GetPuntiOperativi();
                }
                else
                {
                    uffici = Sistema.Users.CurrentUser.Uffici;
                }

                foreach (Anagrafica.CUfficio item in uffici)
                {
                    writer.Append("<option value=\"");
                    writer.Append(Databases.GetID(item));
                    writer.Append("\" ");
                    if (Databases.GetID(selValue) == Databases.GetID(item))
                        writer.Append("selected");
                    writer.Append(">");
                    writer.Append(DMD.WebUtils.HtmlEncode(item.Nome));
                    writer.Append("</option>");
                }

                return writer.ToString();
            }

            public string CreateElencoOperatori(Sistema.CUser selValue, bool onlyValid = true)
            {
                Sistema.CModule module;
                bool canList, canListOwn;
                int itemID;
                int value;
                Sistema.CGroup grp;
                var ret = new System.Text.StringBuilder();
                bool t;
                module = Finanziaria.Pratiche.Module;
                canList = module.UserCanDoAction("list");
                canListOwn = module.UserCanDoAction("list_own");
                value = Databases.GetID(selValue);
                t = false;
                grp = Sistema.Groups.GetItemByName("CRM");
                if (canList)
                {
                    // Otteniamo il gruppo Finanziaria
                    if (grp is object)
                    {
                        foreach (Sistema.CUser user in grp.Members)
                        {
                            if (onlyValid == false || user.UserStato == Sistema.UserStatus.USER_ENABLED)
                            {
                                itemID = Databases.GetID(user);
                                t = t | value == itemID;
                                ret = ret.Append("<option value=\"");
                                ret.Append(itemID);
                                ret.Append("\" ");
                                if (value == itemID)
                                    ret.Append("selected");
                                ret.Append(">");
                                ret.Append(DMD.WebUtils.HtmlEncode(user.Nominativo));
                                ret.Append("</option>");
                            }
                        }
                    }
                }
                else if (grp is object)
                {
                    foreach (Sistema.CUser user in grp.Members)
                    {
                        if (Sistema.Users.CurrentUser.Uffici.SameOffice(user))
                        {
                            if (onlyValid == false || user.UserStato == Sistema.UserStatus.USER_ENABLED)
                            {
                                itemID = Databases.GetID(user);
                                t = t | value == itemID;
                                ret.Append("<option value=\"");
                                ret.Append(itemID);
                                if (value == itemID)
                                    ret.Append("selected");
                                ret.Append(">");
                                ret.Append(DMD.WebUtils.HtmlEncode(user.Nominativo));
                                ret.Append("</option>");
                            }
                        }
                    }
                }

                if (selValue is object & t == false)
                {
                    itemID = Databases.GetID(selValue);
                    if (value == itemID)
                    {
                        ret.Append("<option value=\"");
                        ret.Append(itemID);
                        ret.Append("\" selected>");
                        ret.Append(selValue.Nominativo);
                        ret.Append("</option>");
                    }
                    else
                    {
                        ret.Append("<option value=\"");
                        ret.Append(value);
                        ret.Append("\">INVALID (");
                        ret.Append(value);
                        ret.Append(")</option>");
                    }
                }

                return ret.ToString();
            }

            public string CreateElencoCategorie(string selValue)
            {
                var items = new[] { "Urgente", "Importante", "Normale", "Poco importante" };
                return SystemUtils.CreateElenco(items, selValue);
            }

            public string GetIcon(Anagrafica.CPersona cliente)
            {
                string iconURL = "";
                if (cliente is object)
                    iconURL = cliente.IconURL;
                if (string.IsNullOrEmpty(iconURL))
                    return Sistema.ApplicationContext.BaseURL + "/minidom/widgets/images/default.GIF";
                return Sistema.ApplicationContext.BaseURL + "/minidom/widgets/websvc/getthumb.aspx?p=" + iconURL + "&w=32&h=32";
            }

            public string FormatOfferta(Finanziaria.COffertaCQS offerta)
            {
                if (offerta is null)
                    return "NULL";
                string testo = "";
                testo += "<table>";
                testo += "<tr>";
                testo += "<td>";
                var prodotto = offerta.Prodotto;
                if (prodotto is null)
                {
                    testo += "<b>?</b>";
                }
                else if (prodotto.IdTipoContratto == "C")
                {
                    testo += "<b>CQS&nbsp;</b>";
                }
                else
                {
                    testo += "<b>PD&nbsp;</b>";
                }

                testo += "<span class=\"blue\">" + Sistema.Formats.FormatValuta(offerta.Rata) + " €</span> x <span class=\"blue\">" + offerta.Durata + "</span> = " + Sistema.Formats.FormatValuta(offerta.MontanteLordo) + " €";
                testo += "</td>";
                testo += "</tr>";
                testo += "<tr>";
                testo += "<td>";
                testo += "<b>Netto:</b> <span class=\"blue\">" + Sistema.Formats.FormatValuta(offerta.NettoRicavo) + " €</span>, <b>TAN:</b> <span class=\"blue\">" + Sistema.Formats.FormatPercentage(offerta.TAN, 3) + " %</span> <b>TAEG:</b> <span class=\"blue\">" + Sistema.Formats.FormatPercentage(offerta.TAEG) + " %</span>";
                testo += "</td>";
                testo += "</tr>";
                testo += "</table>";
                return testo;
            }

            public string GetNominativoPersona(Anagrafica.CPersona persona)
            {
                string ret = "";
                if (persona is object)
                    ret = persona.Nominativo;
                if (string.IsNullOrEmpty(ret))
                {
                    if (persona is null)
                    {
                        ret = "Sconosciuto";
                    }
                    else
                    {
                        ret = DMD.RunTime.vbTypeName(persona) + "[" + Databases.GetID(persona) + "]";
                    }
                }

                return ret;
            }

            public string PreparaTestoNotifica(string titolo, Anagrafica.CPersona persona, Sistema.CUser operatore = null, DateTime? data = default)
            {
                string testo;
                testo = "";
                testo += "<table>";
                testo += "<tr>";
                testo += "<td><img src=\"" + GetIcon(persona) + "\" style=\"width:32px;height:32px;\" alt=\"img\" /></td>";
                testo += "<td>";
                testo += "<b style=\"color:red;font-size:12pt;\">" + titolo + "</b></br/>";
                testo += "<a style=\"color:blue;font-size:12pt;font-weight:bold;\" href=\"#\" onclick=\"SystemUtils.EditItem(Anagrafica.Persone.GetItemById(" + Databases.GetID(persona) + ")); return false;\">" + DMD.WebUtils.HtmlEncode(GetNominativoPersona(persona)) + "<a>";
                testo += "</td>";
                testo += "</tr>";
                testo += "</table>";
                if (data.HasValue)
                    testo += "<b class=\"font11pt\">Data:</b> <span class\"font11pt blue\">" + Sistema.Formats.FormatUserDateTime(DMD.DateUtils.Now()) + "</span>" + DMD.Strings.vbNewLine;
                if (operatore is object)
                    testo += "<b class=\"font11pt\">Operatore:</b> <span class\"blue font11pt\">" + DMD.WebUtils.HtmlEncode(operatore.Nominativo) + "</span>" + DMD.Strings.vbNewLine;
                return testo;
            }

            public string PreparaTestoNotificaURL(string titolo, Anagrafica.CPersona persona, Sistema.CUser operatore = null, DateTime? data = default)
            {
                string testo;
                string baseURL = Sistema.ApplicationContext.BaseURL;
                string baseName = "frm" + Sistema.ApplicationContext.IDAziendaPrincipale;
                testo = "";
                testo += "<table>";
                testo += "<tr>";
                testo += "<td><img src=\"" + GetIcon(persona) + "\" style=\"width:32px;height:32px;\" alt=\"img\" /></td>";
                testo += "<td>";
                testo += "<b style=\"color:red;font-size:12pt;\">" + titolo + "</b></br/>";
                testo += "<a style=\"color:blue;font-size:12pt;font-weight:bold;\" target=\"" + baseName + "\" href=\"" + baseURL + "modAnagrafica.aspx?_a=edit&id=" + Databases.GetID(persona) + "\">" + DMD.WebUtils.HtmlEncode(GetNominativoPersona(persona)) + "<a>";
                testo += "</td>";
                testo += "</tr>";
                testo += "</table>";
                if (data.HasValue)
                    testo += "<b class=\"font11pt\">Data:</b> <span class\"font11pt blue\">" + Sistema.Formats.FormatUserDateTime(DMD.DateUtils.Now()) + "</span>" + DMD.Strings.vbNewLine;
                if (operatore is object)
                    testo += "<b class=\"font11pt\">Operatore:</b> <span class\"blue font11pt\">" + DMD.WebUtils.HtmlEncode(operatore.Nominativo) + "</span>" + DMD.Strings.vbNewLine;
                return testo;
            }
        }

        private static CCRMUtilsClas m_CRMUtils = null;

        public static CCRMUtilsClas CRMUtils
        {
            get
            {
                if (m_CRMUtils is null)
                    m_CRMUtils = new CCRMUtilsClas();
                return m_CRMUtils;
            }
        }
    }
}