using System;

namespace minidom.PBX
{

    /// <summary>
    /// Evento di digitazione
    /// </summary>
    [Serializable]
    public class DialEventArgs
        : DMDEventArgs
    {

        /// <summary>
        /// Numero digitato
        /// </summary>
        public readonly string Number;

        /// <summary>
        /// Costruttore
        /// </summary>
        public DialEventArgs()
        {
            Number = "";
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        public DialEventArgs(string number)
        {
            Number = number;
        }
    }

    /// <summary>
    /// Evento generato quando inizia la composizione di un numero
    /// </summary>
    public delegate void BeginDialEventHandler(object sender, DialEventArgs e);

    /// <summary>
    /// Evento generato quando un numero è stato composto 
    /// </summary>
    public delegate void EndDialEventHandler(object sender, DialEventArgs e);

}