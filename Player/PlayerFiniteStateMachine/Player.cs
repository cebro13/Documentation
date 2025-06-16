using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public event EventHandler<EventArgs> OnColorChangedBack;

    //CONSTANTS
    #region Animation constants
    private const string IS_DASHING_UNDER = "IsDashingUnder";
    private const string IS_DASHING_UNDER_END = "IsDashingUnderEnd";
    private const string IS_IDLE = "IsIdle";
    private const string IS_MOVE = "IsMove";
    private const string IS_IN_AIR = "IsInAir";
    private const string IS_LAND = "IsLand";
    private const string IS_LOOK_FOR = "IsLookFor";
    private const string IS_HAUNTING = "IsHaunting";
    private const string IS_HAUNTING_IN_OBJECT = "IsHauntingInObject";
    private const string IS_FLY = "IsFly";
    private const string IS_HIDDEN = "IsHidden";
    private const string IS_LOOK_FOR_FEAR = "IsLookForFear";
    private const string IS_FOLLOW_WAYPOINTS = "IsFollowWaypoints";
    private const string IS_PUSH = "IsPush";
    private const string IS_CLIMB_STAIRS = "IsClimbStairs";
    private const string IS_DEAD = "IsDead";
    private const string IS_CLIMB_LADDER = "IsClimbLadder";
    private const string IS_SWITCH_LEVER = "IsSwitchLever";
    private const string IS_CINEMATIC = "IsCinematic";
    private const string IS_HEALING = "IsHealing";
    private const string IS_NEW_FOUND_KNOWLEDGE = "IsNewFoundKnowledge";
    private const string IS_NEW_POWER_UP = "IsNewPowerUp";
    private const string IS_NEW_ITEM = "IsNewItem";
    private const string IS_DAMAGED = "IsDamaged";
    private const string IS_WAIT_FOR_BUS = "IsWaitForBus";
    private const string IS_FEAR_SUCCESS = "IsFearSuccess";
    private const string IS_FEAR_FAILURE = "IsFearFailure";
    private const string IS_FEARING = "IsFearing";
    private const string IS_NO_MANA = "IsNoMana";
    private const string IS_MANING = "IsManing";
    #endregion

    //TODO create new class on the side for this.
    public const int DEFAULT_LAYER = 0;
    public const int HAUNTABLE_OBJECT_LAYER = 7;
    public const int PLAYER_HITBOX_LAYER = 3;
    public const int WATER_LAYER = 4;
    public const int ENEMY_LAYER = 14;
    public const int PLAYER_LAYER = 12;
    public const int GROUND_LAYER = 6;
    public const int STAIRS_LAYER = 16;
    public const int RESET_GAME_OBJECT_LAYER = 20;
    public const int GRABBABLE_LAYER = 19;
    public const int TWO_WAY_PLATFORM_LAYER = 17;
    public const int INTERACTABLE_NPC_LAYER = 22;
    public const int FEAR_LAYER = 24;

    //SINGLETON
    public static Player Instance { get; private set; }

    //STATEMACHINE
    #region StateMachine
    public PlayerStateMachine playerStateMachine {get; private set;}
    public PlayerIdleState IdleState {get; private set;}
    public PlayerMoveState MoveState {get; private set;}
    public PlayerJumpState JumpState {get; private set;}
    public PlayerInAirState InAirState {get; private set;}
    public PlayerLandState LandState {get; private set;}
    public PlayerDashUnderState DashUnderState {get; private set;}
    public PlayerDashUnderEndState DashUnderEndState {get; private set;}
    public PlayerLookForHauntState LookForHauntState {get; private set;}
    public PlayerLookForCluesState LookForCluesState {get; private set;}
    public PlayerHauntingState HauntingState {get; private set;}
    public PlayerHauntingInObjectState HauntingInObjectState {get; private set;}
    public PlayerFlyState FlyState {get; private set;}
    public PlayerHiddenState HiddenState {get; private set;}
    public PlayerLookForFearState LookForFearState {get; private set;}
    public PlayerFollowWaypointsState FollowWayPointsState {get; private set;}
    public PlayerPushState PushState {get; private set;}
    public PlayerDeadState DeadState{get; private set;}
    public PlayerClimbStairsState ClimbStairsState{get; private set;}
    public PlayerClimbLadderState ClimbLadderState{get; private set;}
    public PlayerSwitchLeverState SwitchLeverState{get; private set;}
    public PlayerCinematicState CinematicState{get; private set;}
    public PlayerDoNothingState DoNothingState{get; private set;}
    public PlayerHealingState HealingState{get; private set;}
    public PlayerNewFoundKnowledgeState NewFoundKnowledgeState{get; private set;}
    public PlayerNewPowerUpState NewPowerUpState {get; private set;}
    public PlayerNewItemState NewItemState {get; private set;}
    public PlayerDamagedState DamagedState {get; private set;}
    public PlayerWaitForBusState WaitForBusState {get; private set;}
    public PlayerFearFailureState FearFailureState {get; private set;}
    public PlayerFearSuccessState FearSuccessState {get; private set;}
    public PlayerFearingState FearingState {get; private set;}
    public PlayerNoManaState NoManaState {get; private set;}
    public PlayerManingState ManingState {get; private set;}
    #endregion

    public Core Core { get; private set; }

    //COMPONENT SERIALIZED
    [SerializeField] private PlayerData m_playerData;
    [SerializeField] private SpriteRenderer m_playerSprite;

    //LayerMask
    [SerializeField] private LayerMask m_HauntableObjectLayerMask;
    [SerializeField] private LayerMask m_playerHitboxLayerMask;
    [SerializeField] private LayerMask m_playerLayerMask;

    //Components
    [SerializeField] private GameObject m_hauntDirectionIndicatorGameObject;
    [SerializeField] private GameObject m_lookForCluesGameObject;
    [SerializeField] private GameObject m_lookForFearGameObject;
    [SerializeField] private float m_maxDelayBeforeColorBack = 10f;
    [SerializeField] private CustomColor m_customColor;

    [Header("Debugging")]
    [SerializeField] public bool m_hasAllPower = true;

    //private Color m_currentColor;

    private Rigidbody2D m_rb;
    private CapsuleCollider2D m_collider;
    private Coroutine m_coroutineGoingBackToWhite;
   
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        Core = GetComponentInChildren<Core>();

        playerStateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(playerStateMachine, m_playerData, IS_IDLE);
        MoveState = new PlayerMoveState(playerStateMachine, m_playerData, IS_MOVE);
        JumpState = new PlayerJumpState(playerStateMachine, m_playerData, IS_IN_AIR);
        InAirState = new PlayerInAirState(playerStateMachine, m_playerData, IS_IN_AIR);
        LandState = new PlayerLandState(playerStateMachine, m_playerData, IS_LAND);
        DashUnderState = new PlayerDashUnderState(playerStateMachine, m_playerData, IS_DASHING_UNDER);
        DashUnderEndState = new PlayerDashUnderEndState(playerStateMachine, m_playerData, IS_DASHING_UNDER_END);
        LookForHauntState = new PlayerLookForHauntState(playerStateMachine, m_playerData, IS_LOOK_FOR);
        LookForCluesState = new PlayerLookForCluesState(playerStateMachine, m_playerData, IS_LOOK_FOR);
        HauntingState = new PlayerHauntingState(playerStateMachine, m_playerData, IS_HAUNTING);
        HauntingInObjectState = new PlayerHauntingInObjectState(playerStateMachine, m_playerData, IS_HAUNTING_IN_OBJECT);
        FlyState = new PlayerFlyState(playerStateMachine, m_playerData, IS_FLY);
        HiddenState = new PlayerHiddenState(playerStateMachine, m_playerData, IS_HIDDEN);
        LookForFearState = new PlayerLookForFearState(playerStateMachine, m_playerData, IS_LOOK_FOR_FEAR);
        FollowWayPointsState = new PlayerFollowWaypointsState(playerStateMachine, m_playerData, IS_FOLLOW_WAYPOINTS);
        PushState = new PlayerPushState(playerStateMachine, m_playerData, IS_PUSH);
        ClimbStairsState = new PlayerClimbStairsState(playerStateMachine, m_playerData, IS_CLIMB_STAIRS);
        DeadState = new PlayerDeadState(playerStateMachine, m_playerData, IS_DEAD);
        ClimbLadderState = new PlayerClimbLadderState(playerStateMachine, m_playerData, IS_CLIMB_LADDER);
        SwitchLeverState = new PlayerSwitchLeverState(playerStateMachine, m_playerData, IS_SWITCH_LEVER);
        CinematicState = new PlayerCinematicState(playerStateMachine, m_playerData, IS_CINEMATIC);
        DoNothingState = new PlayerDoNothingState(playerStateMachine, m_playerData, IS_IDLE);
        HealingState = new PlayerHealingState(playerStateMachine, m_playerData, IS_HEALING);
        NewFoundKnowledgeState = new PlayerNewFoundKnowledgeState(playerStateMachine, m_playerData, IS_NEW_FOUND_KNOWLEDGE);
        NewPowerUpState = new PlayerNewPowerUpState(playerStateMachine, m_playerData, IS_NEW_POWER_UP);
        NewItemState = new PlayerNewItemState(playerStateMachine, m_playerData, IS_NEW_ITEM);
        DamagedState = new PlayerDamagedState(playerStateMachine, m_playerData, IS_DAMAGED);
        WaitForBusState = new PlayerWaitForBusState(playerStateMachine, m_playerData, IS_WAIT_FOR_BUS);
        FearFailureState = new PlayerFearFailureState(playerStateMachine, m_playerData, IS_FEAR_FAILURE);
        FearSuccessState = new PlayerFearSuccessState(playerStateMachine, m_playerData, IS_FEAR_SUCCESS);
        FearingState = new PlayerFearingState(playerStateMachine, m_playerData, IS_FEARING);
        NoManaState = new PlayerNoManaState(playerStateMachine, m_playerData, IS_NO_MANA);
        ManingState = new PlayerManingState(playerStateMachine, m_playerData, IS_MANING);
        m_rb = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<CapsuleCollider2D>();
    }
    
    private void Start()
    {
        SetCameraOnPlayer(true);
        RespawnAtCheckPoint();
    }

    private void Update()
    {
        Core.LogicUpdate();
        playerStateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        playerStateMachine.CurrentState.PhysicsUpdate();
    }

    public void SetHauntDirectionIndicatorAngle(float angle)
    {
        m_hauntDirectionIndicatorGameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void SetColliderSize(Vector2 size)
    {
        m_collider.size = size;
    }

    public void SetBoxColOffset(Vector2 offset)
    {
        m_collider.offset = offset;
    }

    public void ShowHauntDirectionIndicator()
    {
        m_hauntDirectionIndicatorGameObject.SetActive(true);
    }

    public void ShowLookForCluesVisual()
    {
        m_lookForCluesGameObject.SetActive(true);
    }

    public void ShowLookForFearVisual()
    {
        m_lookForFearGameObject.SetActive(true);
    }

    public void SetCameraOnPlayer(bool isInstantTransition)
    {
        VCamManager.Instance.SetCameraFollower(transform, isInstantTransition);
    }

    public void SetColliderSizeOffsetDashUnderEndToCurrent()
    {
        DashUnderEndState.SetColliderSizeOffset(m_collider.size, m_collider.offset);
    }

    public GameObject GetLookForCluesGameObject()
    {
        return m_lookForCluesGameObject;
    }

    public GameObject GetLookForFearGameObject()
    {
        return m_lookForFearGameObject;
    }

    public void SetRigidBodyStatic()
    {
        m_rb.bodyType = RigidbodyType2D.Static;
    }

    public void SetRigidBodyDynamic()
    {
        m_rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void DisablePlayerHauntingInObject()
    {
       m_rb.bodyType = RigidbodyType2D.Kinematic;
       m_collider.enabled = false;
    }

    public void EnablePlayerHauntingInObject()
    {
        m_rb.bodyType = RigidbodyType2D.Dynamic;
        //There is a delay to avoid the weird interaction where both col box are at the same spot.
        float delay = 0.2f;
        StartCoroutine(EnableColWithHauntedObjectAfterDelay(delay));
        m_collider.enabled = true;
    }

    public void HideHauntDirectionIndicator()
    {
        m_hauntDirectionIndicatorGameObject.SetActive(false);
    }

    public void HideLookForCluesVisual()
    {
        m_lookForCluesGameObject.SetActive(false);
    }

    public void HideLookForFearVisual()
    {
        m_lookForFearGameObject.SetActive(false);
    }

    private void AnimationTrigger()
    {
        playerStateMachine.CurrentState.AnimationTrigger();
    }

    private void AnimationFinishedTrigger()
    {
        playerStateMachine.CurrentState.AnimationFinishedTrigger();
    }

    public void RespawnAtCheckPoint()
    {
        if(CheckpointManager.Instance.GetActiveCheckpoint().gameObject.TryGetComponent(out BusStopCheckpoint busStopCheckpoint) && DataPersistantManager.Instance.IsBusDrivingIn())
        {
            busStopCheckpoint.DrivingIn();
            transform.position = busStopCheckpoint.GetSpawnPoint().position;
            playerStateMachine.Initialize(WaitForBusState);
            busStopCheckpoint.OnDrivingInDone += BusStopCheckpoint_OnDrivingInDone;
            DataPersistantManager.Instance.SetIsBusDrivingIn(false);
            DataPersistantManager.Instance.SaveGame(gameObject);
        }
        else
        {
            transform.position = CheckpointManager.Instance.GetActiveCheckpoint().transform.position;
            playerStateMachine.Initialize(IdleState);
        }
    }

    private void BusStopCheckpoint_OnDrivingInDone(object sender, BusStopCheckpoint.OnDrivingInDoneEventArgs e)
    {
        playerStateMachine.ChangeState(IdleState);
        e.busStopCheckpoint.OnDrivingInDone -= BusStopCheckpoint_OnDrivingInDone;
        PlayerOverrideState.Instance.OnMoveToTransformStop += PlayerOverrideState_OnMoveToTransformStop;
        PlayerOverrideState.Instance.SetObjectOverriding(gameObject);
        PlayerOverrideState.Instance.MoveToTransform(e.busStopCheckpoint.transform);
    }

    private void PlayerOverrideState_OnMoveToTransformStop(object sender, EventArgs e)
    {
        if(PlayerOverrideState.Instance.GetObjectOverriding() != gameObject)
        {
            return;
        }
        PlayerOverrideState.Instance.SetObjectOverriding(null);
    }

    public LayerMask GetPlayerLayerMask()
    {
        return m_playerLayerMask;
    }

    public LayerMask GetPlayerHitboxLayerMask()
    {
        return m_playerHitboxLayerMask;
    }

    public RaycastHit2D FindNewSelectedHauntedObjectRaycast(float angle)
    {
        Vector2 vectorToLookAtHauntingObject = (Vector2)(Quaternion.Euler(0,0,angle) * Vector2.right); 
        RaycastHit2D raycast = Physics2D.Raycast(m_collider.bounds.center, vectorToLookAtHauntingObject, m_playerData.lookingForHauntMaxDistance, m_HauntableObjectLayerMask);
        return raycast;
    }

    private IEnumerator EnableColWithHauntedObjectAfterDelay(float delay)
    {
        Physics2D.IgnoreLayerCollision(PLAYER_LAYER, HAUNTABLE_OBJECT_LAYER, true);
        yield return new WaitForSeconds(delay);
        Physics2D.IgnoreLayerCollision(PLAYER_LAYER, HAUNTABLE_OBJECT_LAYER, false);
    }

    public CustomColor GetPlayerColor()
    {
        //TODO NB REMOVE
        return m_customColor;
    }

    public CustomColor.colorIndex GetPlayerColorIdx()
    {
        return m_customColor.index;
    }

    public float GetPlayerFloatHeight()
    {
        return m_playerData.floatHeight;
    }

    public int GetPlayerIncreaseHauntDistancePerUpgrade()
    {
        return m_playerData.increaseDistancePerUpgrade;
    }

    public void SetMaxDelayBeforeColorBack(float delay)
    {
        m_maxDelayBeforeColorBack = delay;
    }

    public float GetMaxDelayBeforeColorBack()
    {
        return m_maxDelayBeforeColorBack;
    }

    public GameObject InstantiateFearMaterialization(GameObject fearMaterialization)
    {
        return Instantiate(fearMaterialization, transform.position, Quaternion.identity);
    }

    public void SetPlayerColor(CustomColor customColor, bool isLightColor = false)
    {
        if (m_coroutineGoingBackToWhite != null)
        {
            StopCoroutine(m_coroutineGoingBackToWhite);
        }
        m_coroutineGoingBackToWhite = StartCoroutine(ColorGoingBackToWhite(m_maxDelayBeforeColorBack));
        if (m_customColor.index == customColor.index)
        {
            return;
        }
        m_customColor.index = customColor.index;
        PlayerAnimator.Instance.SetTriggerColor(m_customColor.index, isLightColor);
    }

    private IEnumerator ColorGoingBackToWhite(float delay)
    {
        yield return new WaitForSeconds(delay);
        m_customColor.index = CustomColor.colorIndex.BLANK;
        PlayerAnimator.Instance.SetTriggerColor(m_customColor.index);
        OnColorChangedBack?.Invoke(this, EventArgs.Empty);
        m_coroutineGoingBackToWhite = null;
    }
}