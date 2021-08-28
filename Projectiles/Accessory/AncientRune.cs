using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace _7f.Projectiles.Accessory
{
    public class AncientRune : ModProjectile
    {
        
        public override void SetDefaults()
        {
            //projectile.scale = 4;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.width = 160;
            projectile.height = 160;
            projectile.tileCollide = false;
            
            projectile.timeLeft = 60*10;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.penetrate = 30;
        }
        public override void AI()
        {
            Player owner = Main.player[projectile.owner];
            projectile.position = new Vector2(owner.position.X-70f,owner.position.Y-60f);
            for (int i = 0; i <= 2; i++) {
            int MyDust = Dust.NewDust(projectile.position,140, 140, DustID.TeleportationPotion, Main.rand.NextFloat(0.5f,1f),Main.rand.NextFloat(0.5f,1f),0,default(Color),Main.rand.NextFloat(1f,1.2f));
            Main.dust[MyDust].noGravity = true;
            }
            projectile.rotation += 0.004f; 
            //projectile.timeLeft--;
            if (projectile.timeLeft < 590) {
                projectile.damage = 0;
                projectile.hostile = true;
            }
            if (projectile.timeLeft < 90) {
                projectile.alpha += 4;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.damage = 0;
            projectile.hostile = true;
        }
    }
}