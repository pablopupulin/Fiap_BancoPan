using Application.Models;
using Application.Repository;
using Application.UseCases.Refresh.Models;
using MediatR;

namespace Application.UseCases.Refresh;

public class RefreshUseCase : IRequestHandler<RefreshRequest, Notifable<RefreshResponse>>
{
    private readonly ILoginRepository _loginRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshUseCase(ILoginRepository loginRepository, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
    {
        _loginRepository = loginRepository;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Notifable<RefreshResponse>> Handle(RefreshRequest request, CancellationToken cancellationToken)
    {
        var oldToken = await _refreshTokenRepository.GetRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (oldToken is null)
            return new Notifable<RefreshResponse>("Invalid Code");

        if (oldToken.ExpireIn < DateTimeOffset.Now)
            return new Notifable<RefreshResponse>("Invalid Code");

        var user = await _userRepository.GetUserAsync(oldToken.UserId);

        if (user is null)
            return new Notifable<RefreshResponse>("Invalid Code");

        var login = user.Login();
        var refreshToken = login.GetRefreshToken();

        await Task.WhenAll(_loginRepository.SaveLoginAsync(login, cancellationToken),
            _refreshTokenRepository.SaveRefreshTokenAsync(refreshToken, refreshToken.ExpireIn, cancellationToken));

        var response = new RefreshResponse(login);
        return new Notifable<RefreshResponse>(response);
    }
}