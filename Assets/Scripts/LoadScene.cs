using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void ReloadScene() 
    {
        SceneManager.LoadScene("prototype2");
    }
}
