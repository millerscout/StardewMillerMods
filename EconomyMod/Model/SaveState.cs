namespace EconomyMod.Model
{
    public class SaveState
    {
        public bool CalculatedUsableSoil = false;
        public byte PostPoneDaysLeftDefault = 1;
        public byte PostPoneDaysLeft = 1;
        public int PendingTaxAmount = 0;
        public int UsableSoil;
    }
}
