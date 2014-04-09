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

using Graphics2D;
using Helpers;

namespace DiskWars
{
    public class PowerUp
    {
        public enum TYPE { pierce, big, speed, shield };

        public Animation animation;
        Light pULight;
        public float respawnTimer;
        public float activeTime;
        public bool active = false;
        public PowerUp.TYPE type;
        public Vector2 spawn;

        public bool enabled = true;
        public void enable()
        {
            animation.setVisible(true);
            pULight.setEnabled(true);
            enabled = true;
        }

        public void disable()
        {
            animation.setVisible(false);
            pULight.setEnabled(false);
            enabled = false;
        }


        public PowerUp(Vector2 spawn, PowerUp.TYPE type)
        {
            this.type = type;
            this.spawn = spawn;

            switch (type)
            {
                case TYPE.big:
                    animation = Animation.createSingleFrameAnimation("player/powerbig", spawn, 1.0f);
                    pULight = new Light(new Color(1f, 0.2f, 0.2f), animation.position, Constants.POWERUPLIGHTPOWER * 2, Constants.POWERUPLIGHTSIZE);
                    break;
                case TYPE.pierce:
                    animation = Animation.createSingleFrameAnimation("player/powerpierce", spawn, 1.0f);
                    pULight = new Light(new Color(1f, 1f, 0.2f), animation.position, Constants.POWERUPLIGHTPOWER * 2, Constants.POWERUPLIGHTSIZE);
                    break;
                case TYPE.shield:
                    animation = Animation.createSingleFrameAnimation("player/greenplayer", spawn, 1.0f);
                    pULight = new Light(new Color(0.2f, 1f, 0.2f), animation.position, Constants.POWERUPLIGHTPOWER * 2, Constants.POWERUPLIGHTSIZE);
                    break;
                case TYPE.speed:
                    animation = Animation.createSingleFrameAnimation("player/blueplayer", spawn, 1.0f);
                    pULight = new Light(new Color(0.2f, 0.2f, 1f), animation.position, Constants.POWERUPLIGHTPOWER * 2, Constants.POWERUPLIGHTSIZE);
                    break;
            }

            this.activeTime = Constants.POWERUPTIMER;
            this.respawnTimer = Constants.POWERUPRESPAWN;

            animation.setScale(Constants.POWERUPSCALE);
        }

        public PowerUp reset()
        {
            this.activeTime = Constants.POWERUPTIMER;
            return this;
        }

        public void update(float gameTime)
        {
            if (respawnTimer > 0 && !active)
                respawnTimer -= gameTime;

            if (!active && respawnTimer <= 0)
            {
                active = true;
                animation.setVisible(true);
            }

            if (active)
            {
                pULight.position = animation.position;
            }
        }
    }
}
