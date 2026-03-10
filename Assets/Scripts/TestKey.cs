using UnityEngine;

public class TestKey : MonoBehaviour
{
    void Update()
    {
        Debug.Log("Update draait");

        if (UnityEngine.Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("K werkt!");
        }
    }
}
