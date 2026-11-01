using System.ComponentModel;

namespace RobotPlanet.Models
{
    public class SimulationResult : INotifyPropertyChanged
    {
        private string robotName;
        private string status;
        private double result;
        private long durationMs;
        private string error;

        public string RobotName
        {
            get => robotName;
            set { robotName = value; OnPropertyChanged(nameof(RobotName)); }
        }

        public string Status
        {
            get => status;
            set { status = value; OnPropertyChanged(nameof(Status)); }
        }

        public double Result
        {
            get => result;
            set { result = value; OnPropertyChanged(nameof(Result)); }
        }

        public long DurationMs
        {
            get => durationMs;
            set { durationMs = value; OnPropertyChanged(nameof(DurationMs)); }
        }

        public string Error
        {
            get => error;
            set { error = value; OnPropertyChanged(nameof(Error)); }
        }

        public SimulationResult(string robotName)
        {
            this.robotName = robotName;
            status = "Pending";
            result = 0;
            durationMs = 0;
            error = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
