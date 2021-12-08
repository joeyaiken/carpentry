using System.Threading.Tasks;

namespace Carpentry.PlaywrightTests.Common
{
    public interface IRunnableTest
    {
        Task Run();
    }
}