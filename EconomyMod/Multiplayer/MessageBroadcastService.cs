using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EconomyMod.Multiplayer.Messages;
using StardewModdingAPI;
using StardewValley;

namespace EconomyMod.Multiplayer
{
    public class MessageBroadcastService
    {
        private IModHelper helper;
        private TaxationService taxation;
        private Dictionary<BroadcastType, Action<BroadcastMessage>> broadcastActions;

        public MessageBroadcastService(IModHelper helper, TaxationService taxation)
        {
            this.helper = helper;
            this.taxation = taxation;

            helper.Events.Multiplayer.ModMessageReceived += Multiplayer_ModMessageReceived;

            this.taxation.OnPayTaxesCompleted += Taxation_OnPayTaxesCompleted;
            this.taxation.OnPostPoneTaxesCompleted += Taxation_OnPostPoneTaxesCompleted;


            this.broadcastActions = new Dictionary<BroadcastType, Action<BroadcastMessage>>() {
                {  BroadcastType.Taxation_Paid, PaidTaxes},
                {  BroadcastType.Taxation_Postpone, PostPone},

            };

        }

        private void Taxation_OnPostPoneTaxesCompleted(object sender, int tax)
        {
            if (Util.IsValidMultiplayer)
            {
                BroadcastMessage message = new BroadcastMessage()
                {
                    Tax = tax,
                    DisplayName = Game1.player.displayName
                };
                helper.Multiplayer.SendMessage(message, $"{BroadcastType.Taxation_Postpone}", modIDs: new[] { Util.ModManifest.UniqueID });
            }
        }

        private void Taxation_OnPayTaxesCompleted(object sender, int tax)
        {
            if (Util.IsValidMultiplayer)
            {
                BroadcastMessage message = new BroadcastMessage()
                {
                    Tax = tax,
                    DisplayName = Game1.player.displayName
                };
                helper.Multiplayer.SendMessage(message, $"{BroadcastType.Taxation_Paid}", modIDs: new[] { Util.ModManifest.UniqueID });
            }
        }

        private void Multiplayer_ModMessageReceived(object sender, StardewModdingAPI.Events.ModMessageReceivedEventArgs e)
        {

            if (Util.IsValidMultiplayer)
            {
                //from playerid
                if (e.FromModID == Util.ModManifest.UniqueID)
                {
                    var message = e.ReadAs<Messages.BroadcastMessage>();
                    broadcastActions[message.Type](message);

                }
            }
        }

        private void PaidTaxes(BroadcastMessage message)
        {
            Game1.chatBox.addInfoMessage($"{message.DisplayName} paid his taxes");
        }
        private void PostPone(BroadcastMessage message)
        {
            Game1.chatBox.addInfoMessage(helper.Translation.Get("PostponeChatText").ToString().Replace("#playerName#", message.DisplayName).Replace("#Tax#", $"{message.Tax}"));
        }
    }
}
