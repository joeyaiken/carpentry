using System.Threading.Tasks;

namespace Carpentry.PlaywrightTests.e2e
{
    public interface IRunnableTest
    {
        Task Run();
    }
}