using Unity.VisualScripting;
using UnityEngine;

public class OpenURLbutton : MonoBehaviour
{
    

    public void openurl(string url)
    {
        Application.OpenURL(url);
    }
    
}
