using Terraria;
using Terraria.ModLoader;

namespace _7f.Buffs
{
	public class BigBruh : ModBuff
	{
		public override void SetDefaults() {
			DisplayName.SetDefault("BigBruh");
			Description.SetDefault("Your Ancient Rune recovers");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

	}
}