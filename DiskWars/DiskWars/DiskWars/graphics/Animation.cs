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
    /// An animation is the bread and butter of the rendering engine, as it contains many images in the form of a spritesheet
    /// along with data for collision detection and actual animation.
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// The name of the file on which the spritesheet is stored
        /// </summary>
        public String sheetname;
        /// <summary>
        /// The spritesheet associated with this animation
        /// </summary>
        SpriteSheet sheet;
        /// <summary>
        /// The current sequence and image in that sequence of the animation
        /// </summary>
        int currentSequence, currentimage;
        /// <summary>
        /// The position of the animation
        /// </summary>
        public Vector2 position;
        /// <summary>
        /// A timer to keep track of animation progress
        /// </summary>
        float timer = 0;
        /// <summary>
        /// A list for frames that may need to come after the current sequence is done
        /// </summary>
        List<int> followup = new List<int>();
        /// <summary>
        /// The layer depth and rotation of the image(s)
        /// </summary>
        float depth, rot = 0f, scale = 1f;
        /// <summary>
        /// Whether the animation is visible and if it's part of the HUD
        /// </summary>
        bool visibilty = true, isHud = false;
        /// <summary>
        /// image flipping, if needed
        /// </summary>
        SpriteEffects fx = SpriteEffects.None;
        /// <summary>
        /// The tint for the color map
        /// </summary>
        Color tint = Color.White;
        /// <summary>
        /// whether or not this is a code-generated single-frame animation
        /// </summary>
        private bool isSingleFrame = false;
        /// <summary>
        /// the color map and normal map for single-frame animations
        /// </summary>
        private Texture2D singlesheetcolor = null, singlesheetnormals = null;
        /// <summary>
        /// The source rectangle 
        /// </summary>
        private Rectangle frameRectangle = Rectangle.Empty;

        /// <summary>
        /// Creates a new animation with default location 0,0 and depth 0.5f 
        /// </summary>
        /// <param name="filename">The file to use as spritesheet</param>
        public Animation(String filename)
        {
            sheetname = filename;
            sheet = RenderingEngine.instance.addAnimation(this);
            currentSequence = 0;
            currentimage = 0;
            position = new Vector2(0, 0);
            depth = 0.5f;
        }

        /// <summary>
        /// Creates a new animation with default location 0,0
        /// </summary>
        /// <param name="filename">The file to use as spritesheet</param>
        /// <param name="layerDepth">The layer depth of the images</param>
        public Animation(String filename, float layerDepth)
        {
            sheetname = filename;
            sheet = RenderingEngine.instance.addAnimation(this);
            currentSequence = 0;
            currentimage = 0;
            position = new Vector2(0, 0);
            depth = layerDepth;
        }
        /// <summary>
        /// Creates a new animation with default depth 0.5f
        /// </summary>
        /// <param name="filename">The file to use as spritesheet</param>
        /// <param name="position">The position for the animation</param>
        public Animation(String filename, Vector2 position)
        {
            sheetname = filename;
            sheet = RenderingEngine.instance.addAnimation(this);
            currentSequence = 0;
            currentimage = 0;
            this.position = position;
            depth = 0.5f;
        }
        /// <summary>
        /// Creates a new animation
        /// </summary>
        /// <param name="filename">The file to use as spritesheet</param>
        /// <param name="position">The position for the animation</param>
        /// <param name="layerDepth">The layer depth of the images</param>
        public Animation(String filename, Vector2 position, float layerDepth)
        {
            sheetname = filename;
            sheet = RenderingEngine.instance.addAnimation(this);
            currentSequence = 0;
            currentimage = 0;
            this.position = position;
            depth = layerDepth;
        }

        /// <summary>
        /// creates a single-frame texture with same normal and color map
        /// </summary>
        /// <param name="texturename">the name of color and normal map</param>
        /// <param name="position">the position of the animation</param>
        /// <param name="single">place 'true' here :p</param>
        private Animation(String texturename, Vector2 position, bool single)
        {
            sheetname = null;
            isSingleFrame = true;
            RenderingEngine.instance.addAnimation(this, texturename, texturename);
            currentSequence = 0;
            currentimage = 0;
            this.position = position;
            depth = 0.5f;
        }

        /// <summary>
        /// creates a single-frame texture with a normal and color map
        /// </summary>
        /// <param name="texturename">the name of color map</param>
        /// <param name="normalname">the name of the normal map</param>
        /// <param name="position">the position of the animation</param>
        /// <param name="single">place 'true' here :p</param>
        private Animation(String texturename, String normalname, Vector2 position, bool single)
        {
            sheetname = null;
            isSingleFrame = true;
            RenderingEngine.instance.addAnimation(this, texturename, normalname);
            currentSequence = 0;
            currentimage = 0;
            this.position = position;
            depth = 0.5f;
        }

        /// <summary>
        /// creates a single-frame texture with same normal and color map
        /// </summary>
        /// <param name="texturename">the name of color and normal map</param>
        /// <param name="position">the position of the animation</param>
        /// <param name="layerdepth">the layer depth for drawing purposes</param>
        /// <param name="single">place 'true' here :p</param>
        private Animation(String texturename, Vector2 position, bool single, float layerdepth)
        {
            sheetname = null;
            isSingleFrame = true;
            RenderingEngine.instance.addAnimation(this, texturename, texturename);
            currentSequence = 0;
            currentimage = 0;
            this.position = position;
            depth = layerdepth;
        }

        /// <summary>
        /// creates a single-frame texture with a normal and color map
        /// </summary>
        /// <param name="texturename">the name of color map</param>
        /// <param name="normalname">the name of the normal map</param>
        /// <param name="position">the position of the animation</param>
        /// <param name="layerdepth">the layer depth for drawing purposes</param>
        /// <param name="single">place 'true' here :p</param>
        private Animation(String texturename, String normalname, Vector2 position, bool single, float layerdepth)
        {
            sheetname = null;
            isSingleFrame = true;
            RenderingEngine.instance.addAnimation(this, texturename, normalname);
            currentSequence = 0;
            currentimage = 0;
            this.position = position;
            depth = layerdepth;
        }

        /// <summary>
        /// creates a new animation, using a single Texture2D as both color and normal map
        /// </summary>
        /// <param name="texturename">the name for color map (and normal map)</param>
        /// <param name="position">the initial location of the animation</param>
        /// <returns>the new animation</returns>
        public static Animation createSingleFrameAnimation(String texturename, Vector2 position)
        {
            return new Animation(texturename, position, true);
        }
        /// <summary>
        /// creates a new animation, using a color and normal map
        /// </summary>
        /// <param name="texturename">the name for color map (and normal map)</param>
        /// <param name="position">the initial location of the animation</param>
        /// <returns>the new animation</returns>
        public static Animation createSingleFrameAnimation(String texturename, String normalname, Vector2 position)
        {
            return new Animation(texturename, normalname, position, true);
        }
        /// <summary>
        /// creates a new animation, using a single Texture2D as both color and normal map
        /// </summary>
        /// <param name="texturename">the name for color map (and normal map)</param>
        /// <param name="position">the initial location of the animation</param>
        /// <param name="layerdepth">the layer depth for drawing purposes</param>
        /// <returns>the new animation</returns>
        public static Animation createSingleFrameAnimation(String texturename, Vector2 position, float layerdepth)
        {
            return new Animation(texturename, position, true, layerdepth);
        }
        /// <summary>
        /// creates a new animation, using a color and normal map
        /// </summary>
        /// <param name="texturename">the name for color map (and normal map)</param>
        /// <param name="normalname">the name for color map (and normal map)</param>
        /// <param name="position">the initial location of the animation</param>
        /// <param name="layerdepth">the layer depth for drawing purposes</param>
        /// <returns>the new animation</returns>
        public static Animation createSingleFrameAnimation(String texturename, String normalname, Vector2 position, float layerdepth)
        {
            return new Animation(texturename, normalname, position, true, layerdepth);
        }

        /// <summary>
        /// Gets the source rectangle of the current image of the current sequence with respect to the full spritesheet
        /// </summary>
        /// <returns>The current source rectangle</returns>
        public Rectangle currentSourceRectangle()
        {
            if (isSingleFrame)
            {
                if (frameRectangle == Rectangle.Empty)
                {
                    return new Rectangle(0, 0, singlesheetcolor.Width, singlesheetcolor.Height);
                }
                else
                    return frameRectangle;
            }

            Rectangle temp = sheet.getBounds(currentSequence);
            temp.X += currentimage * temp.Width;
            if (frameRectangle != Rectangle.Empty)
            {
                temp.X += frameRectangle.X;
                temp.Y += frameRectangle.Y;
                temp.Width = temp.Width < frameRectangle.Width ? temp.Width : frameRectangle.Width;
                temp.Height = temp.Height < frameRectangle.Height ? temp.Height : frameRectangle.Height;
            }
            return temp;
        }
        /// <summary>
        /// Sets the source rectangle to use - for single frame animations only
        /// </summary>
        /// <param name="newsource">the new source rectangle</param>
        public void setFrameSourceRectangle(Rectangle newsource)
        {
                frameRectangle = newsource;
        }
        /// <summary>
        /// sets the image's scale
        /// </summary>
        /// <param name="scale">the new scale</param>
        public void setScale(float scale)
        {
            this.scale = scale;
        }
        /// <summary>
        /// returns the current scale
        /// </summary>
        /// <returns>the anim's scale</returns>
        public float getScale()
        {
            return scale;
        }

        /// <summary>
        /// Returns the full color map of the sprite sheet
        /// </summary>
        /// <returns>the color map</returns>
        public Texture2D spriteSheetImage()
        {
            if (isSingleFrame)
                return singlesheetcolor;
            return sheet.getImage();
        }
        /// <summary>
        /// Returns the full normal map of the sprite sheet
        /// </summary>
        /// <returns>the normal map</returns>
        public Texture2D spriteSheetNormals()
        {
            if (isSingleFrame)
                return singlesheetnormals;
            return sheet.getNormals();
        }

        /// <summary>
        /// sets the tinting color of the animation to the given color
        /// </summary>
        /// <param name="color">the new tint</param>
        public void setTint(Color color)
        {
            tint = color;
        }

        /// <summary>
        /// returns the tint of this animation's color map
        /// </summary>
        /// <returns>the current tint color</returns>
        public Color getTint()
        {
            return tint;
        }

        /// <summary>
        /// Set image-flip properties for the animation
        /// </summary>
        /// <param name="effect">the desired spriteeffect</param>
        public void setSpriteEffects(SpriteEffects effect)
        {
            fx = effect;
        }

        /// <summary>
        /// gives this animation's spriteeffects
        /// </summary>
        /// <returns>this animation's spriteeffects</returns>
        public SpriteEffects getSpriteEffects()
        {
            return fx;
        }

        /// <summary>
        /// Gets the total number of sequences in this animation
        /// </summary>
        /// <returns>the number of sequences</returns>
        public int numberOfSequences()
        {
            if (isSingleFrame)
                return 1;
            return sheet.numberOfSequences();
        }
        /// <summary>
        /// Returns the number of images in the specified sequence
        /// </summary>
        /// <param name="sequence">the sequence to check</param>
        /// <returns>the number of images in the sequence</returns>
        public int numberOfImagesInSequence(int sequence)
        {
            if (isSingleFrame)
                return 1;
            return sheet.numberOfImagesInSequence(sequence);
        }

        /// <summary>
        /// The width of the current sequence of animation
        /// </summary>
        /// <returns>The width of the image</returns>
        public int getWidth()
        {
            if (isSingleFrame)
                return (int)(singlesheetcolor.Width * scale);
            return (int)(sheet.getBounds(currentSequence).Width);
        }
        /// <summary>
        /// The height of the current sequence of animation
        /// </summary>
        /// <returns>the height of the image</returns>
        public int getHeight()
        {
            if (isSingleFrame)
                return (int)(singlesheetcolor.Height);
            return (int)(sheet.getBounds(currentSequence).Height);
        }
        /// <summary>
        /// returns the layer depth of the image(s)
        /// </summary>
        /// <returns>the layer depth</returns>
        public float getDepth()
        {
            return depth;
        }
        /// <summary>
        /// gets the rotation of the image(s)
        /// </summary>
        /// <returns>the rotation</returns>
        public float getRotation()
        {
            return rot;
        }
        /// <summary>
        /// Sets the rotation of the image(s)
        /// </summary>
        /// <param name="rotation">the new rotation</param>
        public void setRotation(float rotation)
        {
            rot = rotation;
        }

        /// <summary>
        /// Updates the animation (specifically the current image in the sequence)
        /// </summary>
        /// <param name="gametime">the time elapsed time since the last update (in milliseconds)</param>
        public void update(float gametime)
        {
            if (!isSingleFrame)
            {
                timer += gametime;

                if (timer > sheet.animationTimeOfSequence(currentSequence))
                {
                    timer %= sheet.animationTimeOfSequence(currentSequence);
                    if (followup.Count != 0 && currentimage == sheet.numberOfImagesInSequence(currentSequence) - 1)
                    {
                        currentSequence = followup[0];
                        followup.RemoveAt(0);
                        currentimage = 0;
                    }
                    else
                    {
                        currentimage++;
                        currentimage %= sheet.numberOfImagesInSequence(currentSequence);
                    }
                }
            }
        }

        /// <summary>
        /// Goes to the start specified sequence, without follow-up
        /// </summary>
        /// <param name="Sequence">the sequence to go to</param>
        public void goToSequence(int Sequence)
        {
            currentSequence = Sequence;
            timer = 0;
            currentimage = 0;
        }

        /// <summary>
        /// Goes to specified sequence without follow-up, but maintaining the current image number 
        /// </summary>
        /// <param name="Sequence">the sequence to go to</param>
        public void goToSequenceWithoutReset(int Sequence)
        {
            currentSequence = Sequence;
            currentimage %= sheet.numberOfImagesInSequence(Sequence);
        }

        /// <summary>
        /// Goes to the start specified sequence, following up with a single sequence (the animation will go that sequence once the current one is done)
        /// </summary>
        /// <param name="Sequence">the sequence to go to now</param>
        /// <param name="followupSequence">the sequence to do next</param>
        public void goToSequence(int Sequence, int followupSequence)
        {
            currentSequence = Sequence;
            timer = 0;
            followup.Clear();
            followup.Add(followupSequence);
            currentimage = 0;
        }

        /// <summary>
        /// Goes to a specified sequence, maintaining the current image number and following up with a single sequence (the animation will go that sequence once the current one is done)
        /// </summary>
        /// <param name="Sequence">the sequence to go to now</param>
        /// <param name="followupSequence">the sequence to do next</param>
        public void goToSequenceWithoutReset(int Sequence, int followupSequence)
        {
            currentSequence = Sequence;
            followup.Clear();
            followup.Add(followupSequence);
            currentimage %= sheet.numberOfImagesInSequence(Sequence);
        }


        /// <summary>
        /// Goes to the start of the specified sequence, following up with a list of sequences (the animation will go to those sequence once the current one is done)
        /// </summary>
        /// <param name="Sequence">the sequence to go to now</param>
        /// <param name="followupSequences">the sequences to do next</param>
        public void goToSequence(int Sequence, List<int> followupSequences)
        {
            currentSequence = Sequence;
            timer = 0;
            followup = followupSequences;
            currentimage = 0;
        }

        /// <summary>
        /// Goes to a specified sequence, maintaining the current image number and following up with a list of sequences (the animation will go to those sequence once the current one is done)
        /// </summary>
        /// <param name="Sequence">the sequence to go to now</param>
        /// <param name="followupSequences">the sequences to do next</param>
        public void goToSequenceWithoutReset(int sequence, List<int> followupSequences)
        {
            currentSequence = sequence;
            followup = followupSequences;
            currentimage %= sheet.numberOfImagesInSequence(sequence);
        }

        /// <summary>
        /// Gets the current sequence of this animation
        /// </summary>
        /// <returns>the current sequence</returns>
        public int getSequence()
        {
            return currentSequence;
        }
        /// <summary>
        /// Gets the current image frame number of the current sequence of this animation
        /// </summary>
        /// <returns>the current frame number</returns>
        public int getImageNumber()
        {
            return currentimage;
        }
        /// <summary>
        /// Checks to see if the current sequence is the given sequence
        /// </summary>
        /// <param name="sequenceToCheck">sequence to check against the current sequence</param>
        /// <returns>whether the current sequence is the given one</returns>
        public bool checkSequence(int sequenceToCheck)
        {
            return currentSequence == sequenceToCheck;
        }

        /// <summary>
        /// Rendering Engine will set the colors of this animation
        /// </summary>
        /// <param name="colors">the color sheet - only to be used by RenderingEngine</param>
        public void setSingleColorSheet(Texture2D colors)
        {
            singlesheetcolor = colors;
        }

        /// <summary>
        /// Rendering Engine will set the colors of this animation
        /// </summary>
        /// <param name="colors">the color sheet - only to be used by RenderingEngine</param>
        public void setSingleNormalsSheet(Texture2D normals)
        {
            singlesheetnormals = normals;
        }
        /// <summary>
        /// returns whether this animation was created in code
        /// </summary>
        /// <returns>whether it's a single frame animation</returns>
        public bool isSingleFrameAnimation()
        {
            return isSingleFrame;
        }

        /// <summary>
        /// Rudimentary collision detection between two animation
        /// </summary>
        /// <param name="other">The animation to check this one against</param>
        /// <returns>whether the two animation hit</returns>
        public bool checkHit(Animation other)
        {
            if (other == null) return false;

            float top1 = position.Y - getHeight() / 2, top2 = other.position.Y - other.getHeight() / 2;
            float bot1 = position.Y + getHeight() / 2, bot2 = other.position.Y + other.getHeight() / 2;
            float l1 = position.X - getWidth() / 2, l2 = other.position.X - other.getWidth() / 2;
            float r1 = position.X + getWidth() / 2, r2 = other.position.X + other.getWidth() / 2;

            if (top1 < bot2 && bot1 > top2 && l1 < r2 && r1 > l2)
                return true;

            return false;
        }

        /// <summary>
        /// gives the bounding box for this animation's current sequence
        /// </summary>
        /// <returns>the bounding rectangle</returns>
        public Rectangle getHitbox()
        {
            return new Rectangle((int)(position.X - getWidth() / 2), (int)(position.Y - getHeight() / 2), getWidth(), getHeight());
        }

        /// <summary>
        /// Sets whether this animation will be drawn
        /// </summary>
        /// <param name="value">should the animation be drawn?</param>
        public void setVisible(bool value)
        {
            visibilty = value;
        }
        /// <summary>
        /// Returns whether the animation is being drawn or not
        /// </summary>
        /// <returns>the visibility of the animation</returns>
        public bool isVisible()
        {
            return visibilty;
        }

        /// <summary>
        /// Checks to see if this animation is part of the user interface
        /// </summary>
        /// <returns>if it's part of the UI</returns>
        public bool isPartOfUI()
        {
            return isHud;
        }
        /// <summary>
        /// Indicate this animation is part of the user interface
        /// </summary>
        public void setAsUI()
        {
            isHud = true;
        }

        /// <summary>
        /// Removes the animation from the rendering engine
        /// </summary>
        public void removeFromRenderingEngine()
        {
            RenderingEngine.instance.removeAnimation(this);
        }
    }
}
