using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace _7f.Projectiles.Hostile
{
    public class PhasmaDartHostileGravity : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phasma Dart");
		}
        public override void SetDefaults()
        {
            projectile.scale = 4;
            projectile.width = 4;
            projectile.height = 4;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 180;
            //projectile.localNPCHitCooldown = -1;
            projectile.penetrate = 300;
            projectile.tileCollide = false;
            projectile.alpha = 0;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.BrokenArmor, 60*5);
            projectile.Kill();
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        public override void AI()
        {
            projectile.velocity.Y = Math.Max(projectile.velocity.Y + 0.3f, 7.5f);
            projectile.velocity.X *= 0.97f;
            projectile.spriteDirection = projectile.direction = (projectile.velocity.X > 0).ToDirectionInt();
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            if (projectile.timeLeft < 60) {
                projectile.alpha += 8;
            }
            if (projectile.alpha >= 254) {
                projectile.Kill();
            }
            int DustID = Dust.NewDust(projectile.position, 8, 8, 27, Main.rand.NextFloat(), Main.rand.NextFloat());               

            

           
        }
        //----------------------
        
        //----------------------
        public override void Kill(int timeLeft)
        {
            if (projectile.alpha < 250) {
                Random rnd = new Random();
                for (int i = 0; i < 15; i++) {
                    int DustID = Dust.NewDust(projectile.position, 8, 8, 27, Main.rand.NextFloat(), Main.rand.NextFloat());                
                }
            }
        }
        //----------------------
        
        
    }
}