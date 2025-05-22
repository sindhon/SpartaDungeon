using UnityEngine;

public class CharacterManager : MonoBehaviour   // 플레이어 정보를 전역에서 접근할 수 있도록 해주는 싱글톤 클래스
{
    private static CharacterManager instance;   
    public static CharacterManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }

            return instance;
        }
    }

    private Player player;
    public Player Player
    {
        get { return player; }
        set { player = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
