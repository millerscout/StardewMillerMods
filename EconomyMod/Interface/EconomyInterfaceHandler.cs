using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EconomyMod.Interface.PageContent;
using EconomyMod.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;

namespace EconomyMod.Interface
{
    public class EconomyInterfaceHandler
    {
        private List<ContentElement> Elements = new List<ContentElement>();
        private EconomyPageButton economyPageButton;
        private EconomyPage EconomyPage;

        private int _modOptionsTabPageNumber;
        private readonly TaxationService taxation;

        public EconomyInterfaceHandler(TaxationService taxation)
        {
            Util.Helper.Events.Display.MenuChanged += MenuChanged;
            this.taxation = taxation;
            ModConfig modConfig = Util.Helper.ReadConfig<ModConfig>();

            Version thisVersion = Assembly.GetAssembly(this.GetType()).GetName().Version;

            Elements.Add(new ContentElement(Util.Helper.Translation.Get("BalanceReportText")));
            Elements.Add(new ContentElement(() => $"{Util.Helper.Translation.Get("CurrentLotValueText")}: {taxation.LotValue.Sum}"));
            Elements.Add(new ContentElement(() => $"{Util.Helper.Translation.Get("CurrentTaxBalance")}: {taxation.State?.PendingTaxAmount}"));

        }

        private void OnButtonLeftClicked(object sender, EventArgs e)
        {
            if (Game1.activeClickableMenu is GameMenu)
            {
                SetActiveClickableMenuToModOptionsPage();
                Game1.playSound("smallSelect");
            }
        }


        /// <summary>Raised after a game menu is opened, closed, or replaced.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void MenuChanged(object sender, MenuChangedEventArgs e)
        {
            // remove from old menu
            if (e.OldMenu != null)
            {
                Util.Helper.Events.Display.RenderedActiveMenu -= DrawButton;
                if (economyPageButton != null)
                    economyPageButton.OnLeftClicked -= OnButtonLeftClicked;

                if (e.OldMenu is GameMenu gameMenu)
                {
                    List<IClickableMenu> tabPages = gameMenu.pages;
                    tabPages.Remove(EconomyPage);
                }
            }

            // add to new menu
            if (e.NewMenu is GameMenu newMenu)
            {
                if (economyPageButton == null)
                {
                    EconomyPage = new EconomyPage(Elements, Util.Helper.Events, taxation);
                    economyPageButton = new EconomyPageButton(Util.Helper);
                }

                Util.Helper.Events.Display.RenderedActiveMenu += DrawButton;
                economyPageButton.OnLeftClicked += OnButtonLeftClicked;
                List<IClickableMenu> tabPages = newMenu.pages;

                _modOptionsTabPageNumber = tabPages.Count;
                tabPages.Add(EconomyPage);
            }
        }

        private void SetActiveClickableMenuToModOptionsPage()
        {
            if (Game1.activeClickableMenu is GameMenu menu)
                menu.currentTab = _modOptionsTabPageNumber;
        }

        private void DrawButton(object sender, EventArgs e)
        {
            if (Game1.activeClickableMenu is GameMenu gameMenu &&
                gameMenu.currentTab != 3) //don't render when the map is showing
            {
                if (gameMenu.currentTab == _modOptionsTabPageNumber)
                {
                    economyPageButton.yPositionOnScreen = Game1.activeClickableMenu.yPositionOnScreen + 24;
                }
                else
                {
                    economyPageButton.yPositionOnScreen = Game1.activeClickableMenu.yPositionOnScreen + 16;
                }
                economyPageButton.draw(Game1.spriteBatch);

                //Might need to render hover text here
            }
        }
    }
}
