namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    //TMP
    using TMPro;

    //Photon
    using Photon.Pun;

    //Project
    using static SceneName;

    public class PlayerUIController : MonoBehaviourPunCallbacks
    {
        [Header("Info")]
        [SerializeField] private Canvas playerUICanvas;
        [Space(10)]
        [SerializeField] private Image playerIconPanel;
        [Space(10)]
        [SerializeField] private Slider hpSlider;
        [SerializeField] private Slider defendPointSlider;
        [Space(10)]
        [SerializeField] private Button leaveRoomButton;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI roomCodeText;
        [Space(10)]
        [SerializeField] private GameObject settingPanel;
        [SerializeField] private GameObject interactionKeyIcon;


        private PlayerManager manager;


        //초기화
        public void Initialize()
        {
            manager = GetComponent<PlayerManager>();

            if(!manager.photonView.IsMine)
            {
                Destroy(playerUICanvas.gameObject);
            }
            else
            {
                if(PhotonNetwork.IsConnected)
                {
                    roomCodeText.text = PhotonNetwork.CurrentRoom.Name;
                }

                playerIconPanel.sprite = manager.PlayerIconSprite;
                leaveRoomButton.onClick.AddListener(LeaveRoomButton);
            }

        }

        //타이틀로 돌아가는 버튼
        private void LeaveRoomButton()
        {
            //모든 플레이어가 씬을 같이 넘어가지 않게함
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.LeaveRoom();
        }

        //UI값 업데이트
        public void UpdateUIValue()
        {
            //Hp 슬라이더 값 업데이트
            hpSlider.value = manager.CurrentHp / manager.MaxHp;

            //DefendPoint 슬라이더 값 업데이트
            defendPointSlider.value = manager.CurrentDefendPoint / manager.MaxDefendPorint;
        }


        //셋팅 패널을 토글함
        public void ToggleSettingPanel()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                settingPanel.SetActive(!settingPanel.activeSelf);
                manager.isPause = settingPanel.activeSelf;
            }
        }

        public void ToggleInteractionKeyIcon(bool trigger)
        {
            interactionKeyIcon.SetActive(trigger);
        }
    }
}