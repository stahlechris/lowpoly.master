using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour 
{
    [SerializeField] GameObject canvasPrefab;
    [SerializeField] Sprite mySpriteIcon;
    [SerializeField] HUD hud;
    Camera cameraToLookAt;

    /*
     * If Quaternion.identity: the object keeps the same rotation as the prefab. 
     * If transform.rotation: the object's rotation is combined with the  prefab's
     */
	void Start () 
    {
        cameraToLookAt = Camera.main;
        Instantiate(canvasPrefab, transform.position, Quaternion.identity, transform);
        hud = ObjectFinder.HUD;
	}
    void LateUpdate () 
    {
        //todo if camera hasnt moved, don't recalculate lookAt every frame
        transform.LookAt(cameraToLookAt.transform);
    }

    public void SetupMyUI()
    {
        Transform iconBox = hud.transform.Find("EnemyUIBox/Avatar_Health_Mana(ENEMY)/enemyImage");
        iconBox.GetComponent<Image>().sprite = mySpriteIcon;
        hud.transform.Find("EnemyUIBox").gameObject.SetActive(true);
    }
    public void RemoveMyselfFromTheHUD()
    {
        hud.transform.Find("EnemyUIBox").gameObject.SetActive(false);
    }
}
