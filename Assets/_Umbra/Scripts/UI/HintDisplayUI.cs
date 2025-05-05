using TMPro;
using UnityEngine;

public class HintDisplayUI : MonoBehaviour
{
    public GameObject HintPanel; // Le panneau qui contient le texte d'info
    public TextMeshProUGUI HintText; // Le texte d'info à afficher
    public AudioSource audioSource; // Source audio pour le son de l'info

  

public void ShowHint(string text)
    {
        if (HintText == null || HintPanel == null)
        {
            Debug.LogError("HintText ou HintPanel n'est pas assigné dans l'inspecteur.");
            return;
        }

        Debug.Log($"Affichage de l'indice : {text}");
        HintText.text = text;
        HintPanel.SetActive(true); // Affiche le panneau

        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play(); // Joue le son d'info
        }
    }

    public void HideHint()
    {
        HintPanel.SetActive(false); // Cache le panneau
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // Arrête le son d'info
        }
    }
    
}

