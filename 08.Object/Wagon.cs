namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    //Photon
    using Photon.Pun;

    public class Wagon : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private Transform checkTransform;
        [SerializeField] private float checkRadius;
        [SerializeField] private LayerMask checkLayer;

        private bool once;

        private void Update()
        {
            
            //감지된 플레이어의 수가 현재 방에 참여한 플레이어 수와 같다면 게임 클리어 실행
            Collider[] players = Physics.OverlapSphere(checkTransform.position, checkRadius, checkLayer);

            if(PhotonNetwork.IsConnected)
            {
                if (players.Length == PhotonNetwork.CurrentRoom.PlayerCount && !once)
                {
                    GameManager.Instance.LoadGameResult(GameResultType.Clear);
                    once = true;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(checkTransform.position, checkRadius);
        }
    }
}

