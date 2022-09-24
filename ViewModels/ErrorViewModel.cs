namespace JKNews.ViewModels;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

/* public class ErrorViewModel<T>
{
	public ErrorViewModel(T data, List<string> errors)
	{
		Data = data;
		Errors = errors;
	}

	public ErrorViewModel(T data)
	{
		Data = data;
	}

	public ErrorViewModel(List<string> errors)
	{
		Errors = errors;
	}

	public ErrorViewModel(string error)
	{
		Errors.Add(error);
	}

	public T Data { get; private set; }
	public List<string> Errors { get; private set; } = new();
}*/