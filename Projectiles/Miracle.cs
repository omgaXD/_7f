using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace _7f.Projectiles
{
    public class Miracle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("O.M.F.G's Miracle");
        }
        public override void SetDefaults()
        {
            projectile.ranged = true;
            projectile.width = 26;
            projectile.height = 26;
            projectile.friendly = true;
            projectile.timeLeft = projectile.ai[0] != 1 ? 360 : 60;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 30;
            projectile.aiStyle = 1;
            aiType = ProjectileID.VortexBeaterRocket;
        }
        
        //----------------------
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.boss == true) {
                damage *= 5;
            }
            projectile.Kill();
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
            Main.PlaySound(SoundID.Item115);
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