using UnityEngine;
using UnityEngine.SceneManagement;

 

public class GameOverMenu : MonoBehaviour
{
    public void Rejouer()
    {
        // Recharge la sc�ne de jeu principale (remplace "NomDeTaScene")
        SceneManager.LoadScene("MainScene");
    }

    public void Quitter()
    {
        // Ferme le jeu (ne marche que dans le build final)
        Application.Quit();

        // Pour tester dans l��diteur
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}



