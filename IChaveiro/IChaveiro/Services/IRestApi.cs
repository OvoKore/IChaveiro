using System.Collections.Generic;
using System.Threading.Tasks;
using IChaveiro.DTO;
using IChaveiro.Models;
using Refit;

namespace IChaveiro.Services
{
    public interface IRestApi
    {
        //CEP
        [Get("/{cep}/json")]
        Task<CepDTO> Cep(string cep);

        //TOKEN
        [Post("/login-locksmith")]
        Task<TokenDTO> Login([Body(BodySerializationMethod.Serialized)] Locksmith locksmith);

        [Post("/refresh-token")]
        [Headers("Authorization: Bearer")]
        Task<TokenDTO> RefreshToken();

        //LOCKSMITH
        [Get("/get-user-locksmith")]
        [Headers("Authorization: Bearer")]
        Task<Locksmith> GetLockSmith();

        [Get("/get-status-locksmith")]
        [Headers("Authorization: Bearer")]
        Task<bool> GetStatusLockSmith();
        
        [Post("/change-status-locksmith")]
        [Headers("Authorization: Bearer")]
        Task<bool> ChangeStatusLockSmith();

        [Post("/update-user-locksmith")]
        [Headers("Authorization: Bearer")]
        Task<MsgDTO> UpdateLocksmith([Body(BodySerializationMethod.Serialized)] Locksmith locksmith);

        [Post("/change-user-locksmith-password")]
        [Headers("Authorization: Bearer")]
        Task<MsgDTO> ChangePassword([Body(BodySerializationMethod.Serialized)] ChangePasswordDTO changePasswordDTO);

        [Post("/create-user-locksmith")]
        Task<MsgDTO> Create([Body(BodySerializationMethod.Serialized)] Locksmith locksmith);

        //SERVICE
        [Get("/get-service-list")]
        [Headers("Authorization: Bearer")]
        Task<List<Service>> GetServiceList();

        [Post("/add-service")]
        [Headers("Authorization: Bearer")]
        Task<MsgDTO> AddService([Body(BodySerializationMethod.Serialized)] Service service);

        [Post("/update-service")]
        [Headers("Authorization: Bearer")]
        Task<MsgDTO> UpdateService([Body(BodySerializationMethod.Serialized)] Service service);

        [Post("/delete-service")]
        [Headers("Authorization: Bearer")]
        Task<MsgDTO> DeleteService([Body(BodySerializationMethod.Serialized)] Service service);

        //ADDRESS
        [Get("/get-locksmith-address-list")]
        [Headers("Authorization: Bearer")]
        Task<List<Address>> GetAddressList();

        [Post("/add-locksmith-address")]
        [Headers("Authorization: Bearer")]
        Task<MsgDTO> AddAddress([Body(BodySerializationMethod.Serialized)] Address address);

        [Post("/update-locksmith-address")]
        [Headers("Authorization: Bearer")]
        Task<MsgDTO> UpdateAddress([Body(BodySerializationMethod.Serialized)] Address address);

        [Post("/delete-locksmith-address")]
        [Headers("Authorization: Bearer")]
        Task<MsgDTO> DeleteAddress([Body(BodySerializationMethod.Serialized)] Address address);
    }
}