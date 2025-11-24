using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Selectedconstruction : MonoBehaviour, IPointerClickHandler
{
    public bool isactive = false;
    int constructioncount;
    Sprite constructionimage;
    Constructions_names construction_name;
    [SerializeField] Image selected_construction_image;
    [SerializeField] Text selected_construction_text;
    [SerializeField] GameObject outline2;
    UIManager manager;
    public int Constructioncount
    {
        get
        {
            return constructioncount;
        }
        set
        {
            if (value < 1000)
            {
                if (value > 0)
                {
                    constructioncount = value;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                constructioncount = 999;
            }
            selected_construction_text.text = constructioncount.ToString();
        }
    }
    public Constructions_names Construction_name
    {
        get
        {
            return construction_name;
        }
        private set
        {
            construction_name = value;
        }
    }
    public Image Selected_construction_image
    {
        get
        {
            return selected_construction_image;
        }
        private set
        {
            selected_construction_image = value;
        }
    }
    public void Inetialize(int count, Sprite image, Constructions_names construction)
    {
        Constructioncount = count;
        constructionimage = image;
        construction_name = construction;
        Selected_construction_image.sprite = image;
        manager = FindObjectOfType<UIManager>();
        manager.selectedconstructions.Add(this);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        manager.Selectedconstructionchoosed();
        foreach (Selectedconstruction construction in manager.selectedconstructions)
        {
            if (construction.isactive && construction != this)
            {
                construction.activateOrdisactivate(false);
            }
        }
        activateOrdisactivate(!isactive);
        if (isactive)
        {
            manager.Exchangebuttonsconstruction(false); 
        }
        else
        {
            manager.Exchangebuttonsconstruction(true);
        }
    }
    void OnDestroy()
    {
        manager.selectedconstructions.Remove(this); 
    }
    public void activateOrdisactivate(bool on)
    {
        outline2.SetActive(on);
        isactive = on;
    }

}