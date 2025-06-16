using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_backgroundSpriteRenderer;
    [SerializeField] private SpriteRenderer m_iconSpriteRenderer;
    [SerializeField] private TextMeshPro m_textMeshPro;
    [SerializeField] private GameObject m_background;

    [SerializeField] private bool m_debug;
    [ShowIf("m_debug")]
    [Header("Debug pour la grandeur du padding et la position du transform")]
    [SerializeField] private Vector2 m_paddingAroundBox;
    [Header("Debug pour la position de la plateform. Il faut passer par la méthode Setup lorsqu'on l'utilise pour vrai")]
    [ShowIf("m_debug")]
    [SerializeField] private bool m_testTransformOffset;
    [ShowIf("m_debug")]
    [SerializeField] private Vector2 m_transformOffSet;
    [Header("Debug pour TextWritterLinesRefSO, il faut toujours que ce champ soit vide dans le prefab")]
    [ShowIf("m_debug")]
    [SerializeField] private TextWritterLinesRefSO m_textWritterLinesRefSO;

    private bool m_isSetup;

    private void Awake()
    {
        m_background.SetActive(false);
        m_isSetup = false;
    }

    private void Start()
    {
        if(m_debug)
        {
            Debug.LogWarning("Attention la variable debug de chat bubble est à true!");
        }
        if(m_textWritterLinesRefSO)
        {
            Debug.LogError("Le champs 'textWritterLinesRefSO' devrait être null dans le prefab de ChatBubble");
            Setup(m_textWritterLinesRefSO, m_transformOffSet);
        }
    }

    //TO USE CEB:
    public void Setup(TextWritterLinesRefSO textWritterLinesRefSO, Vector2 transformOffSet)
    {
        //m_textMeshPro.SetText(text);
        TextWriter.AddWriter_Static(m_textMeshPro, textWritterLinesRefSO);
        transform.localPosition = transformOffSet;
        m_isSetup = true;
        m_background.SetActive(true);
    }
    
    //TO USE CEB:
    public bool IsWriterDone()
    {
        return TextWriter.IsWriterDone_Static(m_textMeshPro);
    }

    private void UpdateSizeBox()
    {
        m_textMeshPro.ForceMeshUpdate();
        Vector2 textSize = m_textMeshPro.GetRenderedValues(false);
        m_backgroundSpriteRenderer.size = textSize + m_paddingAroundBox ;
    }

    private void Update()
    {
        if(!m_isSetup)
        {
            return;
        }
        UpdateSizeBox();
        if(m_testTransformOffset)
        {
            transform.localPosition = m_transformOffSet;
        }
        if(transform.rotation.y != 0)
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
