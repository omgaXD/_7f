using Terraria;
using Terraria.ModLoader;

namespace _7f.Buffs
{
	public class BigOof : ModBuff
	{
		public override void SetDefaults() {
			DisplayName.SetDefault("BigBruh");
			Description.SetDefault("Your Ancient Rune recovers");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<Items.Accessories.AncientRuneAcc>().ancientruneinv = true;
		}
	}
}