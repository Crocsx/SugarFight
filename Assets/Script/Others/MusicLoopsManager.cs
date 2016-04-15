using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MusicType{menuMusic,thinkMusic,solveMusic};

/// <summary>
/// Music loops manager.
/// Gestion de boucles musicales avec Fade-In/Fade-Out entre 2 boucles
/// </summary>
public class MusicLoopsManager : MonoBehaviour {

	public static MusicLoopsManager manager;

	public List<AudioClip> clips = new List<AudioClip>();

	[HideInInspector]
	public int currClipIndex=0;

	public AudioSource[]  audioSources;
	public float fadeDuration;

	public bool shouldShowGui;

	private int indexFadeIn;
	private float[] maxVolumes = new float[2] ;


	void Awake()
	{
		if(manager)
			Destroy(manager.gameObject);

		manager = this;

		audioSources = GetComponents<AudioSource>();

		for (int i = 0; i < audioSources.Length; i++) 
		{
			maxVolumes[i] = audioSources[i].volume;
			audioSources[i].clip = clips[i];
		}

	}

	void Start ()
	{
		indexFadeIn = 0;
	}

	IEnumerator FadeOutAndStopAll(float delay)
	{
		yield return new WaitForSeconds(delay+.1f); // Unity bug possiblement si la durée d'attente est nulle ... on ajoute 0,1 pour que cette durée ne soit jamais véritablement nulle
		float elapsedTime = 0;

		while(elapsedTime<fadeDuration)
		{
			float k = elapsedTime/fadeDuration;
			audioSources[indexFadeIn].volume = Mathf.Lerp(0,maxVolumes[indexFadeIn],1-k);			//Fade out 1st audiosource
			audioSources[1-indexFadeIn].volume = Mathf.Lerp(0,maxVolumes[1-indexFadeIn],1-k);		//Fade out 2nd audiosource
			elapsedTime+=Time.deltaTime;
			yield return null;
		}
		audioSources[indexFadeIn].volume =0;
		audioSources[indexFadeIn].Stop();
		audioSources[1-indexFadeIn].volume = 0;
		audioSources[1-indexFadeIn].Stop();
	}


	IEnumerator FadeCoroutine()
	{
		float elapsedTime = 0;
		while(elapsedTime<fadeDuration)
		{
			float k = elapsedTime/fadeDuration;
			audioSources[indexFadeIn].volume = Mathf.Lerp(0,maxVolumes[indexFadeIn],k);			//Fade in 1st audiosource
			audioSources[1-indexFadeIn].volume = Mathf.Lerp(0,maxVolumes[1-indexFadeIn],1-k);	//Fade out 2nd audiosource
			elapsedTime+=Time.deltaTime;
			yield return null;
		}
		audioSources[indexFadeIn].volume = Mathf.Lerp(0,maxVolumes[indexFadeIn],1);
		audioSources[1-indexFadeIn].volume = Mathf.Lerp(0,maxVolumes[1-indexFadeIn],0);
		audioSources[1-indexFadeIn].Stop();
	}
	
	void PlayMusic(int index)
	{
		currClipIndex = index%clips.Count;
		audioSources[1-indexFadeIn].clip = clips[currClipIndex];
		indexFadeIn = 1-indexFadeIn;
		StartCoroutine(FadeCoroutine());
		audioSources[indexFadeIn].Play();
	}

	public void PlayCurrentMusic()
	{
		if(!FlagsManager.manager || FlagsManager.manager.GetFlag("SETTINGS_MUSIC",true))
			PlayMusic(currClipIndex);
	}

	public void PlayMusic(MusicType musicType)
	{
		PlayMusic((int)musicType);
	}

	public void PlayNextMusic()
	{
		if(!FlagsManager.manager || FlagsManager.manager.GetFlag("SETTINGS_MUSIC",true))
			PlayMusic(currClipIndex+1);
	}

	public void StopAll(float delay)
	{
		Debug.Log("InGameMusicManager StopAll("+delay+")");
		StartCoroutine(FadeOutAndStopAll(delay));
	}

	public void StopAllRightAway()
	{
		StopAllCoroutines();
		audioSources[indexFadeIn].volume =0;
		audioSources[1-indexFadeIn].volume = 0;
		audioSources[1-indexFadeIn].Stop();
		audioSources[indexFadeIn].Stop();
	}

	void OnGUI()
	{
		if(!shouldShowGui) return;

		GUILayout.BeginArea(new Rect(10,10,200,Screen.height));

		GUILayout.Label("MUSIC LOOPS MANAGER");
		GUILayout.Space(20);
		for (int i = 0; i < clips.Count; i++) {
			if(GUILayout.Button("PLAY "+clips[i].name))
				PlayMusic(i);
		}
		GUILayout.Space(20);
		if(GUILayout.Button("PLAY CURRENT MUSIC"))
			PlayCurrentMusic();

		if(GUILayout.Button("PLAY NEXT MUSIC"))
			PlayNextMusic();

		if(GUILayout.Button("STOP ALL - FADEOUT"))
			StopAll(0);

		if(GUILayout.Button("STOP ALL - NO FADEOUT"))
			StopAllRightAway();

		GUILayout.EndArea();

	}

}
