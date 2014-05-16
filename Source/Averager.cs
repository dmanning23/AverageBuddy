using System.Collections.Generic;

namespace AverageBuddy
{
	/// <summary>
	/// Template class to help calculate the average value of a history of values. 
	/// This can only be used with types that have a 'zero' and that have the += and / operators overloaded.
	/// Example: Used to smooth frame rate calculations.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Averager<T> where T : struct 
	{
		#region Members

		/// <summary>
		/// this holds the history
		/// </summary>
		private List<T> History { get; set; }

		/// <summary>
		/// The max smaple size we want
		/// </summary>
		public int MaxSize { get; set; }

		/// <summary>
		/// an example of the 'zero' value of the type to be smoothed. 
		/// This would be something like Vector2D(0,0)
		/// </summary>
		private T ZeroValue;

		int _iNext = 0;

		#endregion //Members

		#region Methods

		//to instantiate a Smoother pass it the number of samples you want
		//to use in the smoothing, and an exampe of a 'zero' type
		public Averager(int SampleSize, T zeroValue)
		{
			History = new List<T>();
			MaxSize = SampleSize;
			ZeroValue = zeroValue;
			for (int i = 0; i < MaxSize; i++)
			{
				History.Add(ZeroValue);
			}
		}

		/// <summary>
		/// each time you want to get a new average, feed it the most recent value
		/// and this method will return an average over the last SampleSize updates
		/// </summary>
		/// <param name="mostRecentValue"></param>
		/// <returns></returns>
		public T Update(T mostRecentValue)
		{
			Add(mostRecentValue);

			return Average();
		}

		/// <summary>
		/// Add a new item to the list
		/// </summary>
		/// <param name="mostRecentValue"></param>
		public void Add(T mostRecentValue)
		{
			//add the new value to the correct index
			History[_iNext] = mostRecentValue;
			_iNext++;
			if (_iNext >= MaxSize)
			{
				_iNext = 0;
			}
		}

		/// <summary>
		/// Calculate the average from the current list of stuff
		/// </summary>
		/// <returns></returns>
		public T Average()
		{
			//now to calculate the average of the history list
			dynamic sum = ZeroValue;
			dynamic count = History.Count;

			for (int i = 0; i < count; i++)
			{
				if (0 == i)
				{
					//If this is the first item, use this instead of zero.
					sum = History[i];
				}
				else
				{
					sum += History[i];
				}
			}

			return sum / count;
		}

		#endregion //Methods
	}
}