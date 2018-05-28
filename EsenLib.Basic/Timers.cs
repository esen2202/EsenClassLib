using System;

namespace EsenLib.Basic
{

    public class Timers
    {
        

        public class TON : IDisposable
        {
            public delegate void ValueChangedHandler();
            #region Inputs
            /// <summary>
            /// Timer's Input
            /// </summary>
            private bool i;

            public bool I
            {
                get { return i; }
                set
                {
                    i = value;
                    if (value) StartTON();
                    else StopTON();
                }
            }

            /// <summary>
            /// Present Value (ms)
            /// </summary>
            private int pv;

            public int PV
            {
                get { return pv; }
                set { pv = value; }
            }
            #endregion

            #region Outputs
            /// <summary>
            /// Elapsed Time
            /// </summary>
            public int Elapsed { get; private set; }

            /// <summary>
            /// Timer's Output
            /// </summary>
            public bool Q { get; private set; }
            #endregion

            #region Local
            public event ValueChangedHandler OnOutputSetTrue;
            public event ValueChangedHandler OnTimerStarted;
            public event ValueChangedHandler OnTimerStopped;
            public event System.Timers.ElapsedEventHandler OnTimerElapsed;

            private System.Timers.Timer timer;

            #endregion

            #region Methods

            /// <summary>
            /// Constructure
            /// </summary>
            public TON(int tBase)
            {
              
                timer = new System.Timers.Timer(tBase);
                timer.Elapsed += Timer_Elapsed;
                timer.Enabled = true;
            }

            /// <summary>
            /// Timer Elapsed Method
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
            {
                if (Elapsed < pv)
                {
                    Q = false;
                }
                else if (!Q)
                {
                    OnOutputSetTrue?.Invoke();
                    Q = true;
                }

                Elapsed += 1;
                OnTimerElapsed?.Invoke(sender, e);
            }

            private void StartTON()
            {
                timer?.Start();
                OnTimerStarted?.Invoke();
            }

            private void StopTON()
            {
                OnTimerStopped?.Invoke();
                timer?.Stop();
                
            }

            #endregion

            #region IDisposable Support
            private bool disposedValue = false; // To detect redundant calls

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        timer.Dispose();
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                    // TODO: set large fields to null.


                    disposedValue = true;
                }
            }

            // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
            // ~TON() {
            //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            //   Dispose(false);
            // }

            // This code added to correctly implement the disposable pattern.
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                GC.SuppressFinalize(this);
            }
            #endregion
        }

    }
}
