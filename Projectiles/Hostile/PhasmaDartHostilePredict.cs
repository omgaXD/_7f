using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace _7f.Projectiles.Hostile
{
    public class PhasmaDartHostilePredict : ModProjectile
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
            projectile.timeLeft = 240;
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
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            if (projectile.timeLeft <= 180)
            {
                projectile.spriteDirection = projectile.direction = (projectile.velocity.X > 0).ToDirectionInt();
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
                if (projectile.timeLeft < 60)
                {
                    projectile.alpha += 8;
                }
                if (projectile.alpha >= 254)
                {
                    projectile.Kill();
                }

                int DustID = Dust.NewDust(projectile.position, 8, 8, 27, Main.rand.NextFloat(), Main.rand.NextFloat());



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

            if (projectile.timeLeft > 180)
            {
                Color color = Color.White;
                if (projectile.localAI[0] != 10)
                {
                    pos = projectile.Center;
                    projectile.localAI[0] = 10;
                }
                Texture2D texture = ModContent.GetTexture("_7f/NPCs/Trail");

                float opacity = ((float)projectile.timeLeft - 210f) / 40f;
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
                    c = 0.4f;
                    color *= 0.5f;
                }
                for (int i = 0; i < 720 * c; i++)
                {




                    spriteBatch.Draw(texture, b, null, color * opacity, a.ToRotation(), new Vector2(2, 2), 1, SpriteEffects.None, 1);
                    if (projectile.ai[0] == 1)
                    {
                        opacity -= (i / 128f) / 100f;
                    }
                    b += a;
                }





            }
            return true;

        }

    }
}