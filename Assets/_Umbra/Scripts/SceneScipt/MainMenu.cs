using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("MainScene"); // Remplace par le nom exact de ta scène de jeu
    }

    public void LoadGame()
    {
        Debug.Log("Charger la sauvegarde..."); // Ajoute ton système de sauvegarde ici
    }

    public void OpenOptions()
    {
        // Active le menu des options si tu en as un
        Debug.Log("Options ouvertes");
    }

    public void Quitter()
    {
        // Ferme le jeu (ne marche que dans le build final)
        Application.Quit();

        // Pour tester dans l’éditeur
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
