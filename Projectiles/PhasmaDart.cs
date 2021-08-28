using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace _7f.Projectiles
{
    public class PhasmaDart : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phasma Dart");
		}
        public override void SetDefaults()
        {
            projectile.scale = 4;
            projectile.melee = true;
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.timeLeft = 120;
            //projectile.localNPCHitCooldown = -1;
            projectile.penetrate = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.tileCollide = false;
        }
        public override void AI()
        {
            if (Main.npc[(int)projectile.ai[0]].active != true) {
                projectile.Kill();
            }
            projectile.velocity = (Main.npc[(int)projectile.ai[0]].position - projectile.position);
            projectile.velocity.Normalize();
            projectile.velocity *= 15;
            projectile.timeLeft--;
            projectile.spriteDirection = projectile.direction = (projectile.velocity.X > 0).ToDirectionInt();
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            if (projectile.spriteDirection == 1) // facing right
            {
	            drawOffsetX = 20; // These values match the values in SetDefaults
	            drawOriginOffsetY = 4;
	            drawOriginOffsetX = 0;
            }
            else
            {
            	// Facing left.
            	// You can figure these values out if you flip the sprite in your drawing program.
            	drawOffsetX = 20; // 0 since now the top left corner of the hitbox is on the far left pixel.
            	drawOriginOffsetY = 4; // doesn't change
            	drawOriginOffsetX = 0; // Math works out that this is negative of the other value.
            }
            Player owner = Main.player[projectile.owner];
            Lighting.AddLight(projectile.position, 0f, 0.25f, 0.8f);
            int DustID1 = Dust.NewDust(projectile.position, projectile.width + 4, projectile.height + 4, Main.rand.NextBool() ? 176 : 179, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 120, default(Color), 0.75f); //Spawns dust            
            Main.dust[DustID1].noGravity = true;
            int DustID2 = Dust.NewDust(projectile.position, projectile.width + 4, projectile.height + 4, Main.rand.NextBool() ? 176 : 179, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 120, default(Color), 0.75f); //Spawns dust
            Main.dust[DustID2].noGravity = true;
            
            

            if (projectile.timeLeft < 60) {
                projectile.alpha += 8;
            }
            if (projectile.alpha >= 254) {
                projectile.Kill();
            }
            
            

           //If the distance between the live targeted npc and the projectile is less than 480 pixels
           
        }
        //----------------------
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)	 
        {
            target.AddBuff(BuffID.Venom,180);
            target.AddBuff(BuffID.CursedInferno,180);
            target.AddBuff(BuffID.OnFire,180);
            target.AddBuff(BuffID.Frostburn,180);
            projectile.Kill();

        }
        //----------------------
        public override void Kill(int timeLeft)
        {
            if (projectile.alpha < 250) {
                Random rnd = new Random();
                for (int i = 0; i < 15; i++) {
                    int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height, Main.rand.NextBool() ? 176 : 179, (float)rnd.NextDouble()-.5f, (float)rnd.NextDouble()-.5f, 120, default(Color), 0.75f); //Spawns dust
                }
            }
        }
        //----------------------
        
        
    }
}