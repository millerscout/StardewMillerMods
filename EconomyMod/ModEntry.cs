using System.Collections.Generic;
using EconomyMod.Interface;
using StardewModdingAPI;

namespace EconomyMod
{
    public class ModEntry : Mod
    {
        private TaxationService taxation;
        public EconomyInterfaceHandler Interface { get; set; }
        public override void Entry(IModHelper helper)
        {
            Util.Config = helper.ReadConfig<ModConfig>();

            this.taxation = new TaxationService(helper, this.Monitor);
            this.Interface = new EconomyInterfaceHandler(helper, taxation);
        }
    }
}
