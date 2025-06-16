using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    //[SerializeField] private MeshFilter m_viewMeshFilter;
    [SerializeField] private float m_viewRadius;
    [Range(0, 360)] [SerializeField] private float m_viewAngle;
    [SerializeField] private LayerMask m_groundLayer;
    [SerializeField] private LayerMask m_targetLayer;
    [Header("Debug")]
    [SerializeField] private bool m_drawFieldOfView;

    private List<Transform> m_visibleTargets;
    private bool m_isTargetsClose;

    private void Awake()
    {
        m_visibleTargets = new List<Transform>();
        m_isTargetsClose = false;
    }

    private void Start()
    {
        StartCoroutine(FindTargetsWithDelay(0.2f));
    }

    private void Update()
    {
        if(m_drawFieldOfView)
        {
            DrawFieldOfView();
        }
    }

    private void FindVisibleTargets()
    {
        m_visibleTargets.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, m_viewRadius, m_targetLayer);
        for(int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.right, dirToTarget) < m_viewAngle/2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if(!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, m_groundLayer))
                {
                    m_visibleTargets.Add(target);
                }
            }
        }
    }

    private void FindVisibleTargetsClose()
    {
        m_isTargetsClose = false;
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, m_viewRadius, m_targetLayer);
        for(int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.right, dirToTarget) < (m_viewAngle + 10f)/2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if(!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, m_groundLayer))
                {
                    m_isTargetsClose = true;
                }
            }
        }
    }

    private void DrawFieldOfView()
    {
        Vector2 line = Vector2.right * m_viewRadius;
        Vector2 rotatedLine = Quaternion.Euler(0, 0, m_viewAngle/2 + transform.rotation.eulerAngles.z) * line;
        Debug.DrawLine(transform.position, (Vector2)transform.position + rotatedLine);
        Vector2 rotatedLine2 = Quaternion.Euler(0, 0, -m_viewAngle/2 + transform.rotation.eulerAngles.z) * line;
        Debug.DrawLine(transform.position, (Vector2)transform.position + rotatedLine2);
    }

    public void SetViewRadius(float viewRadius)
    {
        m_viewRadius = viewRadius;
    }

    public float GetViewRadius()
    {
        return m_viewRadius;
    }

    public void SetViewAngle(float viewAngle)
    {
        m_viewAngle = viewAngle;
    }

    public float GetViewAngle()
    {
        return m_viewAngle;
    }
    
    public List<Transform> GetVisibleTargets()
    {
        return m_visibleTargets;
    }

    public bool GetIsTargetsClose()
    {
        return m_isTargetsClose;
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            FindVisibleTargetsClose();
        }
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool hit, Vector3 point, float distance, float angle)
        {
            this.hit = hit;
            this.point = point;
            this.distance = distance;
            this.angle = angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
        }
    }
}
