using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Utility.Helper
{
    public static class UtilRand
    {
        public static Vector3 RandVector(float min, float max) => new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
        public static Vector3 RandVector() => RandVector(float.MinValue, float.MaxValue);

        public static Vector3 RandDirection() => RandVector(-1f, 1f).normalized;
        public static Vector3 RandDirectionXY() => new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        public static Vector3 RandDirectionXZ() => new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

        public static Color RandColour() => new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        public static Color RandColour(float alpha) => new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), alpha);

        public static T RandElement<T>(T[] collection) => collection[Random.Range(0, collection.Length)];
        public static T RandElement<T>(List<T> collection) => collection[Random.Range(0, collection.Count)];

        #region Random NavMesh Position
        /// <summary>
        /// Get a random point on a NavMesh
        /// </summary>
        /// <param name="origin">The original point of the object</param>
        /// <param name="dist">The max distance away from the origin to test for</param>
        /// <param name="maxSampleRate">Will continue to generate a random position until a valid one is found</param>
        /// <returns>A valid position on the NavMesh</returns>
        public static Vector3 RandNavPos(Vector3 origin, float dist, int maxSampleRate)
        {
            Vector3 _pos = origin;

            for (int _iteration = 0; _iteration < maxSampleRate; _iteration++)
            {
                if (!RandNavPos(origin, dist, out _pos)) continue;

                return _pos;
            }

            return origin;
        }
        /// <summary>
        /// Get a random point on a NavMesh
        /// </summary>
        /// <param name="origin">The original point of the object</param>
        /// <param name="dist">The max distance away from the origin to test for</param>
        /// <returns>Whether a valid position was sampled</returns>
        public static bool RandNavPos(Vector3 origin, float dist, out Vector3 position)
        {
            bool _foundPos = false;

            NavMeshHit _navHit;

            Vector3 _randPos = UnityEngine.Random.insideUnitSphere * Mathf.Abs(dist);
            _randPos += origin;

            _foundPos = NavMesh.SamplePosition(_randPos, out _navHit, Mathf.Abs(dist), 1);

            position = _foundPos ? _navHit.position : origin;

            return _foundPos;
        }
        #endregion
    }
}