using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace EconomyMod.Interface.PageContent
{
    public class ContentElementCheckbox : OptionsElement, IContentElement
    {
        public const int pixelsWide = 9;

        public bool isChecked;

        public static Rectangle sourceRectUnchecked = new Rectangle(227, 425, 9, 9);

        public static Rectangle sourceRectChecked = new Rectangle(236, 425, 9, 9);

        public ContentElementCheckbox(string label, int whichOption, int x = -1, int y = -1)
            : base(label, x, y, 36, 36, whichOption)
        {
            //Game1.options.setCheckBoxToProperValue(this);
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (!greyedOut)
            {
                Game1.playSound("drumkit6");
                base.receiveLeftClick(x, y);
                isChecked = !isChecked;
                Game1.options.changeCheckBoxOption(whichOption, isChecked);
            }
        }

        public override void draw(SpriteBatch b, int slotX, int slotY, IClickableMenu context = null)
        {
            b.Draw(Game1.mouseCursors, new Vector2(slotX + Bounds.X, slotY + Bounds.Y), new Rectangle?(_isChecked ? OptionsCheckbox.sourceRectChecked : OptionsCheckbox.sourceRectUnchecked), Color.White * (_canClick ? 1f : 0.33f), 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.4f);
            base.draw(b, slotX, slotY, context);
        }
    }
}
