using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DataScripts;
using UnityEngine;

namespace LogicScripts
{
    public class Logic : Singleton<Logic>
    {
        internal Scenario Scenario;

        public void StartScenario()
        {
            Scenario = new TestScenario();
            Scenario.StartScenario();
        }

        /// <summary>
        /// ����� ������� �� ��������
        /// </summary>
        /// <param name="method"></param>��� ������ �� ��������
        /// <param name="parameters"></param>��������� ��� ���������� �������
        public void InvokeFunction(string method, object[] parameters)
        {
            Type scenarioType = Scenario.GetType();
            MethodInfo scenarioMethod = scenarioType.GetMethod(method);
            scenarioMethod.Invoke(Scenario, parameters);
        }


        /// <summary>
        /// �������� ������� � ����� ��� ��������������� ����������
        /// ���������:
        /// DIALOG: string text - ���������, string nameYes - ��� ������ ��� ������������� ������, string nameNo - ��� ������ ��� ������������� ������ 
        /// INFO MESSAGE:  string Text - ���������, string nameOk - ��� ������ ��� ������� �� 
        /// OUTLINE: string name - ��� ����� � ������� ObjDictianory, string message - ��������� �� �����, params string[] param
        /// </summary>
        /// <param name="parameters"></param> 
        internal void SendCommand(params object[] parameters)
        {
            switch ((CommandType)parameters[0])
            {
                case CommandType.Dialog:
                case CommandType.InfoMessage:
                    Dialog.Instance.Activate(parameters);
                    break;

                case CommandType.Outline:
                case CommandType.Outline_flash:
                    SceneOrganizer.Instance.Pointer(parameters);
                    break;

                case CommandType.ChoosePanel:
                    SceneOrganizer.Instance.ChoosePanel(parameters);
                    break;
                case CommandType.PlayAudio:
                    SceneOrganizer.Instance.PlayAudio(parameters);
                    break;
            }
        }
    }
    public enum CommandType
    {
        InfoMessage,
        ChoosePanel,
        Dialog,
        Outline,
        Outline_flash,
        PlayAudio,
    }

}
