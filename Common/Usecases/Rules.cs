using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Rules : List<IRule>
    {
        [JsonProperty]
        private int _stayedTime = 0;

        public void Perform(int stayedTime = 0)
        {
            // if pick hours not performed, run normal hours rules.
            if (!PerformPeakHours(stayedTime)) PerformNormalHours(stayedTime);
        }

        public int GetStayTime()
        {
            var pickHours = this.FindAll(item => item is PeakHours);
            var isPickHours = pickHours.Any(item => ((PeakHours)item).IsAccepted());

            if (isPickHours) 
                return pickHours.Max(item => item.GetStayedTime());

            return this
                .FindAll(item => item is NormalHours)
                .Max(item => item.GetStayedTime()); 
        }

        private bool PerformPeakHours(int stayedTime = 0)
        {
            var pickHours = this.FindAll(item => item is PeakHours);
            var isPickHours = pickHours.Any(item => ((PeakHours)item).IsAccepted());

            if (isPickHours)
            {
                Parallel.ForEach(pickHours, item => item.Perform(stayedTime)?.Wait());
                _stayedTime = pickHours.Max(item => item.GetStayedTime());
            }

            return isPickHours;
        }

        private void PerformNormalHours(int stayedTime = 0)
        {
            var normalHours = this.FindAll(item => item is NormalHours);
            Parallel.ForEach(normalHours, item => item.Perform(stayedTime)?.Wait());
            _stayedTime = normalHours.Max(item => item.GetStayedTime());
        }
    }
}
