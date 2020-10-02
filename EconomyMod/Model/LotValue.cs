using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace EconomyMod.Model
{
    public class LotValue
    {
        List<Func<int>> Values = new List<Func<int>>();

        public ModConfig Config { get; }
        public int Sum
        {
            get
            {
                return Values.Select(c => c()).Where(c => c > 0).Sum();
            }
        }

        public void Add(Func<int> value)
        {
            Values.Add(value);
        }


        public void AddDefaultLotValue()
        {

            Add(() =>
            {
                if (Util.Config.IncludeGreenhouseOnLotValue)
                    return Game1.getFarm().IsGreenhouse ? Util.Config.GreenhouseValue : -Util.Config.GreenhouseValue;
                return 0;
            });

            Add(() =>
            {
                return Util.Config.LotValue;
            });



        }
    }
}
