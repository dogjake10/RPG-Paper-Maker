﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Paper_Maker
{
    [Serializable]
    public class Autotile
    {
        public int TilesId;


        // -------------------------------------------------------------------
        // CreateCopy
        // -------------------------------------------------------------------

        public Autotile CreateCopy()
        {
            Autotile newAutotile = new Autotile();
            newAutotile.TilesId = TilesId;

            return newAutotile;
        }

        // -------------------------------------------------------------------
        // Update
        // -------------------------------------------------------------------

        public void Update(Autotiles autotiles, int[] coords, int[] portion)
        {
            int[] tiles = new int[4];
            int num = 0;

            // Top left
            if (!autotiles.TileOnLeft(coords, portion) && !autotiles.TileOnTop(coords, portion)) num = 2;
            else if (!autotiles.TileOnTop(coords, portion) && autotiles.TileOnLeft(coords, portion)) num = 4;
            else if (!autotiles.TileOnLeft(coords, portion) && autotiles.TileOnTop(coords, portion)) num = 5;
            else if (autotiles.TileOnLeft(coords, portion) && autotiles.TileOnTop(coords, portion) && autotiles.TileOnTopLeft(coords, portion)) num = 3;
            else num = 1;
            tiles[0] = num - 1;

            // Top right
            if (!autotiles.TileOnRight(coords, portion) && !autotiles.TileOnTop(coords, portion)) num = 2;
            else if (!autotiles.TileOnTop(coords, portion) && autotiles.TileOnRight(coords, portion)) num = 4;
            else if (!autotiles.TileOnRight(coords, portion) && autotiles.TileOnTop(coords, portion)) num = 5;
            else if (autotiles.TileOnRight(coords, portion) && autotiles.TileOnTop(coords, portion) && autotiles.TileOnTopRight(coords, portion)) num = 3;
            else num = 1;
            tiles[1] = num - 1;

            // Bottom left
            if (!autotiles.TileOnLeft(coords, portion) && !autotiles.TileOnBottom(coords, portion)) num = 2;
            else if (!autotiles.TileOnBottom(coords, portion) && autotiles.TileOnLeft(coords, portion)) num = 4;
            else if (!autotiles.TileOnLeft(coords, portion) && autotiles.TileOnBottom(coords, portion)) num = 5;
            else if (autotiles.TileOnLeft(coords, portion) && autotiles.TileOnBottom(coords, portion) && autotiles.TileOnBottomLeft(coords, portion)) num = 3;
            else num = 1;
            tiles[2] = num - 1;

            // Bottom right
            if (!autotiles.TileOnRight(coords, portion) && !autotiles.TileOnBottom(coords, portion)) num = 2;
            else if (!autotiles.TileOnBottom(coords, portion) && autotiles.TileOnRight(coords, portion)) num = 4;
            else if (!autotiles.TileOnRight(coords, portion) && autotiles.TileOnBottom(coords, portion)) num = 5;
            else if (autotiles.TileOnRight(coords, portion) && autotiles.TileOnBottom(coords, portion) && autotiles.TileOnBottomRight(coords, portion)) num = 3;
            else num = 1;
            tiles[3] = num - 1;

            // Update tileId
            TilesId = (tiles[0] * 125) + (tiles[1] * 25) + (tiles[2] * 5) + tiles[3];

            // Update & save update
            int[] portionToUpdate = MapEditor.Control.GetPortion(coords[0], coords[3]);
            MapEditor.Control.AddPortionToUpdate(portionToUpdate);
            MapEditor.Control.AddPortionToSave(portionToUpdate);
            WANOK.AddPortionsToAddCancel(MapEditor.Control.Map.MapInfos.RealMapName, MapEditor.Control.GetGlobalPortion(portionToUpdate));
        }
    }
}
