using System;
using minidom;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CCQSPDProvvigioneXOffertaUrlEvaluator : CCQSPDProvvigioneXOffertaEvaluator
        {
            public override decimal Evaluate(CCQSPDProvvigioneXOfferta pxo, COffertaCQS offerta, CCollection<EstinzioneXEstintore> estinzioni)
            {
                var @params = pxo.Parameters;
                string url = DMD.Strings.Trim(DMD.Strings.CStr(@params.GetItemByKey("url")));
                if (string.IsNullOrEmpty(url))
                    throw new ArgumentNullException("ArgumentNullException: url");
                string tmp = Sistema.RPC.InvokeMethod(url, "p", Sistema.RPC.str2n(DMD.XML.Utils.Serializer.Serialize(offerta)), "pxo", Sistema.RPC.str2n(DMD.XML.Utils.Serializer.Serialize(pxo)), "est", Sistema.RPC.str2n(DMD.XML.Utils.Serializer.Serialize(estinzioni)));
                return (decimal)DMD.XML.Utils.Serializer.DeserializeValuta(tmp);
            }
        }
    }
}