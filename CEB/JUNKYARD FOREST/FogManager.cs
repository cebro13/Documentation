using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class FogZone : MonoBehaviour
{
    public GameObject fogPrefab; // Le prefab de ton objet de brouillard (qui utilise le shader modifié)
    public List<Sprite> fogSprites; // Si tu veux varier les sprites de brouillard
    public int fogCount = 50; // Combien d'instances de brouillard générer
    public Vector2 scaleRange = new Vector2(2f, 5f); // Taille min/max des sprites de brouillard
    public Vector2 alphaRange = new Vector2(0.05f, 0.2f); // Alpha de base min/max (le shader le modulera)
    public Transform player; // Fais glisser ton objet joueur ici dans l'inspecteur
    public float clearRadius = 2.5f; // Le rayon de la zone claire autour du joueur

    // Ajusté pour correspondre au Range du shader pour la clarté
    [Range(0.01f, 60f)]
    public float feathering = 0.5f; // Contrôle la douceur de la transition (0.01 = dur, 1 = très doux)

    // Pas besoin de fadeSpeed ici car le shader gère l'alpha directement
    // public float fadeSpeed = 2f; // Supprimé

    private List<FogData> fogInstances = new List<FogData>();
    private BoxCollider2D box;

    // Classe interne pour garder une référence à l'objet et son renderer
    class FogData
    {
        public GameObject obj;
        // baseAlpha n'est plus nécessaire car l'alpha de base est dans la couleur initiale du SpriteRenderer
        // public float baseAlpha;
        public SpriteRenderer sr;
        public Material mat; // Garde une référence au material pour l'efficacité
    }

    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        if (player == null)
        {
            Debug.LogError("FogZone: Référence au joueur non définie ! Assigne le joueur dans l'inspecteur.", this);
            return; // Arrête si le joueur n'est pas assigné
        }
        GenerateFog();
    }

    // Update est maintenant vide car le shader gère la visibilité
    void Update()
    {
        // Plus rien à faire ici !
        // La logique de calcul de distance et de modification de l'alpha a été déplacée dans le shader.
    }

    void LateUpdate()
    {
        // Met à jour les propriétés du shader pour chaque instance de brouillard
        // LateUpdate est bon pour les choses qui dépendent de la position (comme le joueur)
        // qui peuvent avoir été mises à jour dans Update().
        if (player == null) return; // Sécurité

        Vector3 playerPos = player.position; // Utilise Vector3 car SetVector attend un Vector4 (Vector3 est casté automatiquement)

        foreach (var fog in fogInstances)
        {
            if (fog.mat != null) // Vérifie si le material existe
            {
                // Envoie les données nécessaires au shader
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
            Debug.LogError("FogZone: Prefab de brouillard non défini ! Assigne le prefab dans l'inspecteur.", this);
            return; // Arrête si le prefab n'est pas assigné
        }

        Bounds bounds = box.bounds;

        for (int i = 0; i < fogCount; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                transform.position.z // Génère à la profondeur Z du FogZone
            );

            GameObject fog = Instantiate(fogPrefab, pos, Quaternion.identity, transform);
            SpriteRenderer sr = fog.GetComponent<SpriteRenderer>();

            if (sr == null)
            {
                Debug.LogError("FogZone: Le prefab de brouillard n'a pas de composant SpriteRenderer!", fog);
                Destroy(fog); // Détruit l'instance invalide
                continue; // Passe à l'itération suivante
            }

            // Assigne un sprite aléatoire si disponible
            if (fogSprites != null && fogSprites.Count > 0)
                sr.sprite = fogSprites[Random.Range(0, fogSprites.Count)];

            // Définit l'alpha initial aléatoire (le shader le modifiera ensuite)
            float initialAlpha = Random.Range(alphaRange.x, alphaRange.y);
            sr.color = new Color(1f, 1f, 1f, initialAlpha); // Applique l'alpha initial

            // Définit l'échelle aléatoire
            float scale = Random.Range(scaleRange.x, scaleRange.y);
            fog.transform.localScale = Vector3.one * scale;

            // Crée une instance unique du material pour cet objet afin de pouvoir
            // lui passer des propriétés spécifiques via LateUpdate sans affecter les autres.
            Material uniqueMaterial = sr.material; // Ceci crée une instance si ce n'est pas déjà fait

            fogInstances.Add(new FogData
            {
                obj = fog,
                sr = sr,
                mat = uniqueMaterial // Stocke la référence au material instancié
            });
        }
    }
}