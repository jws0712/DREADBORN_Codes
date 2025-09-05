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
        #region ����
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

        #region ������Ƽ
        public Canvas PlayerUICanvas => playerUICanvas;
        #endregion

        #endregion

        //�ʱ�ȭ
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

        //�ٸ� �÷��̾� ������ �����ִ� UI ����
        private void CreateOtherPlayerUI(Player player)
        {
            //�̹� UI�� �����Ǿ� ������ �������� ����
            if (otherPlayerUIs.ContainsKey(player.ActorNumber)) return;

            //UI ����
            GameObject uiObj = Instantiate(otherPlayerUIPrefab, uiParent);
            uiObj.GetComponent<OtherPlayerUIController>().Initialize(player);
            otherPlayerUIs[player.ActorNumber] = uiObj;

        }

        //Ÿ��Ʋ�� ���ư��� ��ư
        private void LeaveRoomButton()
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);

            Hashtable CP = new();

            CP[PlayerType] = (CharacterType.None).ToString();

            PhotonNetwork.LocalPlayer.SetCustomProperties(CP);

            NetworkManager.Instance.SetNewAutomaticallySyncSceneState(false);

            //��� �÷��̾ ���� ���� �Ѿ�� �ʰ���
            FadeManager.Instance.FadeOut(() =>
            {
                PhotonNetwork.LeaveRoom();
            });
        }

        //��Ʈ ��Ŀ�� Ȱ��ȭ
        public void EnableHitMarker()
        {
            if (hitMarker == null) return;
            hitMarker.SetActive(false);
            hitMarker.SetActive(true);
        }

        #region UI ������Ʈ
        //UI�� ������Ʈ
        public void UpdatePlayerUI()
        {
            //Hp �����̴� �� ������Ʈ
            hpSlider.fillAmount = manager.CurrentHp / manager.MaxHp;

            //DefendPoint �����̴� �� ������Ʈ
            defendPointSlider.fillAmount = manager.CurrentDefensePoint / manager.MaxDefensePoint;


            //Level �ؽ�Ʈ ������Ʈ
            levelText.text = manager.Level.ToString();
        }

        //��ȣ�ۿ� �������� ������Ʈ��
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

        //Skill ������ ������Ʈ
        public void UpdateSkillUI(float current, float max)
        {
            skillIcon.fillAmount = current / max;
        }
        #endregion

        #region UI ���
        //���� �г��� �����
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

        //��ȣ�ۿ� Ű�� ��Ŭ��
        public void ToggleInteractionKeyIcon(bool trigger)
        {
            interactionKeyIcon.SetActive(trigger);
        }

        #endregion

        //�ٸ� �÷��̾ �������� UI����
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            CreateOtherPlayerUI(newPlayer);
        }

        //�ٸ� �÷��̾� ������ �����ִ� UI ����
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            //UI ����
            if (otherPlayerUIs.TryGetValue(otherPlayer.ActorNumber, out GameObject ui))
            {
                Destroy(ui);
                otherPlayerUIs.Remove(otherPlayer.ActorNumber);
            }
        }
    }
}