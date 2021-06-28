using System.Collections;
using System.Collections.Generic;
using Jack.Utility;
using UnityEngine;

namespace Jack.Grid
{
    public class Grid<TGridObject>
    {
        public event System.Action<int, int> OnGridObjectChanged;

        public int Height { get; private set; } = 0;
        public int Width { get; private set; } = 0;
        public float CellSize { get; private set; } = 0;

        public Vector3 Origin { get; private set; } = Vector3.zero;

        private TGridObject[,] m_gridArr;

        // Public //

        public Grid(int width, int height, float cellSize, Vector3 origin, TGridObject defaultVal = default) => InitGrid(width, height, cellSize, origin, (gridObject, tempWidth, tempHeight) => defaultVal);
        public Grid(int width, int height, float cellSize, Vector3 origin, System.Func<Grid<TGridObject>, int, int, TGridObject> defaultVal) => InitGrid(width, height, cellSize, origin, defaultVal);

        public bool GetGridObject(int x, int y, out TGridObject returnObject)
        {
            returnObject = default;
            if (IsValidInput(x, y) == false) return false;

            returnObject = m_gridArr[x, y];
            return true;
        }
        public bool GetGridObject(Vector2Int gridPosition, out TGridObject returnObject) => GetGridObject(gridPosition.x, gridPosition.y, out returnObject);
        public bool GetGridObject(Vector3 worldPosition, out TGridObject returnObject) => GetGridObject(ToGridPosition(worldPosition), out returnObject);

        public void SetGridObject(int x, int y, TGridObject value)
        {
            if (IsValidInput(x, y) == false) return;

            m_gridArr[x, y] = value;

            OnGridObjectChanged?.Invoke(x, y);
        }
        public void SetGridObject(Vector2Int gridPosition, TGridObject value) => SetGridObject(gridPosition.x, gridPosition.y, value);
        public void SetGridObject(Vector3 worldPosition, TGridObject value) => SetGridObject(ToGridPosition(worldPosition), value);

        public void TriggerGridObjectChanged(int x, int y)
        {
            if (IsValidInput(x, y) == false) return;

            OnGridObjectChanged?.Invoke(x, y);
        }

        public Vector3 ToCentredWorldPosition(int x, int y) => ToWorldPosition(x, y) + (new Vector3(CellSize, CellSize) * 0.5f);
        public Vector3 ToCentredWorldPosition(Vector2Int gridPosition) => ToCentredWorldPosition(gridPosition.x, gridPosition.y);
        public Vector3 ToCentredWorldPosition(Vector3 worldPosition) => ToCentredWorldPosition(ToGridPosition(worldPosition));

        public Vector3 ToWorldPosition(int x, int y) => (new Vector3(x, y) * CellSize) + Origin;
        public Vector3 ToWorldPosition(Vector2Int gridPosition) => ToWorldPosition(gridPosition.x, gridPosition.y);

        public Vector2Int ToGridPosition(Vector3 worldPosition)
        {
            Vector3 _pos = worldPosition - Origin;
            int _x = Mathf.FloorToInt(_pos.x / CellSize);
            int _y = Mathf.FloorToInt(_pos.y / CellSize);

            return new Vector2Int(_x, _y);
        }

        public void LoopThroughGrid(System.Action<int, int> loopAction)
        {
            if (loopAction.IsNull()) return;

            for (int _x = 0; _x < Width; _x++)
            {
                for (int _y = 0; _y < Height; _y++)
                {
                    loopAction?.Invoke(_x, _y);
                }
            }
        }
        
        // Private //

        private void InitGrid(int width, int height, float cellSize, Vector3 origin, System.Func<Grid<TGridObject>, int, int, TGridObject> defaultVal)
        {
            Width = Mathf.Clamp(width, 0, int.MaxValue);
            Height = Mathf.Clamp(height, 0, int.MaxValue);
            CellSize = Mathf.Clamp(cellSize, 0, float.MaxValue);

            Origin = origin;

            m_gridArr = new TGridObject[Width, Height];

            LoopThroughGrid((x, y) =>
            {
                m_gridArr[x, y] = defaultVal.Invoke(this, Width, Height);
            });
        }

        private bool IsValidInput(int x, int y) => x.InRange(0, Width - 1) && y.InRange(0, Height - 1);
    }
}