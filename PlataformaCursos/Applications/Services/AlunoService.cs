using PlataformaCursos.Domains;
using PlataformaCursos.DTOs.AlunosDto;
using PlataformaCursos.Exceptions;
using PlataformaCursos.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PlataformaCursos.Applications.Services
{
    public class AlunoService
    {
        private readonly IAlunoRepository _repository;

        public AlunoService(IAlunoRepository repository)
        {
            _repository = repository;
        }

        private static LerAlunoDTO LerDto(Aluno aluno)
        {
            LerAlunoDTO lerAluno = new LerAlunoDTO
            {
                IdAluno = aluno.IdAluno,
                Nome = aluno.Nome,
                Email = aluno.Email,
            };

            return lerAluno;
        }

        public List<LerAlunoDTO> Listar()
        {
            List<Aluno> alunos = _repository.Listar();

            List<LerAlunoDTO> alunosDto = alunos.Select(u => LerDto(u)).ToList();

            return alunosDto;
        }

        private static void ValidarEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                throw new DomainException("Email inválido.");
            }
        }

        private static byte[] HashSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha))
            {
                throw new DomainException("Senha não pode ser vazia.");
            }

            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
        }

        public LerAlunoDTO ObterPorid(int id)
        {
            Aluno aluno = _repository.ObterPorId(id);
            if (aluno == null)
            {
                throw new DomainException("Aluno não encontrado.");
            }
            return LerDto(aluno);
        }

        public LerAlunoDTO ObterPorEmail(string email)
        {
            ValidarEmail(email);
            Aluno aluno = _repository.ObterPorEmail(email);
            if (aluno == null)
            {
                throw new DomainException("Aluno não encontrado.");
            }
            return LerDto(aluno);
        }

        public static void ValidarNome(string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                throw new DomainException("Nome não pode ser vazio.");
            }
        }

        public LerAlunoDTO Adicionar(CriarAlunoDTO alunoDto)
        {
            ValidarEmail(alunoDto.Email);

            if (_repository.EmailExiste(alunoDto.Email))
            {
                throw new DomainException("Email já cadastrado.");
            }
            Aluno aluno = new Aluno
            {
                Nome = alunoDto.Nome,
                Email = alunoDto.Email,
                Senha = HashSenha(alunoDto.Senha)
            };
            _repository.Adicionar(aluno);

            return LerDto(aluno);
        }

        public LerAlunoDTO Atualizar(int id, CriarAlunoDTO alunoDTO)
        {
            Aluno alunoBanco = _repository.ObterPorId(id);

            if (alunoBanco == null)
            {
                throw new DomainException("Aluno não encontrado.");
            }

            ValidarEmail(alunoDTO.Email);
            ValidarNome(alunoDTO.Nome);

            Aluno alunoEmail = _repository.ObterPorEmail(alunoDTO.Email);

            if (alunoEmail != null && alunoEmail.IdAluno != id)
            {
                throw new DomainException("Email já cadastrado.");
            }

            alunoBanco.Nome = alunoDTO.Nome;
            alunoBanco.Email = alunoDTO.Email;
            alunoBanco.Senha = HashSenha(alunoDTO.Senha);

            _repository.Atualizar(alunoBanco);

            return LerDto(alunoBanco);
        }

        public void Deletar(int id)
        {
            Aluno aluno = _repository.ObterPorId(id);

            if (aluno == null)
            {
                throw new DomainException("Aluno não encontrado.");
            }

            _repository.Deletar(id);
        }

    }
}