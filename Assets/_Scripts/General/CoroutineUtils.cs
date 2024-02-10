using System;
using MEC;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using MyMathStuff;
using UnityEditor;

public class CoroutineUtils : MonoSingleton<CoroutineUtils>
{
    #region Init

    public override void Init()
    {
        persistOnSceneLoad = false;
        base.Init();
    }

    #endregion

    #region Coroutine Run Methods

    #region Transform Coroutines

    public CoroutineHandle MoveTransformToTargetPosition(Transform transform, Vector3 targetPosition, float duration, bool useLocalSpace = false)
    {
        return Timing.RunCoroutine(_MoveTransformToTargetPosition(transform, targetPosition, duration, useLocalSpace));
    }

    public CoroutineHandle RotateTransformToTargetRotation(Transform transform, Quaternion targetRotation, float duration, bool useLocalSpace = false)
    {
        return Timing.RunCoroutine(_RotateTransformToTargetRotation(transform, targetRotation, duration, useLocalSpace));
    }

    public CoroutineHandle ScaleTransformToTargetScale(Transform transform, Vector3 targetScale, float duration)
    {
        return Timing.RunCoroutine(_ScaleTransformToTargetScale(transform, targetScale, duration));
    }

    public CoroutineHandle MoveTransformAlongPath(Transform transform, List<Vector3> waypoints, float duration, bool lookAtTheTarget, bool useLocalSpace)
    {
        return Timing.RunCoroutine(_MoveTransformAlongPath(transform, waypoints, duration, lookAtTheTarget, useLocalSpace));
    }

    public CoroutineHandle MoveTransformAlongBezierCurve(Transform transform, List<Vector3> points, float duration, float curveHeight, bool useLocalSpace)
    {
        return Timing.RunCoroutine(_MoveTransformAlongBezierCurve(transform, points, duration, curveHeight, useLocalSpace));
    }

    public CoroutineHandle PingPongTransformScale(Transform transform, Vector3 minScale, Vector3 maxScale, float duration)
    {
        return Timing.RunCoroutine(_PingPongTransformScale(transform, minScale, maxScale, duration));
    }

    public CoroutineHandle PingPonOnceTransformScale(Transform transform, Vector3 minScale, Vector3 maxScale, float duration)
    {
        return Timing.RunCoroutine(_PingPongOnceTransformScale(transform, minScale, maxScale, duration));
    }

    #endregion

    #region RectTransform Coroutines

    public CoroutineHandle MoveRectTransformToTargetPosition(RectTransform rectTransform, Vector3 targetPosition, float duration, bool useLocalSpace = false)
    {
        return Timing.RunCoroutine(_MoveRectTransformToTargetPosition(rectTransform, targetPosition, duration, useLocalSpace));
    }

    public CoroutineHandle RotateRectTransformToTargetRotation(RectTransform rectTransform, Quaternion targetRotation, float duration, bool useLocalSpace = false)
    {
        return Timing.RunCoroutine(_RotateRectTransformToTargetRotation(rectTransform, targetRotation, duration, useLocalSpace));
    }

    public CoroutineHandle MoveRectTransformAlongPath(RectTransform rectTransform, List<Vector3> waypoints, float duration, bool useLocalSpace)
    {
        return Timing.RunCoroutine(_MoveRectTransformAlongPath(rectTransform, waypoints, duration, useLocalSpace));
    }

    public CoroutineHandle ScaleRectTransformToTargetScale(RectTransform rectTransform, Vector3 targetScale, float duration)
    {
        return Timing.RunCoroutine(_ScaleRectTransformToTargetScale(rectTransform, targetScale, duration));
    }

    public CoroutineHandle MoveRectTransformAlongBezierCurve(RectTransform rectTransform, List<Vector3> points, float duration, float curveHeight, bool useLocalSpace)
    {
        return Timing.RunCoroutine(_MoveRectTransformAlongBezierCurve(rectTransform, points, duration, curveHeight, useLocalSpace));
    }

    #endregion

    #region Other

    /* Example how to call delayed action
        Action delayedBlock = () =>
            {
                //Write here what part of the code you want to delay
            };

        CoroutineUtils.instance.DelayedAction(delayedBlock, 2f);
     */
    
    public CoroutineHandle AlignTransformWithTarget(Transform sourceTransform, Transform targetTransform, float duration, bool useLocalSpace, bool alignPosition, bool alignRotation, bool alignScale)
    {
        return Timing.RunCoroutine(_AlignTransformWithTarget(sourceTransform, targetTransform, duration, useLocalSpace, alignPosition, alignRotation, alignScale));
    }

    public CoroutineHandle DelayedAction(Action action, float delay)
    {
        return Timing.RunCoroutine(_DelayedAction(action, delay));
    }

    #endregion

    #region MeshRenderer Coroutines

    public CoroutineHandle ChangeBlendShapeKey(SkinnedMeshRenderer skinnedMeshRenderer, int blendShapeIndex, float targetWeight, float duration)
    {
        return Timing.RunCoroutine(_ChangeBlendShapeKey(skinnedMeshRenderer, blendShapeIndex, targetWeight, duration));
    }
    
    #endregion
    
    #endregion

    #region Coroutines Block

    #region Transform Coroutines

    private IEnumerator<float> _MoveTransformToTargetPosition(Transform transform, Vector3 targetPosition, float duration, bool useLocalSpace)
    {
        Vector3 startPosition = useLocalSpace ? transform.localPosition : transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (transform == null)
                break;

            if (useLocalSpace)
                transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            else
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }

        if (transform != null)
        {
            if (useLocalSpace)
                transform.localPosition = targetPosition;
            else
                transform.position = targetPosition;
        }
    }

    private IEnumerator<float> _RotateTransformToTargetRotation(Transform transform, Quaternion targetRotation, float duration, bool useLocalSpace)
    {
        Quaternion startRotation = useLocalSpace ? transform.localRotation : transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (transform == null)
                break;

            if (useLocalSpace)
                transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            else
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }

        if (transform != null)
        {
            if (useLocalSpace)
                transform.localRotation = targetRotation;
            else
                transform.rotation = targetRotation;
        }
    }

    private IEnumerator<float> _ScaleTransformToTargetScale(Transform transform, Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (transform == null)
                break;

            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }

        if (transform != null)
        {
            transform.localScale = targetScale;
        }
    }

    private IEnumerator<float> _MoveTransformAlongBezierCurve(Transform transform, List<Vector3> points, float duration, float curveHeight, bool useLocalSpace)
    {
        if (points.Count < 4)
        {
            Debug.LogError("At least 4 points are required for a Bezier curve.");
            yield break;
        }

        Vector3 startPoint = points[0];
        Vector3 pointA = points[1];
        Vector3 pointB = points[2];
        Vector3 endPoint = points[3];

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (transform == null)
                break;

            float t = elapsedTime / duration;
            Vector3 curvePoint = MyMathUtility.CalculateBezierPoint(startPoint, pointA, pointB, endPoint, t, curveHeight);

            if (useLocalSpace)
                transform.localPosition = curvePoint;
            else
                transform.position = curvePoint;

            elapsedTime += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }

        if (transform != null)
        {
            if (useLocalSpace)
                transform.localPosition = endPoint;
            else
                transform.position = endPoint;
        }
    }

    private IEnumerator<float> _MoveTransformAlongPath(Transform transform, List<Vector3> waypoints, float duration, bool lookAtTheTarget, bool useLocalSpace)
    {
        if (waypoints.Count < 2)
        {
            Debug.LogError("At least 2 waypoints are required for a path.");
            yield break;
        }

        float elapsedTime = 0f;
        float pathDuration = duration / (waypoints.Count - 1);

        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Vector3 startPoint = waypoints[i];
            Vector3 endPoint = waypoints[i + 1];
            elapsedTime = 0f;

            while (elapsedTime < pathDuration)
            {
                if (transform == null)
                    break;

                float t = elapsedTime / pathDuration;
                Vector3 newPosition = Vector3.Lerp(startPoint, endPoint, t);

                if (useLocalSpace)
                    transform.localPosition = newPosition;
                else
                    transform.position = newPosition;

                if (lookAtTheTarget)
                    transform.LookAt(endPoint);

                elapsedTime += Time.deltaTime;
                yield return Timing.WaitForOneFrame;
            }
        }

        if (transform != null)
        {
            if (useLocalSpace)
                transform.localPosition = waypoints[waypoints.Count - 1];
            else
                transform.position = waypoints[waypoints.Count - 1];
        }
    }

    private IEnumerator<float> _PingPongTransformScale(Transform transform, Vector3 minScale, Vector3 maxScale, float duration)
    {
        float elapsedTime = 0f;

        while (true)
        {
            if (transform == null)
                break;

            elapsedTime += Time.deltaTime;
            float t = Mathf.PingPong(elapsedTime, duration) / duration;
            transform.localScale = Vector3.Lerp(minScale, maxScale, t);
            yield return Timing.WaitForOneFrame;
        }
    }

    private IEnumerator<float> _PingPongOnceTransformScale(Transform transform, Vector3 minScale, Vector3 maxScale, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            if (transform == null)
                break;

            elapsedTime += Time.deltaTime;
            float t = Mathf.PingPong(elapsedTime, duration) / duration;
            transform.localScale = Vector3.Lerp(minScale, maxScale, t);
            yield return Timing.WaitForOneFrame;
        }
    }
    
    private IEnumerator<float> _AlignTransformWithTarget(Transform sourceTransform, Transform targetTransform, float duration, bool useLocalSpace, bool alignPosition, bool alignRotation, bool alignScale)
    {

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (sourceTransform == null)
                break;

            float t = elapsedTime / duration;

            if (alignPosition)
            {
                if (useLocalSpace)
                    sourceTransform.localPosition = targetTransform.localPosition;
                else
                    sourceTransform.position = targetTransform.position;
            }

            if (alignRotation)
            {
                if (useLocalSpace)
                    sourceTransform.localRotation = targetTransform.localRotation;
                else
                    sourceTransform.rotation = targetTransform.rotation;
            }

            if (alignScale)
            {
                sourceTransform.localScale = targetTransform.localScale;
            }

            elapsedTime += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
    }

    #endregion

    #region RectTransform Coroutines

    private IEnumerator<float> _MoveRectTransformToTargetPosition(RectTransform rectTransform, Vector3 targetPosition, float duration, bool useLocalSpace)
    {
        Vector3 startPosition = useLocalSpace ? rectTransform.localPosition : rectTransform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (rectTransform == null)
                break;

            if (useLocalSpace)
                rectTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            else
                rectTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }

        if (rectTransform != null)
        {
            if (useLocalSpace)
                rectTransform.localPosition = targetPosition;
            else
                rectTransform.position = targetPosition;
        }
    }

    private IEnumerator<float> _RotateRectTransformToTargetRotation(RectTransform rectTransform, Quaternion targetRotation, float duration, bool useLocalSpace)
    {
        Quaternion startRotation = useLocalSpace ? rectTransform.localRotation : rectTransform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (rectTransform == null)
                break;

            if (useLocalSpace)
                rectTransform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            else
                rectTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }

        if (rectTransform != null)
        {
            if (useLocalSpace)
                rectTransform.localRotation = targetRotation;
            else
                rectTransform.rotation = targetRotation;
        }
    }

    private IEnumerator<float> _ScaleRectTransformToTargetScale(RectTransform rectTransform, Vector3 targetScale, float duration)
    {
        Vector3 startScale = rectTransform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (rectTransform == null)
                break;

            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }

        if (rectTransform != null)
        {
            rectTransform.localScale = targetScale;
        }
    }

    private IEnumerator<float> _MoveRectTransformAlongBezierCurve(RectTransform rectTransform, List<Vector3> points, float duration, float curveHeight, bool useLocalSpace)
    {
        if (points.Count < 4)
        {
            Debug.LogError("At least 4 points are required for a Bezier curve.");
            yield break;
        }

        Vector3 startPoint = points[0];
        Vector3 pointA = points[1];
        Vector3 pointB = points[2];
        Vector3 endPoint = points[3];

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (rectTransform == null)
                break;

            float t = elapsedTime / duration;
            Vector3 curvePoint = MyMathUtility.CalculateBezierPoint(startPoint, pointA, pointB, endPoint, t, curveHeight);

            if (useLocalSpace)
                rectTransform.localPosition = curvePoint;
            else
                rectTransform.position = curvePoint;

            elapsedTime += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }

        if (rectTransform != null)
        {
            if (useLocalSpace)
                rectTransform.localPosition = endPoint;
            else
                rectTransform.position = endPoint;
        }
    }

    private IEnumerator<float> _MoveRectTransformAlongPath(RectTransform rectTransform, List<Vector3> waypoints, float duration, bool useLocalSpace)
    {
        if (waypoints.Count < 2)
        {
            Debug.LogError("At least 2 waypoints are required for a path.");
            yield break;
        }

        float elapsedTime = 0f;
        float pathDuration = duration / (waypoints.Count - 1);

        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Vector3 startPoint = waypoints[i];
            Vector3 endPoint = waypoints[i + 1];
            elapsedTime = 0f;

            while (elapsedTime < pathDuration)
            {
                if (rectTransform == null)
                    break;

                float t = elapsedTime / pathDuration;
                Vector3 newPosition = Vector3.Lerp(startPoint, endPoint, t);

                if (useLocalSpace)
                    rectTransform.localPosition = newPosition;
                else
                    rectTransform.position = newPosition;

                elapsedTime += Time.deltaTime;
                yield return Timing.WaitForOneFrame;
            }
        }

        if (rectTransform != null)
        {
            if (useLocalSpace)
                rectTransform.localPosition = waypoints[waypoints.Count - 1];
            else
                rectTransform.position = waypoints[waypoints.Count - 1];
        }
    }

    #endregion

    #region Other Coroutines

    private IEnumerator<float> _DelayedAction(Action action, float delay)
    {
        yield return Timing.WaitForSeconds(delay);
        action?.Invoke();
    }
    
  

    #endregion
    
    #region MeshRenderer Coroutines
    
    private IEnumerator<float> _ChangeBlendShapeKey(SkinnedMeshRenderer skinnedMeshRenderer, int blendShapeIndex, float targetWeight, float duration)
    {
        float startWeight = skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (skinnedMeshRenderer == null)
                break;

            float currentWeight = Mathf.Lerp(startWeight, targetWeight, elapsedTime / duration);
            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, currentWeight);

            elapsedTime += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }

        if (skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, targetWeight);
        }
    }

    #endregion
    
    #endregion
    
}
