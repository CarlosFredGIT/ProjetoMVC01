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
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AccountLoginModel model, [FromServices] IUsuarioRepository usuarioRepository)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    //buscar o usuario no banco de dados atraves do email e da senha
                    var usuario = usuarioRepository.Obter(model.Email, model.Senha);

                    //verificar se o usuario foi encontrado
                    if(usuario != null)
                    {
                        //redirecionamento..
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        throw new Exception("Acesso negado, usuário inválido.");
                    }
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(AccountRegisterModel model, [FromServices] IUsuarioRepository usuarioRepository)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    //verificar se o email informado já esta cadastrado no banco de dados
                    if(usuarioRepository.Obter(model.Email) != null)
                    {
                        throw new Exception($"O email {model.Email} já está cadastrado no sistema, tente outro.");
                    }
                    else
                    {
                        var usuario = new Usuario();

                        usuario.IdUsuario = Guid.NewGuid();
                        usuario.Nome = model.Nome;
                        usuario.Email = model.Email;
                        usuario.Senha = model.Senha;
                        usuario.DataCadastro = DateTime.Now;

                        usuarioRepository.Inserir(usuario);

                        TempData["MensagemSucesso"] = $"Parabéns {usuario.Nome}, sua conta de usuário foi criada com sucesso.";
                        ModelState.Clear();
                    }
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }

            return View();
        }

        public IActionResult PasswordRecover()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PasswordRecover(AccountPasswordRecoverModel model)
        {
            return View();
        }
    }
}
