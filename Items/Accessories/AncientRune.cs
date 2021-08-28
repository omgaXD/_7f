using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace _7f.Items.Accessories
{
    public class AncientRune : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Rune");
            Tooltip.SetDefault("The thing.");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 1;
            item.value = 100;
            item.rare = 1;
            item.accessory = true;
            // Set other item.X values here
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AncientRuneAcc>().ancientrune = true;
        }

        public override void AddRecipes()
        {
            // Recipes here. See Basic Recipe Guide
        }
    }
}