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
        public class Tile
        {
            public TILE type;
            public Animation anim;
            public POWER power = POWER.none;
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
            List<Vector2> spawnList = new List<Vector2>();

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
                    else if (temp.R == 250 && temp.G == 0 && temp.B == 250)
                    {
                        spawnList.Add(new Vector2(i * Constants.TILESIZE, j * Constants.TILESIZE));
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
                    else
                    {
                        tiles[i, j] = new Tile(TILE.empty, i * Constants.TILESIZE, j * Constants.TILESIZE);
                        //Console.Write(temp.R+" "+temp.G+" "+temp.B+"\n" );
                    }
                }
            }

            //Add the List spawnList to the array spawns.
            Vector2[] tempSpawns = new Vector2[4 + spawnList.Count];
            for (int k = 0; k < 4; k++)
            {
                tempSpawns[k] = spawns[k];
            }

            for (int l = 0; l < spawnList.Count; l++)
            {
                tempSpawns[l + 4] = spawnList.ElementAt(l);
            }
            spawns = tempSpawns;
        }


    }
}
