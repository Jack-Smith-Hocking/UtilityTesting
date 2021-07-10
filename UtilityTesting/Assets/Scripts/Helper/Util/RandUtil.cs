using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Jack.Utility
{
    public static class RandUtil
    {
        /// <summary>
        /// Returns a random vector with x,y,z between min [inclusive] and max [inclusive]
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector3 Vector(float min, float max) => new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
        /// <summary>
        /// Returns a random vector with x,y,z between the lowest float value and the highest float value
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector3 Vector() => Vector(float.MinValue, float.MaxValue);

        /// <summary>
        /// Return a random normalised vector [x, y, z]
        /// </summary>
        /// <returns></returns>
        public static Vector3 Direction() => Vector(-1f, 1f).normalized;
        /// <summary>
        /// return a random normalised vector [x, y]
        /// </summary>
        /// <returns></returns>
        public static Vector3 DirectionXY() => new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        /// <summary>
        /// return a random normalised vector [x, z]
        /// </summary>
        /// <returns></returns>
        public static Vector3 DirectionXZ() => new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;

        /// <summary>
        /// Return a random colour with rgb values between 0 and 1, where a = 1
        /// </summary>
        /// <returns></returns>
        public static Color Colour() => new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        /// <summary>
        /// Return a random colour with rgb values between 0 and 1
        /// </summary>
        /// <returns></returns>
        public static Color Colour(float alpha) => new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), alpha);

        /// <summary>
        /// Return random element in an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T Element<T>(T[] collection) => collection[Random.Range(0, collection.Length)];
        /// <summary>
        /// Return random element in a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T Element<T>(List<T> collection) => collection[Random.Range(0, collection.Count)];

        #region Random NavMesh Position
        /// <summary>
        /// Get a random point on a NavMesh
        /// </summary>
        /// <param name="origin">The original point of the object</param>
        /// <param name="dist">The max distance away from the origin to test for</param>
        /// <param name="maxSampleRate">Will continue to generate a random position until a valid one is found</param>
        /// <returns>A valid position on the NavMesh</returns>
        public static Vector3 NavMeshPosition(Vector3 origin, float dist, int maxSampleRate)
        {
            Vector3 _pos = origin;

            for (int _iteration = 0; _iteration < maxSampleRate; _iteration++)
            {
                if (!NavMeshPosition(origin, dist, out _pos)) continue;

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
        public static bool NavMeshPosition(Vector3 origin, float dist, out Vector3 position)
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
    public static class Rand_Ext
    {
        /// <summary>
        /// Return random element in an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T Rand<T>(this T[] collection) => collection[Random.Range(0, collection.Length)];
        /// <summary>
        /// Return random element in a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T Rand<T>(this List<T> collection) => collection[Random.Range(0, collection.Count)];
    }
}