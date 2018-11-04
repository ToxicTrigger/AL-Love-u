using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//랜덤 선택 씬에서 사용됨
public class RandomEgg : MonoBehaviour
{
    private Touch tempTouchs;
    private Vector3 touchedPos;
    public bool touchOn;

    public GameObject hit_prefab;

    public Sprite[] sprites;
    private Image _now;

    void Start()
    {
        _now = GetComponent<Image>();
    }

    IEnumerator event_shake()
    {
        for( int i = 0 ; i < 100 ; ++i )
        {
            //yield return new WaitForSeconds(Time.deltaTime);
            float x = Random.Range(-1.0f , 1.0f);
            float y = Random.Range(-1.0f , 1.0f);
            var pos = Camera.main.transform.position;
            pos.x += x;
            pos.y += y;
            Camera.main.transform.position = pos;
            yield return new WaitForSeconds(Time.deltaTime);
            Camera.main.transform.position = new Vector3(0 , 1 , -10);
        }
    }
    IEnumerator event_particle(Vector2 hit)
    {
        for( int i = 0 ; i < 100 ; ++i )
        {
            yield return new WaitForSeconds(Time.deltaTime);
            float x = Random.Range(-1.0f , 1.0f);
            float y = Random.Range(-1.0f , 1.0f);
            var pos = hit;
            pos.x += x;
            pos.y += y;
            Instantiate(hit_prefab , pos , Quaternion.identity , null);
        }
        //알 결정
        int index = Random.Range(0 , 4);
        _now.sprite = sprites[ index ];
        MainTitle_UI.Instance.born_egg = _now.sprite;
        MainTitle_UI.Instance.egg_state[ MainTitle_UI.Instance.selected_title ] = index.ToString();

        PlayerPrefs.SetString("Array_" + MainTitle_UI.Instance.selected_title , index.ToString());
        PlayerPrefs.Save();

        yield return new WaitForSeconds(2.0f);
        MainTitle_UI.Instance.change_scene("GamePlay");

    }

    public void click()
    {
        if( !touchOn )
        {
            Debug.Log(Application.platform.ToString());
            if( Application.platform == RuntimePlatform.WindowsEditor )
            {
                StartCoroutine(event_particle(Input.mousePosition));
                StartCoroutine(event_shake());
                touchOn = true;
            }
            else
            {
                StartCoroutine(event_particle(Input.mousePosition));
                StartCoroutine(event_shake());
                touchOn = true;
            }
        }
    }

}
