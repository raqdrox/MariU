using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using System;

namespace Athena.Mario.RenderScripts
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ColorPalette))]
    public class ColorPaletteEditor : Editor
    {
        private string _colorHexField;
        private int _colorId;
        private char[] _colorHexFileDelim = new char[] {'\n'};
        private TextAsset _colorHexFile;



        public override void OnInspectorGUI()
        {
            var palette = target as ColorPalette;
            base.OnInspectorGUI();
            EditorGUILayout.Space(30f,true);
            _colorHexField=EditorGUILayout.TextField("Hex Color",_colorHexField);
            EditorGUILayout.Space();
            _colorId=EditorGUILayout.IntField("Palette Position",_colorId);
            EditorGUILayout.Space();

            if (GUILayout.Button("Add Color"))
            {
                AddColorToPalette(palette, _colorHexField, _colorId);
                _colorHexField = "";
                _colorId = 0;
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("Remove Color"))
            {   
                palette.colors.RemoveAt(_colorId);
                _colorId=0;
            }
            EditorGUILayout.Space();

            if(GUILayout.Button("Clear Palette"))
            {
                palette.colors.Clear();
            }
            EditorGUILayout.Space(30f,true);
            _colorHexFile=(TextAsset)EditorGUILayout.ObjectField("File Asset",_colorHexFile,typeof(TextAsset),true);
            _colorHexFileDelim = EditorGUILayout.TextField("Delimiter", new string(_colorHexFileDelim)).ToCharArray();
            EditorGUILayout.Space();
            if (GUILayout.Button("Load Colors from File"))
            {
                if(_colorHexFile==null) {
                    Debug.LogError("No file selected");
                    return;
                }
                var lines = _colorHexFile.text.Split(_colorHexFileDelim);
                palette.colors.Clear();
                foreach (var line in lines)
                {

                    AddColorToPalette(palette, line, palette.colors.Count);
                }
            }
            
        }

        private void AddColorToPalette(ColorPalette palette, string colorHex, int idx)
        {
            colorHex = colorHex.Trim();
            if (colorHex.Length == 0)
            {
                return;
            }
            if(!colorHex.StartsWith("#"))
            {
                colorHex = "#" + colorHex;
            }
            
            var success=ColorUtility.TryParseHtmlString(colorHex, out var color);
            if (!success)
            {
                Debug.LogError("Invalid color hex");
                return;
            }
            palette.colors.Insert(idx, color);
        }

    }

    [CanEditMultipleObjects]
    [CustomEditor(typeof(Palette))]
    public class PaletteEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var palette = target as Palette;
            
        
            for (int i = 0; i < palette.palettes.Count; i++)
            {
                
                var cols= palette.GetColors(i);
                if(cols==null) continue;
                GUILayout.Label("Palette ID "+i.ToString()+":");
                GUILayout.BeginHorizontal();
                foreach(var col in cols){
                    EditorGUILayout.ColorField(GUIContent.none, col, false, true,true,GUILayout.Width(50f),GUILayout.Height(50f));
                }
                GUILayout.EndHorizontal();

            } 
            
        }
    }
}