
namespace minidom.S300
{

    /// <summary>
    /// Flag per <see cref="S300Config"/>
    /// </summary>
    public enum WGOptions : int
    {
        /// <summary>
        /// TODO Wiegand26
        /// </summary>
        Wiegand26 = 0,

        /// <summary>
        /// TODO EncryptedWiegand
        /// </summary>
        EncryptedWiegand = 1,

        /// <summary>
        /// TODO FixedWiegandAreaCode
        /// </summary>
        FixedWiegandAreaCode = 2
    }

    /// <summary>
    /// Struttura che racchiude le informazioni sulla configurazione del dispositivo <see cref="S300Device"/>
    /// </summary>
    public struct S300Config
    {

        /// <summary>
        /// TODO RingAllow
        /// </summary>
        public bool RingAllow;

        /// <summary>
        /// TODO RealtimeMode
        /// </summary>
        public bool RealtimeMode;

        /// <summary>
        /// Valore booleano che indica se il dispositivo può aggiornare in maniera "intelligente" in maniera automatica
        /// </summary>
        public bool AutoUpdateFingerprint;

        /// <summary>
        /// Restituisce o imposta un valore intero compreso tra 0 e 5 che definisce il volume dell'altoparlante
        /// </summary>
        public int SpeakerVolume;

        /// <summary>
        /// Restituisce o imposta il ritardo in secondi per il blocco del rele
        /// </summary>
        public int DoorLockDelay;

        /// <summary>
        /// TODO LockDelayTime
        /// </summary>
        public int LockDelayTime { get; set; }

        /// <summary>
        /// TODO AdminPassword
        /// </summary>
        public string AdminPassword { get; set; }

        /// <summary>
        /// TODO FixedWiegandAreaCode
        /// </summary>
        public int FixedWiegandAreaCode;


        /// <summary>
        /// TODO WiegandOption
        /// </summary>
        public WGOptions WiegandOption;

        /// <summary>
        /// Ritardo minimo (in minuti) tra due marcature consecutive
        /// </summary>
        public int MinDelayInOut;
    }
}