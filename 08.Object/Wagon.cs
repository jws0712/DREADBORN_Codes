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
            
            //������ �÷��̾��� ���� ���� �濡 ������ �÷��̾� ���� ���ٸ� ���� Ŭ���� ����
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

