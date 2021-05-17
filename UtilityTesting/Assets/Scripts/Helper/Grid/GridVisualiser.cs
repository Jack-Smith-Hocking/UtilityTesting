using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper.Utility;
using Helper.Updater;
using Sirenix.OdinInspector;

namespace Helper.Grid
{
    public class GridVisualiser : MonoBehaviour
    {
        [TabGroup("Grid")]
        public Transform m_gridBackPlane;

        [Space]

        [TabGroup("Grid")]
        public int m_width;

        [TabGroup("Grid")]
        public int m_height;

        [TabGroup("Grid")]
        public float m_cellSize;

        // //

        [TabGroup("Debug")]
        public bool m_showGridDebug = true;

        [Space]

        [TabGroup("Debug")]
        [ShowIf(nameof(m_showGridDebug))]
        public Transform m_debugParent;

        [TabGroup("Debug")]
        [ShowIf(nameof(m_showGridDebug))]
        public GameObject m_textMeshPrefab;

        private Grid<int> m_testIntGrid;

        private void Start()
        {
            m_testIntGrid = new Grid<int>(m_width, m_height, m_cellSize, transform.position, 0);
            m_testIntGrid.SetGridObject(0, 0, 56);

            if (m_showGridDebug == false) return;

            GridDebugInit(m_testIntGrid, m_textMeshPrefab, m_debugParent);
        }

        private void Update()
        {
            MouseHover();
            MouseClick();
        }

        private void MouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit _rayHit)) return;

                Vector3 _gridPos = _rayHit.point;

                m_testIntGrid.GetGridObject(_gridPos, out int _obj);
                m_testIntGrid.SetGridObject(_gridPos, _obj += 5);
            }
        }
        private void MouseHover()
        {
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit _rayHit)) return;

            Vector3 _gridPos = _rayHit.point;

            m_testIntGrid.GetGridObject(_gridPos, out int _obj);
            m_testIntGrid.SetGridObject(_gridPos, _obj += 1);
        }

        private static void GridDebugInit<T>(Grid<T> grid, GameObject prefab, Transform parent = null)
        {
            if (grid.IsNull()) return;
            if (prefab.IsNull()) return;

            TextMesh[,] m_textGrid = new TextMesh[grid.Width, grid.Height];

            grid.LoopThroughGrid((x, y) =>
            {
                grid.GetGridObject(x, y, out T _obj);

                GameObject _prefabInstance = Instantiate(prefab, grid.ToCentredWorldPosition(x, y), Quaternion.identity, parent);
                m_textGrid[x, y] = _prefabInstance.ExtractComponent<TextMesh>();
                m_textGrid[x, y].text = _obj?.ToString();

                Debug.DrawLine(grid.ToWorldPosition(x, y), grid.ToWorldPosition(x, y + 1), Color.white, float.MaxValue);
                Debug.DrawLine(grid.ToWorldPosition(x, y), grid.ToWorldPosition(x + 1, y), Color.white, float.MaxValue);
            });

            Debug.DrawLine(grid.ToWorldPosition(0, grid.Height), grid.ToWorldPosition(grid.Width, grid.Height), Color.white, float.MaxValue);
            Debug.DrawLine(grid.ToWorldPosition(grid.Width, 0), grid.ToWorldPosition(grid.Width, grid.Height), Color.white, float.MaxValue);

            grid.OnGridObjectChanged += (x, y) =>
            {
                grid.GetGridObject(x, y, out T _obj);
                m_textGrid[x, y].text = _obj?.ToString();
            };
        }

    }
}