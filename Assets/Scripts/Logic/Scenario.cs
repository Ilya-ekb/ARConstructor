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
            Logic.Instance.Scenario = this;
        }

        public abstract void StartScenario(params object[] p);
        public abstract void CheckCoffee(params object[] p);
        public abstract void Refuel(params object[] p);
        public abstract void TakeCup(params object[] p);
        public abstract void LookInBox(params object[] p);
        public abstract void ChooseCoffee(params object[] p);
        public abstract void Americano(params object[] p);
        public abstract void DoubleAmericano(params object[] p);
        public abstract void Espresso(params object[] p);
        public abstract void TakeContainer(params object[] p);
        public abstract void SetContainerBack(params object[] p);
        public abstract void DoubleEspresso(params object[] p);
        public abstract void ButtonNo(params object[] p);
        public abstract void ButtonYes(params object[] p);
        public abstract void FinalScenario(params object[] p);
    }
}
