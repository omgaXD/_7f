using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace _7f.Projectiles
{
    public class PhasmaKnive : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phasma Knive");
		}
        public override void SetDefaults()
        {
            projectile.scale = 4;
            projectile.melee = true;
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.timeLeft = 600;
            //projectile.localNPCHitCooldown = -1;
            projectile.penetrate = 10;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 30; 
        }
        public override void AI()
        {
            projectile.spriteDirection = projectile.direction = (projectile.velocity.X > 0).ToDirectionInt();
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            if (projectile.timeLeft >= 560) {
                projectile.tileCollide = false;
            } else {
                projectile.tileCollide = true;
            }
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
            projectile.timeLeft--;
            Lighting.AddLight(projectile.position, 0f, 0.3f, 1f);
            int DustID = Dust.NewDust(projectile.position, projectile.width + 4, projectile.height + 4, Main.rand.NextBool() ? 176 : 179, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 120, default(Color), 0.75f); //Spawns dust
            Main.dust[DustID].noGravity = true;
            
            if (projectile.timeLeft < 590) {
                Vector2 move = Vector2.Zero;
			    float distance = 400f;
			    bool target = false;
                for (int k = 0; k < 200; k++) {
				if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && projectile.localNPCImmunity[Main.npc[k].whoAmI] < 1) {
					Vector2 newMove = Main.npc[k].Center - projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < distance) {
						move = newMove;
						distance = distanceTo;
						target = true;
					}
				}
			}
			if (target) {
				AdjustMagnitude(ref move);
				projectile.velocity = (projectile.velocity * 1.5f + move);
				projectile.velocity.Normalize();
                projectile.velocity *= 24f;
                projectile.velocity += move.SafeNormalize(move);
			    }
            }
            if (projectile.timeLeft < 380) {
                projectile.alpha += 3;
            }
            if (projectile.alpha >= 254) {
                projectile.Kill();
            }
            
            

           //If the distance between the live targeted npc and the projectile is less than 480 pixels
           
        }
        //----------------------
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)	 
        {
            projectile.timeLeft -= 60;
            Player owner = Main.player[projectile.owner];
            if (Main.myPlayer == projectile.owner && projectile.localAI[1] != 1) {
                projectile.localAI[1] = 1;
                if ((int)(damage/50)>=1 && Main.rand.NextBool()) {
                    owner.statLife += (int)(damage/50);
                    owner.HealEffect((int)(damage/50));
                }
                Projectile.NewProjectile(owner.position,new Vector2(0,0),ModContent.ProjectileType<PhasmaDart>(), (int)(projectile.damage * 0.5f), 0, owner.whoAmI, target.whoAmI);
            }
        }
        //----------------------
        public override void Kill(int timeLeft)
        {
            if (projectile.alpha < 250) {
                Random rnd = new Random();
                for (int i = 0; i < 20; i++) {
                    int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height, Main.rand.NextBool() ? 176 : 179, (float)rnd.NextDouble()-.5f, (float)rnd.NextDouble()-.5f, 120, default(Color), 0.75f); //Spawns dust
                }
            }
        }
        //----------------------
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //if (projectile.localAI[0] != 1) Main.PlaySound(SoundID.Dig,projectile.position);
            projectile.localAI[0] = 1;
            projectile.timeLeft -= 5;
            projectile.alpha += 25;
            projectile.velocity = projectile.oldVelocity * new Vector2((float)0.86,(float)0.86);
            return false;
        }
        //----------------------
        private void AdjustMagnitude(ref Vector2 vector) 
        {
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 12f) {
				vector *= 12f / magnitude;
            }
		}
        
        
    }
}