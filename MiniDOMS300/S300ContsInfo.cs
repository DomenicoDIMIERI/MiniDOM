
namespace minidom.S300
{

    /// <summary>
    /// Struttura che racchiude le informazioni sui contatori di sistema
    /// </summary>
    public struct S300CountsInfo
    {

        /// <summary>
        /// Numero di "persone" definite sul dispositivo
        /// </summary>
        public int PersonsCount;

        /// <summary>
        /// Numero di impronte digitali registrate sul dispositivo
        /// </summary>
        public int FingerPrintsCount;

        /// <summary>
        /// Numero di marcature di ingresso/uscita registrate sul dispositivo
        /// </summary>
        public int ClockingsCounts;
    }
}