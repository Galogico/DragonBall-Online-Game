using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScripts : MonoBehaviour
{
    public void Play(){
        SceneManager.LoadScene("Game");
    }
    public void Exit(){
        Application.Quit();
    }
}
