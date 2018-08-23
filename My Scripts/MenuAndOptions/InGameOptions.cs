using UnityEngine.SceneManagement;
using UnityEngine;

public class InGameOptions : MonoBehaviour 
{
    const string SCENE_NAME = "TestLiamControls";

    public void Quit()
    {
        Application.Quit();
    }
    public void ReloadScene()
    {
        ResetKeys();
        ResetInput();
        SceneManager.LoadScene(SCENE_NAME);
    }

    public void Nudge()
    {
        Rigidbody rb = ObjectFinder.PlayerRigidBody;
        rb.AddRelativeForce(Vector3.forward * 200);
    }

    private void ResetInput()
    {
        Global.COMBAT_INPUT_ENABLED = true;
        Global.USER_INPUT_ENABLED = true;
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
