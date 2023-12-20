/************************************************************
    文件: UITreeViewItem.cs
    作者: 那位先生
    邮箱: 1279544114@qq.com
    日期: 2019/12/31 10:24:53
    功能: 生成UI脚本的树子模板
*************************************************************/
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Game.Editor
{
    public class UITreeViewItem : TreeViewItem
    {
        /// <summary>
        /// 树的路径
        /// </summary>
        public string Path;
        /// <summary>
        /// 是否被选中
        /// </summary>
        public bool Select = false;
        /// <summary>
        /// 要生成的名称
        /// </summary>
        public string Name;
    }
}
