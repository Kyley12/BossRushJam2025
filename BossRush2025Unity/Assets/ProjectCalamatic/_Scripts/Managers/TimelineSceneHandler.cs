using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineSceneHandler : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public string nextSceneName;

    private void Start()
    {
        if (playableDirector == null)
        {
            Debug.LogError("There is no playable director in the scene");
        }
        else if (playableDirector != null)
        {
            playableDirector.stopped += OnTimelineStopped;
        }
    }

    public void OnTimelineStopped(PlayableDirector playableDirector)
    {
        if(playableDirector == this.playableDirector)
        {
            LoadNextScene(nextSceneName);
        }
    }

    private void LoadNextScene(string nextSceneName)
    {
        if(!string.IsNullOrEmpty(nextSceneName))
        {
           SceneManager.LoadScene(nextSceneName);
        }
    }

    private void OnDestroy()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnTimelineStopped;
        }
    }
}
