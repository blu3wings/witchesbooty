using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Booty.Managers.Audio.AudioManager))]
public class AudioManagerEditor : Editor
{
    private Booty.Managers.Audio.AudioManager _audioManager;

    private bool _sfxFoldOut = true;
    private bool _bgmFoldOut = true;
    private bool _ambientFoldOut = true;

    private void OnEnable()
    {
        if (_audioManager == null)
            _audioManager = (Booty.Managers.Audio.AudioManager)target;
    }

    public override void OnInspectorGUI()
    {
        if (_audioManager == null) return;

        _sfxFoldOut = EditorGUILayout.Foldout(_sfxFoldOut, "SFX Audio Group");
        if(_sfxFoldOut)
        {
            displaySFXAudioGroup();
        }

        _bgmFoldOut = EditorGUILayout.Foldout(_bgmFoldOut, "BGM Audio Group");
        if(_bgmFoldOut)
        {
            displayBGMAudioGroup();
        }

        _ambientFoldOut = EditorGUILayout.Foldout(_ambientFoldOut, "Ambient Audio Group");
        if(_ambientFoldOut)
        {

        }
    }

    private void displayBGMAudioGroup()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+ Add BGM Audio Data"))
        {
            _audioManager.addBgmAudioData("");
        }
        EditorGUILayout.EndHorizontal();

        if (_audioManager.bgmSources != null)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(string.Format("Total BGM Audio Data: {0}",
                _audioManager.bgmSources.Count), EditorStyles.boldLabel);

            for (int i = 0; i < _audioManager.bgmSources.Count; i++)
            {
                Booty.Managers.Audio.AudioData data = _audioManager.bgmSources[i];

                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Audio Name", GUILayout.Width(100));
                data.audioName = GUILayout.TextField(data.audioName, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Audio Clip", GUILayout.Width(100));
                data.audioSource = (AudioClip)EditorGUILayout.ObjectField(data.audioSource,
                    typeof(AudioClip), GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Delete Audio Data"))
                {
                    _audioManager.deleteBgmAudioData(i, saveData);
                }
                GUILayout.Box("", GUILayout.Width(300), GUILayout.Height(2));
            }
            EditorGUILayout.EndVertical();
        } 
    }

    private void displaySFXAudioGroup()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+ Add SFX Audio Data"))
        {
            _audioManager.addSfxAudioData("");
        }
        EditorGUILayout.EndHorizontal();

        if (_audioManager.sfxSources != null)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(string.Format("Total SFX Audio Data: {0}",
                _audioManager.sfxSources.Count), EditorStyles.boldLabel);

            for (int i = 0; i < _audioManager.sfxSources.Count; i++)
            {
                Booty.Managers.Audio.AudioData data = _audioManager.sfxSources[i];

                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Audio Name", GUILayout.Width(100));
                data.audioName = GUILayout.TextField(data.audioName, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Audio Clip", GUILayout.Width(100));
                data.audioSource = (AudioClip)EditorGUILayout.ObjectField(data.audioSource,
                    typeof(AudioClip), GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Delete Audio Data"))
                {
                    _audioManager.deleteSfxAudioData(i, saveData);
                }
                GUILayout.Box("", GUILayout.Width(300), GUILayout.Height(2));
            }
            EditorGUILayout.EndVertical();
        } 
    }

    private void saveData()
    {
        EditorApplication.SaveScene();
    }
}