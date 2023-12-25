using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;


namespace NF.Editor
{
    /// <summary>
    /// 
    /// </summary>
    public class CodeTempServices : EditorWindow
    {
        string mDirectoryPaths = string.Empty;
        string mDataName = "TempData";
        string mServiceName = "TempService";
        string mTableInfoName = "NFTableInfo";

        string mUICodeData = "";
        string mUICodeService = "";

        void OnGUI()
        {
            GUILayout.Label("将目标目录拖到窗口中");
            GUILayout.Space(20);
            if (mouseOverWindow == this)
            {//鼠标位于当前窗口
                if (Event.current.type == EventType.DragUpdated)
                {//拖入窗口未松开鼠标
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;//改变鼠标外观
                }
                else if (Event.current.type == EventType.DragExited)
                {//拖入窗口并松开鼠标
                    Focus();//获取焦点，使unity置顶(在其他窗口的前面)
                    if (DragAndDrop.paths != null)
                    {
                        int len = DragAndDrop.paths.Length;
                        for (int i = 0; i < len; i++)
                        {
                            mDirectoryPaths = DragAndDrop.paths[i];
                        }
                    }
                }
            }
            GUILayout.Label("目标路径: " + mDirectoryPaths);
            GUILayout.Space(20);

            GUILayout.Label("Service文件名:");
            mServiceName = GUILayout.TextField(mServiceName);

            GUILayout.Label("Data文件名:");
            mDataName = GUILayout.TextField(mDataName);

            GUILayout.Space(20);
            GUILayout.Label("Service使用Info表");
            mTableInfoName = GUILayout.TextField(mTableInfoName);

            if (GUILayout.Button("创建文件"))
            {
                OnCreateFiles();
            }
        }

        public void OnEnable()
        {
            this.titleContent = new GUIContent("创建模板Service文件");//设置窗口标题
        }

        private void OnDisable()
        {

        }

        private void OnDestroy()
        {
            
        }

        //替换命名空间
        void OnCreateFiles()
        {
            mUICodeData = Resources.Load<TextAsset>("TemplateData").text;
            mUICodeService = Resources.Load<TextAsset>("TemplateService").text;

            mUICodeData = mUICodeData.Replace("$DataScriptName$", mDataName);
            mUICodeService = mUICodeService.Replace("$ServiceScriptName$" , mServiceName);
            mUICodeService = mUICodeService.Replace("$DataScriptName$", mDataName);
            mUICodeService = mUICodeService.Replace("$TableInfo$", mTableInfoName);


            if (!Directory.Exists(mDirectoryPaths)){
                UnityEngine.Debug.Log("创建" + mDirectoryPaths + "文件夹");
                Directory.CreateDirectory(mDirectoryPaths);
            }
            File.WriteAllText(mDirectoryPaths + "/" + mServiceName + ".cs", mUICodeService, System.Text.Encoding.UTF8);
            UnityEngine.Debug.Log("创建" + mServiceName + ".cs 成功");
            
            File.WriteAllText(mDirectoryPaths + "/" + mDataName + ".cs", mUICodeData, System.Text.Encoding.UTF8);
            UnityEngine.Debug.Log("创建" + mDataName + ".cs 成功");

            AssetDatabase.Refresh();
        }


        
    }

    /// <summary>
    /// 替换命名空间
    /// </summary>
    public class EditorCodeTempServices
    {
        [MenuItem("NFTools/Code/创建Service代码模板", false, 0x0002)]
        public static void CreateModuleFiles()
        {
            EditorWindow.GetWindow(typeof(CodeTempServices));
        }
    }
}
