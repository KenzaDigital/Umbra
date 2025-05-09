using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictorySceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button restartButton;
    public Button quitButton;

    void Start()
    {
        // Masquer les boutons au début
        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);

        // Lancer la vidéo
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }

        // Lier les boutons à leurs fonctions
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Afficher les boutons une fois la vidéo terminée
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    void RestartGame()
    {
        SceneManager.LoadScene("Menu"); // 
    }

    void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitter le jeu (ne fonctionne pas dans l'éditeur)");
    }
}
