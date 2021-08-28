using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace _7f.Projectiles
{
	public class TestProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("English Display Name Here");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
		}

		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.timeLeft = 300;

		}

        public override bool OnTileCollide(Vector2 oldVelocity) {
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
            if (projectile.owner == Main.myPlayer) {
				Main.PlaySound(SoundID.Item10, projectile.position);
                if (projectile.velocity.X != oldVelocity.X) {
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y) {
					projectile.velocity.Y = -oldVelocity.Y;
				}
				projectile.timeLeft -= 20;
				projectile.ai[0] = 1;
            }
            return false;
        }
		public override void Kill(int timeLeft) {
			for (int o = 0; o < 5; o++) {
				int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);            	Main.dust[DustID].noGravity = true;
				Main.dust[DustID].velocity*=2f;
			}

			Main.PlaySound(SoundID.Splash, projectile.position);
		}
		public override bool PreAI() {
			projectile.timeLeft--;
			return true;
		}
		public override void AI()
       		 {
            Player owner = Main.player[projectile.owner]; //Makes a player variable of owner set as the player using the projectile
            Lighting.AddLight(projectile.position, 0.5f, 0.25f, 0f);
            projectile.alpha = 0; //Semi Transparent
            projectile.rotation += (float)projectile.direction * 0.8f; //Spins in a good speed
            int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width + 4, projectile.height + 4, 36, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 120, default(Color), 0.75f); //Spawns dust
            Main.dust[DustID].noGravity = true; //Makes dust not fall
            if(projectile.timeLeft <= 200 || projectile.ai[0] == 1) 
            {
                projectile.velocity.Y = Math.Min(1f + projectile.velocity.Y,12f);
            } else {
				projectile.velocity.Y = Math.Min(0.2f + projectile.velocity.Y,12f);
			}
        }


		// Additional hooks/methods here.
	}
}