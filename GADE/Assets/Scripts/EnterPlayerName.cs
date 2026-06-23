using UnityEngine;
using UnityEngine.UI;
public class EnterPlayerName : MonoBehaviour
{
    [SerializeField] InputField playerNameInput;
    public static EnterPlayerName Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerNameInput != null)
        {
            RetrievePlayerName();
        }
    }

   public void RetrievePlayerName()
    {
      //  GameManager.Instance.GetPlayerData(playerNameInput.text);
     PlayerPrefs.SetString("Player Name", playerNameInput.text);
    }
}
