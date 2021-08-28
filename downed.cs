
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using _7f;

namespace downed
{
    class downed : ModWorld
    {
        public static bool downedLGC;
        public override void Initialize() {
			downedLGC = false;
		}
        public override TagCompound Save() {
			var downed = new List<string>();
			if (downedLGC) {
				downed.Add("LGC");
			}

			return new TagCompound {
				["downed"] = downed
			};
		}
        public override void Load(TagCompound tag) {
			var downed = tag.GetList<string>("downed");
			downedLGC= downed.Contains("LGC");
		}
        public override void LoadLegacy(BinaryReader reader) {
			int loadVersion = reader.ReadInt32();
			if (loadVersion == 0) {
				BitsByte flags = reader.ReadByte();
				downedLGC = flags[0];
			}
			else {
				mod.Logger.WarnFormat("_7f: Unknown loadVersion: {0}", loadVersion);
			}
		}

        public override void NetSend(BinaryWriter writer) {
			var flags = new BitsByte();
			flags[0] = downedLGC;
        }

        public override void NetReceive(BinaryReader reader) {
			BitsByte flags = reader.ReadByte();
			downedLGC = flags[0];
			// As mentioned in NetSend, BitBytes can contain 8 values. If you have more, be sure to read the additional data:
			// BitsByte flags2 = reader.ReadByte();
			// downed9thBoss = flags[0];
		}
    }
}