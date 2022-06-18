using Application.Models;
using Application.Repository;
using Application.UseCases.Refresh.Models;
using MediatR;

namespace Application.UseCases.Refresh;

public class RefreshUseCase : IRequestHandler<RefreshRequest, Notifable<RefreshResponse>>
{
    private readonly ILoginRepository _loginRepository;
    private readonly IUserRepository _userRepository;

    public RefreshUseCase(ILoginRepository loginRepository, IUserRepository userRepository)
    {
        _loginRepository = loginRepository;
        _userRepository = userRepository;
    }

    public async Task<Notifable<RefreshResponse>> Handle(RefreshRequest request, CancellationToken cancellationToken)
    {
        var oldLogin = await _loginRepository.GetLoginAsync(request.RefreshToken);

        if (oldLogin is null)
            return new Notifable<RefreshResponse>("Invalid Code");

        if (oldLogin.ExpireIn.AddSeconds(1) > DateTimeOffset.Now)
            return new Notifable<RefreshResponse>("Invalid Code");

        var user = await _userRepository.GetUserAsync(oldLogin.UserId);

        if (user is null)
            return new Notifable<RefreshResponse>("Invalid Code");

        var login = user.Login();

        await _loginRepository.SaveLoginAsync(login);

        var response = new RefreshResponse(login);
        return new Notifable<RefreshResponse>(response);
    }
}