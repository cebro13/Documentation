using UnityEngine;

public class ForestPit_FireflyRepellent : MonoBehaviour
{
    [SerializeField] private int m_id;
    [SerializeField] private float m_despawnTime = 10;

    [Header("Debug")]
    [SerializeField] private bool m_stayAliveInfinity;

    private float m_timeActivation;

    public void Initialize(int id, float despawnTime)
    {
        m_id = id;
        m_despawnTime = despawnTime;
    }

    private void Awake()
    {
        m_timeActivation = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out ForestPit_Firefly firefly))
        {
            firefly.DetachFromPlayer(m_id);
        }
    }

    private void Update()
    {
        if(m_stayAliveInfinity)
        {
            return;
        }

        if(Time.time > m_timeActivation + m_despawnTime)
        {
            Destroy(gameObject);
        }
    }


}
