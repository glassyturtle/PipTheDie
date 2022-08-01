
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int pipsCollected;
    private Vector3 playerSpawnPos;
    public GameObject player, cheseUI, cheseGameObject, winScreen, pauseMenu;
    [SerializeField] Text pipAmtText;
    [SerializeField] Rigidbody2D playerBody;
    [SerializeField] Tilemap tilemap;

    [SerializeField] SpriteRenderer mouseSR;
    [SerializeField] Sprite mouseLeaveSprite;
    public bool hasCheese = false;
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Unpause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    public void CollectCheese()
    {
        Destroy(cheseGameObject);
        cheseUI.gameObject.SetActive(true);
        hasCheese = true;
    }
    public void OpenGateWay()
    {
        mouseSR.sprite = mouseLeaveSprite;
        cheseUI.SetActive(false);
        for (int x = -9; x < -4; x++)
        {
            for (int y = 49; y < 52; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), null);
            }
        }
    }
    private void Start()
    {
        playerSpawnPos = player.transform.position;
    }
    public void ResetLevel()
    {
        playerBody.transform.position = playerSpawnPos;
        playerBody.velocity = Vector3.zero;
    }
    public void WinGame()
    {
        Time.timeScale = 0;
        winScreen.SetActive(true);
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    public void CollectPip()
    {
        pipsCollected++;
        pipAmtText.text = "Pips Collected: " + pipsCollected.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
    }
}
