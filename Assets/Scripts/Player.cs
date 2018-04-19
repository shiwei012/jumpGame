﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System.IO;
using LitJson;

namespace PlayerJson {
	public class JsonHelper {

		public struct Player {
			public string playerId;
			public string playerName;
			public GameObject player;
			public GameObject playerLabel;
			public TextMesh playerLabelTextMesh;
			public GameObject playerPos;
			public Vector3 prePlayerPosition;
			public float VSpeed;
			public float power;
			public int currentPlatformIndex;
			// 决定跳的方向
			public bool direction;
			// 动画播放队列
			public List<string> animationQueue;
			public Object queueLock;
			public bool syncPosFlag;
			public int index;
		};

		public static void test() {
			string s = "{'name': 'david'}";
			LitJson.JsonData x = LitJson.JsonMapper.ToObject(s);
			Debug.Log(x["name"]);
		}


		public static string animJson(string action, Dictionary<string, Player> playerList, string index) {
			string ret = System.String.Format("{{'action': '{0}', 'x': {1}, 'y': {2}, 'z': {3}, 'vspeed': {4}, 'power': {5}, 'index': {6}, 'direction': '{7}'}}", action, 
				playerList[index].prePlayerPosition.x, playerList[index].prePlayerPosition.y, playerList[index].prePlayerPosition.z,
				playerList[index].VSpeed,
				playerList[index].power,
				playerList[index].currentPlatformIndex,
				playerList[index].direction);
			//Debug.Log("animJson: " + ret);
			return ret;
		}
		/*
		public static void animJson(
			string action,
			float prePlayerPositionX,
			float prePlayerPositionY, 
			float prePlayerPositionZ, 
			float VSpeed, 
			float power, 
			int currentPlatformIndex, 
			bool direction) {

		}
		*/
	}
}
