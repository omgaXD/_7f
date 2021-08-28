using Microsoft.Xna.Framework;
using _7f.NPCs;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace _7f
{
    public class _7f : Mod
    {

        public override void Load()
        {
            Filters.Scene["_7f:LGC"] = new Filter(new LGCSkyData("FilterMiniTower").UseColor(1f, -1f, 1f).UseOpacity(0.35f), EffectPriority.VeryHigh);
            SkyManager.Instance["_7f:LGC"] = new LGCSky();

        }
        /*public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
            {
                return;
            }
            if (Main.LocalPlayer.GetModPlayer<Items.Accessories.AncientRuneAcc>().lgcphase3 == true) {
                music = -1;
				priority = MusicPriority.BiomeLow;
            }
        }*/
        public override void AddRecipeGroups()
        {
            RecipeGroup group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Evil YoYo", new int[]
            {
                ItemID.CorruptYoyo,
                ItemID.CrimsonYoyo
            });
            RecipeGroup.RegisterGroup("_7f:EvilYoYo", group);
        }
    }
}