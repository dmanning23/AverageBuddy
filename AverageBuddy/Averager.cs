using System.Collections.Generic;
using System.Diagnostics;

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
		#region Fields

		/// <summary>
		/// an example of the 'zero' value of the type to be smoothed. 
		/// This would be something like Vector2D(0,0)
		/// </summary>
		readonly private T zeroValue;

		protected int NextIndex { get; set; }

		readonly object _lock = new object();

		#endregion //Fields

		#region Members

		/// <summary>
		/// this holds the history
		/// </summary>
		protected List<T> History { get; set; }

		/// <summary>
		/// The max smaple size we want
		/// </summary>
		public int MaxSize { get; set; }

		/// <summary>
		/// Set the size of the averager by seconds instead of sample size
		/// Assumes you are updaintg once a frame at 60fps
		/// </summary>
		public float MaxSeconds
		{
			set
			{
				MaxSize = ToFrames(value);
			}
		}

		#endregion //Members

		#region Methods

		//to instantiate a Smoother pass it the number of samples you want
		//to use in the smoothing, and an exampe of a 'zero' type
		public Averager(int sampleSize, T zero)
		{
			//start the index at -1 so we can tell if this is the first entry
			NextIndex = -1;

			MaxSize = sampleSize;
			zeroValue = zero;

			lock (_lock)
			{
				History = new List<T>();
				for (int i = 0; i < MaxSize; i++)
				{
					History.Add(zeroValue);
				}
			}
		}

		/// <summary>
		/// average something over a period of time
		/// </summary>
		/// <param name="sampleSeconds"></param>
		/// <param name="zeroValue"></param>
		public Averager(float sampleSeconds, T zeroValue) :
			this(ToFrames(sampleSeconds), zeroValue)
		{
		}

		/// <summary>
		/// When you really want the averager to return a certain value (say starting it up)
		/// </summary>
		/// <param name="currentValue">the value for the averager to return</param>
		public void Set(T currentValue)
		{
			for (int i = 0; i < History.Count; i++)
			{
				History[i] = currentValue;
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
		public virtual void Add(T mostRecentValue)
		{
			//add the new value to the correct index
			lock (_lock)
			{
				//increment first 
				NextIndex++;
				if (NextIndex >= MaxSize)
				{
					NextIndex = 0;
				}

				Debug.Assert(NextIndex < History.Count);
				History[NextIndex] = mostRecentValue;
			}
		}

		/// <summary>
		/// Calculate the average from the current list of stuff
		/// </summary>
		/// <returns></returns>
		public T Average()
		{
			//now to calculate the average of the history list
			dynamic sum = zeroValue;

			lock (_lock)
			{
				for (int i = 0; i < MaxSize; i++)
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
			}

			return sum / MaxSize;
		}

		private static int ToFrames(float seconds)
		{
			seconds *= 60.0f;
			return (int)(seconds + 0.5f);
		}

		#endregion //Methods
	}
}