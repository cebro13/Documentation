using System.Collections;
using UnityEngine;
using Cinemachine;
using System;

public class CameraFollowObject : MonoBehaviour
{
    public event EventHandler<EventArgs> OnCameraSwapStart;
    public event EventHandler<EventArgs> OnCameraSwapDone;
    //SINGLETON
    public static CameraFollowObject Instance {get; private set;}

    [Header("Flip Rotation Stats")]
    [SerializeField] private float m_flipRotationTime = 0.5f;
    [SerializeField] private float m_speedToSwapFollowObject = 5f;
    [SerializeField] private float m_speedToFollowPlayerAfterDash = 5f;
    private Transform m_transformToFollow;
    private bool m_isSwitchingFollowObject;
    private bool m_isRotate;
    private bool m_wasDashing;

    private int m_facingDirection;

    private void Awake()
    {
        Instance = this;
        m_isRotate = false;
        m_isSwitchingFollowObject = false;
        m_wasDashing = false;
    }

    private void LateUpdate()
    {
        if(m_isSwitchingFollowObject)
        {
            return;
        }
        
        if (Player.Instance.playerStateMachine.CurrentState == Player.Instance.DashUnderState)
        {
            transform.position = new Vector2(m_transformToFollow.position.x, transform.position.y);
            m_wasDashing = true;
        }
        else if (m_wasDashing)
        {
            if (Vector2.Distance(transform.position, m_transformToFollow.position) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, m_transformToFollow.position, m_speedToFollowPlayerAfterDash * Time.deltaTime);
            }
            else
            {
                m_wasDashing = false;
            }
        }

        if (!m_wasDashing)
        {
            transform.position = m_transformToFollow.position;
        }

        if(m_isRotate)
        {
            CinemachineVirtualCamera camera =  VCamManager.Instance.GetCurrentCamera();
            camera.transform.rotation = m_transformToFollow.rotation;
        }
    }

    public void SetTransformToFollow(Transform newTransform, bool isInstantTransition, bool isRotate = false)
    {
        if(isInstantTransition)
        {
            m_transformToFollow = newTransform;
        }
        else
        {
            StartCoroutine(SwitchCameraFollowerInTime(newTransform, 1f, isRotate));
        }
    }

    public void CallTurn()
    {
        StartCoroutine(FlipYLerp());
    }
    
    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation;

        float elapsedTime = 0f;
        while(elapsedTime < m_flipRotationTime)
        {
            elapsedTime += Time.deltaTime;
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / m_flipRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            yield return null;
        }
    }
    
    private float DetermineEndRotation()
    {

        //TODO: Generalize this
        m_facingDirection = Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetFacingDirection();
        if(m_transformToFollow.TryGetComponent(out TriangleHauntableObject triangleHauntableObject))
        {
            m_facingDirection = triangleHauntableObject.GetFacingDirection();
        }


        if(m_facingDirection == -1)
        {
            return 180f;
        }
        else
        {
            return 0f;
        }
    }

    public void SetIsRotateFalse()
    {
        m_isRotate = false;
    }

    private IEnumerator SwitchCameraFollowerInTime(Transform newTransform, float time, bool isRotate)
    {
        OnCameraSwapStart?.Invoke(this, EventArgs.Empty);
        m_isSwitchingFollowObject = true;
        CinemachineVirtualCamera currentCam = VCamManager.Instance.GetCurrentCamera();
        m_isRotate = false;
        while(Vector2.Distance(transform.position, newTransform.position) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position , newTransform.position, m_speedToSwapFollowObject * Time.deltaTime);
            if(isRotate)
            {
                currentCam.transform.rotation = Quaternion.RotateTowards(currentCam.transform.rotation, newTransform.rotation, m_speedToSwapFollowObject * 8 * Time.deltaTime);
            }
            else if(currentCam.transform.rotation != Quaternion.identity)
            {
                currentCam.transform.rotation = Quaternion.RotateTowards(currentCam.transform.rotation, Quaternion.identity, m_speedToSwapFollowObject * 8 * Time.deltaTime);
            }
            yield return null;
        }
        m_isRotate = isRotate;
        m_transformToFollow = newTransform;
        if(isRotate)
        {
            currentCam.transform.rotation = newTransform.rotation;
        }
        else if(currentCam.transform.rotation != Quaternion.identity)
        {
            currentCam.transform.rotation = Quaternion.identity;
        }
        m_isSwitchingFollowObject = false;
        OnCameraSwapDone?.Invoke(this, EventArgs.Empty);
    }
}
