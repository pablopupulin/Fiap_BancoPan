using Application.Models;
using Application.Repository;
using Application.UseCases.Login.Models;
using MediatR;

namespace Application.UseCases.Login;

public class LoginUseCase : IRequestHandler<LoginRequest, Notifable<LoginResponse>>
{
    private readonly IIntentionRepository _intentionRepository;
    private readonly ILoginRepository _loginRepository;
    private readonly IUserRepository _userRepository;

    public LoginUseCase(IIntentionRepository intentionRepository, IUserRepository userRepository,
        ILoginRepository loginRepository)
    {
        _intentionRepository = intentionRepository;
        _userRepository = userRepository;
        _loginRepository = loginRepository;
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

        await _loginRepository.SaveLoginAsync(login);

        var response = new LoginResponse(login);

        return new Notifable<LoginResponse>(response);
    }
}