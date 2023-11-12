using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static private FollowCam S;
    static public GameObject POI;
    
    public enum eView { none, slingshot, castle, both };

    [Header("Inscribed")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;
    public GameObject viewBothGO;

    [Header("Set Dynamically")]
    public float camZ;
    public eView nextView = eView.slingshot;

    private void Awake()
    {   
        S = this;
        camZ = this.transform.position.z;
    }

    private void FixedUpdate() 
    {
        Vector3 destination;

        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            destination = POI.transform.position;
            if (POI.tag == "Projectile") 
            {
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    POI = null;
                    return;
                }
            }
        }

        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.x = Mathf.Max( minXY.x, destination.x );
        destination.y = Mathf.Max( minXY.y, destination.y );
        destination.z = camZ;
        transform.position = destination;
        Camera.main.orthographicSize = destination.y + 10;
    }

    public void SwitchView(eView newView)
    {
        if (newView == eView.none)
        {
            newView = nextView;
        }

        switch(newView)
        {
            case eView.slingshot:
                POI = null;
                nextView = eView.castle;
                break;

            case eView.castle:
                POI = MissionDemolition.GET_CASTLE();
                nextView = eView.both;
                break;

            case eView.both:
                POI = viewBothGO;
                nextView = eView.slingshot;
                break;
        }
    }

    public void SwitchView()
    {
        SwitchView(eView.none);
    }

    static public void SWITCH_VIEW(eView newView)
    {
        S.SwitchView(newView);
    }
}
