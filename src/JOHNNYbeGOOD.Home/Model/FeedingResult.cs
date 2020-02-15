using System;

namespace JOHNNYbeGOOD.Home.Model
{
    public class FeedingResult
    {
        public bool Succeeded { get; private set; }

        public string SlotUsed { get; private set; }


        private FeedingResult()
        {

        }

        /// <summary>
        /// Failed feeding result
        /// </summary>
        /// <returns></returns>
        public static FeedingResult Failed()
        {
            return new FeedingResult { Succeeded = false };
        }

        /// <summary>
        /// Successfull feeding result
        /// </summary>
        /// <param name="slotName"></param>
        /// <returns></returns>
        public static FeedingResult Success(string slotName)
        {
            return new FeedingResult { Succeeded = true, SlotUsed = slotName };
        }
    }
}