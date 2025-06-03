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
        [SerializeField] private Canvas playerUICanvas = null;
        [Space(10)]
        [SerializeField] private Image playerIconPanel = null;
        [Space(10)]
        [SerializeField] private Slider hpSlider = null;
        [SerializeField] private Slider defendPointSlider = null;
        [Space(10)]
        [SerializeField] private Button leaveRoomButton = null;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI roomCodeText = null;
        [Space(10)]
        [SerializeField] private GameObject settingPanel = null;

        private PlayerManager manager = null;


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

        //방에서 나오는 버튼
        private void LeaveRoomButton()
        {
            GameManager.Instance.SetAutomaticallySyncScene(false);
            SceneManager.LoadScene(TitleScene);
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
            }
        }
    }
}