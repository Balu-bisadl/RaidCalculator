using UnityEngine;
using UnityEngine.UI;

public class PanelInfoController : MonoBehaviour
{
    [SerializeField] GameObject Panelinfo;
    [SerializeField] Text Panelinfo_text;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = Panelinfo.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenPanel(string message)
    {
        Panelinfo_text.text = message;
        Panelinfo.SetActive(true);
        animator.SetBool("open",true);

    }
    public void ClosePanel()
    {
        animator.SetBool("open",false);
        
    }
}
