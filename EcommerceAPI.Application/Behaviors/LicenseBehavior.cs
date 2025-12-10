using EcommerceAPI.Application.Infrastructure.Config;
using MediatR;

public class LicenseBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly SecretsConfig _secrets;

    public LicenseBehavior(SecretsConfig secrets)
    {
        _secrets = secrets;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        return await next();
    }
}
