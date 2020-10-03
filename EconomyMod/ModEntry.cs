using System.Collections.Generic;
using EconomyMod.Interface;
using EconomyMod.Multiplayer;
using StardewModdingAPI;

namespace EconomyMod
{
    public class ModEntry : Mod
    {
        private TaxationService taxation;
        public EconomyInterfaceHandler Interface { get; set; }
        public MessageBroadcastService messageBroadcast { get; set; }
        public override void Entry(IModHelper helper)
        {
            Util.Config = helper.ReadConfig<ModConfig>();
            Util.ModManifest = this.ModManifest;
            Util.Monitor = this.Monitor;

            this.taxation = new TaxationService(helper);
            this.Interface = new EconomyInterfaceHandler(helper, taxation);
            this.messageBroadcast = new MessageBroadcastService(helper, taxation);

        }
    }
}
