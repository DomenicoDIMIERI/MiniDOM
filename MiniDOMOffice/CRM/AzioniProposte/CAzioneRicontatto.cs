using DMD;

namespace minidom
{
    public partial class CustomerCalls
    {


        /// <summary>
        /// Propone un ricontatto
        /// </summary>
        /// <remarks></remarks>
        public class CAzioneRicontatto 
            : CAzioneProposta
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAzioneRicontatto()
            {
            }

            /// <summary>
            /// Inizializza il comando
            /// </summary>
            /// <param name="c"></param>
            protected internal override void Initialize(CContattoUtente c)
            {
                CTelefonata tel = (CTelefonata)c;
                base.Initialize(tel);
                // MyBase.Descrizione = "Ricontattare " & tel.NomePersona & " al N°" & tel.Numero & " dal " & Formats.FormatUserDate(tel.DataRicontatto)
                SetCommand("try { CustomerCalls.ReCall(" + DBUtils.GetID(Contatto, 0) + ", '" + Strings.Replace(tel.NumeroOIndirizzo, "'", @"\'") + "'); } catch (ex) { alert(ex); }");
                SetIsScript(true);
            }
        }
    }
}