using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace _7f.Projectiles.Hostile
{
    public class PhasmaMoth : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phasma Moth (lol cata xdxd)");
            Main.projFrames[projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            projectile.width = 156;
            projectile.height = 120;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 480;
            //projectile.localNPCHitCooldown = -1;
            projectile.penetrate = 300;
            projectile.tileCollide = false;
            projectile.alpha = 0;
        }
        public float phase
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }
        public float time
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }
        const int dash = 1;
        const int launchsmall = 2;
        const int fadeaway = 3;
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.BrokenArmor, 60 * 5);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        Vector2 pos;
        public override void AI()
        {
            time--;
            Player player = Main.player[projectile.owner];


            if (projectile.timeLeft == 419)
            {
                phase = dash;
            }
            if (projectile.timeLeft == 320)
            {
                phase = launchsmall;
            }
            if (projectile.timeLeft == 260)
            {
                phase = fadeaway;
            }

            if (phase == dash)
            {

                // + MathHelper.PiOver2;
                if (projectile.timeLeft > 360)
                {
                    projectile.rotation = (player.position - projectile.position).ToRotation();
                    projectile.velocity = Vector2.Normalize(-(player.position - projectile.position)) * 3f;
                    projectile.spriteDirection = projectile.direction = (player.position.X - projectile.position.X > 0).ToDirectionInt();
                    if (projectile.spriteDirection == 1)
                    {
                        projectile.rotation += MathHelper.Pi;
                    }
                }
                else
                {
                    if (projectile.timeLeft == 360)
                    {
                        pos = player.position;
                        projectile.rotation = (pos - projectile.position).ToRotation();
                        projectile.spriteDirection = projectile.direction = (pos.X - projectile.position.X > 0).ToDirectionInt();
                        if (projectile.spriteDirection == 1)
                        {
                            projectile.rotation += MathHelper.Pi;
                        }
                    }

                    Main.PlaySound(SoundID.Roar, projectile.position, 0);
                    projectile.velocity += Vector2.Normalize(pos - projectile.position) * 2f;
                    if (Math.Abs((Vector2.Normalize(projectile.velocity) * 18).X) < Math.Abs(projectile.velocity.X))
                    {
                        projectile.velocity = Vector2.Normalize(projectile.velocity) * 18;
                    }
                }
            }
            if (phase == launchsmall)
            {
                projectile.spriteDirection = projectile.direction = (player.position.X - projectile.position.X > 0).ToDirectionInt();
                projectile.velocity *= 0.7f;
                projectile.rotation = (player.position - projectile.position).ToRotation();
                if (projectile.spriteDirection == 1)
                {
                    projectile.rotation += MathHelper.Pi;
                }
                if (projectile.timeLeft % 4 == 0)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        int angle = i * 60;
                        Vector2 velocity = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(angle));
                        
                        Projectile.NewProjectile(projectile.position, velocity, ModContent.ProjectileType<Projectiles.Hostile.PhasmaMothSmall>(), projectile.damage, 0, projectile.owner, (projectile.timeLeft - 290)/16f, time);
                    }
                }
            }
            if (phase == fadeaway)
            {
                projectile.spriteDirection = projectile.direction = (player.position.X - projectile.position.X > 0).ToDirectionInt();
                projectile.rotation = (player.position - projectile.position).ToRotation();
                if (projectile.spriteDirection == 1)
                {
                    projectile.rotation += MathHelper.Pi;
                }
                projectile.damage = 0;
                projectile.alpha += 8;
                if (projectile.alpha > 250)
                {
                    projectile.Kill();
                }
            }


        }

        //----------------------
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (projectile.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameHeight = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
            int startY = frameHeight * projectile.frame;
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            origin.X = (float)(projectile.spriteDirection == 1 ? sourceRectangle.Width - 20 : 20);

            Color drawColor = projectile.GetAlpha(lightColor);
            Main.spriteBatch.Draw(texture,
                projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY),
                sourceRectangle, drawColor, projectile.rotation + MathHelper.Pi, origin, projectile.scale, spriteEffects, 0f);
            if (++projectile.frameCounter >= 4)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 6)
                {
                    projectile.frame = 0;
                }
            }
            return false;
        }
        //----------------------

        //----------------------


    }
}