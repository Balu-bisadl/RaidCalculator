using UnityEngine;
using UnityEngine.UI;

public class ChoosedConstructionPanel : MonoBehaviour
{
    [SerializeField] public Image construction_image;
    [SerializeField] Text construction_count_text;
    [SerializeField] Image Button_disactivation_image;
    public Constructions_names construction_name;
    public int construction_count;
    public bool panel_is_disactive = false;

    public void SetConstructionPanel(Constructions_names construction_name, int count, Image image)
    {

    }
}
