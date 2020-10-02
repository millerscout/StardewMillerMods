using StardewModdingAPI;

namespace EconomyMod
{
    public class ModEntry : Mod
    {
        private ModConfig Config { get; set; }
        private TaxationService taxation;

        private bool AskedForPaymentToday = false;

        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();
            this.taxation = new TaxationService(helper, this.Monitor, Config);
        }
    }
}
