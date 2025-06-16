using System.Collections;
using UnityEngine;
using System;

public class SwitchableLiftGameObject : MonoBehaviour, ISwitchable
{
    private const string IS_ACCEPTING = "IsAccepting";
    private const string IS_REFUSING = "IsRefusing";
    private const string OPEN = "Open";
    private const string CLOSE = "Close";
    private const string ACTIVATE_UI = "ActivateUI";
    private const string DEACTIVATE_UI = "DeactivateUI";

    enum Trigger
    {
        Collider,
        Switch
    }
    [SerializeField] private Transform m_transformToMove;
    [SerializeField] private Trigger m_trigger;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_interactRadius;
    [SerializeField] private Transform m_pointA;
    [SerializeField] private Transform m_pointB;
    [SerializeField] private int m_sendBarrelOrderInLayerTo = 2;
    [SerializeField] private string m_sendBarrelSortingLayerTo;
    [SerializeField] private Animator m_animator;
    [SerializeField] private LiftAnimator m_liftAnimator;
    [SerializeField] private TriggerUI m_triggerUI;
 
    private GameObject m_liftedGameObject;
    private BarrelLiquidSwitchableElectricity m_currentBarrelLiquidElectric;
    private int m_direction; //Direction est 1 ou -1;
    private bool m_isMoving;
    private int m_originalOrder;
    private string m_orignalSortingLayer;
    private bool m_isWaitingForNextCycle;

    private Vector2 m_pointAPosition;
    private Vector2 m_pointBPosition;

    private void Awake()
    {
        m_direction = 1;
        m_isMoving = false;
        m_liftedGameObject = null;
        m_isWaitingForNextCycle = false;
        m_currentBarrelLiquidElectric = null;
        m_pointAPosition = m_pointA.position;
        m_pointBPosition = m_pointB.position;
    }

    private void Start()
    {
        m_liftAnimator.OnStartMoving += LiftAnimator_OnStartMoving;
        m_liftAnimator.OnAccepting += LiftAnimator_OnAccepting;
        m_liftAnimator.OnRefusingDone += LiftAnimator_OnRefusingDone;
        m_liftAnimator.OnDoorOpen += LiftAnimator_OnDoorOpen;
        m_liftAnimator.OnShowUI += LiftAnimator_OnShowUI;
        m_liftAnimator.OnHideUI += LiftAnimator_OnHideUI;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(!collider.CompareTag(TagDatabaseServer.DETECTION_TAG))
        {
            return;
        }
        if(m_trigger != Trigger.Collider || m_isMoving || m_liftedGameObject != null)
        {
            return;
        }
        HandleLiftState(m_pointA);
    }

    private void LiftAnimator_OnAccepting(object sender, EventArgs e)
    {
        m_animator.SetTrigger(CLOSE);
    }

    private void LiftAnimator_OnRefusingDone(object sender, EventArgs e)
    {
        m_liftedGameObject.layer = Player.GRABBABLE_LAYER;
        m_isWaitingForNextCycle = true;
    }

    private void LiftAnimator_OnStartMoving(object sender, EventArgs e)
    {
        m_isMoving = true;
    }

    private void LiftAnimator_OnDoorOpen(object sender, EventArgs e)
    {
        HandleLiftDone();
    }

    private void LiftAnimator_OnShowUI(object sender, EventArgs e)
    {
        m_animator.SetTrigger(ACTIVATE_UI);
    }

    private void LiftAnimator_OnHideUI(object sender, EventArgs e)
    {
        m_animator.SetTrigger(DEACTIVATE_UI);
    }

    public void Switch()
    {
        if(m_trigger != Trigger.Switch || m_isMoving || m_liftedGameObject != null)
        {
            return;
        }
        HandleLiftState(m_pointA);
    }

    private void HandleLiftState(Transform point)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, m_interactRadius);
        foreach(Collider2D collider in colliders)
        {
            //Get if it's an explodable barrel
            if(collider.TryGetComponent(out BarrelLiquidSwitchableElectricity barrelLiquidElectric))
            {
                m_currentBarrelLiquidElectric = barrelLiquidElectric;
                m_liftedGameObject = collider.gameObject;
            }
            else
            {
                continue;
            }
        }
        HandleTriggerLift();
    }

    private void HandleTriggerLift()
    {
        if(m_currentBarrelLiquidElectric.GetIsElectricityRunning())
        {
            TriggerLiftTransition();
        }
        else
        {
            TriggerLiftDenial();
        }
    }


    private void TriggerLiftDenial()
    {
        m_liftedGameObject.layer = Player.DEFAULT_LAYER;
        if(Player.Instance.playerStateMachine.CurrentState == Player.Instance.PushState)
        {
            Player.Instance.playerStateMachine.ChangeState(Player.Instance.IdleState);
        }
        StartCoroutine(CenterBarrelToRefuse());       
    }

    private void TriggerLiftTransition()
    {
        //Change Physics and give velocity
        if(m_liftedGameObject.TryGetComponent(out Rigidbody2D rigidbody))
        {
            m_liftedGameObject.layer = Player.DEFAULT_LAYER;
            if(Player.Instance.playerStateMachine.CurrentState == Player.Instance.PushState)
            {
                Player.Instance.playerStateMachine.ChangeState(Player.Instance.IdleState);
            }
            rigidbody.bodyType = RigidbodyType2D.Static;
            StartCoroutine(CenterBarrelToLift());
        }
        else
        {
            Debug.Log("Rigidbody2D n'est pas implémenté sur cet objet");
        }
    }

    private void ChangeOrderInLayerOfChildren(int orderToChange, string sortingLayerToChange, bool keepOriginalLayer)
    {
        foreach(Transform child in m_liftedGameObject.transform)
        {
            if(child.CompareTag(TagDatabaseServer.SPRITE_1_TAG))
            {
                if(child.TryGetComponent(out SpriteRenderer spriteRenderer))
                {
                    if(keepOriginalLayer)
                    {
                        m_originalOrder = spriteRenderer.sortingOrder;
                        m_orignalSortingLayer = spriteRenderer.sortingLayerName;
                    }
                    spriteRenderer.sortingOrder = orderToChange;
                    spriteRenderer.sortingLayerName = sortingLayerToChange;
                }
                else
                {
                    Debug.LogError(child.name + " n'implémente pas de spriteRenderer component");
                }
            }
            foreach(Transform subChild in child)
            {
                if(subChild.CompareTag(TagDatabaseServer.SPRITE_1_TAG))
                {
                    if(subChild.TryGetComponent(out SpriteRenderer spriteRenderer))
                    {
                        if(keepOriginalLayer)
                        {
                            m_originalOrder = spriteRenderer.sortingOrder;
                        }
                        spriteRenderer.sortingOrder = orderToChange;
                        spriteRenderer.sortingLayerName = sortingLayerToChange;
                    }
                    else
                    {
                        Debug.LogError(subChild.name + " n'implémente pas de spriteRenderer component");
                    }
                }
            }
        }
    }

    private void DeactivateGroundCollider()
    {
        foreach(Transform child in m_liftedGameObject.transform)
        {
            if(child.CompareTag(TagDatabaseServer.GROUND_TAG))
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private void ReactivateGroundCollider()
    {
        foreach(Transform child in m_liftedGameObject.transform)
        {
            if(child.CompareTag(TagDatabaseServer.GROUND_TAG))
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if(m_isWaitingForNextCycle)
        {
            m_liftedGameObject = null;
            m_currentBarrelLiquidElectric = null;
            m_isWaitingForNextCycle = false;
        }
        if(!m_isMoving)
        {
            return;
        }
        if(m_direction == 1)
        {
            m_transformToMove.position = Vector2.MoveTowards(m_transformToMove.position, m_pointBPosition, Time.deltaTime * m_speed);
            if(Vector2.Distance(m_transformToMove.position, m_pointBPosition) < 0.2f)
            {
                m_transformToMove.position = m_pointBPosition;
                m_animator.SetTrigger(OPEN);
                m_isMoving = false;
            }
        }
        else if(m_direction == -1)
        {
            m_transformToMove.position = Vector2.MoveTowards(m_transformToMove.position, m_pointAPosition, Time.deltaTime * m_speed);
            if(Vector2.Distance(m_transformToMove.position, m_pointAPosition) < 0.2f)
            {
                m_transformToMove.position = m_pointAPosition;
                m_animator.SetTrigger(OPEN);
                m_isMoving = false;
            }
        }
    }

    private void HandleLiftDone()
    {
        //Change Physics
        if(m_liftedGameObject.TryGetComponent(out Rigidbody2D rigidbody))
        {
            m_liftedGameObject.transform.SetParent(null);
            ReactivateGroundCollider();
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            m_liftedGameObject.layer = Player.GRABBABLE_LAYER;
        }

        //Change layer from spriteRenderers
        ChangeOrderInLayerOfChildren(m_originalOrder, m_orignalSortingLayer, false);

        //Change variable
        m_isWaitingForNextCycle = true;
        m_direction *= -1;

        m_triggerUI.InhibitUI(false);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_pointA.position, m_interactRadius);
        Gizmos.DrawWireSphere(m_pointB.position, m_interactRadius);
    }

    private IEnumerator CenterBarrelToLift()
    {
        while(Mathf.Abs(m_liftedGameObject.transform.position.x - m_pointA.position.x) > 0.1f )
        {
            m_liftedGameObject.transform.position = Utils.MoveTowardsInX(m_liftedGameObject.transform.position, m_pointA.position, Time.deltaTime*m_speed);
            yield return null;
        }
        m_liftedGameObject.transform.position = new Vector2(m_pointA.position.x, m_liftedGameObject.transform.position.y);
        m_liftedGameObject.transform.SetParent(transform);
        //Change layer from spriteRenderers
        ChangeOrderInLayerOfChildren(m_sendBarrelOrderInLayerTo, m_sendBarrelSortingLayerTo, true);
        DeactivateGroundCollider();
        m_triggerUI.InhibitUI(true);
        m_animator.SetTrigger(IS_ACCEPTING);
    }

    private IEnumerator CenterBarrelToRefuse()
    {
        while(Mathf.Abs(m_liftedGameObject.transform.position.x - m_pointA.position.x) > 0.1f )
        {
            m_liftedGameObject.transform.position = Utils.MoveTowardsInX(m_liftedGameObject.transform.position, m_pointA.position, Time.deltaTime*m_speed);
            yield return null;
        }
        m_animator.SetTrigger(IS_REFUSING);
    }
}
