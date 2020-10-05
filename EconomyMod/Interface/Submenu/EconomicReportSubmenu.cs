//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using EconomyMod.Interface.PageContent;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using StardewValley;
//using StardewValley.Menus;

//namespace EconomyMod.Interface.Submenu
//{
//    public class EconomicReportSubmenu
//    {

//        private List<ContentElement> Elements = new List<ContentElement>();
//        private List<ClickableComponent> Slots = new List<ClickableComponent>();
//        private int currentPage;
//        public ClickableTextureComponent sideTabButton { get; }

//        private ClickableComponent payButton;
//        private TaxationService taxation;
//        private EconomyPage economyPage;

//        public EconomicReportSubmenu(EconomyPage economyPage)
//        {
//            this.taxation = economyPage.taxation;
//            this.economyPage = economyPage;
//            Elements.Add(new ContentElement(Util.Helper.Translation.Get("BalanceReportText")));
//            Elements.Add(new ContentElement(() => $"{Util.Helper.Translation.Get("CurrentLotValueText")}"));
//            Elements.Add(new ContentElement(() => $"{taxation.LotValue.Sum}g"));
//            Elements.Add(new ContentElement(() => $"{Util.Helper.Translation.Get("CurrentTaxBalance")}:"));
//            Elements.Add(new ContentElement(() => $"{taxation.State?.PendingTaxAmount}g"));


//            var SidetabRect = InterfaceHelper.GetSideTabSizeForPage(economyPage, economyPage.sideTabs.Count) ;
//            sideTabButton = new ClickableTextureComponent(string.Concat(0), SidetabRect, "", "Tax payment and report", Util.Helper.Content.Load<Texture2D>($"assets/Interface/sidebarButtonReport.png"), new Rectangle(0, 0, 16, 16), 4f);

//            payButton = new ClickableComponent(new Rectangle(economyPage.xPositionOnScreen + 64, Game1.activeClickableMenu.height + 50, (int)Game1.dialogueFont.MeasureString("_____________").X, 96), "", "_____________");

//            economyPage.OnDraw += (object _, SpriteBatch batch) => Draw(batch);
//            economyPage.OnHover += (object _, Tuple<int, int> coord) => PerformHover(coord.Item1, coord.Item2);
//            economyPage.OnLeftClick += (object _, Tuple<int, int> coord) => ReceiveLeftClick(coord.Item1, coord.Item2);

//            for (int i = 0; i < Elements.Count; ++i)
//                Slots.Add(new ClickableComponent(
//                    new Rectangle(
//                        economyPage.xPositionOnScreen + Game1.tileSize / 4,
//                        economyPage.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + i * (economyPage.height - Game1.tileSize * 2) / 7,
//                        economyPage.width - Game1.tileSize / 2,
//                        (economyPage.height - Game1.tileSize * 2) / 7 + Game1.pixelZoom),
//                    i.ToString()));

//        }

//        private void Draw(SpriteBatch batch)
//        {
//            if (economyPage.currentTab == Convert.ToInt32(sideTabButton.name))
//            {

//                int currentItemIndex = 0;
//                for (int i = 0; i < Slots.Count; ++i)
//                {
//                    if (currentItemIndex >= 0 &&
//                        currentItemIndex + i < Elements.Count)
//                    {
//                        Elements[currentItemIndex + i].Draw(batch, Slots[i].bounds.X, Slots[i].bounds.Y);
//                    }
//                }
//                DrawPayButton();
//            }
//        }

//        private void ReceiveLeftClick(int x, int y)
//        {
//            if (payButton.containsPoint(x, y) && taxation.State.PendingTaxAmount != 0)
//            {
//                taxation.PayTaxes();
//            }
//        }
//        private void PerformHover(int x, int y)
//        {
//            if (payButton.containsPoint(x, y) && taxation.State.PendingTaxAmount != 0)
//            {
//                if (payButton.scale == 0f)
//                {
//                    Game1.playSound("Cowboy_gunshot");
//                }
//                payButton.scale = 1f;
//            }
//            else
//            {
//                payButton.scale = 0f;
//            }
//        }

//        private void DrawPayButton()
//        {
//            if (taxation.State.PendingTaxAmount != 0)
//            {
//                IClickableMenu.drawTextureBox(Game1.spriteBatch, Game1.mouseCursors, new Rectangle(432, 439, 9, 9), payButton.bounds.X, payButton.bounds.Y, payButton.bounds.Width, payButton.bounds.Height, (payButton.scale > 0f) ? Color.Wheat : Color.White, 4f);
//                Utility.drawTextWithShadow(Game1.spriteBatch, "Pay", Game1.dialogueFont, new Vector2(payButton.bounds.Center.X, payButton.bounds.Center.Y + 4) - Game1.dialogueFont.MeasureString("Pay") / 2f, Game1.textColor, 1f, -1f, -1, -1, 0f);
//            }
//        }


//    }
//}
