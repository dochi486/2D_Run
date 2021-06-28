using UnityEngine;

namespace Run
{
    public class FinishFlag : MonoBehaviour
    {
        // Start is called before the first frame update
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.GetComponent<Player>() == null)
                return;

            RunGameManager.instance.StageClear();
        }
    }
}
