using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jack.Utility;
using System.Linq;

namespace Jack.Utility
{
    public static class DistUtil
    {
        public static Vector3 ToPoint(GameObject point) => point.transform.position;
        public static Vector3 ToPoint(Component point) => point.ToPoint();

        #region SortClosest
        /// <summary>
        /// Sort a collection of points based on the distance to point, ascending order
        /// </summary>
        public static ICollection<TPoint> SortClosestPoints<TPoint>(Vector3 point, ICollection<TPoint> points) where TPoint : Component
        {
            ICollection<TPoint> _sortedPoints = points.Sort((p1, p2) =>
            {
                return CompareDist(point, p1.ToPoint(), p2.ToPoint());
            });

            return _sortedPoints;
        }

        /// <summary>
        /// Sort a collection of points based on the distance to point, ascending order
        /// </summary>
        public static ICollection<GameObject> SortClosestPoints(Vector3 point, ICollection<GameObject> points)
        {
            ICollection<GameObject> _sortedPoints = points.Sort((p1, p2) =>
            {
                return CompareDist(point, p1.ToPoint(), p2.ToPoint());
            });

            return _sortedPoints;
        }

        /// <summary>
        /// Sort a collection of points based on the distance to point, ascending order
        /// </summary>
        public static ICollection<Vector3> SortClosestPoints(Vector3 point, ICollection<Vector3> points)
        {
            ICollection<Vector3> _sortedPoints = points.Sort((p1, p2) =>
            {
                return CompareDist(point, p1, p2);
            });

            return _sortedPoints;
        }
        #endregion

        #region GetClosestPoint
        /// <summary>
        /// Get the closest point from a collection
        /// </summary>
        public static TPoint GetClosestPoint<TPoint>(Vector3 point, ICollection<TPoint> points) where TPoint : Component
        {
            return SortClosestPoints(point, points).ElementAt(0);
        }

        /// <summary>
        /// Get the closest point from a collection
        /// </summary>
        public static GameObject GetClosestPoint(Vector3 point, ICollection<GameObject> points)
        {
            return SortClosestPoints(point, points).ElementAt(0);
        }

        /// <summary>
        /// Get the closest point from a collection
        /// </summary>
        public static Vector3 GetClosestPoint(Vector3 point, ICollection<Vector3> points)
        {
            return SortClosestPoints(point, points).ElementAt(0);
        }
        #endregion

        #region GetClosest
        /// <summary>
        /// Return the closest of two points from an anchor
        /// </summary>
        public static TPoint GetClosest<TPoint>(Vector3 anchor, TPoint p1, TPoint p2) where TPoint : Component
        {
            float _comp = CompareDist(anchor, ToPoint(p1), ToPoint(p2));

            return _comp <= 0 ? p1 : p2;
        }

        /// <summary>
        /// Return the closest of two points from an anchor
        /// </summary>
        public static GameObject GetClosest(Vector3 anchor, GameObject p1, GameObject p2)
        {
            float _comp = CompareDist(anchor, ToPoint(p1), ToPoint(p2));
            return _comp <= 0 ? p1 : p2;
        }

        /// <summary>
        /// Return the closest of two points from an anchor
        /// </summary>
        public static Vector3 GetClosest(Vector3 anchor, Vector3 p1, Vector3 p2)
        {
            float _comp = CompareDist(anchor, p1, p2);

            return _comp <= 0 ? p1 : p2;
        }
        #endregion

        #region CompareDist
        /// <summary>
        /// .CompareTo the distances from p1->anchor and p2->anchor
        /// </summary>
        /// <returns>(p1 < p2 = < 0), (Equal to -> 0), (p1 > p2 = > 0)</returns>
        public static int CompareDist(Vector3 anchor, Component p1, Component p2) => CompareDist(anchor, p1.ToPoint(), p2.ToPoint());

        /// <summary>
        /// .CompareTo the distances from p1->anchor and p2->anchor
        /// </summary>
        /// <returns>(p1 < p2 = < 0), (Equal to -> 0), (p1 > p2 = > 0)</returns>
        public static int CompareDist(Vector3 anchor, GameObject p1, GameObject p2) => CompareDist(anchor, p1.ToPoint(), p2.ToPoint());

        /// <summary>
        /// .CompareTo the distances from p1->anchor and p2->anchor
        /// </summary>
        /// <returns>(p1 < p2 = < 0), (Equal to -> 0), (p1 > p2 = > 0)</returns>
        public static int CompareDist(Vector3 anchor, Vector3 p1, Vector3 p2)
        {
            float _p1Dist = DistUtil.DistanceSqrd(p1, anchor);
            float _p2Dist = DistUtil.DistanceSqrd(p2, anchor);

            return _p1Dist.CompareTo(_p2Dist);
        }
        #endregion

        #region Distance Squared
        public static float DistanceSqrd(Component p1, Component p2) => DistanceSqrd(p1.ToPoint(), p2.ToPoint());

        public static float DistanceSqrd(GameObject p1, GameObject p2) => DistanceSqrd(p1.ToPoint(), p2.ToPoint());

        public static float DistanceSqrd(Vector3 a, Vector3 b) => (a - b).sqrMagnitude;
        #endregion

        #region InDistance
        public static bool InDistance(Component p1, Component p2, float dist) => InDistance(p1.ToPoint(), p2.ToPoint(), dist);

        public static bool InDistance(GameObject p1, GameObject p2, float dist) => InDistance(p1.ToPoint(), p2.ToPoint(), dist);

        /// <summary>
        /// Check if two Vector3s are within 'dist' of each other
        /// </summary>
        /// <param name="dist">[inclusive]</param>
        public static bool InDistance(Vector3 a, Vector3 b, float dist) => (a - b).sqrMagnitude <= (dist * dist);
        #endregion

        #region OutDistance
        public static bool OutDistance(Component p1, Component p2, float dist) => OutDistance(p1.ToPoint(), p2.ToPoint(), dist);

        public static bool OutDistance(GameObject p1, GameObject p2, float dist) => OutDistance(p1.ToPoint(), p2.ToPoint(), dist);

        /// <summary>
        /// Check if two Vector3s are further than 'dist' from each other
        /// </summary>
        /// <param name="dist">[exclusive]</param>
        public static bool OutDistance(Vector3 a, Vector3 b, float dist) => (a - b).sqrMagnitude > (dist * dist);
        #endregion
    }

    public static class Dist_Ext
    {
        public static Vector3 ToPoint(this Component comp) => comp.transform.position;
        public static Vector3 ToPoint(this GameObject go) => go.transform.position;
    }
}