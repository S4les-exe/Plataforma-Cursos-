using PlataformaCursos.Applications.Autenticacao;
using PlataformaCursos.Domains;
using PlataformaCursos.DTOs.AutenticacaoDto;
using PlataformaCursos.Exceptions;
using PlataformaCursos.Interfaces;

namespace PlataformaCursos.Applications.Services
{
    public class AutenticacaoService
    {
        private readonly IAlunoRepository _repository;
        private readonly GeradorTokenJwt _tokenJwt;

        public AutenticacaoService(IAlunoRepository repository, GeradorTokenJwt tokenJwt)
        {
            _repository = repository;
            _tokenJwt = tokenJwt;
        }

        private static bool VerificarSenha(string senhaDigitada, byte[] senhaHashBanco)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var hashDigitado = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senhaDigitada));

            return hashDigitado.SequenceEqual(senhaHashBanco);
        }

        public TokenDTO Login(LoginDTO loginDto)
        {
            Aluno? aluno = _repository.ObterPorEmail(loginDto.Email);

            if (aluno == null)
            {
                throw new DomainException("Email ou senha inválidos.");
            }

            if (!VerificarSenha(loginDto.Senha, aluno.Senha))  //comparar a senha digitada com a senha armazenada
            {
                throw new DomainException("E-mail ou senha inválidos.");
            }

            var token = _tokenJwt.GerarToken(aluno); //Gerando o token JWT

            TokenDTO novoToken = new TokenDTO { Token = token };

            return novoToken;
        }
    }
}
