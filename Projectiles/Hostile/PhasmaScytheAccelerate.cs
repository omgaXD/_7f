using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace _7f.Projectiles.Hostile
{
    public class PhasmaScytheAccelerate : ModProjectile
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
            projectile.timeLeft = 137;
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

            projectile.spriteDirection = projectile.direction = (projectile.velocity.X > 0).ToDirectionInt();
            projectile.rotation += 12f;
            if (projectile.timeLeft <= 300)
            {
                if (projectile.timeLeft < 30)
                {
                    projectile.alpha += 16;
                }
                if (projectile.alpha >= 254)
                {
                    projectile.Kill();
                }
                projectile.velocity *= 1.02f;
                Vector2 velocity = projectile.velocity;
                velocity.Normalize();
                if (Math.Abs(projectile.velocity.X) > Math.Abs(velocity.X * 16f)) {
                    projectile.velocity = velocity * 16f;
                }
                int DustID = Dust.NewDust(projectile.position, 8, 8, 27, 0, 0);



                //If the distance between the live targeted npc and the projectile is less than 480 pixels
            }
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

        Vector2 pos = new Vector2();
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            if (projectile.timeLeft > 107f)
            {
                Color color = Color.White;
                if (projectile.localAI[0] != 10)
                {
                    pos = projectile.Center;
                    projectile.localAI[0] = 10;
                }
                Texture2D texture = ModContent.GetTexture("_7f/NPCs/Trail");

                float opacity = ((float)projectile.timeLeft - 107f) / 30f;
                Vector2 a = projectile.velocity;
                a.Normalize();
                a *= 4;

                Vector2 b = pos - Main.screenPosition;


                float c = 1f;
                if (projectile.ai[0] != 1)
                {
                    b -= a * 120;
                    color *= 0.75f;
                }
                else
                {
                    c = 0.33f;
                    color *= 0.5f;
                }
                for (int i = 0; i < 719 * c; i++)
                {




                    spriteBatch.Draw(texture, b, null, color * opacity, a.ToRotation(), new Vector2(2, 2), 1, SpriteEffects.None, 1);
                    b += a;
                    opacity -= 0.003f;
                }





            }
            return true;

        }

    }
}