using UnityEngine;

public class ConspiBoss_ComputerScreen : MonoBehaviour, ICanInteract
{
    [SerializeField] private ConspiBoss_ComputerScreenUI m_computerConspirationistUI;

    private void Start()
    {

    }

    public void Interact()
    {
        Debug.Log("Ici");
        m_computerConspirationistUI.OpenComputer();
    }
}
