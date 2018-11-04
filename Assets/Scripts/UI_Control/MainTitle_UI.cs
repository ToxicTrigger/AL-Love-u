using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//메인 화면에서의 UI 간 이벤트를 처리 하기 위함
public class MainTitle_UI : MonoBehaviour
{
    public static MainTitle_UI Instance = null;
    public Animator fade_ani;
    //메인화면에서 선택된 관리 칸 번호
    public int selected_title;
    public Sprite born_egg;

    //4개의 관리 칸에 존재하는 알, 수인의 이름을 저장
    public string[] egg_state;

    //버튼들의 이미지를 설정
    public Button array_0, array_1, array_2, array_3;

    public Sprite[] egg_animal;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }

        var tmp = FindObjectsOfType<Animator>();
        foreach (var i in tmp)
        {
            if (i.name.Equals("FadeAnimation"))
            {
                fade_ani = i;
                break;
            }
        }
        DontDestroyOnLoad(gameObject);

        //각 칸의 상태를 초기에 Load
        egg_state[0] = PlayerPrefs.GetString("Array_0");
        egg_state[1] = PlayerPrefs.GetString("Array_1");
        egg_state[2] = PlayerPrefs.GetString("Array_2");
        egg_state[3] = PlayerPrefs.GetString("Array_3");
        int[] result = new int[4];

        try
        {
            result[0] = int.Parse(PlayerPrefs.GetString("Array_0"));
        }
        catch
        {
            result[0] = 0;
        }
        try
        {
            result[1] = int.Parse(PlayerPrefs.GetString("Array_1"));
        }
        catch
        {
            result[1] = 0;
        }
        try
        {
            result[2] = int.Parse(PlayerPrefs.GetString("Array_2"));
        }
        catch
        {
            result[2] = 0;
        }
        try
        {
            result[3] = int.Parse(PlayerPrefs.GetString("Array_3"));
        }
        catch
        {
            result[3] = 0;
        }

        array_0.image.sprite = egg_animal[result[0]];
        array_1.image.sprite = egg_animal[result[1]];
        array_2.image.sprite = egg_animal[result[2]];
        array_3.image.sprite = egg_animal[result[3]];
    }

    public IEnumerator event_change_scene(string SceneName)
    {
        fade_ani.SetBool("fade", true);
        yield return new WaitForSeconds(1.0f);
        yield return SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
        fade_ani.SetBool("fade", false);
    }

    //씬 변경을 하면서 추가적으로 선택된 관리 칸의 번호를 저장
    public void change_scene(int SelectedTitle)
    {
        selected_title = SelectedTitle;
        if(egg_state[selected_title].Equals(""))
        {
            StartCoroutine(event_change_scene("RandomEgg"));
        }
        else
        {
            StartCoroutine(event_change_scene("GamePlay"));
        }
       
    }

    //씬 변경을 호출
    public void change_scene(string SceneName)
    {
        StartCoroutine(event_change_scene(SceneName));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
