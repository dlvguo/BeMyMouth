using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeachPanel : BasePanel
{

    public InputField gestureInputField;

    //提示按钮
    public Button tipBtu;
    public Button exitBtu;
    public GameObject yesImg, noImg;
    public AniManager aniManager;

    // Start is called before the first frame update
    void Start()
    {
        yesImg.SetActive(false);
        noImg.SetActive(false);
        tipBtu.gameObject.SetActive(false);
        //关闭
        tipBtu.onClick.AddListener(() => tipBtu.gameObject.SetActive(false));
        exitBtu.onClick.AddListener(() => Facade.Instance.PopUIPanel());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (gestureInputField.text == string.Empty)
            {
                tipBtu.gameObject.SetActive(true);
            }
            else
            {
                //RecognizeGes(GetLeapController().Frame());
            }
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (gestureInputField.text.Length > 0 && !aniManager.IsPlay)
            {
                aniManager.ProcessQueue(gestureInputField.text.ToCharArray());
            }

        }
    }
}
