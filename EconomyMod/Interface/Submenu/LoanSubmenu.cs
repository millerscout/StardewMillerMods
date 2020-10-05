using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EconomyMod.Interface.PageContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace EconomyMod.Interface.Submenu
{
    public class LoanSubmenu
    {

        private List<ContentElement> Elements = new List<ContentElement>();
        private List<ClickableComponent> Slots = new List<ClickableComponent>();
        private int currentPage;
        public ClickableTextureComponent sideTabButton { get; }

        private ClickableComponent LoanButton;
        private TaxationService taxation;
        private EconomyPage economyPage;

        public LoanSubmenu(EconomyPage economyPage)
        {
            this.taxation = economyPage.taxation;
            this.economyPage = economyPage;
            ///TODO: Localization
            Elements.Add(new ContentElement("Loans"));


            sideTabButton = new ClickableTextureComponent(string.Concat(1), new Rectangle(economyPage.xPositionOnScreen - 48, economyPage.yPositionOnScreen + 64 * (2 + economyPage.sideTabs.Count), 64, 64), "", "Loan", Util.Helper.Content.Load<Texture2D>($"assets/Interface/LoanButton.png"), new Rectangle(0, 0, 16, 16), 4f);


            LoanButton = new ClickableComponent(InterfaceHelper.GetButtonSizeForPage(economyPage), "", "_____________");

            economyPage.OnDraw += (object _, SpriteBatch batch) => Draw(batch);
            economyPage.OnHover += (object _, Tuple<int, int> coord) => PerformHover(coord.Item1, coord.Item2);
            economyPage.OnLeftClick += (object _, Tuple<int, int> coord) => ReceiveLeftClick(coord.Item1, coord.Item2);

            for (int i = 0; i < 7; ++i)
                Slots.Add(new ClickableComponent(
                    new Rectangle(
                        economyPage.xPositionOnScreen + Game1.tileSize / 4,
                        economyPage.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + i * (economyPage.height - Game1.tileSize * 2) / 7,
                        economyPage.width - Game1.tileSize / 2,
                        (economyPage.height - Game1.tileSize * 2) / 7 + Game1.pixelZoom),
                    i.ToString()));

        }

        private void Draw(SpriteBatch batch)
        {
            if (economyPage.currentTab == Convert.ToInt32(sideTabButton.name))
            {

                if (economyPage.contentId == 0)
                {
                    int currentItemIndex = 0;


                    for (int i = 0; i < Slots.Count; ++i)
                    {
                        InterfaceHelper.Draw(Slots[i].bounds);
                        if (currentItemIndex >= 0 &&
                            currentItemIndex + i < Elements.Count)
                        {
                            Elements[currentItemIndex + i].Draw(batch, Slots[i].bounds.X, Slots[i].bounds.Y);
                        }
                    }
                    DrawPayButton();
                }
            }
        }
        private void ReceiveLeftClick(int x, int y)
        {
            if (LoanButton.containsPoint(x, y) && taxation.State.PendingTaxAmount != 0)
            {
                economyPage.contentId = 1;
            }
        }
        private void PerformHover(int x, int y)
        {
            if (LoanButton.containsPoint(x, y) && taxation.State.PendingTaxAmount != 0)
            {
                if (LoanButton.scale == 0f)
                {
                    Game1.playSound("Cowboy_gunshot");
                }
                LoanButton.scale = 1f;
            }
            else
            {
                LoanButton.scale = 0f;
            }
        }

        private void DrawPayButton()
        {
            if (taxation.State.PendingTaxAmount != 0)
            {
                IClickableMenu.drawTextureBox(Game1.spriteBatch, Game1.mouseCursors, new Rectangle(432, 439, 9, 9), LoanButton.bounds.X, LoanButton.bounds.Y, LoanButton.bounds.Width, LoanButton.bounds.Height, (LoanButton.scale > 0f) ? Color.Wheat : Color.White, 4f);
                var btnPosition = new Vector2(LoanButton.bounds.Center.X, LoanButton.bounds.Center.Y + 4) - Game1.dialogueFont.MeasureString("Loan Funds - Pelican Town 10000g") / 2f;
                Utility.drawTextWithShadow(Game1.spriteBatch, "Loan Funds - Pelican Town 10000g", Game1.dialogueFont, btnPosition, Game1.textColor, 1f, -1f, -1, -1, 0f);

                InterfaceHelper.Draw(LoanButton.bounds, center: true);
                InterfaceHelper.Draw(btnPosition, InterfaceHelper.InterfaceHelperType.TextInsideButton);
            }
        }


    }
}
