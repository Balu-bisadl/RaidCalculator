using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum Resources_names
{
    explosive_bullet, rocket, C4, satchel, beancan, sulfur, charcoal, gunpowder, explosives, lowgradefuel, frags, pipe, rope, stash, techparts, cloth, propane_bomb,propane
}
public enum Constructions_names
{
    wooden_wall, stone_wall,metal_wall,hqm_wall, metal_door, metal_double_door, hqm_door, hqm_double_door, wooden_door, wooden_double_door, high_wooden_wall,high_stone_wall,garage_door,shopfront,window,armored_window,metal_bars,wooden_bars,fence_wall,fence_gate,cell,barricade
}
[Serializable]
public class WeaponForConstruction
{
    public Resources_names weapon_name;
    public float weapon_damage;
}
[Serializable] 
public class WeaponSettings
{
    public Resources_names weapon_name;
    public Sprite weapon_image;
}
[Serializable]
public class ResourceSettings
{
    public Resources_names resources_name;
    public Sprite resource_image;
    public int minstack = 1;
    //������� ������� ���������� ������� �� ������� ((n//minstack)+1) x minstack, ���� n �� ������� ������ �� �������
}
[Serializable]
public class ConstructionSettings
{
    public Constructions_names constructions_name;
    public Sprite construction_image;
    public float constructionhp;
    public List<WeaponForConstruction> weaponsforconstruction = new List<WeaponForConstruction>();
    
    //������� ������� ���������� ������� �� ������� ((n//minstack)+1) x minstack, ���� n �� ������� ������ �� �������
}
public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;
    public Dictionary<Resources_names, Dictionary<Resources_names, int>> resources_dict = new Dictionary<Resources_names, Dictionary<Resources_names, int>>();
    [SerializeField] public List<ResourceSettings> resources_settings;
    [SerializeField] public List<ConstructionSettings> constructions_settings;
    [SerializeField] public List<WeaponSettings> weapon_settings;

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
        return null;
    }
    public Sprite Get_sprite_for_construction(Constructions_names construction_name)
    {
        foreach (var pair in constructions_settings)
        {
            if (pair.constructions_name == construction_name)
            {
                return pair.construction_image;
            }
        }
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
    //Это функция определения всех типов и количества оружия для уничтожения конструкции
    public Dictionary<WeaponForConstruction, int> CalculateweaponsforDestroy(Constructions_names construction, int construction_count)
    {
        Dictionary<WeaponForConstruction, int> weaponsfordestroy = new Dictionary<WeaponForConstruction, int>();
        // формирования списка оружия для уничтожения конструкции 
        foreach (ConstructionSettings constructionSetting in constructions_settings)
        {
            if (constructionSetting.constructions_name == construction)
            {
                float constructionhp = constructionSetting.constructionhp; // определяем хп кострукции 
                List<WeaponForConstruction> weaponsforconstructionsorted = WeaponsForConstructionSorting(constructionSetting.weaponsforconstruction); // сортировка оружия по возрастанию урона
                int weaponindex = weaponsforconstructionsorted.Count-1; // выбираем оружие с наибольшим уроном
                while (constructionhp > 0)
                {
                    // если при нанесении урона данным оружием приведет к избыточному урону то переключаемся на тип оружия с меньшим урном(если это не слабейшее оружие)
                    if (constructionhp - weaponsforconstructionsorted[weaponindex].weapon_damage<0)
                    {
                        if (weaponindex!= 0)
                        {
                            weaponindex -= 1;
                        }
                        else
                        {
                            constructionhp -= weaponsforconstructionsorted[weaponindex].weapon_damage;
                            // добавить оружие в словарь
                            if (weaponsfordestroy.ContainsKey(weaponsforconstructionsorted[weaponindex]))
                            {
                                weaponsfordestroy[weaponsforconstructionsorted[weaponindex]] += construction_count;
                            }
                            else
                            {
                                weaponsfordestroy.Add(weaponsforconstructionsorted[weaponindex], construction_count);
                            }
                        }
                    }
                    else // наносим урон данный типом оружия
                    {
                        constructionhp -= weaponsforconstructionsorted[weaponindex].weapon_damage;
                        // добавить оружие в словарь 
                        if (weaponsfordestroy.ContainsKey(weaponsforconstructionsorted[weaponindex]))
                        {
                            weaponsfordestroy[weaponsforconstructionsorted[weaponindex]] += construction_count;
                        }
                        else
                        {
                            weaponsfordestroy.Add(weaponsforconstructionsorted[weaponindex], construction_count);
                        }

                    }
                }
            }  
        }
        return weaponsfordestroy;
    }
    List<WeaponForConstruction> WeaponsForConstructionSorting(List<WeaponForConstruction> weaponsforconstruction)
    {
        List<WeaponForConstruction> SortedList = weaponsforconstruction.OrderBy(w=>w.weapon_damage).ToList();
        return SortedList;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
