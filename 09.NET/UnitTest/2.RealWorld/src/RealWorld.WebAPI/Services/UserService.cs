using FluentValidation;
using Microsoft.Extensions.Logging;
using RealWorld.WebAPI.Dtos;
using RealWorld.WebAPI.Logging;
using RealWorld.WebAPI.Models;
using RealWorld.WebAPI.Repositories;
using RealWorld.WebAPI.Validators;
using System.Diagnostics;

namespace RealWorld.WebAPI.Services;

public sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILoggerAdaptor<IUserService> _logger;

    public UserService(IUserRepository userRepository, ILoggerAdaptor<IUserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Tüm kullanıcılar getiriliyor.");
        try
        {
            return await _userRepository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Bir hata ile karşılaşıldı.");
            throw;
        }
        finally
        {
            _logger.LogInformation("Tüm kullanıcı listesi çekildi.");
        }
    }

    public async Task<bool> CreateAsync(CreateUserDto request, CancellationToken cancellationToken = default)
    {
        CreateUserDtoValidator validator = new();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            throw new ValidationException(string.Join(", ", result.Errors.Select(s => s.ErrorMessage)));
        }

        var nameIsExist = await _userRepository.NameIsExists(request.Name, cancellationToken);
        if (nameIsExist)
        {
            throw new ArgumentException("Bu isim daha önce kaydedilmiş");
        }

        var user = CreateUserDtoToUserObject(request);

        _logger.LogInformation("Kullanıcı adı: {0} bu olan kullanıcı kaydı yapılmaya başlandı.", user.Name);
        var stopWatch = Stopwatch.StartNew();
        try
        {
            return await _userRepository.CreateAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı kaydı esnasında bir hatayla karşılaştım");
            throw;
        }
        finally
        {
            stopWatch.Stop();
            _logger.LogInformation("User Id: {0} olan kullanıcı {1}ms de oluşturuldu", user.Id, stopWatch.ElapsedMilliseconds);
        }
    }

    public User CreateUserDtoToUserObject(CreateUserDto request)
    {
        return new User()
        {
            Name = request.Name,
            DateOfBirth = request.DateOfBirth,
            Age = request.Age,
        };
    }

    public async Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        User? user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if(user is null)
        {
            throw new ArgumentException("Kullanıcı bulunamadı");
        }

        _logger.LogInformation("{0} id numarasına sahip kullanıcı siliniyor.", id);
        var stopWatch = Stopwatch.StartNew();

        try
        {
            return await _userRepository.DeleteAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Kullanıcı silinirken bir hatayla karşılaşıldı.");
            throw;
        } finally
        {
            stopWatch.Stop();
            _logger.LogInformation("Kullanıcı id'si {0} olan kullanıcı kaydı {1}ms de silindi", user.Id, stopWatch.ElapsedMilliseconds);
        }
    }

    public async Task<bool> UpdateAsync(UpdateUserDto request, CancellationToken cancellationToken = default)
    {
        User? user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
        {
            throw new ArgumentException("Kullanıcı bulunamadı.");
        }

        UpdateUserDtoValidator validator = new();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            throw new ValidationException(string.Join("\n", result.Errors.Select(s => s.ErrorMessage)));
        }

        if(request.Name != user.Name)
        {
            var nameIsExist = await _userRepository.NameIsExists(request.Name, cancellationToken);

            if(nameIsExist) 
            {
                throw new ArgumentException("Bu isim dha önce kaydedilmiş.");
            }
        }

        CreateUpdateUserObject(ref user, request);

        _logger.LogInformation("{0} kullanıcının güncelleme işlemi yapılmaya başlandı.", request.Name);
        var stopWatch = Stopwatch.StartNew();
        try
        {
            return await _userRepository.UpdateAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Kullanıcı güncelleme esnasında bir hata ile karşılaştım.");
            throw;

        } finally
        {
            stopWatch.Stop();
            _logger.LogInformation("{0} id'li kullanıcının güncelleme işlemini {1}ms de tamamladım.", user.Id, stopWatch.ElapsedMilliseconds);
        }
    }

    public void CreateUpdateUserObject(ref User user, UpdateUserDto request)
    {
        user.Name = request.Name;
        user.Age = request.Age;
        user.DateOfBirth = request.DateOfBirth;
    }
}
