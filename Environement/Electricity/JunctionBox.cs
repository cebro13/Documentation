using System;
using UnityEngine;
using UnityEngine.U2D;

public class JunctionBox : MonoBehaviour, ICanInteract, IDataPersistant
{
    enum eJunctionPlugHole
    {
        LeftTop,
        LeftBottom,
        RightTop,
        RightBottom,
        Left,
        Right,
    }

    public event EventHandler<EventArgs> OnUnplug;
    public event EventHandler<EventArgs> OnPlug;

    [Header("These must always be set, even if they are not used.")]
    [SerializeField] private Transform m_topPlugLeft;
    [SerializeField] private Transform m_bottomPlugLeft;
    [SerializeField] private Transform m_unplugLeft;

    [Header("These must always be set, even if they are not used.")]
    [SerializeField] private Transform m_topPlugRight;
    [SerializeField] private Transform m_bottomPlugRight;
    [SerializeField] private Transform m_unplugRight;
    
    [Header("If there is only one ElectricWirePlug on the left side or the right side, it will be able to unplug.")]
    [Header("If a spline is able to unplug, the visual sprite must be at the bottom, event if it is chosen at top.")]
    [SerializeField] private ElectricWirePlug m_electricWirePlugLeftTop;
    [SerializeField] private ElectricWirePlug m_electricWirePlugLeftBottom;
    [SerializeField] private ElectricWirePlug m_electricWirePlugRightTop;
    [SerializeField] private ElectricWirePlug m_electricWirePlugRightBottom;

    [Header("Data persistant")]
    [SerializeField] private bool m_isDataPersistantActivate;
    [SerializeField] private string m_ID;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }
    
    private bool m_isLeftPlugBottom;
    private bool m_isRightPlugBottom;

    private bool m_canMoveLeft;
    private bool m_canMoveRight;

    private Spline m_splineLeft;
    private Spline m_splineRight;

    private int m_splineLeftIndex = 0;
    private int m_splineRightIndex = 0;

    private Vector2 m_topLeftPlugPosition;
    private Vector2 m_bottomLeftPlugPosition;
    private Vector2 m_unplugLeftPosition;

    private Vector2 m_topRightPlugPosition;
    private Vector2 m_bottomRightPlugPosition;
    private Vector2 m_unplugRightPosition;

    private bool m_isPlugLeftMoving;
    private bool m_isUnplugLeftPositionReached;

    private bool m_isPlugRightMoving;
    private bool m_isUnplugRightPositionReached;

    private float m_distanceThreshold = 0.1f;

    private IHasElectricityRunning m_hasElectrictyRunningLeft;
    private IHasElectricityRunning m_hasElectrictyRunningRight;

    //X is left, Y is right;
    private Vector2 m_persistantPosition;

    private Collider2D m_collider2D;
    private TriggerControlInputUI m_triggerControlInputUI;

    private bool m_isSwitchPlugActivate;

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_persistantPosition.x = 0;
        m_persistantPosition.y = 0;
        m_collider2D = GetComponent<Collider2D>();
        m_triggerControlInputUI = GetComponent<TriggerControlInputUI>();
        m_isSwitchPlugActivate = true;
    }

    private void Start()
    {
        InitSplines();
        SetWirePlugElectrictyRunning(true);
        SetWirePlugElectrictyRunning(false);
    }

    private void ElectricWirePlugLeftTop_OnElectricityChange(object sender, ElectricWirePlug.OnElectricityChangeEventArgs e)
    {
        SetElectrictyRunningJunction(eJunctionPlugHole.LeftTop, m_electricWirePlugLeftTop);
    }

    private void ElectricWirePlugLeftBottom_OnElectricityChange(object sender, ElectricWirePlug.OnElectricityChangeEventArgs e)
    {
        SetElectrictyRunningJunction(eJunctionPlugHole.LeftBottom, m_electricWirePlugLeftBottom);
    }

    private void ElectricWirePlugRightTop_OnElectricityChange(object sender, ElectricWirePlug.OnElectricityChangeEventArgs e)
    {
        SetElectrictyRunningJunction(eJunctionPlugHole.RightTop, m_electricWirePlugRightTop);
    }

    private void ElectricWirePlugRightBottom_OnElectricityChange(object sender, ElectricWirePlug.OnElectricityChangeEventArgs e)
    {
        SetElectrictyRunningJunction(eJunctionPlugHole.RightBottom, m_electricWirePlugRightBottom);
    }

    private void ElectricWirePlugLeft_OnElectricityChange(object sender, ElectricWirePlug.OnElectricityChangeEventArgs e)
    {
        if(m_electricWirePlugLeftBottom)
        {
            SetElectrictyRunningJunction(eJunctionPlugHole.Left, m_electricWirePlugLeftBottom);
        }
        else
        {
            SetElectrictyRunningJunction(eJunctionPlugHole.Left, m_electricWirePlugLeftTop);
        }
    }

    private void ElectricWirePlugRight_OnElectricityChange(object sender, ElectricWirePlug.OnElectricityChangeEventArgs e)
    {
        if(m_electricWirePlugRightBottom)
        {
            SetElectrictyRunningJunction(eJunctionPlugHole.Right, m_electricWirePlugRightBottom);
        }
        else
        {
            SetElectrictyRunningJunction(eJunctionPlugHole.Right, m_electricWirePlugRightTop);
        }
    }

    public void LoadData(GameData data)
    {
        if(m_isDataPersistantActivate)
        {
            data.junctionBox.TryGetValue(m_ID, out m_persistantPosition);
            if(data.newDataPersistant.TryGetValue(m_ID, out m_isSwitchPlugActivate))
            {
                if(m_isSwitchPlugActivate)
                {
                    ActivateSwitchPlug();
                }
                else
                {
                    DeactivateSwitchPlug();
                }
            }
            
        }
    }

    public void SaveData(GameData data)
    {
        if(!m_isDataPersistantActivate)
        {
            return;
        }
        if(data.junctionBox.ContainsKey(m_ID))
        {
            data.junctionBox.Remove(m_ID);
        }
        data.junctionBox.Add(m_ID, m_persistantPosition);

        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_isSwitchPlugActivate);
    }

    private void SetElectricityRunningJunctionSpecific(ref IHasElectricityRunning hasElectricityRunningOppositeSide, ref ElectricWirePlug electricWirePlugOpposite, bool isPlugBottom, bool isPlugBottomOppositeSide, ElectricWirePlug electricWirePlug, bool isSendingCurrent)
    {
        //Le cas où moveable plug à l'opposé
        bool arePlugsPlugged = !m_isPlugLeftMoving && !m_isPlugRightMoving;
        if(hasElectricityRunningOppositeSide != null)
        {
            //Si la plug à opposé est au même endroit
            if(isPlugBottomOppositeSide == isPlugBottom)
            {
                //On set l'électricité de la plug à droite à isElectricityRunning
                if(isSendingCurrent)
                {
                    electricWirePlug.SetElectricityRunning(0, hasElectricityRunningOppositeSide.IsElectricityRunning() && arePlugsPlugged);
                }
                else
                {
                    hasElectricityRunningOppositeSide.SetElectricityRunning(0, electricWirePlug.IsElectricityRunning() && arePlugsPlugged);
                }
            }
            else
            {
                //Sinon, on set la plug opposé à false
                if(isSendingCurrent)
                {
                    electricWirePlug.SetElectricityRunning(0, false);
                }
                else
                {
                    hasElectricityRunningOppositeSide.SetElectricityRunning(0, false);
                }
            }
        }
        //Le cas où il y un une plug fixe à l'opposé, on set la plug top droit à isElectricityRunning
        else if(electricWirePlugOpposite != null)
        {
            if(isSendingCurrent)
            {
                electricWirePlug.SetElectricityRunning(0, electricWirePlugOpposite.IsElectricityRunning() && arePlugsPlugged);
            }
            else
            {
                electricWirePlugOpposite.SetElectricityRunning(0, electricWirePlug.IsElectricityRunning() && arePlugsPlugged);
            }
        }
    }

    private void InitSplines()
    {
        m_isPlugLeftMoving = false;
        m_isPlugRightMoving = false;

        m_isUnplugLeftPositionReached = false;
        m_isUnplugRightPositionReached = false;

        m_canMoveLeft = false;
        m_canMoveRight = false;

        if(!m_electricWirePlugLeftBottom)
        {
            m_hasElectrictyRunningLeft = InitHasElectricityRunningInterface(m_electricWirePlugLeftTop);
            m_electricWirePlugLeftTop.OnElectricityChange += ElectricWirePlugLeft_OnElectricityChange;
            m_splineLeft = m_electricWirePlugLeftTop.GetWireSpline();
            m_splineLeftIndex =  m_electricWirePlugLeftTop.GetCurrentDirection() == ElectricWirePlug.eCurrentDirection.IsSendingCurrent ? 0 : (m_splineLeft.GetPointCount() - 1);
            m_isLeftPlugBottom = false;
            m_canMoveLeft = true;
        }
        else if(!m_electricWirePlugLeftTop)
        {
            m_hasElectrictyRunningLeft = InitHasElectricityRunningInterface(m_electricWirePlugLeftBottom);
            m_electricWirePlugLeftBottom.OnElectricityChange += ElectricWirePlugLeft_OnElectricityChange;
            m_splineLeft = m_electricWirePlugLeftBottom.GetWireSpline();
            m_splineLeftIndex =  m_electricWirePlugLeftBottom.GetCurrentDirection() == ElectricWirePlug.eCurrentDirection.IsSendingCurrent ? 0 : (m_splineLeft.GetPointCount() - 1);
            m_isLeftPlugBottom = true;
            m_canMoveLeft = true;
        }
        else
        {
            m_electricWirePlugLeftTop.OnElectricityChange += ElectricWirePlugLeftTop_OnElectricityChange;
            m_electricWirePlugLeftBottom.OnElectricityChange += ElectricWirePlugLeftBottom_OnElectricityChange;
        }
        
        if(!m_electricWirePlugRightBottom)
        {
            m_hasElectrictyRunningRight = InitHasElectricityRunningInterface(m_electricWirePlugRightTop);
            m_electricWirePlugRightTop.OnElectricityChange += ElectricWirePlugRight_OnElectricityChange;
            m_splineRight = m_electricWirePlugRightTop.GetWireSpline();
            m_splineRightIndex =  m_electricWirePlugRightTop.GetCurrentDirection() == ElectricWirePlug.eCurrentDirection.IsSendingCurrent ? 0 : (m_splineRight.GetPointCount() - 1);
            m_isRightPlugBottom = false;
            m_canMoveRight = true;
        }
        else if(!m_electricWirePlugRightTop)
        {
            m_hasElectrictyRunningRight = InitHasElectricityRunningInterface(m_electricWirePlugRightBottom);
            m_electricWirePlugRightBottom.OnElectricityChange += ElectricWirePlugRight_OnElectricityChange;
            m_splineRight = m_electricWirePlugRightBottom.GetWireSpline();
            m_splineRightIndex =  m_electricWirePlugRightBottom.GetCurrentDirection() == ElectricWirePlug.eCurrentDirection.IsSendingCurrent ? 0 : (m_splineRight.GetPointCount() - 1);
            m_isRightPlugBottom = true;
            m_canMoveRight = true;
        }
        else
        {
            m_electricWirePlugRightTop.OnElectricityChange += ElectricWirePlugRightTop_OnElectricityChange;
            m_electricWirePlugRightBottom.OnElectricityChange += ElectricWirePlugRightBottom_OnElectricityChange;
        }

        if(m_splineLeft != null)
        {
            m_bottomLeftPlugPosition = m_splineLeft.GetPosition(m_splineLeftIndex);
            m_topLeftPlugPosition = (Vector2)m_splineLeft.GetPosition(m_splineLeftIndex) - (Vector2)(m_bottomPlugLeft.position - m_topPlugLeft.position);
            m_unplugLeftPosition = (Vector2)m_splineLeft.GetPosition(m_splineLeftIndex) - (Vector2)(m_bottomPlugLeft.position - m_unplugLeft.position);
            
            if(m_persistantPosition.x == 0)
            {
                //This means either there is no data persistant, or data persistant is not activate.
                if(!m_isLeftPlugBottom)
                {
                    m_splineLeft.SetPosition(m_splineLeftIndex, m_topLeftPlugPosition);
                }
            }
            else if(m_persistantPosition.x == 1)
            {
                //if x == 1 we put it at the bottom, but it already is
                m_isLeftPlugBottom = true;
            }
            else if(m_persistantPosition.x == 2)
            {
                m_splineLeft.SetPosition(m_splineLeftIndex, m_topLeftPlugPosition);
                m_isLeftPlugBottom = false;
            }
            
        }

        if(m_splineRight != null)
        {
            m_bottomRightPlugPosition = m_splineRight.GetPosition(m_splineRightIndex);
            m_topRightPlugPosition = (Vector2)m_splineRight.GetPosition(m_splineRightIndex) - (Vector2)(m_bottomPlugRight.position - m_topPlugRight.position);
            m_unplugRightPosition = (Vector2)m_splineRight.GetPosition(m_splineRightIndex) - (Vector2)(m_bottomPlugRight.position - m_unplugRight.position);

            if(m_persistantPosition.y == 0)
            {
                //This means either there is no data persistant, or data persistant is not activate.
                if(!m_isRightPlugBottom)
                {
                    m_splineRight.SetPosition(m_splineRightIndex, m_topRightPlugPosition);
                }
            }
            else if(m_persistantPosition.y == 1)
            {
                //if x == 1 we put it at the bottom, but it already is
                m_isRightPlugBottom = true;
            }
            else if(m_persistantPosition.y == 2)
            {
                m_splineRight.SetPosition(m_splineRightIndex, m_topRightPlugPosition);
                m_isRightPlugBottom = false;
            }
            //Else if y == 1 we put it at the bottom, but it already is
        }
    }

    private void SetElectrictyRunningJunction(eJunctionPlugHole electrictySource, ElectricWirePlug electricPlug)
    {
        bool isSendingCurrent = electricPlug.GetCurrentDirection() == ElectricWirePlug.eCurrentDirection.IsSendingCurrent;

        switch(electrictySource)
        {
            case eJunctionPlugHole.LeftTop:
            {
                SetElectricityRunningJunctionSpecific(ref m_hasElectrictyRunningRight, ref m_electricWirePlugRightTop, false, m_isRightPlugBottom, electricPlug, isSendingCurrent);
                break;
            }
            case eJunctionPlugHole.LeftBottom:
            {
                SetElectricityRunningJunctionSpecific(ref m_hasElectrictyRunningRight, ref m_electricWirePlugRightBottom, true, m_isRightPlugBottom, electricPlug, isSendingCurrent);
                break;
            }
            case eJunctionPlugHole.RightTop:
            {
                SetElectricityRunningJunctionSpecific(ref m_hasElectrictyRunningLeft, ref m_electricWirePlugLeftTop, false, m_isLeftPlugBottom, electricPlug, isSendingCurrent);
                break;
            }
            case eJunctionPlugHole.RightBottom:
            {
                SetElectricityRunningJunctionSpecific(ref m_hasElectrictyRunningLeft, ref m_electricWirePlugLeftBottom, true, m_isLeftPlugBottom, electricPlug, isSendingCurrent);
                break;
            }
            case eJunctionPlugHole.Left:
            {
                if(m_isLeftPlugBottom)
                {
                    SetElectricityRunningJunctionSpecific(ref m_hasElectrictyRunningRight, ref m_electricWirePlugRightBottom, m_isLeftPlugBottom, m_isRightPlugBottom, electricPlug, isSendingCurrent);
                }
                else
                {
                    SetElectricityRunningJunctionSpecific(ref m_hasElectrictyRunningRight, ref m_electricWirePlugRightTop, !m_isLeftPlugBottom, !m_isRightPlugBottom, electricPlug, isSendingCurrent);
                }
                break;
            }
            case eJunctionPlugHole.Right:
            {
                if(m_isRightPlugBottom)
                {
                    SetElectricityRunningJunctionSpecific(ref m_hasElectrictyRunningLeft, ref m_electricWirePlugLeftBottom, m_isRightPlugBottom, m_isLeftPlugBottom, electricPlug, isSendingCurrent);
                }
                else
                {
                    SetElectricityRunningJunctionSpecific(ref m_hasElectrictyRunningLeft, ref m_electricWirePlugLeftTop, !m_isRightPlugBottom, !m_isLeftPlugBottom, electricPlug, isSendingCurrent);
                }
                break;
            }
            
            default:
            {
                Debug.LogError("Ce cas ne devrait pas arriver");
                break;
            }
        }
    }

    private void SetWirePlugElectrictyRunning(bool isRightMoving)
    {
        if(isRightMoving)
        {
            if(m_hasElectrictyRunningRight != null)
            {
                if(m_electricWirePlugRightBottom)
                {
                    SetElectrictyRunningJunction(eJunctionPlugHole.Right, m_electricWirePlugRightBottom);
                }
                else
                {
                    SetElectrictyRunningJunction(eJunctionPlugHole.Right, m_electricWirePlugRightTop);
                }
            }
            else
            {
                if(m_electricWirePlugRightBottom)
                {
                    SetElectrictyRunningJunction(eJunctionPlugHole.RightBottom, m_electricWirePlugRightBottom);
                }
                if(m_electricWirePlugRightTop)
                {
                    SetElectrictyRunningJunction(eJunctionPlugHole.RightTop, m_electricWirePlugRightTop);
                }    
            }
        }

        if(!isRightMoving)
        {
            if(m_hasElectrictyRunningLeft != null)
            {
                if(m_electricWirePlugLeftBottom)
                {
                    SetElectrictyRunningJunction(eJunctionPlugHole.Left, m_electricWirePlugLeftBottom);
                }
                else
                {
                    SetElectrictyRunningJunction(eJunctionPlugHole.Left, m_electricWirePlugLeftTop);
                }
            }
            else
            {
                if(m_electricWirePlugLeftBottom)
                {
                    SetElectrictyRunningJunction(eJunctionPlugHole.LeftBottom, m_electricWirePlugLeftBottom);
                }
                if(m_electricWirePlugLeftTop)
                {
                    SetElectrictyRunningJunction(eJunctionPlugHole.LeftTop, m_electricWirePlugLeftTop);
                }    
            }
        }
    }

    public void Interact()
    {
        if(Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition().x - transform.position.x > 0)
        {
            if(!m_isPlugRightMoving && m_canMoveRight)
            {
                m_isPlugRightMoving = true;
                SetWirePlugElectrictyRunning(true);
                OnUnplug?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            if(!m_isPlugLeftMoving && m_canMoveLeft)
            {
                m_isPlugLeftMoving = true;
                SetWirePlugElectrictyRunning(false);
                OnUnplug?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void Interact(Utils.Direction dir)
    {
        if(dir == Utils.Direction.Right)
        {
            if(!m_canMoveRight)
            {
                Debug.LogError("Impossible de déplacer le fil de droite");
            }
            else
            {
                m_isPlugRightMoving = true;
                SetWirePlugElectrictyRunning(true);
                OnUnplug?.Invoke(this, EventArgs.Empty);
            }
        }
        else if (dir == Utils.Direction.Left)
        {
            if(!m_canMoveLeft)
            {
                Debug.LogError("Impossible de déplacer le fil de gauche");
            }
            else
            {
                m_isPlugLeftMoving = true;
                SetWirePlugElectrictyRunning(false);
                OnUnplug?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            Debug.LogError("Dir doit être à 1 ou -1");
        }
    }

    private void Update()
    {
        if(m_isPlugLeftMoving)
        {
            HandlePlug(ref m_isUnplugLeftPositionReached, ref m_isLeftPlugBottom, ref m_isPlugLeftMoving, m_splineLeft, m_splineLeftIndex, m_unplugLeftPosition, m_bottomLeftPlugPosition, m_topLeftPlugPosition, false);
        }
        if(m_isPlugRightMoving)
        {
            HandlePlug(ref m_isUnplugRightPositionReached, ref m_isRightPlugBottom, ref m_isPlugRightMoving, m_splineRight, m_splineRightIndex, m_unplugRightPosition, m_bottomRightPlugPosition, m_topRightPlugPosition, true);
        }
        
    }

    private void HandlePlug(ref bool isUnplugPositionReached, ref bool isPlugButtom, ref bool isPlugMoving, Spline spline, int splineIndex, Vector2 unplugPosition, Vector2 bottomPlugPosition, Vector2 topPlugPosition, bool isRightMoving)
    {

        if(!isUnplugPositionReached)
        {
            Vector2 moveTowardVector = Vector2.MoveTowards(spline.GetPosition(splineIndex), unplugPosition, 5f*Time.deltaTime);
            spline.SetPosition(splineIndex, moveTowardVector);    
            if(Vector2.Distance(spline.GetPosition(splineIndex), unplugPosition) < m_distanceThreshold)
            {
                isUnplugPositionReached = true;
            }
        }
        else if(!isPlugButtom)
        {
            Vector2 moveTowardVector = Vector2.MoveTowards(spline.GetPosition(splineIndex), bottomPlugPosition, 5f*Time.deltaTime);
            spline.SetPosition(splineIndex, moveTowardVector);            
            
            if(Vector2.Distance(spline.GetPosition(splineIndex), bottomPlugPosition) < m_distanceThreshold)
            {
                isPlugMoving = false;
                isPlugButtom = true;
                isUnplugPositionReached = false;
                SetWirePlugElectrictyRunning(isRightMoving);
                if(!isRightMoving)
                {
                    m_persistantPosition.x = 1;
                }
                else
                {
                    m_persistantPosition.y = 1;
                }
                OnPlug?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            Vector2 moveTowardVector = Vector2.MoveTowards(spline.GetPosition(splineIndex), topPlugPosition, 5f*Time.deltaTime);
            spline.SetPosition(splineIndex, moveTowardVector);            
            
            if(Vector2.Distance(spline.GetPosition(splineIndex), topPlugPosition) < m_distanceThreshold)
            {
                isPlugMoving = false;
                isPlugButtom = false;
                isUnplugPositionReached = false;
                SetWirePlugElectrictyRunning(isRightMoving);
                if(!isRightMoving)
                {
                    m_persistantPosition.x = 2;
                }
                else
                {
                    m_persistantPosition.y = 2;
                }
                OnPlug?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private IHasElectricityRunning InitHasElectricityRunningInterface(ElectricWirePlug electricWire)
    {
        if(electricWire != null)
        {
            if(electricWire.TryGetComponent(out IHasElectricityRunning hasElectrictyRunningLeftTop))
            {
                return hasElectrictyRunningLeftTop;
            }
            else
            {
                Debug.LogError(electricWire.name + " does not have a component that implements IHasElectricityRunning");
            }
        }
        return null;
    }

    public void DeactivateSwitchPlug()
    {
        m_triggerControlInputUI.DeactivateUI();
        m_collider2D.enabled = false;
        m_isSwitchPlugActivate = false;
    }

    public void ActivateSwitchPlug()
    {
        m_triggerControlInputUI.ActivateUI();
        m_collider2D.enabled = true;
        m_isSwitchPlugActivate = true;
    }

}
