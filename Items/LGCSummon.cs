using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace _7f.Items
{
    public class LGCSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Suspicious Cat food");
            Tooltip.SetDefault("Use it to break barrier between you and Kitty's true power");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.maxStack = 20;
            item.value = 0;
            item.rare = ItemRarityID.Purple;
            item.consumable = false;
            item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.UseSound = SoundID.Item44;
            // Set other item.X values here
        }
        public override bool CanUseItem(Player player) {
            return !NPC.AnyNPCs(ModContent.NPCType<NPCs.LGC>());
        }
        public override bool UseItem(Player player) {
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.LGC>());
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}

        public override void AddRecipes()
        {
            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(ItemID.Fish, 4);
            recipe1.AddIngredient(ItemID.LunarBar, 1);
            recipe1.AddTile(TileID.AlchemyTable);
            recipe1.SetResult(this);
			recipe1.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.SharkFin, 4);
            recipe2.AddIngredient(ItemID.LunarBar, 1);
            recipe2.AddTile(TileID.AlchemyTable);
            recipe2.SetResult(this);
			recipe2.AddRecipe();
        }
    }
}