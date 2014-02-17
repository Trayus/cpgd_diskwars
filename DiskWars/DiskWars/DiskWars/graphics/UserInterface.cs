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
    /// A container for Animations and Strings to be displayed as the Interface for a rendering engine
    /// All text and animations in the UI are drawn in absolute screen coordinates
    /// </summary>
    class UserInterface
    {
        /// <summary>
        /// All the animations that are (visible or otherwise) part of the UI
        /// </summary>
        List<Animation> animations;
        /// <summary>
        /// All the text that is part of the UI
        /// </summary>
        List<HUDText> text;

        /// <summary>
        /// Creates a new User Interface 
        /// </summary>
        public UserInterface()
        {
            animations = new List<Animation>();
            text = new List<HUDText>();
        }

        /// <summary>
        /// Adds a given animation to the User Interface 
        /// Animations that are part of the UI are drawn in absolute screen coordinates
        /// </summary>
        /// <param name="anim">The anination to add to the UI</param>
        public void addAnimation(Animation anim)
        {
            animations.Add(anim);
            anim.setAsUI();
        }

        /// <summary>
        /// Removes a given animation from the UI and rendering engine
        /// </summary>
        /// <param name="anim">The animation to remove</param>
        public void removeAnimation(Animation anim)
        {
            anim.removeFromRenderingEngine();
            animations.Remove(anim);
        }

        /// <summary>
        /// Adds a HUDtext to the user interface
        /// </summary>
        /// <param name="text">The text to add</param>
        public void addText(HUDText text)
        {
            this.text.Add(text);
        }
        /// <summary>
        /// Removes a HUDtext from the user interface
        /// </summary>
        /// <param name="text">The text to remove</param>
        public void removeText(HUDText text)
        {
            this.text.Remove(text);
        }

        /// <summary>
        /// Returns all animations that are part of the UI
        /// </summary>
        /// <returns>all animations that are part of the UI</returns>
        public List<Animation> getUI()
        {
            return animations;
        }

        /// <summary>
        /// Draws all the HUDtext objects to the screen
        /// </summary>
        /// <param name="s">the spritebatch with which to draw</param>
        public void DrawText(SpriteBatch s, float scale)
        {
            foreach (HUDText t in text)
            {
                if (t.isEnabled())
                {
                    s.DrawString(t.getFont(), t.getText(), t.position * scale, t.getColor(), 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
                }
            }
        }

        /// <summary>
        /// Nukes all the Animations and texts in this UI
        /// </summary>
        public void removeAll()
        {
            foreach (Animation anim in animations)
                anim.removeFromRenderingEngine();
            text.Clear();
            animations.Clear();
        }
    }
}
