using UnityEngine;

public class InfiniteScroller2D : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Camera referenceCamera;
    [SerializeField] private Transform[] segments;
    [SerializeField] private bool autoDetectSegmentWidth = true;
    [SerializeField] private float segmentWidth = 10f;
    [SerializeField] private float spawnAheadOffset = 20f;
    [SerializeField] private float recycleOffset = 2f;

    void Reset()
    {
        segments = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            segments[i] = transform.GetChild(i);
        }
    }

    void LateUpdate()
    {
        if (target == null || segments == null || segments.Length == 0 || segmentWidth <= 0f)
        {
            return;
        }

        Transform leftmostSegment = segments[0];
        Transform rightmostSegment = segments[0];

        for (int i = 1; i < segments.Length; i++)
        {
            if (segments[i] == null)
            {
                continue;
            }

            if (leftmostSegment == null || segments[i].position.x < leftmostSegment.position.x)
            {
                leftmostSegment = segments[i];
            }

            if (rightmostSegment == null || segments[i].position.x > rightmostSegment.position.x)
            {
                rightmostSegment = segments[i];
            }
        }

        if (leftmostSegment == null || rightmostSegment == null)
        {
            return;
        }

        float currentSegmentWidth = GetSegmentWidth(leftmostSegment);

        if (currentSegmentWidth <= 0f)
        {
            return;
        }

        Camera activeCamera = referenceCamera != null ? referenceCamera : Camera.main;
        float leftCameraEdge = target.position.x;
        float rightCameraEdge = target.position.x;

        if (activeCamera != null && activeCamera.orthographic)
        {
            leftCameraEdge = activeCamera.transform.position.x - activeCamera.orthographicSize * activeCamera.aspect;
            rightCameraEdge = activeCamera.transform.position.x + activeCamera.orthographicSize * activeCamera.aspect;
        }

        float recyclePoint = leftmostSegment.position.x + currentSegmentWidth + recycleOffset;

        if (leftCameraEdge > recyclePoint)
        {
            float rightmostWidth = GetSegmentWidth(rightmostSegment);
            Vector3 newPosition = rightmostSegment.position + new Vector3(rightmostWidth, 0f, 0f);
            leftmostSegment.position = newPosition;
        }

        EnsureCoverageAhead(rightCameraEdge);
    }

    float GetSegmentWidth(Transform segment)
    {
        if (!autoDetectSegmentWidth || segment == null)
        {
            return segmentWidth;
        }

        Renderer segmentRenderer = segment.GetComponent<Renderer>();

        if (segmentRenderer != null)
        {
            return segmentRenderer.bounds.size.x;
        }

        Collider2D segmentCollider = segment.GetComponent<Collider2D>();

        if (segmentCollider != null)
        {
            return segmentCollider.bounds.size.x;
        }

        return segmentWidth;
    }

    void EnsureCoverageAhead(float rightCameraEdge)
    {
        Transform rightmostSegment = null;
        float farthestRightEdge = float.MinValue;

        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i] == null)
            {
                continue;
            }

            float width = GetSegmentWidth(segments[i]);
            float segmentRightEdge = segments[i].position.x + width;

            if (segmentRightEdge > farthestRightEdge)
            {
                farthestRightEdge = segmentRightEdge;
                rightmostSegment = segments[i];
            }
        }

        if (rightmostSegment == null)
        {
            return;
        }

        while (farthestRightEdge < rightCameraEdge + spawnAheadOffset)
        {
            Transform leftmostSegment = GetLeftmostSegment();

            if (leftmostSegment == null || leftmostSegment == rightmostSegment)
            {
                return;
            }

            float rightmostWidth = GetSegmentWidth(rightmostSegment);
            Vector3 newPosition = rightmostSegment.position + new Vector3(rightmostWidth, 0f, 0f);
            leftmostSegment.position = newPosition;

            rightmostSegment = leftmostSegment;
            farthestRightEdge = rightmostSegment.position.x + GetSegmentWidth(rightmostSegment);
        }
    }

    Transform GetLeftmostSegment()
    {
        Transform leftmostSegment = null;

        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i] == null)
            {
                continue;
            }

            if (leftmostSegment == null || segments[i].position.x < leftmostSegment.position.x)
            {
                leftmostSegment = segments[i];
            }
        }

        return leftmostSegment;
    }
}
