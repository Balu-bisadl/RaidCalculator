using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image Choose_image;
    [SerializeField] Button Add_button;
    [SerializeField] Button Change_button;
    [SerializeField] Button Calculate_button;
    [SerializeField] Button Delete_button;
    [SerializeField] Button Delete_all_button;
    [SerializeField] InputField Item_count_input;
    [SerializeField] GameObject Selecteditem_prefab;
    [SerializeField] GameObject Selectedpanel;
    [SerializeField] GameObject PanelChange;
    [SerializeField] InputField PanelChangeInput;
    [SerializeField] GameObject CalculateItemPanelPrefab;
    [SerializeField] Transform CalculatePanelContent;
    [SerializeField] GameObject BaseItemPanelprefab;
    [SerializeField] Transform LastResultContent;
    Selecteditem[] ItemsForCalculate;
    Resources_names resource_name;
    public List<Selecteditem> selecteditems = new List<Selecteditem>();
    int item_count;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Item_count_input != null)
        {
            Item_count_input.onValueChanged.AddListener(OnInputChanged);
        }
        else
        {
            Debug.LogError("Inputfield ?? ????????");
        }
        CalculateButtonActivator();
    }

    // Update is called once per frame
    void Update()
    {
        Checkitemcountinputactive();
        Selecteditem selecteditem = Findactiveselecteditem();
        if (selecteditem != null)
        {
            Delete_button.gameObject.SetActive(true);
            Delete_all_button.gameObject.SetActive(false);
        }
        else
        {
            Delete_button.gameObject.SetActive(false);
            Delete_all_button.gameObject.SetActive(true);
        }

    }
    // ???????? ?????????? ???? ????? ?????????? ?????????? ???????? ???? ???????, ?? ???????? ?????? ?? "????????"
    void Checkitemcountinputactive()
    {
        if (Item_count_input.isFocused)
        {
            Exchangebuttons(true);
            
        }
    }
    public void Button_item_change(Image buttonimage,Resources_names resource)
    {
        Choose_image.sprite = buttonimage.sprite;
        resource_name = resource;
    }
    void OnInputChanged(string newText )
    {
        if (!string.IsNullOrEmpty(newText) && newText.All(char.IsDigit))
        {
            item_count= int.Parse(newText);
        }
        else
        {
            print("??????????? ????");
        }
    }
    private void OnDestroy()
    {
        if (Item_count_input != null)
            Item_count_input.onValueChanged.RemoveListener(OnInputChanged);
    }
    public void AddButton()
    {
        if (item_count > 0)
        {
            GameObject selecteditem = Instantiate(Selecteditem_prefab, Selectedpanel.transform);
            selecteditem.GetComponent<Selecteditem>().Inetialize(item_count, Choose_image.sprite, resource_name);
            CalculateButtonActivator();
        }
    }
    public void Selecteditemchoosed()
    {
        Change_button.interactable = true;    
    }
    public void Exchangebuttons(bool addactivate)
    {
        if (addactivate)
        {
            Change_button.gameObject.SetActive(false);
            Add_button.gameObject.SetActive(true);
            foreach (var item in selecteditems)
            {
                if (item.isactive)
                {
                    item.activateOrdisactivate(false);
                }
            }
        }
        else
        {
            Change_button.gameObject.SetActive(true);
            Add_button.gameObject.SetActive(false);
        }
    }
    public void ChangePanelactivator(bool on)
    {
        PanelChange.SetActive(on);
    }
    public void Changepanel_Buttonaccept()
    {
        Selecteditem item  = Findactiveselecteditem();
        if (item != null)
        {
            item.Itemcount = int.Parse(PanelChangeInput.text);
        }
    }
    Selecteditem Findactiveselecteditem()
    {
        foreach(Selecteditem item in selecteditems)
        {
            if (item.isactive)
            {
                return item;
            }
        }
        return null;
    }
    void Destroy_selected_item(Selecteditem item )
    {
        Destroy(item.gameObject);
        Exchangebuttons(true);
        Invoke("CalculateButtonActivator", 0.01f);
    }
    void CalculateButtonActivator()
    {
        if (Selectedpanel.transform.childCount == 0)
        {
            Calculate_button.interactable = false;
        }
        else
        {
            Calculate_button.interactable = true;
        }
    }
    public void Delete_selected_itembutton()
    {
        Destroy_selected_item(Findactiveselecteditem());
        PanelChangeInput.text = "";
    }
    public void Change_selected_itembutton()
    {
        int count = int.Parse(PanelChangeInput.text);
        PanelChangeInput.text = "";
        Selecteditem item = Findactiveselecteditem();
        if (item != null)
        {
            item.Itemcount = count;
        }
        Exchangebuttons(true);
    }
    public void Destroy_all()
    {
        foreach (Selecteditem item in selecteditems)
        {
            Destroy_selected_item(item);
        }
    }
    public void Calculate()
    {
        CreateResourcePanelsForCalculate();
    }

    void CreateResourcePanelsForCalculate()
    {
        ItemsForCalculate = Selectedpanel.gameObject.GetComponentsInChildren<Selecteditem>();
        foreach (Transform item in CalculatePanelContent.transform)
        {
            Destroy(item.gameObject);
        }
        var all_simple_resources_dict = new Dictionary<Resources_names, int>();
        for (int i = 0; i < ItemsForCalculate.Length; i++)
        {
            int itemcount = ItemsForCalculate[i].Itemcount;
            Resources_names resource_name = ItemsForCalculate[i].Resource_name;
            Image item_image = ItemsForCalculate[i].Selected_item_image;
            GameObject CalculateItemPanel = Instantiate(CalculateItemPanelPrefab, CalculatePanelContent);
            ChoosedElementPanel choosed_element_panel = CalculateItemPanel.GetComponent<ChoosedElementPanel>();
            choosed_element_panel.SetElementPanel(resource_name, itemcount, item_image);
            if (choosed_element_panel.panel_is_disactive == false)
            {
                var Current_item_simple_resources = choosed_element_panel.simple_resources;


                int main_item_minstack = Gamemanager.instance.Get_minstak_for_resource(resource_name);
                int mod = itemcount % main_item_minstack;
                if (mod != 0)
                {
                    int n = itemcount / main_item_minstack;
                    itemcount = (n + 1) * main_item_minstack;
                }

                foreach (var item in Current_item_simple_resources)
                {
                    if (all_simple_resources_dict.ContainsKey(item.Key))
                    {
                        all_simple_resources_dict[item.Key] += item.Value * itemcount / main_item_minstack;
                    }
                    else
                    {
                        all_simple_resources_dict[item.Key] = item.Value * itemcount / main_item_minstack;
                    }
                }
            }
        }

    // Запустить создание панелей с базовыми ресурсами на основе all_simple_resources_dict
        CreateSimpleResourcePanels(all_simple_resources_dict);
    }

    void CreateSimpleResourcePanels(Dictionary<Resources_names, int> Resources_dict)
    {
        // GameObject[] last_result_items = LastResultContent.Get
        for (int i = 0; i < LastResultContent.childCount; i++)
        {
            Destroy(LastResultContent.GetChild(i).gameObject);
        }
        foreach (var item in Resources_dict)
        {
            int item_count = item.Value;
            Resources_names item_name = item.Key;
            Sprite item_image = Gamemanager.instance.Get_sprite_for_resource(item_name);
            GameObject itempanel = Instantiate(BaseItemPanelprefab,LastResultContent);
            itempanel.GetComponent<BaseItemPanel>().Inicialize(item_image, item_count);
        }
    }

    public void UpdateResourcePanelsForCalculate()
    {

        var all_simple_resources_dict = new Dictionary<Resources_names, int>();


        for (int j = 0; j < CalculatePanelContent.childCount; j++)
        {
            ChoosedElementPanel choosed_element_panel =
                CalculatePanelContent.GetChild(j).gameObject.GetComponent<ChoosedElementPanel>();
            int itemcount = choosed_element_panel.main_item_count;
            
            if (choosed_element_panel.panel_is_disactive == false)
            {
                var Current_item_simple_resources = choosed_element_panel.simple_resources;


                int main_item_minstack = Gamemanager.instance.Get_minstak_for_resource(choosed_element_panel.main_item_resource_name);
                int mod = itemcount % main_item_minstack;
                if (mod != 0)
                {
                    int n = itemcount / main_item_minstack;
                    itemcount = (n + 1) * main_item_minstack;
                }

                foreach (var item in Current_item_simple_resources)
                {
                    if (all_simple_resources_dict.ContainsKey(item.Key))
                    {
                        all_simple_resources_dict[item.Key] += item.Value * itemcount / main_item_minstack;
                    }
                    else
                    {
                        all_simple_resources_dict[item.Key] = item.Value * itemcount / main_item_minstack;
                    }
                }
            }
        }


        // Запустить создание панелей с базовыми ресурсами на основе all_simple_resources_dict
        CreateSimpleResourcePanels(all_simple_resources_dict);
    }
}
