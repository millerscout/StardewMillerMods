using System;
using System.Collections.Generic;
using EconomyMod;
using EconomyMod.Interface;
using EconomyMod.Interface.PageContent;
using EconomyMod.Interface.Submenu;
using EconomyMod.Multiplayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

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
            Util.Helper = helper;

            this.taxation = new TaxationService();
            var framework = new UIFramework();
            framework.AddNewPage(() => new EconomyPageRework(
                framework,
                Util.Helper.Content.Load<Texture2D>($"assets/Interface/tabIcon.png"),
                Util.Helper.Translation.Get("BalanceReportText"),
                taxation), 0, new SidetabData(Util.Helper.Content.Load<Texture2D>($"assets/Interface/sidebarButtonReport.png"), "Tax payment and report", 0));

            framework.AddNewPage(() => new LoanPageRework(framework, taxation), 0, new SidetabData(Util.Helper.Content.Load<Texture2D>($"assets/Interface/LoanButton.png"), "Loan", 0));



            this.messageBroadcast = new MessageBroadcastService(taxation);

#if DEBUG
            Util.IsDebug = true;
            Monitor.Log("-----------------------DEBUG MODE-----------------------", LogLevel.Alert);
            this.Helper.Events.GameLoop.DayStarted += (object sender, StardewModdingAPI.Events.DayStartedEventArgs e) => Game1.addHUDMessage(new HUDMessage("DEBUG MODE", 2));

            helper.Events.Input.ButtonPressed += (object sender, ButtonPressedEventArgs e) =>
             {
                 if (!Context.IsWorldReady)
                     return;

                 if (e.Button == SButton.F)
                 {
                     InterfaceHelper.DrawGuidelines = !InterfaceHelper.DrawGuidelines;
                 }
                 if (e.Button == SButton.G)
                 {
                     var message = new Multiplayer.Messages.BroadcastMessage();
                     Util.Helper.Multiplayer.SendMessage(message, "nil", modIDs: new[] { Util.ModManifest.UniqueID });

                 }
             };
#endif
        }

    }


}

public class LoanPageRework : Page
{
    public LoanPageRework(UIFramework ui, TaxationService taxation) : base(ui)
    {
        LoanButton = new ClickableComponent(InterfaceHelper.GetButtonSizeForPage(this), "", "_____________");

        for (int i = 0; i < 7; ++i)
            Slots.Add(new ClickableComponent(
                new Rectangle(
                    xPositionOnScreen + Game1.tileSize / 4,
                    yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + i * (height - Game1.tileSize * 2) / 7,
                    width - Game1.tileSize / 2,
                    (height - Game1.tileSize * 2) / 7 + Game1.pixelZoom),
                i.ToString()));

        Draw = () =>
        {

            int currentItemIndex = 0;


            for (int i = 0; i < Slots.Count; ++i)
            {
                InterfaceHelper.Draw(Slots[i].bounds);
                if (currentItemIndex >= 0 &&
                    currentItemIndex + i < Elements.Count)
                {
                    Elements[currentItemIndex + i].Draw(Game1.spriteBatch, Slots[i].bounds.X, Slots[i].bounds.Y);
                }
            }

            if (taxation.State.PendingTaxAmount != 0)
            {
                IClickableMenu.drawTextureBox(Game1.spriteBatch, Game1.mouseCursors, new Rectangle(432, 439, 9, 9), LoanButton.bounds.X, LoanButton.bounds.Y, LoanButton.bounds.Width, LoanButton.bounds.Height, (LoanButton.scale > 0f) ? Color.Wheat : Color.White, 4f);
                var btnPosition = new Vector2(LoanButton.bounds.Center.X, LoanButton.bounds.Center.Y + 4) - Game1.dialogueFont.MeasureString("Loan Funds - Pelican Town 10000g") / 2f;
                Utility.drawTextWithShadow(Game1.spriteBatch, "Loan Funds - Pelican Town 10000g", Game1.dialogueFont, btnPosition, Game1.textColor, 1f, -1f, -1, -1, 0f);

                InterfaceHelper.Draw(LoanButton.bounds, center: true);
                InterfaceHelper.Draw(btnPosition, InterfaceHelper.InterfaceHelperType.TextInsideButton);
            }
        };
        this.LeftClickAction += Leftclick;
    }

    private void Leftclick(object sender, Coordinate e)
    {
    }

    public ClickableComponent LoanButton { get; }
}

