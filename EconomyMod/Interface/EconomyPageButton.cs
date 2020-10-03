using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace EconomyMod.Interface
{
    class EconomyPageButton : IClickableMenu
    {
        private IModHelper helper;

        public Texture2D IconTexture { get; set; }
        public Rectangle Bounds { get; }

        public event EventHandler OnLeftClicked;

        public EconomyPageButton(IModHelper helper)
        {
            width = 64;
            height = 64;
            GameMenu activeClickableMenu = Game1.activeClickableMenu as GameMenu;
            this.helper = helper;
            xPositionOnScreen = activeClickableMenu.xPositionOnScreen + activeClickableMenu.width - 304;
            yPositionOnScreen = activeClickableMenu.yPositionOnScreen + 16;
            Bounds = new Rectangle(xPositionOnScreen, yPositionOnScreen, width, height);
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.Display.MenuChanged += OnMenuChanged;


            IconTexture = helper.Content.Load<Texture2D>($"assets/Interface/tabIcon.png");
        }

        /// <summary>Raised after a game menu is opened, closed, or replaced.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is GameMenu menu)
            {
                xPositionOnScreen = menu.xPositionOnScreen + menu.width - 304;
            }
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        public void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.MouseLeft || e.Button == SButton.ControllerA)
            {
                int x = (int)e.Cursor.ScreenPixels.X;
                int y = (int)e.Cursor.ScreenPixels.Y;
                if (isWithinBounds(x, y))
                {
                    receiveLeftClick(x, y);
                    OnLeftClicked?.Invoke(this, null);
                }
            }

        }

        public override void draw(SpriteBatch b)
        {
            base.draw(b);

            Game1.spriteBatch.Draw(Game1.mouseCursors,
                new Vector2(xPositionOnScreen, yPositionOnScreen),
                new Rectangle(16, 368, 16, 16),
                Color.White,
                0.0f,
                Vector2.Zero,
                Game1.pixelZoom,
                SpriteEffects.None,
                1f);

            b.Draw(IconTexture,
                new Vector2(xPositionOnScreen + 8, yPositionOnScreen + 14),
                new Rectangle(0, 0, 16, 16),
                Color.White,
                0.0f,
                Vector2.Zero,
                3f,
                SpriteEffects.None,
                1f);

            if (isWithinBounds(Game1.getMouseX(), Game1.getMouseY()))
            {
                IClickableMenu.drawHoverText(Game1.spriteBatch, this.helper.Translation.Get("BalanceReportText"), Game1.smallFont);
            }
            if (!Game1.options.hardwareCursor)
            {
                b.Draw(Game1.mouseCursors, new Vector2(Game1.getMouseX(), Game1.getMouseY()), Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16), Color.White, 0f, Vector2.Zero, 4f + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
            }
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
        }
    }
}
