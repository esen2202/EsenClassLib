using EsenLib.Basic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestWPF1
{
    public class ModelView : INotifyPropertyChanged
    {
        private string _elapsed;

        public string Elapsed
        {
            get { return _elapsed; }
            set
            {
                _elapsed = value;
                OnPropertyRaised("Elapsed");
            }
        }
        private string _q;

        public string Q
        {
            get { return _q; }
            set
            {
                _q = value;
                OnPropertyRaised("Q");
            }
        }

        private string _pv;

        public string PV
        {
            get { return _pv; }
            set { _pv = value; }
        }


        Timers.TON ton1;

        public ModelView()
        {
            ton1 = new Timers.TON(Convert.ToInt32(100));
            ton1.OnOutputSetTrue += Ton1_OnOutputSetTrue;
            ton1.OnTimerElapsed += Ton1_OnTimerElapsed;
        }

        private void Ton1_OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //LblElapsedTime.Content = ton1.Elapsed.ToString();
            //LblQ.Content = ton1.Q.ToString();
            Elapsed = ton1.Elapsed.ToString();
            Q = ton1.Q.ToString();
        }

        private void Ton1_OnOutputSetTrue()
        {

        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ton1.PV = Convert.ToInt32(PV);
            ton1.I = true;
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            ton1.I = false;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

    }
}
