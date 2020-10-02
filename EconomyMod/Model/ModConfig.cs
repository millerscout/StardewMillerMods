namespace EconomyMod
{
    public class ModConfig
    {
        public int LotPrice { get; set; } = 1500000;
        public TaxPaymentType TaxPaymentType { get; set; }
        public byte ThresholdInPercentageToAskAboutPayment { get; set; } = 60;

        public int[] ListOfDepreciationObjects = new int[] {
            746, //Jack O Lantern
            747, //RottenPlant
            748, //RottenPlant
            784, //Weeds
            785, //Weeds
            786, //Weeds
            674, //Weeds
            675, //Weeds
            676, //Weeds
            677, //Weeds
            678, //Weeds
            679, //Weeds,
            295, //Twig
            450, //Stone
        };
    }
}
