namespace TiKiTuTo.Controller
{
    public class Controller
    {
        private StateMachine StateMachine { get; }

        public Controller(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }



        public void Run()
        {
            while (true)
            {
                int userChoice = StateMachine.ExecuteCurrentState();
                StateMachine.HandleUserChoice(userChoice);
            }
        }




    }
}
