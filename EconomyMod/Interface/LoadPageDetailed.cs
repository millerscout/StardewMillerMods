using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace EconomyMod.Interface
{
    public class LoadPageDetailed: IClickableMenu
    {
        private const int Width = 800;

        public Dictionary<int, ClickableTextureComponent> sideTabs = new Dictionary<int, ClickableTextureComponent>();
        private List<ClickableComponent> Slots = new List<ClickableComponent>();
        public TaxationService taxation;
        private string hoverText;
        public int currentTab;


        public event EventHandler<Tuple<int, int>> OnHover;
        public event EventHandler<Tuple<int, int>> OnLeftClick;
        public event EventHandler<SpriteBatch> OnDraw;
        public LoadPageDetailed(IModEvents events, TaxationService taxation)
            : base(Game1.activeClickableMenu.xPositionOnScreen, Game1.activeClickableMenu.yPositionOnScreen + 10, Width, Game1.activeClickableMenu.height)
        {
            this.taxation = taxation;

            events.Display.MenuChanged += OnMenuChanged;

        }

        /// <summary>Raised after a game menu is opened, closed, or replaced.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is GameMenu)
            {
                xPositionOnScreen = Game1.activeClickableMenu.xPositionOnScreen;
                yPositionOnScreen = Game1.activeClickableMenu.yPositionOnScreen + 10;
                height = Game1.activeClickableMenu.height;

                for (int i = 0; i < Slots.Count; ++i)
                {
                    var next = Slots[i];
                    next.bounds.X = xPositionOnScreen + Game1.tileSize / 4;
                    next.bounds.Y = yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + i * (height - Game1.tileSize * 2) / 7;
                    next.bounds.Width = width - Game1.tileSize / 2;
                    next.bounds.Height = (height - Game1.tileSize * 2) / 7 + Game1.pixelZoom;
                }
            }
        }


        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            foreach (KeyValuePair<int, ClickableTextureComponent> v in sideTabs)
            {
                if (v.Value.containsPoint(x, y) && currentTab != v.Key)
                {
                    Game1.playSound("smallSelect");
                    sideTabs[currentTab].bounds.X -= Constants.sideTab_widthToMoveActiveTab;
                    currentTab = Convert.ToInt32(v.Value.name);

                    //TODO: reset page on current submenu
                    v.Value.bounds.X += Constants.sideTab_widthToMoveActiveTab;
                }
            }

            OnLeftClick?.Invoke(this, new Tuple<int, int>(x, y));
        }



        public override void receiveRightClick(int x, int y, bool playSound = true)
        {

        }

        public override void receiveGamePadButton(Buttons b)
        {
            if (b == Buttons.A)
            {
                receiveLeftClick(Game1.getMouseX(), Game1.getMouseY());
            }
        }

        public override void performHoverAction(int x, int y)
        {
            if (!GameMenu.forcePreventClose)
            {
                hoverText = "";
            }



            foreach (ClickableTextureComponent c2 in sideTabs.Values)
            {
                if (c2.containsPoint(x, y))
                {
                    hoverText = c2.hoverText;
                    return;
                }
            }

            OnHover?.Invoke(this, new Tuple<int, int>(x, y));
        }

        public override void draw(SpriteBatch batch)
        {
            DrawSideTabs(batch);
            OnDraw?.Invoke(this, batch);
            batch.End();

            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            DrawHoverText(batch);

        }

        private void DrawHoverText(SpriteBatch batch)
        {
            if (hoverText != "")
                IClickableMenu.drawHoverText(batch, hoverText, Game1.smallFont);
        }

        private void DrawSideTabs(SpriteBatch batch)
        {
            foreach (ClickableTextureComponent tab in sideTabs.Values)
            {
                tab.draw(batch);
            }
        }
    }
}
