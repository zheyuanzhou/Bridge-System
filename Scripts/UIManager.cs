using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Image blackImage;
    private float alpha;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        alpha = 1;
        while(alpha > 0)
        {
            alpha -= Time.deltaTime;
            blackImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0);
        }
    }

    IEnumerator FadeOut(string _sceneName)
    {
        alpha = 0;

        while(alpha < 1)
        {
            alpha += Time.deltaTime;
            blackImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0);
        }

        SceneManager.LoadScene(_sceneName);
    }

    public void MenuButton(string _sceneName)
    {
        Debug.Log("Click");
        StartCoroutine(FadeOut(_sceneName));
    }
}
