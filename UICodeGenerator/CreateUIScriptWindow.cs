/************************************************************
    文件: CreateUIScript.cs
    作者: 那位先生
    邮箱: 1279544114@qq.com
    日期: 2019/12/31 13:57:20
    功能: 绘制代码生成器窗口
*************************************************************/
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Game.Editor
{
    public class CreateUIScriptWindow : EditorWindow
    {
        public static GUIStyle GUIStyle = new GUIStyle();
        public static GUIStyle GUIStyle1 = new GUIStyle();

        [SerializeField] TreeViewState m_TreeViewState;

        // The TreeView is not serializable it should be reconstructed from the tree data.
        UITreeView m_TreeView;
        SearchField m_SearchField;

        private Vector2 _scrollPos1;
        private Vector2 _scrollPos2;

        void OnEnable()
        {
            // Check if we already had a serialized view state (state 
            // that survived assembly reloading)
            if (m_TreeViewState == null)
                m_TreeViewState = new TreeViewState();

            m_TreeView = new UITreeView(m_TreeViewState);
            m_SearchField = new SearchField();
            m_SearchField.downOrUpArrowKeyPressed += m_TreeView.SetFocusAndEnsureSelectedItem;
        }

        void OnGUI()
        {

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical("Box", GUILayout.Width(292));
            EditorGUILayout.LabelField("UI节点展示，选择需要的节点", GUILayout.Width(292));
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("Box", GUILayout.Width(700));
            EditorGUILayout.LabelField("脚本预览", GUILayout.Width(700));
            if (GUI.Button(new Rect(930, 5, 80, 20), "生成脚本文件"))
            {
                UIScript.CreateScript();
            }
            EditorGUILayout.EndVertical();


            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            _scrollPos1 = EditorGUILayout.BeginScrollView(_scrollPos1, "Box", GUILayout.Width(300), GUILayout.Height(500));
            EditorGUILayout.BeginVertical();
            DoToolbar();
            DoTreeView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();

            _scrollPos2 = EditorGUILayout.BeginScrollView(_scrollPos2, "Box", GUILayout.Width(708), GUILayout.Height(500));
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(UIScript.UICode, GUIStyle, GUILayout.Width(650), GUILayout.Height(500 + 105 * m_TreeView.GetSelectNum()));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
        }

        void DoToolbar()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Space(100);
            GUILayout.FlexibleSpace();
            m_TreeView.searchString = m_SearchField.OnToolbarGUI(m_TreeView.searchString);
            GUILayout.EndHorizontal();
        }

        void DoTreeView()
        {
            Rect rect = GUILayoutUtility.GetRect(0, 100000, 0, 100000);
            m_TreeView.OnGUI(rect);
        }

        // Add menu named "My Window" to the Window menu
        [MenuItem("GameObject/UI/UI代码生成器")]
        static void ShowWindow()
        {
            GUIStyle.normal.textColor = Color.yellow;
            GUIStyle.fontSize = 15;
            GUIStyle.alignment = TextAnchor.UpperLeft;

            UIScript.UICode = "";
            var window = GetWindowWithRect<CreateUIScriptWindow>(new Rect(Screen.width / 2, Screen.height / 2, 1020, 540));
            window.titleContent = new GUIContent("UI CODE生成器");
            window.Show();
        }
    }
}
