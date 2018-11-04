using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//알의 정보, 처리를 담당
public class EggInformation : MonoBehaviour
{
    private Touch tempTouchs;
    private Vector3 touchedPos;
    private bool touchOn;

    public Sprite[] sprites;
    private SpriteRenderer _now;

    public GameObject hit_prefab;

    [Tooltip("최대 클릭 횟수")]
    public int MAX_CLICK = 100;
    [Tooltip("현재 클릭 횟수")]
    public int click_num = 0;
    [Tooltip("최대 클릭 횟수 <= 현재 클릭 횟수")]
    public bool egg_born;

    void Awake()
    {
        _now = GetComponent<SpriteRenderer>();
        sprites[ 0 ] = MainTitle_UI.Instance.egg_animal[ int.Parse(MainTitle_UI.Instance.egg_state[ MainTitle_UI.Instance.selected_title ]) ];
        _now.sprite = sprites[ 0 ];
    }

    void Update()
    {
        if( egg_born )
        {
            _now.sprite = sprites[ 1 ];
        }
        else
        {
            if( click_num >= MAX_CLICK )
            {
                egg_born = true;
                return;
            }

            _now.sprite = sprites[ 0 ];

#if UNITY_EDITOR
            if( Input.GetMouseButtonDown(0) )
            {
                touchedPos = ( Input.mousePosition );
                Ray ray = Camera.main.ScreenPointToRay(touchedPos);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray , Mathf.Infinity);

                if( hit.collider != null && hit.collider.transform == transform )
                {
                    ++click_num;
                    Instantiate(hit_prefab , hit.point , Quaternion.identity , null);
                    Debug.Log(touchedPos);
                }
            }
#endif

#if UNITY_ANDROID
            //touchOn = false;

            if( Input.touchCount > 0 )
            {    //터치가 1개 이상이면.
                for( int i = 0 ; i < Input.touchCount ; i++ )
                {
                    tempTouchs = Input.GetTouch(i);
                    if( tempTouchs.phase == TouchPhase.Began )
                    {    //해당 터치가 시작됐다면.
                        touchedPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);//get world position.
                        touchOn = true;
                        Debug.Log(touchedPos);
                        Ray ray = Camera.main.ScreenPointToRay(touchedPos);
                        RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero);

                        if( hit )
                        {
                            ++click_num;
                            Instantiate(hit_prefab , hit.point , Quaternion.identity , null);
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
