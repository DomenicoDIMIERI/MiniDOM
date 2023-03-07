using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{
    public class PreventiviModuleHandler : CBaseModuleHandler
    {
        public PreventiviModuleHandler() : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SEdit | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SAnnotations)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CPreventivoCursor();
        }

        public string GetOfferteByPreventivo(object renderer)
        {
            int pid = (int)this.n2int(renderer, "pid");
            var preventivo = Finanziaria.Preventivi.GetItemById(pid);
            if (preventivo.Offerte.Count > 0)
            {
                return DMD.XML.Utils.Serializer.Serialize(preventivo.Offerte.ToArray());
            }
            else
            {
                return "";
            }
        }

        public string Calc(object renderer)
        {
            string @params = this.n2str(renderer, "params");
            Finanziaria.CPreventivo p = (Finanziaria.CPreventivo)DMD.XML.Utils.Serializer.Deserialize(@params);
            p.Calcola();
            return DMD.XML.Utils.Serializer.Serialize(p, XMLSerializeMethod.Document);
        }
    }
}