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


        //�ʱ�ȭ
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

        //Ÿ��Ʋ�� ���ư��� ��ư
        private void LeaveRoomButton()
        {
            //��� �÷��̾ ���� ���� �Ѿ�� �ʰ���
            PhotonNetwork.AutomaticallySyncScene = false;

            PhotonNetwork.LeaveRoom();

        }

        //UI�� ������Ʈ
        public void UpdateUIValue()
        {
            //Hp �����̴� �� ������Ʈ
            hpSlider.value = manager.CurrentHp / manager.MaxHp;

            //DefendPoint �����̴� �� ������Ʈ
            defendPointSlider.value = manager.CurrentDefendPoint / manager.MaxDefendPorint;
        }

        //���� �г��� �����
        public void ToggleSettingPanel()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                settingPanel.SetActive(!settingPanel.activeSelf);
                manager.isPause = settingPanel.activeSelf;
            }
        }
    }
}