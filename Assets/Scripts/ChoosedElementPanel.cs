using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosedElementPanel : MonoBehaviour
{
    [SerializeField] public Image main_item_image;
    [SerializeField] Text main_item_count_text;
    [SerializeField] GameObject object_components_panel;
    [SerializeField] GameObject component_prefab;
    [SerializeField] GameObject simple_component_prefab;
    [SerializeField] Transform simple_components_panel;
    [SerializeField] Image Button_disactivation_image;
    public Resources_names main_item_resource_name;
    public Dictionary<Resources_names, int> simple_resources = new Dictionary<Resources_names, int>();
    public int main_item_count;
    public bool panel_is_disactive = false;

    public void SetElementPanel(Resources_names resource_name, int count, Image image)
    {
        main_item_resource_name = resource_name;
        int main_item_minstack = Gamemanager.instance.Get_minstak_for_resource(resource_name);
        int mod = count % main_item_minstack;
        if (mod != 0)
        {
            int n = count / main_item_minstack;
            main_item_count = (n + 1) * main_item_minstack;
        }
        else
        {
            main_item_count = count;
        }

        main_item_image.sprite = image.sprite;
        main_item_count_text.text = main_item_count.ToString();
        Dictionary<Resources_names, int> componentsdict = Gamemanager.instance.resources_dict[main_item_resource_name];
        List<(Resources_names, int)> changes = new List<(Resources_names, int)>();
        foreach (KeyValuePair<Resources_names, int> kvp in componentsdict)
        {
            //changes.Add((kvp.Key, kvp.Value));
            Resources_names resource = kvp.Key;
            int resource_value = kvp.Value * main_item_count / main_item_minstack;
            GameObject main_component = Instantiate(component_prefab, object_components_panel.transform);
            main_component.GetComponent<Componentpanel>().Component_init(resource, resource_value,
                Gamemanager.instance.Get_sprite_for_resource(resource));

        }

        simple_resources = Gamemanager.instance.Get_simple_recources(main_item_resource_name);
        foreach (KeyValuePair<Resources_names, int> kvp in simple_resources)
        {
            GameObject simple_component = Instantiate(simple_component_prefab, simple_components_panel.transform);
            simple_component.GetComponent<Componentpanel>().Component_init(kvp.Key,
                kvp.Value * main_item_count / main_item_minstack,
                Gamemanager.instance.Get_sprite_for_resource(kvp.Key));
        }
    }

    public void Button_disactivation_panel()
    {
        if (panel_is_disactive == true)
        { 
            panel_is_disactive = false;
            print("disactivated");
            Button_disactivation_image.color = new Color(0, 0, 0, 0);
        }
        
        else
        {
            panel_is_disactive = true;
            print("activate");
            Button_disactivation_image.color = new Color(0, 0, 0, 0.8f);
        }
        FindObjectOfType<UIManager>().UpdateResourcePanelsForCalculate();
    }
        
    
}