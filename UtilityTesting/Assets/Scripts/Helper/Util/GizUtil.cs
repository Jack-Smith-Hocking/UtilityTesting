using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jack.Utility
{
    public static class GizUtil
    {
        #region GizmosColour
        /// <summary>
        /// The original Gizmos.color
        /// </summary>
        public static Color? RealDefaultColour { get; private set; } = null;

        /// <summary>
        /// Default Gizmos colour according to this class
        /// </summary>
        public static Color? DefaultColour { get; set; } = null;

        public static Color CurrentColour
        {
            get => Gizmos.color;
            set => SetColour(value);
        }

        public static void SetDefaultColour(Color colour) => DefaultColour = colour;
        public static void ResetDefaultColour() => DefaultColour = RealDefaultColour;

        public static void SetColour(Color colour)
        {
            DefaultColour = DefaultColour ?? Gizmos.color;
            RealDefaultColour = RealDefaultColour ?? Gizmos.color;

            Gizmos.color = colour;
        }
        public static void ResetColour() => Gizmos.color = DefaultColour.Value;
        #endregion

        #region DrawLineGradient
        /// <summary>
        /// Draw a line from start => end, with a colour gradient instead of single colour
        /// </summary>
        /// <param name="start">Start point of the line</param>
        /// <param name="end">End point of the line</param>
        /// <param name="startColour">The starting gradient colour</param>
        /// <param name="endColour">The colour to lerp into</param>
        /// <param name="segmentCount">How many line segments, more segments will have a smoother gradient</param>
        /// <param name="lerpSpeed">Speed to lerp into the end colour</param>
        /// <param name="gradientFunc">Leave null for linear gradient, input int is the segment index</param>
        public static void DrawLineGradient(Vector3 start, Vector3 end, Color startColour, Color endColour, int segmentCount = 20, float lerpSpeed = 1, System.Func<int, float> gradientFunc = null)
        {
            float _dist = Vector3.Distance(start, end);
            Vector3 _dir = MathUtil.Direction(start, end);

            DrawLineGradient(new Ray(start, _dir), _dist, startColour, endColour, segmentCount, lerpSpeed, gradientFunc);
        }
        /// <summary>
        /// Draw a line based on direction and length, with a colour gradient instead of single colour
        /// </summary>
        /// <param name="start">Start point of the line</param>
        /// <param name="dir">Direction to draw the line in</param>
        /// <param name="length">The length of the line to draw</param>
        /// <param name="startColour">The starting gradient colour</param>
        /// <param name="endColour">The colour to lerp into</param>
        /// <param name="segmentCount">How many line segments, more segments will have a smoother gradient</param>
        /// <param name="lerpSpeed">Speed to lerp into the end colour</param>
        /// <param name="gradientFunc">Leave null for linear gradient, input int is the segment index</param>
        public static void DrawLineGradient(Vector3 start, Vector3 dir, float length, Color startColour, Color endColour, int segmentCount = 20, float lerpSpeed = 1, System.Func<int, float> gradientFunc = null)
        {
            DrawLineGradient(new Ray(start, dir), length, startColour, endColour, segmentCount, lerpSpeed, gradientFunc);
        }

        /// <summary>
        /// Draw a line based on direction and length, with a colour gradient instead of single colour
        /// </summary>
        /// <param name="ray">Ray representing the start point and direction of the line</param>
        /// <param name="length">The length of the line to draw</param>
        /// <param name="startColour">The starting gradient colour</param>
        /// <param name="endColour">The colour to lerp into</param>
        /// <param name="segmentCount">How many line segments, more segments will have a smoother gradient</param>
        /// <param name="lerpSpeed">Speed to lerp into the end colour</param>
        /// <param name="gradientFunc">Leave null for linear gradient, input int is the segment index</param>
        public static void DrawLineGradient(Ray ray, float length, Color startColour, Color endColour, int segmentCount = 20, float lerpSpeed = 1, System.Func<int, float> gradientFunc = null)
        {
            segmentCount = Mathf.Clamp(segmentCount, 1, 250); // Too many segments is bad for performance
            gradientFunc = gradientFunc.SetIfNull((index) => index / (float)segmentCount); // Default linear gradient

            float _lineSegmentLength = length / segmentCount;
            Vector3 _lineSegment = ray.direction * _lineSegmentLength;

            for (int _gradIndex = 0; _gradIndex < segmentCount; _gradIndex++)
            {
                float _gradRatio = gradientFunc.Invoke(_gradIndex);

                Vector3 _segmentStart = ray.Point(_gradIndex * _lineSegmentLength); // Calculate the start of the new line segment

                GizUtil.CurrentColour = Color.Lerp(startColour, endColour, _gradRatio * lerpSpeed); // Update Gizmos colour based on lerp
                Gizmos.DrawLine(_segmentStart, _segmentStart + _lineSegment);
            }

            ResetColour();
        }
        #endregion
    }
}