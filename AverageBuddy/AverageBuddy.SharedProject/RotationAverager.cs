using System;

namespace AverageBuddy
{
	/// <summary>
	/// Template class to help calculate the average value of a history of rotation values. 
	/// </summary>
	public class RotationAverager : Averager<float>
	{
		#region Methods

		//to instantiate a Smoother pass it the number of samples you want
		//to use in the smoothing, and an exampe of a 'zero' type
		public RotationAverager(int sampleSize) :
			base(sampleSize, 0.0f)
		{
		}

		/// <summary>
		/// average something over a period of time
		/// </summary>
		/// <param name="sampleSeconds"></param>
		public RotationAverager(float sampleSeconds) :
			base(sampleSeconds, 0.0f)
		{
		}

		/// <summary>
		/// Add a new item to the list
		/// Also does special logic for cleaning up -180,180 problem
		/// </summary>
		/// <param name="mostRecentValue"></param>
		public override void Add(float mostRecentValue)
		{
			//make sure this isn't the first item to be added
			if (-1 < NextIndex)
			{
				//get the last item
				float prevRotation = History[NextIndex];

				//get the difference between that one and this one
				if ((prevRotation - mostRecentValue) > Math.PI)
				{
					//put in a valid range
					mostRecentValue += (float)(2.0 * Math.PI);
				}
				else if ((prevRotation - mostRecentValue) < -Math.PI)
				{
					mostRecentValue -= (float)(2.0 * Math.PI);
				}
			}

			base.Add(mostRecentValue);
		}

		#endregion //Methods
	}
}