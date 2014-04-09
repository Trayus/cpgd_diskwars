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
    class Disk
    {
        public Animation animation;
        public Vector2 velocity;
        Player player;
        public Light disklight;
        public bool stopped = false;
        public float diskVelocity;
        public int diskRadius;
        public bool diskPierce;

        public Disk(Player p, String name, Vector2 position, Color light)
        {
            player = p;
            animation = Animation.createSingleFrameAnimation(name, position, 0.85f);
            disklight = new Light(light, animation.position, Constants.DISKLIGHTPOWER * 2, Constants.DISKLIGHTSIZE);
            diskVelocity = Constants.DISKVELOCITY;
            diskRadius = Constants.DISKRADIUS;
            diskPierce = false;
        }

        public void setScale(float scale)
        {
            animation.setScale(scale);
        }
        public void setPosition(Vector2 pos)
        {
            animation.position = pos;
        }
        public void setVelocity(Vector2 vel)
        {
            vel.Normalize();
            velocity = vel;
        }
        public Vector2 getPosition()
        {
            return animation.position;
        }
        public void setRotation(float rot)
        {
            animation.setRotation(rot);
        }

        public void update(float gameTime)
        {
            //Console.Write("X: " + animation.position.X + "\n");
            //Console.Write("Y: " + animation.position.Y + "\n");
            if (!player.holdingDisk)
            {
                animation.position += velocity * gameTime * diskVelocity;
                if (Constants.WRAP)
                {
                    if (animation.position.X < 0)
                        animation.position.X += 1920;
                    if (animation.position.X > 1920)
                        animation.position.X -= 1920;
                    if (animation.position.Y < 0)
                        animation.position.Y += 1080;
                    if (animation.position.Y > 1080)
                        animation.position.Y -= 1080;
                }
            }

            disklight.position = animation.position;
        }

        public bool collide(Disk other)
        {
            if (Vector2.Distance(animation.position, other.animation.position) < 2 * diskRadius)
            {
                if (!this.stopped && !diskPierce)
                {
                    Vector2 axis = animation.position - other.animation.position;
                    axis.Normalize();
                    Vector3 midden = Vector3.Cross(new Vector3(axis, 0), Vector3.UnitZ);
                    Vector2 mid = new Vector2(midden.X, midden.Y);

                    float speed = this.velocity.Length();
                    this.velocity = this.velocity * Vector2.Dot(this.velocity, axis) + mid * Vector2.Dot(this.velocity, mid);
                    if (this.velocity.X != 0 && this.velocity.Y != 0)
                    {
                        this.velocity.Normalize();
                        this.velocity *= speed;
                    }
                }
                if (!other.stopped)
                {
                    Vector2 axis = other.animation.position - animation.position;
                    axis.Normalize();
                    Vector3 midden = Vector3.Cross(new Vector3(axis, 0), Vector3.UnitZ);
                    Vector2 mid = new Vector2(midden.X, midden.Y);

                    float speed = other.velocity.Length();
                    other.velocity = other.velocity * Vector2.Dot(other.velocity, axis) + mid * Vector2.Dot(other.velocity, mid);
                    if (other.velocity.X != 0 && other.velocity.Y != 0)
                    {
                        other.velocity.Normalize();
                        other.velocity *= speed;
                    }
                }
                return true;
            }
            return false;
        }

        public void retrieve(Vector2 to)
        {
            if (!Constants.BOUNCETOSLOW || !stopped)
            {
                Vector2 dir = to - animation.position;
                dir.Normalize();
                this.velocity = (this.velocity * 0.9f + dir * 0.1f);
            }
        }

        public void collisionDetectionVsMap(Map m)
        {
            if (player.holdingDisk)
                return;

            bool aa = false, ab = false, ac = false, ba = false, bc = false, ca = false, cb = false, cc = false;
            int ax = (int)((animation.position.X - diskRadius + Constants.TILESIZE / 2) / Constants.TILESIZE),
                ay = (int)((animation.position.Y - diskRadius + Constants.TILESIZE / 2) / Constants.TILESIZE);
            int bx = (int)((animation.position.X + Constants.TILESIZE / 2) / Constants.TILESIZE),
                by = (int)((animation.position.Y + Constants.TILESIZE / 2) / Constants.TILESIZE);
            int cx = (int)((animation.position.X + diskRadius + Constants.TILESIZE / 2) / Constants.TILESIZE),
                cy = (int)((animation.position.Y + diskRadius + Constants.TILESIZE / 2) / Constants.TILESIZE);

            if (ay >= 0 && ay < Constants.MAPY)
            {
                if (ax >= 0 && ax < Constants.MAPX) aa = m.tiles[ax, ay].type == Map.TILE.wall && m.tiles[ax, ay].wall != Map.WALL.pass;
                if (bx >= 0 && bx < Constants.MAPX) ab = m.tiles[bx, ay].type == Map.TILE.wall && m.tiles[bx, ay].wall != Map.WALL.pass;
                if (cx >= 0 && cx < Constants.MAPX) ac = m.tiles[cx, ay].type == Map.TILE.wall && m.tiles[cx, ay].wall != Map.WALL.pass;
            }
            if (by >= 0 && by < Constants.MAPY)
            {
                if (ax >= 0 && ax < Constants.MAPX) ba = m.tiles[ax, by].type == Map.TILE.wall && m.tiles[ax, by].wall != Map.WALL.pass;
                if (cx >= 0 && cx < Constants.MAPX) bc = m.tiles[cx, by].type == Map.TILE.wall && m.tiles[cx, by].wall != Map.WALL.pass;
            }
            if (cy >= 0 && cy < Constants.MAPY)
            {
                if (ax >= 0 && ax < Constants.MAPX) ca = m.tiles[ax, cy].type == Map.TILE.wall && m.tiles[ax, cy].wall != Map.WALL.pass;
                if (bx >= 0 && bx < Constants.MAPX) cb = m.tiles[bx, cy].type == Map.TILE.wall && m.tiles[bx, cy].wall != Map.WALL.pass;
                if (cx >= 0 && cx < Constants.MAPX) cc = m.tiles[cx, cy].type == Map.TILE.wall && m.tiles[cx, cy].wall != Map.WALL.pass;
            }

            if ((aa && ac) || ab)
            {
                animation.position.Y = ((ay + 1) * Constants.TILESIZE - Constants.TILESIZE / 2) + diskRadius;
                if (m.tiles[bx, ay].wall == Map.WALL.bounce)
                {
                    if (!Constants.BOUNCETOSLOW)
                    {
                        velocity.Y = 0;
                        velocity.X = 0;
                        player.released = true;
                    }
                    else
                    {
                        velocity.Y = 0;
                        velocity.X = 0;
                        stopped = true;
                    }
                }
                else
                    velocity.Y = -velocity.Y;
            }
            if ((ca && cc) || cb)
            {
                animation.position.Y = (cy * Constants.TILESIZE - Constants.TILESIZE / 2) - diskRadius;
                if (m.tiles[bx, cy].wall == Map.WALL.bounce)
                {
                    if (!Constants.BOUNCETOSLOW)
                    {
                        velocity.Y = 0;
                        velocity.X = 0;
                        player.released = true;
                    }
                    else
                    {
                        velocity.Y = 0;
                        velocity.X = 0;
                        stopped = true;
                    }
                }
                else
                    velocity.Y = -velocity.Y;
            }
            if ((aa && ca) || ba)
            {
                animation.position.X = ((ax + 1) * Constants.TILESIZE - Constants.TILESIZE / 2) + diskRadius;
                if (m.tiles[ax, by].wall == Map.WALL.bounce)
                {
                    if (!Constants.BOUNCETOSLOW)
                    {
                        velocity.Y = 0;
                        velocity.X = 0;
                        player.released = true;
                    }
                    else
                    {
                        velocity.Y = 0;
                        velocity.X = 0;
                        stopped = true;
                    }
                }
                else
                    velocity.X = -velocity.X;
            }
            if ((ac && cc) || bc)
            {
                animation.position.X = (cx * Constants.TILESIZE - Constants.TILESIZE / 2) - diskRadius;
                if (m.tiles[cx, by].wall == Map.WALL.bounce)
                {
                    if (!Constants.BOUNCETOSLOW)
                    {
                        velocity.Y = 0;
                        velocity.X = 0;
                        player.released = true;
                    }
                    else
                    {
                        velocity.Y = 0;
                        velocity.X = 0;
                        stopped = true;
                    }
                }
                else
                    velocity.X = -velocity.X;
            }

            if (ax >= 0 && ax < Constants.MAPX && ay >= 0 && ay < Constants.MAPY)
            {
                if (m.tiles[ax, ay].wall == Map.WALL.destr && m.tiles[ax, ay].respawn == 0)
                {
                    m.tiles[ax, ay].type = Map.TILE.floor;
                    m.tiles[ax, ay].anim.removeFromRenderingEngine();
                    m.tiles[ax, ay].anim = Animation.createSingleFrameAnimation("tiles/breakfloor",
                        new Vector2(m.tiles[ax, ay].anim.position.X, m.tiles[ax, ay].anim.position.Y), 0.1f);
                    m.tiles[ax, ay].respawn = Constants.DESTR_RESPAWN;
                }
                if (m.tiles[ax, by].wall == Map.WALL.destr && m.tiles[ax, by].respawn == 0)
                {
                    m.tiles[ax, by].type = Map.TILE.floor;
                    m.tiles[ax, by].anim.removeFromRenderingEngine();
                    m.tiles[ax, by].anim = Animation.createSingleFrameAnimation("tiles/breakfloor",
                        new Vector2(m.tiles[ax, by].anim.position.X, m.tiles[ax, by].anim.position.Y), 0.1f);
                    m.tiles[ax, by].respawn = Constants.DESTR_RESPAWN;
                }
                if (m.tiles[ax, cy].wall == Map.WALL.destr && m.tiles[ax, cy].respawn == 0)
                {
                    m.tiles[ax, cy].type = Map.TILE.floor;
                    m.tiles[ax, cy].anim.removeFromRenderingEngine();
                    m.tiles[ax, cy].anim = Animation.createSingleFrameAnimation("tiles/breakfloor",
                        new Vector2(m.tiles[ax, cy].anim.position.X, m.tiles[ax, cy].anim.position.Y), 0.1f);
                    m.tiles[ax, cy].respawn = Constants.DESTR_RESPAWN;
                }

                if (m.tiles[bx, ay].wall == Map.WALL.destr && m.tiles[bx, ay].respawn == 0)
                {
                    m.tiles[bx, ay].type = Map.TILE.floor;
                    m.tiles[bx, ay].anim.removeFromRenderingEngine();
                    m.tiles[bx, ay].anim = Animation.createSingleFrameAnimation("tiles/breakfloor",
                        new Vector2(m.tiles[bx, ay].anim.position.X, m.tiles[bx, ay].anim.position.Y), 0.1f);
                    m.tiles[bx, ay].respawn = Constants.DESTR_RESPAWN;
                }
                if (m.tiles[bx, by].wall == Map.WALL.destr && m.tiles[bx, by].respawn == 0)
                {
                    m.tiles[bx, by].type = Map.TILE.floor;
                    m.tiles[bx, by].anim.removeFromRenderingEngine();
                    m.tiles[bx, by].anim = Animation.createSingleFrameAnimation("tiles/breakfloor",
                        new Vector2(m.tiles[bx, by].anim.position.X, m.tiles[bx, by].anim.position.Y), 0.1f);
                    m.tiles[bx, by].respawn = Constants.DESTR_RESPAWN;
                }
                if (m.tiles[bx, cy].wall == Map.WALL.destr && m.tiles[bx, cy].respawn == 0)
                {
                    m.tiles[bx, cy].type = Map.TILE.floor;
                    m.tiles[bx, cy].anim.removeFromRenderingEngine();
                    m.tiles[bx, cy].anim = Animation.createSingleFrameAnimation("tiles/breakfloor",
                        new Vector2(m.tiles[bx, cy].anim.position.X, m.tiles[bx, cy].anim.position.Y), 0.1f);
                    m.tiles[bx, cy].respawn = Constants.DESTR_RESPAWN;
                }

                if (m.tiles[cx, ay].wall == Map.WALL.destr && m.tiles[cx, ay].respawn == 0)
                {
                    m.tiles[cx, ay].type = Map.TILE.floor;
                    m.tiles[cx, ay].anim.removeFromRenderingEngine();
                    m.tiles[cx, ay].anim = Animation.createSingleFrameAnimation("tiles/breakfloor",
                        new Vector2(m.tiles[cx, ay].anim.position.X, m.tiles[cx, ay].anim.position.Y), 0.1f);
                    m.tiles[cx, ay].respawn = Constants.DESTR_RESPAWN;
                }
                if (m.tiles[cx, by].wall == Map.WALL.destr && m.tiles[cx, by].respawn == 0)
                {
                    m.tiles[cx, by].type = Map.TILE.floor;
                    m.tiles[cx, by].anim.removeFromRenderingEngine();
                    m.tiles[cx, by].anim = Animation.createSingleFrameAnimation("tiles/breakfloor",
                        new Vector2(m.tiles[cx, by].anim.position.X, m.tiles[cx, by].anim.position.Y), 0.1f);
                    m.tiles[cx, by].respawn = Constants.DESTR_RESPAWN;
                }
                if (m.tiles[cx, cy].wall == Map.WALL.destr && m.tiles[cx, cy].respawn == 0)
                {
                    m.tiles[cx, cy].type = Map.TILE.floor;
                    m.tiles[cx, cy].anim.removeFromRenderingEngine();
                    m.tiles[cx, cy].anim = Animation.createSingleFrameAnimation("tiles/breakfloor",
                        new Vector2(m.tiles[cx, cy].anim.position.X, m.tiles[cx, cy].anim.position.Y), 0.1f);
                    m.tiles[cx, cy].respawn = Constants.DESTR_RESPAWN;
                }
            }
        }
    }
}
