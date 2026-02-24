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

    [Header ("Конструкции")]

    [SerializeField] InputField Construction_count_input;
    [SerializeField] GameObject Selectedconstruction_prefab;
    [SerializeField] GameObject Selectedpanel_constructions;
    [SerializeField] Button Add_construction_button;
    [SerializeField] Button Change_construction_button;
    [SerializeField] Button Calculate_construction_button;
    [SerializeField] Button Delete_construction_button;
    [SerializeField] Button Delete_all_construction_button;
    [SerializeField] Image Choose_construction_image;
    [SerializeField] GameObject PanelConstructionChange;
    [SerializeField] InputField PanelChangeConstructionInput;
    [SerializeField] Transform LastResultConstructionsContent;
    [SerializeField] GameObject BaseConstructionPanelprefab;
    [SerializeField] GameObject CalculateConstructionPanelPrefab;
    [SerializeField] Transform PanelWeaponsForAllConstructionsDestroy;
    [SerializeField] Text CharcoalcountText;
    [SerializeField] Text SulfurcountText;
    Selecteditem[] ItemsForCalculate;
    Selectedconstruction[] ConstructionsForCalculate;
    Resources_names resource_name;
    Constructions_names construction_name;
    [HideInInspector]public List<Selecteditem> selecteditems = new List<Selecteditem>();
    [HideInInspector]public List<Selectedconstruction> selectedconstructions = new List<Selectedconstruction>();
    [HideInInspector] public Dictionary<WeaponForConstruction, int> weaponsfordestroyallconstructions = new Dictionary<WeaponForConstruction, int>();
    int item_count;
    int construction_count;
    
   
    [HideInInspector] public bool is_boom_menu;
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
        if (Construction_count_input != null)
        {
            Construction_count_input.onValueChanged.AddListener(OnInputChanged);
        }
        CalculateButtonActivator();

    }

    // Update is called once per frame
    void Update()
    {
        Checkitemcountinputactive();
        Checkconstructioncountinputactive();
        Selecteditem selecteditem = Findactiveselecteditem();
        Selectedconstruction selectedconstruction = Findactiveselectedconstruction();
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
        if (selectedconstruction != null)
        {
            Delete_construction_button.gameObject.SetActive(true);
            Delete_all_construction_button.gameObject.SetActive(false);
        }
        else
        {
            Delete_construction_button.gameObject.SetActive(false);
            Delete_all_construction_button.gameObject.SetActive(true);
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
    void Checkconstructioncountinputactive()
    {
        if (Construction_count_input.isFocused)
        {
            Exchangebuttonsconstruction(true);

        }
    }
    public void Button_construction_change(Image buttonimage,Constructions_names construction)
    {
        construction_name =  construction;
        Choose_construction_image.sprite = buttonimage.sprite;
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
            if (is_boom_menu)
            {
                item_count = int.Parse(newText);
            }
            else
            {
                construction_count = int.Parse(newText);
            }
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
        if (Construction_count_input != null)
            Construction_count_input.onValueChanged.RemoveListener(OnInputChanged);
    }
    public void AddButton()
    {
        if (is_boom_menu)
        {
            if (item_count > 0)
            {
                GameObject selecteditem = Instantiate(Selecteditem_prefab, Selectedpanel.transform);
                selecteditem.GetComponent<Selecteditem>().Inetialize(item_count, Choose_image.sprite, resource_name,true);
                CalculateButtonActivator();
            }
        }
        else
        {
            if (construction_count > 0)
            {
                GameObject selected_construction = Instantiate(Selectedconstruction_prefab, Selectedpanel_constructions.transform);
                selected_construction.GetComponent<Selectedconstruction>().Inetialize(construction_count, Choose_construction_image.sprite, construction_name);
                CalculateButtonActivator();
            }
        }
    }
    public void Selecteditemchoosed()
    {
        Change_button.interactable = true;    
    }
    public void Selectedconstructionchoosed()
    {
        Change_construction_button.interactable = true;
    }
    public void Exchangebuttonsconstruction(bool addactivate)
    {
        if (addactivate)
        {
            Change_construction_button.gameObject.SetActive(false);
            Add_construction_button.gameObject.SetActive(true);
            foreach (var item in selectedconstructions)
            {
                if (item.isactive)
                {
                    item.activateOrdisactivate(false);
                }
            }
        }
        else
        {
            Change_construction_button.gameObject.SetActive(true);
            Add_construction_button.gameObject.SetActive(false);
        }
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
    public void ChangePanelconstructionactivator(bool on)
    {
        PanelConstructionChange.SetActive(on);
    }
    public void Changepanel_Buttonaccept()
    {
        Selecteditem item  = Findactiveselecteditem();
        if (item != null)
        {
            item.Itemcount = int.Parse(PanelChangeInput.text);
        }
    }
    public void Changepanel_Buttonconstructionaccept()
    {
        Selectedconstruction item = Findactiveselectedconstruction();
        if (item != null)
        {
            item.Constructioncount = int.Parse(PanelChangeConstructionInput.text);
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
    Selectedconstruction Findactiveselectedconstruction()
    {
        foreach (Selectedconstruction construction in selectedconstructions)
        {
            if (construction.isactive)
            {
                return construction;
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
    void Destroy_selected_construction(Selectedconstruction construction)
    {
        Destroy(construction.gameObject);
        Exchangebuttonsconstruction(true);
        Invoke("CalculateButtonActivator", 0.01f); 
    }
    void CalculateButtonActivator()
    {
        if (is_boom_menu)
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
        else
        {
            if (Selectedpanel_constructions.transform.childCount == 0)
            {
                Calculate_construction_button.interactable = false;
            }
            else
            {
                Calculate_construction_button.interactable = true;
            }
        }
    }
    public void Delete_selected_itembutton()
    {
        Destroy_selected_item(Findactiveselecteditem());
        PanelChangeInput.text = "";
    }
    public void Delete_selected_constructionbutton()
    {
        Destroy_selected_construction(Findactiveselectedconstruction());
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
    public void Change_selected_constructionbutton()
    {
        int count = int.Parse(PanelChangeConstructionInput.text);
        PanelChangeConstructionInput.text = "";
        Selectedconstruction item = Findactiveselectedconstruction();
        if (item != null)
        {
            item.Constructioncount = count;
        }
        Exchangebuttonsconstruction(true);
    }
    public void Destroy_all()
    {
        foreach (Selecteditem item in selecteditems)
        {
            Destroy_selected_item(item);
        }
    }
    public void Destroy_all_construction()
    {
        foreach (Selectedconstruction construction in selectedconstructions)
        {
            Destroy_selected_construction(construction);
        }
    }
    public void Calculate()
    {    
        if (is_boom_menu == true)
        { 
            CreateResourcePanelsForCalculate();
        }
        else 
        {
            CreateConstructionPanelsForCalculate();
        }
 
    }

    public void CreateConstructionPanelsForCalculate()
    {
        ConstructionsForCalculate = Selectedpanel_constructions.gameObject.GetComponentsInChildren<Selectedconstruction>();
        foreach (Transform construction in CalculatePanelContent)
        {
            Destroy(construction.gameObject);
        }
        CreateChoosedConstructionsPanel(ConstructionsForCalculate);
    }
    public void WeaponsForConstructionsDetailButton()
    {
        CreateResourcePanelsForCalculate(false);
    }
    void CreateResourcePanelsForCalculate(bool isboommenu = true)
    {
        if (isboommenu)
        {
            ItemsForCalculate = Selectedpanel.gameObject.GetComponentsInChildren<Selecteditem>();
            foreach (Transform item in CalculatePanelContent.transform)
            {
                Destroy(item.gameObject);
            }
        }
        else
        {            
            ItemsForCalculate = PanelWeaponsForAllConstructionsDestroy.gameObject.GetComponentsInChildren<Selecteditem>();
            foreach (Transform item in CalculatePanelContent.transform)
            {
                Destroy(item.gameObject);
            }            
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
    void CreateChoosedConstructionsPanel(Selectedconstruction[] ConstructionsForCalculate)
    {
        List<Dictionary<WeaponForConstruction, int>> weaponsfordestroy = new List<Dictionary<WeaponForConstruction, int>>();

        for (int i = 0; i < LastResultConstructionsContent.childCount; i++)
        {
            Destroy(LastResultConstructionsContent.GetChild(i).gameObject);
        }
        foreach (Selectedconstruction construction in ConstructionsForCalculate)
        {
            int construction_count = construction.Constructioncount;
            Constructions_names construction_name = construction.Construction_name;
            Sprite construction_image = Gamemanager.instance.Get_sprite_for_construction(construction_name);
            GameObject constructionpanel = Instantiate(BaseConstructionPanelprefab, LastResultConstructionsContent);
            constructionpanel.GetComponent<BaseConstructionPanel>().Inicialize(construction_image, construction_count,construction_name); // Доделать
            weaponsfordestroy.Add(Gamemanager.instance.CalculateweaponsforDestroy(construction_name,construction_count));
        }
        // формирование словаря с типами и количеством оружия для уничтожения всех выбранных конструкций
        weaponsfordestroyallconstructions = new Dictionary<WeaponForConstruction, int>();
        foreach (Dictionary<WeaponForConstruction, int> weapons in weaponsfordestroy)
        {
            foreach(var kvp in weapons)
            {
                if (weaponsfordestroyallconstructions.Count > 0)
                {
                    bool addednewweapon = true;
                    foreach (var kvp2 in weaponsfordestroyallconstructions)
                    {
                        if(kvp.Key.weapon_name == kvp2.Key.weapon_name)
                        {
                            weaponsfordestroyallconstructions[kvp2.Key] += kvp.Value;
                            addednewweapon = false;
                            break;
                        }
                    }
                    if (addednewweapon)
                    {
                        weaponsfordestroyallconstructions[kvp.Key] = kvp.Value;
                    }
                }  
                else
                {
                    weaponsfordestroyallconstructions[kvp.Key] = kvp.Value; 
                }
            }
        }
        //По словарю weaponsfordestroyallconstructions создать ячейки с оружием в интерфейсе под валютой
        int Charcoalcount = 0;
        int Sulfurcount = 0;
        foreach (var kvp in weaponsfordestroyallconstructions)
        {
            WeaponForConstruction currentweapon = kvp.Key;
            int currentweaponcount = kvp.Value;
            Resources_names weaponname = currentweapon.weapon_name;
            Dictionary<Resources_names, int> simple_resources = Gamemanager.instance.Get_simple_recources(weaponname);
            int currentresourceminstack = Gamemanager.instance.Get_minstak_for_resource(weaponname);
            int factcount = currentweaponcount / currentresourceminstack;
            if (currentweaponcount % currentresourceminstack != 0)
            {
                factcount += 1;
            }
            foreach (KeyValuePair<Resources_names, int> res in simple_resources)
            { 
               if (res.Key == Resources_names.charcoal)
                {
                    Charcoalcount += res.Value*factcount;
                    //надо учитывать minstack при расчете чтобы не добавлял лишнюю пулю
                }
                else if (res.Key == Resources_names.sulfur)
                {
                    Sulfurcount += res.Value*factcount;
                }
            }
            GameObject currentweaponobject = Instantiate(Selecteditem_prefab, PanelWeaponsForAllConstructionsDestroy);
            Sprite currentweaponsprite = null;
            foreach (WeaponSettings weaponsetting in Gamemanager.instance.weapon_settings)
            {
                if (weaponsetting.weapon_name == weaponname)
                {
                    currentweaponsprite = weaponsetting.weapon_image;
                }
            }
            currentweaponobject.GetComponent<Selecteditem>().Inetialize(currentweaponcount, currentweaponsprite, weaponname, true);
        }
        CharcoalcountText.text = $"{Charcoalcount}";
        SulfurcountText.text = $"{Sulfurcount}";
    }
    public void ClearPanelWeaponsForAllConstructions()
    {
        foreach (Transform weaponfordestroy in PanelWeaponsForAllConstructionsDestroy.transform)
        {
            Destroy(weaponfordestroy.gameObject); 
        }
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
