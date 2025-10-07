using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Raiditem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public Resources_names resource_name;
    [SerializeField] Image image;

    public void OnPointerClick(PointerEventData eventData) 
    {
        FindObjectOfType<UIManager>().Button_item_change(image, resource_name);
    }
}
