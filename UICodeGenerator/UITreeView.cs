/************************************************************
    文件: UITreeView.cs
    作者: 那位先生
    邮箱: 1279544114@qq.com
    日期: 2019/12/31 10:27:31
    功能: UI的树，绘制UI的树结构图
*************************************************************/
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Game.Editor
{
    public class UITreeView : TreeView
    {
        public static GUIStyle GUIStyleFalse = new GUIStyle();
        public static GUIStyle GUIStyleTrue = new GUIStyle();

        private List<TreeViewItem> m_TreeViewItemList;
        public UITreeView(TreeViewState treeViewState) : base(treeViewState)
        {
            GUIStyleFalse.normal.textColor = Color.white;
            GUIStyleFalse.fontSize = 12;
            GUIStyleFalse.alignment = TextAnchor.MiddleLeft;

            GUIStyleTrue.normal.textColor = Color.yellow;
            GUIStyleTrue.fontSize = 12;
            GUIStyleTrue.alignment = TextAnchor.MiddleLeft;
            Reload();
        }
        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            m_TreeViewItemList = UIScript.CreateTreeViewItem();
            SetupParentsAndChildrenFromDepths(root, m_TreeViewItemList);
            return root;
        }
        protected override void RowGUI(RowGUIArgs args)
        {
            //base.RowGUI(args);
            UITreeViewItem item = (UITreeViewItem)args.item;
            var rowsRect = args.rowRect;
            var toggleRect = rowsRect;
            toggleRect.xMin = (15 * (item.depth + 1));
            toggleRect.y += 1f;
            toggleRect.width = 16;
            EditorGUI.BeginChangeCheck();
            if (item.Path != "this")
            {
                item.Select = EditorGUI.Toggle(toggleRect, item.Select); // hide when outside cell rect
            }
            Rect labelRect = toggleRect;
            labelRect.xMin += toggleRect.width + 2f;
            labelRect.width = 1000;
            if (EditorGUI.EndChangeCheck())
            {
                UIScript.CreateUICode();
            }
            if (item.Select)
            {
                GUI.Label(labelRect, args.label, GUIStyleTrue);
                return;
            }
            GUI.Label(labelRect, args.label, GUIStyleFalse);
        }

        /// <summary>
        /// 获取被选中的数量
        /// </summary>
        /// <returns></returns>
        public int GetSelectNum()
        {
            int num = 0;
            for (int i = 0; i < m_TreeViewItemList.Count; i++)
            {
                UITreeViewItem item = (UITreeViewItem)m_TreeViewItemList[i];
                if (item.Select)
                {
                    num++;
                }
            }
            return num;
        }
    }
}
