using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace _7f.Projectiles.Hostile
{
    public class PhasmaScytheRotate : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phasma Scythe");

        }



        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 274;
            //projectile.localNPCHitCooldown = -1;
            projectile.penetrate = 300;
            projectile.tileCollide = false;
            projectile.alpha = 0;
            projectile.localAI[0] = 1;

        }


        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.BrokenArmor, 60 * 5);
            projectile.Kill();
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        public override void AI()
        {
            NPC npc = Main.npc[(int)projectile.ai[0]];
            projectile.spriteDirection = projectile.direction = (projectile.velocity.X > 0).ToDirectionInt();
            projectile.rotation += 12f;
            if (projectile.timeLeft < 30)
            {
                projectile.alpha += 16;
            }
            if (projectile.alpha >= 254)
            {
                projectile.Kill();
            }
            //projectile.velocity *= 1.05f;
            projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(projectile.ai[1]));
            Vector2 velocity = projectile.velocity;
            velocity.Normalize();
            if (Math.Abs(projectile.velocity.X) > Math.Abs(velocity.X * 16f))
            {
                projectile.velocity = velocity * 16f;
            }
            //int DustID = Dust.NewDust(projectile.position, 8, 8, 27, 0, 0);
            Lighting.AddLight(projectile.position, 1f, 0f, 1f);
        }
        //----------------------

        //----------------------
        public override void Kill(int timeLeft)
        {
            if (projectile.alpha < 250)
            {
                Random rnd = new Random();
                for (int i = 0; i < 15; i++)
                {
                    int DustID = Dust.NewDust(projectile.position, 8, 8, 27, Main.rand.NextFloat(), Main.rand.NextFloat());
                }
            }
        }
        //----------------------


    }
}