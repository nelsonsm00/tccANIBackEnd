using ANI.Arquitetura;
using ANI.Arquitetura.Erro;
using ANI.Models.Usuario;
using ANI.Models.Nutricionista;
using ANI.Modulos.Nutricionista;
using ANI.Segurança;

namespace ANI.Modulos.Usuario
{
    public class UsuarioModulo : ModuloBase
    {
        private const int ORIGEM_NUTRICIONISTA = 1;

        private NutricionistaModulo nutricionistaModulo;

        public UsuarioModulo() : this(null) { }

        public UsuarioModulo(DataBase? db) : base(db) 
        {
            this.nutricionistaModulo = new NutricionistaModulo(this.getDataBase());
        }

        public UsuarioModel Get(int pId)
        {
            return null;
        }

        public UsuarioModel Get(string pLogin, string pSenha)
        {
            pSenha = Utils.Base64Encode(pSenha);

            return this.ExecuteQuery<UsuarioModel>($@"  Select  ID Id,
                                                                LOGIN Login,
                                                                SENHA Senha,
                                                                EMAIL Email
                                                        From USUARIO
                                                        Where LOGIN = '{pLogin}' and SENHA = '{pSenha}'");
        }

        public LoginResponseModel Login(LoginRequestModel parametros)
        {
            UsuarioModel usuario = this.Get(parametros.Login, parametros.Senha);
            if (usuario == null)
                throw new ANIException("Login ou senha incorretos.", 404);

            LoginResponseModel response = new LoginResponseModel()
            {
                Id = usuario.Id,
                Login = usuario.Login
            };

            if (parametros.Origem == ORIGEM_NUTRICIONISTA)
            {
                NutricionistaModel nutricionistaModel = this.nutricionistaModulo.Get(response.Id);
                if (nutricionistaModel == null)
                    throw new ANIException("Usuário não é um nutricionista.", 412);
                response.Conta = nutricionistaModel.Conta;
            }

            response.Token = TokenService.GeraToken(usuario);
            return response;
        }

        public int Post(UsuarioModel pRegistro)
        {
            if (this.ExisteRegistro("USUARIO", $@"ID = {pRegistro.Id}"))
                return pRegistro.Id;
            //Não insere a pessoa. Responsabilidade de quem chama o método.
            if (pRegistro.Login == null && pRegistro.Email == null)
                throw new ANIException(Mensagens.e0131);

            pRegistro.Login = pRegistro.Login ?? pRegistro.Email;
            pRegistro.Senha = pRegistro.Senha ?? "YW5pMjAyMg==";  //ani2022

            this.ExecuteCommand<UsuarioModel>($@"  Insert into USUARIO
                                                        (ID, LOGIN, SENHA, EMAIL)
                                                    Values
                                                        (@Id, @Login, @Senha, @Email)", pRegistro);
            return pRegistro.Id;
        }

        public void PutEmail(int pId, string email)
        {
            this.ExecuteCommand<UsuarioModel>($@"Update USUARIO Set
                                                    EMAIL = @Email
                                                  Where ID = @Id", new UsuarioModel() { Id = pId, Email = email });
        }
    }
}
