using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class InGameOptions : MonoBehaviour 
{
    Scene currentScene;

    const string SCENE_NAME = "TestLiamControls";
    [SerializeField] Transform inventoryPanel;
    [SerializeField] Image[] inventoryPanelImages;//serialize for visualition
    [SerializeField] Text[] inventoryPanelText;
    [SerializeField] Color[] cachedInventoryPanelImageColors;
    bool InventoryActive { get; set; }

    private void Start()
    {
        InventoryActive = true;
        inventoryPanelImages = inventoryPanel.GetComponentsInChildren<Image>();
        cachedInventoryPanelImageColors = new Color[inventoryPanelImages.Length];
        //Cache original color values to use when enabling inventory
        for (int i = 0; i < inventoryPanelImages.Length; i++)
        {
            cachedInventoryPanelImageColors[i] = inventoryPanelImages[i].color;
        }
        inventoryPanelText = inventoryPanel.GetComponentsInChildren<Text>();
    }
    public void Quit()
    {
        Reset();
        Application.Quit();
    }
    public void ReloadScene()
    {
        currentScene = SceneManager.GetActiveScene();

        Reset();
        SceneManager.LoadScene(currentScene.name);
    }

    public void Nudge()
    {
        Rigidbody rb = ObjectFinder.PlayerRigidBody;
        rb.AddRelativeForce(Vector3.forward * 200);
    }
    public void ToggleInventoryPanel()
    {
        //Make a transparent color to assign to all the images.
        Color transparentColor = Color.clear;
        //Text components just use white color.
        Color whiteColor = Color.white;
        //If player has an active (showing) inventory panel...
        if (InventoryActive) //panel: slot => border => image => itemQuantity
        {
            //Cycle through every image element in the panel.
            for (int i = 0; i < inventoryPanelImages.Length; i++)
            {
                //Turn down alpha to 0 to make it seem gone
                inventoryPanelImages[i].color = transparentColor;
                //TODO Disable ability to click the item so you dont accidentally use it while invisible
                //Avoid a second for loop and just implement a null check to change the texts.
                if (i < inventoryPanelText.Length)
                {
                    inventoryPanelText[i].color = transparentColor;
                }
            }
            InventoryActive = false;
        }
        else
        {
            //Cycle through every image element in the panel.
            for (int i = 0; i < inventoryPanelImages.Length; i++)
            {
                //Turn color back to what it was
                inventoryPanelImages[i].color = cachedInventoryPanelImageColors[i];
                //TODO Enable ability to click the item
                // Turn alpha back on text components too
                if (i < inventoryPanelText.Length)
                {
                    inventoryPanelText[i].color = whiteColor;
                }
            }
            InventoryActive = true;
        }
    }
    void Reset()
    {
        ResetKeys();
        ResetInput();
        ResetBoolFlags();
    }
    void ResetInput()
    {
        Global.COMBAT_INPUT_ENABLED = true;
        Global.USER_INPUT_ENABLED = true;
        Global.HAVING_A_CONVERSATION = false;
    }
    void ResetBoolFlags()
    {
        ObjectFinder.PlayerController.alreadySpeaking = false;
    }

    void ResetKeys()
    {
        KeyManager.hasIpKey = false;
        KeyManager.hasEdKey = false;
        KeyManager.hasOceanKey = false;
        KeyManager.hasTempleKey = false;
        KeyManager.hasCarlKey = false;
        KeyManager.hasSpaceKey = false;
        KeyManager.hasDeadForestKey = false;
        KeyManager.hasFlowerGardenKey = false;
        KeyManager.hasSwordCaveKey = false;
        KeyManager.hasGuardianKey = false;
    }
}
