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
        private EconomyPageButton economyPageButton;
        private EconomyPage EconomyPage;

        private int pageNumber;
        private readonly TaxationService taxation;

        public EconomyInterfaceHandler(TaxationService taxation)
        {
            Util.Helper.Events.Display.MenuChanged += MenuChanged;
            this.taxation = taxation;
            ModConfig modConfig = Util.Helper.ReadConfig<ModConfig>();

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
                    EconomyPage.contentId = 0;
                    ////TODO: Dispose unused resources.
                }
            }

            // add to new menu
            if (e.NewMenu is GameMenu newMenu)
            {
                if (economyPageButton == null)
                {
                    EconomyPage = new EconomyPage(Util.Helper.Events, taxation);
                    economyPageButton = new EconomyPageButton(Util.Helper);
                }

                Util.Helper.Events.Display.RenderedActiveMenu += DrawButton;
                economyPageButton.OnLeftClicked += OnButtonLeftClicked;
                List<IClickableMenu> tabPages = newMenu.pages;

                pageNumber = tabPages.Count;
                tabPages.Add(EconomyPage);
            }
        }

        private void SetActiveClickableMenuToModOptionsPage()
        {
            if (Game1.activeClickableMenu is GameMenu menu)
                menu.currentTab = pageNumber;
        }

        private void DrawButton(object sender, EventArgs e)
        {
            if (Game1.activeClickableMenu is GameMenu gameMenu &&
                gameMenu.currentTab != 3) //don't render when the map is showing
            {
                if (gameMenu.currentTab == pageNumber)
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
