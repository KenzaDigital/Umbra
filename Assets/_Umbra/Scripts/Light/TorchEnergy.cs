using System.Collections;
using UnityEngine;

public class TorchEnergy : MonoBehaviour
{
    public float maxEnergy = 100f; // �nergie maximale
    public float currentEnergy = 100f; // �nergie actuelle
    public float energyConsumption = 10f; // �nergie consomm�e par seconde
    public Light torchLight; // R�f�rence � la lumi�re de la torche
    public float lowEnergyThreshold = 20f; // Seuil d'�nergie faible
    public float flickerFrequency = 0.1f; // Fr�quence de clignotement en secondes
    private bool isFlickering = false; // Indique si la torche clignote

    private void Update()
    {
        if (currentEnergy > 0)
        {
            // R�duction de l'�nergie au fil du temps
            currentEnergy -= energyConsumption * Time.deltaTime;

            // Appliquez les effets visuels si l'�nergie est faible
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
            // Si l'�nergie est �puis�e, �teignez la lumi�re
            torchLight.enabled = false;
        }
    }

    private IEnumerator FlickerEffect()
    {
        isFlickering = true;

        while (currentEnergy <= lowEnergyThreshold)
        {
            // Alterne entre une intensit� faible et normale
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
            torchLight.intensity = 1f; // R�glez l'intensit� par d�faut
        }
    }
}