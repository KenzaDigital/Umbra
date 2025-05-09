using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueUI;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject[] choiceButtons;

    private int currentLineIndex = 0;
    private NpcDialogueData currentData;

    private void Start()
    {
        foreach (GameObject btn in choiceButtons)
        {
            btn.SetActive(false);
        }
    }
    public void Update()
    {
        if (dialogueUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseDialogue();
        }
    }
    public void StartDialogue(NpcDialogueData data)
    {
        dialogueUI.SetActive(true);
        currentData = data;
        currentLineIndex = 0;
        ShowNextLine();
    }

    public void ShowNextLine()
    {
        if (currentLineIndex < currentData.dialogueLines.Length)
        {
            dialogueText.text = currentData.dialogueLines[currentLineIndex];
            npcNameText.text = currentData.npcName;
            currentLineIndex++;
        }
        else
        {
            ShowFinalChoices(currentData);
        }
    }

    public GameObject nextButton; 

public void ShowFinalChoices(NpcDialogueData data)
    {
        dialogueText.text = data.finalQuestion;
        npcNameText.text = data.npcName;

        if (nextButton != null)
            nextButton.SetActive(false); // Cacher le bouton Suivant

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int index = i;
            if (i < data.dialogueChoices.Length)
            {
                choiceButtons[i].SetActive(true);
                TextMeshProUGUI choiceText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>(true);
                choiceText.text = data.dialogueChoices[i];
                choiceText.gameObject.SetActive(true);
                choiceButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                choiceButtons[i].GetComponent<Button>().onClick.AddListener(() => OnChoiceSelected(index, data));

            }
            else
            {
                choiceButtons[i].SetActive(false);
            }
        }
    }

    private void OnChoiceSelected(int index, NpcDialogueData data)
    {
        if (index == data.correctChoiceIndex)
        {
            Debug.Log("Bonne réponse !");
            SceneManager.LoadScene("VictoryScene");
        }
        else
        {
            dialogueText.text = "Ce n’est pas la vérité... Réfléchis bien.";
        }

        foreach (GameObject btn in choiceButtons)
        {
            btn.SetActive(false);
        }
    }

    public void CloseDialogue()
    {
        dialogueUI.SetActive(false);
    }
}
