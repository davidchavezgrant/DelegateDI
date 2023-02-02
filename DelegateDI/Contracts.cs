namespace DelegateDI;

internal interface IService
{
	Guid Id { get; }
	Task LogAsync();
	void Log();
}

internal delegate void MySingletonDelegate();

internal delegate void MyScopedDelegate();

internal delegate void MyTransientDelegate();

internal delegate Task MySingletonDelegateAsync();

internal delegate Task MyScopedDelegateAsync();

internal delegate Task MyTransientDelegateAsync();