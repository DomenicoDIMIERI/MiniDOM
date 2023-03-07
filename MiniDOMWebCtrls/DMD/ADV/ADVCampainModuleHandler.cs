using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{
    public class ADVCampainModuleHandler 
        : CBaseModuleHandler
    {
        public ADVCampainModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new ADV.CCampagnaPubblicitariaCursor();
        }

        public string UpdateResult(object renderer)
        {
            int id = (int)this.n2int(renderer, "id");
            var item = ADV.RisultatiCampagna.GetItemById(id);
            item.Update();
            var serializer = new XMLSerializer();
            try
            {
                return serializer.Serialize(item, XMLSerializeMethod.Document);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                // serializer.Dispose()
            }
        }

        public string SendAgain(object renderer)
        {
            int id = (int)this.n2int(renderer, "id");
            var item = ADV.RisultatiCampagna.GetItemById(id);
            item.Invia();
            var serializer = new XMLSerializer();
            try
            {
                return serializer.Serialize(item, XMLSerializeMethod.Document);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                // serializer.Dispose()
            }
        }

        public string UpdateCampagna(object renderer)
        {
            int id = (int)this.n2int(renderer, "id");
            if (id == 0)
                return "";

            using (var c = new ADV.CRisultatoCampagnaCursor())
            {
                c.IDCampagna.Value = id;
                c.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
                while (!c.EOF())
                {
                    try
                    {
                        c.Item.Update();
                    }
                    catch (Exception ex)
                    {
                        Sistema.Events.NotifyUnhandledException(ex);
                    }

                    c.MoveNext();
                }

            }

            return id.ToString();
        }

        public string GetListaDiInvio(object renderer)
        {
            string text = this.n2str(renderer, "item", "");
            ADV.CCampagnaPubblicitaria camp = (ADV.CCampagnaPubblicitaria)DMD.XML.Utils.Serializer.Deserialize(text);
            return DMD.XML.Utils.Serializer.Serialize(camp.GetListaDiInvio());
        }

        public string SendCampagna(object renderer)
        {
            WebSite.ASP_Server.ScriptTimeout = 20 * 60;
            int id = (int)this.n2int(renderer, "id");
            var camp = ADV.Campagne.GetItemById(id);
            CCollection<ADV.CRisultatoCampagna> lista = null;
            string lst = this.n2str(renderer, "lista", "");
            if (!string.IsNullOrEmpty(lst))
            {
                lista = (CCollection<ADV.CRisultatoCampagna>)DMD.XML.Utils.Serializer.Deserialize(lst);
            }
            else
            {
                lista = camp.GetListaDiInvio();
            }

            camp.Invia(lista);
            return "";
        }

        public string GetLotti(object renderer)
        {
            int id = (int)this.n2int(renderer, "id");
            var camp = ADV.Campagne.GetItemById(id);
            return DMD.XML.Utils.Serializer.Serialize(camp.GetLotti());
        }

        public string GetConfig(object renderer)
        {
            return DMD.XML.Utils.Serializer.Serialize(ADV.Configuration);
        }

        public string SaveConfig(object renderer)
        {
            string text = this.n2str(renderer, "text", "");
            ADV.ADVConfig config = (ADV.ADVConfig)DMD.XML.Utils.Serializer.Deserialize(text);
            config.Save(true);
            return "";
        }

        // Public Overrides Function ExecuteAction(renderer As Object, actionName As String) As String
        // Select Case actionName
        // Case "GetConfig" : Return Me.GetConfig
        // Case "SaveConfig" : Return Me.SaveConfig
        // Case Else : Return MyBase.ExecuteAction(renderer, actionName)
        // End Select

        // End Function

    }
}