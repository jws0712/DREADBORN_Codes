namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    //Photon
    using Photon.Pun;
    using Photon.Realtime;
    using Hashtable = ExitGames.Client.Photon.Hashtable;

    //TMP
    using TMPro;

    //Project
    using static SceneName;
    using Sound;

    public class ResultSceneUIManager : MonoBehaviourPun
    {
        [Header("Info")]
        [SerializeField] private GameObject resultScenePlayerUIPrefab;
        [SerializeField] private Transform uiParent;
        [SerializeField] private Image underField;
        [SerializeField] private Button leaveButton;
        [SerializeField] private Button continuousButton;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private Color failColor;
        [SerializeField] private Color clearColor;
        [Header("SFX")]
        [SerializeField] private AudioClip buttonClickSFX;

        private void Start()
        {
            leaveButton.onClick.AddListener(LeaveButton);
            continuousButton.onClick.AddListener(ContinuousButton);

            SetGameResult();

            //플레이어 UI 생성
            foreach (Player other in PhotonNetwork.PlayerList)
            {
                GenerateResultScenePlayerUI(other);
            }
        }

        private void Update()
        {

            if (PhotonNetwork.IsMasterClient)
            {
                continuousButton.interactable = true;
            }
            else
            {
                continuousButton.interactable = false;
            }
        }

        //게임 결과값 설정
        private void SetGameResult()
        {
            //플레이 타임 표시
            float time = GameManager.Instance.PlayTime;

            int min = Mathf.FloorToInt(time / 60);
            int sec = Mathf.FloorToInt(time % 60);

            timeText.text = $"{min:D2}:{sec:D2}";

            //게임 결과 텍스트 및 색 표시
            switch (GameManager.Instance.GameResult)
            {
                case GameResultType.Clear:
                    {
                        underField.color = clearColor;
                        resultText.color = clearColor;
                        resultText.text = "CLEAR!";
                        break;
                    }
                case GameResultType.Fail:
                    {
                        underField.color = failColor;
                        resultText.color = failColor;
                        resultText.text = "FAIL!";
                        break;
                    }
            }
        }

        //플레이어 UI 생성
        private void GenerateResultScenePlayerUI(Player player)
        {
            GameObject uiObj = Instantiate(resultScenePlayerUIPrefab, uiParent);
            uiObj.GetComponent<ResultScenePlayerUIController>().Initialize(player);
        }

        //나가기 버튼
        private void LeaveButton()
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);
            FadeManager.Instance.FadeOut(() =>
            {
                PhotonNetwork.LeaveRoom();
            });
        }

        //계속하기 버튼
        private void ContinuousButton()
        {
            SoundManager.instance.SFXPlay("Click", buttonClickSFX);

            photonView.RPC("OnSelectContinuousButton", RpcTarget.All);
        }

        [PunRPC]
        public void OnSelectContinuousButton()
        {
            FadeManager.Instance.FadeOut(() =>
            {
                SceneManager.LoadScene(LobbyScene);
            });
        }
    }
}
