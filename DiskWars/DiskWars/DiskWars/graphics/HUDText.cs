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
    /// A text object to be part of the User Interface
    /// </summary>
    public class HUDText
    {
        /// <summary>
        /// The actual text to be drawn
        /// </summary>
        String text;
        /// <summary>
        /// The font with which to draw the text
        /// </summary>
        SpriteFont font;
        /// <summary>
        /// Where to draw the text on the screen
        /// </summary>
        public Vector2 position;
        /// <summary>
        /// Whether or not the text should be drawn
        /// </summary>
        bool enabled;
        /// <summary>
        /// How much to scale the text
        /// </summary>
        float scale;
        /// <summary>
        /// The color with which to draw the text
        /// </summary>
        Color color;

        /// <summary>
        /// Creates a new HUDtext with default font and a scale of 1
        /// </summary>
        /// <param name="initialText">The actual text</param>
        /// <param name="position">where to draw the text</param>
        /// <param name="color">the color of the text</param>
        public HUDText(String initialText, Vector2 position, Color color)
        {
            text = initialText;
            font = RenderingEngine.instance.getDefaultFont();
            this.position = position;
            enabled = true;
            this.color = color;
            this.scale = 1f;
        }
        
        /// <summary>
        /// Creates a new HUDtext with a default scale of 1
        /// </summary>
        /// <param name="initialText">The actual text</param>
        /// <param name="position">where to draw the text</param>
        /// <param name="customfont">the custom font for the text</param>
        /// <param name="color">the color of the text</param>
        public HUDText(String initialText, Vector2 position, SpriteFont customfont, Color color)
        {
            text = initialText;
            font = customfont;
            this.position = position;
            enabled = true;
            this.color = color;
            this.scale = 1f;
        }

        /// <summary>
        /// Creates a new HUDtext with the default font
        /// </summary>
        /// <param name="initialText">The actual text</param>
        /// <param name="position">where to draw the text</param>
        /// <param name="color">the color of the text</param>
        /// <param name="scale">the scale with which to draw the text</param>
        public HUDText(String initialText, Vector2 position, Color color, float scale)
        {
            text = initialText;
            font = RenderingEngine.instance.getDefaultFont();
            this.position = position;
            enabled = true;
            this.color = color;
            this.scale = scale;
        }

        /// <summary>
        /// Creates a new HUDtext
        /// </summary>
        /// <param name="initialText">The actual text</param>
        /// <param name="position">where to draw the text</param>
        /// <param name="color">the color of the text</param>
        /// <param name="scale">the scale with which to draw the text</param>
        /// <param name="customfont">the custom font for the text</param>
        public HUDText(String initialText, Vector2 position, Color color, float scale, SpriteFont customfont)
        {
            text = initialText;
            font = customfont;
            this.position = position;
            enabled = true;
            this.color = color;
            this.scale = scale;
        }

        /// <summary>
        /// Gets the scale of the text (NOT the font size)
        /// </summary>
        /// <returns>the current scale</returns>
        public float getScale()
        {
            return scale;
        }
        /// <summary>
        /// Sets the scale of the text (NOT the font size)
        /// </summary>
        /// <param name="scale">the new scale</param>
        public void setScale(float scale)
        {
            this.scale = scale;
        }

        /// <summary>
        /// returns the current text 
        /// </summary>
        /// <returns>the current text</returns>
        public String getText()
        {
            return text;
        }
        /// <summary>
        /// Sets the text
        /// </summary>
        /// <param name="text">the new text</param>
        public void setText(String text)
        {
            this.text = text;
        }

        /// <summary>
        /// Gets the current color of the text
        /// </summary>
        /// <returns>the current color</returns>
        public Color getColor()
        {
            return color;
        }
        /// <summary>
        /// Sets the color of the text 
        /// </summary>
        /// <param name="color">the new color</param>
        public void setColor(Color color)
        {
            this.color = color;
        }

        /// <summary>
        /// Gets the current font of the text
        /// </summary>
        /// <returns>the font of the text</returns>
        public SpriteFont getFont()
        {
            return font;
        }
        /// <summary>
        /// Sets the font
        /// </summary>
        /// <param name="font">the new font</param>
        public void setFont(SpriteFont font)
        {
            this.font = font;
        }

        /// <summary>
        /// returns whether the text is currently being drawn
        /// </summary>
        /// <returns>if the text is being drawn</returns>
        public bool isEnabled()
        {
            return enabled;
        }
        /// <summary>
        /// Sets whether the text should be drawn
        /// </summary>
        /// <param name="on">should the text be drawn?</param>
        public void setEnabled(bool on)
        {
            enabled = on;
        }

    }
}
