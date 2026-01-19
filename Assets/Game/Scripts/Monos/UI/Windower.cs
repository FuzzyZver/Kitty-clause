using UnityEngine;

public class Windower : MonoBehaviour
{
    public void OpenCloseWindow(GameObject window)
    {
        if(window.activeSelf)
        {
            window.SetActive(false);
        }
        else if(!window.activeSelf)
        {
            window.SetActive(true);
        }
    }
}
