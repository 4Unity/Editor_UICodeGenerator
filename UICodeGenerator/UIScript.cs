/************************************************************
    文件: UIScript.cs
    作者: 那位先生
    邮箱: 1279544114@qq.com
    日期: 2019/12/31 16:23:51
    功能: 生成UI代码
*************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;


namespace Game.Editor
{
    public class UIScript : MonoBehaviour
    {
        public static string UICode { get; set; }//保存要生成的脚本代码字符串
        private static List<TreeViewItem> m_TreeViewItem = new List<TreeViewItem>();//树list
        private static int m_Id = 0;//树的自增长id

        //字符串1，声明组件代码的模板
        private const string Image = "\t\tprivate Image m{0};";
        private const string Text = "\t\tprivate Text m{0};";
        private const string Button = "\t\tprivate Button m{0};";
        private const string Transform = "\t\tprivate Transform m{0};";
        private const string InputField = "\t\tprivate InputField m{0};";
        private const string ScrollList = "\t\tprivate ScrollList m{0};";
        private const string Dropdown = "\t\tprivate Dropdown m{0};";
        private const string Scrollbar = "\t\tprivate Scrollbar m{0};";
        private const string Slider = "\t\tprivate Slider m{0};";
        private const string Toggle = "\t\tprivate Toggle m{0};";

        // 字符串2，查找组件代码的模板
        private const string FindImage = "\t\t\tm{0} = transform.Find(\"{1}\").GetComponent<Image>();";
        private const string FindText = "\t\t\tm{0} = transform.Find(\"{1}\").GetComponent<Text>();";
        private const string FindButton = "\t\t\tm{0} = transform.Find(\"{1}\").GetComponent<Button>();";
        private const string FindInputField = "\t\t\tm{0} = transform.Find(\"{1}\").GetComponent<InputField>();";
        private const string FindScrollList = "\t\t\tm{0} = transform.Find(\"{1}\").GetComponent<ScrollList>();";
        private const string FindDropdown = "\t\t\tm{0} = transform.Find(\"{1}\").GetComponent<Dropdown>();";
        private const string FindScrollbar = "\t\t\tm{0} = transform.Find(\"{1}\").GetComponent<Scrollbar>();";
        private const string FindSlider = "\t\t\tm{0} = transform.Find(\"{1}\").GetComponent<Slider>();";
        private const string FindToggle = "\t\t\tm{0} = transform.Find(\"{1}\").GetComponent<Toggle>();";
        private const string FindTransform = "\t\t\tm{0} = transform.Find(\"{1}\");";

        // 字符串3，查找组件代码的模板
        private const string RegistEvent = "\t\t\tm{0}.onClick.AddListener(OnClick{0});";
        // 字符串3，查找组件代码的模板
        private const string EventFuncCode = "\t\tprivate void OnClick{0}()\n\t";

        // 字符串4，查找组件代码的模板
        private const string RegistItemRenderEvent = "\t\t\tm{0}.onCellForRowAtIndexPath = ItemRender{0};";
        // 字符串4，查找组件代码的模板
        private const string EventItemRenderFuncCode = "\t\tprivate void ItemRender{0}(int index, Transform child, UICell cell)\n\t";

        // 字符串4，查找组件代码的模板
        private const string RegistInitScrollListEvent = "\t\t\tm{0}.onInitScrollList = InitScrollList{0};";
        // 字符串4，查找组件代码的模板
        private const string EventInitScrollListFuncCode = "\t\tprivate void InitScrollList{0}()\n\t";


        //脚本名称，和预制体名称一致
        private static string m_ScriptName = "";

        private static GameObject m_Root = null;
        //脚本保存路径
        private static string Path = Application.dataPath + "/GameMain/_ScriptGenerator";

        //保存需要生成的组件的名称
        private static List<string> m_NameList = new List<string>();

        /// <summary>
        /// 生成UI代码字符串
        /// </summary>
        /// <returns></returns>
        public static string CreateUICode()
        {
            Sort(m_TreeViewItem);
            UICode = "";
            //组件变量代码字符串
            string m_UIComponentCode = "";
            //查找组件代码字符串
            string m_FindUIComponentCode = "";
            //注册按钮代码
            string m_RegistUICode = "";
            //按钮事件代码
            string m_UIEventCode = "";

            m_NameList.Clear();
            for (int i = 0; i < m_TreeViewItem.Count; i++)
            {
                UITreeViewItem uITreeViewItem = (UITreeViewItem)m_TreeViewItem[i];
                if (!uITreeViewItem.Select)
                    continue;
                int j = 1;
                string nameStr = uITreeViewItem.Name;
                while (m_NameList.Contains(nameStr))
                {
                    nameStr = uITreeViewItem.displayName + "_" + j++;
                }
                uITreeViewItem.Name = nameStr;
                m_NameList.Add(nameStr);
                //单条组件变量代码字符串
                string m_UIComCode = "";
                //单条查找组件代码字符串
                string m_FindUIComCode = "";
                //单条注册按钮代码
                string m_ReUICode = "";
                //单条按钮事件代码
                string m_UIEveCode = "";
                string name = uITreeViewItem.displayName.ToUpper();
                if (name.Contains("BUTTON") || name.Contains("BTN"))//按钮
                {
                    m_FindUIComCode = string.Format(FindButton, uITreeViewItem.Name.Replace(" ",""), uITreeViewItem.Path);
                    m_UIComCode = string.Format(Button, uITreeViewItem.Name.Replace(" ", ""));
                    
                    m_ReUICode = string.Format(RegistEvent, uITreeViewItem.Name.Replace(" ", ""));
                    m_UIEveCode = string.Format(EventFuncCode, uITreeViewItem.Name.Replace(" ", ""))+"\t{\n\t\t}";
                    if (m_RegistUICode != "")
                    {
                        m_RegistUICode = m_RegistUICode + "\n" + m_ReUICode;
                    }
                    else { m_RegistUICode += m_ReUICode; }
                    if (m_UIEventCode != "")
                    {
                        m_UIEventCode = m_UIEventCode + "\n" + m_UIEveCode;
                    }
                    else { m_UIEventCode += m_UIEveCode; }
                }
                //图片
                else if (name.Contains("IMAGE") || name.Contains("IMG") || name.Contains("BG") || name.Contains("ICON") || name.Contains("TEXTURE"))
                {
                    m_FindUIComCode = string.Format(FindImage, uITreeViewItem.Name.Replace(" ", ""), uITreeViewItem.Path);
                    m_UIComCode = string.Format(Image, uITreeViewItem.Name.Replace(" ", ""));
                }
                //文本
                else if (name.Contains("TEXT") || name.Contains("TXT"))
                {
                    m_FindUIComCode = string.Format(FindText, uITreeViewItem.Name.Replace(" ", ""), uITreeViewItem.Path);
                    m_UIComCode = string.Format(Text, uITreeViewItem.Name.Replace(" ", ""));
                }
                //InputField
                else if (name.Contains("INPUTFIELD"))
                {
                    m_FindUIComCode = string.Format(FindInputField, uITreeViewItem.Name.Replace(" ", ""), uITreeViewItem.Path);
                    m_UIComCode = string.Format(InputField, uITreeViewItem.Name.Replace(" ", ""));
                }
                //Dropdown
                else if (name.Contains("DROPDOWN"))
                {
                    m_FindUIComCode = string.Format(FindDropdown, uITreeViewItem.Name.Replace(" ", ""), uITreeViewItem.Path);
                    m_UIComCode = string.Format(Dropdown, uITreeViewItem.Name.Replace(" ", ""));
                }
                //Scrollbar
                else if (name.Contains("SCROLLBAR"))
                {
                    m_FindUIComCode = string.Format(FindScrollbar, uITreeViewItem.Name.Replace(" ", ""), uITreeViewItem.Path);
                    m_UIComCode = string.Format(Scrollbar, uITreeViewItem.Name.Replace(" ", ""));
                }
                //ScrollRect
                else if (name.Contains("SCROLL"))
                {
                    m_FindUIComCode = string.Format(FindScrollList, uITreeViewItem.Name.Replace(" ", ""), uITreeViewItem.Path);
                    m_UIComCode = string.Format(ScrollList, uITreeViewItem.Name.Replace(" ", ""));

                    string m_ReUICode1 = string.Format(RegistItemRenderEvent, uITreeViewItem.Name.Replace(" ", ""));
                    string m_ReUICode2 = string.Format(RegistInitScrollListEvent, uITreeViewItem.Name.Replace(" ", ""));

                    string m_UIEveCode1 = string.Format(EventItemRenderFuncCode, uITreeViewItem.Name.Replace(" ", ""))+"\t{\n\t\t}";
                    string m_UIEveCode2 = string.Format(EventInitScrollListFuncCode, uITreeViewItem.Name.Replace(" ", ""))+"\t{\n\t\t}";

                    if (m_RegistUICode != "")
                    {
                        m_RegistUICode = m_RegistUICode + "\n" + m_ReUICode1 + "\n" + m_ReUICode2;
                    }
                    else { m_RegistUICode += m_ReUICode; }
                    if (m_UIEventCode != "")
                    {
                        m_UIEventCode = m_UIEventCode + "\n" + m_UIEveCode1+ "\n" + m_UIEveCode2;
                    }
                    else { m_UIEventCode += m_UIEveCode1+ "\n" + m_UIEveCode2; }
                }
                //Slider
                else if (name.Contains("SLIDER"))
                {
                    m_FindUIComCode = string.Format(FindSlider, uITreeViewItem.Name.Replace(" ", ""), uITreeViewItem.Path);
                    m_UIComCode = string.Format(Slider, uITreeViewItem.Name.Replace(" ", ""));
                }
                //Toggle
                else if (name.Contains("TOGGLE"))
                {
                    m_FindUIComCode = string.Format(FindToggle, uITreeViewItem.Name.Replace(" ", ""), uITreeViewItem.Path);
                    m_UIComCode = string.Format(Toggle, uITreeViewItem.Name.Replace(" ", ""));
                }
                //Tranform
                else
                {
                    m_FindUIComCode = string.Format(FindTransform, uITreeViewItem.Name.Replace(" ", ""), uITreeViewItem.Path);
                    m_UIComCode = string.Format(Transform, uITreeViewItem.Name.Replace(" ", ""));
                }
                if (m_FindUIComponentCode != "")
                {
                    m_FindUIComponentCode = m_FindUIComponentCode + "\n" + m_FindUIComCode;
                }
                else { m_FindUIComponentCode += m_FindUIComCode; }
                if (m_UIComponentCode != "")
                {
                    m_UIComponentCode = m_UIComponentCode + "\n" + m_UIComCode;
                }
                else { m_UIComponentCode += m_UIComCode; }
            }
            UICode = Resources.Load<TextAsset>("Template").text;
            UICode = UICode.Replace("$UI Component Variables$", m_UIComponentCode).Replace("$FindUICode$", m_FindUIComponentCode).Replace("$NewBehaviourScript$", m_ScriptName);
            UICode = UICode.Replace("$AddBtnCode$", m_RegistUICode).Replace("$UIEventCode$", m_UIEventCode);
            UICode = UICode.Replace("$Time$", string.Concat(DateTime.Now.Year, "/", DateTime.Now.Month, "/", DateTime.Now.Day, " ", DateTime.Now.Hour, ":", DateTime.Now.Minute, ":", DateTime.Now.Second));
            return UICode;
        }

        /// <summary>
        /// 创建UI脚本的文件
        /// </summary>
        /// <param name="uIScript"></param>
        public static void CreateScript()
        {
            if (!Directory.Exists(Path))
            {
                Debug.Log(Path + "路径，不存在文件夹:" + m_ScriptName);
                Debug.Log("创建" + m_ScriptName + "文件夹");
                Directory.CreateDirectory(Path);
            }
            File.WriteAllText(Path + "/" + m_ScriptName + ".cs", UICode, System.Text.Encoding.UTF8);
            UICode = "生成成功";
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 生成树
        /// </summary>
        public static List<TreeViewItem> CreateTreeViewItem()
        {
            m_Id = 0;
            m_TreeViewItem.Clear();
            m_Root = Selection.activeGameObject;
            m_ScriptName = m_Root.name;
            ForeachGameObject(m_Root);
            return m_TreeViewItem;
        }

        /// <summary>
        /// 递归函数,遍历所有节点
        /// </summary>
        private static void ForeachGameObject(GameObject go)
        {
            int m_depth = 0;
            string m_displayName = go.name;
            string m_Name = go.name;
            string path = go.name;
            if (go == m_Root)
            {
                path = "this";
            }
            Transform ts = go.transform;
            while (ts.parent != null && ts.gameObject != m_Root)
            {
                ts = ts.parent;
                m_depth++;
                if (ts.gameObject != m_Root)
                {
                    path = ts.name + "/" + path;
                }
                else
                {
                    break;
                }
            }
            UITreeViewItem uITreeViewItem = new UITreeViewItem { id = m_Id++, depth = m_depth, displayName = m_displayName, Select = false, Path = path, Name = m_Name };
            m_TreeViewItem.Add(uITreeViewItem);
            for (int i = 0; i < go.transform.childCount; i++)
            {
                ForeachGameObject(go.transform.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 排序函数,对Code进行排序
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static void Sort(List<TreeViewItem> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count - i - 1; j++)
                {
                    if (string.Compare(list[j].displayName, list[j + 1].displayName) == 1)
                    {
                        var temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }
    }
}
