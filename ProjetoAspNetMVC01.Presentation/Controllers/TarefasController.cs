using Microsoft.AspNetCore.Mvc;
using ProjetoAspNetMVC01.Presentation.Models;
using ProjetoAspNetMVC01.Repository.Entities;
using ProjetoAspNetMVC01.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAspNetMVC01.Presentation.Controllers
{
    public class TarefasController : Controller
    {
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(TarefasCadastroModel model,
            [FromServices] ITarefaRepository tarefaRepository)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var tarefa = new Tarefa();

                    tarefa.IdTarefa = Guid.NewGuid();
                    tarefa.Nome = model.Nome;
                    tarefa.Data = DateTime.Parse(model.Data);
                    tarefa.Hora = TimeSpan.Parse(model.Hora);
                    tarefa.Descricao = model.Descricao;
                    tarefa.Prioridade = model.Prioridade;

                    tarefaRepository.Inserir(tarefa);

                    TempData["MensagemSucesso"] = $"Tarefa {tarefa.Nome}, cadastrada com sucesso.";

                    ModelState.Clear();
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Erro: {e.Message}";
                }
            }

            return View();
        }

        public IActionResult Consulta()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Consulta(TarefasConsultaModel model,
            [FromServices] ITarefaRepository tarefaRepository)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Tarefas = tarefaRepository.ConsultarPorDatas(DateTime.Parse(model.DataMin), DateTime.Parse(model.DataMax));
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Erro: {e.Message}";
                }
            }

            return View(model);
        }

        public IActionResult Relatorio()
        {
            return View();
        }
    }
}
