using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace _7f.Projectiles
{
    public class GOsc : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Supernova Star");
        }
        public override void SetDefaults()
        {
            projectile.melee = true;
            projectile.width = 90;
            projectile.height = 34;
            projectile.friendly = true;
            projectile.timeLeft = 360;
            //projectile.localNPCHitCooldown = -1;
            projectile.penetrate = 10;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 30;
            projectile.tileCollide = false;
        }
        public override void AI()
        {
            //projectile.spriteDirection = projectile.direction = (projectile.velocity.X > 0).ToDirectionInt();
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(0f);
            Player owner = Main.player[projectile.owner];
            Lighting.AddLight(projectile.position, 1f, 0.8f, 1);
            //int DustID = Dust.NewDust(projectile.position, projectile.width + 4, projectile.height + 4, 21, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 0, default(Color), 0.75f); //Spawns dust
            //Main.dust[DustID].noGravity = true;
            if (Main.rand.Next(0,10) == 0) {
                Main.PlaySound(SoundID.Item9, projectile.position);
            }
            if (projectile.timeLeft < 340)
            {
                Vector2 move = Vector2.Zero;
                float distance = 150f;
                bool target = false;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && projectile.localNPCImmunity[Main.npc[k].whoAmI] < 1)
                    {
                        Vector2 newMove = Main.npc[k].Center - projectile.Center; // there
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance)
                        {
                            move = newMove;
                            distance = distanceTo;
                            target = true;
                        }
                    }
                }
                if (target)
                {
                    move = Vector2.Normalize(move) * 15;
                    projectile.velocity += move * 0.4f;
                    projectile.velocity = Vector2.Normalize(projectile.velocity) * 18;
                }
                if (projectile.ai[0] < projectile.position.Y)
                {
                    projectile.tileCollide = true;
                }
            }



            //If the distance between the live targeted npc and the projectile is less than 480 pixels

        }
        //----------------------
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.timeLeft -= 60;
            Player owner = Main.player[projectile.owner];
            projectile.velocity *= 0.97f;
        }
        //----------------------
        public override void Kill(int timeLeft)
        {
            if (projectile.alpha < 250)
            {
                Random rnd = new Random();
                for (int i = 0; i < 20; i++)
                {
                    int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height, 21, (float)rnd.NextDouble() - .5f, (float)rnd.NextDouble() - .5f, 120, default(Color), 0.75f); //Spawns dust
                }
            }
        }
        //----------------------
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.position);
            projectile.Kill();
            return false;
        }
        //----------------------
        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 9f)
            {
                vector *= 9f / magnitude;
            }
        }


    }
}