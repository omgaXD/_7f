using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace _7f.Projectiles.Hostile
{
    public class PhasmaMothSmall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phasma Moth (Small)");
            Main.projFrames[projectile.type] = 3;
        }



        public override void SetDefaults()
        {
            projectile.width = 62;
            projectile.height = 68;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 240;
            //projectile.localNPCHitCooldown = -1;
            projectile.penetrate = 300;
            projectile.tileCollide = false;
            projectile.alpha = 0;
            projectile.localAI[0] = 1;
            
        }
        public float time
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.Inflate((int)(projectile.width / -3f) , (int)(projectile.height / -3f));
        }
        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            damage = 0;
        }
        Vector2 pos = new Vector2();

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        public override void AI()
        {
            time--;
            if (time <= 20) {
                projectile.alpha += 16;
                projectile.damage = 0;
            }
            if (time <= 0) {
                projectile.Kill();
            }
            if (projectile.localAI[0]++ == 1) {
                pos = projectile.position;
            }
            if (projectile.position == pos && projectile.timeLeft < 220) {
                projectile.Kill();
            }
            projectile.spriteDirection = projectile.direction = (projectile.velocity.X > 0).ToDirectionInt();
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);


            //projectile.velocity *= 1.05f;
            projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(projectile.ai[0]));
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
                sourceRectangle, drawColor, projectile.rotation + (projectile.spriteDirection == 1 ? -MathHelper.PiOver2 : MathHelper.PiOver2), origin, projectile.scale, spriteEffects, 0f);
            if (++projectile.frameCounter >= 4)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 3)
                {
                    projectile.frame = 0;
                }
            }
            return false;
        }

    }
}