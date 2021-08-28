using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;


namespace _7f.NPCs
{
	public class LGCSkyData : ScreenShaderData
	{
		private int puritySpiritIndex;

		public LGCSkyData(string passName)
			: base(passName) {
		}

		private void UpdateLGCIndex() {
			int LGCType = ModContent.NPCType<NPCs.LGC>();
			if (puritySpiritIndex >= 0 && Main.npc[puritySpiritIndex].active && Main.npc[puritySpiritIndex].type == LGCType) {
				return;
			}
			puritySpiritIndex = -1;
			for (int i = 0; i < Main.maxNPCs; i++) {
				if (Main.npc[i].active && Main.npc[i].type == LGCType) {
					puritySpiritIndex = i;
					break;
				}
			}
		}

		public override void Apply() {
			UpdateLGCIndex();
			if (puritySpiritIndex != -1) {
				UseTargetPosition(Main.npc[puritySpiritIndex].Center);
			}
			base.Apply();
		}
	}
}