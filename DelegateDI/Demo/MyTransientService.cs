namespace DelegateDI.Demo;

internal class MyTransientService: IService, IDisposable
{
	public MyTransientService()
	{
		this.Id = Guid.NewGuid();
		Console.WriteLine($"Created {nameof(MyTransientService)} with ID: {this.Id}");
	}

	public void Dispose() => Console.WriteLine($"Disposed {nameof(MyTransientService)} with ID: {this.Id}");

	/// <inheritdoc />
	public Guid Id { get; }

	/// <inheritdoc />
	public Task LogAsync()
	{
		this.Log();
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public void Log() => Console.WriteLine($"{nameof(MyTransientService)} with ID: {this.Id}");
}