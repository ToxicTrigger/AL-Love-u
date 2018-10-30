using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//랜덤 선택 씬에서 사용됨
public class RandomEgg : MonoBehaviour
{
    private Touch tempTouchs;
    private Vector3 touchedPos;
    private bool touchOn;

    public GameObject hit_prefab;

    public Sprite[] sprites;
    private SpriteRenderer _now;

    void Awake()
    {
        _now = GetComponent<SpriteRenderer>();
    }

    IEnumerator event_shake()
    {
        for(int i = 0; i < 100; ++i)
        {
            //yield return new WaitForSeconds(Time.deltaTime);
            float x = Random.Range(-1.0f, 1.0f);
            float y = Random.Range(-1.0f, 1.0f);
            var pos = Camera.main.transform.position;
            pos.x += x;
            pos.y += y;
            Camera.main.transform.position = pos;
            yield return new WaitForSeconds(Time.deltaTime);
            Camera.main.transform.position = new Vector3(0, 1, -10);
        }
    }
    IEnumerator event_particle(Vector2 hit)
    {
        for(int i = 0; i< 100; ++i)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            float x = Random.Range(-1.0f, 1.0f);
            float y = Random.Range(-1.0f, 1.0f);
            var pos = hit;
            pos.x += x;
            pos.y += y;
            Instantiate(hit_prefab, pos , Quaternion.identity, null);
        }
        //알 결정
        int index = Random.Range(0, 4);
        _now.sprite = sprites[index];
        MainTitle_UI.Instance.born_egg = _now.sprite;
        MainTitle_UI.Instance.egg_state[MainTitle_UI.Instance.selected_title] = index.ToString();

        PlayerPrefs.SetString("Array_" + MainTitle_UI.Instance.selected_title, index.ToString());
        PlayerPrefs.Save();

        yield return new WaitForSeconds(2.0f);
        MainTitle_UI.Instance.change_scene("GamePlay");

    }

    void Update()
    {
        if(!touchOn )
        {

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                touchedPos = (Input.mousePosition);
                Ray ray = Camera.main.ScreenPointToRay(touchedPos);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                if (hit.collider != null && hit.collider.transform == transform)
                {
                    StartCoroutine(event_particle(hit.point));
                    StartCoroutine(event_shake());
                    touchOn = true;
                    Debug.Log(touchedPos);
                }
            }
#endif

#if UNITY_ANDROID
        touchOn = false;

        if (Input.touchCount > 0)
        {    //터치가 1개 이상이면.
            for (int i = 0; i < Input.touchCount; i++)
            {
                tempTouchs = Input.GetTouch(i);
                if (tempTouchs.phase == TouchPhase.Began)
                {    //해당 터치가 시작됐다면.
                    touchedPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);//get world position.
                    touchOn = true;
                    Debug.Log(touchedPos);
                    Ray ray = Camera.main.ScreenPointToRay(touchedPos);
                    RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                    if (hit.collider != null && hit.collider.transform == transform)
                    {
                        StartCoroutine(event_particle(hit.point));
                        StartCoroutine(event_shake());
                        touchOn = true;
                        Debug.Log(touchedPos);
                    }
                    break;   //한 프레임(update)에는 하나만.
                }
            }
        }
#endif
        }
    }
}
