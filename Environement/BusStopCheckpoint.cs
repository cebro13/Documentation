using System;
using UnityEngine;

public class BusStopCheckpoint : Checkpoint, ICanInteract
{
    public event EventHandler<EventArgs> OnIdle;
    public event EventHandler<EventArgs> OnDrivingIn;
    public event EventHandler<EventArgs> OnDrivingOut;
    public event EventHandler<OnDrivingInDoneEventArgs> OnDrivingInDone;
    public class OnDrivingInDoneEventArgs : EventArgs
    {
        public BusStopCheckpoint busStopCheckpoint;
    }

    private const string IS_DRIVING_OUT = "isDrivingOut";
    private const string IS_DRIVING_IN = "isDrivingIn";
    private const string IS_IDLE = "isIdle";
    [SerializeField] private BusStopUI.eBusStop m_thisBusStop;
    [SerializeField] private Transform m_moveToThisTransformInAnimation;
    [SerializeField] private Transform m_spawnPointDrivingIn;
    private Loader.Scene m_scene;
    private int m_checkpoint;

    private Animator m_animator;

    private bool m_isDrivingOut = false;
    private bool m_isDrivingIn = false;
    private bool m_isIdle = false;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();   
    }

    private void Start()
    {
        PlayerOverrideState.Instance.OnMoveToTransformStop += PlayerOverrideState_OnMoveToTransformStop;
        BusStopsUI.Instance.OnBusStopChosen += BusStopsUI_OnBusStopChosen;
    }

    private void PlayerOverrideState_OnMoveToTransformStop(object sender, EventArgs e)
    {
        if(PlayerOverrideState.Instance.GetObjectOverriding() != gameObject)
        {
            return;
        }
        PlayerOverrideState.Instance.SetObjectOverriding(null);
        DataPersistantManager.Instance.SetCheckpoint(m_checkpoint, m_scene);
        DataPersistantManager.Instance.SetIsBusDrivingIn(true);
        DataPersistantManager.Instance.SaveGame(gameObject);
        Player.Instance.playerStateMachine.ChangeState(Player.Instance.WaitForBusState);
        DrivingOut();
    }

    private void BusStopsUI_OnBusStopChosen(object sender, BusStopUI.OnBusStopClickEventArgs e)
    {
        
        if(BusStopsUI.Instance.GetCorrespondingSceneAndCheckpoint(e.busStop, out Loader.Scene scene, out int checkpoint))
        {
            HandlePlayerMovement();
            m_scene = scene;
            m_checkpoint = checkpoint;
        }
        else
        {
            Debug.LogError("Erreur pas normal");
            return;
        }
    }

    public void DrivingOut()
    {
        OnDrivingOut?.Invoke(this, EventArgs.Empty);
        SetAllAnimatorBoolFalse();
        m_isDrivingOut = true;
        SetAnimator();
    }

    public void DrivingOutAnimationDone()
    {
        Loader.Load(m_scene);
    }

    public void DrivingIn()
    {
        OnDrivingIn?.Invoke(this, EventArgs.Empty);
        SetAllAnimatorBoolFalse();
        m_isDrivingIn = true;
        SetAnimator();
    }

    public void DrivingInAnimationDone()
    {
        OnDrivingInDone?.Invoke(this, new OnDrivingInDoneEventArgs{busStopCheckpoint = this});
        Idle();
    }

    public void Idle()
    {
        OnIdle?.Invoke(this, EventArgs.Empty);
        SetAllAnimatorBoolFalse();
        m_isIdle = true;
        SetAnimator();
    }

    private void SetAllAnimatorBoolFalse()
    {
        m_isDrivingOut = false;
        m_isDrivingIn = false;
        m_isIdle = false;
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_DRIVING_OUT, m_isDrivingOut);
        m_animator.SetBool(IS_DRIVING_IN, m_isDrivingIn);
        m_animator.SetBool(IS_IDLE, m_isIdle);
    }

    private void HandlePlayerMovement()
    {
        PlayerOverrideState.Instance.SetObjectOverriding(gameObject);
        PlayerOverrideState.Instance.MoveToTransform(m_moveToThisTransformInAnimation);
    }

    public void Interact()
    {
        BusStopsUI.Instance.OpenBusStop(m_thisBusStop);
    }

    public Transform GetSpawnPoint()
    {
        return m_spawnPointDrivingIn;
    }
}
