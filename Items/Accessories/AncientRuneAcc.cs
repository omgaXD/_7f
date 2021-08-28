using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace _7f.Items.Accessories
{
    public class AncientRuneAcc : ModPlayer
    {

        public bool ancientrune;
        public bool ancientruneinv;

        /*public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (ancientrune && !player.HasBuff(ModContent.BuffType<Buffs.BigBruh>())) {
                
                Main.npc[npc.whoAmI].HealEffect(damage*100, true);
                Main.npc[npc.whoAmI].life -= damage * 100;
                damage = 0;
            }
        }*/
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {

            if (ancientrune && !player.HasBuff(ModContent.BuffType<Buffs.BigBruh>()))
            {
                player.AddBuff(ModContent.BuffType<Buffs.BigBruh>(), 60 * 45);
                player.AddBuff(ModContent.BuffType<Buffs.BigOof>(), 60 * 10);
                customDamage = true;

                Main.PlaySound(SoundID.NPCDeath59, player.position);
                Projectile.NewProjectile(player.position, new Vector2(0f, 0f), ModContent.ProjectileType<Projectiles.Accessory.AncientRune>(), damage * 100, 0, player.whoAmI);
                damage = 0;
                return false;

            }
            if (ancientruneinv)
            {
                customDamage = true;
                damage = 0;
                return false;
            }
            return true;

        }

        public override void UpdateBiomeVisuals()
        {
            bool uselgc = NPC.AnyNPCs(ModContent.NPCType<NPCs.LGC>());
            if (uselgc)
            {
                uselgc = false;
                for (int i = 0; i <= Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active == true && Main.npc[i].type == ModContent.NPCType<NPCs.LGC>() && Main.npc[i].localAI[2] >= 4)
                    {
                        uselgc = true;
                        break;
                    }
                }
            }
            player.ManageSpecialBiomeVisuals("_7f:LGC", uselgc);
        }
        public override void ResetEffects()
        {
            ancientrune = false;
            ancientruneinv = false;
        }
    }
}