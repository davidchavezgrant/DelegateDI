namespace DelegateDI;

internal interface IService
{
	Guid Id { get; }
	Task LogAsync();
	void Log();
}

internal delegate Guid MySingletonDelegate();

internal delegate Guid MyScopedDelegate();

internal delegate Guid MyTransientDelegate();

internal delegate Task<Guid> MySingletonDelegateAsync();

internal delegate Task<Guid> MyScopedDelegateAsync();

internal delegate Task<Guid> MyTransientDelegateAsync();