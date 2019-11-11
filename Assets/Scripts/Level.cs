using System;
using Assets.Scripts.LevelConstructor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public class Level : MonoBehaviour
    {
        public int LevelNumber;

        [Tooltip("Тайлмап пола, а также тайлы начала и конца уровня")]
        public Tilemap Floor;

        [Tooltip("Тайлмап препядствий, которые нельзя передвигать")]
        public Tilemap Obstacles;

        [Tooltip("Тайлмап активных объектов, которые можно передвигать")]
        public Tilemap MovableObjects;

        private LevelData _data;

        public Vector3 PlayerStartPosition => Floor.CellToWorld(_data.StartTilePosition);

        /// <summary>
        /// Загрузка уровня
        /// </summary>
        /// <param name="data"></param>
        public void LoadLevel(LevelData data)
        {
            ClearLevel();
            _data = data;
            LevelNumber = data.LevelNumber;

            if (data.Floor == null) throw new Exception("Something goes wrong. Level can`t be without floor");
            foreach (var tilePosData in data.Floor)
            {
                Floor.SetTile(tilePosData.Position, tilePosData.Tile);
            }

            //загрузка препятствий 
            foreach (var tilePosData in data.Obstacles)
            {
                Obstacles.SetTile(tilePosData.Position, tilePosData.Tile);
            }

            //загрузка активных объектов, которые можно передвигать
            foreach (var tilePosData in data.MovableObjects)
            {
                MovableObjects.SetTile(tilePosData.Position, tilePosData.Tile);
            }
        }

        public void ClearLevel()
        {
            Floor.ClearAllTiles();
            Obstacles.ClearAllTiles();
            MovableObjects.ClearAllTiles();
        }

        public void RestartLevel()
        {
            LoadLevel(_data);
        }

        /// <summary>
        /// Следующай позиция игрока при меремещении
        /// </summary>
        /// <param name="playerPos"></param>
        /// <param name="direction"></param>
        /// <returns>позиция игрока после перемещения в мировых координатах</returns>
        public Vector3 NextPlayerPosition(Vector3 playerPos, Vector3Int direction)
        {
            var playerCellPos = Floor.WorldToCell(playerPos);
            var playerNextPosition = playerCellPos + direction;
            if (Floor.GetTile(playerNextPosition) != null) //является ли следующая клетка полом
            {
                //не уперлись ли мы в препятсвие
                if (Obstacles.GetTile(playerNextPosition) == null)
                {
                    if (TryMoveActiveObject(playerNextPosition, direction))
                    {
                        playerCellPos = playerNextPosition;
                    }
                }
            }
            return Floor.CellToWorld(playerCellPos);
        }

        /// <summary>
        /// Пытаемся переместить объект, если он встречается на пути
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool TryMoveActiveObject(Vector3Int position, Vector3Int direction)
        {
            var activeObjectTile = MovableObjects.GetTile(position);
            if (activeObjectTile == null) return true;

            var nextPos = position + direction;
            if (Floor.GetTile(nextPos) != null && Obstacles.GetTile(nextPos) == null &&
                MovableObjects.GetTile(nextPos) == null)
            {
                MovableObjects.SetTile(position, null);
                MovableObjects.SetTile(nextPos, activeObjectTile);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Финишная ли данная позиция
        /// </summary>
        /// <param name="position">in woorlds coords</param>
        /// <returns></returns>
        public bool IsOnFinishCell(Vector3 position)
        {
            var cellPos = Floor.WorldToCell(position);
            return cellPos == _data.FinishTilePosition;
        }
    }
}
