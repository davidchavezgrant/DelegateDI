namespace DelegateDI;

class MyScopedService: IService, IDisposable
{
	public MyScopedService()
	{
		this.Id = Guid.NewGuid();
		Console.WriteLine($"Created {nameof(MyScopedService)} with ID: {this.Id}");
	}

	public void Dispose() => Console.WriteLine($"Disposed {nameof(MyScopedService)} with ID: {this.Id}");

	/// <inheritdoc />
	public Guid Id { get; }

	/// <inheritdoc />
	public Task LogAsync()
	{
		this.Log();
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public void Log() => Console.WriteLine($"{nameof(MyScopedService)} with ID: {this.Id}");
}