using Terraria;
using Terraria.ModLoader;

namespace _7f.Buffs
{
	public class DemonicFight : ModBuff
	{
		public override void SetDefaults() {
			DisplayName.SetDefault("Demonic Fight");
			Description.SetDefault("If cat can fly infinitely, why can't you?\nRegen is disabled.");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.wingTime = 1;
			player.lifeRegenCount = 0;
		}
	}
}