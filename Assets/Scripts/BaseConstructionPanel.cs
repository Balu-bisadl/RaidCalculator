using UnityEngine;
using UnityEngine.UI;

public class BaseConstructionPanel : MonoBehaviour
{
    [SerializeField] Image ConstructionImage;
    [SerializeField] Text ConstructionText;

    public void Inicialize(Sprite sprite, int constructioncount)
    {
        ConstructionImage.sprite = sprite;
        ConstructionText.text = constructioncount.ToString();
    }
}

