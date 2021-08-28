using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace _7f.NPCs
{
    public class LGC : ModNPC
    {
        int[] css = { 0, 0, 0, 0, 0, 0 };
        private float attackTimer
        {
            get => npc.ai[0];
            set => npc.ai[0] = value;
        }
        private float laserTimer
        {
            get => npc.ai[1];
            set => npc.ai[1] = value;
        }
        public float phase
        {
            get => npc.localAI[2];
            set => npc.localAI[2] = value;
        }

        private float timer
        {
            get => npc.localAI[3];
            set => npc.localAI[3] = value;
        }


        private float subphase
        {
            get => npc.localAI[0];
            set => npc.localAI[0] = value;
        }
        private float subphaseTimer
        {
            get => npc.localAI[1];
            set => npc.localAI[1] = value;
        }

        int phaseTimer = 0;

        bool dir = true;
        bool sep = true;

        int nextSubphase = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Little Grey cat");
            Main.npcFrameCount[npc.type] = 4;
            NPCID.Sets.TrailCacheLength[npc.type] = 25;
        }
        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.lavaImmune = true;
            npc.lifeMax = 69420;
            npc.boss = true;
            npc.knockBackResist = 0;
            npc.defense = 50;
            npc.dontTakeDamageFromHostiles = true;
            npc.friendly = false;
            npc.noTileCollide = true;
            npc.damage = 35;
            npc.width = 34;
            npc.height = 32;
            attackTimer = 0;
            subphaseTimer = 0;
            subphase = 1;
            npc.noGravity = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/LGC_a");
            musicPriority = MusicPriority.BossHigh;
            //todo: set music priority in phase 3/4
        }
        float tpdir;
        int attackType;
        int[] dusts = new int[360];
        float radius = (Main.expertMode ? 480f : 720f);
        Vector2 tppos = new Vector2();
        Vector2 teleport = new Vector2();
        int healthTimer = 0;
        int a = 0;

        public override void AI()
        {
            Player player = Main.player[npc.target];
            int DustID = Dust.NewDust(npc.position, 8, 8, 27, Main.rand.NextFloat(), Main.rand.NextFloat());
            Main.dust[DustID].noGravity = true;
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.velocity = new Vector2(0f, -10f);
                    if (npc.timeLeft > 30)
                    {
                        npc.timeLeft = 30;
                    }
                    return;
                }
            }
            //########################### SHIELD CREATION #####################

            timer++;
            for (int i = 0; i < 180; i++)
            {
                Vector2 shield = npc.position;

                shield.X += (float)Math.Cos(MathHelper.ToRadians(i * 4)) * radius;
                shield.Y += (float)Math.Sin(MathHelper.ToRadians(i * 4)) * radius;

                Main.dust[dusts[i]].position = shield;
            }
            if (timer % 60 == 0)
            {
                timer = 0;
                for (int i = 0; i < 180; i++)
                {

                    Main.dust[dusts[i]].active = false;
                }


                for (int i = 0; i < 180; i++)
                {
                    Vector2 shield = npc.position;

                    shield.X += (float)Math.Cos(MathHelper.ToRadians(i * 4)) * radius;
                    shield.Y += (float)Math.Sin(MathHelper.ToRadians(i * 4)) * radius;

                    int Dust2 = Dust.NewDust(shield, 8, 8, 27, Main.rand.NextFloat(), Main.rand.NextFloat());
                    Main.dust[Dust2].noGravity = true;

                    Main.dust[Dust2].scale = 3f;
                    dusts[i] = Dust2;
                }
            }

            if (Vector2.Distance(player.position, npc.position) > radius)
            {
                player.AddBuff(BuffID.Cursed, 1);
                if (phase >= 4)
                    player.AddBuff(BuffID.CursedInferno, 1);
            }










            //#######################################################################################
            //Phase 1 subphase 1: floating around player and shooting Phasma Darts in 6 directions
            if (phase == 0 && subphase == 1)
            {
                npc.coldDamage = false;
                laserTimer++;
                if (laserTimer % 30 == 0)
                {
                    int rnd = Main.rand.Next(0, 91);
                    laserTimer = 0;
                    for (int i = -180; i <= 180; i += 60)
                    {
                        Projectile.NewProjectile(npc.position, new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(i + rnd)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostilePredict>(), npc.damage, 1, Main.myPlayer, 1);
                    }
                    Main.PlaySound(SoundID.Item33, npc.position);
                }



                Vector2 circle = new Vector2();
                if (dir)
                {
                    circle.X = player.position.X + (float)Math.Cos(MathHelper.ToRadians(subphaseTimer * 3f)) * 240f;
                    circle.Y = player.position.Y + (float)Math.Sin(MathHelper.ToRadians(subphaseTimer * 3f)) * 240f;
                }
                else
                {
                    circle.X = player.position.X + (float)Math.Sin(MathHelper.ToRadians(subphaseTimer * 3f)) * 240f;
                    circle.Y = player.position.Y + (float)Math.Cos(MathHelper.ToRadians(subphaseTimer * 3f)) * 240f;
                }
                //Main.NewText(circle);
                Vector2 move = new Vector2(circle.X - npc.position.X, circle.Y - npc.position.Y);
                //move.Normalize();
                npc.velocity *= 0.97f;
                npc.velocity.Y += move.Y / 2f;
                npc.velocity.X += move.X / 2f;

                Vector2 velocityCheck = npc.velocity;
                velocityCheck.Normalize();
                if (Math.Abs(velocityCheck.X * 15) < Math.Abs(npc.velocity.X) || Math.Abs(velocityCheck.Y * 15) < Math.Abs(npc.velocity.Y))
                {

                    npc.velocity = velocityCheck * 15;
                }



                subphaseTimer++;
                if (subphaseTimer > 300) { subphaseTimer = 0; subphase++; }

            }
            //#######################################################################################
            //Phase 1 subphase 2: trying to ram player and charging to them when close enough
            if (phase == 0 && subphase == 2)
            {
                npc.coldDamage = true;
                subphaseTimer++;

                attackTimer++;
                if (attackTimer == 45)
                {
                    Vector2 jump = new Vector2(player.position.X - npc.position.X, player.position.Y - npc.position.Y);
                    jump.Normalize();
                    npc.velocity = jump * 16f;
                    Main.PlaySound(SoundID.Roar, npc.position, 0);
                }
                else if (attackTimer > 45 && attackTimer < 60)
                {
                    int Dust1 = Dust.NewDust(npc.position, 8, 8, 27, Main.rand.NextFloat(), Main.rand.NextFloat());
                    Main.dust[Dust1].noGravity = true;
                }
                else if (attackTimer >= 60)
                {
                    attackTimer = 0;
                    npc.velocity *= 0.7f;
                }
                else
                {
                    npc.velocity *= 0.98f;
                }


                if (attackTimer < 30)
                {
                    Vector2 move = new Vector2(player.position.X - npc.position.X, player.position.Y - npc.position.Y);
                    move.Normalize();
                    npc.velocity *= 0.97f;
                    npc.velocity.Y += move.Y / 2f;
                    npc.velocity.X += move.X / 2f;
                    if (Vector2.Distance(player.position, npc.position) > 240)
                    {
                        npc.velocity *= 1.1f;
                    }
                    Vector2 velocityCheck = npc.velocity;
                    velocityCheck.Normalize();
                    if (Math.Abs(velocityCheck.X * 10) < Math.Abs(npc.velocity.X) || Math.Abs(velocityCheck.Y * 10) < Math.Abs(npc.velocity.Y))
                    {
                        npc.velocity = velocityCheck * 10;
                    }
                }
                if (subphaseTimer > 300)
                {
                    subphaseTimer = 0;
                    subphase--;
                    dir = Main.rand.NextBool();
                    npc.velocity = new Vector2(0, 0);
                }
            }

            if (phase == 0)
            {
                phaseTimer++;
            }



            //################## PHASE 2 ##################







            if ((npc.life <= npc.lifeMax * 0.80f && phase == 0) || (phaseTimer >= 11 * 60 && phase == 0))
            {

                phase++;
                music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/LGC_b");
                subphase = 1;

                for (int i = 1; i < 7; i++)
                {
                    int k = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<NPCs.CS>(), 0, i - 1, 0, 0, 0, npc.target);
                    Main.npc[k].localAI[1] = npc.whoAmI;
                    css[i - 1] = k;

                }
                npc.dontTakeDamage = true;
                attackTimer = 0;
                npc.coldDamage = true;
            }

            if (phase == 1)
            {
                int counter = 0;
                for (int i = 0; i < 6; i++)
                {
                    if (Main.npc[css[i]].type != ModContent.NPCType<NPCs.CS>() || Main.npc[css[i]].active == false)
                    {
                        counter++;
                    }
                    else if (counter == 5)
                    {
                        Main.npc[css[i - 1]].localAI[2] = 5;
                        break;
                    }
                    else { break; }
                }
                if (counter == 6)
                {
                    npc.dontTakeDamage = false;
                    phase++;
                }





                if (attackTimer == 45)
                {

                    float posX1 = Main.rand.NextFloat(-700f, 701f);
                    float posY1 = -720f;

                    float posX2 = -posX1;
                    float posY2 = -posY1;




                    Vector2 pos1 = new Vector2(posX1 + player.position.X, posY1 + player.position.Y);
                    Vector2 pos2 = new Vector2(posX2 + player.position.X, posY2 + player.position.Y);

                    float angle = (pos2 - pos1).ToRotation();
                    sep = !sep;
                    pos1.X -= 300 * 6f;
                    pos2.X -= 300 * (sep ? -6 : 6);
                    Vector2 velocity = pos2 - pos1;
                    velocity.Normalize();
                    velocity *= 12;
                    Main.PlaySound(SoundID.Item105, player.position);
                    for (int i = 0; i <= 12; i++)
                    {
                        Projectile.NewProjectile(pos1, velocity, ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostilePredict>(), npc.damage, 2, Main.myPlayer);
                        pos1.X += 300f;
                        pos2.X += 300 * (sep ? -1 : 1);
                        velocity = pos2 - pos1;
                        velocity.Normalize();
                        velocity *= 14;
                    }
                }



                attackTimer++;


                if (attackTimer == 45)
                {
                    Vector2 jump = new Vector2(player.position.X - npc.position.X, player.position.Y - npc.position.Y);
                    jump.Normalize();
                    npc.velocity = jump * 9f;
                    Main.PlaySound(SoundID.Roar, npc.position, 0);
                }
                else if (attackTimer > 45 && attackTimer < 67)
                {
                    Main.dust[DustID].scale = 2f;
                }
                else if (attackTimer >= 68)
                {
                    attackTimer = 0;
                    npc.velocity *= 0.7f;
                }
                else
                {
                    npc.velocity *= 0.98f;
                }


                if (attackTimer < 30)
                {
                    Vector2 move = new Vector2(player.position.X - npc.position.X, player.position.Y - npc.position.Y);
                    move.Normalize();
                    npc.velocity *= 0.97f;
                    npc.velocity.Y += move.Y / 1.6f;
                    npc.velocity.X += move.X / 1.6f;
                    if (Vector2.Distance(player.position, npc.position) > 240)
                    {
                        npc.velocity *= 1.1f;
                    }
                    Vector2 velocityCheck = npc.velocity;
                    velocityCheck.Normalize();
                    if (Math.Abs(velocityCheck.X * 10) < Math.Abs(npc.velocity.X) || Math.Abs(velocityCheck.Y * 10) < Math.Abs(npc.velocity.Y))
                    {
                        npc.velocity = velocityCheck * 10;
                    }
                }








            }

            if (phase == 2 && subphase == 1)
            {
                npc.coldDamage = true;
                laserTimer++;
                if (laserTimer % 34 == 0)
                {
                    Vector2 velocity = player.position - npc.position;
                    velocity.Normalize();
                    velocity *= 10f;
                    laserTimer = 0;
                    for (float i = -50f; i <= 50f; i += 50f)
                    {
                        Projectile.NewProjectile(npc.position, velocity.RotatedBy(MathHelper.ToRadians(i)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostile>(), npc.damage, 1, Main.myPlayer);
                    }
                    Main.PlaySound(SoundID.Item33, npc.position);
                }


                Vector2 circle = new Vector2();
                if (dir)
                {
                    circle.X = player.position.X + (float)Math.Cos(MathHelper.ToRadians(subphaseTimer * 1.5f)) * 360f;
                    circle.Y = player.position.Y + 480f;
                }
                else
                {
                    circle.X = player.position.X + (float)Math.Sin(MathHelper.ToRadians(subphaseTimer * 1.5f)) * 360f;
                    circle.Y = player.position.Y + 480f;
                }
                //Main.NewText(circle);
                Vector2 move = new Vector2(circle.X - npc.position.X, circle.Y - npc.position.Y);
                //move.Normalize();
                npc.velocity.Y += move.Y / 1.3f;
                npc.velocity.X += move.X / 1.3f;

                Vector2 velocityCheck = npc.velocity;
                velocityCheck.Normalize();
                if (Math.Abs(velocityCheck.X * 15) < Math.Abs(npc.velocity.X) || Math.Abs(velocityCheck.Y * 15) < Math.Abs(npc.velocity.Y))
                {

                    npc.velocity = velocityCheck * 15;
                }



                subphaseTimer++;
                if (subphaseTimer > 300) { subphaseTimer = 0; attackTimer = 0; subphase = 2; npc.coldDamage = false; }

            }
            if (phase == 2 && subphase == 2)
            {





                if (attackTimer == 1 && subphaseTimer < 360)
                {

                    tpdir = Main.rand.Next(0, 4);
                    tpdir *= 90;
                    tpdir = MathHelper.ToRadians(tpdir);
                    teleport = player.position + new Vector2(120, 0).RotatedBy(tpdir);


                }

                if (attackTimer <= 136 && attackTimer % 2 == 0)
                {

                    int Dusta = Dust.NewDust(teleport, 8, 8, 27);
                    Main.dust[Dusta].scale = 3;

                }
                if (attackTimer == 137)
                {
                    npc.velocity = new Vector2(0, 0);
                    npc.position.Y = teleport.Y;
                    npc.position.X = teleport.X;
                    npc.velocity = new Vector2(0, 0);
                    Main.PlaySound(SoundID.Item46);
                    Vector2 velocity = new Vector2(12, 0);
                    for (int i = -180; i < 180; i += 30)
                    {
                        Projectile.NewProjectile(npc.position, velocity.RotatedBy(MathHelper.ToRadians(i)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostile>(), npc.damage, 1, Main.myPlayer);
                    }
                    attackTimer = 0;

                }

                Vector2 move = new Vector2(player.position.X - npc.position.X, player.position.Y - npc.position.Y);
                move.Normalize();
                if (Vector2.Distance(player.position, npc.position) > 360)
                {
                    move *= 1.2f;
                }
                npc.velocity *= 0.97f;
                npc.velocity.Y += move.Y / 1.3f;
                npc.velocity.X += move.X / 1.3f;

                Vector2 velocityCheck = npc.velocity;
                velocityCheck.Normalize();
                if (Math.Abs(velocityCheck.X * 15) < Math.Abs(npc.velocity.X) || Math.Abs(velocityCheck.Y * 15) < Math.Abs(npc.velocity.Y))
                {
                    npc.velocity = velocityCheck * 15;
                }






                attackTimer++;
                subphaseTimer++;

                if (subphaseTimer >= 365)
                {
                    subphaseTimer = 0; attackTimer = 0; subphase = Main.rand.NextBool() ? 1 : 3;
                }
            }

            if (phase == 2 && subphase == 3)
            {

                if (attackTimer == 0)
                {
                    //Vector2 teleport = new Vector2();
                    dir = Main.rand.NextBool();
                    tppos = new Vector2(player.position.X + (dir ? 480f : -480f), player.position.Y + Main.rand.NextFloat(-240, 240));
                }
                if (attackTimer < 34 && attackTimer % 2 == 0)
                {
                    int dust4 = Dust.NewDust(tppos, 16, 16, 21);
                    Main.dust[dust4].scale = 2;

                }
                if (attackTimer == 34)
                {
                    npc.Teleport(tppos);
                    npc.velocity = (player.position - npc.position) / 25f;
                    Main.PlaySound(SoundID.ForceRoar, (int)npc.position.X, (int)npc.position.Y, 0, 1, 0.6f);
                    attackTimer = -1;
                }
                if (attackTimer > 20)
                {
                    npc.velocity *= 0.9f;
                }




                attackTimer++;
                subphaseTimer++;
                if (subphaseTimer >= 250)
                {
                    subphaseTimer = 0; attackTimer = 0; subphase = Main.rand.NextBool() ? 1 : 2;
                }
            }
            //########################################################################################################################################################
            //########################################################################################################################################################
            //########################################################### PHASE 3 - 4 ################################################################################
            //########################################################################################################################################################
            //########################################################################################################################################################
            if (phase == 3)
            {
                music = -1;
                npc.boss = false;
                musicPriority = MusicPriority.None;
                if (healthTimer == 0)
                {
                    CombatText.NewText(npc.Hitbox, new Color(180, 0, 180), "Thought defeating ME will be this easy huh?..", true, false);
                }

                if (healthTimer == 120)
                {
                    CombatText.NewText(npc.Hitbox, new Color(180, 0, 180), "Defeating a pet of Moonlord...", true, false);
                }

                if (healthTimer == 240)
                {
                    CombatText.NewText(npc.Hitbox, new Color(180, 0, 180), "Actually, MOONLORD IS MY PET MUHAHA", true, false);
                }
                if (healthTimer >= 241 && healthTimer % 4 == 0 && healthTimer <= 260)
                {
                    CombatText.NewText(npc.Hitbox, new Color(180, 0, 180), Main.rand.Next(0, 3) == 0 ? "HAHA" : "HA", true, false);
                }

                if (healthTimer == 420)
                {
                    CombatText.NewText(npc.Hitbox, new Color(180, 0, 180), "Now... Take these hearts and become a great CAT FOOD FOR ME! MUHAHAHA", true, false);
                }

                if (healthTimer == 580)
                {
                    CombatText.NewText(npc.Hitbox, new Color(180, 0, 180), "Bad luck have suffering!..", true, false);
                }

                if (healthTimer <= 600 && healthTimer % 10 == 0)
                {
                    npc.life += npc.lifeMax / 72;
                    Item.NewItem(npc.position, Vector2.Zero, ItemID.Heart);
                    Main.PlaySound(SoundID.Meowmere, npc.position);
                    npc.HealEffect(npc.lifeMax / 72 + Main.rand.Next(-100, 100));
                }

                Vector2 move = new Vector2(player.position.X - npc.position.X, player.position.Y + 180 - npc.position.Y);
                move.Normalize();
                if (Vector2.Distance(player.position, npc.position) > 360)
                {
                    move *= 1.2f;
                }
                npc.velocity.Y += move.Y / 4f;
                npc.velocity.X += move.X / 4f;

                Vector2 velocityCheck = npc.velocity;
                velocityCheck.Normalize();
                if (Math.Abs(velocityCheck.X * 9) < Math.Abs(npc.velocity.X) || Math.Abs(velocityCheck.Y * 9) < Math.Abs(npc.velocity.Y))
                {
                    npc.velocity = velocityCheck * 9;
                }

                healthTimer++;
                if (healthTimer > 600)
                {
                    npc.boss = true;
                    music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/LGC_d");
                    phase = 4;
                    npc.dontTakeDamage = false;
                    subphase = 1;
                    attackTimer = 0;
                    laserTimer = 0;
                    subphaseTimer = 0;
                    radius *= 1.5f;
                    musicPriority = MusicPriority.BossHigh;
                    nextSubphase = 2;

                }
            }
            //#################################################### PHASE 4 ###################################################
            if (phase == 4)
            {
                player.AddBuff(ModContent.BuffType<Buffs.DemonicFight>(), 1);
                if (subphase == 1)
                {
                    if (attackTimer == 0)
                    {
                        dir = !dir;
                    }
                    if (attackTimer <= 30)
                    {
                        npc.velocity *= 0.98f;
                    }
                    if (attackTimer <= 137 && attackTimer > 30)
                    {
                        npc.damage = 0;
                        Vector2 move = new Vector2(player.position.X + (dir ? 480 : -480) - npc.position.X, player.position.Y - npc.position.Y);
                        move.Normalize();
                        if (Vector2.Distance(move, npc.position) > 40)
                        {
                            npc.velocity += move * 4f;
                        }
                        Vector2 velocityCheck = npc.velocity;
                        velocityCheck.Normalize();
                        if (Math.Abs(velocityCheck.X * 20) < Math.Abs(npc.velocity.X) || Math.Abs(velocityCheck.Y * 20) < Math.Abs(npc.velocity.Y))
                        {
                            npc.velocity = velocityCheck * 20;
                        }
                    }
                    if ((attackTimer == 34 && !(subphaseTimer == 34)) || attackTimer == 68 || attackTimer == 103 || attackTimer == 137)
                    {
                        int rot = Main.rand.NextBool() ? 0 : 30;
                        for (int i = 0; i < 360; i += 60)
                        {

                            Vector2 pos = new Vector2(720, 0).RotatedBy(MathHelper.ToRadians(i + rot));
                            Projectile.NewProjectile(pos + player.position, -pos / 240f, ModContent.ProjectileType<Projectiles.Hostile.PhasmaScytheAccelerate>(), 69, 0, Main.myPlayer, 1);
                        }
                    }
                    if (attackTimer == 137)
                    {
                        npc.damage = 69;
                        attackTimer = -1;
                        Vector2 velocity = (player.position - npc.position);
                        velocity.Normalize();
                        npc.velocity = velocity * 30;
                        Main.PlaySound(SoundID.ForceRoar, npc.position, 0);
                    }
                    attackTimer++;
                    subphaseTimer++;

                    if (subphaseTimer == 410) { Main.PlaySound(SoundID.Item9, npc.position); subphaseTimer = 0; attackTimer = nextSubphase == 3 ? 67 : 0; npc.damage = 69; subphase = nextSubphase; attackType = 1; npc.defense = 500; }
                }

                if (subphase == 2)
                {
                    npc.velocity *= 0.5f;
                    attackTimer++;

                    if (attackTimer == 34)
                    {
                        attackTimer = 0;
                        for (float i = 0 + a; i < 360 + a; i += 60f)
                        {
                            Projectile.NewProjectile(npc.position, new Vector2(7.5f, 0).RotatedBy(MathHelper.ToRadians(i)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaScytheRotate>(), 69, 0, Main.myPlayer, npc.whoAmI, dir ? 0.7f : -0.7f);

                            dir = !dir;
                            Projectile.NewProjectile(npc.position, new Vector2(7.5f, 0).RotatedBy(MathHelper.ToRadians(i)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaScytheRotate>(), 69, 0, Main.myPlayer, npc.whoAmI, dir ? 0.7f : -0.7f);
                        }

                        a += 20;
                    }

                    subphaseTimer++;
                    if (subphaseTimer == 411)
                    {
                        Main.PlaySound(SoundID.Item9, npc.position);
                        subphaseTimer = 0;
                        attackTimer = 0;
                        npc.damage = 69;
                        subphase = 6;
                        npc.defense = 50;
                        nextSubphase = 3;
                    }

                }

                if (subphase == 3)
                {
                    npc.velocity *= 0.5f;
                    attackTimer++;
                    if (attackTimer == 68 && subphaseTimer < 360)
                    {
                        dir = !dir;
                        int intdir = dir ? 1 : 0;
                        attackTimer = 0;
                        Main.PlaySound(SoundID.Item12, npc.position);
                        float posX3 = Main.rand.NextFloat(-120, 120);
                        float posY3 = Main.rand.NextFloat(-120, 120);
                        int proj1 = Projectile.NewProjectile(new Vector2(posX3, posY3).RotatedBy(MathHelper.ToRadians(0f)) + npc.position, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hostile.PhasmaSpear>(), 69, 3, Main.myPlayer);
                        Main.projectile[proj1].ai[1] = intdir;
                        if (dir)
                        {
                            proj1 = Projectile.NewProjectile(new Vector2(posX3, posY3).RotatedBy(MathHelper.ToRadians(90f)) + npc.position, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hostile.PhasmaSpear>(), 69, 3, Main.myPlayer, MathHelper.ToRadians(90), 1);
                            proj1 = Projectile.NewProjectile(new Vector2(posX3, posY3).RotatedBy(MathHelper.ToRadians(180f)) + npc.position, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hostile.PhasmaSpear>(), 69, 3, Main.myPlayer, MathHelper.ToRadians(180), 1);
                            proj1 = Projectile.NewProjectile(new Vector2(posX3, posY3).RotatedBy(MathHelper.ToRadians(270f)) + npc.position, Vector2.Zero, ModContent.ProjectileType<Projectiles.Hostile.PhasmaSpear>(), 69, 3, Main.myPlayer, MathHelper.ToRadians(270), 1);
                        }


                    }
                    subphaseTimer++;
                    if (subphaseTimer == 410) { Main.PlaySound(SoundID.Item9, npc.position); subphaseTimer = 0; attackTimer = 0; laserTimer = 0; npc.damage = 0; subphase = 1; npc.defense = 50; nextSubphase = 4; }
                }

                if (subphase == 4)
                {
                    npc.velocity *= 0.5f;
                    attackTimer++;
                    laserTimer++;
                    if (laserTimer % 20 == 0)
                    {
                        Projectile.NewProjectile(npc.Center, new Vector2(0, 12).RotatedBy(MathHelper.ToRadians(laserTimer / 2)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaSW>(), 69, 0, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center, new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(laserTimer / 2)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaSW>(), 69, 0, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center, new Vector2(0, -12).RotatedBy(MathHelper.ToRadians(laserTimer / 2)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaSW>(), 69, 0, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center, new Vector2(-12, 0).RotatedBy(MathHelper.ToRadians(laserTimer / 2)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaSW>(), 69, 0, Main.myPlayer);
                    }
                    if (attackType == 1 && attackTimer == 136)
                    {
                        for (int i = -960; i < 960; i += 160)
                        {
                            Projectile.NewProjectile(new Vector2(player.position.X + i, player.position.Y - 960), new Vector2(0, 15), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostilePredict>(), 69, 0, Main.myPlayer);
                            Projectile.NewProjectile(new Vector2(player.position.X + i, player.position.Y + 960), new Vector2(0, -15), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostilePredict>(), 69, 0, Main.myPlayer);
                        }

                        for (int i = -960; i < 960; i += 160)
                        {
                            Projectile.NewProjectile(new Vector2(player.position.X - 960, player.position.Y + i), new Vector2(15, 0), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostilePredict>(), 69, 0, Main.myPlayer);
                            Projectile.NewProjectile(new Vector2(player.position.X + 960, player.position.Y + i), new Vector2(-15, 0), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostilePredict>(), 69, 0, Main.myPlayer);
                        }
                        attackType++;
                        attackTimer = 0;
                        Main.PlaySound(SoundID.Item46, player.position);
                    }

                    if (attackType == 2 && attackTimer == 137)
                    {
                        for (int i = 0; i < 360; i += 60)
                        {
                            Projectile.NewProjectile(new Vector2(480, 0).RotatedBy(MathHelper.ToRadians(i)) + player.position, new Vector2(-1.5f, 0).RotatedBy(MathHelper.ToRadians(i)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostilePredict>(), 69, 0, Main.myPlayer, 1);
                        }
                        for (int i = 20; i < 360; i += 60)
                        {
                            Projectile.NewProjectile(new Vector2(480, 0).RotatedBy(MathHelper.ToRadians(i)) + player.position, new Vector2(-3, 0).RotatedBy(MathHelper.ToRadians(i)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostilePredict>(), 69, 0, Main.myPlayer, 1);
                        }
                        for (int i = 40; i < 360; i += 60)
                        {
                            Projectile.NewProjectile(new Vector2(480, 0).RotatedBy(MathHelper.ToRadians(i)) + player.position, new Vector2(-4.5f, 0).RotatedBy(MathHelper.ToRadians(i)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostilePredict>(), 69, 0, Main.myPlayer, 1);
                        }
                        attackType++;
                        attackTimer = 0;
                        Main.PlaySound(SoundID.Item46, player.position);
                    }

                    if (attackType == 3 && attackTimer == 136)
                    {


                        for (int i = 0; i < 360; i += 30)
                        {
                            Vector2 distanceVector = Vector2.Normalize(new Vector2(10, 0).RotatedBy(MathHelper.ToRadians(i))).RotatedBy(MathHelper.PiOver2);
                            Projectile.NewProjectile(new Vector2(-480, 0).RotatedBy(MathHelper.ToRadians(i)) + player.position + distanceVector * 120, new Vector2(10f, 0).RotatedBy(MathHelper.ToRadians(i)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostilePredict>(), 69, 0, Main.myPlayer);
                        }
                        for (int i = 360; i > 0; i -= 30)
                        {
                            Vector2 distanceVector = Vector2.Normalize(new Vector2(10, 0).RotatedBy(MathHelper.ToRadians(i))).RotatedBy(MathHelper.PiOver2);
                            Projectile.NewProjectile(new Vector2(480, 0).RotatedBy(MathHelper.ToRadians(i)) + player.position + distanceVector * 120, new Vector2(-10f, 0).RotatedBy(MathHelper.ToRadians(i)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostilePredict>(), 69, 0, Main.myPlayer);
                        }
                        attackType++;
                        attackTimer = 0;
                        Main.PlaySound(SoundID.Item46, player.position);
                    }

                    if (attackType == 4 && attackTimer == 137)
                    {
                        for (int i = -960; i < 960; i += 160)
                        {
                            Projectile.NewProjectile(new Vector2(player.position.X + i, player.position.Y - 960), new Vector2(0, 15).RotatedBy(MathHelper.PiOver4), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostilePredict>(), 69, 0, Main.myPlayer);
                            Projectile.NewProjectile(new Vector2(player.position.X + i, player.position.Y + 960), new Vector2(0, -15).RotatedBy(MathHelper.PiOver4), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostilePredict>(), 69, 0, Main.myPlayer);
                        }

                        for (int i = -960; i < 960; i += 160)
                        {
                            Projectile.NewProjectile(new Vector2(player.position.X - 960, player.position.Y + i), new Vector2(15, 0).RotatedBy(MathHelper.PiOver4), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostilePredict>(), 69, 0, Main.myPlayer);
                            Projectile.NewProjectile(new Vector2(player.position.X + 960, player.position.Y + i), new Vector2(-15, 0).RotatedBy(MathHelper.PiOver4), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostilePredict>(), 69, 0, Main.myPlayer);
                        }
                        attackTimer = 0;
                        Main.PlaySound(SoundID.Item46, player.position);
                        attackType = 5;
                        laserTimer = 0;
                    }
                    if (attackType == 5)
                    {
                        laserTimer = 0;
                        nextSubphase = 5;
                        attackType = 1;
                        attackTimer = 0;
                        subphase = 6;
                        npc.defense = 50;
                        Main.PlaySound(SoundID.Item9, npc.position);
                    }

                }

                if (subphase == 5)
                {
                    npc.velocity *= 0.4f;
                    if (subphaseTimer < 310)
                    {
                        var proj1 = Projectile.NewProjectileDirect(npc.position, new Vector2(8f * (1f - subphaseTimer / 600f), 0f).RotatedBy(MathHelper.ToRadians(attackTimer * (subphaseTimer / 600f) * 19.43f)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostile>(), 69, 0, Main.myPlayer);
                        proj1.timeLeft *= 8;
                        var proj2 = Projectile.NewProjectileDirect(npc.position, new Vector2(-8f * (1f - subphaseTimer / 600f), 0f).RotatedBy(MathHelper.ToRadians(attackTimer * (subphaseTimer / 600f) * 19.43f)), ModContent.ProjectileType<Projectiles.Hostile.PhasmaDartHostile>(), 69, 0, Main.myPlayer);
                        proj2.timeLeft *= 8;

                    }
                    attackTimer++;
                    attackTimer %= 360;
                    subphaseTimer++;
                    if (subphaseTimer == 410)
                    {
                        Main.PlaySound(SoundID.Item9, npc.position); subphaseTimer = 0; attackTimer = 0; laserTimer = 0; npc.damage = 0; subphase = 1; npc.defense = 50; nextSubphase = 2;
                    }

                }
                if (subphase == 6)
                {
                    if (attackTimer == 0)
                    {
                        dir = !dir;
                    }
                    if (attackTimer <= 30)
                    {
                        npc.velocity *= 0.98f;
                    }
                    if (attackTimer <= 137 && attackTimer > 30)
                    {
                        npc.damage = 0;
                        Vector2 move = new Vector2(player.position.X + (dir ? 480 : -480) - npc.position.X, player.position.Y - npc.position.Y);
                        move.Normalize();
                        if (Vector2.Distance(move, npc.position) > 40)
                        {
                            npc.velocity += move * 4f;
                        }
                        Vector2 velocityCheck = npc.velocity;
                        velocityCheck.Normalize();
                        if (Math.Abs(velocityCheck.X * 20) < Math.Abs(npc.velocity.X) || Math.Abs(velocityCheck.Y * 20) < Math.Abs(npc.velocity.Y))
                        {
                            npc.velocity = velocityCheck * 20;
                        }
                    }
                    if ((attackTimer == 68 && subphaseTimer != 68) || attackTimer == 137)
                    {
                        Main.PlaySound(SoundID.Item116, npc.position);
                        for (int i = -41; i < 41; i += 40)
                        {
                            Projectile.NewProjectile(npc.position, Vector2.Normalize(player.position - npc.position).RotatedBy(MathHelper.ToRadians(i)) * 18, ModContent.ProjectileType<Projectiles.Hostile.PhasmaMoth>(), 69, 0, Main.myPlayer, 1, 410 - subphaseTimer);
                        }
                    }
                    if (attackTimer == 137)
                    {
                        npc.damage = 69;
                        attackTimer = -1;
                        Vector2 velocity = (player.position - npc.position);
                        velocity.Normalize();
                        npc.velocity = velocity * 30;
                        Main.PlaySound(SoundID.ForceRoar, npc.position, 0);
                    }
                    attackTimer++;
                    subphaseTimer++;

                    if (subphaseTimer == 410) { Main.PlaySound(SoundID.Item9, npc.position); subphaseTimer = 0; attackTimer = nextSubphase == 3 ? 67 : 0; npc.damage = 69; subphase = nextSubphase; attackType = 1; npc.defense = 500; }
                }
            }


        }
        public override bool CheckDead()
        {
            if (phase <= 2)
            {
                npc.life = npc.lifeMax / 100;
                npc.dontTakeDamage = true;
                phase = 3;
                return false;
            }
            else
            {
                return true;
            }
        }
        private const int Phase1a = 0;
        private const int Phase1b = 1;
        private const int Phase2a = 2;
        private const int Phase2b = 3;

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

            if (phase == 0)
            {
                npc.frameCounter++;
                if (npc.frameCounter < 30)
                {
                    npc.frame.Y = Phase1a * frameHeight;
                }
                else if (npc.frameCounter < 60)
                {
                    npc.frame.Y = Phase1b * frameHeight;
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }
            else
            {
                npc.frameCounter++;
                if (npc.frameCounter < 30)
                {
                    npc.frame.Y = Phase2a * frameHeight;
                }
                else if (npc.frameCounter < 60)
                {
                    npc.frame.Y = Phase2b * frameHeight;
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }

        }

        public override void NPCLoot()
        {
            downed.downed.downedLGC = true;
            Item.NewItem(npc.position, new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Accessories.AncientRune>(), 1);
            Item.NewItem(npc.position, new Vector2(npc.width, npc.height), ItemID.Fake_MushroomChest, Main.rand.Next(40, 75));
            Item.NewItem(npc.position, new Vector2(npc.width, npc.height), ItemID.LunarOre, Main.rand.Next(40, 75));
            Item.NewItem(npc.position, new Vector2(npc.width, npc.height), ItemID.MartianChandelier, 69);
            if (Main.rand.Next(1, 4) == 1)
            {
                Item.NewItem(npc.position, new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Weapons.PhasmaKnives>(), 1);
            }

        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }
        int angle = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var texture = ModContent.GetTexture("_7f/NPCs/LGC");
            var position = npc.Center;
            angle++;
            for (int i = 0; i < 4; i++)
            {
                var drawPosition = (position + new Vector2(16f, 0f).RotatedBy(MathHelper.PiOver2 * i + MathHelper.ToRadians(angle % 360))) - Main.screenPosition;
                var color = new Color(190, 0, 190, 2);
                spriteBatch.Draw(texture, drawPosition, new Rectangle(0, 0, 34, 32), color, npc.rotation, new Vector2(17, 16), 1, npc.spriteDirection != 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1);
                // draw it
            }
            for (int i = 0; i < 4; i++)
            {
                var drawPosition = (position + new Vector2(12f, 0f).RotatedBy(MathHelper.PiOver2 * i + MathHelper.ToRadians(360 - angle % 360))) - Main.screenPosition;
                var color = new Color(190, 0, 190, 2);
                spriteBatch.Draw(texture, drawPosition, new Rectangle(0, 0, 34, 32), color, npc.rotation, new Vector2(17, 16), 1, npc.spriteDirection != 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1);
                // draw it
            }
            Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
            for (int k = 0; k < npc.oldPos.Length; k++)
            {
                Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                Color color = npc.GetAlpha(drawColor) * ((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length);
                spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, null, color, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
            }
            return true;
        }

        /*public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Player player = Main.player[npc.target];
            if (phase == 0 && attackTimer == 2 && subphase == 2)
            {

                float posX1, posY1, posX2, posY2;
                float posX = posX1 = Main.rand.NextFloat(-400f, 401f);
                float posY = posY1 = -1440f;
                posX2 = posX1 * -1;
                posY2 = posY1 * -1;




                Vector2 pos1 = new Vector2(posX1 + player.position.X, posY1 + player.position.Y);
                Vector2 pos2 = new Vector2(posX2 + player.position.X, posY2 + player.position.Y);
                posX1 += player.position.X;
                posY1 += player.position.Y;
                
                
                Main.NewText(pos1);
                Main.NewText(pos2);
                Main.NewText(Vector2.Distance(pos1, pos2));
                for (float i = Vector2.Distance(pos1, pos2); i > 0; i -= 4)
                {
                    //new Vector2(4, 0).RotatedBy(MathHelper.ToRadians((pos2 - pos1).ToRotation()));
                    Texture2D texture = ModContent.GetTexture("_7f/NPCs/Trail");
                    posX1 += (pos1.X - pos2.X) / (Vector2.Distance(pos1, pos2) / 4);
                    posY1 += (pos1.Y - pos2.Y) / (Vector2.Distance(pos1, pos2) / 4);
                    //spriteBatch.Draw(texture, new Vector2(posX1, posY1), new Rectangle( 0, 0, 4, 4) , Color.White, (float)((pos2 - pos1).ToRotation()), new Vector2(2, 2), 10, SpriteEffects.None, 0f);
                    spriteBatch.Draw(texture, new Vector2(posX1 - Main.screenPosition.X, posY1 - Main.screenPosition.Y), Color.White);
                }
                Main.NewText("line is done");

            }



            return true;
        }*/


    }
}