using System.Collections;
using UnityEngine;

public class TorchEnergy : MonoBehaviour
{
    public float maxEnergy = 100f; // Énergie maximale
    public float currentEnergy = 100f; // Énergie actuelle
    public float energyConsumption = 10f; // Énergie consommée par seconde
    public Light torchLight; // Référence à la lumière de la torche
    public float lowEnergyThreshold = 20f; // Seuil d'énergie faible
    public float flickerFrequency = 0.1f; // Fréquence de clignotement en secondes
    private bool isFlickering = false; // Indique si la torche clignote

    private void Update()
    {
        if (currentEnergy > 0)
        {
            // Réduction de l'énergie au fil du temps
            currentEnergy -= energyConsumption * Time.deltaTime;

            // Appliquez les effets visuels si l'énergie est faible
            if (currentEnergy <= lowEnergyThreshold && !isFlickering)
            {
                StartCoroutine(FlickerEffect());
            }
            else if (currentEnergy > lowEnergyThreshold && isFlickering)
            {
                StopCoroutine(FlickerEffect());
                isFlickering = false;
                ResetLightIntensity();
            }
        }
        else
        {
            // Si l'énergie est épuisée, éteignez la lumière
            torchLight.enabled = false;
        }
    }

    private IEnumerator FlickerEffect()
    {
        isFlickering = true;

        while (currentEnergy <= lowEnergyThreshold)
        {
            // Alterne entre une intensité faible et normale
            torchLight.intensity = Random.Range(0.1f, 0.5f);
            yield return new WaitForSeconds(flickerFrequency);
            torchLight.intensity = Random.Range(0.5f, 1f);
            yield return new WaitForSeconds(flickerFrequency);
        }

        isFlickering = false;
    }

    private void ResetLightIntensity()
    {
        if (torchLight != null)
        {
            torchLight.intensity = 1f; // Réglez l'intensité par défaut
        }
    }
}