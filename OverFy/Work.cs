using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OverFy
{
    public class Work
    {
        private Thread _workerThread;
        private CancellationTokenSource _cancellationTokenSource;
        bool running = false;

        public Work()
        {

        }

        public void Start()
        {
            if (_workerThread != null) return;

            //selectedjoystick = combo_joysticks.SelectedIndex;

            _cancellationTokenSource = new CancellationTokenSource();
            _workerThread = new Thread(BackgroundWorker_DoWork)
            {
                Name = "OverFly",
                IsBackground = true
            };
            _workerThread.Start(_cancellationTokenSource.Token);
            running = true;

        }

        public void Stop()
        {
            if (_workerThread == null) return;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
            _workerThread.Join();
            _workerThread = null;
            running = false;
        }

        private void BackgroundWorker_DoWork(object tokenObject)
        {
            var cancellationToken = (CancellationToken)tokenObject;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    RivaTuner.print("Writing on rivatuner is ease xdxdxdxdxd");
                    Task.Delay(1000, cancellationToken).Wait(cancellationToken); //60 FPS

                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch
                {

                }
            }
        }
    }
}
