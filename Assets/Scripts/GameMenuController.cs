using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        fadeAnimator.SetTrigger("FadeIn");
    }

    public void PlayAgain()
    {
        StartCoroutine(FadeAndLoad());
    }

    public void QuitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private IEnumerator FadeAndLoad()
    {
        fadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene("Game");
    }
}
