using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace _7f.NPCs
{
    public class CS : ModNPC
    {
        private float index
        {
            get => npc.ai[0];
        }
        private float attackTimer
        {
            get => npc.ai[1];
            set => npc.ai[1] = value;
        }
        private int kitty
        {
            get => (int)npc.localAI[1];
        }

        private float timer
        {
            get => npc.localAI[2];
            set => npc.localAI[2] = value;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kitty's servant");
        }
        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.lavaImmune = true;
            npc.lifeMax = 5000;
            npc.knockBackResist = 0;
            npc.defense = 50;
            npc.dontTakeDamageFromHostiles = true;
            npc.friendly = false;
            npc.noTileCollide = true;
            npc.damage = 35;
            npc.width = 30;
            npc.height = 30;
            attackTimer = 0;
            npc.noGravity = true;
        }
        public override void AI()
        {
            //####### MOVEMENT #########
            timer = timer % 360 + 1;

            Player player = Main.player[npc.target];
            NPC myKitty = Main.npc[kitty];
            Vector2 circle = new Vector2();

            circle.X = myKitty.position.X + (float)Math.Cos(MathHelper.ToRadians(timer * 3f + index * 60)) * 120f;
            circle.Y = myKitty.position.Y + (float)Math.Sin(MathHelper.ToRadians(timer * 3f + index * 60)) * 120f;

            //Main.NewText(circle);
            Vector2 move = new Vector2(circle.X - npc.position.X, circle.Y - npc.position.Y);
            //move.Normalize();
            npc.position = circle;

            Vector2 velocityCheck = npc.velocity;
            velocityCheck.Normalize();
            if (Math.Abs(velocityCheck.X * 20) < Math.Abs(npc.velocity.X) || Math.Abs(velocityCheck.Y * 15) < Math.Abs(npc.velocity.Y))
            {
                npc.velocity = velocityCheck * 20;
            }

            //####### ATTACK #########
            attackTimer++;
            if ((attackTimer + index * 10) %120 == 0)
            {
                Vector2 velocity = player.position - npc.position;
                velocity.Normalize();
                velocity *= 6;
                velocity += npc.velocity / 2f;
                Projectile.NewProjectile(npc.position, velocity, ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostile>(), npc.damage, 1, Main.myPlayer);
                Main.PlaySound(SoundID.Item33, npc.position);
                attackTimer = 0;

            }
        }

        public override void FindFrame(int frameHeight)
        {
            Player player = Main.player[npc.target];
            if (player.position.X - npc.position.X > 0)
            {
                npc.spriteDirection = 1;
                npc.rotation = (player.position - npc.position).ToRotation();
            }
            else if (player.position.X - npc.position.X < 0)
            {
                npc.spriteDirection = -1;
                npc.rotation = (player.position - npc.position).ToRotation() - 135f;
            }

        }
    }
}