using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class FogZone : MonoBehaviour
{
    public GameObject fogPrefab; // Le prefab de ton objet de brouillard (qui utilise le shader modifi�)
    public List<Sprite> fogSprites; // Si tu veux varier les sprites de brouillard
    public int fogCount = 50; // Combien d'instances de brouillard g�n�rer
    public Vector2 scaleRange = new Vector2(2f, 5f); // Taille min/max des sprites de brouillard
    public Vector2 alphaRange = new Vector2(0.05f, 0.2f); // Alpha de base min/max (le shader le modulera)
    public Transform player; // Fais glisser ton objet joueur ici dans l'inspecteur
    public float clearRadius = 2.5f; // Le rayon de la zone claire autour du joueur

    // Ajust� pour correspondre au Range du shader pour la clart�
    [Range(0.01f, 60f)]
    public float feathering = 0.5f; // Contr�le la douceur de la transition (0.01 = dur, 1 = tr�s doux)

    // Pas besoin de fadeSpeed ici car le shader g�re l'alpha directement
    // public float fadeSpeed = 2f; // Supprim�

    private List<FogData> fogInstances = new List<FogData>();
    private BoxCollider2D box;

    // Classe interne pour garder une r�f�rence � l'objet et son renderer
    class FogData
    {
        public GameObject obj;
        // baseAlpha n'est plus n�cessaire car l'alpha de base est dans la couleur initiale du SpriteRenderer
        // public float baseAlpha;
        public SpriteRenderer sr;
        public Material mat; // Garde une r�f�rence au material pour l'efficacit�
    }

    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        if (player == null)
        {
            Debug.LogError("FogZone: R�f�rence au joueur non d�finie ! Assigne le joueur dans l'inspecteur.", this);
            return; // Arr�te si le joueur n'est pas assign�
        }
        GenerateFog();
    }

    // Update est maintenant vide car le shader g�re la visibilit�
    void Update()
    {
        // Plus rien � faire ici !
        // La logique de calcul de distance et de modification de l'alpha a �t� d�plac�e dans le shader.
    }

    void LateUpdate()
    {
        // Met � jour les propri�t�s du shader pour chaque instance de brouillard
        // LateUpdate est bon pour les choses qui d�pendent de la position (comme le joueur)
        // qui peuvent avoir �t� mises � jour dans Update().
        if (player == null) return; // S�curit�

        Vector3 playerPos = player.position; // Utilise Vector3 car SetVector attend un Vector4 (Vector3 est cast� automatiquement)

        foreach (var fog in fogInstances)
        {
            if (fog.mat != null) // V�rifie si le material existe
            {
                // Envoie les donn�es n�cessaires au shader
                fog.mat.SetVector("_PlayerWorldPos", playerPos);
                fog.mat.SetFloat("_ClearRadius", clearRadius);
                fog.mat.SetFloat("_Feathering", feathering);
            }
        }
    }

    void GenerateFog()
    {
        if (fogPrefab == null)
        {
            Debug.LogError("FogZone: Prefab de brouillard non d�fini ! Assigne le prefab dans l'inspecteur.", this);
            return; // Arr�te si le prefab n'est pas assign�
        }

        Bounds bounds = box.bounds;

        for (int i = 0; i < fogCount; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                transform.position.z // G�n�re � la profondeur Z du FogZone
            );

            GameObject fog = Instantiate(fogPrefab, pos, Quaternion.identity, transform);
            SpriteRenderer sr = fog.GetComponent<SpriteRenderer>();

            if (sr == null)
            {
                Debug.LogError("FogZone: Le prefab de brouillard n'a pas de composant SpriteRenderer!", fog);
                Destroy(fog); // D�truit l'instance invalide
                continue; // Passe � l'it�ration suivante
            }

            // Assigne un sprite al�atoire si disponible
            if (fogSprites != null && fogSprites.Count > 0)
                sr.sprite = fogSprites[Random.Range(0, fogSprites.Count)];

            // D�finit l'alpha initial al�atoire (le shader le modifiera ensuite)
            float initialAlpha = Random.Range(alphaRange.x, alphaRange.y);
            sr.color = new Color(1f, 1f, 1f, initialAlpha); // Applique l'alpha initial

            // D�finit l'�chelle al�atoire
            float scale = Random.Range(scaleRange.x, scaleRange.y);
            fog.transform.localScale = Vector3.one * scale;

            // Cr�e une instance unique du material pour cet objet afin de pouvoir
            // lui passer des propri�t�s sp�cifiques via LateUpdate sans affecter les autres.
            Material uniqueMaterial = sr.material; // Ceci cr�e une instance si ce n'est pas d�j� fait

            fogInstances.Add(new FogData
            {
                obj = fog,
                sr = sr,
                mat = uniqueMaterial // Stocke la r�f�rence au material instanci�
            });
        }
    }
}