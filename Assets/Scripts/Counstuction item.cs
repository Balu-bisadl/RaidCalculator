using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Counstuctionitem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public Constructions_names construction_name;
    [SerializeField] Image image;

    public void OnPointerClick(PointerEventData eventData)
    {
        FindObjectOfType<UIManager>().Button_construction_change(image, construction_name);
    }
}
