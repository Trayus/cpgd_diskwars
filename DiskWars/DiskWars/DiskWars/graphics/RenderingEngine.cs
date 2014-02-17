using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Graphics2D
{
    /// <summary>
    /// Handles all drawing of UI, animations, backgrounds and uses deferred shading (and lights)
    /// </summary>
    class RenderingEngine : DrawableGameComponent
    {
        /// <summary>
        /// The spritebatch for the game's drawing
        /// </summary>
        SpriteBatch spriteBatch;
        /// <summary>
        /// The single instance of rendering engine for the current game
        /// </summary>
        public static RenderingEngine instance;
        /// <summary>
        /// The camera used for rendering (position and scale)
        /// </summary>
        public static Camera camera;
        /// <summary>
        /// Animations and strings to be drawn in front of everything else
        /// </summary>
        public static UserInterface UI;
        /// <summary>
        /// A boolean representing whether the HUD should be drawns
        /// </summary>
        public static bool HUDactive = true;

        /// <summary>
        /// The default font for the rendering engine
        /// </summary>
        SpriteFont font;
        /// <summary>
        /// A list of all animations, enabled or otherwise, that may or may not need to be drawn
        /// </summary>
        List<Animation> animations;
        /// <summary>
        /// A list of all backgrounds used (they can be layered on top of each other)
        /// </summary>
        List<Background> bgs;
        /// A list of all static backgrounds used (they can be layered on top of each other)
        /// </summary>
        List<UniformBackground> staticbgs;
        /// <summary>
        /// A Dictionary to keep track of all the loaded spritesheets, so as to do less loading
        /// </summary>
        Dictionary<String, SpriteSheet> sheetmap;
        /// <summary>
        /// A Dictionary to keep track of all the loaded textures, so as to do less loading
        /// </summary>
        Dictionary<String, Texture2D> singletextures;
        /// <summary>
        /// Simple Particle System
        /// </summary>
        GenericParticles particles;
        /// <summary>
        /// The width and height of the screen
        /// </summary>
        int w, h;
        /// <summary>
        /// whether or not the Engine is active at the moment
        /// </summary>
        bool shouldrender = true;
        /// <summary>
        /// whether or not the lights are active at the moment
        /// </summary>
        public static bool useLights = true;


        /// <summary>
        /// The ambient light color to use when rendering deferredly
        /// </summary>
        public static Color ambientLight = new Color(.3f, .3f, .3f, 1);
        /// <summary>
        /// All the lights (enabled or otherwise) that would need to be taken into account when rendering
        /// </summary>
        private List<Light> lights = new List<Light>();
        /// <summary>
        /// The glossyness of the entire scene (how much light it reflects)
        /// </summary>
        private float specularStrength = 0.4f;

        // Shader stuff  for the next 20 lines
        private Effect lightEffect;
        private Effect lightCombinedEffect;

        private EffectTechnique lightEffectTechniquePointLight;
        private EffectTechnique lightEffectTechniqueConeLight;
        private EffectTechnique lightEffectTechniqueCircleLight;
        private EffectTechnique lightEffectTechniqueLineLight;
        private EffectParameter lightEffectParameterStrength;
        private EffectParameter lightEffectParameterPosition;
        private EffectParameter lightEffectParameterEndPosition;
        private EffectParameter lightEffectParameterConeDirection;
        private EffectParameter lightEffectParameterConeAngle;
        private EffectParameter lightEffectParameterLineRadius;
        private EffectParameter lightEffectParameterMinSize;
        private EffectParameter lightEffectParameterMaxSize;
        private EffectParameter lightEffectParameterHardEdge;
        private EffectParameter lightEffectParameterLightColor;
        private EffectParameter lightEffectParameterLightDecay;
        private EffectParameter lightEffectParameterScreenWidth;
        private EffectParameter lightEffectParameterScreenHeight;
        private EffectParameter lightEffectParameterNormapMap;

        private EffectTechnique lightCombinedEffectTechnique;
        private EffectParameter lightCombinedEffectParamAmbient;
        private EffectParameter lightCombinedEffectParamLightAmbient;
        private EffectParameter lightCombinedEffectParamAmbientColor;
        private EffectParameter lightCombinedEffectParamColorMap;
        private EffectParameter lightCombinedEffectParamShadowMap;
        private EffectParameter lightCombinedEffectParamNormalMap;

        /// <summary>
        /// An array to hold 4 vertices (texture coordinates) to render onto
        /// </summary>
        public VertexPositionColorTexture[] Vertices;
        /// <summary>
        /// A buffer to hold 4 vertices, corresponding with the corners of the screen
        /// </summary>
        public VertexBuffer VertexBuffer;

        /// <summary>
        /// Color: one of the 3 maps that is needed for deferred rendering
        /// </summary>
        private RenderTarget2D colorMapRenderTarget;
        /// <summary>
        /// Normals: one of the 3 maps that is needed for deferred rendering
        /// </summary>
        private RenderTarget2D normalMapRenderTarget;
        /// <summary>
        /// Shadows: one of the 3 maps that is needed for deferred rendering
        /// </summary>
        private RenderTarget2D shadowMapRenderTarget;

        /// <summary>
        /// A Rendering Engine tracks all Animations, Lights and Backgrounds them and draws them with deferred shading
        /// </summary>
        /// <param name="maingame">The game for which this Rendering Engine will serve</param>
        /// <param name="screenWidth">The initial width of the screen</param>
        /// <param name="screenHeight">The initial height of the screen</param>
        public RenderingEngine(Game maingame, int screenWidth, int screenHeight)
            : base(maingame)
        {
            instance = this;
            camera = new Camera();
            UI = new UserInterface();
            w = screenWidth;
            h = screenHeight;
        }
        /// <summary>
        /// returns a font
        /// </summary>
        /// <returns>The rendering engine's default font</returns>
        public SpriteFont getDefaultFont()
        {
            return font;
        }

        /// <summary>
        /// Updates the screen size; important for rendering purposes
        /// </summary>
        /// <param name="screenWidth">new screen width</param>
        /// <param name="screenHeight">new screen height</param>
        public void updateScreenSize(int screenWidth, int screenHeight)
        {
            w = screenWidth;
            h = screenHeight;            
        }

        /// <summary>
        /// Does nothing...
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// updates the rendering engine's stored width and height
        /// </summary>
        /// <param name="neww">new screen width</param>
        /// <param name="newh">new screen height</param>
        public void updateBounds(int neww, int newh)
        {
            w = neww;
            h = newh;

            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            SurfaceFormat format = pp.BackBufferFormat;

            colorMapRenderTarget = new RenderTarget2D(GraphicsDevice, w, h);
            normalMapRenderTarget = new RenderTarget2D(GraphicsDevice, w, h);
            shadowMapRenderTarget = new RenderTarget2D(GraphicsDevice, w, h, false, format, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
        }

        /// <summary>
        /// Loads the spritebatch, shaders and initializes the lists to be used for data storage
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            SurfaceFormat format = pp.BackBufferFormat;

            Vertices = new VertexPositionColorTexture[4];
            Vertices[0] = new VertexPositionColorTexture(new Vector3(-1, 1, 0), Color.White, new Vector2(0, 0));
            Vertices[1] = new VertexPositionColorTexture(new Vector3(1, 1, 0), Color.White, new Vector2(1, 0));
            Vertices[2] = new VertexPositionColorTexture(new Vector3(-1, -1, 0), Color.White, new Vector2(0, 1));
            Vertices[3] = new VertexPositionColorTexture(new Vector3(1, -1, 0), Color.White, new Vector2(1, 1));
            VertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColorTexture), Vertices.Length, BufferUsage.None);
            VertexBuffer.SetData(Vertices);

            colorMapRenderTarget = new RenderTarget2D(GraphicsDevice, w, h);
            normalMapRenderTarget = new RenderTarget2D(GraphicsDevice, w, h);
            shadowMapRenderTarget = new RenderTarget2D(GraphicsDevice, w, h, false, format, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);


            lightEffect = Game.Content.Load<Effect>("graphics/MultiTarget");
            lightCombinedEffect = Game.Content.Load<Effect>("graphics/DeferredCombined");

            lightEffectTechniquePointLight = lightEffect.Techniques["DeferredPointLight"];
            lightEffectTechniqueCircleLight = lightEffect.Techniques["DeferredCircleLight"];
            lightEffectTechniqueConeLight = lightEffect.Techniques["DeferredConeLight"];
            lightEffectTechniqueLineLight = lightEffect.Techniques["DeferredLineLight"];
            lightEffectParameterConeDirection = lightEffect.Parameters["coneDirection"];
            lightEffectParameterConeAngle = lightEffect.Parameters["coneAngle"];
            lightEffectParameterLineRadius = lightEffect.Parameters["radius"];
            lightEffectParameterMinSize = lightEffect.Parameters["minsize"];
            lightEffectParameterMaxSize = lightEffect.Parameters["maxsize"];
            lightEffectParameterHardEdge = lightEffect.Parameters["coneHardEdge"];
            lightEffectParameterLightColor = lightEffect.Parameters["lightColor"];
            lightEffectParameterLightDecay = lightEffect.Parameters["lightDecay"];
            lightEffectParameterNormapMap = lightEffect.Parameters["NormalMap"];
            lightEffectParameterPosition = lightEffect.Parameters["lightPosition"];
            lightEffectParameterEndPosition = lightEffect.Parameters["lightEndPosition"];
            lightEffectParameterScreenHeight = lightEffect.Parameters["screenHeight"];
            lightEffectParameterScreenWidth = lightEffect.Parameters["screenWidth"];
            lightEffectParameterStrength = lightEffect.Parameters["lightStrength"];

            lightCombinedEffectTechnique = lightCombinedEffect.Techniques["DeferredCombined2"];
            lightCombinedEffectParamAmbient = lightCombinedEffect.Parameters["ambient"];
            lightCombinedEffectParamLightAmbient = lightCombinedEffect.Parameters["lightAmbient"];
            lightCombinedEffectParamAmbientColor = lightCombinedEffect.Parameters["ambientColor"];
            lightCombinedEffectParamColorMap = lightCombinedEffect.Parameters["ColorMap"];
            lightCombinedEffectParamShadowMap = lightCombinedEffect.Parameters["ShadingMap"];
            lightCombinedEffectParamNormalMap = lightCombinedEffect.Parameters["NormalMap"];
            

            font = base.Game.Content.Load<SpriteFont>(@"graphics/font");

            animations = new List<Animation>();
            bgs = new List<Background>();
            staticbgs = new List<UniformBackground>();
            sheetmap = new Dictionary<string, SpriteSheet>();
            singletextures = new Dictionary<string, Texture2D>();
            particles = new GenericParticles();
        }

        /// <summary>
        /// Should this rendering engine be rendering anything?
        /// </summary>
        /// <returns>Whether the engine is active</returns>
        public static bool isActive()
        {
            return RenderingEngine.instance.shouldrender;
        }

        /// <summary>
        /// Sets whether the rendering engine should be doing anything
        /// </summary>
        /// <param name="active">whether the rendering engine should be active</param>
        public static void setActive(bool active)
        {
            RenderingEngine.instance.shouldrender = active;
        }

        /// <summary>
        /// Returns the textures asked for
        /// </summary>
        /// <param name="filename">The file name of the texture you want</param>
        /// <returns>The requested texture</returns>
        public Texture2D requestTexture(String filename)
        {
            // disabling normal maps
            if (filename.Contains("_normals"))
                return null;

            return base.Game.Content.Load<Texture2D>(filename);
        }

        /// <summary>
        /// Adds an animation to the rendering engine (so it can be rendered)
        /// </summary>
        /// <param name="anim">The animation to be added</param>
        /// <returns>The spritesheet the animation will need to use</returns>
        public SpriteSheet addAnimation(Animation anim)
        {
            animations.Add(anim);
            if (sheetmap.ContainsKey(anim.sheetname))
            {
                return sheetmap[anim.sheetname];
            }
            else
            {
                sheetmap[anim.sheetname] = new SpriteSheet(anim.sheetname);
                return sheetmap[anim.sheetname];
            }
        }

        /// <summary>
        /// Adds a single-frame animation to the rendering engine
        /// </summary>
        /// <param name="anim">the animation to add (should be single-frame)</param>
        /// <param name="colorname">the name of the color map file</param>
        /// <param name="normalname">the name of the normal map file</param>
        public void addAnimation(Animation anim, String colorname, String normalname)
        {
            animations.Add(anim);

            if (!singletextures.ContainsKey(colorname))
                singletextures[colorname] = requestTexture(colorname);
            if (!singletextures.ContainsKey(normalname))
                singletextures[normalname] = requestTexture(normalname);

            anim.setSingleColorSheet(singletextures[colorname]);
            anim.setSingleNormalsSheet(singletextures[normalname]);
        }

        /// <summary>
        /// Makes sure the provided animation exists in RenderingEngine (adds it if not)
        /// </summary>
        /// <param name="anim">the animation to be made sure is part of the engine</param>
        public void ensureAdded(Animation anim)
        {
            if (!animations.Contains(anim))
                animations.Add(anim);
        }
        /// <summary>
        /// Makes sure the provided light exists in RenderingEngine (adds it if not)
        /// </summary>
        /// <param name="anim">the light to be made sure is part of the engine</param>
        public void ensureAdded(Light light)
        {
            if (!lights.Contains(light))
                lights.Add(light);
        }

        /// <summary>
        /// Removes the given animation from the rendering engine
        /// </summary>
        /// <param name="item">The animation to be removed</param>
        public void removeAnimation(Animation item)
        {
           animations.Remove(item);
        }

        /// <summary>
        /// Removes all the animations from the Rendering Engine - a force to be reckoned with
        /// </summary>
        public void removeAllAnimations()
        {
            animations = new List<Animation>();
        }
        /// <summary>
        /// Removes all the lights from the Rendering Engine - a force to be reckoned with
        /// </summary>
        public void removeAllLights()
        {
            lights = new List<Light>();
        }

        

        /// <summary>
        /// Adds the given background to the rendering engine (so it can be rendered)
        /// </summary>
        /// <param name="bg">The background to be added</param>
        public void addBackground(Background bg)
        {
            bgs.Add(bg);
        }
        /// <summary>
        /// Adds the given background to the rendering engine (so it can be rendered)
        /// </summary>
        /// <param name="bg">The background to be added</param>
        public void addBackground(UniformBackground bg)
        {
            staticbgs.Add(bg);
        }

        /// <summary>
        /// Removes the given background from the rendering engine
        /// </summary>
        /// <param name="bg">the background to remove</param>
        public void removeBackground(Background bg)
        {
            bgs.Remove(bg);
        }/// <summary>
        /// Removes the given background from the rendering engine
        /// </summary>
        /// <param name="bg">the background to remove</param>
        public void removeBackground(UniformBackground bg)
        {
            staticbgs.Remove(bg);
        }
        /// <summary>
        /// Clears all backgrounds from the rendering engine
        /// </summary>
        public void removeAllBackgrounds()
        {
            bgs.Clear();
            staticbgs.Clear();
        }

        /// <summary>
        /// Adds the given light to the rendering engine, so it can be used in shading
        /// </summary>
        /// <param name="light">The light to add</param>
        public void addLight(Light light)
        {
            lights.Add(light);
        }
        /// <summary>
        /// Returns a light based on the string given (Lights can have strings to identify them, making 
        /// them usable in multiple different objects without passing references)
        /// </summary>
        /// <param name="lightId">The id of the light to find</param>
        /// <returns>The requested light (may be null if no light corresponds to the string)</returns>
        public Light findByID(String lightId)
        {
            foreach (Light l in lights)
            {
                if (l.getID().Equals(lightId))
                    return l;
            }
            return null;
        }
        /// <summary>
        /// Removes the given light from the rendering engine
        /// </summary>
        /// <param name="light">The light to remove</param>
        public void removeLight(Light light)
        {
            lights.Remove(light);
        }

        /// <summary>
        /// Updates the camera and the animations
        /// </summary>
        /// <param name="gameTime">the elapsed time since the last time update was called</param>
        public override void Update(GameTime gameTime)
        {
            if (shouldrender)
            {
                foreach (Animation a in animations)
                {
                    a.update(gameTime.ElapsedGameTime.Milliseconds);
                }
                camera.update(gameTime.ElapsedGameTime.Milliseconds);
                particles.update(gameTime.ElapsedGameTime.Milliseconds);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Checks to see if an animation is on screen. O(1) complexity, though not very efficient:
        /// checks 9 points in and around the animation to see if any of them are on screen
        /// </summary>
        /// <param name="anim">The animation to check</param>
        /// <returns>whether the animation is on screen</returns>
        private bool checkOnScreen(Animation anim)
        {
            Vector2 bb = (anim.position - camera.getRefPoint()) * camera.getScale() + new Vector2(w / 2, h / 2);
            if (!checkNearby(bb))
                return false;

            Vector2 aa = (anim.position - camera.getRefPoint() + new Vector2(anim.getWidth() / 2, anim.getHeight() / 2)) * camera.getScale() + new Vector2(w / 2, h / 2);
            Vector2 ab = (anim.position - camera.getRefPoint() + new Vector2(0, anim.getHeight() / 2)) * camera.getScale() + new Vector2(w / 2, h / 2);
            Vector2 ac = (anim.position - camera.getRefPoint() + new Vector2(-anim.getWidth() / 2, anim.getHeight())) * camera.getScale() + new Vector2(w / 2, h / 2);
            Vector2 ba = (anim.position - camera.getRefPoint() + new Vector2(anim.getWidth() / 2, 0)) * camera.getScale() + new Vector2(w / 2, h / 2);
            Vector2 bc = (anim.position - camera.getRefPoint() + new Vector2(-anim.getWidth() / 2, 0)) * camera.getScale() + new Vector2(w / 2, h / 2);
            Vector2 ca = (anim.position - camera.getRefPoint() + new Vector2(anim.getWidth() / 2, -anim.getHeight() / 2)) * camera.getScale() + new Vector2(w / 2, h / 2);
            Vector2 cb = (anim.position - camera.getRefPoint() + new Vector2(0, -anim.getHeight() / 2)) * camera.getScale() + new Vector2(w / 2, h / 2);
            Vector2 cc = (anim.position - camera.getRefPoint() + new Vector2(-anim.getWidth() / 2, -anim.getHeight() / 2)) * camera.getScale() + new Vector2(w / 2, h / 2);

            if (checkOnScreen(aa) || checkOnScreen(ab) || checkOnScreen(ac) ||
                checkOnScreen(ba) || checkOnScreen(bb) || checkOnScreen(bc) ||
                checkOnScreen(ca) || checkOnScreen(cb) || checkOnScreen(cc))
                return true;
            
            return false;
        }

        /// <summary>
        /// Checks if a given Vector2 is on screen (between 0-width and 0-height)
        /// </summary>
        /// <param name="sloc">the vector to check</param>
        /// <returns>whether the vector is on screen</returns>
        private bool checkOnScreen(Vector2 sloc)
        {
            if (sloc.X >= 0 && sloc.X <= w)
                if (sloc.Y >= 0 && sloc.Y <= h)
                    return true;

            return false;
        }

        /// <summary>
        /// Checks if a given Vector2 is close to or on the screen (between -width - 2*width and -height - 2*height)
        /// Use as a heuristic.
        /// </summary>
        /// <param name="sloc">The vector to check</param>
        /// <returns>whether the vector is close to the screen</returns>
        private bool checkNearby(Vector2 sloc)
        {
            if (sloc.X > -w && sloc.X < w * 2)
                if (sloc.Y > -h && sloc.Y < h * 2)
                    return true;

            return false;
        }

        /// <summary>
        /// Draws everything (uses helper methods for defered rendering -> color, normal and shadow maps are created)
        /// Note that this also draws the UI.
        /// </summary>
        /// <param name="gameTime">The time elapsed since the last update</param>
        public override void Draw(GameTime gameTime)
        {
            if (shouldrender)
            {
                GraphicsDevice.Clear(Color.Black);

                GraphicsDevice.SetRenderTarget(colorMapRenderTarget);
                GraphicsDevice.Clear(Color.Transparent);
                DrawColorMap();
                GraphicsDevice.SetRenderTarget(null);


                GraphicsDevice.SetRenderTarget(normalMapRenderTarget);
                GraphicsDevice.Clear(new Color(0.5f, 0.5f, 0.5f));
                //DrawNormalMap();
                GraphicsDevice.SetRenderTarget(null);

                GenerateShadowMap();
                GraphicsDevice.Clear(Color.Black);
                DrawCombinedMaps();

                if (RenderingEngine.HUDactive)
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    List<Animation> ui = UI.getUI();
                    foreach (Animation anim in ui)
                    {
                        if (anim.isVisible())
                        {
                            spriteBatch.Draw(anim.spriteSheetImage(), (anim.position) * camera.getScale(),
                                    anim.currentSourceRectangle(), Color.White, 0f, Vector2.Zero,
                                    camera.getScale(), SpriteEffects.None, 1f);
                        }
                    }
                    UI.DrawText(spriteBatch, camera.getScale());
                    spriteBatch.End();
                }
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws the color, normal and shadow maps to the screen together: in essence this completes the deferred render
        /// </summary>
        private void DrawCombinedMaps()
        {
            lightCombinedEffect.CurrentTechnique = lightCombinedEffectTechnique;
            lightCombinedEffectParamAmbient.SetValue(1f);
            lightCombinedEffectParamLightAmbient.SetValue(4);
            lightCombinedEffectParamAmbientColor.SetValue(ambientLight.ToVector4());
            lightCombinedEffectParamColorMap.SetValue(colorMapRenderTarget);
            lightCombinedEffectParamShadowMap.SetValue(shadowMapRenderTarget);
            lightCombinedEffectParamNormalMap.SetValue(normalMapRenderTarget);
            lightCombinedEffect.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, lightCombinedEffect);
            spriteBatch.Draw(colorMapRenderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();

        }

        /// <summary>
        /// Draws the color map of all Animations (non-UI) and Backgrounds
        /// </summary>
        private void DrawColorMap()
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            foreach (UniformBackground bg in staticbgs)
            {
                bg.Draw(spriteBatch, camera.getRefPoint(), camera.getScale(), new Vector2(w, h));
            } 
            foreach (Background bg in bgs)
            {
                bg.Draw(spriteBatch, camera.getRefPoint(), camera.getScale(), new Vector2(w, h));
            }

            foreach (Animation a in animations)
            {
                if (a.isVisible() && !a.isPartOfUI())
                {
                    spriteBatch.Draw(a.spriteSheetImage(), (a.position - camera.getRefPoint()) * camera.getScale() + new Vector2(w / 2, h / 2),
                        a.currentSourceRectangle(), a.getTint(), a.getRotation(),
                        new Vector2(a.currentSourceRectangle().Width / 2, a.currentSourceRectangle().Height / 2),
                        camera.getScale() * a.getScale(), a.getSpriteEffects(), a.getDepth());
                }
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Draws the normal map of all Animations (non-UI) and Backgrounds
        /// </summary>
        private void DrawNormalMap()
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            
            foreach (UniformBackground bg in staticbgs)
            {
                bg.DrawNormals(spriteBatch, camera.getRefPoint(), camera.getScale(), new Vector2(w, h));
            }
            foreach (Background bg in bgs)
            {
                bg.DrawNormals(spriteBatch, camera.getRefPoint(), camera.getScale(), new Vector2(w, h));
            }

            foreach (Animation a in animations)
            {
                if (a.isVisible() && !a.isPartOfUI() /*&& checkOnScreen(a)*/)
                {
                    spriteBatch.Draw(a.spriteSheetNormals(), (a.position - camera.getRefPoint()) * camera.getScale() + new Vector2(w / 2, h / 2),
                        a.currentSourceRectangle(), Color.White, a.getRotation(),
                        new Vector2(a.currentSourceRectangle().Width / 2, a.currentSourceRectangle().Height / 2),
                        camera.getScale() * a.getScale(), a.getSpriteEffects(), a.getDepth());
                }
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Creates the shadow map for the normal and color maps drawn
        /// </summary>
        private Texture2D GenerateShadowMap()
        {
            GraphicsDevice.SetRenderTarget(shadowMapRenderTarget);
            GraphicsDevice.Clear(Color.Transparent);

            if (useLights)
            {
                foreach (var light in lights)
                {
                    if (!light.isEnabled()) continue;
                    Vector2 pos = (light.position - camera.getRefPoint()) * camera.getScale() + new Vector2(w / 2, h / 2);
                    if (!checkNearby(pos)) continue;

                    GraphicsDevice.SetVertexBuffer(VertexBuffer);

                    // Draw all the light sources
                    lightEffectParameterStrength.SetValue(light.power);
                    lightEffectParameterPosition.SetValue(new Vector3(pos, 0));
                    lightEffectParameterLightColor.SetValue(light.getColor());
                    lightEffectParameterLightDecay.SetValue(light.size * camera.getScale()); // Value between 0.00 and 2.00   
                    lightEffect.Parameters["specularStrength"].SetValue(specularStrength);

                    if (light is ConeLight)
                    {
                        lightEffect.CurrentTechnique = lightEffectTechniqueConeLight;
                        lightEffectParameterConeDirection.SetValue(new Vector3(((ConeLight)light).direction, 0));
                        lightEffectParameterConeAngle.SetValue(((ConeLight)light).angle);
                        if (((ConeLight)light).hardedge)
                        {
                            lightEffectParameterHardEdge.SetValue(1);
                        }
                        else
                        {
                            lightEffectParameterHardEdge.SetValue(0);
                        }
                    }
                    else if (light is CircleLight)
                    {
                        lightEffect.CurrentTechnique = lightEffectTechniqueCircleLight;
                        lightEffectParameterMinSize.SetValue(((CircleLight)light).minsize);
                        lightEffectParameterMaxSize.SetValue(((CircleLight)light).maxsize);
                        if (((CircleLight)light).hardedge)
                        {
                            lightEffectParameterHardEdge.SetValue(1);
                        }
                        else
                        {
                            lightEffectParameterHardEdge.SetValue(0);
                        }
                    }
                    else if (light is LineLight)
                    {
                        lightEffect.CurrentTechnique = lightEffectTechniqueLineLight;
                        Vector2 endpos = ((((LineLight)light).endpoint + light.position) - camera.getRefPoint()) * camera.getScale() + new Vector2(w / 2, h / 2);
                        lightEffectParameterEndPosition.SetValue(new Vector3(endpos, 0));
                        lightEffectParameterLineRadius.SetValue(((LineLight)light).radius);

                        if (((LineLight)light).hardedge)
                        {
                            lightEffectParameterHardEdge.SetValue(1);
                        }
                        else
                        {
                            lightEffectParameterHardEdge.SetValue(0);
                        }
                    }
                    else
                    {
                        lightEffect.CurrentTechnique = lightEffectTechniquePointLight;
                    }

                    lightEffectParameterScreenWidth.SetValue(GraphicsDevice.Viewport.Width);
                    lightEffectParameterScreenHeight.SetValue(GraphicsDevice.Viewport.Height);
                    lightEffect.Parameters["ambientColor"].SetValue(ambientLight.ToVector4());
                    lightEffectParameterNormapMap.SetValue(normalMapRenderTarget);
                    lightEffect.Parameters["ColorMap"].SetValue(colorMapRenderTarget);
                    lightEffect.CurrentTechnique.Passes[0].Apply();

                    // Add Belding (Black background)
                    GraphicsDevice.BlendState = BlendBlack;

                    // Draw some magic
                    GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, Vertices, 0, 2);
                }
            }

            // Deactive the rander targets to resolve them
            GraphicsDevice.SetRenderTarget(null);

            return shadowMapRenderTarget;
        }


        /// <summary>
        /// Special Blendstate (spritebatch parameter) for deferred rendering
        /// </summary>
        public static BlendState BlendBlack = new BlendState()
        {
            ColorBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.One,

            AlphaBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.One
        };



    }
}
