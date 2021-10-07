using System.Collections;
using System.Collections.Generic;
using DataScripts;
using UnityEngine;

namespace LogicScripts
{
    public class TestScenario : Scenario
    {
        private Logic logic;
        public TestScenario() : base()
        {
            logic = Logic.Instance;
        }

        public override void StartScenario(params object[] p)
        {
            if (scenarioState == 0)
            {
                scenarioState = 10;
                logic.SendCommand(CommandType.PlayAudio, 1);
                logic.SendCommand(CommandType.Outline, 0,"��������� � �����");
                logic.SendCommand(CommandType.Outline, 1, "������� ����");
                logic.SendCommand(CommandType.Dialog, "������� ������� ���� � ����������\n\n���� ��������?", "CheckCoffee", "Refuel");
            }
        }

        public override void CheckCoffee(params object[] p)
        {
            throw new System.NotImplementedException();                                                                                          
        }

        public override void Refuel(params object[] p)
        {
            throw new System.NotImplementedException();
        }

        public override void ButtonNo(params object[] p)
        {
            throw new System.NotImplementedException();
        }

        public override void ButtonYes(params object[] p)
        {
            throw new System.NotImplementedException();
        }
    }
}
