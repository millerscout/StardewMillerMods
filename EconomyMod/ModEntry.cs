using StardewModdingAPI;

namespace EconomyMod
{
    public class ModEntry : Mod
    {
        private TaxationService taxation;

        private bool AskedForPaymentToday = false;

        public override void Entry(IModHelper helper)
        {
            Util.Config = helper.ReadConfig<ModConfig>();
            this.taxation = new TaxationService(helper, this.Monitor);
        }
    }
}
