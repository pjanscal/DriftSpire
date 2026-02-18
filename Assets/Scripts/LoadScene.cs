using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour

{
    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
