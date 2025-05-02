using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TorchEnergy : MonoBehaviour
{
    [Header("Energy Settings")]
    public float maxEnergy = 100f;
    public float currentEnergy = 100f;
    public float energyConsumption = 10f;

    [Header("Light & Flicker")]
    public Light2D torchLight;
    public float lowEnergyThreshold = 20f;
    public float flickerFrequency = 0.1f;
    private bool isFlickering = false;

    [Header("UI")]
    public Slider energySlider;
    public Image sliderFillImage; // Pour changer la couleur du fill

    private void Start()
    {
        if (energySlider != null)
        {
            energySlider.maxValue = maxEnergy;
            energySlider.value = currentEnergy;
        }
    }

    private void Update()
    {
        if (currentEnergy > 0)
        {
            currentEnergy -= energyConsumption * Time.deltaTime;

            // Mise à jour du slider
            UpdateSlider();

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
            torchLight.enabled = false;
            UpdateSlider();
        }
    }

    public void Recharge(float amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);

        if (!torchLight.enabled && currentEnergy > 0)
        {
            torchLight.enabled = true;
        }

        UpdateSlider();
        Debug.Log($"Torch recharged by {amount}. Current energy: {currentEnergy}");
    }

    private void UpdateSlider()
    {
        if (energySlider != null)
        {
            energySlider.value = currentEnergy;

            // Changement de couleur selon le pourcentage
            if (sliderFillImage != null)
            {
                float pct = currentEnergy / maxEnergy;

                if (pct > 0.5f)
                    sliderFillImage.color = Color.green;
                else if (pct > 0.2f)
                    sliderFillImage.color = new Color(1f, 0.64f, 0f); // orange
                else
                    sliderFillImage.color = Color.red;
            }
        }
    }

    private IEnumerator FlickerEffect()
    {
        isFlickering = true;

        while (currentEnergy <= lowEnergyThreshold)
        {
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
            torchLight.intensity = 1f;
        }
    }
}
