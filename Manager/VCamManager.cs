using System.Collections;
using UnityEngine;
using Cinemachine;

public class VCamManager : MonoBehaviour
{
    public static VCamManager Instance{get; private set;}
    [SerializeField] private CinemachineVirtualCamera[] m_cinemachineVirtualCameras;
    [SerializeField] private Transform m_mainCameraTransform;
    [SerializeField] private CinemachineBrain m_cinemachineBrain;

    [SerializeField] private float m_fallSpeedYDampingChangeThreshold = -7f;
    [SerializeField] private float m_dampPanAmount = 0.25f;
    [SerializeField] private float m_fallBiasY = 1f;
    [SerializeField] private float m_fallPanTime = 0.35f;


    private bool m_isLerpingYDamping;
    private bool m_isLerpingFromPlayerFalling;
    private float m_lastSoftZoneHeight;

    private CinemachineVirtualCamera m_currentCamera;
    private CinemachineFramingTransposer m_framingTransposer;
    
    private float m_currentCameraYDamping;
    private float m_currentCameraYBias;
    private Vector2 m_startingTrackedObjectOffset;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Il existe plusieurs instance de VComManager");
        }
        Instance = this;

        if(m_cinemachineBrain == null)
        {
            Debug.LogError("Il faut ajouter la référence de CinemachineBrain de la Main Camera à l'objet VCamManager");
        }

        foreach(CinemachineVirtualCamera virtualCamera in m_cinemachineVirtualCameras)
        {
            if(virtualCamera.enabled)
            {
                m_currentCamera = virtualCamera;
            }
        }
        if(m_currentCamera != null)
        {
            m_framingTransposer = m_currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        
            //Set the YDamping amount so it's based on the inspector value
            m_currentCameraYDamping = m_framingTransposer.m_YDamping;
            m_currentCameraYBias = m_framingTransposer.m_BiasY;
        }

    }

    #region Lerp the Y damping
    public void LerpYDamping(bool isPlayerFalling)
    {
        StartCoroutine(LerpYAction(isPlayerFalling));
    }

    public IEnumerator LerpYAction(bool isPlayerFalling)
    {
        m_isLerpingYDamping = true;
        //Grab starting damping amount
        float startDampAmount = m_framingTransposer.m_YDamping;
        float endDampAmount;
        float startBiasYAmount = m_framingTransposer.m_BiasY;
        float endBiasYAmount;

        //Determine the end damping amount
        if(isPlayerFalling)
        {
            endDampAmount = m_dampPanAmount;
            endBiasYAmount = m_fallBiasY;
            m_isLerpingFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = m_currentCameraYDamping;
            endBiasYAmount = m_currentCameraYBias;
        } 

        //lerp the pan amount
        float elapsedTime = 0f;
        while(elapsedTime < m_fallPanTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedDampAmount = Mathf.Lerp(startDampAmount, endDampAmount, elapsedTime / m_fallPanTime);
            float lerpedBiasY = Mathf.Lerp(startBiasYAmount, endBiasYAmount, elapsedTime / m_fallPanTime);
            m_framingTransposer.m_YDamping = lerpedDampAmount;
            m_framingTransposer.m_BiasY = lerpedBiasY;
            yield return null;
        }
        m_isLerpingYDamping = false;
    }
    #endregion

    #region Pan Camera
    public void panCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }

    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Debug.Log("m_framingTransposer.m_TrackedObjectOffset " + m_framingTransposer.m_TrackedObjectOffset);
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;
        if(!panToStartingPos) //handleFromTrigger
        {
            m_lastSoftZoneHeight = m_framingTransposer.m_SoftZoneHeight;
            m_framingTransposer.m_SoftZoneHeight = 0f;
            switch(panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                case PanDirection.Left:
                    endPos = Vector2.left;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.right;
                    break;
                default:
                    break;
            }
            endPos *= panDistance;
            startingPos = m_currentCamera.transform.position;
            endPos += startingPos;
        }
        //handle the direction settings when moving back to the starting position
        else
        {
            startingPos = m_framingTransposer.m_TrackedObjectOffset;
            endPos = m_startingTrackedObjectOffset;
        }
        
        float elapsedTime = 0f;
        while(elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            m_framingTransposer.m_TrackedObjectOffset = panLerp;

            yield return null;
        }

        if(panToStartingPos)
        {
            m_framingTransposer.m_SoftZoneHeight = m_lastSoftZoneHeight;
        }
    }
    #endregion
    
    #region Swap Cameras
    public void SwapCamera(CinemachineVirtualCamera newCamera)
    {
        if(m_currentCamera != null)
        {
            m_currentCamera.enabled = false;
        }
        if(m_isLerpingFromPlayerFalling || m_isLerpingYDamping) //On fait en sorte que le damping est réintialisé si jamais on était entrain de lerp
        {
            m_framingTransposer.m_BiasY = m_currentCameraYBias;
            m_framingTransposer.m_YDamping = m_currentCameraYDamping;
        }
        newCamera.enabled = true;
        m_currentCamera = newCamera;
        m_framingTransposer = m_currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        m_currentCameraYDamping = m_framingTransposer.m_YDamping;
    }

    #endregion
    
    private void Start()
    {
        if(m_currentCamera != null)
        {
            m_currentCamera.Follow = CameraFollowObject.Instance.transform;
            m_startingTrackedObjectOffset = m_framingTransposer.m_TrackedObjectOffset;
        }
    }

    private void Update()
    {
        if(Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetVelocityY() < m_fallSpeedYDampingChangeThreshold && !m_isLerpingYDamping && !m_isLerpingFromPlayerFalling)
        {
            if(Player.Instance.playerStateMachine.CurrentState != Player.Instance.DashUnderState)
            {
                LerpYDamping(true);
            }
        }

        if(Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetVelocityY() >= 0f && !m_isLerpingYDamping && m_isLerpingFromPlayerFalling)
        {
            m_isLerpingFromPlayerFalling = false;
            LerpYDamping(false);
        }
    }

    public CinemachineVirtualCamera GetCurrentCamera()
    {
        return m_currentCamera;
    }

    public void SetCameraFollower(Transform newTransform, bool isInstantTransition, bool isRotate = false)
    {
        CameraFollowObject.Instance.SetTransformToFollow(newTransform, isInstantTransition, isRotate);
    }

    public Transform GetMainCameraTransform()
    {
        if(m_mainCameraTransform == null)
        {
            Debug.LogError("Il faut attacher l'objet 'MainCamera' à cet objet");
        }
        return m_mainCameraTransform;
    }

    public void SetCameraBlend(float blend)
    {
        m_cinemachineBrain.m_DefaultBlend.m_Time = blend;
    }

    public float GetCameraBlend()
    {
        return m_cinemachineBrain.m_DefaultBlend.m_Time;
    }
}
