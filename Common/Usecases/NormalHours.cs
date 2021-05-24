using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{

    public class NormalHours : IRule
    {
        [JsonProperty]
        private int _millisecondsWait;

        public NormalHours(int millisecondsWait)
        {
            _millisecondsWait = millisecondsWait;
        }

        public Task Perform(int stayedTime = 0) =>
            Task.Run(() =>
            Thread.Sleep(_millisecondsWait - stayedTime < 0 ? 0 : _millisecondsWait - stayedTime));

        public int GetStayedTime()=> _millisecondsWait;
        
    }
}
