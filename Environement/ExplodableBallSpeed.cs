using Cinemachine;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class ExplodableBallSpeed : MonoBehaviour, IExplodable
{
    public event EventHandler<EventArgs> OnExplode;

    [Header("Functionnality")]
    [SerializeField] private float m_maxSqrSpeedDifferenceForExplode = 70f;
    [Header("Effects")]
    [SerializeField] private GameObject m_explosionFX;
    [SerializeField] private ScreenShakeProfilerRefSO m_screenShakeProfilerSmallRefSO;
    
    private CinemachineImpulseSource m_impulseSource;
    private Rigidbody2D m_rb;
    private float m_sqrLastVelocity;


    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_impulseSource = GetComponent<CinemachineImpulseSource>();
        m_sqrLastVelocity = 0f;
    }

    private void FixedUpdate()
    {
        float sqrCurrentVelocity = Vector2.SqrMagnitude(m_rb.velocity);
        if(m_sqrLastVelocity > sqrCurrentVelocity)
        {
            if(m_sqrLastVelocity - sqrCurrentVelocity > m_maxSqrSpeedDifferenceForExplode)
            {
                Explode();
            }
        }
        m_sqrLastVelocity = sqrCurrentVelocity;
    }

    public void Explode()
    {
        OnExplode?.Invoke(this, EventArgs.Empty);
        Instantiate(m_explosionFX, transform.position, m_explosionFX.transform.rotation);
        CameraShakeManager.Instance.CameraExplosionFromProfile(m_screenShakeProfilerSmallRefSO, m_impulseSource);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            Explode();
            Player.Instance.Core.GetCoreComponent<PlayerStats>().TriggerOnDeathEvent();
        }
        if(collider.gameObject.TryGetComponent(out IExplodable explodable))
        {
            Explode();
        }
    }

}
