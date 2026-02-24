using UnityEngine;

public class Weaponchange : MonoBehaviour
{
    Gamemanager gamemanager;
    UIManager uimanager;
    [SerializeField] GameObject weaponoffimage;
    [SerializeField] Resources_names weaponname;
    public bool weaponon = true;
    public void weapontoggle()
    {
        if (weaponon)
        {
            foreach (var weaponstate in gamemanager.weaponsforconstructionsstate)
            {
                if (weaponstate.Value == true && weaponstate.Key != weaponname)
                {
                    weaponon = false;
                    weaponoffimage.SetActive(true);
                    break;
                }
            }
        }
        else
        {
            weaponon = true;
            weaponoffimage.SetActive(false);
        }
        uimanager.ClearPanelWeaponsForAllConstructions();
        gamemanager.weaponsforconstructionsstate[weaponname] = weaponon;
        uimanager.CreateConstructionPanelsForCalculate();
    }
    private void Start()
    {
        gamemanager = FindObjectOfType<Gamemanager>();
        uimanager = FindObjectOfType<UIManager>();
    }
}
