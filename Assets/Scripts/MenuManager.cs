using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartEasy()
    {
        SceneManager.LoadScene("EasyScene");
    }

    public void StartMedium()
    {
        SceneManager.LoadScene("MediumScene");
    }

    public void StartHard()
    {
        SceneManager.LoadScene("HardScene");
    }   
}
