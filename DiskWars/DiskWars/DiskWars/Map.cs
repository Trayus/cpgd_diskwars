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
    public class Map
    {
        public enum TILE { empty, floor, wall, spawnfloor, upgradefloor };
        public enum POWER { stealth, spike, speed, repel, none };
        public enum WALL { bounce, destr, pass, none };
        public class Tile
        {
            public TILE type;
            public Animation anim;
            public POWER power = POWER.none;
            public WALL wall = WALL.none;
            public float respawn = 0;
            public Tile(TILE type, float x, float y)
            {
                this.type = type;

                switch (type)
                {
                    case TILE.floor:
                        anim = Animation.createSingleFrameAnimation("tiles/testset_floor", new Vector2(x, y), 0.1f);
                        break;
                    case TILE.wall:
                        anim = Animation.createSingleFrameAnimation("tiles/testset_wall", new Vector2(x, y), 0.1f);
                        break;
                }
            }
            public Tile(TILE type, float x, float y, WALL wall)
            {
                this.type = type;
                this.wall = wall;

                switch (type)
                {
                    case TILE.floor:
                        anim = Animation.createSingleFrameAnimation("tiles/testset_floor", new Vector2(x, y), 0.1f);
                        break;
                    case TILE.wall:
                        switch (wall)
                        {
                            case WALL.bounce: anim = Animation.createSingleFrameAnimation("tiles/testset_bounce", new Vector2(x, y), 0.1f); break;
                            case WALL.destr: anim = Animation.createSingleFrameAnimation("tiles/testset_destructible1", new Vector2(x, y), 0.1f); break;
                            case WALL.pass: anim = Animation.createSingleFrameAnimation("tiles/testset_passthrough", new Vector2(x, y), 0.1f); break;
                            default: anim = Animation.createSingleFrameAnimation("tiles/testset_wall", new Vector2(x, y), 0.1f); break;
                        }
                        break;
                }
            }
        }
        public Tile[,] tiles;
        public Vector2[] spawns = new Vector2[4];

        public Map(String name, ContentManager content)
        {
            tiles = new Tile[Constants.MAPX, Constants.MAPY];
            parse(content.Load<Texture2D>(name));
        }

        private void parse(Texture2D map)
        {
            Color[] data = new Color[Constants.MAPX * Constants.MAPY];
            map.GetData<Color>(data);
            for (int j = 0; j < Constants.MAPY; j++)
            {
                for (int i = 0; i < Constants.MAPX; i++)
                {
                    Color temp = data[j * Constants.MAPX + i];

                    if (temp.R == 100 && temp.G == 100 && temp.B == 100)
                    {
                        tiles[i, j] = new Tile(TILE.floor, i * Constants.TILESIZE, j * Constants.TILESIZE);
                    }
                    else if (temp.R == 200 && temp.G == 200 && temp.B == 200)
                    {
                        tiles[i, j] = new Tile(TILE.wall, i * Constants.TILESIZE, j * Constants.TILESIZE);
                    }
                    else if (temp.R == 250 && temp.G == 250 && temp.B == 250)
                    {
                        tiles[i, j] = new Tile(TILE.wall, i * Constants.TILESIZE, j * Constants.TILESIZE);
                    }
                    else if (temp.R == 250 && temp.G == 0 && temp.B == 0)
                    {
                        spawns[0] = new Vector2(i * Constants.TILESIZE, j * Constants.TILESIZE);
                        tiles[i, j] = new Tile(TILE.floor, i * Constants.TILESIZE, j * Constants.TILESIZE);
                    }
                    else if (temp.R == 250 && temp.G == 250 && temp.B == 0)
                    {
                        spawns[1] = new Vector2(i * Constants.TILESIZE, j * Constants.TILESIZE);
                        tiles[i, j] = new Tile(TILE.floor, i * Constants.TILESIZE, j * Constants.TILESIZE);
                    }
                    else if (temp.R == 0 && temp.G == 250 && temp.B == 0)
                    {
                        spawns[2] = new Vector2(i * Constants.TILESIZE, j * Constants.TILESIZE);
                        tiles[i, j] = new Tile(TILE.floor, i * Constants.TILESIZE, j * Constants.TILESIZE);
                    }
                    else if (temp.R == 0 && temp.G == 0 && temp.B == 250)
                    {
                        spawns[3] = new Vector2(i * Constants.TILESIZE, j * Constants.TILESIZE);
                        tiles[i, j] = new Tile(TILE.floor, i * Constants.TILESIZE, j * Constants.TILESIZE);
                    }
                    else if (temp.G == 0 && temp.B == 250)
                    {
                        tiles[i, j] = new Tile(TILE.floor, i * Constants.TILESIZE, j * Constants.TILESIZE);
                        if (temp.R == 100)
                            tiles[i, j].power = POWER.speed;
                        if (temp.R == 150)
                            tiles[i, j].power = POWER.stealth;
                        if (temp.R == 200)
                            tiles[i, j].power = POWER.spike;
                        if (temp.R == 250)
                            tiles[i, j].power = POWER.repel;
                    }
                    else if (temp.G == 250 && temp.B == 250)
                    {
                        if (temp.R == 100)
                            tiles[i, j] = new Tile(TILE.wall, i * Constants.TILESIZE, j * Constants.TILESIZE, WALL.pass);
                        if (temp.R == 150)
                            tiles[i, j] = new Tile(TILE.wall, i * Constants.TILESIZE, j * Constants.TILESIZE, WALL.bounce);
                        if (temp.R == 200)
                            tiles[i, j] = new Tile(TILE.wall, i * Constants.TILESIZE, j * Constants.TILESIZE, WALL.destr);
                    }
                    else
                    {
                        tiles[i, j] = new Tile(TILE.empty, i * Constants.TILESIZE, j * Constants.TILESIZE);
                    }
                }
            }
        }


    }
}
