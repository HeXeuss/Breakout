using UnityEngine;

public class Settings : MonoBehaviour
{
 
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            gameObject.SetActive(false);
        }
    }
}
