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
    class Player
    {
        Animation animation;
        public Disk disk;
        Vector2 velocity = Vector2.Zero;
        Vector2[] spawn;
        Light playerlight;
        int num;
        public bool holdingDisk = true, released = false, reset = false;
        float diskTimer;
        float respawnTimer;
        public bool alive = true;
        public int score = 0;
        Random random;

        public bool enabled = true;
        public void enable()
        {
            animation.setVisible(true);
            playerlight.setEnabled(true);
            disk.animation.setVisible(true);
            disk.disklight.setEnabled(true);
            enabled = true;

            animation.position = getRandomSpawn();
            velocity = Vector2.Zero;
            alive = true;
            disk.setPosition(animation.position);
            holdingDisk = true;
        }
        public void disable()
        {
            animation.setVisible(false);
            playerlight.setEnabled(false);
            disk.animation.setVisible(false);
            disk.disklight.setEnabled(false);
            enabled = false;
        }

        public Player(Vector2[] spawn, int num)
        {
            this.num = num;
            this.spawn = spawn;
            switch (num)
            {
                case 1:
                    animation = Animation.createSingleFrameAnimation("player/redplayer", spawn[num-1], 1.0f);
                    disk = new Disk(this, "player/reddisk", spawn[num - 1] + new Vector2(0, 20), new Color(0.8f, 0.1f, 0.1f));
                    playerlight = new Light(new Color(1f, 0.2f, 0.2f), animation.position, Constants.PLAYERLIGHTPOWER * 2, Constants.PLAYERLIGHTSIZE);
                    break;
                case 2:
                    animation = Animation.createSingleFrameAnimation("player/yellowplayer", spawn[num - 1], 1.0f);
                    disk = new Disk(this, "player/yellowdisk", spawn[num - 1] + new Vector2(0, 20), new Color(0.8f, 0.8f, 0.1f));
                    playerlight = new Light(new Color(1f, 1f, 0.2f), animation.position, Constants.PLAYERLIGHTPOWER, Constants.PLAYERLIGHTSIZE);
                    break;
                case 3:
                    animation = Animation.createSingleFrameAnimation("player/greenplayer", spawn[num - 1], 1.0f);
                    disk = new Disk(this, "player/greendisk", spawn[num - 1] + new Vector2(0, 20), new Color(0.1f, 0.8f, 0.1f));
                    playerlight = new Light(new Color(0.2f, 1f, 0.2f), animation.position, Constants.PLAYERLIGHTPOWER, Constants.PLAYERLIGHTSIZE);
                    break;
                case 4:
                    animation = Animation.createSingleFrameAnimation("player/blueplayer", spawn[num - 1], 1.0f);
                    disk = new Disk(this, "player/bluedisk", spawn[num - 1] + new Vector2(0, 20), new Color(0.1f, 0.1f, 0.8f));
                    playerlight = new Light(new Color(0.2f, 0.2f, 1f), animation.position, Constants.PLAYERLIGHTPOWER * 2, Constants.PLAYERLIGHTSIZE);
                    break;
            }
            animation.setScale(Constants.PLAYERSCALE);
            disk.setScale(Constants.PLAYERSCALE);
            random = new Random(num);
        }

        public void kill()
        {
            animation.setVisible(false);
            disk.animation.setVisible(false);
            disk.disklight.setEnabled(false);
            respawnTimer = Constants.RESPAWN;
            alive = false;
            score += Constants.SCOREPERDEATH;
        }

        public bool check(Disk other)
        {
            if (Vector2.Distance(animation.position, other.animation.position) < Constants.DISKRADIUS + Constants.PLAYERRADIUS)
            {
                kill();
                return true;
            }
            return false;
        }
        
        public void update(float gameTime)
        {
            if (respawnTimer > 0)
                respawnTimer -= gameTime;

            if (!alive && respawnTimer <= 0)
            {
                animation.position = getRandomSpawn();
                velocity = Vector2.Zero;
                alive = true;
                animation.setVisible(true);
                disk.animation.setVisible(true);
                disk.disklight.setEnabled(true);
                disk.setPosition(animation.position);
                holdingDisk = true;
            }

            if (alive)
            {
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

                velocity.X += Input.LSTICKX(num) * gameTime * Constants.ACCELERATION;
                if (Math.Abs(velocity.X) > Constants.MAXVELOCITY) velocity.X = Constants.MAXVELOCITY * Math.Sign(velocity.X);
                velocity.Y -= Input.LSTICKY(num) * gameTime * Constants.ACCELERATION;
                if (Math.Abs(velocity.Y) > Constants.MAXVELOCITY) velocity.Y = Constants.MAXVELOCITY * Math.Sign(velocity.Y);

                if (Input.LSTICKX(num) < 0.1f && Input.LSTICKX(num) > -0.1f || velocity.X > 0 && Input.LSTICKX(num) > -0.1f || velocity.X < 0 && Input.LSTICKX(num) < -0.1f)
                    velocity.X /= Constants.SLIDE;
                if (Input.LSTICKY(num) < 0.1f && Input.LSTICKY(num) > -0.1f || velocity.Y < 0 && Input.LSTICKY(num) > -0.1f || velocity.Y > 0 && Input.LSTICKY(num) < -0.1f)
                    velocity.Y /= Constants.SLIDE;

                animation.position += velocity * gameTime * Constants.VELOCITY;

                playerlight.position = animation.position;
                
                if (Math.Abs(Input.RSTICKX(num)) > 0.05f || Math.Abs(Input.RSTICKY(num)) > 0.05f)
                {
                    Vector2 dir = new Vector2(Input.RSTICKX(num), Input.RSTICKY(num));
                    //dir.Normalize();

                    animation.setRotation((float)(Math.Atan2(-dir.Y, dir.X)) - (float)Math.PI / 2);
                }

                if (diskTimer > 0)
                    diskTimer -= gameTime;
                if (!holdingDisk && diskTimer <= 0 && (Vector2.Distance(disk.getPosition(), animation.position) < Constants.PLAYERRADIUS))
                {
                    holdingDisk = true;
                    disk.setVelocity(Vector2.Zero);
                }
                if (holdingDisk && !reset && (Input.RT(num) < 0.2f && Input.LT(num) < 0.2f))
                    reset = true;
                if ((Input.RT(num) > 0.2f || Input.LT(num) > 0.2f) && holdingDisk && reset)
                {
                    holdingDisk = false;
                    disk.setVelocity(new Vector2((float)Math.Cos(animation.getRotation() + (float)Math.PI / 2), (float)Math.Sin(animation.getRotation() + (float)Math.PI / 2)));
                    diskTimer = Constants.MINDISKTIME;
                    released = false;
                    reset = false;
                }
                if ((Input.RT(num) < 0.2f && Input.LT(num) < 0.2f) && !holdingDisk && diskTimer <= 0)
                    released = true;
                if (released)
                    disk.retrieve(animation.position);

                if (holdingDisk)
                {
                    disk.setPosition(animation.position + new Vector2((float)Math.Cos(animation.getRotation() + (float)Math.PI / 2),
                                                                     (float)Math.Sin(animation.getRotation() + (float)Math.PI / 2)) * 20);
                    disk.setRotation(animation.getRotation());
                }
            }
        }



        public void collisionDetectionVsMap(Map m)
        {
            if (!alive) return;

            bool aa = false, ab = false, ac = false, ba = false, bc = false, ca = false, cb = false, cc = false;
            int ax = (int)((animation.position.X - Constants.PLAYERRADIUS + Constants.TILESIZE / 2) / Constants.TILESIZE),
                ay = (int)((animation.position.Y - Constants.PLAYERRADIUS + Constants.TILESIZE / 2) / Constants.TILESIZE);
            int bx = (int)((animation.position.X + Constants.TILESIZE / 2) / Constants.TILESIZE),
                by = (int)((animation.position.Y + Constants.TILESIZE / 2) / Constants.TILESIZE);
            int cx = (int)((animation.position.X + Constants.PLAYERRADIUS + Constants.TILESIZE / 2) / Constants.TILESIZE),
                cy = (int)((animation.position.Y + Constants.PLAYERRADIUS + Constants.TILESIZE / 2) / Constants.TILESIZE);

            if (!(bx >= 0 && bx < Constants.MAPX && by >= 0 && by < Constants.MAPY) || 
                (bx >= 0 && bx < Constants.MAPX && by >= 0 && by < Constants.MAPY && m.tiles[bx, by].type == Map.TILE.empty))
            {
                kill();
                return;
            }

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
                animation.position.Y = ((ay + 1) * Constants.TILESIZE - Constants.TILESIZE / 2) + Constants.PLAYERRADIUS;
                velocity.Y = 0;
            }
            if ((ca && cc) || cb)
            {
                animation.position.Y = (cy * Constants.TILESIZE - Constants.TILESIZE / 2) - Constants.PLAYERRADIUS;
                velocity.Y = 0;
            } 
            if ((aa && ca) || ba)
            {
                animation.position.X = ((ax + 1) * Constants.TILESIZE - Constants.TILESIZE / 2) + Constants.PLAYERRADIUS;
                velocity.X = 0;
            }
            if ((ac && cc) || bc)
            {
                animation.position.X = (cx * Constants.TILESIZE - Constants.TILESIZE / 2) - Constants.PLAYERRADIUS;
                velocity.X = 0;
            }
        }

        public Vector2 getRandomSpawn()
        {
            int randomNumber = random.Next(0, spawn.Length);
            Console.Write(spawn.Length);
            return spawn[randomNumber];
        }
    }
}
