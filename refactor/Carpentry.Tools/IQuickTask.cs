namespace Carpentry.Tools;

/// <summary>
/// Provides a common interface for tasks contained in the Carpentry.Tools console app
/// A 'Quick Task' is a specific task that can be called with a console param/launch setting
/// </summary>
public interface IQuickTask
{
    Task Run();
}