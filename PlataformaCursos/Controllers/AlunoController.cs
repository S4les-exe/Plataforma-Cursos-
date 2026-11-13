using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlataformaCursos.Applications.Services;
using PlataformaCursos.DTOs.AlunosDto;
using PlataformaCursos.Exceptions;

namespace PlataformaCursos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {
        private readonly AlunoService _service;

        public AlunoController(AlunoService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<LerAlunoDTO>> Listar()
        {
            return Ok(_service.Listar());
        }

        [HttpGet("{id}")]   
        public ActionResult<LerAlunoDTO> ObterPorId(int id)
        {
            try
            {
                return Ok(_service.ObterPorid(id));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("email/{email}")]
        public ActionResult<LerAlunoDTO> ObterPorEmail(string email)
        {
            try
            {
                return Ok(_service.ObterPorEmail(email));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<LerAlunoDTO> Adicionar(CriarAlunoDTO alunoDTO)
        {
            try
            {
                LerAlunoDTO alunoCriado = _service.Adicionar(alunoDTO);
                return CreatedAtAction(nameof(ObterPorId), new { id = alunoCriado.IdAluno }, alunoCriado);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<LerAlunoDTO> Atualizar(int id, CriarAlunoDTO alunoDTO)
        {
            try
            {
                LerAlunoDTO usuarioAtualizado = _service.Atualizar(id, alunoDTO);
                return StatusCode(200, usuarioAtualizado);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Deletar(int id)
        {
            try
            {
                _service.Deletar(id);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
    