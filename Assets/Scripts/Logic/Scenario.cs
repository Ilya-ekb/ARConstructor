using DataScripts;
using LogicScripts;

namespace LogicScripts
{
    public abstract class Scenario
    {
        protected int scenarioState;

        protected Scenario()
        {
            scenarioState = 0;
            Logic.Instance.scenario = this;
        }

        public abstract void StartScenario(params object[] p);
        public abstract void CheckCoffee(params object[] p);
        public abstract void Refuel(params object[] p);
        public abstract void ButtonNo(params object[] p);
        public abstract void ButtonYes(params object[] p);
    }
}
