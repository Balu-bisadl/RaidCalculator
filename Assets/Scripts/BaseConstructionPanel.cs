using UnityEngine;
using UnityEngine.UI;

public class BaseConstructionPanel : MonoBehaviour
{
    [SerializeField] Image ConstructionImage;
    [SerializeField] Text ConstructionText;
    public int ConstructionCount;
    public Constructions_names ConstructionName;

    public void Inicialize(Sprite sprite, int constructioncount,Constructions_names constructionname)
    {
        ConstructionImage.sprite = sprite;
        ConstructionText.text = constructioncount.ToString();
        ConstructionName = constructionname;
        ConstructionCount = constructioncount;
    }
}

