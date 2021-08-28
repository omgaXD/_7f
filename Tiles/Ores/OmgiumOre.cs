using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
namespace _7f.Tiles.Ores
{
    public class OmgiumOre : ModTile
	{
		public override void SetDefaults()
		{
            TileID.Sets.Ore[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileValue[Type] = 410;
            Main.tileShine[Type] = 975;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			dustType = DustID.Sparkle;
			drop = ModContent.ItemType<Items.Placeable.OmgiumOre>();
            Main.tileSpelunker[Type] = true;
			
            ModTranslation name = CreateMapEntryName();
			name.SetDefault("Omgium Ore");
			AddMapEntry(new Color(40, 211, 40), name);
			// Set other values here
            soundType = SoundID.Tink;
			soundStyle = 1;
            minPick = 180;
            mineResist = 4f;
		}
	}
}