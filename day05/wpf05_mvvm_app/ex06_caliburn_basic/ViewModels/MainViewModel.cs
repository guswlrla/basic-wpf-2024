using Caliburn.Micro;

namespace ex06_caliburn_basic.ViewModels
{
    class MainViewModel : Conductor<object>
    {
        public string Greeting {  get { return "Hello, Caliburn!!"; } }
    }
}
