using UnityEngine;
using UnityEditor;
using DuloGames.UI;
namespace Game.UI {
[CustomEditor(typeof(UIRecievableItemSlot), true)]
public class UIRecievableItemSlotEditor : UIEditor.UISlotBaseEditor {
    private SerializedProperty m_backgroundQuality;
    private SerializedProperty m_qualityFrame;
    private SerializedProperty m_plusText;
    private SerializedProperty m_amountText;
    private SerializedProperty m_boundIcon;
    private SerializedProperty m_selected;
    protected override void OnEnable() {
        base.OnEnable();
        
        this.m_backgroundQuality = this.serializedObject.FindProperty("BackgroundQuality");
        this.m_qualityFrame = this.serializedObject.FindProperty("QualityFrame");
        this.m_plusText = this.serializedObject.FindProperty("PlusText");
        this.m_amountText = this.serializedObject.FindProperty("AmountText");
        this.m_boundIcon = this.serializedObject.FindProperty("BoundIcon");
        this.m_selected = this.serializedObject.FindProperty("Recieved");
    }
    protected void DrawCustomProperties() {
        EditorGUILayout.LabelField("Custom Properties", EditorStyles.boldLabel);
        EditorGUI.indentLevel = (EditorGUI.indentLevel + 1);
        
        EditorGUILayout.PropertyField(this.m_backgroundQuality, new GUIContent("Background Quality"));
        EditorGUILayout.PropertyField(this.m_qualityFrame, new GUIContent("Quality Frame"));
        EditorGUILayout.PropertyField(this.m_plusText, new GUIContent("Plus Text"));
        EditorGUILayout.PropertyField(this.m_amountText, new GUIContent("Amount Text"));
        EditorGUILayout.PropertyField(this.m_boundIcon, new GUIContent("Bound Icon"));
        EditorGUILayout.PropertyField(this.m_selected, new GUIContent("SelectedObj"), true);

        EditorGUI.indentLevel = (EditorGUI.indentLevel - 1);
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        this.DrawCustomProperties();
        this.serializedObject.ApplyModifiedProperties();
    }
}
}