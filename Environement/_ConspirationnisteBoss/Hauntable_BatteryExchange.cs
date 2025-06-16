using System;
using System.Collections.Generic;
using UnityEngine;

public class Hauntable_BatteryExchange : HauntableObject, IDataPersistant
{
    public event EventHandler<EventArgs> OnBatteryMoving;
    public event EventHandler<EventArgs> OnBatteryIdle;
    public event EventHandler<EventArgs> OnBatteryPowerOn;
    public event EventHandler<EventArgs> OnBatteryPowerOff;

    private const string IS_BATTERY_MOVING = "isBatteryMove";
    private const string IS_BATTERY_IDLE = "isBatteryIdle";
    private const string IS_BATTERY_POWER = "isBatteryPower";

    [Header("List of objects to activate and deactivate")]
    [SerializeField] private List<GameObject> m_gameObjectsToActivateWhenRight;
    [SerializeField] private List<GameObject> m_gameObjectsToActivateWhenLeft;

    [Header("List of objects to handle electricity")]
    [SerializeField] private List<GameObject> m_electricGameObjectWhenRight;
    [SerializeField] private List<GameObject> m_electricGameObjectWhenLeft;

    [Header("List of objects to switch")]
    [SerializeField] private List<GameObject> m_switchGameObjectWhenRight;
    [SerializeField] private List<GameObject> m_switchGameObjectWhenLeft;
    [SerializeField] private List<GameObject> m_switchGameObjectWhenOffRight;
    [SerializeField] private List<GameObject> m_switchGameObjectWhenOffLeft;

    [Header("Position of battery sockets")]
    [SerializeField] private Transform m_transformBatterySocketRight;
    [SerializeField] private Transform m_transformBatterySocketLeft;
    [SerializeField] private float m_offsetToRemovePower = 2f;

    [Header("Speed and physics")]
    [SerializeField] private float m_maxSpeed;
    [SerializeField] private float m_acceleration;
    [SerializeField] private float m_deceleration;

    [Header("DataPersistant")]
    [SerializeField] private bool m_isDataPersistantActive;
    [ShowIf("m_isDataPersistantActive")]
    [SerializeField] private bool m_isSetAndSaveAfterMoveRight;
    [ShowIf("m_isDataPersistantActive")]
    [SerializeField] private bool m_isSetAndSaveAfterMoveLeft;
    [ShowIf("m_isDataPersistantActive")]
    [SerializeField] private string m_ID;

    private List<IHasElectricityRunning> m_hasElectricityRunningLeftList;
    private List<IHasElectricityRunning> m_hasElectricityRunningRightList;
    private List<ISwitchable> m_switchableLeftList;
    private List<ISwitchable> m_switchableRightList;
    private List<ISwitchable> m_switchableOffLeftList;
    private List<ISwitchable> m_switchableOffRightList;

    private Vector2 m_positionBatterySocketRight;
    private Vector2 m_positionBatterySocketLeft;

    private Animator m_animator;
    private bool m_isAnimationDone;
    private bool m_isBatteryIdle;
    private bool m_isBatteryMoving;
    private bool m_isBatteryPower;

    private Rigidbody2D m_rb;
    private int m_facingDirection;
    private bool m_isPowerRightActivate;
    private bool m_isPowerLeftActivate;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = Guid.NewGuid().ToString();
    }

    override protected void Awake()
    {
        base.Awake();
        if (string.IsNullOrEmpty(m_ID) && m_isDataPersistantActive)
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_rb = GetComponent<Rigidbody2D>();
        m_positionBatterySocketRight = m_transformBatterySocketRight.position;
        m_positionBatterySocketLeft = m_transformBatterySocketLeft.position;
        m_facingDirection = 1;
        m_isAnimationDone = true;

        m_hasElectricityRunningLeftList = new List<IHasElectricityRunning>();
        m_hasElectricityRunningRightList = new List<IHasElectricityRunning>();

        m_switchableLeftList = new List<ISwitchable>();
        m_switchableRightList = new List<ISwitchable>();
        m_switchableOffLeftList = new List<ISwitchable>();
        m_switchableOffRightList = new List<ISwitchable>();

        InitializeHasElectricityList(m_hasElectricityRunningLeftList, m_electricGameObjectWhenLeft);
        InitializeHasElectricityList(m_hasElectricityRunningRightList, m_electricGameObjectWhenRight);
        InitializeSwitchableList(m_switchableLeftList, m_switchGameObjectWhenLeft);
        InitializeSwitchableList(m_switchableRightList, m_switchGameObjectWhenRight);
        InitializeSwitchableList(m_switchableOffLeftList, m_switchGameObjectWhenOffLeft);
        InitializeSwitchableList(m_switchableOffRightList, m_switchGameObjectWhenOffRight);
    }

    private void InitializeHasElectricityList(List<IHasElectricityRunning> listToPopulate, List<GameObject> listOfGameObjects)
    {
        foreach(GameObject hasElectricityRunningGameObject in listOfGameObjects)
        {
            IHasElectricityRunning hasElectricityRunning = hasElectricityRunningGameObject.GetComponent<IHasElectricityRunning>();
            if(hasElectricityRunning != null)
            {   
                listToPopulate.Add(hasElectricityRunning);
            }
            else
            {
                Debug.LogError("Object " + hasElectricityRunningGameObject.name + " has no IHasElectricityRunning interface");
            }
        }
    }

    private void InitializeSwitchableList(List<ISwitchable> listToPopulate, List<GameObject> listOfGameObjects)
    {
        foreach(GameObject switchableGameObject in listOfGameObjects)
        {
            ISwitchable switchable = switchableGameObject.GetComponent<ISwitchable>();
            if(switchable != null)
            {   
                listToPopulate.Add(switchable);
            }
            else
            {
                Debug.LogError("Object " + switchableGameObject.name + " has no ISwitchable interface");
            }
        }
    }

    public void LoadData(GameData data)
    {
        if(!m_isDataPersistantActive)
        {
            return;
        }
        Vector3 position;
        bool isThereData = data.position.TryGetValue(m_ID, out position);
        if(isThereData)
        {
            transform.position = position;
            NoLongerHauntable();
        }
    }

    public void SaveData(GameData data)
    {
        if(!m_isDataPersistantActive)
        {
            return;
        }
        if(data.position.ContainsKey(m_ID))
        {
            data.position.Remove(m_ID);
        }
        data.position.Add(m_ID, transform.position);
    }

    protected override void Start()
    {
        base.Start();
        m_animator = m_hauntableObjectAnimator.GetAnimator();
        if(CheckIfBatteryPositionPowerOn(Utils.Direction.Left) && !m_isPowerLeftActivate)
        {
            BatteryPowerOn(Utils.Direction.Left);
        }
        else if(CheckIfBatteryPositionPowerOn(Utils.Direction.Right) && !m_isPowerRightActivate)
        {
            BatteryPowerOn(Utils.Direction.Right);
        }
        else
        {
            BatteryPowerOff(Utils.Direction.Left);
            BatteryPowerOff(Utils.Direction.Right);
        }
        BatteryIdle();
    }

    protected override void Update()
    {
        base.Update();

        if(!m_isToProcessUpdate || m_isPlayerUnhaunting)
        {
            return;
        }

        HandleUpdateLogic();
    }

    private void HandleUpdateLogic()
    {
        if(m_isBatteryPower)
        {
            return;
        }

        if(CheckIfBatteryPositionPowerOn(Utils.Direction.Left) && !m_isPowerLeftActivate)
        {
            BatteryPower(Utils.Direction.Left);
        }
        else if(CheckIfBatteryPositionPowerOn(Utils.Direction.Right) && !m_isPowerRightActivate)
        {
            BatteryPower(Utils.Direction.Right);
        }
        else if(CheckIfBatteryPositionPowerOff(Utils.Direction.Left) && m_isPowerLeftActivate)
        {
            BatteryPowerOff(Utils.Direction.Left);
        }
        else if(CheckIfBatteryPositionPowerOff(Utils.Direction.Right) && m_isPowerRightActivate)
        {
            BatteryPowerOff(Utils.Direction.Right);
        }
        else if(Mathf.Abs(GameInput.Instance.xInput) > 0.3)
        {
            Utils.Direction direction = (Utils.Direction)(int)Mathf.Sign(GameInput.Instance.xInput);
            if(!CheckIfCanMoveDirection(direction))
            {
                if(!m_isBatteryPower)
                {
                    BatteryIdle();
                    return;
                }
            }
            if(!m_isBatteryMoving)
            {
                BatteryMoving();
            }
        }
        else
        {
            if(!m_isBatteryIdle)
            {
                BatteryIdle();
            }

        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(m_isToProcessUpdate && m_isBatteryMoving)
        {
            CheckIfFlip();
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        float horizontalInput = GameInput.Instance.xInput;
        Utils.Direction direction = (Utils.Direction)(int)Mathf.Sign(horizontalInput);
        if(!CheckIfCanMoveDirection(direction))
        {
            m_rb.velocity = new Vector2(0f, 0f);
            return;
        }

        float velPower = 0.9f;
        float targetSpeed = horizontalInput * m_maxSpeed;
        float speedDif = targetSpeed - m_rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? m_acceleration : m_deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        m_rb.AddForce(movement*Vector2.right*10); 
    }

    private bool CheckIfCanMoveDirection(Utils.Direction direction)
    {
        if(direction == Utils.Direction.Left)
        {
            return !(Mathf.Abs(transform.position.x - m_positionBatterySocketLeft.x) < 0.1f);
        }
        else
        {
            return !(Mathf.Abs(transform.position.x - m_positionBatterySocketRight.x) < 0.1f);
        }
    }

    public void CheckIfFlip()
    {
        float xInput = GameInput.Instance.xInput;
        if(xInput != 0 && Mathf.Sign(xInput) != m_facingDirection)
        {
            Flip();
            CameraFollowObject.Instance.CallTurn();
        }
    }

    public void Flip()
    {
        m_facingDirection *= -1;
        m_rb.transform.Rotate(0.0f, 180.0f, 0.0f);
    }


    private void BatteryIdle()
    {
        OnBatteryIdle?.Invoke(this, EventArgs.Empty);
        m_isBatteryIdle = true;
        m_isBatteryMoving = false;
        m_isBatteryPower = false;
        SetAnimator();
    }

    private void BatteryMoving()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        OnBatteryMoving?.Invoke(this, EventArgs.Empty);
        m_isBatteryIdle = false;
        m_isBatteryMoving = true;
        m_isBatteryPower = false;
        SetAnimator();
    }

    private void BatteryPower(Utils.Direction direction)
    {
        Player.Instance.HauntingState.SetCanUnhaunt(false);
        OnBatteryPowerOn?.Invoke(this, EventArgs.Empty);
        m_isAnimationDone = false;
        m_isBatteryIdle = false;
        m_isBatteryMoving = false;
        m_isBatteryPower = true;
        BatteryPowerOn(direction);
        SetAnimator();
    }

    private void BatteryPowerAnimationDone()
    {
        m_isAnimationDone = true;
        Player.Instance.HauntingState.SetCanUnhaunt(true);
        BatteryIdle();
    }

    private void BatteryPowerOff(Utils.Direction direction)
    {
        if(direction == Utils.Direction.Left)
        {
            m_isPowerLeftActivate = false;
        }
        else
        {
            m_isPowerRightActivate = false;
        }
        OnBatteryPowerOff?.Invoke(this, EventArgs.Empty);
        HandlePowerOff(direction);
    }

    private void BatteryPowerOn(Utils.Direction direction)
    {
        if(direction == Utils.Direction.Left)
        {
            m_isPowerLeftActivate = true;
        }
        else
        {
            m_isPowerRightActivate = true;
        }
        HandlePowerOn(direction);
    }

    private void HandlePowerOn(Utils.Direction direction)
    {
        if(direction == Utils.Direction.Left)
        {
            foreach(GameObject gameObjectToHandle in m_gameObjectsToActivateWhenLeft)
            {
                gameObjectToHandle.SetActive(m_isPowerLeftActivate);
            }
            foreach(IHasElectricityRunning hasElectricityRunning in m_hasElectricityRunningLeftList)
            {
                //Context 2 is left
                hasElectricityRunning.SetElectricityRunning(Utils.ElectricalContext.CONTEXT_2, m_isPowerLeftActivate);
            }
            foreach(ISwitchable hasSwitchable in m_switchableLeftList)
            {
                hasSwitchable.Switch();
            }
            if(m_isSetAndSaveAfterMoveLeft)
            {
                NoLongerHauntable();
            }
        }
        else
        {
            foreach(GameObject gameObjectToHandle in m_gameObjectsToActivateWhenRight)
            {
                gameObjectToHandle.SetActive(m_isPowerRightActivate);
            }
            foreach(IHasElectricityRunning hasElectricityRunning in m_hasElectricityRunningRightList)
            {
                //Context 1 is right
                hasElectricityRunning.SetElectricityRunning(Utils.ElectricalContext.CONTEXT_1, m_isPowerRightActivate);
            }
            foreach(ISwitchable hasSwitchable in m_switchableRightList)
            {
                hasSwitchable.Switch();
            }
            if(m_isSetAndSaveAfterMoveRight)
            {
                NoLongerHauntable();
            }
        }
        if(m_isDataPersistantActive)
        {
            DataPersistantManager.Instance.SaveGame();
        }
    }

    private void HandlePowerOff(Utils.Direction direction)
    {
        if(direction == Utils.Direction.Left)
        {
            foreach(GameObject gameObjectToHandle in m_gameObjectsToActivateWhenLeft)
            {
                gameObjectToHandle.SetActive(m_isPowerLeftActivate);
            }
            foreach(IHasElectricityRunning hasElectricityRunning in m_hasElectricityRunningLeftList)
            {
                //Context 2 is left
                hasElectricityRunning.SetElectricityRunning(Utils.ElectricalContext.CONTEXT_2, m_isPowerLeftActivate);
            }
            foreach(ISwitchable switchable in m_switchableOffLeftList)
            {
                switchable.Switch();
            }
        }
        else
        {
            foreach(GameObject gameObjectToHandle in m_gameObjectsToActivateWhenRight)
            {
                gameObjectToHandle.SetActive(m_isPowerRightActivate);
            }
            foreach(IHasElectricityRunning hasElectricityRunning in m_hasElectricityRunningRightList)
            {
                //Context 1 is right
                hasElectricityRunning.SetElectricityRunning(Utils.ElectricalContext.CONTEXT_1, m_isPowerRightActivate);
            }
            foreach(ISwitchable switchable in m_switchableOffRightList)
            {
                switchable.Switch();
            }
        }
    }

    private bool CheckIfBatteryPositionPowerOn(Utils.Direction direction)
    {
        if(direction == Utils.Direction.Left)
        {
            return Mathf.Abs(transform.position.x - m_positionBatterySocketLeft.x) < 0.1f;
        }
        else
        {
            return Mathf.Abs(transform.position.x - m_positionBatterySocketRight.x) < 0.1f;
        }
    }

    private bool CheckIfBatteryPositionPowerOff(Utils.Direction direction)
    {
        if(direction == Utils.Direction.Left)
        {
            return Mathf.Abs(transform.position.x - m_positionBatterySocketLeft.x) > m_offsetToRemovePower;
        }
        else
        {
            return Mathf.Abs(transform.position.x - m_positionBatterySocketRight.x) > m_offsetToRemovePower;
        }
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_BATTERY_IDLE, m_isBatteryIdle);
        m_animator.SetBool(IS_BATTERY_MOVING, m_isBatteryMoving);
        m_animator.SetBool(IS_BATTERY_POWER, m_isBatteryPower);
    }

    public override void PlayerUnhauntStart()
    {
        base.PlayerUnhauntStart();
        BatteryIdle();
    }
}
