using UnityEngine;

public  class GameController : MonoBehaviour
{
    public static string gameState;
    private bool gameEnd = false;
    public GameObject _endGameScreen;

    private void Update()
    {

        if (gameState == "Over" && !gameEnd || Input.GetButtonDown("Pause")) 
        {
            Time.timeScale = 0;
            _endGameScreen.SetActive(true);
            gameEnd = true;
        }
    }

    public void Reset() 
    {
        Time.timeScale = 1;
        _endGameScreen.SetActive(false);
        gameEnd = false;
        gameState = "NotOver";
    }

}
