using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Resources_names
{
    explosive_bullet, rocket, C4, satchel, beancan, sulfur, charcoal, gunpowder, explosives, lowgradefuel, frags, pipe, rope, stash, techparts, cloth, propane_bomb,propane
}
[Serializable]
public class ResourceSettings
{
    public Resources_names resources_name;
    public Sprite resource_image;
    public int minstack = 1;
    //������� ������� ���������� ������� �� ������� ((n//minstack)+1) x minstack, ���� n �� ������� ������ �� �������
}
public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;
    public Dictionary<Resources_names, Dictionary<Resources_names, int>> resources_dict = new Dictionary<Resources_names, Dictionary<Resources_names, int>>();
    [SerializeField] public List<ResourceSettings> resources_settings;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        resources_dict[Resources_names.explosive_bullet] = new Dictionary<Resources_names, int>
        {
            {Resources_names.sulfur, 10},
            {Resources_names.frags, 10},
            {Resources_names.gunpowder, 20}
        };
        resources_dict[Resources_names.gunpowder] = new Dictionary<Resources_names, int>
        {
            {Resources_names.sulfur, 2},
            {Resources_names.charcoal, 3}
        };
        resources_dict[Resources_names.explosives] = new Dictionary<Resources_names, int>
        {
            {Resources_names.sulfur, 10},
            {Resources_names.gunpowder, 50},
            {Resources_names.lowgradefuel, 3},
            {Resources_names.frags, 10}
        };
        resources_dict[Resources_names.rocket] = new Dictionary<Resources_names, int>
        {
            {Resources_names.gunpowder, 150},
            {Resources_names.explosives, 10},
            {Resources_names.pipe, 2}
        };
        resources_dict[Resources_names.C4] = new Dictionary<Resources_names, int>
        {
            {Resources_names.explosives, 20},
            {Resources_names.cloth, 5},
            {Resources_names.techparts, 2}
        };
        resources_dict[Resources_names.beancan] = new Dictionary<Resources_names, int>
        {
            {Resources_names.gunpowder, 60},
            {Resources_names.frags, 20}
        };
        resources_dict[Resources_names.stash] = new Dictionary<Resources_names, int>
        {
            {Resources_names.cloth, 10}
        };
        resources_dict[Resources_names.satchel] = new Dictionary<Resources_names, int>
        {
            {Resources_names.beancan, 4},
            {Resources_names.stash, 1},
            {Resources_names.rope, 1}
        };
        resources_dict[Resources_names.propane_bomb] = new Dictionary<Resources_names, int>
        {
            {Resources_names.gunpowder, 450},
            {Resources_names.propane, 1},
            {Resources_names.lowgradefuel, 20}
        };
    }
    public Sprite Get_sprite_for_resource(Resources_names resource_name)
    {
        foreach (var pair in resources_settings)
        {
            if (pair.resources_name == resource_name)
            {
                return pair.resource_image;
            }
        }
        print("���������� ����������� ��� �������");
        return null;
    }
    public int Get_minstak_for_resource(Resources_names resource_name)
    {
        foreach (var pair in resources_settings)
        {
            if (pair.resources_name == resource_name)
            {
                return pair.minstack;
            }
        }
        print("�� ��������� ������������ ������");
        return 0;
    }
    public Dictionary<Resources_names, int> Get_simple_recources(Resources_names resource_name)
    {
        Dictionary<Resources_names, int> basecomponents = new Dictionary<Resources_names, int>(resources_dict[resource_name]);
        List <(Resources_names, int )> changes = new List<(Resources_names, int)>();
        Dictionary<Resources_names, int> result = new Dictionary<Resources_names, int>();
        foreach (KeyValuePair<Resources_names, int> kvp in basecomponents)
        {
            Resources_names resource = kvp.Key;
            int Count = kvp.Value;
            
            if (resources_dict.ContainsKey(resource)) //���� ������ ������� �� ������ ��������
            {
                Dictionary<Resources_names, int> children_components = Get_simple_recources(resource); // ������� ��� ���� �������� �� ������� �� �������
                foreach(var child in children_components)
                {
                    if (result.ContainsKey(child.Key))
                    {
                        result[child.Key] += child.Value*Count;
                    }
                    else
                    {
                        result[child.Key] = child.Value*Count;
                    }
                }
                
                //���� ������ �� ������� �� ����� ��� ���������� �� ������� �� ������� � ������� ����������� �������� ��� ���, ����� �������� ���� ��������� � ������� � �������� ������������, � ���� �� ��� ��� ���� ������ �������� ����������
            }
            else // ���� ������ �������
            {
                if (result.ContainsKey(resource))
                {
                    result[resource] += Count;
                }
                else
                {
                    result[resource] = Count;
                }
            }
        }
        return result;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
