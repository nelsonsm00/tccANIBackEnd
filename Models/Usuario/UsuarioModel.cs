using ANI.Arquitetura;

namespace ANI.Models.Usuario
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string? Login { get; set; }
        public string? Senha { get; set; }
        public string? SenhaDecriptografada => string.IsNullOrEmpty(Senha) ? null : Utils.Base64Decode(Senha);
        public string? Email { get; set; }
    }

    public class LoginRequestModel
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public int Origem { get; set; }
    }

    public class LoginResponseModel
    {
        public int Id { get; set; }
        public int Conta { get; set; }
        public string Login { get; set; }
        public string Token { get; set; }
    }
}
