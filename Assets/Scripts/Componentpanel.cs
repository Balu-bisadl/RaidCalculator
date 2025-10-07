using UnityEngine;
using UnityEngine.UI;

public class Componentpanel : MonoBehaviour
{
    [SerializeField] Text count_text;
    [SerializeField] Image item_image;
    int item_count;
    Resources_names current_resource_name;
    public void Component_init(Resources_names resource_name, int count, Sprite image)
    {
        current_resource_name = resource_name;
        item_count = count;
        item_image.sprite = image;
        count_text.text = item_count.ToString();
    }
}
