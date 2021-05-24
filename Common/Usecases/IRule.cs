using System.Threading.Tasks;

namespace Common
{
    public interface IRule
    {
        Task Perform(int stayedTime = 0); 
        int GetStayedTime();
    }
}
