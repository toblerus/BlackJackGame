using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button ExitButton;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        PlayButton.onClick.AddListener(Play);
        ExitButton.onClick.AddListener(Exit);
    }

    private void Play()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    private void Exit()
    {
        Application.Quit();
    }
}
