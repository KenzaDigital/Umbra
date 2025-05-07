using UnityEngine;
using UnityEngine.SceneManagement;

 

public class GameOverMenu : MonoBehaviour
{
    public void Rejouer()
    {
        // Recharge la scène de jeu principale (remplace "NomDeTaScene")
        SceneManager.LoadScene("MainScene");
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



