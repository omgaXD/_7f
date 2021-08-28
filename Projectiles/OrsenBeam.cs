using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace _7f.Projectiles
{
    public class OrsenBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orsen");
        }
        public override void SetDefaults()
        {
            projectile.melee = true;
            projectile.width = 26;
            projectile.height = 26;
            projectile.friendly = true;
            projectile.timeLeft = projectile.ai[0] != 1 ? 360 : 60;
            //projectile.localNPCHitCooldown = -1;
            projectile.penetrate = 5;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 30;
        }
        public override void AI()
        {
            if (projectile.ai[0] == 1)
            {
                projectile.tileCollide = false;
            }
            projectile.spriteDirection = projectile.direction = (projectile.velocity.X > 0).ToDirectionInt();
            projectile.rotation += MathHelper.ToRadians(10f);
            Player owner = Main.player[projectile.owner];
            Lighting.AddLight(projectile.position, 0.3f, 1f, 0.3f);
            if (Main.rand.Next(0, 4) == 0)
            {
                int DustID = Dust.NewDust(projectile.Center, projectile.width + 4, projectile.height + 4, 131, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 0, default(Color), 0.75f); //Spawns dust
                Main.dust[DustID].noGravity = true;
            }

            if (projectile.timeLeft < 180)
            {
                projectile.alpha += 5;
            }
            if (projectile.alpha >= 254)
            {
                projectile.Kill();
            }



            //If the distance between the live targeted npc and the projectile is less than 480 pixels

        }
        //----------------------
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            const int radius = 1200;
            if (projectile.ai[0] != 1)
            {

                Main.PlaySound(SoundID.Item119, projectile.position);
                for (int i = 0; i < 360; i += 6)
                {
                    Vector2 pos = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(i));
                    Projectile.NewProjectile(pos + target.position, Vector2.Normalize(-pos) * 30, projectile.type, damage * 3, 10, projectile.owner, 1);
                }
                projectile.Kill();
            }
        }
        //----------------------
        public override void Kill(int timeLeft)
        {
            if (projectile.alpha < 250)
            {
                for (int i = 0; i < 20; i++)
                {
                    int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height, 131, 0, 0, 120, default(Color), 0.75f); //Spawns dust
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


    }
}