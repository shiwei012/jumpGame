﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.Networking;


public class yourName : MonoBehaviour {

	public InputField name;
	public Text tip;
	public Button go;

	FileUtils fileUtils;
	HttpUtils httpUtils;
	/*
	name
	*/
	private static readonly string CONF_FILE = "config";

	private static readonly string WEB_SERVER_URL = "http://192.168.45.130:5000";

	float _waitTime = 2f;//前后两次按退出间隔时间
    void OnGUI() {
        if (_waitTime < 2) {
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 30, 200, 60), "再按一次退出");
            _waitTime -= Time.deltaTime;
            if (_waitTime < 0) {
                _waitTime = 2;
            }
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Application.Quit();
            }
        }
        if (Input.GetKeyUp(KeyCode.Escape) && _waitTime == 2) {
            _waitTime -= Time.deltaTime;
        }
    }

	public IEnumerator register(string nameStr) {
    	// validate
        UnityWebRequest www = UnityWebRequest.Get(WEB_SERVER_URL + "/register?name=" + nameStr);
		yield return www.Send();
		if (www.isError) {
            Debug.Log(www.error);
            setTips("无法连接服务器");
        } else {
            if (www.downloadHandler.text == "fail") {
            	setTips("请换一个名字");
            } else if (www.downloadHandler.text == "success") {
            	ArrayList lines = fileUtils.readFileToLines(CONF_FILE);
            	if (lines == null) {
            		fileUtils.writeFile(CONF_FILE, name.text);
            	} else {
            		fileUtils.writeFile(CONF_FILE, name.text + "\n");
            		foreach (string line in lines) {
            			fileUtils.writeFile(CONF_FILE, line + "\n");
            		}
            	}
            	//Application.LoadLevel("menu");
            	MasterSceneManager.Instance.LoadNext("menu");
            	setTips("Application.LoadLeve menu");
            }
        }
    }

	void initUI() {
		tip.gameObject.SetActive(false);
	}

	IEnumerator closeTips() {
		yield return new WaitForSeconds(1);
		tip.text = "";
		tip.gameObject.SetActive(false);
	}

	void setTips(string text) {
		tip.text = text;
		tip.gameObject.SetActive(true);
		StartCoroutine(closeTips());
	}

	// Use this for initialization
	void Start () {
		initUI();
		fileUtils = GetComponent<FileUtils>();
		ArrayList lines = fileUtils.readFileToLines(CONF_FILE);
		string nameTxt = "你的名字";
		bool flag = false;
		if (lines != null) {
			print("conf: ");
			for (int i = 0; i < lines.Count; i++) {
				print(lines[i]);
				if (i == 0) {
					nameTxt = (string) lines[i];
					flag = true;
				}
			}
		}
		name.placeholder.GetComponent<Text>().text = nameTxt;
		if (flag) {
			MasterSceneManager.Instance.LoadNext("menu");
		}
	}

	public void onGoBtnClick() {
		print("go clicked");
		if (string.IsNullOrEmpty(name.text)) {
			setTips("还不能走，请输入你的名字");
			return;
		}
		StartCoroutine(register(name.text));
	}

	public void onBackBtnClick() {
		MasterSceneManager.Instance.LoadPrevious();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
