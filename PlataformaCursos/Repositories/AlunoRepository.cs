using PlataformaCursos.Contexts;
using PlataformaCursos.Domains;
using PlataformaCursos.Interfaces;

namespace PlataformaCursos.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly PlataformaCursosDBContext _context;

        public AlunoRepository(PlataformaCursosDBContext context)
        {
            _context = context;
        }

        public List<Aluno> Listar()
        {
            return _context.Aluno.ToList();
        }

        public Aluno? ObterPorId(int id)
        {
            return _context.Aluno.Find(id);
        }

        public Aluno? ObterPorEmail(string email)
        {
            return _context.Aluno.FirstOrDefault(u => u.Email == email);
        }

        public bool EmailExiste(string email)
        {
            return _context.Aluno.Any(u => u.Email == email);
        }

        public void Adicionar(Aluno aluno)
        {
            _context.Aluno.Add(aluno);
            _context.SaveChanges();
        }

        public void Atualizar(Aluno aluno)
        {
            Aluno? alunoBanco = _context.Aluno.FirstOrDefault(alunoAux => alunoAux.IdAluno == aluno.IdAluno);

            if (aluno == null)
            {
                return;
            }

            alunoBanco.Nome = aluno.Nome;
            alunoBanco.Email = aluno.Email;
            alunoBanco.Senha = aluno.Senha;

            _context.SaveChanges();
        }

        public void Deletar(int id)
        {
            Aluno? alunoBanco = _context.Aluno.FirstOrDefault(alunoAux => alunoAux.IdAluno == id);

            if (alunoBanco == null)
            {
                return;
            }

            _context.Aluno.Remove(alunoBanco);
            _context.SaveChanges();
        }
    }
}