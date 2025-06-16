using System;
using Unity.VisualScripting;
using UnityEngine;

public class HauntableFlower_FireflyRepellent : HauntableObject
{
    public event EventHandler<EventArgs> OnFire;
    public event EventHandler<EventArgs> OnChangeAngleStart;
    public event EventHandler<EventArgs> OnChangeAngleStop;
    public event EventHandler<EventArgs> OnIdle;

    private const string IS_FLOWER_FIRE = "isFlowerFire";
    private const string IS_FLOWER_CHANGING_ANGLE = "isFlowerChangingAngle";
    private const string IS_FLOWER_IDLE = "isFlowerIdle";
    private const string RECHARGE_TIME_FLOAT = "rechargeTime";

    [SerializeField] private float m_timerBeforeNextShot;
    [SerializeField] private GameObject m_flowerCanonGO;
    [SerializeField] private GameObject m_transformCanonSpawnPoint;
    [SerializeField] private float m_angleChangeSpeed;
    [Header("La différence entre les deux angles doit être strictement inférieure à 180")]
    [SerializeField] private float m_maxAngleRight;
    [SerializeField] private float m_maxAngleLeft;

    [SerializeField] private GameObject m_lobBombFireflyRepellentPrefab;
    [SerializeField] private float m_lobBombSpeed;
    [SerializeField] private int m_id;

    private Animator m_animator;
    private float m_activationTime;

    private bool m_isFlowerFire;
    private bool m_isFlowerChangingAngle;
    private bool m_isFlowerIdle;
    
    private Utils.Direction m_currentDirection;

    protected override void Awake()
    {
        base.Awake();
        m_isFlowerFire = false;
        m_isFlowerChangingAngle = false;
        m_isFlowerIdle = false;
        m_activationTime = Time.time;

        float currentAngle = Mathf.Repeat(m_flowerCanonGO.transform.eulerAngles.z, 360f);
        float maxAngleRight = Mathf.Repeat(m_maxAngleRight, 360f);
        float maxAngleLeft = Mathf.Repeat(m_maxAngleLeft, 360f);
    
        // Check if the current angle is within the valid range
        if (!IsAngleWithinRange(currentAngle, maxAngleRight, maxAngleLeft))
        {
            // Snap to the closest boundary
            float closestAngle = GetClosestBoundary(currentAngle, maxAngleRight, maxAngleLeft);
            m_flowerCanonGO.transform.rotation = Quaternion.Euler(0, 0, closestAngle);
        }
    }

    private float GetClosestBoundary(float angle, float angleMin, float angleMax)
    {
        float distanceToRight = Mathf.Abs(Mathf.DeltaAngle(angle, angleMin));
        float distanceToLeft = Mathf.Abs(Mathf.DeltaAngle(angle, angleMax));
    
        // Return the closest boundary
        return distanceToRight < distanceToLeft ? angleMin : angleMax;
    }

    private bool IsAngleWithinRange(float angle, float angleMin, float angleMax)
    {
        if (angleMin < angleMax)
        {
            // Range does not wrap around 360 degrees
            return angle >= angleMin && angle <= angleMax;
        }
        else
        {
            // Range wraps around 360 degrees
            return angle >= angleMin || angle <= angleMax;
        }
    }

    protected override void Start()
    {
        base.Start();
        m_animator = m_hauntableObjectAnimator.GetAnimator();
        FlowerIdle();
    }

    protected override void Update()
    {
        base.Update();

        HandleTimeRecharge();

        if(!m_isToProcessUpdate)
        {
            if(m_isFlowerChangingAngle)
            {
                FlowerIdle();
            }
            return;
        }

        HandleUpdateLogic();

        if(m_isFlowerFire)
        {
            HandleFlowerFire();
        }
        else if(m_isFlowerChangingAngle)
        {
            HandleFlowerChangingAngle();
        }
        else if(m_isFlowerIdle)
        {
            HandleFlowerIdle();
        }
        else
        {
            Debug.LogError("Ne devrait pas être dans ce cas.");
        }

    }

    private void HandleTimeRecharge()
    {
        float timer = (m_timerBeforeNextShot - (Time.time - m_activationTime))/m_timerBeforeNextShot;
        if(timer < 0)
        {
            timer = 0;
        }
        m_animator.SetFloat(RECHARGE_TIME_FLOAT, timer);
    }

    private void HandleUpdateLogic()
    {
        if(GameInput.Instance.interactInput)
        {
            if(m_activationTime + m_timerBeforeNextShot > Time.time)
            {
                return;
            }
            FlowerFire();
        }
        else if(GameInput.Instance.xInput < -0.1f || GameInput.Instance.xInput > 0.1f)
        {
            FlowerChangingAngle();
        }
        else
        {
            FlowerIdle();
        }
    }

    private void HandleFlowerFire()
    {
        if(HandleFlowerFireTimer())
        {
            FlowerIdle();
        }
    }

    private bool HandleFlowerFireTimer()
    {
        return false;
    }

    private void HandleFlowerChangingAngle()
    {
        float xInput = GameInput.Instance.xInput;
        float targetAngle = 0f;
        // Determine direction based on input
        if (xInput < -0.1f)
        {
            m_currentDirection = Utils.Direction.Left;
            
            targetAngle = m_maxAngleLeft;
        }
        else if (xInput > 0.1f)
        {
            m_currentDirection = Utils.Direction.Right;
            targetAngle = m_maxAngleRight;
        }

        RotateFlower(targetAngle);
    }

    private void RotateFlower(float targetAngle)
    {
        float currentAngle = Mathf.Repeat(m_flowerCanonGO.transform.eulerAngles.z, 360f);
        targetAngle = Mathf.Repeat(targetAngle, 360f);

        if (m_currentDirection == Utils.Direction.Left && targetAngle > currentAngle)
        {
            targetAngle -= 360f; // Force counterclockwise rotation
        }
        else if (m_currentDirection == Utils.Direction.Right && targetAngle < currentAngle)
        {
            targetAngle += 360f; // Force clockwise rotation
        }
        // Smoothly rotate towards the target angle
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, m_angleChangeSpeed * Time.deltaTime);
        m_flowerCanonGO.transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }


    private void HandleFlowerIdle()
    {
        
    }

    private void FlowerIdle()
    {
        if(!m_hauntableObjectAnimator.IsAnimationDone())
        {
            return;
        }

        m_isFlowerFire = false;
        m_isFlowerChangingAngle = false;
        m_isFlowerIdle = true;

        OnChangeAngleStop?.Invoke(this, EventArgs.Empty);
        OnIdle?.Invoke(this, EventArgs.Empty);

        SetAnimator();
    }

    private void FlowerChangingAngle()
    {
        if(!m_hauntableObjectAnimator.IsAnimationDone())
        {
            return;
        }

        m_isFlowerFire = false;
        m_isFlowerChangingAngle = true;
        m_isFlowerIdle = false;

        OnChangeAngleStart?.Invoke(this, EventArgs.Empty);

        SetAnimator();
    }

    private void FlowerFire()
    {
        if(!m_hauntableObjectAnimator.IsAnimationDone())
        {
            return;
        }
        OnChangeAngleStop?.Invoke(this, EventArgs.Empty);
        m_isFlowerFire = true;
        m_isFlowerChangingAngle = false;
        m_isFlowerIdle = false;

        m_hauntableObjectAnimator.AnimationStart();
        SetAnimator();
    }
    //Animator call
    private void Fire()
    {
        m_activationTime = Time.time;
        OnFire?.Invoke(this, EventArgs.Empty);
        float cannonAngle = Mathf.Repeat(m_flowerCanonGO.transform.eulerAngles.z, 360f);
        Utils.Direction direction = cannonAngle < 180f ? Utils.Direction.Left : Utils.Direction.Right;
        GameObject lobBomb = Instantiate(m_lobBombFireflyRepellentPrefab, m_transformCanonSpawnPoint.transform.position, Quaternion.identity);
        float startAngle = Mathf.Abs(Mathf.DeltaAngle(0, cannonAngle)); // Get the absolute angle relative to 0
        if(direction == Utils.Direction.Left)
        {
            startAngle = 90 - startAngle;
        }
        else
        {
            startAngle = 90 - startAngle;
        }
        lobBomb.GetComponent<LobBomb_FireflyRepellent>().InitializeWithSpeed(startAngle, m_lobBombSpeed, direction);
        lobBomb.GetComponent<LobBomb_FireflyRepellent>().InitializeId(m_id);
    }

    //Animator call
    private void FlowerFireDone()
    {
        m_hauntableObjectAnimator.AnimationDone();
        FlowerIdle();
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_FLOWER_FIRE, m_isFlowerFire);
        m_animator.SetBool(IS_FLOWER_CHANGING_ANGLE, m_isFlowerChangingAngle);
        m_animator.SetBool(IS_FLOWER_IDLE, m_isFlowerIdle);
    }
}
