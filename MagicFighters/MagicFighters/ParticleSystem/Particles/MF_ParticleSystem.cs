using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace MagicFighters.ParticleSystem
{
    public class MF_ParticleSystem
    {
        Random random;

        SpriteBatch spriteBatch;

        List<MF_Particle> particles;

        Dictionary<string, Texture2D> textureDictionary;

        public void CreateFireParticles(Rectangle rect)
        {

            Vector2[] velocities = new Vector2[]
            {
                new Vector2(0,2),
                new Vector2(0,-2),
                new Vector2(2,-2),
                new Vector2(2,0),
                new Vector2(2,2),
                new Vector2(-2,2),
                new Vector2(-2,0),
                new Vector2(-2,-2),
               
            };
            for (int i = 0; i < velocities.Length; ++i)
            {
                MF_Particle p = CreateParticle();
                p.TextureName = "Textures/Particles/fire";
                p.Texture = textureDictionary[p.TextureName];
                p.Color = new Color(125, 108, 43);
                p.Position.X = rect.X + (rect.Width - 50) * (float)random.NextDouble();
                p.Position.Y = rect.Y + (rect.Height - 50) * (float)random.NextDouble();
                p.Alpha = 1.0f;
                p.AlphaRate = -2.0f;
                p.Life = 0.5f;
                p.Rotation = 0.0f;
                p.RotationRate = -2.0f + 4.0f * (float)random.NextDouble();
                p.Scale = 0.35f;
                p.ScaleRate = 0.2f;
                p.Velocity = velocities[i] * (float)random.NextDouble();
            }


        }

        public void CreatePictureParticles(Rectangle rect)
        {

            Vector2[] velocities = new Vector2[]
            {
                new Vector2(0,2),
                new Vector2(0,-2),
                new Vector2(2,-2),
                new Vector2(2,0),
                new Vector2(2,2),
                new Vector2(-2,2),
                new Vector2(-2,0),
                new Vector2(-2,-2),
               
            };
            for (int i = 0; i < velocities.Length; ++i)
            {
                MF_Particle p = CreateParticle();
                p.TextureName = "Textures/Particles/fire";
                p.Texture = textureDictionary[p.TextureName];
                p.Color = new Color(125, 108, 43);
                p.Position.X = rect.X + (rect.Width - 50) * (float)random.NextDouble();
                p.Position.Y = rect.Y + (rect.Height - 50) * (float)random.NextDouble();
                p.Alpha = 1.0f;
                p.AlphaRate = -2.0f;
                p.Life = 0.5f;
                p.Rotation = 0.0f;
                p.RotationRate = -2.0f + 4.0f * (float)random.NextDouble();
                p.Scale = 0.35f;
                p.ScaleRate = 0.2f;
                p.Velocity = velocities[i] * (float)random.NextDouble();
            }
            

        }
        /// <summary>
        /// Creates a particle, preferring to reuse a dead one in the particles list 
        /// before creating a new one.
        /// </summary>
        /// <returns></returns>
        MF_Particle CreateParticle()
        {
            MF_Particle p = null;

            for (int i = 0; i < particles.Count; ++i)
            {
                if (particles[i].Life <= 0.0f)
                {
                    p = particles[i];
                    break;
                }
            }

            if (p == null)
            {
                p = new MF_Particle();
                particles.Add(p);
            }

            p.Color = Color.White;

            return p;
        }
        /// <summary>
        /// This constructor should only be used by the XML serializer.
        /// </summary>
        public MF_ParticleSystem()
        {
            random = new Random();

            ResetParticles();
        }

        // Clears the currently displayed particles
        public void ResetParticles()
        {
            particles = new List<MF_Particle>();
        }

        public MF_ParticleSystem(ContentManager content, SpriteBatch spriteBatch)
        {
            random = new Random();
           
            ResetParticles();

            InitializeAssets(content, spriteBatch);
        }
        /// <summary>
        /// Initializes the particle system's assets. Call this method before attempting to use the particle system.
        /// </summary>
        /// <param name="content">The content manager to use for loading particle textures.</param>
        /// <param name="spriteBatch">The sprite batch to use for drawing the particles on screen.</param>
        public void InitializeAssets(ContentManager content, SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;

            textureDictionary = new Dictionary<string, Texture2D>();
            
        }
        Color[] fireColor;
        Color[] palette;
        int[,] fmap;
        bool Visible;
        Random rand;
        int minFuel, maxFuel, oxygen;
        GraphicsDevice GraphicsDevice;
        protected void BuildPalette()
        {
            for (int x = 0; x < palette.Length; x++)
                palette[x] = new Color(255, Math.Min(255, x * 2), x / 3, x * 5);
        }
        public void InitFire(GraphicsDevice GraphicsDevice, Point Resolution)
        {
            textureDictionary.Clear();
            this.GraphicsDevice = GraphicsDevice;
            rand = new Random(DateTime.Now.Millisecond);

            
            textureDictionary.Add("fire", new Texture2D(GraphicsDevice, Resolution.X, Resolution.Y));
            fireColor = new Color[textureDictionary["fire"].Width * textureDictionary["fire"].Height];
             textureDictionary.Add("fmp",new Texture2D(GraphicsDevice, Resolution.X, Resolution.Y));
            fmap = new int[textureDictionary["fire"].Width, textureDictionary["fire"].Height];
            textureDictionary.Add("paletteTexture", new Texture2D(GraphicsDevice, (int)Math.Sqrt(256), (int)Math.Sqrt(256)));
            palette = new Color[textureDictionary["paletteTexture"].Width * textureDictionary["paletteTexture"].Height];
            textureDictionary["paletteTexture"].SetData<Color>(palette);

            BuildPalette();


        }
        public void UpdateFire(GameTime gameTime)
        {
            if (!Visible)
                return;
            
            int w = fmap.GetLength(0);
            int h = fmap.GetLength(1);
            int[] fm = new int[w * h];
            for (int x = 0; x < w; x++)
                fmap[x, (h - 1)] = rand.Next(minFuel, maxFuel);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    fmap[x, y] = (int)(((fmap[(x - 1 + w) % w, (y + 1) % h]
                    + fmap[(x) % w, (y + 2) % h]
                    + fmap[(x + 1) % w, (y + 1) % h]
                    + fmap[(x) % w, (y + 2) % h])
                    * 1) / (4 + oxygen));
                    fm[x + (y * w)] = fmap[x, y];
                }
            }
            textureDictionary["fmp"].SetData<int>(fm);
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    fireColor[x + (y * w)] = palette[fmap[x, y]];
            textureDictionary["fire"].SetData<Color>(fireColor);
        }
        public void DrawFire(GameTime gameTime)
        {
            if (textureDictionary["fire"] != null)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                spriteBatch.Draw(textureDictionary["fire"], new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
                spriteBatch.End();
                // Debug
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
                spriteBatch.Draw(textureDictionary["paletteTexture"], new Rectangle(GraphicsDevice.PresentationParameters.BackBufferWidth - 128, 0, 128, 128), Color.White);
                spriteBatch.Draw(textureDictionary["fmp"], new Rectangle(GraphicsDevice.PresentationParameters.BackBufferWidth - 256, 0, 128, 128), Color.White);
                spriteBatch.End();
            }
        }
        /// <summary>
        /// Sets the textures for all particles according to the texture name in their TextureName field.
        /// </summary>
        public void SetParticleTextures()
        {
            foreach (MF_Particle particle in particles)
            {
                particle.Texture = textureDictionary[particle.TextureName];
            }
        }

        /// <summary>
        /// Update all active particles.
        /// </summary>
        /// <param name="elapsed">The amount of time elapsed since last Update.</param>
        public void Update(float elapsed)
        {
            for (int i = 0; i < particles.Count; ++i)
            {
                particles[i].Life -= elapsed;
                if (particles[i].Life <= 0.0f)
                {
                    continue;
                }
                particles[i].Position += particles[i].Velocity;// *elapsed;
                particles[i].Rotation += particles[i].RotationRate * elapsed;
                particles[i].Alpha += particles[i].AlphaRate * elapsed;
                particles[i].Scale += particles[i].ScaleRate * elapsed;

                if (particles[i].Alpha <= 0.0f)
                    particles[i].Alpha = 0.0f;
            }
        }
        /// <summary>
        /// Draws the particles.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < particles.Count; ++i)
            {
                MF_Particle p = particles[i];
                if (p.Life <= 0.0f)
                    continue;

                float alphaF = 255.0f * p.Alpha;
                if (alphaF < 0.0f)
                    alphaF = 0.0f;
                if (alphaF > 255.0f)
                    alphaF = 255.0f;

                spriteBatch.Draw(p.Texture, p.Position, null, new Color(p.Color.R, p.Color.G, p.Color.B, (byte)alphaF), p.Rotation, new Vector2(p.Texture.Width / 2, p.Texture.Height / 2), p.Scale, SpriteEffects.None, 0.0f);
            }
        }

        /// <summary>
        /// Draws the particles.
        /// </summary>
        public void Draw()
        {
            for (int i = 0; i < particles.Count; ++i)
            {
                MF_Particle p = particles[i];
                if (p.Life <= 0.0f)
                    continue;

                float alphaF = 255.0f * p.Alpha;
                if (alphaF < 0.0f)
                    alphaF = 0.0f;
                if (alphaF > 255.0f)
                    alphaF = 255.0f;

                spriteBatch.Draw(p.Texture, p.Position, null, new Color(p.Color.R, p.Color.G, p.Color.B, (byte)alphaF), p.Rotation, new Vector2(p.Texture.Width / 2, p.Texture.Height / 2), p.Scale, SpriteEffects.None, 0.0f);
            }
        }

    }
}
