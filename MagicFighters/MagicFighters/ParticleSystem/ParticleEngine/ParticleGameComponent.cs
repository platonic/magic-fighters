

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
namespace MagicFighters.ParticleEngine
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class ParticleComponent
	{
		private SpriteBatch spriteBatch;
		public List<Emitter> particleEmitterList;

		public ParticleComponent(SpriteBatch spriteBatch,Game game)
			
		{
            this.spriteBatch = spriteBatch;
            particleEmitterList = new List<Emitter>();
		}

		/// <summary>
		/// Allows the game component to perform any initialization it needs to before starting
		/// to run.  This is where it can query for any required services and load content.
		/// </summary>
		public  void Initialize()
		{
			

			
		}

		protected  void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			//spriteBatch = new SpriteBatch(Game.GraphicsDevice);

		
		}

		protected  void UnloadContent()
		{
			
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public  void Update(GameTime gameTime)
		{
			foreach (Emitter emitter in particleEmitterList)
			{
				emitter.UpdateParticles(gameTime);
			}
			
		}

		public  void Draw(GameTime gameTime)
		{
			//spriteBatch.Begin();

			foreach (Emitter emitter in particleEmitterList)
			{
				emitter.DrawParticles(gameTime, spriteBatch);
			}
			
		//	spriteBatch.End();

			
		}
	}
}
