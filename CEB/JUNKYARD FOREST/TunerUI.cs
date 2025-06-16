using UnityEngine;
using TMPro;

public class RadioTunerUI : MonoBehaviour
{
    [Header("Objects")]
    public RectTransform redDial;               // Molette rouge
    public RectTransform needle;                // Aiguille du cadran
    public TextMeshProUGUI frequencyText;       // Texte d'affichage numérique

    [Header("Settings")]
    public float minFrequency = 88.0f;
    public float maxFrequency = 108.0f;

    public float minNeedleRotation = 45f;   //  à GAUCHE = 88 MHz
    public float maxNeedleRotation = -45f;  //  à DROITE = 108 MHz

    public float dialRotationSpeed = 40f;

    private float currentFrequency = 88.0f;

    void Update()
    {
        float input = Input.GetAxis("Horizontal");

        if (Mathf.Abs(input) > 0.01f)
        {
            float deltaFreq = input * Time.deltaTime * 4f; 
            currentFrequency = Mathf.Clamp(currentFrequency + deltaFreq, minFrequency, maxFrequency);

            UpdateUI(deltaFreq);
        }
    }

    void UpdateUI(float deltaRotation)
    {
        // Affichage de la fréquence
        frequencyText.text = currentFrequency.ToString("F1") + " MHz";

        // Aiguille : inversion de l'interpolation
        float t = (currentFrequency - minFrequency) / (maxFrequency - minFrequency);
        float angle = Mathf.Lerp(minNeedleRotation, maxNeedleRotation, t);
        needle.localEulerAngles = new Vector3(0f, 0f, angle);

        // Molette rouge : tourne toujours de façon fluide
        redDial.Rotate(0f, 0f, -deltaRotation * dialRotationSpeed);
    }
}
