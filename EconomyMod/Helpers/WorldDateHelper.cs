using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EconomyMod.Model;
using StardewValley;

namespace EconomyMod.Helpers
{
    public static class Helper
    {
        public static CustomWorldDate ToWorldDate(this int day)
        {
            return new CustomWorldDate(day);
        }
        public static string GetLocalizedSeason(this Season season)
        {
            return Utility.getSeasonNameFromNumber((int)season - 1);
        }

    }
}
