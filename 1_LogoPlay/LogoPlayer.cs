using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(LogoCoroutine());
    }

    private IEnumerator LogoCoroutine()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("2_GameTitle");
    }
}
