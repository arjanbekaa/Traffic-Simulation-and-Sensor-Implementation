using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyMathStuff{
    public static class MyMathUtility
    {
        //Returns a random rotation
        public static Vector3 GetRandomRotation()
        {
            return new Vector3(Random.Range(-360f, 360f), Random.Range(-360f, 360f), Random.Range(-360f, 360f));
        }

        //Returns a random normalized roatation
        public static Vector3 GetRandomNormalizedVector3()
        {
            return new Vector3(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        }
        
        public static Vector3 RandomizeVectorWithinRadius(Vector3 center, float radius)
        {
            return center + Random.insideUnitSphere * radius;
        }

        // Calculate the points of the bezier curve
        public static Vector3 CalculateBezierPoint(Vector3 startPoint, Vector3 pointA, Vector3 pointB, Vector3 endPoint, float t, float curveHeight)
        {

            // Calculate the four points of the bezier curve
            Vector3 pointAB = Vector3.Lerp(startPoint, pointA, t);
            Vector3 pointBC = Vector3.Lerp(pointA, pointB, t);
            Vector3 pointCD = Vector3.Lerp(pointB, endPoint, t);
            Vector3 pointABC = Vector3.Lerp(pointAB, pointBC, t);
            Vector3 pointBCD = Vector3.Lerp(pointBC, pointCD, t);
            Vector3 curvePoint = Vector3.Lerp(pointABC, pointBCD, t);

            // Add curvature to the curve by raising the middle point
            curvePoint += Vector3.up * curveHeight * Mathf.Sin(t * Mathf.PI);

            return curvePoint;
        }

        public static Vector3 CalculateBezierPoint(Vector3 startPoint, Vector3 pointA, Vector3 pointB, Vector3 endPoint, Vector3 dir, float t, float curveHeight)
        {

            // Calculate the four points of the bezier curve
            Vector3 pointAB = Vector3.Lerp(startPoint, pointA, t);
            Vector3 pointBC = Vector3.Lerp(pointA, pointB, t);
            Vector3 pointCD = Vector3.Lerp(pointB, endPoint, t);
            Vector3 pointABC = Vector3.Lerp(pointAB, pointBC, t);
            Vector3 pointBCD = Vector3.Lerp(pointBC, pointCD, t);
            Vector3 curvePoint = Vector3.Lerp(pointABC, pointBCD, t);

            // Add curvature to the curve by raising the middle point
            curvePoint += dir * curveHeight * Mathf.Sin(t * Mathf.PI);

            return curvePoint;
        }

        public static List<Vector3> GetBezierControlPoints(Vector3 startPoint, Vector3 endPoint, Vector3 direction, float height)
        {
            List<Vector3> controlPoints = new List<Vector3>();

            Vector3 pointA = startPoint + direction * height;
            Vector3 pointB = endPoint - direction * height;

            controlPoints.Add(startPoint);
            controlPoints.Add(pointA);
            controlPoints.Add(pointB);
            controlPoints.Add(endPoint);

            return controlPoints;
        }
    }
}
