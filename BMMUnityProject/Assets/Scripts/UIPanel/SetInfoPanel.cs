using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UI;

public class SetInfoPanel : BasePanel
{
    private InputField nameIF;
    private InputField passwordIF;
    private InputField rePasswordIF;

    private Button conBtn;
    private Button cancelBtn;
    private Button tipsButton;


    private SetInfoRequest setInfoRequest;



    public override void InitPanelThings()
    {
        transform.Find("conBtn").GetComponent<Button>().onClick.AddListener(OnConBtnClick);
        transform.Find("cancelBtn").GetComponent<Button>().onClick.AddListener(OnCancelBtnClick);
        tipsButton = transform.Find("TipsButton").GetComponent<Button>();
        tipsButton.onClick.AddListener(() => OnTipsClick());
        nameIF = transform.Find("nameIF").GetComponent<InputField>();
        passwordIF = transform.Find("passwordIF").GetComponent<InputField>();
        rePasswordIF = transform.Find("rePasswordIF").GetComponent<InputField>();
        tipsButton.gameObject.SetActive(false);
        setInfoRequest = GetComponent<SetInfoRequest>();
    }


    private void OnTipsClick()
    {
        tipsButton.gameObject.SetActive(false);
    }

    private void OnConBtnClick()
    {
        if (nameIF.text != string.Empty)
        {
            if (passwordIF.text.Equals(string.Empty))
            {
                setInfoRequest.SendRequest(Facade.Instance.GetUserID().ToString(), nameIF.text, string.Empty);
            }
            else if (passwordIF.text.Equals(rePasswordIF.text))
            {
                setInfoRequest.SendRequest(Facade.Instance.GetUserID().ToString(), nameIF.text, passwordIF.text);
            }
            else
            {
                tipsButton.transform.Find("Text").GetComponent<Text>().text = "请输入相同的密码";
                tipsButton.gameObject.SetActive(true);
            }
        }
        else if (passwordIF.text != string.Empty)
        {
            if (passwordIF.text.Equals(rePasswordIF.text))
            {
                setInfoRequest.SendRequest(Facade.Instance.GetUserID().ToString(), nameIF.text, passwordIF.text);
            }
            else
            {
                tipsButton.transform.Find("Text").GetComponent<Text>().text = "请输入相同的密码";
                tipsButton.gameObject.SetActive(true);

            }
        }
    }

    //回应
    public void OnSetInfoReponse(ReturnCode returnCode)
    {
        uiMng.SyncPopPanel();

        if (returnCode == ReturnCode.Success)
        {
            //uiMng.SyncPopPanel();
            if (nameIF.text != string.Empty)
                Facade.Instance.SetNickName(nameIF.text);
            Debug.Log("修改成功");
            //OnCancelBtnClick();
        }
        else
        {
            //uiMng.SyncShowPanelMessage(UIPanelType.RegPanel, "注册失败，请稍后再试");
        }
    }

    //返回上个页面 
    private void OnCancelBtnClick()
    {
        uiMng.PopPanel();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
    }

    public override void OnResume()
    {
        base.OnResume();
        gameObject.SetActive(true);
    }

    public override void OnPause()
    {
        gameObject.SetActive(false);
    }

    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }
}