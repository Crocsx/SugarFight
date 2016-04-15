using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Linq;
using Random = UnityEngine.Random;

[System.Serializable]
public class MyAudioClip
{
	public MyAudioClip(AudioClip clip,float volume)
	{
		this.clip = clip;
		this.volume = volume;
	}

	public AudioClip clip;
	public float volume;
}

/// <summary>
/// Sfx manager.
/// Gestion de SFX sur 4 AudioSource max
/// </summary>
public class SfxManager : MonoBehaviour {

	public static SfxManager manager;

	public TextAsset sfxXmlSetup;
	public string resourcesFolderName;

	public int nAudioSources = 2;

	public bool shouldShowGui;

	List<AudioSource> audioSources = new List<AudioSource>();
	Dictionary<string,MyAudioClip> dicoAudioClips = new Dictionary<string, MyAudioClip>();

	void Awake()
	{
		if(manager) Destroy(manager.gameObject);
		manager = this;
	}

	AudioSource AddAudioSource()
	{
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSources.Add(audioSource);
		
		audioSource.loop = false;
		audioSource.playOnAwake = false;

		return audioSource;
	}

	// Use this for initialization
	void Start () {

		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(sfxXmlSetup.text);

		foreach(XmlNode node in xmlDoc.GetElementsByTagName("SFX"))
		{
			if(node.NodeType!= XmlNodeType.Comment)

			dicoAudioClips.Add(
				node.Attributes["name"].Value,
			    new MyAudioClip(
				(AudioClip)Resources.Load(resourcesFolderName+"/"+node.Attributes["name"].Value,typeof(AudioClip)),
				float.Parse(node.Attributes["volume"].Value)));
		}

		for (int i = 0; i < nAudioSources; i++) 
			AddAudioSource();
	}

	public void PlaySfx(string sfxName)
	{
		if(FlagsManager.manager && !FlagsManager.manager.GetFlag("SETTINGS_SFX",true))
			return;

		MyAudioClip audioClip;
		if(!dicoAudioClips.TryGetValue(sfxName,out audioClip))
		{
			Debug.LogError("SFX, no audio clip with name: "+sfxName);
			return;
		}

		AudioSource audioSource = audioSources.Find(item=>!item.isPlaying);
		if(audioSource) 
			audioSource.PlayOneShot(audioClip.clip,audioClip.volume);

	}

	void OnGUI()
	{
		if(!shouldShowGui) return;


		GUILayout.BeginArea(new Rect(Screen.width*.5f,10,200,Screen.height));
		GUILayout.Label("SFX MANAGER");
		GUILayout.Space(20);
		foreach (var item in dicoAudioClips) {
			if(GUILayout.Button("PLAY "+item.Key))
				PlaySfx(item.Key);
		}
		GUILayout.EndArea();
		
	}

}
