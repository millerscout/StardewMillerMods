using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EconomyMod.Model;
using Microsoft.Xna.Framework;
using Pathoschild.Stardew.LookupAnything.Framework.ItemScanning;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using xTile.Layers;

namespace EconomyMod
{
    public class TaxationService
    {
        private IModHelper Helper;
        private IMonitor Monitor;
        public SaveState State;
        private WorldItemScanner WorldItemScanner;
        public LotValue LotValue;

        internal bool AskedForPaymentToday { get; set; }
        public TaxationService(IModHelper helper, IMonitor monitor)
        {
            this.Helper = helper;
            this.Monitor = monitor;

            helper.Events.GameLoop.DayStarted += this.GameLoop_DayStarted;
            helper.Events.GameLoop.DayEnding += this.DayEnding;
            helper.Events.GameLoop.ReturnedToTitle += this.GameLoop_ReturnedToTitle;

            LotValue = new LotValue();
            LotValue.AddDefaultLotValue();
            if (Util.Config.IncludeOwnedObjectsOnLotValue)
            {
                WorldItemScanner = new WorldItemScanner(helper.Reflection);

                LotValue.Add(() =>
                {
                    var items = WorldItemScanner.GetAllOwnedItems();

                    var ItemPrices = items.Select(c =>
                        c.Item.GetType().GetProperty("Price") != null ? (int)c.Item.GetType().GetProperty("Price").GetValue(c.Item) : c.Item.salePrice()
                    );

                    return ItemPrices.Sum();
                });
            }
        }

        private void GameLoop_ReturnedToTitle(object sender, ReturnedToTitleEventArgs e)
        {
            this.AskedForPaymentToday = false;
        }

        private void DayEnding(object sender, DayEndingEventArgs e)
        {
            this.AskedForPaymentToday = false;
            Helper.Data.WriteJsonFile(Path.Combine("Save", $"{Game1.player.displayName}_{Game1.uniqueIDForThisGame}_data"), State);
        }

        private void GameLoop_DayStarted(object sender, DayStartedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            State = Helper.Data.ReadJsonFile<SaveState>(Path.Combine("Save", $"{Game1.player.displayName}_{Game1.uniqueIDForThisGame}_data"));
            if (State == null)
                State = new SaveState();

            if (!Game1.player.useSeparateWallets && !Game1.player.IsLocalPlayer)
            {
                return;
            }
            if (!this.AskedForPaymentToday)
            {
                int CurrentLotValue = this.CalculateLotValue();
                this.Monitor.Log($"{Helper.Translation.Get("PostponedPaymentText")}: {State.PendingTaxAmount}.", LogLevel.Info);
                this.Monitor.Log($"{Helper.Translation.Get("CurrentLotValueText")}: {CurrentLotValue}.", LogLevel.Info);

                int Tax = CurrentLotValue / 28 / 4 + State.PendingTaxAmount;
                this.Monitor.Log($"[Hardcoded for now] {Helper.Translation.Get("PaymentModeText")}: {Helper.Translation.Get("DailyText")}, {Helper.Translation.Get("TaxValueText")}: {Tax}", LogLevel.Info);
                this.Monitor.Log($"{Helper.Translation.Get("SeparateWalletsText")}: {Game1.player.useSeparateWallets}", LogLevel.Info);
                if (Game1.player.useSeparateWallets)
                {

                    int validFarmers = Game1.getAllFarmers().Select(c => c.name).Where(c => !string.IsNullOrEmpty(c)).Count();

                    this.Monitor.Log($"{Helper.Translation.Get("ValidFarmersText")}: {validFarmers}", LogLevel.Info);
                    Tax /= validFarmers;
                    this.Monitor.Log($"{Helper.Translation.Get("TaxEachFarmerText")}: {Tax}", LogLevel.Info);
                }

                if (Game1.player.Money - Tax <= 0 || Game1.player.Money == 0)
                {
                    PostponePayment(Tax);
                    return;
                }
                if (State.PostPoneDaysLeft == 0)
                {
                    this.PayTaxes(Tax);
                    return;
                }


                if (Tax * 100 / Game1.player.Money >= Util.Config.ThresholdInPercentageToAskAboutPayment)
                {
                    Response[] responses = {
                    new Response ("A", $"{Helper.Translation.Get("PayText")} ( {Tax} )G"),
                    new Response ("B", $"{Helper.Translation.Get("PostponeText")} ( {Tax+Tax/5 } ) G")
                };
                    Game1.currentLocation.createQuestionDialogue($"{Helper.Translation.Get("TaxAboveThresholdText")}", responses, (Farmer _, string answer) =>
                    {
                        switch (answer.Split(' ')[0])
                        {
                            case "A":
                                this.PayTaxes(Tax);
                                break;

                            case "B":
                                this.PostponePayment(Tax);
                                break;
                        }
                    });

                }
                else
                {
                    this.PayTaxes(Tax);
                }
                this.AskedForPaymentToday = true;
            }
        }

        /// <summary>Get a rectangular grid of tiles.</summary>
        /// <param name="x">The X coordinate of the top-left tile.</param>
        /// <param name="y">The Y coordinate of the top-left tile.</param>
        /// <param name="width">The grid width.</param>
        /// <param name="height">The grid height.</param>
        /// Code Taken from Pathoschild's DebugMod.
        public IEnumerable<Vector2> GetTiles(int x, int y, int width, int height)
        {
            for (int curX = x, maxX = x + width - 1; curX <= maxX; curX++)
            {
                for (int curY = y, maxY = y + height - 1; curY <= maxY; curY++)
                    yield return new Vector2(curX, curY);
            }
        }

        private int CalculateLotValue()
        {
            var farm = Game1.getFarm();

            CalculateUsableSoil();

            int depreciationPercentage = (100 - (State.UsableSoil - CalculateDepreciation()) * 100 / State.UsableSoil);
            return depreciationPercentage > 0 ? LotValue.Sum / depreciationPercentage : LotValue.Sum;

            int CalculateDepreciation()
            {
                int depreciationObjectsCount = 0;
                foreach (int item in Util.Config.ListOfDepreciationObjects)
                {
                    depreciationObjectsCount += farm.numberOfObjectsOfType(item, false);
                }
                return depreciationObjectsCount;
            }

            void CalculateUsableSoil()
            {
                if (!State.CalculatedUsableSoil || State.UsableSoil == 0)
                {
                    if (State.UsableSoil > 0) State.UsableSoil = 0;
                    Layer layer = farm.Map.GetLayer("Back");
                    foreach (var tile in this.GetTiles(0, 0, layer.LayerWidth, layer.LayerHeight))
                    {
                        if (farm.doesTileHaveProperty((int)tile.X, (int)tile.Y, "Diggable", "Back") != null)
                        {
                            State.UsableSoil++;
                        }
                    }
                    this.Monitor.Log($"Detected {State.UsableSoil} usable soil.", LogLevel.Info);
                    State.CalculatedUsableSoil = true;

                    /// Failover when we couldn't calculate.
                    if (State.UsableSoil == 0) State.UsableSoil = 3687;

                }
            }

        }

        internal void PayTaxes(int Tax = 0)
        {
            if (Tax == 0) Tax = State.PendingTaxAmount;
            Game1.player.Money = Math.Max(0, Game1.player.Money - Tax);
            State.PendingTaxAmount = 0;
            State.PostPoneDaysLeft = State.PostPoneDaysLeftDefault;
            Game1.addHUDMessage(new HUDMessage(Helper.Translation.Get("TaxPaidText").ToString().Replace("#Tax#", $"{Tax}"), 2));

        }

        private void PostponePayment(int tax)
        {
            Game1.addHUDMessage(new HUDMessage(Helper.Translation.Get("PostponedPaymentText"), 2));
            if (State.PostPoneDaysLeft > 0)
                State.PostPoneDaysLeft -= 1;
            State.PendingTaxAmount += State.PendingTaxAmount / 5;
            State.PendingTaxAmount += tax + tax / 5;
            Game1.chatBox.addInfoMessage(Helper.Translation.Get("PostponeChatText").ToString().Replace("#playerName#", Game1.player.displayName).Replace("#Tax#", $"{State.PendingTaxAmount}"));

        }
    }
}
