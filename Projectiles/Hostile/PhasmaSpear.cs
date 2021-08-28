using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace _7f.Projectiles.Hostile
{
    public class PhasmaSpear : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phasma Spear");
        }
        public override void SetDefaults()
        {
            projectile.width = 54;
            projectile.height = 54;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 240;
            //projectile.localNPCHitCooldown = -1;
            projectile.penetrate = 300;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.coldDamage = false;
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
            Player player = Main.player[projectile.owner];

            if (projectile.timeLeft == 180)
            {
                Vector2 velocity = Vector2.Zero;
                if (projectile.ai[1] == 0)
                {
                    velocity = (player.position + player.velocity * 36) - projectile.position;
                }
                else
                {
                    velocity = player.position + - projectile.position;
                }
                velocity.Normalize();
                projectile.velocity = velocity.RotatedBy(projectile.ai[0]) * 24f;
                projectile.coldDamage = true;
                Main.PlaySound(SoundID.Item33, projectile.position);
            }
            if (projectile.timeLeft < 180)
            {
                projectile.spriteDirection = projectile.direction = (projectile.velocity.X > 0).ToDirectionInt();
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(projectile.velocity.X > 0 ? 45f : 135f);
                if (projectile.timeLeft % 15 == 0)
                {
                    Projectile.NewProjectile(projectile.position, new Vector2(6, 0).RotatedBy(projectile.velocity.ToRotation() + MathHelper.ToRadians(90f)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaSWPredict>(), 37, 1, projectile.owner);
                    Projectile.NewProjectile(projectile.position, new Vector2(6, 0).RotatedBy(projectile.velocity.ToRotation() + MathHelper.ToRadians(-90f)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaSWPredict>(), 37, 1, projectile.owner);

                }
                if (projectile.timeLeft < 60)
                {
                    projectile.alpha += 8;
                }
                if (projectile.alpha >= 254)
                {
                    projectile.Kill();
                }

                int DustID = Dust.NewDust(projectile.position, 8, 8, 27, Main.rand.NextFloat(), Main.rand.NextFloat());

            }
            else
            {
                projectile.alpha -= 8;
                projectile.rotation += MathHelper.ToRadians(18f);
            }

            //If the distance between the live targeted npc and the projectile is less than 480 pixels

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

            if (projectile.timeLeft >= 180f && projectile.ai[1] == 0)
            {
                if (projectile.localAI[0] != 10)
                {
                    pos = projectile.Center;
                    projectile.localAI[0] = 10;
                }
                Color color = Color.White;
                Texture2D texture = ModContent.GetTexture("_7f/NPCs/Trail");
                Player player = Main.player[projectile.owner];

                float opacity = ((float)projectile.timeLeft - 180f) / 60f;
                Vector2 a = (player.position + player.velocity * 36f) - projectile.position;
                a.Normalize();
                a *= 4;

                Vector2 b = pos - Main.screenPosition;


                float c = 1f;
                color *= 0.75f;

                for (int i = 0; i < 360 * c; i++)
                {
                    spriteBatch.Draw(texture, b, null, color * opacity, a.ToRotation(), new Vector2(2, 2), 1, SpriteEffects.None, 1);
                    b += a;
                }





            }
            return true;
        }
    }
}