using UnityEngine;
using System.Collections;

public class PathSplines : MonoBehaviour {

	public Transform[] trans;
    public Transform LookAt;
    public bool skip;

	LTSpline cr;
    float iter;

    void OnEnable(){
        Vector3[] pos = new Vector3[trans.Length];

        for (var i = 0; i < trans.Length; i++)
        {
            pos[i] = trans[i].position;
        }
        cr = new LTSpline(pos);
    }

	void Start ()
    {
        if (skip)
            OnEnd();
        else
            LeanTween.moveSpline(transform.gameObject, cr.pts, 17.5f).setOrientToPath(false).setDirection(1f).onComplete += OnEnd;
        
	}
	
    void OnEnd()
    {
        transform.GetComponent<FightingCamera>().active = true;
        EventManager.TriggerEvent("OnCinematicFinished");
    }

	void Update () {
		iter += Time.deltaTime;
		if(iter>1.0f)
			iter = 0.0f;

        transform.LookAt(LookAt);
    }
}
