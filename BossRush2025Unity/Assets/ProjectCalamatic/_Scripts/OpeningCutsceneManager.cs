using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class OpeningCutsceneManager : MonoBehaviour
{
    public CutsceneDataSO cutsceneDataSO;
    public TextMeshProUGUI dialogueText; // UI Text element for dialogue
    public float minTypingSpeed = 0.05f; // Minimum typing speed
    public float maxTypingSpeed = 0.1f; // Maximum typing speed
    public int maxLinesOnScreen = 6; // Maximum lines displayed on screen
    public float spinSpeed = 0.1f; // Speed of the spin animation
    public string nextSceneName; // Name of the next scene to load

    private Queue<string> displayedLines = new Queue<string>(); // Stores current lines on the screen
    private bool isPaused = false;
    private bool isCutsceneFinished = false;

    private void Start()
    {
        ResetCutsceneState();
        StartCutscene(cutsceneDataSO);
    }

    private void Update()
    {
        // Skip cutscene when Space key is pressed
        if (Input.GetKeyDown(KeyCode.Space) && !isCutsceneFinished)
        {
            SkipCutscene();
        }

        // Go to the next scene when Enter key is pressed after the cutscene is finished
        if (isCutsceneFinished && Input.GetKeyDown(KeyCode.Return)) // KeyCode.Return for Enter key
        {
            GoToNextScene();
        }
    }

    public void StartCutscene(CutsceneDataSO cutsceneData)
    {
        StartCoroutine(ProcessCutscene(cutsceneData));
    }

    private IEnumerator ProcessCutscene(CutsceneDataSO cutsceneData)
    {
        foreach (string line in cutsceneData.lineData)
        {
            yield return StartCoroutine(TypeText(line));
        }

        // Once all lines are processed, wait for Enter key to transition
        isCutsceneFinished = true;
        DisplayContinueMessage();
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
        // Show a message prompting the user to press Enter
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