using Application.Models;
using Application.Repository;
using Application.UseCases.Login.Models;
using MediatR;

namespace Application.UseCases.Login;

public class LoginUseCase : IRequestHandler<LoginRequest, Notifable<LoginResponse>>
{
    private readonly IIntentionRepository _intentionRepository;
    private readonly ILoginRepository _loginRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;

    public LoginUseCase(IIntentionRepository intentionRepository, IUserRepository userRepository,
        ILoginRepository loginRepository, IRefreshTokenRepository refreshTokenRepository)
    {
        _intentionRepository = intentionRepository;
        _userRepository = userRepository;
        _loginRepository = loginRepository;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Notifable<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var intention = await _intentionRepository.GetIntentionAsync(request.IntentionId, cancellationToken);

        if (intention == null) return new Notifable<LoginResponse>("Tente novamente");

        var user = await _userRepository.GetUserAsync(intention.User);

        if (user is null) return new Notifable<LoginResponse>("Usuario ou senha invalido");

        var possiblePasswords = intention.GetPossiblePasswords(request.Password);

        if (possiblePasswords is null) return new Notifable<LoginResponse>("Usuario ou senha invalido");

        if (!user.ValidPasswords(possiblePasswords)) return new Notifable<LoginResponse>("Usuario ou senha invalido");

        var login = user.Login();
        var refreshToken = login.GetRefreshToken();

        await Task.WhenAll(
            _loginRepository.SaveLoginAsync(login, cancellationToken),
            _refreshTokenRepository.SaveRefreshTokenAsync(refreshToken, refreshToken.ExpireIn, cancellationToken));

        var response = new LoginResponse(login);

        return new Notifable<LoginResponse>(response);
    }
}