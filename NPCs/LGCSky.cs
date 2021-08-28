using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace _7f.NPCs
{
	public class LGCSky : CustomSky
	{
		private bool isActive;
		private float intensity;
		private int LGCIndex;


		public override void OnLoad() {
		}

		public override void Update(GameTime gameTime) {
			if (isActive && intensity < 1f) {
				intensity += 0.01f;
			}
			else if (!isActive && intensity > 0f) {
				intensity -= 0.01f;
			}
		}
		private bool UpdateLGCIndex() {
			int LGCType = ModContent.NPCType<NPCs.LGC>();
			if (LGCIndex >= 0 && Main.npc[LGCIndex].active && Main.npc[LGCIndex].type == LGCType) {
				return true;
			}
			LGCIndex = -1;
			for (int i = 0; i < Main.maxNPCs; i++) {
				if (Main.npc[i].active && Main.npc[i].type == LGCType) {
					LGCIndex = i;
					break;
				}
			}
			return LGCIndex >= 0;
		}

		private float GetIntensity() {
			return 1f - Utils.SmoothStep(3000f, 6000f, 200f);
		}

		public override Color OnTileColor(Color inColor) {
			float intensity = GetIntensity();
			return new Color(Vector4.Lerp(new Vector4(((Main.DiscoR * 0.7f)/2+128 )/ 255f, 100 / 255f, ((Main.DiscoR * 0.7f)/2+128 )/ 255f, 1f), inColor.ToVector4(), 1f - intensity));
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth) {
			if (maxDepth >= 0f && minDepth < 0f) {
				float intensity = GetIntensity();
				spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color((Main.DiscoR/255f) * 0.1f,0,(Main.DiscoR/255f) * 0.1f) * intensity);
				
			}
		}

		public override float GetCloudAlpha() {
			return 0f;
		}

		public override void Activate(Vector2 position, params object[] args) {
			isActive = true;
		}

		public override void Deactivate(params object[] args) {
			isActive = false;
		}

		public override void Reset() {
			isActive = false;
		}

		public override bool IsActive() {
			return isActive || intensity > 0f;
		}
	}
}