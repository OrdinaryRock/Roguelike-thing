using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private string gameLevel;
    [SerializeField] private string menuLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginGame()
    {
        SceneManager.LoadScene(gameLevel);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(menuLevel);
    }
}
