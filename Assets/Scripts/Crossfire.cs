using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossfire : MonoBehaviour
{
    public LayerMask ScreenLayerMask;

    Camera m_Camera;
    SpriteRenderer m_SpriteRender;
    Color m_OnEnemyColor;
    Color m_OriginColor;

    private void Awake()
    {
        m_Camera = Camera.main;
        m_SpriteRender = GetComponent<SpriteRenderer>();
        m_OnEnemyColor = Color.red;
        m_OriginColor = m_SpriteRender.color;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate foever
        transform.Rotate(Vector3.forward, 60 * Time.deltaTime);

        //Screen Ray
        Ray mouseRay = m_Camera.ScreenPointToRay(Input.mousePosition);
        
        //Set Position
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float dist2Ground;

        if (ground.Raycast(mouseRay, out dist2Ground))
        {
            Vector3 hitPoint = mouseRay.GetPoint(dist2Ground);
            //Debug.DrawLine(mouseRay.origin, hitPoint, Color.red);
            Vector3 correctPostion = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);
            transform.position = correctPostion;
        }

        //Set Color
        RaycastHit hitinfo;
        if(Physics.Raycast(mouseRay, out hitinfo, dist2Ground, ScreenLayerMask.value))
        {
            if(hitinfo.collider.tag == "Enemy")
            {
                m_SpriteRender.color = m_OnEnemyColor;
            }
        }
        else
            m_SpriteRender.color = m_OriginColor;
    }
}
