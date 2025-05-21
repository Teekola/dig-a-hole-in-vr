using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        fadeAnimator.SetTrigger("FadeIn");
    }

    public void GameOver()
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
        SceneManager.LoadScene("GameOver");
    }
}
