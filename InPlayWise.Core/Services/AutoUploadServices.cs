using InPlayWise.Core.IServices;
using System.Timers;

namespace InPlayWise.Core.Services
{
	public class AutoUploadServices
	{
		private Thread _timerThread;
		private bool _isTimerActive = true;

		public void StartTimer(int interval)
		{
			_timerThread = new Thread(() =>
			{
				while (_isTimerActive)
				{
					// Code to be executed at a fixed interval
					Console.WriteLine($"Timer event at {DateTime.Now}");
					Thread.Sleep(interval);
				}
			});

			_timerThread.Start();
		}

		public void StopTimer()
		{
			_isTimerActive = false;
			_timerThread.Join();
		}
	}
}


