using UnityEngine;
using System;

public class WaterInContainer : MonoBehaviour
{
    [SerializeField] private float m_finalPositionY;
    [SerializeField] private float m_finalScaleY;
    [SerializeField] private float m_fillSpeed;
    [SerializeField] private bool m_testFill = false;
    [SerializeField] private bool m_testResetFill = false;

    private HasGameplayProgress m_hasGameplayProgress;
    private float m_startPositionY;
    private float m_startScaleY;

    private void Awake()
    {
        m_hasGameplayProgress = GetComponent<HasGameplayProgress>();
        m_startPositionY = transform.localPosition.y;
        m_startScaleY = transform.localScale.y;
    }

    private void Start()
    {
        AdjustHeight(m_hasGameplayProgress.GetProgress());
        m_hasGameplayProgress.OnProgressChanged += GameplayProgress_OnProgressChanged;
        m_hasGameplayProgress.OnProgressReset += GameplayProgress_OnProgressReset;
    }

    private void GameplayProgress_OnProgressChanged(object sender, HasGameplayProgress.OnProgressChangedEventArgs e)
    {
        AdjustHeight(e.progress);
    }

    private void GameplayProgress_OnProgressReset(object sender, EventArgs e)
    {
        transform.localPosition = new Vector2(transform.localPosition.x, m_startPositionY);
        transform.localScale = new Vector2(transform.localScale.x, m_startScaleY);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.WATER_LAYER)
        {
            m_hasGameplayProgress.UpdateProgress(m_fillSpeed*Time.deltaTime);
        }
    }

    private void Update()
    {
        if(m_testFill)
        {
            m_hasGameplayProgress.UpdateProgress(m_fillSpeed*Time.deltaTime);
        }
        if(m_testResetFill)
        {
            m_hasGameplayProgress.ResetProgress();
            m_testResetFill = false;
        }
    }

    private void AdjustHeight(float progress)
    {
        float positionFactor = Mathf.Abs(m_startPositionY-m_finalPositionY);
        float scaleFactor = Mathf.Abs(m_startScaleY-m_finalScaleY);
        transform.localScale = Vector2.MoveTowards(transform.localScale, new Vector2(transform.localScale.x, m_finalScaleY), scaleFactor*progress);
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, new Vector2(transform.localPosition.x, m_finalPositionY), positionFactor*progress);
    }
}
