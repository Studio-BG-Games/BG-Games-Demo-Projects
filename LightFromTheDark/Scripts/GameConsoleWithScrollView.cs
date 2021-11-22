using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameConsoleWithScrollView : MonoBehaviour
{
    public bool show_output = true;
    public bool show_stack = false;
    public static GameConsoleWithScrollView I;
    void Awake()
    {
        I = this;
        strb.AppendLine("CONSOLE:");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            show = !show;
            Debug.Log("~");
        }
    }

    int error_count = 0;

    System.Text.StringBuilder strb = new System.Text.StringBuilder();

    void OnEnable()
    {
        Application.RegisterLogCallback(HandleLog);
    }

    [System.Obsolete]
    void OnDisable()
    {
        Application.RegisterLogCallback(null);
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Exception || type == LogType.Error)
        {
            error_count++;
        }

        if (show_output || show_stack)
        {
            //strb.Append("\n");
            if (show_output)
            {
                strb.AppendLine(logString);
            }
            //вписываем стек всегда если есть ошибка
            if (show_stack || type == LogType.Exception || type == LogType.Error)
            {
                strb.AppendLine(stackTrace);
            }
        }
    }

    Rect pos_rect = new Rect(50, 75 + 50, 400, 400);
    public Rect view_rect = new Rect(0, 0, 400, 60000);
    Vector2 scroll_pos;
    public bool show = false;
    public void OnGUI()
    {
        if (show)
        {
            //strb!
            GUI.Label(new Rect(pos_rect.x, pos_rect.y - 20, 200, 50), "[errors " + error_count + "] length: " + strb.Length);//, "box");

            scroll_pos = GUI.BeginScrollView(pos_rect, scroll_pos, view_rect);
            GUI.TextArea(new Rect(0, 0, view_rect.width - 50, view_rect.height), strb.ToString());
            GUI.EndScrollView();
        }
    }
}