using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scenes;

namespace Assets.Scenes
{
    public class Setting_Status
    {
        Image hp_fill;
        Image hgp_fill;
        Image tp_fill;

        static bool ispoison;
        static bool isbleed;
        static bool iscold;


        public Setting_Status()
        {
            hp_fill = GameObject.Find("HP_Fillgage").GetComponent<Image>();
            hgp_fill = GameObject.Find("HGP_Fillgage").GetComponent<Image>();
            tp_fill = GameObject.Find("TP_Fillgage").GetComponent<Image>();

            ispoison = false;
            isbleed = false;
            iscold = false;
        }
        /// <summary>
        /// 상태 바 조절 함수
        /// </summary>
        /// <param name="st">"HP", "HGP", "TP" 세가지 이며 채력, 배고픔, 목마름 순이다</param>
        /// <param name="point">변동폭 이며 깎으려면 음수 더하려면 양수 넣으면 된다</param>
        public void ChangeStatusBar(string st, int point)
        {
            switch(st)
            {
                case "HP":
                    hp_fill.fillAmount += point / 100.0f;
                    break;
                case "HGP":
                    hgp_fill.fillAmount += point / 100.0f;
                    break;
                case "TP":
                    tp_fill.fillAmount += point / 100.0f;
                    break;
            }
        }
        /// <summary>
        /// 시간에 따른 배고픔, 목마름 감소 함수
        /// </summary>
        /// <param name="wait">감소 주기</param>
        /// <param name="hgp">배고픔 감소량</param>
        /// <param name="tp">목마름 감소량</param>
        /// <returns></returns>
        public IEnumerator HungerAndThirst(WaitForSeconds wait, int hgp, int tp)
        {
            while(true)
            {
                yield return wait;
                ChangeStatusBar("HGP", -hgp);
                ChangeStatusBar("TP", -tp);
            }
        }
        /// <summary>
        /// 중독 상태인지 확인 해서 채력 닳는 함수
        /// </summary>
        /// <param name="wait_poison">중독시 채력 닳는 주기</param>
        /// <param name="wait_fix">WaitForFixedUpdate</param>
        /// <param name="hp">채력 감소량(양수)</param>
        /// <param name="count">중독으로 인한 채력감소 횟수</param>
        /// <returns></returns>
        public IEnumerator StatusPoison(WaitForSeconds wait_poison, WaitForFixedUpdate wait_fix, int hp, int count)
        {
            int i;
            while (true)
            {
                yield return wait_fix;
                if (ispoison)
                {
                    i = 0;
                    while(i < count)
                    {
                        yield return wait_poison;
                        if (!ispoison)
                        {
                            break;
                        }
                        ChangeStatusBar("HP", -hp);
                    }
                    ispoison = false;
                }
                yield return wait_fix;
            }
        }
        /// <summary>
        /// 중독 상태변환 함수
        /// </summary>
        /// <param name="_bool">true = 중독</param>
        public void SetPoison(bool _bool)
        {
            ispoison = _bool;
        }
        /// <summary>
        /// 출혈 상태 인지 확인해서 채력감소 하는 함수
        /// </summary>
        /// <param name="wait_bleed">출혈 주기</param>
        /// <param name="wait_fix">WaitForFixedUpdate</param>
        /// <param name="hp">채력 감소량</param>
        /// <param name="count">출혈로 인한 채력감소 횟수</param>
        /// <returns></returns>
        public IEnumerator StatusBleed(WaitForSeconds wait_bleed, WaitForFixedUpdate wait_fix, int hp, int count)
        {
            int i;
            while (true)
            {
                yield return wait_fix;
                if (isbleed)
                {
                    i = 0;
                    while (i < count)
                    {
                        yield return wait_bleed;
                        if (!isbleed)
                        {
                            break;
                        }
                        ChangeStatusBar("HP", -hp);
                    }
                    isbleed = false;
                }
                yield return wait_fix;
            }
        }
        /// <summary>
        /// 출혈 상태변환 함수
        /// </summary>
        /// <param name="_bool">true = 출혈</param>
        public void SetBleed(bool _bool)
        {
            isbleed = _bool;
        }
        /// <summary>
        /// 저체온 상태이상인지 판단하여 채력 감소하는 함수
        /// </summary>
        /// <param name="wait_cold">채력 감소 주기</param>
        /// <param name="wait_fix">WaitForFixedUpdate</param>
        /// <param name="hp">채력 감소량</param>
        /// <param name="count">채력감소 횟수</param>
        /// <returns></returns>
        public IEnumerator StatusCold(WaitForSeconds wait_cold, WaitForFixedUpdate wait_fix, int hp, int count)
        {
            int i;
            while (true)
            {
                yield return wait_fix;
                if (iscold)
                {
                    i = 0;
                    while (i < count)
                    {
                        yield return wait_cold;
                        if (!iscold)
                        {
                            break;
                        }
                        ChangeStatusBar("HP", -hp);
                    }
                    iscold = false;
                }
                yield return wait_fix;
            }
        }
        /// <summary>
        /// 저체온 상태이상 변환 함수
        /// </summary>
        /// <param name="_bool">true = 저체온증</param>
        public void SetCold(bool _bool)
        {
            iscold = _bool;
        }
    }
}
