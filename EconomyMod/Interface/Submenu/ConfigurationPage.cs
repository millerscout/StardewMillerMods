using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EconomyMod.Helpers;
using EconomyMod.Interface.PageContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace EconomyMod.Interface.Submenu
{
    public class ConfigurationPage : Page
    {
        public ConfigurationPage(UIFramework ui, Texture2D Icon = null, string hoverText = null) : base(ui, Icon, hoverText)
        {

            //Elements.Add(new OptionsElement(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11233")));
            //Elements.Add(new ContentElementText(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11234")));
            Elements.Add(new ContentElementHeaderText(Util.Helper.Translation.Get("Configuration").Default("Economy Mod Configuration")));


            Elements.Add(new ContentElementSlider("Threshold To Ask About Payment", () => Util.Config.ThresholdInPercentageToAskAboutPayment, (o) => Util.Config.SetThresholdInPercentageToAskAboutPayment(Convert.ToByte(o))));



            this.Draw = DrawContent;
            this.DrawHover = DrawHoverContent;
            ui.OnLeftClick += Leftclick;

        }

        private void Leftclick(object sender, Coordinate e)
        {
            foreach (var el in Elements)
            {
                if (el is ContentElementSlider slider)
                {

                    if (e.X >= slider.clickArea.X && e.X <= slider.clickArea.X+slider.clickArea.Width && e.Y >= slider.clickArea.Y && e.Y <= slider.clickArea.Y+slider.clickArea.Height)
                        slider.receiveLeftClick(e.X - slider.clickArea.X, e.Y - slider.clickArea.Y);
                }
            }

        }

        private void DrawHoverContent(int arg1, int arg2)
        {
            //throw new NotImplementedException();
        }

        private void DrawContent()
        {

            int currentItemIndex = 0;
            for (int i = 0; i < Slots.Count; ++i)
            {
                if (Slots[i].bounds.X != xPositionOnScreen + Game1.tileSize / 4)
                {
                    Slots[i].bounds = new Rectangle(xPositionOnScreen + Game1.tileSize / 4, yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + i * (height - Game1.tileSize * 2) / 7, width - Game1.tileSize / 2, (height - Game1.tileSize * 2) / 7 + Game1.pixelZoom);
                }
                if (currentItemIndex >= 0 &&
                    currentItemIndex + i < Elements.Count)
                {
                    Elements[currentItemIndex + i].draw(Game1.spriteBatch, Slots[i].bounds.X, Slots[i].bounds.Y);
                    InterfaceHelper.Draw(Slots[i].bounds, InterfaceHelper.InterfaceHelperType.Cyan);

                    if (Elements[currentItemIndex + i] is ContentElementSlider slider)
                    {
                        if (slider.clickArea.IsEmpty)
                        {
                            var bounds = Slots[i].bounds;
                            var clickArea = new Rectangle(bounds.X+32, bounds.Y+16, slider.bounds.Width, slider.bounds.Height);
                            if (slider.bounds.X != clickArea.X || slider.bounds.Y != clickArea.Y)
                            {
                                slider.clickArea = clickArea;
                            }
                        }
                        InterfaceHelper.Draw(slider.clickArea, InterfaceHelper.InterfaceHelperType.Red);

                    }
                    //else
                    InterfaceHelper.Draw(Slots[i].bounds, InterfaceHelper.InterfaceHelperType.Cyan);
                }
            }
            //}
            //throw new NotImplementedException();
        }

    }
}
