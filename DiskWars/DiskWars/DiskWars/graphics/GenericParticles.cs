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
    class GenericParticles
    {

        public static GenericParticles instance;
        private List<Particle> particles;

        /// <summary>
        /// You should be RenderingEngine to do this
        /// </summary>
        public GenericParticles()
        {
            GenericParticles.instance = this;
            particles = new List<Particle>();
        }

        public void update(float gametime)
        {
            foreach (Particle p in particles)
                p.update(gametime);

            for (int i = 0; i < particles.Count; i++)
                if (particles[i].time <= 0)
                {
                    particles[i].removeAnimation();
                    particles.RemoveAt(i);
                    i--;
                }
        }

        public static void createParticle(string texturefile, Vector2 position, Vector2 velocity, Vector2 acceleration, Color tint, float time, bool decay)
        {
            Animation anim = Animation.createSingleFrameAnimation(texturefile, position, 0.2f);
            anim.setTint(tint);
            GenericParticles.instance.particles.Add(new Particle(anim, time, velocity, acceleration, decay));
        }


        private class Particle
        {
            Animation anim;
            public float time, totaltime;
            Vector2 velocity, acceleration;
            public bool decay;

            public Particle(Animation anim, float time, Vector2 velocity, Vector2 acceleration, bool decay)
            {
                this.anim = anim;
                this.time = time;
                totaltime = time;
                this.velocity = velocity;
                this.acceleration = acceleration;
                this.decay = decay;
            }

            public void update(float gametime)
            {
                velocity += gametime * acceleration;
                anim.position += gametime * velocity;

                time -= gametime;

                if (decay)
                {
                    anim.setTint(anim.getTint() * (time / totaltime));
                }

            }

            public void removeAnimation()
            {
                anim.removeFromRenderingEngine();
            }

        }

    }
}
