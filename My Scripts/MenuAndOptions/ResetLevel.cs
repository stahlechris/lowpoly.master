using UnityEngine.SceneManagement;
using UnityEngine;

public class ResetLevel : MonoBehaviour 
{
    const string PLAYER_NAME = "Liam";
    const string SCENE_NAME = "TestLiamControls";
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.name == PLAYER_NAME)
        {
            ResetKeys();
            SceneManager.LoadScene(SCENE_NAME);
        }
    }
    void ResetKeys()
    {
        KeyManager.hasIpKey = false;
        KeyManager.hasEdKey = false;
        KeyManager.hasOceanKey = false;
        KeyManager.hasTempleKey = false;
        KeyManager.hasCarlKey = false;
        KeyManager.hasBehindTempleKey = false;
        KeyManager.hasDeadForestKey = false;
        KeyManager.hasFlowerGardenKey = false;
        KeyManager.hasSwordCaveKey = false;
        KeyManager.hasGuardianKey = false;
    }
}
