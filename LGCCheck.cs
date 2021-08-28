using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using _7f;

namespace _7f.LGCCheck 
{
    
    class LGCCheck : GlobalNPC 
    {

        

        public override bool PreNPCLoot(NPC npc)
        {
            
            if (npc.type == ModContent.NPCType<NPCs.LGC>() && !downed.downed.downedLGC) {
                Main.NewText("Ancient cat released the power of Omgium", Microsoft.Xna.Framework.Color.Gray);
                for (int k = 0; k < (int)(((byte)(Main.maxTilesX * Main.maxTilesY) / 40000f)); k++) {
			// 10. We randomly choose an x and y coordinate. The x coordinate is choosen from the far left to the far right coordinates. The y coordinate, however, is choosen from between WorldGen.worldSurfaceLow and the bottom of the map. We can use this technique to determine the depth that our ore should spawn at.
			        int x = WorldGen.genRand.Next(0, Main.maxTilesX);
			        int y = WorldGen.genRand.Next((int)WorldGen.rockLayerLow, (int)WorldGen.rockLayerHigh);

			// 11. Finally, we do the actual world generation code. In this example, we use the WorldGen.OreRunner method. This method spawns splotches of the Tile type we provide to the method. The behavior of TileRunner is detailed in the Useful Methods section below.
			        WorldGen.OreRunner(x, y, WorldGen.genRand.Next(8, 12), WorldGen.genRand.Next(8, 12), (ushort)ModContent.TileType<Tiles.Ores.OmgiumOre>());
		        }
            }
            return true;
        }
    }
}