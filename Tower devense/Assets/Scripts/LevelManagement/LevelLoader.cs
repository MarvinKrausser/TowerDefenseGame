using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public void startTransition(int levelIndex)
    {
        StartCoroutine(loadLevel(levelIndex));
    }

    IEnumerator loadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        switch (levelIndex)
        {
            case 2:
                FindObjectOfType<SoundManagerScript>().play("GameStart");
                break;
            case 3:
                FindObjectOfType<SoundManagerScript>().play("Chase");
                break;
            case 0:
                FindObjectOfType<SoundManagerScript>().stop("Chase");
                break;
        }
        SceneManager.LoadScene(levelIndex);
    }
}