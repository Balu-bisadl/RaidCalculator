using UnityEngine;

public class ChangeMenus : MonoBehaviour
{
    [SerializeField] bool is_boom_menu;
    private void OnEnable()
    {
        FindObjectOfType<UIManager>().is_boom_menu = is_boom_menu;
    }

}
