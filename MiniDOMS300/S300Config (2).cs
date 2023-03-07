
namespace minidom.S300
{
    public enum WGOptions : int
    {
        Wiegand26 = 0,
        EncryptedWiegand = 1,
        FixedWiegandAreaCode = 2
    }

    /// <summary>
    /// Struttura che racchiude le informazioni sulla configurazione del dispositivo
    /// </summary>
    public struct S300Config
    {

        /// <summary>
        /// 
        /// </summary>
        public bool RingAllow;
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

        public int LockDelayTime { get; set; }
        public string AdminPassword { get; set; }

        public int FixedWiegandAreaCode;
        public WGOptions WiegandOption;

        /// <summary>
        /// Ritardo minimo (in minuti) tra due marcature consecutive
        /// </summary>
        public int MinDelayInOut;
    }
}