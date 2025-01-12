using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public enum CurrentCutscene
    {
        Opening,
        StartMenu
    }

    public CurrentCutscene currentScene;

    public CutsceneDataSO cutsceneDataSO;
    public TextMeshProUGUI dialogueText; // UI Text element for dialogue
    public float minTypingSpeed = 0.05f; // Minimum typing speed
    public float maxTypingSpeed = 0.1f; // Maximum typing speed
    public int maxLinesOnScreen = 6; // Maximum lines displayed on screen
    public float spinSpeed = 0.1f; // Speed of the spin animation
    public string nextSceneName; // Name of the next scene to load
    public Button startButton; // Reference to the Start button (UI)

    private Queue<string> displayedLines = new Queue<string>(); // Stores current lines on the screen
    private bool isPaused = false;
    private bool isCutsceneFinished = false;

    private void Start()
    {
        ResetCutsceneState();

        // Assign Start button functionality for Start Menu cutscene
        if (currentScene == CurrentCutscene.StartMenu && startButton != null)
        {
            startButton.onClick.AddListener(() => StartCutscene(cutsceneDataSO));
        }

        // Automatically start the cutscene for the Opening scene
        if (currentScene == CurrentCutscene.Opening)
        {
            StartCutscene(cutsceneDataSO);
        }
    }

    private void Update()
    {
        // Skip cutscene functionality for Opening scene only
        if (currentScene == CurrentCutscene.Opening && Input.GetKeyDown(KeyCode.Space) && !isCutsceneFinished)
        {
            SkipCutscene();
        }

        // Move to the next scene when Enter is pressed after Opening cutscene finishes
        if (currentScene == CurrentCutscene.Opening && isCutsceneFinished && Input.GetKeyDown(KeyCode.Return))
        {
            GoToNextScene();
        }

        if(currentScene == CurrentCutscene.StartMenu && isCutsceneFinished)
        {
            GoToNextScene();
        }
    }

    public void StartCutscene(CutsceneDataSO cutsceneData)
    {
        if (!isCutsceneFinished) // Prevent restarting if already finished
        {
            StartCoroutine(ProcessCutscene(cutsceneData));
        }
    }

    private IEnumerator ProcessCutscene(CutsceneDataSO cutsceneData)
    {
        foreach (string line in cutsceneData.lineData)
        {
            yield return StartCoroutine(TypeText(line));
        }

        isCutsceneFinished = true;

        if (currentScene == CurrentCutscene.Opening)
        {
            DisplayContinueMessage();
        }
    }

    private IEnumerator TypeText(string text)
    {
        string currentLine = "";
        for (int i = 0; i < text.Length; i++)
        {
            if (isPaused)
            {
                yield return new WaitUntil(() => !isPaused);
            }

            char currentChar = text[i];

            if (currentChar == '-')
            {
                isPaused = true;

                currentLine += "[complete]";
                UpdateDialogueText(currentLine);

                yield return StartCoroutine(SpinEffect(currentLine));

                isPaused = false;
                continue;
            }

            currentLine += currentChar;
            UpdateDialogueText(currentLine);

            yield return new WaitForSeconds(Random.Range(minTypingSpeed, maxTypingSpeed));
        }

        AddLineToScreen(currentLine);
    }

    private IEnumerator SpinEffect(string currentLine)
    {
        string[] spinFrames = { "/", "-", "\\", "|" };
        int frameIndex = 0;

        float spinDuration = 1.0f;
        float elapsedTime = 0f;

        while (elapsedTime < spinDuration)
        {
            string spinningText = currentLine.Replace("[complete]", spinFrames[frameIndex]);
            UpdateDialogueText(spinningText);

            frameIndex = (frameIndex + 1) % spinFrames.Length;

            elapsedTime += spinSpeed;
            yield return new WaitForSeconds(spinSpeed);
        }

        string completedText = currentLine.Replace("[complete]", "[complete]");
        UpdateDialogueText(completedText);
    }

    private void AddLineToScreen(string line)
    {
        displayedLines.Enqueue(line);

        if (displayedLines.Count > maxLinesOnScreen)
        {
            displayedLines.Dequeue();
        }

        UpdateDialogueText();
    }

    private void UpdateDialogueText(string currentLine = "")
    {
        string fullText = string.Join("\n\n", displayedLines.ToArray());
        if (!string.IsNullOrEmpty(currentLine))
        {
            fullText += $"\n{currentLine}";
        }

        dialogueText.text = fullText;
    }

    private void DisplayContinueMessage()
    {
        dialogueText.text += "\n[Press Enter to Continue]";
    }

    private void SkipCutscene()
    {
        StopAllCoroutines();

        displayedLines.Clear();
        foreach (string line in cutsceneDataSO.lineData)
        {
            displayedLines.Enqueue(line);
        }

        UpdateDialogueText();

        isCutsceneFinished = true;
        DisplayContinueMessage();
    }

    private void GoToNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    private void ResetCutsceneState()
    {
        isCutsceneFinished = false;
        isPaused = false;
        displayedLines.Clear();
    }
}