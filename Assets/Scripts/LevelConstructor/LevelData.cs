using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.LevelConstructor
{
    public class LevelData : ScriptableObject
    {
        [Tooltip("Номер уровня")]
        public int LevelNumber;

        [Tooltip("Позиция стартового тайла")]
        public Vector3Int StartTilePosition;

        [Tooltip("Позиция конечного тайла")]
        public Vector3Int FinishTilePosition;

        /// <summary>
        /// Тайлы пола
        /// </summary>
        public TilePosData[] Floor;

        /// <summary>
        /// Taйлы объектов, которые нельзя передвигать
        /// </summary>
        public TilePosData[] Obstacles;

        /// <summary>
        /// Тайлы активных объектов, которые можно передвигать
        /// </summary>
        public TilePosData[] MovableObjects;

        public void InitFloorData(Tilemap floorMap, TileBase startTile, TileBase finishTile)
        {
            var res = new List<TilePosData>();
            for (var x = floorMap.cellBounds.xMin; x <= floorMap.cellBounds.xMax; x++)
            {
                for (var y = floorMap.cellBounds.yMin; y <= floorMap.cellBounds.yMax; y++)
                {
                    var pos = new Vector3Int(x, y, 0);
                    var tile = floorMap.GetTile(pos);
                    if (tile == null) continue;
                    res.Add(new TilePosData
                    {
                        Tile = tile,
                        Position = pos
                    });
                    if (tile.name == startTile.name) //инициализация стартовой позиции
                    {
                        StartTilePosition = pos;
                    }
                    if (tile.name == finishTile.name) //инициализация конечной позиции
                    {
                        FinishTilePosition = pos;
                    }
                }
            }
            Floor = res.ToArray();
        }

        public void InitObstacles(Tilemap obstacles)
        {
            var res = new List<TilePosData>();
            for (var x = obstacles.cellBounds.xMin; x <= obstacles.cellBounds.xMax; x++)
            {
                for (var y = obstacles.cellBounds.yMin; y <= obstacles.cellBounds.yMax; y++)
                {
                    var pos = new Vector3Int(x, y, 0);
                    var tile = obstacles.GetTile(pos);
                    if (tile == null) continue;
                    res.Add(new TilePosData
                    {
                        Tile = tile,
                        Position = pos
                    });
                }
            }
            Obstacles = res.ToArray();
        }

        public void InitMovableObjects(Tilemap movableObjects)
        {
            var res = new List<TilePosData>();
            for (var x = movableObjects.cellBounds.xMin; x <= movableObjects.cellBounds.xMax; x++)
            {
                for (var y = movableObjects.cellBounds.yMin; y <= movableObjects.cellBounds.yMax; y++)
                {
                    var pos = new Vector3Int(x, y, 0);
                    var tile = movableObjects.GetTile(pos);
                    if (tile == null) continue;
                    res.Add(new TilePosData
                    {
                        Tile = tile,
                        Position = pos
                    });
                }
            }
            MovableObjects = res.ToArray();
        }

        /// <summary>
        /// Проверка на целостность данных
        /// </summary>
        /// <returns></returns>
        public bool IsDataOk()
        {
            //на уровне обязательно должен присутсвовать пол, и координата старта отличаться от координа финиша
            return Floor != null && StartTilePosition != FinishTilePosition;
        }
    }

    [Serializable]
    public struct TilePosData
    {
        public TileBase Tile;
        public Vector3Int Position;
    }
}
