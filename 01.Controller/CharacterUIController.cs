namespace DREADBORN
{
    //System
    using System;
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    //TMP
    using TMPro;

    //Photon
    using Photon.Pun;
    using Photon.Realtime;
    using Hashtable = ExitGames.Client.Photon.Hashtable;

    //Project
    using static CustomPropertyKey;
    using Sound;

    public class CharacterUIController : MonoBehaviourPunCallbacks
    {
        #region 변수
        [Header("Info")]
        [SerializeField] private Canvas playerUICanvas;
        [Space(10)]
        [SerializeField] private Image skillIcon;
        [SerializeField] private Image skillIconBackGround;
        [SerializeField] private Image playerIconPanel;
        [SerializeField] private Image interactionHoldGauge;
        [SerializeField] private Image hpSlider;
        [SerializeField] private Image defendPointSlider;
        [Space(10)]
        [SerializeField] private Button leaveRoomButton;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI roomCodeText;
        [Space(10)]
        [SerializeField] private GameObject interactionGauge;
        [SerializeField] private GameObject settingPanel;
        [SerializeField] private GameObject interactionKeyIcon;
        [SerializeField] private GameObject hitMarker;
        [SerializeField] private GameObject otherPlayerUIPrefab;
        [Space(10)]
        [SerializeField] private Transform uiParent;
        [Header("SFX")]
        [SerializeField] private AudioClip buttonClickSFX;

        private Dictionary<int, GameObject> otherPlayerUIs = new Dictionary<int, GameObject>();

        private CharacterManager manager;

        #region 프로퍼티
        public Canvas PlayerUICanvas => playerUICanvas;
        #endregion

        #endregion

        //초기화
        public void Initialize()
        {
            manager = GetComponent<CharacterManager>();

            if (manager.photonView.IsMine)
            {
                if (PhotonNetwork.IsConnected)
                {
                    roomCodeText.text = PhotonNetwork.CurrentRoom.Name;
                }

                nameText.text = manager.CharacterType.ToString();

                playerIconPanel.sprite = manager.IconSprite;

                skillIcon.sprite = manager.SkillIcon;
                skillIconBackGround.sprite = manager.SkillIcon;

                interactionGauge.SetActive(false);

                leaveRoomButton.onClick.AddListener(LeaveRoomButton);

                foreach (Player other in PhotonNetwork.PlayerListOthers)
                {
                    CreateOtherPlayerUI(other);
                }
            }
        }

        //다른 플레이어 정보를 보여주는 UI 생성
        private void CreateOtherPlayerUI(Player player)
        {
            //이미 UI가 생성되어 있으면 생성하지 않음
            if (otherPlayerUIs.ContainsKey(player.ActorNumber)) return;

            //UI 생성
            GameObject uiObj = Instantiate(otherPlayerUIPrefab, uiParent);
            uiObj.GetComponent<OtherPlayerUIController>().Initialize(player);
            otherPlayerUIs[player.ActorNumber] = uiObj;

        }

        //타이틀로 돌아가는 버튼
        private void LeaveRoomButton()
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);

            Hashtable CP = new();

            CP[PlayerType] = (CharacterType.None).ToString();

            PhotonNetwork.LocalPlayer.SetCustomProperties(CP);

            NetworkManager.Instance.SetNewAutomaticallySyncSceneState(false);

            //모든 플레이어가 씬을 같이 넘어가지 않게함
            FadeManager.Instance.FadeOut(() =>
            {
                PhotonNetwork.LeaveRoom();
            });
        }

        //히트 마커를 활성화
        public void EnableHitMarker()
        {
            if (hitMarker == null) return;
            hitMarker.SetActive(false);
            hitMarker.SetActive(true);
        }

        #region UI 업데이트
        //UI값 업데이트
        public void UpdatePlayerUI()
        {
            //Hp 슬라이더 값 업데이트
            hpSlider.fillAmount = manager.CurrentHp / manager.MaxHp;

            //DefendPoint 슬라이더 값 업데이트
            defendPointSlider.fillAmount = manager.CurrentDefensePoint / manager.MaxDefensePoint;


            //Level 텍스트 업데이트
            levelText.text = manager.Level.ToString();
        }

        //상호작용 게이지를 업데이트함
        public void UpdateInteractionGauge(float current, float max = 1f)
        {
            if(max <= 0)
            {
                interactionGauge.SetActive(false);
                return;
            }

            if(current > 0)
            {
                interactionGauge.SetActive(true);
            }

            interactionHoldGauge.fillAmount = current / max;
        }

        //Skill 게이지 업데이트
        public void UpdateSkillUI(float current, float max)
        {
            skillIcon.fillAmount = current / max;
        }
        #endregion

        #region UI 토글
        //셋팅 패널을 토글함
        public void ToggleSettingPanel()
        {
            if (!manager.photonView.IsMine) return;

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                settingPanel.SetActive(!settingPanel.activeSelf);
                manager.IsPaused = settingPanel.activeSelf;
            }

            if(settingPanel.activeSelf)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

        }

        //상호작용 키를 토클함
        public void ToggleInteractionKeyIcon(bool trigger)
        {
            interactionKeyIcon.SetActive(trigger);
        }

        #endregion

        //다른 플레이어가 들어왔을때 UI생성
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            CreateOtherPlayerUI(newPlayer);
        }

        //다른 플레이어 정보를 보여주는 UI 삭제
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            //UI 삭제
            if (otherPlayerUIs.TryGetValue(otherPlayer.ActorNumber, out GameObject ui))
            {
                Destroy(ui);
                otherPlayerUIs.Remove(otherPlayer.ActorNumber);
            }
        }
    }
}