﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using Mono.Data.Sqlite;
using Assets.Scenes;

namespace Assets.Scenes
{
    class Dispancer
    {
        /// <summary>
        /// 나무 오브젝트들 받아서 리스트로 반환하는 함수
        /// </summary>
        /// <returns>List<GameObject></returns>
        public List<GameObject> TreeDispanceList()
        {
            List<GameObject> list = new List<GameObject>();
            int i = 0;
            while(true)
            {
                if(GameObject.Find("Tree" + i) == null)
                {
                    break;
                }
                list.Add(GameObject.Find("Tree" + i));
                i++;
            }
            return list;
        }
    }
}