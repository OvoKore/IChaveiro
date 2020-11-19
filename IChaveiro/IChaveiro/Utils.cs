using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IChaveiro.Services;
using Realms;
using Refit;
using IChaveiro.DTO;
using IChaveiro.ModelRealm;

namespace IChaveiro
{
    public class Utils
    {
        public static List<string> SexList = new List<string>() { "Male", "Female", "Another" };

        public static bool EmailValidator(string Email)
        {
            string emailRegex = @"(^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$)";
            return Regex.IsMatch(Email, emailRegex);
        }
        public static bool PasswordValidator(string Senha)
        {
            string passwordRegex = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$";
            return Regex.IsMatch(Senha, passwordRegex);
        }

        public static bool CnpjValidator(string Cnpj)
        {
            if (string.IsNullOrEmpty(Cnpj))
                return false;

            Cnpj = Cnpj.Trim();
            Cnpj = Cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (Cnpj.Length != 14)
                return false;

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;

            tempCnpj = Cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj += digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito += resto.ToString();
            return Cnpj.EndsWith(digito);
        }

        public static bool CpfValidator(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return false;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf += digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito += resto.ToString();
            return cpf.EndsWith(digito);
        }

        public static string Crypt(string password)
        {
            var hexString = BitConverter.ToString(Encoding.Default.GetBytes(password)).Replace("-", "");
            char[] listChar = hexString.ToCharArray();
            Array.Reverse(listChar);
            return new string(listChar);
        }

        public static IRestApi GetApi(bool with_token = true, string type_token = "AccessToken")
        {
            if (with_token)
            {
                return RestService.For<IRestApi>(EndPoints.BaseUrl, GetRefitSettings(type_token));
            }
            else
            {
                return RestService.For<IRestApi>(EndPoints.BaseUrl);
            }
        }

        public static RefitSettings GetRefitSettings(string type_token = "AccessToken")
        {
            string token = string.Empty;
            if (type_token == "AccessToken")
            {
                token = Realm.GetInstance().All<TokenRealm>().First().AccessToken;
            }
            else if (type_token == "RefreshToken")
            {
                token = Realm.GetInstance().All<TokenRealm>().First().RefreshToken;
            }
            RefitSettings refitSettings = new RefitSettings()
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(token)
            };
            return refitSettings;
        }

        public static List<StateDTO> GetStates()
        {
            Assembly assembly = typeof(Utils).GetTypeInfo().Assembly;
            string stateJson = "State.json";
            Stream streamState = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.json.{stateJson}");
            StreamReader readerState = new StreamReader(streamState);
            string jsonStringState = readerState.ReadToEnd();
            List<StateDTO> JsonEstado = JsonConvert.DeserializeObject<List<StateDTO>>(jsonStringState);
            return JsonEstado;
        }

        public static List<CityDTO> GetCities()
        {
            Assembly assembly = typeof(Utils).GetTypeInfo().Assembly;
            string cityJson = "City.json";
            Stream streamCity = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.json.{cityJson}");
            StreamReader readerCity = new StreamReader(streamCity);
            string jsonStringCity = readerCity.ReadToEnd();
            List<CityDTO> JsonCidade = JsonConvert.DeserializeObject<List<CityDTO>>(jsonStringCity);
            return JsonCidade;
        }
    }
}