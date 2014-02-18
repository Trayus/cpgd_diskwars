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


        public Disk(Player p, String name, Vector2 position, Color light)
        {
            player = p;
            animation = Animation.createSingleFrameAnimation(name, position, 0.9f);
            disklight = new Light(light, animation.position, Constants.DISKLIGHTPOWER * 2, Constants.DISKLIGHTSIZE);
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
            if (!player.holdingDisk)
            {
                animation.position += velocity * gameTime * Constants.DISKVELOCITY;
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
            if (Vector2.Distance(animation.position, other.animation.position) < 2 * Constants.DISKRADIUS)
            {
                {
                    Vector2 axis = animation.position - other.animation.position;
                    axis.Normalize();
                    Vector3 midden = Vector3.Cross(new Vector3(axis, 0), Vector3.UnitZ);
                    Vector2 mid = new Vector2(midden.X, midden.Y);

                    float speed = this.velocity.Length();
                    this.velocity = this.velocity * Vector2.Dot(this.velocity, axis) + mid * Vector2.Dot(this.velocity, mid);
                    this.velocity.Normalize();
                    this.velocity *= speed;
                }
                {
                    Vector2 axis = other.animation.position - animation.position;
                    axis.Normalize();
                    Vector3 midden = Vector3.Cross(new Vector3(axis, 0), Vector3.UnitZ);
                    Vector2 mid = new Vector2(midden.X, midden.Y);

                    float speed = other.velocity.Length();
                    other.velocity = other.velocity * Vector2.Dot(other.velocity, axis) + mid * Vector2.Dot(other.velocity, mid);
                    other.velocity.Normalize();
                    other.velocity *= speed;
                }
                return true;
            }
            return false;
        }

        public void retrieve(Vector2 to)
        {
            Vector2 dir = to - animation.position;
            dir.Normalize();
            this.velocity = (this.velocity * 0.9f + dir * 0.1f);
        }

        public void collisionDetectionVsMap(Map m)
        {
            if (player.holdingDisk)
                return;

            bool aa = false, ab = false, ac = false, ba = false, bc = false, ca = false, cb = false, cc = false;
            int ax = (int)((animation.position.X - Constants.DISKRADIUS + Constants.TILESIZE / 2) / Constants.TILESIZE),
                ay = (int)((animation.position.Y - Constants.DISKRADIUS + Constants.TILESIZE / 2) / Constants.TILESIZE);
            int bx = (int)((animation.position.X + Constants.TILESIZE / 2) / Constants.TILESIZE),
                by = (int)((animation.position.Y + Constants.TILESIZE / 2) / Constants.TILESIZE);
            int cx = (int)((animation.position.X + Constants.DISKRADIUS + Constants.TILESIZE / 2) / Constants.TILESIZE),
                cy = (int)((animation.position.Y + Constants.DISKRADIUS + Constants.TILESIZE / 2) / Constants.TILESIZE);

            if (ay >= 0 && ay < Constants.MAPY)
            {
                if (ax >= 0 && ax < Constants.MAPX) aa = m.tiles[ax, ay].type == Map.TILE.wall;
                if (bx >= 0 && bx < Constants.MAPX) ab = m.tiles[bx, ay].type == Map.TILE.wall;
                if (cx >= 0 && cx < Constants.MAPX) ac = m.tiles[cx, ay].type == Map.TILE.wall;
            }
            if (by >= 0 && by < Constants.MAPY)
            {
                if (ax >= 0 && ax < Constants.MAPX) ba = m.tiles[ax, by].type == Map.TILE.wall;
                if (cx >= 0 && cx < Constants.MAPX) bc = m.tiles[cx, by].type == Map.TILE.wall;
            }
            if (cy >= 0 && cy < Constants.MAPY)
            {
                if (ax >= 0 && ax < Constants.MAPX) ca = m.tiles[ax, cy].type == Map.TILE.wall;
                if (bx >= 0 && bx < Constants.MAPX) cb = m.tiles[bx, cy].type == Map.TILE.wall;
                if (cx >= 0 && cx < Constants.MAPX) cc = m.tiles[cx, cy].type == Map.TILE.wall;
            }

            if ((aa && ac) || ab)
            {
                animation.position.Y = ((ay + 1) * Constants.TILESIZE - Constants.TILESIZE / 2) + Constants.DISKRADIUS;
                velocity.Y = -velocity.Y;
            }
            if ((ca && cc) || cb)
            {
                animation.position.Y = (cy * Constants.TILESIZE - Constants.TILESIZE / 2) - Constants.DISKRADIUS;
                velocity.Y = -velocity.Y;
            }
            if ((aa && ca) || ba)
            {
                animation.position.X = ((ax + 1) * Constants.TILESIZE - Constants.TILESIZE / 2) + Constants.DISKRADIUS;
                velocity.X = -velocity.X;
            }
            if ((ac && cc) || bc)
            {
                animation.position.X = (cx * Constants.TILESIZE - Constants.TILESIZE / 2) - Constants.DISKRADIUS;
                velocity.X = -velocity.X;
            }
        }
    }
}
