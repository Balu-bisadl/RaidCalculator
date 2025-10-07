using UnityEngine;
using UnityEngine.UI;

public class BaseItemPanel : MonoBehaviour
{
    [SerializeField] Image ItemImage;
    [SerializeField] Text ItemText;

    public void Inicialize(Sprite sprite, int itemcount)
    {
        ItemImage.sprite = sprite;
        ItemText.text = itemcount.ToString();
    }
}
