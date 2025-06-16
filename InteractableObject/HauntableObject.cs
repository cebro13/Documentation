using System;
using Cinemachine;
using UnityEngine;

public class HauntableObject : MonoBehaviour
{
    public event EventHandler<EventArgs> OnHauntStart; 
    public event EventHandler<EventArgs> OnHauntCancel;
    public event EventHandler<EventArgs> OnUnhauntStart;
    public event EventHandler<EventArgs> OnUnhauntCancel;
    
    [Header("Dans le cas où virtual camera est à null, on set la caméra sur l'objet directement")]
    [SerializeField] private bool m_useSpecificCamera = false;
    [ShowIf("m_useSpecificCamera")]
    [SerializeField] protected CinemachineVirtualCamera m_virtualCamera;
    [SerializeField] protected HauntableObjectAnimator m_hauntableObjectAnimator;
    [SerializeField] private HauntableDetect m_hauntableDetect;
    [Header("Affiche les contrôles du hauntable object. Par défaut on utilise toujours le first.")]
    [SerializeField] private string m_firstControlText;
    [SerializeField] private string m_secondControlText;

    private CinemachineVirtualCamera m_previousCamera;

    protected bool m_isHaunted;
    protected bool m_isToProcessUpdate;
    protected bool m_isPlayerUnhaunting;
    private bool m_isUnhaunting;
    private bool m_isRotate;
    private bool m_canHaunt;

    private bool m_queueOutOfRange;
    private bool m_isCameraSwapping;

    protected virtual void Awake()
    {
        m_isHaunted = false;
        m_isUnhaunting = false;
        m_isRotate = false;
        m_canHaunt = false;
        m_queueOutOfRange = false;
        m_isCameraSwapping = false;
    }

    protected virtual void Start()
    {
        m_hauntableDetect.SetHauntableObject(this);
        Player.Instance.HauntingState.OnHauntCancel += Player_OnHauntCancel;
        Player.Instance.HauntingState.OnUnhauntStart += Player_OnUnhauntStart;
        Player.Instance.HauntingState.OnUnhauntCancel += Player_OnUnhauntCancel;
        Player.Instance.HauntingInObjectState.OnHauntCancel += Player_OnHauntCancel;
        Player.Instance.HauntingInObjectState.OnUnhauntStart += Player_OnUnhauntStart;
        Player.Instance.HauntingInObjectState.OnUnhauntCancel += Player_OnUnhauntCancel;
        CameraFollowObject.Instance.OnCameraSwapStart += CameraFollowObject_OnCameraSwapStart;
        CameraFollowObject.Instance.OnCameraSwapDone += CameraFollowObject_OnCameraSwapDone;

        m_hauntableDetect.OnObjectSelected += HauntableDetect_OnObjectSelected;
        m_hauntableDetect.OnObjectUnselected += HauntableDetect_OnObjectUnselected;

        m_hauntableDetect.OnPlayerInRange += HauntableDetect_OnPlayerInRange;
        m_hauntableDetect.OnPlayerNotInRange += HauntableDetect_OnPlayerNotInRange;
    }

    private void HauntableDetect_OnPlayerInRange(object sender, EventArgs e)
    {
        if(m_isHaunted && m_queueOutOfRange)
        {
            m_queueOutOfRange = false;
        }
        else
        {
            if(m_canHaunt)
            {
                return;
            }
            m_hauntableObjectAnimator.SetIsPlayerInRange(true);
            m_canHaunt = true;
        }
    }

    private void HauntableDetect_OnPlayerNotInRange(object sender, EventArgs e)
    {
        if(m_isCameraSwapping)
        {
            m_queueOutOfRange = true;
        }
        if(m_isHaunted)
        {
            m_queueOutOfRange = true;
        }
        else
        {
            if(!m_canHaunt)
            {
                return;
            }
            m_hauntableObjectAnimator.SetIsPlayerInRange(false);
            m_canHaunt = false;
        }

    }

    private void CameraFollowObject_OnCameraSwapStart(object sender, EventArgs e)
    {
        m_canHaunt = false;
        m_isCameraSwapping = true;
    }

    private void CameraFollowObject_OnCameraSwapDone(object sender, EventArgs e)
    {
        if(m_queueOutOfRange)
        {
            m_hauntableObjectAnimator.SetIsPlayerInRange(false);
            m_canHaunt = false;
            m_queueOutOfRange = false;
        }
        else
        {
            m_canHaunt = true;
        }
        m_isCameraSwapping = false;
    }

    private void Player_OnHauntCancel(object sender, System.EventArgs e)
    {
        if(m_isHaunted)
        {
            m_hauntableObjectAnimator.IdleState();
            m_isHaunted = false;
            OnHauntCancel?.Invoke(this, EventArgs.Empty);
            PlayerHauntCancel();
            HandleUI(false);

        }
    }

    private void Player_OnUnhauntStart(object sender, System.EventArgs e)
    {
        if(m_isHaunted)
        {
            OnUnhauntStart?.Invoke(this, EventArgs.Empty);
            m_hauntableObjectAnimator.UnhauntState();
            PlayerUnhauntStart();
        } 
    }

    private void Player_OnUnhauntCancel(object sender, System.EventArgs e)
    {
        if(m_isHaunted)
        {
            OnUnhauntCancel?.Invoke(this, EventArgs.Empty);
            m_hauntableObjectAnimator.HauntState();
            PlayerUnhauntCancel();
        } 
    }

    public virtual void HauntableDetect_OnObjectSelected(object sender, EventArgs e)
    {
        m_hauntableObjectAnimator.SelectedState();
    }

    public virtual void HauntableDetect_OnObjectUnselected(object sender, EventArgs e)
    {
        if(!m_isHaunted)
        {
            m_hauntableObjectAnimator.IdleState();
        }
    }

    public virtual void PlayerHauntCancel()
    {
        if(m_virtualCamera != null)
        {
            if(!m_useSpecificCamera)
            {
                Debug.LogError("Use Specific Camera doit être à true");
            }
            VCamManager.Instance.SwapCamera(m_previousCamera);
        }
    }

    public virtual void PlayerHauntStart()
    {
        m_isPlayerUnhaunting = false;
    }
    public virtual void PlayerUnhauntCancel()
    {
        m_isPlayerUnhaunting = false;
    }
    public virtual void PlayerUnhauntStart()
    {
        m_isPlayerUnhaunting = true;
    }
    public virtual void PlayerSelectedStart(){}
    public virtual void PlayerSelectedCancel(){}
    
    protected virtual void Update()
    {
        if(m_isHaunted)
        {
            m_isToProcessUpdate = !m_isUnhaunting;
        }
        else
        {
            m_isToProcessUpdate = false;
        }
    }

    protected virtual void FixedUpdate()
    {
        if(m_isHaunted)
        {
            m_isToProcessUpdate = !m_isUnhaunting;
        }
        else
        {
            m_isToProcessUpdate = false;
        }
    }

    protected virtual void SetIsRotate(bool isRotate)
    {
        m_isRotate = isRotate;
    }

    public bool IsCanHaunt()
    {
        return m_canHaunt;
    }

    public void Haunt()
    {
        m_isHaunted = true;
        m_isUnhaunting = false;
        m_hauntableObjectAnimator.HauntState();
        OnHauntStart?.Invoke(this, EventArgs.Empty);
        PlayerHauntStart();
        if(m_virtualCamera != null)
        {
            m_previousCamera = VCamManager.Instance.GetCurrentCamera();
            VCamManager.Instance.SwapCamera(m_virtualCamera);
        }
        else
        {
            VCamManager.Instance.SetCameraFollower(transform, false, m_isRotate);
        }
        HandleUI(true);
    }

    private void HandleUI(bool isActive)
    {
        if(isActive)
        {
            ControlInputUI.Instance.DisableUI();
            SmallFlagUI.Instance.DisableUI();
            if(!HauntableControlUI.Instance.GetIsShow())
            {
                HauntableControlUI.Instance.TriggerTextShow(m_firstControlText, m_secondControlText);
            }
        }
        else
        {
            ControlInputUI.Instance.EnableUI();
            SmallFlagUI.Instance.EnableUI();
            if(HauntableControlUI.Instance.GetIsShow())
            {
                HauntableControlUI.Instance.TriggerTextHide();
            }
        }
    }

    public void NoLongerHauntable()
    {
        m_hauntableObjectAnimator.SetNoLongerHauntable();
        m_hauntableDetect.SetNoLongerHauntable();
        if(Player.Instance.playerStateMachine.CurrentState == Player.Instance.HauntingState)
        {
            Player.Instance.HauntingState.ForceUnhaunt();
        }
    }
}
