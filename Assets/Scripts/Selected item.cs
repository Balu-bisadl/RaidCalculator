using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Selecteditem : MonoBehaviour, IPointerClickHandler
{
    public bool isactive = false;
    int itemcount;
    Sprite itemimage;
    Resources_names resource_name;
    [SerializeField] Image selected_item_image;
    [SerializeField] Text selected_item_text;
    [SerializeField] GameObject outline2;
    UIManager manager;
    public int Itemcount
    {
        get
        {
            return itemcount;
        }
        set
        {
            if (value <1000)
            {
                if (value > 0)
                {
                    itemcount = value;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                itemcount = 999;
            }
            selected_item_text.text = itemcount.ToString();
        }
    }
    public Resources_names Resource_name
    {
        get
        {
            return resource_name;
        }
        private set
        {
            resource_name = value;
        }
    }
    public Image Selected_item_image
    {
        get
        {
            return selected_item_image;
        }
        private set
        {
            selected_item_image = value;
        }
    }
    public void Inetialize (int count,Sprite image, Resources_names resource)
    {
        Itemcount = count;
        itemimage = image;
        resource_name = resource; 
        selected_item_image.sprite = image;
        manager = FindObjectOfType<UIManager>();
        manager.selecteditems.Add(this);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        manager.Selecteditemchoosed();
        foreach (Selecteditem item in manager.selecteditems)
        {
            if (item.isactive && item != this)
            {
                item.activateOrdisactivate(false);
            }
        }
        activateOrdisactivate(!isactive);
        if (isactive)
        {
            manager.Exchangebuttons(false);
        }
        else
        {
            manager.Exchangebuttons(true);
        }
    }
    void OnDestroy ()
    { 
        manager.selecteditems.Remove(this);
    }
    public void activateOrdisactivate(bool on)
    {
        outline2.SetActive(on);
        isactive = on;
    }

}
