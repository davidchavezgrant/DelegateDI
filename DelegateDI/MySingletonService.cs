namespace DelegateDI;

class MySingletonService: IService, IDisposable
{
	public MySingletonService()
	{
		this.Id = Guid.NewGuid();
		Console.WriteLine($"Created {nameof(MySingletonService)} with ID: {this.Id}");
	}

	/// <inheritdoc />
	public void Dispose() => Console.WriteLine($"Disposed {nameof(MySingletonService)} with ID: {this.Id}");

	/// <inheritdoc />
	public Guid Id { get; }

	/// <inheritdoc />
	public Task LogAsync()
	{
		this.Log();
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public void Log() => Console.WriteLine($"{nameof(MySingletonService)} with ID: {this.Id}");
}