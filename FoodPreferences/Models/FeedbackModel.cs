using System.Collections.Generic;

namespace FoodPreferences.Models
{
    public class FeedbackModel
    {
        public string Name { get; set; }
        public string Food { get; set; }
        public string Preference { get; set; }

    }

    public class FeedbackModels
    {
        public IList<FeedbackModel> Feedbacks { get; set; } = new List<FeedbackModel>();
        public FeedbackModels()
        {

        }
        public FeedbackModels(IList<FeedbackModel> feedbacks)
        {
            Feedbacks = feedbacks;
        }
    }

}
