using DMD;

namespace minidom
{
    public partial class CustomerCalls
    {


        /// <summary>
        /// Propone un nuovo contatto
        /// </summary>
        /// <remarks></remarks>
        public class CAzioneChiama 
            : CAzioneProposta
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAzioneChiama()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="c"></param>
            public CAzioneChiama(CTelefonata c) 
                : base(c)
            {
            }

            /// <summary>
            /// Prepara le azioni
            /// </summary>
            /// <param name="c"></param>
            protected internal override void Initialize(CContattoUtente c)
            {
                CTelefonata tel = (CTelefonata)c;
                // MyBase.Descrizione = "Nuova telefonata a " & tel.NomePersona & " al N°" & tel.Numero & " dal " & Formats.FormatUserDate(tel.DataRicontatto)
                SetCommand("try { CustomerCalls.PlaceCall(" + Contatto.IDPersona + ", '" + Strings.Replace(tel.NumeroOIndirizzo, "'", @"\'") + "'); } catch (ex) { alert(ex); }");
                SetIsScript(true);
                base.Initialize(c);
            }
        }
    }
}